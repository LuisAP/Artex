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
    
    public partial class orden_de_compra
    {
        public int ID { get; set; }
        public decimal SUB_TOTAL { get; set; }
        public decimal TOTAL { get; set; }
        public int FECHA { get; set; }
        public int ESTATUS { get; set; }
        public Nullable<int> ID_SUBPROGRAMA { get; set; }
        public Nullable<int> ID_PROGRAMACION_SEMANAL { get; set; }
        public string FORMA_PAGO { get; set; }
        public string TIEMPO_DE_ENTREGA { get; set; }
        public string PLAZO_PAGO { get; set; }
        public Nullable<int> ID_CUENTA_POR_PAGAR { get; set; }
    }
}
