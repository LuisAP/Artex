using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.DB;

namespace Artex.Models.ViewModels.Catalogos
{
    public class AtributoProductoModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre:")]
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción:")]
        public string Descripcion { get; set; }

        [Display(Name = "Activo:")]
        public bool Activo { get; set; }

        [Display(Name = "Código:")]
        [Required(ErrorMessage = "El codigo de atributo es requerido")]
        [RegularExpression("[0-9]{3}", ErrorMessage = "el valor debe de ser numerico de tres digitos")]
        public string Codigo { get; set; }

        /*
        [Display(Name = "Precio por unidad de medida:")]
        [Required(ErrorMessage = "El precio es requerido")]
        public string Precio { get; set; }
        */

/*
        [Display(Name = "Tipo de atributo:")]
        public int TipoAtributo { get; set; }
        public IEnumerable<tipo_atributo> TipoAtributoList { get; set; }

        [Display(Name = "Grado de telas:")]
        public int grado { get; set; }
        public IEnumerable<atributo_grado> gradoList { get; set; }
        */
        /*
        [Display(Name = "Unidad de medida:")]
        [Required(ErrorMessage = "La unidad de medida es requerida")]
        public int UnidadMedida { get; set; }
        public IEnumerable<unidad_medida> UnidadMedidaList { get; set; }
        */
        /*
        [Display(Name = "Materia prima:")]
        [Required(ErrorMessage = "Debe seleccionar una meteria prima")]
        public int MateriaPrima { get; set; }
        public IEnumerable<insumo_mp> MateriaPrimaList { get; set; }


                [Display(Name = "Area de fabricacion:")]
                public int AreaFabricacion { get; set; }
                public IEnumerable<area_de_fabricacion> AreaFabricacionList { get { return new List<area_de_fabricacion>(); } set { } }


                [Display(Name = "Proceso de fabricacion:")]
                public int ProcesoFabricacion { get; set; }
                public IEnumerable<procesos_de_fabricacion> ProcesoFabricacionList { get { return new List<procesos_de_fabricacion>(); } set { } }
                */
    }
    public class AddSubAtributoModel
    {
        public int id { get; set; }
        public int IdPadre { get; set; }

        [Display(Name = "Nombre:")]
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción:")]
        public string Descripcion { get; set; }

        [Display(Name = "Activo:")]
        public bool Activo { get; set; }

        [Display(Name = "Código:")]
        [Required(ErrorMessage = "El codigo de atributo es requerido")]
        //[RegularExpression("[0-9]{3}", ErrorMessage = "el valor debe de ser numerico de tres digitos")]
        public string Codigo { get; set; }
    }
}