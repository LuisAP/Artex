using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using Artex.Models.DAL.DAO;
using Artex.Models.DAL.DTO.RecursosHumanos;
using Artex.Models.ViewModels.RecursosHumanos;

namespace Artex.Models.BLL.RecursosHumanos
{
    public class ModuloBLL
    {
        public List<ModuloDTO> ObtenerModeloModuloSubmodulos(rol rolEntity, ArtexConnection artexContext)
        {
            List<ModuloDTO> listaModuloSubmodulo = new List<ModuloDTO>();
          

            List<modulo> listaModulos = artexContext.modulo.ToList();
            List<modulo> modulosSeleccionados = rolEntity != null ? rolEntity.modulo.ToList() : null;

            // Listar todos los modulos padres
            List<modulo> modulosRaiz = listaModulos.Where(m => m.ID_PADRE == null).ToList();

                foreach (modulo moduloPadre in modulosRaiz)
                {
                    ModuloDTO moduloSubmoduloDto = new ModuloDTO();

                moduloSubmoduloDto.id = moduloPadre.ID;
                moduloSubmoduloDto.nombre = moduloPadre.NOMBRE;
                moduloSubmoduloDto.esRaiz = true;
                //habilitar modulo si pertenece al rol
                moduloSubmoduloDto.habilitado = modulosSeleccionados != null ? modulosSeleccionados.Contains(moduloPadre) : false;


                // Traer todos los modulos hijos de este modulo
                List<modulo> modulosHijos = listaModulos.Where(m => m.ID_PADRE == moduloPadre.ID).ToList();

                    List<ModuloDTO> listaModuloSubmoduloHijos = new List<ModuloDTO>();

                    foreach (modulo moduloHijo in modulosHijos)
                    {
                    ModuloDTO moduloSubmoduloDtoHijo = new ModuloDTO();
                    moduloSubmoduloDtoHijo.id = moduloHijo.ID;
                    moduloSubmoduloDtoHijo.nombre = moduloHijo.NOMBRE;
                    moduloSubmoduloDtoHijo.esRaiz = false;
                    moduloSubmoduloDtoHijo.idPadre = (int)moduloHijo.ID_PADRE;

                    //habilitar Submodulo si pertenece al rol
                    if (modulosSeleccionados!=null&& modulosSeleccionados.Contains(moduloHijo))
                    {
                        moduloSubmoduloDtoHijo.habilitado = true;
                        var permisosModulo = rolEntity.permisos_modulo.FirstOrDefault(m => m.ID_MODULO == moduloSubmoduloDtoHijo.id);
                        if (permisosModulo != null)
                        {
                            moduloSubmoduloDtoHijo.ver = permisosModulo.VER;
                            moduloSubmoduloDtoHijo.editar = permisosModulo.EDITAR;
                            moduloSubmoduloDtoHijo.crear = permisosModulo.CREAR;
                            moduloSubmoduloDtoHijo.eliminar = permisosModulo.ELIMINAR;
                            moduloSubmoduloDtoHijo.reportes = permisosModulo.REPORTE;
                        }
                    }

                   

                    listaModuloSubmoduloHijos.Add(moduloSubmoduloDtoHijo);
                    }

                    moduloSubmoduloDto.listaSubmodulo = listaModuloSubmoduloHijos;

                    listaModuloSubmodulo.Add(moduloSubmoduloDto);
                }
            

            return listaModuloSubmodulo;
        }
        public List<PermisosEspecialesDTO> ObtenerPermisosEspeciales(rol rolEntity, ArtexConnection artexContext)
        {
            List<PermisosEspecialesDTO> listaDTO = new List<PermisosEspecialesDTO>();
            List<permisos_especiales> listaEntity = artexContext.permisos_especiales.ToList();
            List<permisos_especiales> listPorRol =  rolEntity != null ? rolEntity.permisos_especiales.ToList() : null;

            foreach (permisos_especiales entity in listaEntity)
            {
                PermisosEspecialesDTO dto = new PermisosEspecialesDTO();

                dto.id = entity.ID;
                dto.nombre = entity.NOMBRE;
                dto.habilitado = listPorRol != null ? listPorRol.Contains(entity):false ;
                

                listaDTO.Add(dto);
            }


            return listaDTO;
        }

    }
}