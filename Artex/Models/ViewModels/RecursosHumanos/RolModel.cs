using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.Models.DAL.DTO.RecursosHumanos;


namespace Artex.Models.ViewModels.RecursosHumanos
{
    public class RolModel
    {
        public int idRol { get; set; }

        [Required(ErrorMessage = "El nombre es requerdio")]
        [Display(Name = "Nombre:")]
        public string nombre { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Descripción:")]
        public string descripcion { get; set; }

        public List<ModuloDTO> listaModulosSubmodulos { get; set; }

        public List<PermisosEspecialesDTO> listaPermisosEspecilaes { get; set; }

        [Display(Name = "Activo:")]
        public bool habilitado { get; set; }

    }
}