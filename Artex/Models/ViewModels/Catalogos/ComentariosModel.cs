using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.Util.Sistema;
using Artex.DB;

namespace Artex.Models.ViewModels.Catalogos
{
    public class ComentariosModel {
        public int Id { get; set; }

        public IEnumerable<tipo_programa> ProgramaList { get { return new List<tipo_programa>(); } set { } }
        [Display(Name = "Tipo de Programa")]
        [Required(ErrorMessage = "El Programa es requerido")]
        public int? Programa { get; set; }

        [Display(Name ="Comentario")]
        [Required(ErrorMessage = "Escriba un comentario")]
        public string Comentario { get; set; }

        public PermisosModel permisos { get; set; }
    }
}