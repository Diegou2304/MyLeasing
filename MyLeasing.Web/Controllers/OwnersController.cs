using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLeasing.Web.Data;
using MyLeasing.Web.Data.Entities;
using MyLeasing.Web.Helpers;
using MyLeasing.Web.Models;

namespace MyLeasing.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class OwnersController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;

        public OwnersController(DataContext dataContext,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IImageHelper imageHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
        }

        // GET: Owners
        public IActionResult Index()
        {

            //Owners y sus users incluidos usuarios. Con las propiedades y sus contratos
            return View(_dataContext.Owners
                .Include(o => o.User)
                .Include(o => o.Properties)
                .Include(o => o.Contracts));

        }

        // GET: Owners/Details/5
        //Le mandamos el id de los details, el signo indica que puede llegar nulo
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Buscame el primero donee el id sea el mismo al que me pasaron
            var owner = await _dataContext.Owners
                .Include(o => o.User)
                .Include(o => o.Properties)
                .ThenInclude(p => p.PropertyImages)
                .Include(o => o.Contracts)
                .ThenInclude(c => c.Lessee)
                .ThenInclude(l => l.User)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // GET: Owners/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            //Verificamos si el modelo es valido
            if (ModelState.IsValid)
            {
                // Creamos el usuario a partir del modelo de la view
                var user = await CreateUserAsync(model);
                if (user != null)
                {
                    var owner = new Owner
                    {
                        Contracts = new List<Contract>(),
                        Properties = new List<Property>(),
                        User = user,

                    };

                    _dataContext.Owners.Add(owner);
                    await _dataContext.SaveChangesAsync();
                    return RedirectToAction("Index");

                }
                ModelState.AddModelError(string.Empty, "User already exists");
            }
            return View(model);
        }

        private async Task<User> CreateUserAsync(AddUserViewModel model)
        {
            //Llenamos los datos para completar el user
            var user = new User()
            {
                Address = model.Address,
                Document = model.Document,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username,
                Email = model.Username,



            };
            //Añadimos el usuario
            var result = await _userHelper.AddUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                user = await _userHelper.GetUserByEmailAsync(model.Username);
                await _userHelper.AddUserToRoleAsync(user, "Owner");
                return user;
            }
            return null;
        }

        // GET: Owners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _dataContext.Owners.FindAsync(id);
            if (owner == null)
            {
                return NotFound();
            }
            return View(owner);
        }

        // POST: Owners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Owner owner)
        {
            if (id != owner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dataContext.Update(owner);
                    await _dataContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnerExists(owner.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(owner);
        }

        // GET: Owners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _dataContext.Owners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var owner = await _dataContext.Owners.FindAsync(id);
            _dataContext.Owners.Remove(owner);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OwnerExists(int id)
        {
            return _dataContext.Owners.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AddProperty(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }

            //Busca por la clave primaria, con limitantes
            var owner = await _dataContext.Owners.FindAsync(id);
            if (owner == null)
            {
                return NotFound();

            }
            var model = new PropertyViewModel
            {

                OwnerId = owner.Id,
                PropertyTypes = _combosHelper.GetComboPropertyTypes()
            };

            return View(model);







        }

        [HttpPost]
        public async Task<IActionResult> AddProperty(PropertyViewModel model)
        {

            if (ModelState.IsValid)
            {
                //Mandamos true or false dependiendo si es nueva o no
                var property = await _converterHelper.ToPropertyAsync(model, true);

                _dataContext.Properties.Add(property);
                await _dataContext.SaveChangesAsync();

                //Dtalles del propietario
                return RedirectToAction($"Details/{model.OwnerId}");

            }

            return View(model);
        }
        public async Task<IActionResult> EditProperty(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }

            //Busca por la clave primaria, con limitantes
            //FindAync para datos sin relaciones
            var property = await _dataContext.Properties
                .Include(p => p.Owner)
                .Include(pt => pt.PropertyType)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
            {
                return NotFound();

            }

            //Convertirlo a un property View Model, porque nos busca la propiedad, pero tenemos que mostrar con el modelo view model adaptado a su vista.

            var model = _converterHelper.ToPropertyViewModel(property);
            return View(model);


        }
        [HttpPost]
        public async Task<IActionResult> EditProperty(PropertyViewModel model)
        {

            if (ModelState.IsValid)
            {
                //Mandamos true or false dependiendo si es nueva o no, falso porque esta editando
                var property = await _converterHelper.ToPropertyAsync(model, false);


                _dataContext.Properties.Update(property);
                await _dataContext.SaveChangesAsync();

                //Dtalles del propietario
                return RedirectToAction($"Details/{model.OwnerId}");

            }

            return View(model);
        }

        public async Task<IActionResult> DetailsProperty(int? id)
        {

            if (id == null)
            {
                return NotFound();

            }

            var property = await _dataContext.Properties
                .Include(o => o.Owner)
                .ThenInclude(o => o.User)
                .Include(c => c.Contracts)
                .ThenInclude(l => l.Lessee)
                .ThenInclude(u => u.User)
                .Include(pt => pt.PropertyType)
                .Include(pi => pi.PropertyImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
            {
                return NotFound();
            }
            return View(property);




        }
        //Le mandamos el id
        //Verifico el estado del modelo
        //Busca la propiedad asignada a ese id
        //Si la encuentra tenemos que asignar al property los campos del proerty view model
        //Esto recupera los datos correspondientes de la imagen
        public async Task<IActionResult> AddImage(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var property = await _dataContext.Properties.FindAsync(id);

            if (property == null)
            {
                return NotFound();
            }


            //Hacemos esto para la indicar y guardar en el model el id de la propiedad
            var model = new PropertyImageViewModel
            {
                Id = property.Id,




            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> AddImage(PropertyImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile);

                }
                var propertyImage = new PropertyImage
                {
                    ImageUrl = path,
                    Property = await _dataContext.Properties.FindAsync(model.Id)
                };

                _dataContext.PropertyImages.Add(propertyImage);

                await _dataContext.SaveChangesAsync();

                return RedirectToAction($"{nameof(DetailsProperty)}/{ model.Id}");
            }
            return NotFound();
        }
    }













}

