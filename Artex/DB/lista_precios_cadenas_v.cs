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
    
    public partial class lista_precios_cadenas_v
    {
        public int ID_PRODUCTO { get; set; }
        public string PRODUCTO { get; set; }
        public string CODIGO { get; set; }
        public Nullable<double> COSTO { get; set; }
        public Nullable<double> FACTOR_CADENAS { get; set; }
        public Nullable<double> PRECIO_SUGERIDO { get; set; }
        public Nullable<decimal> PRECIO_CADENAS { get; set; }
        public string INDICADOR { get; set; }
    }
}
