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
    
    public partial class puntuacion
    {
        public int ID { get; set; }
        public int ID_TIENDA { get; set; }
        public Nullable<int> PUNTUACION_DISPONIBLE_FABRICA { get; set; }
        public Nullable<int> PUNTUACION_DISPONIBLE_COMERCIALIZADORA { get; set; }
        public int PUNTUACION_MAX_PLANTA { get; set; }
    }
}