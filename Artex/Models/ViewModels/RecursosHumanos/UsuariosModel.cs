using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.DB;
using Artex.Util;

namespace Artex.Models.ViewModels.RecursosHumanos
{
    public class UsuariosModel
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Nombre de usuario")]
       public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
       // [MaxLength(50, ErrorMessage = "Name cannot be greater than 50")]
        [StringLength(8, ErrorMessage = "El número de caracteres de {0} debe contener entre {2} y {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        public Boolean Activo { get; set; }

        [Display(Name = "Rol")]
        [Required]
        public int rol { get; set; }
        public IEnumerable<rol> rolList { get; set; }

        [Display(Name = "personal")]
        public int? personal { get; set; }
        public IEnumerable<empleados_v> personalList { get; set; }


        //datos de persona si no es empleado
        [Display(Name = "Es Personal de la empresa")]
        public Boolean esEmpleado { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string apellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        public string apellidoMaterno{ get; set; }
    }


    //Editar usuario
    public class UpdateUsuariosModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        public Boolean Activo { get; set; }

        [Display(Name = "Rol")]
        [Required]
        public int rol { get; set; }
        public IEnumerable<rol> rolList { get; set; }

        [Display(Name = "personal")]
        public int? personal { get; set; }
        public IEnumerable<empleados_v> personalList { get; set; }


        //datos de persona si no es empleado
        [Display(Name = "Es Personal de la empresa")]
        public Boolean esEmpleado { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string apellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        public string apellidoMaterno { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }
    public class CheckCodeViewModel
    {

        [Display(Name = "Ingrese el codigo de verificacion")]
        public string Codigo { get; set; }

        public string id { get; set; }
    }
    public class ResetPassViewModel
    {

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        public string Codigo { get; set; }
        public string id { get; set; }

    }

    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

    }
}
