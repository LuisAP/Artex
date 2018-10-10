using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Artex.Models.ViewModels.Email
{
    public class ContactoViewModel
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Mensaje { get; set; }
    }
    public class EmailModel
    {
        [Required]
        [EmailAddress]
        public string From { get; set; }

        [Required]
        [EmailAddress]
        public string To { get; set; }

        [Required]
        public string Message { get; set; }

        public string Subject { get; set; }

        [Required]
        public string Password { get; set; }

        public string Name { get; set; }
    }
}