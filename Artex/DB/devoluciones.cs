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
    
    public partial class devoluciones
    {
        public int ID { get; set; }
        public string MOTIVO_DEVOLUCION { get; set; }
        public Nullable<decimal> MONTO_DEVOLUCION { get; set; }
        public string OBSERVACIONES { get; set; }
        public Nullable<int> ID_PRODUCTO_COMPRADO { get; set; }
        public Nullable<int> ID_PRODUCTO_VENDIDO { get; set; }
        public Nullable<int> ID_INGRESO { get; set; }
        public Nullable<int> ID_EGRESO { get; set; }
        public Nullable<int> ID_ARCHIVOS { get; set; }
        public string TIPO { get; set; }
    }
}
