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
    
    public partial class archivo
    {
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public string TIPO { get; set; }
        public string UBICACION { get; set; }
        public Nullable<System.DateTime> FECHA_CREACION { get; set; }
        public Nullable<System.DateTime> FECHA_ELIMINACION { get; set; }
        public Nullable<int> ID_GUARDIDO { get; set; }
        public Nullable<int> ID_ELIMINO { get; set; }
        public int ID_ARCHIVOS { get; set; }
    }
}