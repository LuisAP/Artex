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
    
    public partial class servicio_garantia
    {
        public int ID { get; set; }
        public Nullable<bool> ES_COBRABLE { get; set; }
        public Nullable<int> AREA_PRODUCCION { get; set; }
        public Nullable<int> PUNTUACION { get; set; }
        public Nullable<System.DateTime> FECHA_INGRESO { get; set; }
        public Nullable<System.DateTime> FECHA_PROMESA { get; set; }
        public Nullable<System.DateTime> FECHA_ENTREGA { get; set; }
        public int ID_PRODUCTO_VENDIDO { get; set; }
        public Nullable<int> ID_PEDIDO_GENERADO { get; set; }
        public int ID_ESTATUS { get; set; }
    }
}