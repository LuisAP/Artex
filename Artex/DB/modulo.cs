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
    
    public partial class modulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public modulo()
        {
            this.permisos_modulo = new HashSet<permisos_modulo>();
            this.rol = new HashSet<rol>();
        }
    
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public Nullable<int> ID_PADRE { get; set; }
        public string URL { get; set; }
        public string CONTROLADOR { get; set; }
        public int NIVEL { get; set; }
        public int VERSION { get; set; }
        public string ICONO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<permisos_modulo> permisos_modulo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rol> rol { get; set; }
    }
}
