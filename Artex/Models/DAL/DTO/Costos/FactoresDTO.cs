using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Artex.Models.DAL.DTO.Costos
{
    public class FactoresDTO
    {
    }
    public class lineaDTO
    {
        public int ID { get; set; }
        public string NOMBRE { get; set; }

        public double? FACTOR_FABRICA {get;set;}

        public double? FACTOR_POP { get; set; }
        public double? FACTOR_FRANQUICIA { get; set; }
        public double? FACTOR_PROYECTOS { get; set; }
        public double? FACTOR_CADENAS { get; set; }

        public double? DESCUENTO_POP { get; set; }
        public double? DESCUENTO_FRANQUICIA { get; set; }
        public double? DESCUENTO_PROYECTOS { get; set; }
        public double? DESCUENTO_CADENAS { get; set; }
    }
}