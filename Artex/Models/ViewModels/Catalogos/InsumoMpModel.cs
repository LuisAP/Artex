using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.DB;

namespace Artex.Models.ViewModels.Catalogos
{
    public class InsumoMpModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre:")]
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción:")]
        public string Descripcion { get; set; }

        [Display(Name = "Código:")]
        public string Codigo { get; set; }


        [Display(Name = "Precio de compra:")]
        [Required(ErrorMessage = "El precio de compra es requerido")]
        public string PrecioCompra { get; set; }

        [Display(Name = "Precio de entrega:")]
      //  [Required(ErrorMessage = "El precio de entrega es requerido")]
        public string PrecioEntrega { get; set; }


        [Display(Name = "Tipo:")]
        public String Tipo { get; set; }
       // public IEnumerable<String> TipoList { get; set; }

        [Display(Name = "Presentación de compra:")]
        [Required(ErrorMessage = "Debe seleccionar una presentacion de compra")]
        public int PresentacionCompra { get; set; }
        public IEnumerable<unidad_medida> PresentacionCompraList { get; set; }

        [Display(Name = "Presentación de entrega:")]
        [Required(ErrorMessage = "Debe seleccionar una presentacion de compra")]
        public int PresentacionEntrega { get; set; }
        public IEnumerable<unidad_medida> PresentacionEntregaList { get; set; }

     
     
        [Display(Name = "Tipo de compra:")]
        [Required(ErrorMessage = "Debe seleccionar una opcion")]
        public String TipoCompra { get; set; }

        [Display(Name = "Tipo de Explosión:")]
        [Required(ErrorMessage = "Debe seleccionar una opcion")]
        public String TipoExplosion { get; set; }

        [Display(Name = "Tipo de atributo:")]
        public int? tipoAtributo { get; set; }
        public IEnumerable<atributo> AtributoList { get; set; }

        [Display(Name = "Atributo:")]
        public int? Atributo { get; set; }
        public IEnumerable<atributo_tipo> tipoAtributoList { get; set; }

        [Display(Name = "Activo:")]
        public bool Activo { get; set; }

       
    }
}