using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.Util.Sistema;
using Artex.DB;

namespace Artex.Models.ViewModels.Catalogos
{
    public class DescuentosModel
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de Descuento: ")]
        public int? Tipo { get; set; }

        [Display(Name = "Producto: ")]
        public int? Producto { get; set; }
        public IEnumerable<producto> Vendedorlist { get { return new List<producto>(); } set { } }

        [Display(Name = "Familia: ")]
        public int? Familia { get; set; }
        public IEnumerable<familia_producto>Familialist { get { return new List<familia_producto>(); } set { } }

        [Display(Name = "Linea de Negocio: ")]
        public int? Linea { get; set; }
        public IEnumerable<linea_negocio> Linealist { get { return new List<linea_negocio>(); } set { } }

        [Display(Name = "Descuento: ")]
        public int Descuento { get; set; }

        [Display(Name = "Activo:")]
        public bool Activo { get; set; }

        public PermisosModel permisos { get; set; }
    }
}