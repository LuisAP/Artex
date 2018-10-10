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
    
    public partial class proveedor
    {
        public int ID { get; set; }
        public string TIPO_PERSONA { get; set; }
        public string NOMBRE { get; set; }
        public string APELLIDO_PATERNO { get; set; }
        public string APELLIDO_MATERNO { get; set; }
        public string EMPRESA { get; set; }
        public string RAZON_SOCIAL { get; set; }
        public Nullable<int> ID_CONTACTO { get; set; }
        public Nullable<int> ID_CUENTA { get; set; }
        public bool ES_DE_CREDITO { get; set; }
        public Nullable<decimal> MONTO_CREDITO { get; set; }
        public Nullable<decimal> SALDO { get; set; }
        public string TIEMPO_ENTREGA { get; set; }
        public string TIEMPO_CREDITO { get; set; }
        public string TELEFONO { get; set; }
        public string CELULAR { get; set; }
        public string RFC { get; set; }
        public string EMAIL { get; set; }
        public Nullable<int> ID_DIRECCION { get; set; }
        public string GIRO { get; set; }
        public bool ACTIVO { get; set; }
        public string CALIFICACION { get; set; }
    
        public virtual cuenta cuenta { get; set; }
        public virtual direccion direccion { get; set; }
        public virtual persona_contacto persona_contacto { get; set; }
    }
}