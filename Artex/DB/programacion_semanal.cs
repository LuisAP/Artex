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
    
    public partial class programacion_semanal
    {
        public int ID { get; set; }
        public int NUMERO_SEMANA { get; set; }
        public int PUNTUACION { get; set; }
        public Nullable<int> PUNTUACION_DISPONIBLE_PLANTA { get; set; }
        public Nullable<int> PUNTUACION_DISPONIBLE_FABRICA { get; set; }
        public Nullable<int> PUNTUACION_DISPONIBLE_COMERCIALIZADORA { get; set; }
        public string DESCRIPCION { get; set; }
        public Nullable<System.DateTime> FECHA_INICIO { get; set; }
        public Nullable<System.DateTime> FECHA_TERMINO { get; set; }
        public Nullable<decimal> COSTO_FABRICACION_ESTIMADO { get; set; }
        public Nullable<decimal> COSTO_FABRICACION_REAL { get; set; }
        public int ANIO { get; set; }
    }
}
