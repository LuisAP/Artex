using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Artex.Models.DAL.DTO.Ventas
{
    public class ConfiguracionesDTO
    {
        public int idProducto { get; set; }

        public List<caraDTO> ListaCaras { get; set; }

    }
    public class caraDTO
    {
        public int idProducto { get; set; }
        public int idCara { get; set; }
        public string cara { get; set; }

        public List<comodinDTO> ListaComodines { get; set; }//comodines por cara

    }
    public class comodinDTO
    {
        public int idFormulacion { get; set; }
        public int idAtributo { get; set; }
        public int startLevel { get; set; }
        public int endLevel { get; set; }
        public string nombre { get; set; }

        public List<combosDTO> listaCombos { get; set; }//combos por comodin

    }

    public class combosDTO
    {
        public string nombre { get; set; }
        public string codigo { get; set; }
        public int nivel { get; set; }             //el nivel se obtine  directamente de la basde de datos
        public int numCmbo { get; set; }        //profundidad indica la posicion del combo en la cascada de combos
        public int seleccionado { get; set; }
        public Boolean ultimoCmbo { get; set; }

    }

    public class SeleccionProductoDTO
    {
        public int idProducto { get; set; }

        public List<atributoSeleccionadosDTO> listaseleccionados { get; set; }

    }
    public class atributoSeleccionadosDTO
        {
            public int idFormulacion { get; set; }

            public int idSeleccionado { get; set; }
        }
}