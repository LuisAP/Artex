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
    
    public partial class pagos_realizados
    {
        public int ID { get; set; }
        public int ID_CUENTA_POR_PAGAR { get; set; }
        public int ID_CUENTA_ORIGEN { get; set; }
        public int ID_CUENTA_DESTINO { get; set; }
        public System.DateTime FECHA { get; set; }
        public decimal MONTO { get; set; }
        public decimal SALDO_POR_PAGAR { get; set; }
        public decimal SALDO_CUENTA { get; set; }
    }
}
