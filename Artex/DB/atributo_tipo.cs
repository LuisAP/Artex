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
    
    public partial class atributo_tipo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public atributo_tipo()
        {
            this.configuracion_producto_piso = new HashSet<configuracion_producto_piso>();
            this.configuracion_producto = new HashSet<configuracion_producto>();
            this.insumo = new HashSet<insumo>();
            this.materia_prima = new HashSet<materia_prima>();
        }
    
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
        public int ID_ATRIBUTO { get; set; }
        public bool ACTIVO { get; set; }
        public string CODIGO { get; set; }
    
        public virtual atributo atributo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<configuracion_producto_piso> configuracion_producto_piso { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<configuracion_producto> configuracion_producto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<insumo> insumo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<materia_prima> materia_prima { get; set; }
    }
}
