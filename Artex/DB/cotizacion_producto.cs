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
    
    public partial class cotizacion_producto
    {
        public int ID { get; set; }
        public int ID_COTIZACION { get; set; }
        public int ID_PRODUCTO { get; set; }
        public string CODIGO { get; set; }
        public string ORIGEN_DEL_PRODUCTO { get; set; }
        public int ID_ORIGEN_DEL_PRODUCTO { get; set; }
        public decimal PRECIO { get; set; }
        public float DESCUENTO_UNITARIO { get; set; }
        public decimal IMPORTE { get; set; }
        public int CANTIDAD { get; set; }
    
        public virtual cotizacion cotizacion { get; set; }
        public virtual producto producto { get; set; }
    }
}
