using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.DB;
using Artex.Util;

namespace Artex.Models.DAL.DTO.Ventas
{
    public class ConfiguraPiezaDTO
    {
        //[Display(Name = "Piezas")]
        public int idPieza { get; set; }
        public String pieza { get; set; }

        public int idTipoAtributo { get; set; }
        public String TipoAtributo { get; set; }
       // public IEnumerable<atributos_configuracion> atributosList { get; set; }

    }
    public class SeleccionarAtributosDTO
    {
        //[Display(Name = "Piezas")]
        public int idPieza { get; set; }
        public String pieza { get; set; }

        public int idTipoAtributo { get; set; }
        public String TipoAtributo { get; set; }
       // public IEnumerable<atributos_configuracion> atributosList { get; set; }

    }
}