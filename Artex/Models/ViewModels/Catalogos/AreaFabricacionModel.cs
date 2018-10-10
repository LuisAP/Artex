using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Artex.Models.ViewModels.Catalogos
{
    public class AreaFabricacionModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre:")]
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción:")]
        public string Descripcion { get; set; }

        [Display(Name = "Activo:")]
        public bool Activo { get; set; }
    }
}