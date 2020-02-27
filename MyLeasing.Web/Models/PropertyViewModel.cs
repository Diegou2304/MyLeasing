using Microsoft.AspNetCore.Mvc.Rendering;
using MyLeasing.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLeasing.Web.Models
{
    public class PropertyViewModel: Property
    {
        //Adicionarlo porque property esta relacionado con todo el objeto
        public int OwnerId { get; set; }

        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        [Display(Name ="Property Type")]
        [Range(1, int.MaxValue,ErrorMessage ="Debes seleccionar un tipo de propiedad")]
        public int PropertyTypeId { get; set; }

        public IEnumerable<SelectListItem> PropertyTypes { get; set; }
    }
}
