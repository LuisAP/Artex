using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using Artex.Models.DAL.DTO.Costos;

namespace Artex.Models.BLL.Costos
{
    public class FactorCanalBLL
    {
        public List<lineaDTO> ListLineaNegocioDTO(List<linea_negocio> linea, List<factor_canal_linea> factorLinea)
        {
            List<lineaDTO> listDTO = new List<lineaDTO>();

            foreach (linea_negocio l in linea)
            {
                var dto = new lineaDTO();
                var factor_linea = factorLinea.FirstOrDefault(m => m.ID_LINEA_NEGOCIO == l.ID);

                dto.ID = l.ID;
                dto.NOMBRE = l.NOMBRE;

                if (factor_linea != null)
                {
                    dto.FACTOR_POP = factor_linea.FACTOR_POP;
                    dto.FACTOR_FRANQUICIA = factor_linea.FACTOR_FRANQUICIA;
                    dto.FACTOR_CADENAS = factor_linea.FACTOR_CADENAS;
                    dto.FACTOR_PROYECTOS = factor_linea.FACTOR_PROYECTOS;

                }
             
                listDTO.Add(dto);
            }

            return listDTO;
        }

        public lineaDTO LineaNegocioDTO(linea_negocio linea, factor_canal_linea factorLinea)
        {

              var dto = new lineaDTO();

                dto.ID = linea.ID;
                dto.NOMBRE = linea.NOMBRE;

                if (factorLinea != null)
                {
                    dto.FACTOR_POP = factorLinea.FACTOR_POP;
                    dto.FACTOR_FRANQUICIA = factorLinea.FACTOR_FRANQUICIA;
                    dto.FACTOR_CADENAS = factorLinea.FACTOR_CADENAS;
                    dto.FACTOR_PROYECTOS = factorLinea.FACTOR_PROYECTOS;

                }
            
          

            return dto;
        }
     
    }
}