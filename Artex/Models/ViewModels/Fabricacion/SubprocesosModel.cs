using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.Util.Sistema;
using Artex.DB;


namespace Artex.Models.ViewModels.Fabricacion
{
    public class SubprocesosModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre:")]
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción: ")]
        public string Descripcion { get; set; }

        [Display(Name = "Cotización: ")]
        public int Proceso { get; set; }
        public IEnumerable<procesos_de_fabricacion> Procesolist { get { return new List<procesos_de_fabricacion>(); } set { } }

        [Display(Name = "Clave: ")]
        public string Clave { get; set; }

        [Display(Name = "Activo:")]
        public bool Activo { get; set; }

        public PermisosModel permisos { get; set; }
    }
}