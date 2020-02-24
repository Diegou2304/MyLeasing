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

        

        //Todo lo borramos porque ya tenemos los datos en el modelo de usuario extendido.
        public User User { get; set; }
        //Owner Property
        public ICollection<Property> Properties { get; set; } 

        public ICollection<Contract> Contracts { get; set; }

    }
}
