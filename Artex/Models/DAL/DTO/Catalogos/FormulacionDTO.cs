using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;

namespace Artex.Models.DAL.DTO.Catalogos
{
    public class FormulacionDTO
    {
        public List<FormulacionMpDTO> listaMp { get; set; }
        public List<FormulacionInsumoDTO> listaInsumos { get; set; }
    }
    public class FormulacionMpDTO
    {
        public int id { get; set; }
        public materia_prima mp { get; set; }
        public string cantidad { get; set; }
        public string unidadMedida { get; set; }
        public Boolean seleccionado { get; set; }
    }
    public class FormulacionInsumoDTO
    {
        public int id { get; set; }
        public insumo insumos { get; set; }
        public string cantidad { get; set; }
        public string unidadMedida { get; set; }
        public Boolean seleccionado { get; set; }
    }

    public class piezasDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public Boolean seleccionado { get; set; }
        public Boolean configurable { get; set; }
        public List<ComodinesDTO> comodines { get; set; }

    }

    public class ComodinesDTO
    {
        public int idRaiz { get; set; }
        public int idFormulacionAtributo { get; set; }
        public string nombre { get; set; }
        //public Boolean seleccionado { get; set; }

    }



    public class ConfiguracionDTO
    {
        public int idProducto { get; set; }
        public int idPieza { get; set; }
        public int idFormulacion { get; set; }

        public string nombre { get; set; }
       
        public List<AtributosSeleccionadosDTO> seleccionados { get; set; }

    }

    public class AtributosSeleccionadosDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        //public Boolean seleccionado { get; set; }

    }
}