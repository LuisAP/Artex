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
    
    public partial class stok_insumo
    {
        public int ID { get; set; }
        public Nullable<float> CANTIDAD_EXISTENTE { get; set; }
        public Nullable<float> CANTIDAD_MINIMA { get; set; }
        public Nullable<float> CANTIDAD_MAXIMA { get; set; }
        public Nullable<int> ID_ALMACEN { get; set; }
        public Nullable<float> PUNTO_REORDEN { get; set; }
    }
}