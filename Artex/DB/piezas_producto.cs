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
    
    public partial class piezas_producto
    {
        public int ID_PRODUCTO { get; set; }
        public int ID_PIEZA { get; set; }
        public bool ES_CONFIGURABLE { get; set; }
    
        public virtual piezas_configurables piezas_configurables { get; set; }
        public virtual producto producto { get; set; }
    }
}