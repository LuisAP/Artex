using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Artex.Models.DAL.DTO.RecursosHumanos
{
    public class ModuloDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public bool esRaiz { get; set; }
        public int idPadre { get; set; }
        public bool habilitado { get; set; }

        public bool ver { get; set; }
        public bool crear { get; set; }
        public bool editar { get; set; }
        public bool eliminar { get; set; }
        public bool reportes { get; set; }

        public List<ModuloDTO> listaSubmodulo { get; set; }
    }
}