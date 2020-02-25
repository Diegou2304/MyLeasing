using Microsoft.AspNetCore.Mvc;
using MyLeasing.Web.Helpers;
using MyLeasing.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLeasing.Web.Controllers
{
    public class AccountController: Controller
    {
        private readonly IUserHelper _userHelper;


        //Ya nos podemos loguear.
        public AccountController(IUserHelper userHelper)
        {
            _userHelper = userHelper;

        }

        [HttpGet]
        public IActionResult Login()
        {
            

            return View();
        }


        [HTTPPost]
        public async Task <IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);

                if (result.Succeeded)
                {
                    //Nos vamos al index del controlador home
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "User or Password Incorrect");

            }


            return View(model);

        }
        public async Task<IActionResult> Logout()
        {


            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }







    }
}
