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
    
    public partial class formulacion_comodin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public formulacion_comodin()
        {
            this.atributo_subatributo = new HashSet<atributo_subatributo>();
        }
    
        public int ID { get; set; }
        public int ID_PRODUCTO { get; set; }
        public int ID_PIEZA { get; set; }
        public int ID_COMODIN { get; set; }
        public double CANTIDAD { get; set; }
    
        public virtual piezas_configurables piezas_configurables { get; set; }
        public virtual producto producto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<atributo_subatributo> atributo_subatributo { get; set; }
    }
}
