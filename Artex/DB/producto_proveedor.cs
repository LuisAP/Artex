//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Artex.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class producto_proveedor
    {
        public int ID { get; set; }
        public Nullable<decimal> PRECIO { get; set; }
        public Nullable<float> DESCUENTO { get; set; }
        public string ID_PROVEEDOR { get; set; }
        public string ID_PRODUCTO { get; set; }
        public string ID_INSUMO { get; set; }
    }
}
