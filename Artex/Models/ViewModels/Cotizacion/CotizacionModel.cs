using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.Util.Sistema;
using Artex.DB;

namespace Artex.Models.ViewModels.Cotizacion
{
    public class CotizacionModel {

        public int Id { get; set; }

        public string Codigo { get; set; }

        [Display(Name = "Vendedor:")]       
        public string Nombre { get; set; }

        [Display(Name = "Tienda:")]
        public string Tienda { get; set; }

        [Display(Name = "Calle:")]
        public string Calle { get; set; }

        [Display(Name = "Ciudad:")]
        public string Ciudad { get; set; }

        [Display(Name = "Numero Exterior:")]
        public string Num_ext { get; set; }

        [Display(Name = "Código Postal:")]
        public string CP { get; set; }

        [Display(Name = "Fecha:")]
        public string Fecha { get; set; }

        [Display(Name = "Fecha:")]
        public DateTime FechaF { get; set; }

        [Display(Name = "Apellido Paterno:")]
        public string Apellido_p { get; set; }

        [Display(Name = "Apellido Materno:")]
        public string Apellido_m { get; set; }

        [Display(Name = "Cantidad:")]
        public int Cantidad { get; set; }

        public IEnumerable<pais> paisList { get; set; }
        [Display(Name = "País")]
        public int? pais { get; set; }

        [Display(Name = "Cliente")]
        public int? Cliente { get; set; }
        public IEnumerable<cliente> Clientelist { get { return new List<cliente>(); } set { } }

        [Display(Name = "Vendedor")]
        public int? Vendedor { get; set; }
        public IEnumerable<empleado> Vendedorlist { get { return new List<empleado>(); } set { } }

        [Display(Name = "Cotización")]
        public int? Cotizacion { get; set; }
        public IEnumerable<cotizacion> Cotizacionlist { get { return new List<cotizacion>(); } set { } }

        [Display(Name = "Tipo de cliente")]
        public Boolean esClienteMayoreo { get; set; }

        [Display(Name = "Tipo de cliente")]
        public Boolean esClienteRetail { get; set; }

        public IEnumerable<producto> programaList { get; set; }
        [Display(Name = "Programa:")]
        public int? Programa { get; set; }

        public IEnumerable<producto> productList { get; set; }
        [Display(Name = "Productos:")]
        public int? Prductos { get; set; }

        //
        [Display(Name = "Cliente:")]
        public string Nombrec { get; set; }

        [Display(Name = "Apellido Paterno:")]
        public string Apellido_pc { get; set; }

        [Display(Name = "Apellido Materno:")]
        public string Apellido_mc { get; set; }

        [Display(Name = "Correo:")]
        public string Correo { get; set; }

        [Display(Name = "Celular:")]
        public string Celular { get; set; }

        public PermisosModel permisos { get; set; }
    }
}