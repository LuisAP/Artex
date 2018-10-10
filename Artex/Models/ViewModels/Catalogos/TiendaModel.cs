using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.Util.Sistema;
using Artex.DB;

namespace Artex.Models.ViewModels.Catalogos
{
    public class TiendaModel{
        public int Id { get; set; }

        [Display(Name = "Nombre:")]
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Responsable")]
        public int Responsable { get; set; }

        [Display(Name = "Credito de Fabricacíon")]
        [Required(ErrorMessage = "El campo solo acepta valores numericos")]
        public int Credito_F { get; set; }

        [Display(Name = "Crédito de Fabricación Máximo")]
        public int Credito_FM { get; set; }

        [Display(Name = "Crédito de Comercialización")]
        public int Credito_C { get; set; }

        [Display(Name = "Crédito de Comercialización Máximo")]
        public int Credito_CM { get; set; }

        [Display(Name = "Calle")]
        public string Calle { get; set; }

        [Display(Name = "Número Exterior")]
        public string Num_Ext { get; set; }

        [Display(Name = "Número Interior")]
        public string Num_Int { get; set; }

        [Display(Name = "Colonia")]
        public string Colonia { get; set; }

        [Display(Name = "Ciudad")]
        public string Ciudad { get; set; }

        [Display(Name = "Municipio")]
        public string Municipio { get; set; }

        [Display(Name = "C.P.")]
        public string CP { get; set; }

        public IEnumerable<pais> paisList { get { return new List<pais>(); } set { } }
        [Display(Name = "País")]
        public int? Pais { get; set; }

        [Display(Name = "Estado")]
        public int? Estado { get; set; }
        public IEnumerable<estado> estadoList { get { return new List<estado>(); } set { } }

        [Display(Name = "Activo:")]
        public bool Activo { get; set; }

        public PermisosModel permisos { get; set; }

        //[Display(Name = "Unidad de negocio:")]
        //public int UnidadNegocio { get; set; }
        public IEnumerable<empleado> empleadoList { get { return new List<empleado>(); } set { } }

    }
}