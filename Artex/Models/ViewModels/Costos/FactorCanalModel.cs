using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.Util.Sistema;
using Artex.DB;

namespace Artex.Models.ViewModels.Costos
{
    public class FactorCanalModel
    {

        public int idLinea { get; set; }
        public int idProducto { get; set; }
        public int idFamilia { get; set; }

        [Display(Name = "Familia: ")]
        public int? idAddFamilia { get; set; }
        public IEnumerable<familia_producto> FamiliaList { get { return new List<familia_producto>(); } set { } }

        public string indicador { get; set; }



        [Display(Name = "POP: ")]
       // [Required(ErrorMessage = "El descuento para POP es requerido")]
        public string factorPOP { get; set; }

        [Display(Name = "Cadenas: ")]
        //[Required(ErrorMessage = "El descuento para Cadenas es requerido")]
        public string factorCadenas { get; set; }

        [Display(Name = "Franquicia: ")]
       // [Required(ErrorMessage = "El descuento para Franquicia es requerido")]
        public string factorFranquicia { get; set; }

        [Display(Name = "Proyectos: ")]
        //[Required(ErrorMessage = "El descuento para proyectos es requerido")]
        public string factorProyectos { get; set; }


        public PermisosModel permisos { get; set; }
    }
}