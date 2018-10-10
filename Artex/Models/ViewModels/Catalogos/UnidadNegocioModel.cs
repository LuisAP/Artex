using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.DB;
using Artex.Models.DAL.DAO;
using Artex.Util.Sistema;

namespace Artex.Models.ViewModels.Catalogos
{
    public class UnidadNegocioModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre:")]
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción:")]
        public string Descripcion { get; set; }

        [Display(Name = "Activo:")]
        public bool Activo { get; set; }

        [Display(Name = "Empresa:")]
        [Required(ErrorMessage = "La empresa es requerida")]
        public int Empresa { get; set; }
        public IEnumerable<empresa> EmpresaList { get { return new List<empresa>(); } set { } }

        public PermisosModel permisos { get; set; }
    }
}