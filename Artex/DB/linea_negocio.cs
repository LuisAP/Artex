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
    
    public partial class linea_negocio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public linea_negocio()
        {
            this.descuentos = new HashSet<descuentos>();
            this.factor_canal_familia = new HashSet<factor_canal_familia>();
            this.factor_fabrica_familia = new HashSet<factor_fabrica_familia>();
        }
    
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
        public Nullable<int> ID_UNIDAD_NEGOCIO { get; set; }
        public bool ACTIVO { get; set; }
        public string LETRA_CODIGO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<descuentos> descuentos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<factor_canal_familia> factor_canal_familia { get; set; }
        public virtual factor_canal_linea factor_canal_linea { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<factor_fabrica_familia> factor_fabrica_familia { get; set; }
        public virtual factor_fabrica_linea factor_fabrica_linea { get; set; }
        public virtual unidad_de_negocio unidad_de_negocio { get; set; }
    }
}