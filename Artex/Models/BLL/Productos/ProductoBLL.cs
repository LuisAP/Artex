using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.Models.DAL.DTO.Catalogos;
using Artex.DB;
using System.Data.Entity;
using Artex.Util;
using Artex.Models.ViewModels.Catalogos;

namespace Artex.Models.BLL.Productos
{
    public class ProductoBLL
    {
        /*
        //obtener configuracion por piezaa
        public ListaTipoAtributosDTO ObtenerConfiguracion(int idPieza, producto prodEntity, ArtexConnection db)
        {
            var configProductoPieza = prodEntity.configuracion_producto.Where(m => m.ID_PIEZA == idPieza);
            var tipoAtributo = db.tipo_atributo.Where(m => m.ACTIVO);
            var insumos = db.insumo.Where(m => m.ACTIVO);
            var materiaPrima = db.materia_prima.Where(m => m.ACTIVO).Include(m => m.unidad_medida);
            int idProd = prodEntity.ID;

            List<int> atributosActivos = prodEntity.configuracion_producto.Where(m => m.ID_PIEZA == idPieza).Select(m => m.ID_ATRIBUTO).ToList();
            List<int?> idMpActivo = db.configuracion_producto.Where(m => m.ID_PRODUCTO == idProd).Where(m => m.ID_PIEZA == idPieza).Where(m => m.ID_MP != null).Select(m => m.ID_MP).ToList();
           
            atributosActivos = atributosActivos == null ? new List<int>(): atributosActivos;
            idMpActivo = idMpActivo == null ? new List<int?>() : idMpActivo;
            configProductoPieza = configProductoPieza == null ? new List<configuracion_producto>() : configProductoPieza;



            ListaTipoAtributosDTO ListTipoAtributoDto = new ListaTipoAtributosDTO();
            ListTipoAtributoDto.listaTipoAtributos = new List<TipoAtributosDTO>();


            foreach (tipo_atributo tpAtibuto in tipoAtributo)
                {
                    TipoAtributosDTO tipoDto = new TipoAtributosDTO();
                    tipoDto.listaAtributo = new List<AtributosDTO>();
                    tipoDto.tipoAtributo = tpAtibuto;

                    tipoDto.identificador = ""+ idPieza+tpAtibuto.ID;

                var atributos = tpAtibuto.atributos_configuracion;
                foreach (atributos_configuracion atributo in atributos)
                {
                    AtributosDTO atributoDto = new AtributosDTO();
                    atributoDto.listaMp = new List<materiaPrimaDTO>();
                    atributoDto.atributo = atributo;
                    atributoDto.idTipoAtributo = atributo.ID_TIPO_ATRIBUTO;
                    atributoDto.id = atributo.ID;
                    atributoDto.seleccionado = atributosActivos.Contains(atributo.ID);


                    //obtenemos todas las mp relacionadas con el atributo
                    var mpByatributo = materiaPrima.Where(m => m.ID_ATRIBUTO == atributo.ID);
                    foreach (materia_prima mp in mpByatributo)
                    {
                        materiaPrimaDTO mpDto = new materiaPrimaDTO();
                        mpDto.mp = mp;
                        mpDto.id = mp.ID;
                        var aux = configProductoPieza.FirstOrDefault(m => m.ID_MP == mp.ID);
                        mpDto.cantidad = aux != null ?ExtensionMethods.decimalToString((decimal)aux.CANTIDAD) : "0";
                        mpDto.unidadMedida = mp.unidad_medida.NOMBRE;
                        mpDto.seleccionado = idMpActivo.Contains(mp.ID);

                        atributoDto.listaMp.Add(mpDto);
                    }
                    tipoDto.listaAtributo.Add(atributoDto);
                }
                ListTipoAtributoDto.listaTipoAtributos.Add(tipoDto);
                }
            
            return ListTipoAtributoDto;

        }
      */  public List<PiezasDto> ObtenerPiezas(producto prodEntity, ArtexConnection db)
        {

            List<int> PiezasProducto = null;
            if (prodEntity != null)
            {

                PiezasProducto = prodEntity.piezas_configurables.Select(m=>m.ID).ToList();
            }
            PiezasProducto = PiezasProducto ==null? new List<int>(): PiezasProducto;

            var todasPiezas = db.piezas_configurables.Where(m => m.ACTIVO);

            List<PiezasDto> piezas = new List<PiezasDto>();
            foreach( piezas_configurables p in todasPiezas)
            {
                PiezasDto dto = new PiezasDto();
                dto.pieza = p;
                dto.id = p.ID;
                dto.seleccionado = PiezasProducto.Contains(p.ID);
                piezas.Add(dto);
            }

            return piezas;
        }


