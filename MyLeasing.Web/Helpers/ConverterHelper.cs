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
        

        PropertyViewModel IConverterHelper.ToProperty(PropertyViewModel model, bool isNew)
        {
            return new PropertyViewModel
            {
                Address = model.Address,
                Contracts = isNew ? new List<Contract>() : model.Contracts,


            };
        }
    }
}
