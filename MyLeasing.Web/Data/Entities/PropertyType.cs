using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Data.Entities
{
    public class PropertyType
    {
        public int Id { get; set; }

        [Display(Name = "Property Type")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Name { get; set; }

        //Otra opcion relaciones 1 a varios, colocar las relaciones en las dos partes para mejorar el uso de busqueda de datos.
        public ICollection<Property> Properties { get; set; }
    }

}