        public string GenerarCodigoConfiguracion()
        {
            String codigo="";





            return codigo;
        }

        public static Boolean GenerarCodigoProducto(ref ProductoModel model, ref producto entity, ArtexConnection db)
        {
            String codigo = "";
            Boolean succes = false;

            try
            {
                //obtenemos las variables para generar codigo
                int consecutivo = 0;
                var un = db.unidad_de_negocio.Find(model.UnidadNegocio);
                var ln = db.linea_negocio.Find(model.LineaNegocio);
                var ds = db.disenio.Find(model.disenio);
                var fm = db.familia_producto.Find(model.FamilaProducto);

                //obtenemos el ultimo registro con codigo base
                var ultimo = db.producto
                    .Where(m => m.ID_UNIDAD_NEGOCIO == un.ID)
                    .Where(m => m.ID_LINEA_NEGOCIO == ln.ID)
                    .Where(m => m.ID_DISENIO == ds.ID)
                    .Where(m => m.ID_FAMILIA_PRODUCTO == fm.ID)
                    .OrderByDescending(m => m.ID).FirstOrDefault();

                if (ultimo != null)
                {
                    consecutivo = int.Parse(ultimo.CODIGO.Split('-')[1]);

                }
                consecutivo++;

                //generamos el codigo
                codigo = un.LETRA_CODIGO + ln.LETRA_CODIGO + ds.LETRA_CODIGO + fm.LETRA_CODIGO + "-" + ExtensionMethods.rellenarCadena(consecutivo,4);
                entity.CODIGO = codigo;
                entity.ID_UNIDAD_NEGOCIO = model.UnidadNegocio;
                entity.ID_LINEA_NEGOCIO = model.LineaNegocio;
                entity.ID_DISENIO = model.disenio;
                entity.ID_FAMILIA_PRODUCTO = model.FamilaProducto;

                succes = true;
            }
            catch (Exception e)
            {
                succes = false;
            }
            return succes;
        }

            /*
            public FormulacionDTO ObtenerFormulacion(producto prodEntity, ArtexConnection db)
            {
                FormulacionDTO formulacionDTO = new FormulacionDTO();
                 formulacionDTO.listaMp = new List<FormulacionMpDTO>();
                 formulacionDTO.listaInsumos = new List<FormulacionInsumoDTO>();
                var mpEntity = db.materia_prima.Where(m => m.ACTIVO).Include(m => m.unidad_medida);
                var insumosEntity = db.insumo.Where(m => m.ACTIVO).Include(m => m.unidad_medida);

                var mpEntityActivo = prodEntity.materia_prima;
                var insumosEntityActivo = prodEntity.insumo;
                var formulacionEntity = prodEntity.formulacion;


               // List<FormulacionMpDTO> mpDtoList = new List<FormulacionMpDTO>();
               //List<FormulacionInsumoDTO> insumoDtoList = new List<FormulacionInsumoDTO>();

                //agregar materias primas y comprobar si estan ligadas al producto
                foreach (materia_prima mp in mpEntity)
                {
                    FormulacionMpDTO mpDto = new FormulacionMpDTO();
                    mpDto.mp = mp;
                    mpDto.id = mp.ID;
                    mpDto.unidadMedida = mp.unidad_medida.NOMBRE;
                    if (mpEntityActivo.Contains(mp))
                    {
                        mpDto.cantidad = ExtensionMethods.decimalToString((decimal)formulacionEntity.FirstOrDefault(m => m.ID_MP == mp.ID).CANTIDAD);
                        mpDto.seleccionado = true;
                    }
                    else
                        mpDto.cantidad = "0";
                    formulacionDTO.listaMp.Add(mpDto);
                }


                //agregar insumos primas y comprobar si estan ligados al producto
                foreach (insumo ins in insumosEntity)
                {
                    FormulacionInsumoDTO insumoDto = new FormulacionInsumoDTO();
                    insumoDto.insumos = ins;
                    insumoDto.id = ins.ID;
                    insumoDto.unidadMedida = ins.unidad_medida.NOMBRE;
                    if (insumosEntityActivo.Contains(ins))
                    {
                        insumoDto.cantidad = ExtensionMethods.decimalToString((decimal)formulacionEntity.FirstOrDefault(m => m.ID_MP == ins.ID).CANTIDAD);
                        insumoDto.seleccionado = true;
                    }
                    else
                        insumoDto.cantidad = "0";
                    formulacionDTO.listaInsumos.Add(insumoDto);
                }

                return  formulacionDTO;
            }
            */

        }
}