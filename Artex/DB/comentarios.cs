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
    
    public partial class comentarios
    {
        public int ID { get; set; }
        public string COMENTARIO { get; set; }
        public Nullable<int> ID_TIPO_PROGRAMA { get; set; }
        public Nullable<int> ID_PROMOCION { get; set; }
    
        public virtual tipo_programa tipo_programa { get; set; }
    }
}
