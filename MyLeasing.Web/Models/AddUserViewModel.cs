using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLeasing.Web.Models
{
    public class AddUserViewModel
    {
        [Display(Name ="Email")]
        [Required (ErrorMessage ="Este campo es requerido")]
        [MaxLength(100,ErrorMessage ="no puede tener mas de 100 caracteres")]
        [EmailAddress]
        public string Username { get; set; }

        [Display(Name ="Document")]
        [Required(ErrorMessage ="El campo es obligatorio")]
        [MaxLength(50, ErrorMessage = "no puede tener mas de 50 caracteres")]
        public string Document { get; set; }

        [Display(Name ="First Name")]
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Display(Name ="Address")]
        [MaxLength(100,ErrorMessage ="El campo no puede tener mas de 100 caracteres")]
        public string Address { get; set; }

        [Display(Name ="Phone Number")]
        [MaxLength(50,ErrorMessage ="El campo no puede tener mas de 50 caracteres")]
        public string PhoneNumber { get; set; }

        [Display(Name ="Password")]
        [Required]
        [DataType(DataType.Password)]
        [StringLength(20,MinimumLength =6, ErrorMessage ="Tiene que contener entre {2} y {1}")]
        public string Password { get; set; }

        [Display(Name = "Password Confirm")]
        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Tiene que contener entre {2} y {1}")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
    }
}
