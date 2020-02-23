using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Data.Entities
{
    public class Owner
    {
       
        //Primero tenemos que tener propiedades
        //Data Notacion controla como queremos los datos en la base de datos
        public int Id { get; set; }

        [Required(ErrorMessage ="El campo es obligatorio")]
        [MaxLength(30, ErrorMessage ="Su nombre es muy largo, solo se permiten 30 caracteres")]
        public string Document { get; set; }


        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [MaxLength(20)]
        [Display(Name = "Fixed Phone")]
        public string FixedPhone { get; set; }

        [MaxLength(20)]
        [Display(Name = "CellPhone")]
        public string CellPhone { get; set; }


        [MaxLength(20)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        //Campos de lectura no se mapea en base de datos, esto es un delegado
        [Display (Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        public string FullNameWithDocument => $"{FirstName}{LastName} - {Document}";
        
        //Owner Property
        public ICollection<Property> Properties { get; set; } 

        public ICollection<Contract> Contracts { get; set; }

    }
}
