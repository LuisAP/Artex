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
    
    public partial class descuentos
    {
        public int ID { get; set; }
        public Nullable<int> PRODUCT_ID { get; set; }
        public Nullable<int> FAMILIA_ID { get; set; }
        public Nullable<int> LINEA_DE_NEGOCIO_ID { get; set; }
    
        public virtual familia_producto familia_producto { get; set; }
        public virtual linea_negocio linea_negocio { get; set; }
        public virtual producto producto { get; set; }
    }
}
