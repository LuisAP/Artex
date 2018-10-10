using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.DB;
using Artex.Util.Sistema;

namespace Artex.Models.ViewModels.Catalogos
{
    public class LineaNegocioModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre:")]
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción:")]
        public string Descripcion { get; set; }

        [Display(Name = "Activo:")]
        public bool Activo { get; set; }

        [Display(Name = "Unidad de negocio:")]
        public int UnidadNegocio { get; set; }
        public IEnumerable<unidad_de_negocio> UnidadNegocioList { get { return new List<unidad_de_negocio>(); } set { } }

        public PermisosModel permisos { get; set; }
    }
}