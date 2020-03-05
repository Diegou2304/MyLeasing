using MyLeasing.Web.Data;
using MyLeasing.Web.Data.Entities;
using MyLeasing.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLeasing.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _dataContext;
        private readonly ICombosHelper _combosHelper;

        //Inyectamos porque necesitamos usar la db
        public ConverterHelper(
            DataContext dataContext,
            ICombosHelper combosHelper)
        {
            _dataContext = dataContext;
            _combosHelper = combosHelper;
        }


        public async Task<Property> ToPropertyAsync(PropertyViewModel model, bool isNew)
        {
            return new Property
            {
                Address = model.Address,
                Contracts = isNew ? new List<Contract>() : model.Contracts,
                HasParkingLot = model.HasParkingLot,
                //Create diferente de 0, si no es nueva le ponemos el del modelo
                Id = isNew ? 0 : model.Id,
                IsAvailable = model.IsAvailable,
                Neighborhood = model.Neighborhood,
                //Aqui tenemos problemas con el owner porque el property model no trae el owner.
                Owner = await _dataContext.Owners.FindAsync(model.OwnerId),
                Price = model.Price,
                PropertyImages = isNew ? new List<PropertyImage>() : model.PropertyImages,
                PropertyType = await _dataContext.PropertyTypes.FindAsync(model.PropertyTypeId),
                Remarks = model.Remarks,
                Rooms = model.Rooms,
                SquareMeters = model.SquareMeters,
                Stratum = model.Stratum,





            };
        }

        //Aqui convertiremos
        public PropertyViewModel ToPropertyViewModel(Property property)
        {
            //Tenemos que mandarle el owner y el property type
            return new PropertyViewModel
            {
                Address = property.Address,
                Contracts = property.Contracts,
                HasParkingLot = property.HasParkingLot,
                //Create diferente de 0, si no es nueva le ponemos el del modelo
                Id = property.Id,
                IsAvailable = property.IsAvailable,
                Neighborhood = property.Neighborhood,
                //Aqui tenemos problemas con el owner porque el property model no trae el owner.
                //Aqui le pasamos la entidad
                Owner = property.Owner,
                Price = property.Price,
                PropertyImages = property.PropertyImages,
                PropertyType = property.PropertyType,
                Remarks = property.Remarks,
                Rooms = property.Rooms,
                SquareMeters = property.SquareMeters,
                Stratum = property.Stratum,

                //Parte propia del view model

                OwnerId = property.Owner.Id,
                PropertyTypeId = property.PropertyType.Id,
                PropertyTypes =  _combosHelper.GetComboPropertyTypes(),




            };
        }
    }
}
    

