using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using Artex.Models.DAL.DTO.Costos;

namespace Artex.Models.BLL.Costos
{
    public class FactorFabricaBLL
    {
        public List<lineaDTO> ListLineaNegocioDTO(List<linea_negocio> linea, List<factor_fabrica_linea> factorLinea)
        {
            List<lineaDTO> listDTO = new List<lineaDTO>();

            foreach (linea_negocio l in linea)
            {
                var dto = new lineaDTO();
                var descuento_linea = factorLinea.FirstOrDefault(m => m.ID_LINEA_NEGOCIO == l.ID);

                dto.ID = l.ID;
                dto.NOMBRE = l.NOMBRE;

                if (descuento_linea != null)
                {
                    dto.FACTOR_FABRICA = descuento_linea.FACTOR_FABRICA;
                    dto.DESCUENTO_POP = descuento_linea.DESCUENTO_POP;
                    dto.DESCUENTO_FRANQUICIA = descuento_linea.DESCUENTO_FRANQUICIA;
                    dto.DESCUENTO_CADENAS = descuento_linea.DESCUENTO_CADENAS;
                    dto.DESCUENTO_PROYECTOS = descuento_linea.DESCUENTO_PROYECTOS;

                }
               
                listDTO.Add(dto);
            }

            return listDTO;
        }

    }
}