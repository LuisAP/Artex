using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artex.Util.Sistema;
using System.Data.Entity;
using Artex.Models.DAL.DAO;
using Artex.Models.DAL.DAO.Views;
using Artex.DB;
using Artex.Models.ViewModels.Catalogos;
using Artex.Util;
using Artex.Models.DAL.DTO.Catalogos;
using Artex.Models.BLL.Productos;
using Artex.Models.BLL.Catalogos;

namespace Artex.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        private ArtexConnection db = new ArtexConnection();

        private const string ABSOLUTE_PATH = "~/Views/Catalogos/Productos/ListaProductos.cshtml";
        private const string VER_ABSOLUTE_PATH = "~/Views/Catalogos/Productos/ListaProductos.cshtml";
        private const string CREAR_ABSOLUTE_PATH = "~/Views/Catalogos/Productos/CrearProducto.cshtml";
        private const string EDITAR_ABSOLUTE_PATH = "~/Views/Catalogos/Productos/EditarProducto.cshtml";

        // GET: Producto
        public ActionResult Index()
        {
            PermisosModel model = PermisosModulo.ObtenerPermisos(Modulo.PRODUCTOS);

            if (model == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult GetAlls()
        {

            var consulta = db.producto_v;

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID_PRODUCTO,
                            NOMBRE = c.NOMBRE,
                            DESCRIPCION = c.DESCRIPCION,
                            STATUS_SKU = c.STATUS_SKU,
                            CODIGO = c.CODIGO,
                            //ACTIVO = c.ACTIVO,
                            FOTO = c.FOTO,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Crear()
        {
            if (!PermisosModulo.ObtenerPermiso(Modulo.PRODUCTOS, Permiso.CREAR))
            {
                TempData["message"] = "danger,No tiene permisos.";
                return RedirectToAction("Index");
            }
            ProductoModel model = new ProductoModel();
            ProductoBLL prodBLL = new ProductoBLL();
            producto entity = null;
            try
            {
                model.UnidadNegocioList = db.unidad_de_negocio.Where(m => m.ACTIVO);
                model.LineaNegocioList = db.linea_negocio.Where(m => m.ACTIVO);
                model.disenioList = db.disenio.Where(m => m.ACTIVO);
                model.FamilaProductoList = db.familia_producto.Where(m => m.ACTIVO);
                model.EstatusSkuList = db.estatus_sku.Where(m => m.ACTIVO);
                model.lineaProductoList = db.linea_producto.Where(m => m.ACTIVO);
                model.conceptoProductoList = db.concepto_producto.Where(m => m.ACTIVO);
                model.segmentoList = db.segmento.Where(m => m.ACTIVO);
                model.estiloList = db.estilo_producto.Where(m => m.ACTIVO);
              //  model.Piezas = prodBLL.ObtenerPiezas(entity, db);
                model.Editable = true;


            }
            catch (Exception e)
            {
                LogUtil.ExceptionLog(e);
                model = null;
            }
            ViewBag.Editar = true;
            return View(CREAR_ABSOLUTE_PATH, model);
        }
        public ActionResult Ver(int id)
        {
            if (!PermisosModulo.ObtenerPermiso(Modulo.PRODUCTOS, Permiso.EDITAR))
            {
                TempData["message"] = "danger,No tiene permisos.";
                return RedirectToAction("Index");
            }
            ProductoModel model = new ProductoModel();
            ProductoBLL prodBLL = new ProductoBLL();
            producto entity = db.producto.Find(id);
            try
            {
                model.UnidadNegocioList = db.unidad_de_negocio.Where(m => m.ACTIVO);
                model.LineaNegocioList = db.linea_negocio.Where(m => m.ACTIVO);
                model.disenioList = db.disenio.Where(m => m.ACTIVO);
                model.FamilaProductoList = db.familia_producto.Where(m => m.ACTIVO);
                model.EstatusSkuList = db.estatus_sku.Where(m => m.ACTIVO);
                model.lineaProductoList = db.linea_producto.Where(m => m.ACTIVO);
                model.conceptoProductoList = db.concepto_producto.Where(m => m.ACTIVO);
                model.segmentoList = db.segmento.Where(m => m.ACTIVO);
                model.estiloList = db.estilo_producto.Where(m => m.ACTIVO);
               // model.Piezas = prodBLL.ObtenerPiezas(entity, db);

                entityToModel(ref entity, ref model);
              //  model.configuracion = CargarConfiguracion(entity.ID);

            }
            catch (Exception e)
            {
                LogUtil.ExceptionLog(e);
                model = null;
                TempData["message"] = "danger,Ocurrio un error al cargar sus datos";
                return RedirectToAction("Index");
            }
            ViewBag.Editar = false;
            return View(EDITAR_ABSOLUTE_PATH, model);
        }

        public ActionResult Editar(int id)
        {
            if (!PermisosModulo.ObtenerPermiso(Modulo.PRODUCTOS, Permiso.EDITAR))
            {
                TempData["message"] = "danger,No tiene permisos.";
                return RedirectToAction("Index");
            }
            ProductoModel model = new ProductoModel();
            ProductoBLL prodBLL = new ProductoBLL();
            producto entity = db.producto.Find(id);
            try
            {
                model.UnidadNegocioList = db.unidad_de_negocio.Where(m => m.ACTIVO);
                model.LineaNegocioList = db.linea_negocio.Where(m => m.ACTIVO);
                model.disenioList = db.disenio.Where(m => m.ACTIVO);
                model.FamilaProductoList = db.familia_producto.Where(m => m.ACTIVO);
                model.EstatusSkuList = db.estatus_sku.Where(m => m.ACTIVO);
                model.lineaProductoList = db.linea_producto.Where(m => m.ACTIVO);
                model.conceptoProductoList = db.concepto_producto.Where(m => m.ACTIVO);
                model.segmentoList = db.segmento.Where(m => m.ACTIVO);
                model.estiloList = db.estilo_producto.Where(m => m.ACTIVO);
                model.PiezasSeleccionadas = prodBLL.ObtenerPiezas(entity, db);

                entityToModel(ref entity, ref model);
               // model.configuracion = CargarConfiguracion(entity.ID);

            }
            catch (Exception e)
            {
                LogUtil.ExceptionLog(e);
                model = null;
                TempData["message"] = "danger,Ocurrio un error al cargar sus datos";
                return RedirectToAction("Index");
            }
            ViewBag.Editar = true;
            return View(EDITAR_ABSOLUTE_PATH, model);
        }

        [HttpPost]
        public JsonResult GuardarNuevo(ProductoModel model)
        {

            var rm = new ResponseModel();
            producto entity = new producto();

            if (!ModelState.IsValid || !ProductoBLL.GenerarCodigoProducto(ref model,ref entity,db))
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }


            try
            {

                modelToEntity(ref entity, ref model);


                db.producto.Add(entity);
                if (db.SaveChanges() > 0 || db.Entry(entity).State == EntityState.Unchanged)
                {
                    rm.response = true;
                    rm.message = null; //"Sus datos se guardaron correctamente";
                        rm.href = "Editar?id=" + entity.ID;
                    TempData["message"] = "success,Sus datos se guardaron correctamente";
                }

            }
            catch (Exception e)
            {
                rm.SetResponse(false, e.Message);

                // LogUtil.ExceptionLog(e);
            }



            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarActualizado(ProductoModel model)
        {

            var rm = new ResponseModel();
            var entity = db.producto.Find(model.Id);
            Boolean succes = true;

            if((bool)entity.EDITABLE 
                && (entity.ID_UNIDAD_NEGOCIO!=model.UnidadNegocio
                || entity.ID_LINEA_NEGOCIO != model.LineaNegocio
                || entity.ID_DISENIO != model.disenio
                || entity.ID_FAMILIA_PRODUCTO != model.FamilaProducto))


                succes = ProductoBLL.GenerarCodigoProducto(ref model, ref entity, db);

            if (!ModelState.IsValid || !succes)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }
            

            try
            {
                

                modelToEntity(ref entity, ref model);

                if (db.SaveChanges() > 0 || db.Entry(entity).State == EntityState.Unchanged)
                {
                    rm.response = true;
                    rm.message = null; //"Sus datos se guardaron correctamente";
                    rm.href = "Editar?id=" + entity.ID;
                    TempData["message"] = "success,Sus datos se guardaron correctamente";
                }

            }
            catch (Exception e)
            {
                rm.SetResponse(false, e.Message);

                // LogUtil.ExceptionLog(e);
            }



            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReconfigurarPiezas(List<piezasDTO> piezas,int IdProducto )
        {
            var rm = new ResponseModel();
            Boolean esConfigurable = false;
            var entityProd = db.producto.Find(IdProducto);

            foreach (piezasDTO p in piezas)
            {
                //var pieza = db.piezas_configurables.Find(p.id);
                var existe = db.piezas_producto.FirstOrDefault(m => m.ID_PRODUCTO == IdProducto && m.ID_PIEZA == p.id);

                if (p.seleccionado)
                {
                    if (existe != null)
                    {
                        entityProd.piezas_producto.FirstOrDefault(m=> m.ID_PIEZA==p.id).ES_CONFIGURABLE = p.configurable;
                        //Si la pieza cambia a no configurable se eliminan los comodines ligados
                        if (!p.configurable)
                            entityProd.formulacion_comodin.Where(m => m.ID_PIEZA == p.id).ToList().ForEach(m => { db.formulacion_comodin.Remove(m); });

                        
                    }
                    else
                    {
                        piezas_producto nuevo = new piezas_producto();
                        nuevo.ID_PIEZA = p.id;
                        nuevo.ES_CONFIGURABLE = p.configurable;
                        entityProd.piezas_producto.Add(nuevo);
                    }

                }
                else
                {
                    if (existe != null)
                    {
                        entityProd.piezas_producto.Remove(existe);

                        //eliminar la configuracion y comodines ligados a esta pieza de èste producto
                       entityProd.formulacion.Where(m=> m.ID_PIEZA==p.id).ToList().ForEach(m => { db.formulacion.Remove(m); });
                       entityProd.formulacion_comodin.Where(m=> m.ID_PIEZA==p.id).ToList().ForEach(m => { db.formulacion_comodin.Remove(m); });
                    }
                }
            }
            entityProd.ES_PERSONALIZABLE = esConfigurable;

            if (db.SaveChanges() > 0)
            {
                rm.response = true;
                rm.message = null;// "Sus datos se guardaron correctamente";
              //  rm.href = "self";
            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        
        public void modelToEntity(ref producto entity, ref ProductoModel model)
        {
            
              

            entity.NOMBRE = model.Nombre;
            entity.DESCRIPCION = model.Descripcion;                     
            entity.ID_STATUS_SKU = model.EstatusSku;
            entity.ID_LINEA_PRODUCTO = model.lineaProducto;
            entity.ID_CONCEPTO_PRODUCTO = model.conceptoProducto;
            entity.ID_SEGMENTO = model.segmento;
            entity.ID_ESTILO = model.estilo;
           // entity.ES_PERSONALIZABLE = model.EsPersonalizable;
            entity.PUNTUACION = model.Puntuacion != null ? (double)ExtensionMethods.ConverToDecimalFormat(model.Puntuacion) : entity.PUNTUACION;


            //entity.ACTIVO = model.Activo;

      
        }
        public void entityToModel(ref producto entity, ref ProductoModel model)
        {
            model.Editable = entity.EDITABLE;
            model.Codigo = entity.CODIGO;
            model.Id = entity.ID;
            model.Nombre = entity.NOMBRE;
            model.Descripcion = entity.DESCRIPCION;
            model.UnidadNegocio = entity.ID_UNIDAD_NEGOCIO;
            model.LineaNegocio = entity.ID_LINEA_NEGOCIO;
            model.disenio = (int)entity.ID_DISENIO;
            model.FamilaProducto = (int)entity.ID_FAMILIA_PRODUCTO;
            model.EstatusSku = entity.ID_STATUS_SKU;
            model.lineaProducto = (int)entity.ID_LINEA_PRODUCTO;
            model.conceptoProducto = (int)entity.ID_CONCEPTO_PRODUCTO;
            model.segmento = (int)entity.ID_SEGMENTO;
            model.estilo = (int)entity.ID_ESTILO;
            model.EsPersonalizable = entity.ES_PERSONALIZABLE;
            model.Puntuacion = entity.PUNTUACION > 0 ? ExtensionMethods.decimalToString((decimal)entity.PUNTUACION) : "";
           // model.Activo = entity.ACTIVO;

/*
            model.PrecioCompra = ExtensionMethods.decimalToString((decimal)entity.PRECIO_COMPRA);
            model.PrecioRetail = ExtensionMethods.decimalToString(entity.PRECIO_RETAIL);
            model.PrecioMayoreo = ExtensionMethods.decimalToString(entity.PRECIO_MAYOREO);
            model.DescuentoRetail = ExtensionMethods.decimalToString((decimal)entity.DESCUENTO_RETAIL);
            model.DescuentoMayoreo = ExtensionMethods.decimalToString((decimal)entity.DESCUENTO_MAYOREO);
            model.DescuentoCadenas = ExtensionMethods.decimalToString((decimal)entity.FACTOR_CADENAS);
            model.DescuentoFranquicia = ExtensionMethods.decimalToString((decimal)entity.FACTOR_FRANQUICIA);
            */
        }

        [HttpPost]
        public JsonResult Subir(HttpPostedFileBase file, int id) {
            var rm = new ResponseModel();
            string archivo = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + file.FileName).ToLower();
            file.SaveAs(Server.MapPath("~/Content/img/img_producto/" + archivo));
            using (ArtexConnection db = new ArtexConnection())
            {
                var consulta = db.producto.Where(m => m.ID == id).FirstOrDefault();
                if (consulta == null)
                {
                    rm.message = "error";
                    rm.response = false;
                }
                else {
                    consulta.FOTO = archivo;                    
                }

                if (db.SaveChanges() > 0 || db.Entry(consulta).State == EntityState.Unchanged)
                {
                    rm.result = true;
                    rm.message = "La imagen se subio exitosamente";
                    rm.function = "reload(true,'" + rm.message + "')";
                }

            }
           

            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        
       
        #region Formulacion
        public ActionResult ObtenerMpProducto(int idProd, int idPieza)
        {

            var consulta = db.formulacion_v.Where(m => m.ID_PRODUCTO == idProd).Where(m => m.ID_PIEZA == idPieza);
            var jsonData = new
            {
                rows = (
                      from c in consulta
                      select new
                      {
                          ID = c.ID,
                          ID_PRODUCTO = c.ID_PRODUCTO,
                          PRODUCTO = c.PRODUCTO,
                          ID_MP = c.ID_MP,
                          MP = c.MP,
                          CANTIDAD = c.CANTIDAD,
                          PRESENTACION_ENTREGA = c.PRESENTACION_ENTREGA,
                          //TIPO = c.TIPO,
                      }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerComodinProducto(int idProd, int idPieza)
        {

            var consulta = db.formulacion_comodin_v.Where(m => m.ID_PRODUCTO == idProd).Where(m => m.ID_PIEZA == idPieza);
            var jsonData = new
            {
                rows = (
                      from c in consulta
                      select new
                      {
                          ID = c.ID,
                          ID_PRODUCTO = c.ID_PRODUCTO,
                          PRODUCTO = c.PRODUCTO,
                          ID_COMODIN = c.ID_COMODIN,
                          COMODIN = c.COMODIN,
                          CANTIDAD = c.CANTIDAD,
                          //PRESENTACION_ENTREGA = c.PRESENTACION_ENTREGA,
                          //TIPO = c.TIPO,
                      }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult GetMp(int idProd, int idPieza)
        {
            HashSet<int> idMpIncluido = new HashSet<int>(from x in db.formulacion.Where(m => m.ID_PRODUCTO==idProd && m.ID_PIEZA== idPieza).Select(x => (int)x.ID_MP) select x);
            IEnumerable<materia_prima> consulta = db.materia_prima.Where(m=> m.ACTIVO);

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE
                        }).Where(x => !idMpIncluido.Contains(x.ID)).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetComodines(int idProd, int idPieza)
        {
            HashSet<int> idAgregados = new HashSet<int>(from x in db.formulacion_comodin.Where(m => m.ID_PRODUCTO == idProd && m.ID_PIEZA == idPieza).Select(x => x.ID_COMODIN) select x);
            IEnumerable<atributo_subatributo> consulta = db.atributo_subatributo.Where(m => m.TIENE_HIJOS);

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE
                        }).Where(x => !idAgregados.Contains(x.ID)).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult AgregarMp(int Id, int IdPieza, int mp, String cantidad)
        {
            var rm = new ResponseModel();

            if(db.formulacion.Any(m=> m.ID_PRODUCTO== Id && m.ID_PIEZA== IdPieza && m.ID_MP == mp))
            {
                rm.SetResponse(false, "Esta materia prima ya ha sido agregada");
                return Json(rm, JsonRequestBehavior.AllowGet);
            }

            formulacion entity = new formulacion();
            entity.ID_PRODUCTO = Id;
            entity.ID_PIEZA = IdPieza;
            entity.ID_MP = mp;
            entity.CANTIDAD =(float) ExtensionMethods.ConverToDecimalFormat(cantidad);

            db.formulacion.Add(entity);
            if (db.SaveChanges() > 0)
            {
                rm.response = true;
                rm.message = "Sus datos se guardaron correctamente";
                rm.function = "reload('cmb-mp" + IdPieza + "','grid-mp" + IdPieza+"')";
            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AgregarComodin(int Id, int IdPieza, int comodin, String cantidad)
        {
            var rm = new ResponseModel();

            if (db.formulacion_comodin.Any(m => m.ID_PRODUCTO == Id && m.ID_PIEZA == IdPieza && m.ID_COMODIN == comodin))
            {
                rm.SetResponse(false, "Este comodin y aha sido agregado");
                return Json(rm, JsonRequestBehavior.AllowGet);
            }

            formulacion_comodin entity = new formulacion_comodin();
            entity.ID_PRODUCTO = Id;
            entity.ID_PIEZA = IdPieza;
            entity.ID_COMODIN = comodin;
            entity.CANTIDAD = (float)ExtensionMethods.ConverToDecimalFormat(cantidad);

            db.formulacion_comodin.Add(entity);
            if (db.SaveChanges() > 0)
            {
                rm.response = true;
                rm.message = "Sus datos se guardaron correctamente";
                rm.function = "reload('cmb-comodin" + IdPieza + "','grid-comodin" + IdPieza + "')";
            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }
     
        [HttpPost]
        public JsonResult EditaFormulacion(int editId, string   editCantidad)
        {
            var rm = new ResponseModel();
            var entity = db.formulacion.Find(editId);
            if (entity==null)
            {
                rm.SetResponse(false, "Error tecnico");
                return Json(rm, JsonRequestBehavior.AllowGet);

            }

            entity.CANTIDAD = (float)ExtensionMethods.ConverToDecimalFormat(editCantidad);

            if (db.SaveChanges() > 0)
            {
                rm.response = true;
                rm.message = "Sus datos se guardaron correctamente";
                rm.function = "reload('cmb-mp" + entity.ID_PIEZA + "','grid-mp" + entity.ID_PIEZA + "')";

            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditaFormulacionCmdn(int editId, string editCantidad)
        {
            var rm = new ResponseModel();
            var entity = db.formulacion_comodin.Find(editId);
            if (entity == null)
            {
                rm.SetResponse(false, "Error tecnico");
                return Json(rm, JsonRequestBehavior.AllowGet);

            }

            entity.CANTIDAD = (float)ExtensionMethods.ConverToDecimalFormat(editCantidad);

            if (db.SaveChanges() > 0)
            {
                rm.response = true;
                rm.message = "Sus datos se guardaron correctamente";
                rm.function = "reload('cmb-comodin" + entity.ID_PIEZA + "','grid-comodin" + entity.ID_PIEZA + "')";

            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteFormulacion(int id)
        {
            var rm = new ResponseModel();
            Boolean esMp = false;
            var entity = db.formulacion.Find(id);
            if (entity == null)
            {
                rm.SetResponse(false, "Error tecnico");
                return Json(rm, JsonRequestBehavior.AllowGet);

            }

            //esMp = entity.ID_MP > 0;
            db.formulacion.Remove(entity);
            if (db.SaveChanges() > 0)
            {
                rm.response = true;
                rm.message = "El registro se elimino correctamente";
                rm.function = "reload('cmb-mp" + entity.ID_PIEZA + "','grid-mp" + entity.ID_PIEZA + "')";

            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteFormulacionCmdn(int id)
        {
            var rm = new ResponseModel();
            Boolean esMp = false;
            var entity = db.formulacion_comodin.Find(id);
            if (entity == null)
            {
                rm.SetResponse(false, "Error tecnico");
                return Json(rm, JsonRequestBehavior.AllowGet);

            }

            db.formulacion_comodin.Remove(entity);
            if (db.SaveChanges() > 0)
            {
                rm.response = true;
                rm.message = "El registro se elimino correctamente";
                rm.function = "reload('cmb-comodin" + entity.ID_PIEZA + "','grid-comodin" + entity.ID_PIEZA + "')";

            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Agregar productos a tienda

        #endregion


        public ActionResult GetPiezas(int idProd)
        {
            var rm = new ResponseModel();
            var entityProd = db.piezas_producto.Where(m => m.ID_PRODUCTO == idProd).Include(m => m.piezas_configurables).ToList();

             var piezas = db.piezas_configurables.ToList();


            var jsonData = new
            {
                rows = (
                      from c in piezas
                      select new
                      {
                          NOMBRE = c.NOMBRE,
                          SELECCIONAR= entityProd.Any(m=> m.ID_PIEZA==c.ID),
                          CONFIGURAR= entityProd.Any(m => m.ID_PIEZA == c.ID && m.ES_CONFIGURABLE)

                      }).ToArray()
            };

            rm.response = true;
            rm.result = jsonData.rows;
            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        #region Configuracion

      /*  public JsonResult GetAtributoSubAtributo(int aux, int? id = null)
        {
            db.Configuration.ProxyCreationEnabled = false;
            AtributosBLL atributosBLL = new AtributosBLL();
            List<atributo_subatributo> listaAtributos = null;



            if (id > 0)
            {
                listaAtributos = db.atributo_subatributo.Where(m => m.ID == id).ToList();           // listaAtributos.Add(padre);
                listaAtributos = db.atributo_subatributo.Where(m => m.ID_PADRE == id).ToList();


            }
            else
            {
                listaAtributos = db.atributo_subatributo.Where(m => m.ID == aux).ToList();           // listaAtributos.Add(padre);
                listaAtributos.AddRange(atributosBLL.ListarHijos(aux, ref db));


            }

            // var listaAtributos = db.atributo_subatributo.Where(m => m.ID == id).ToList();           // listaAtributos.Add(padre);


            var jsonData = new
            {
                rows = (
                   from c in listaAtributos
                   select new
                   {
                       ID = c.ID,
                       NOMBRE = c.NOMBRE + " (" + c.CODIGO + ")",
                       DESCRIPCION = c.DESCRIPCION,
                       NIVEL = c.NIVEL,
                       ID_PADRE = c.ID_PADRE,
                       hasChildren = c.TIENE_HIJOS,
                   }).ToArray()
            };


            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }*/
        public ActionResult GetSubAtributos(int prod, int pieza, int comodin, int? id = null)
        {

            //var consulta = db.atributo_subatributo ;
            List<atributo_subatributo> consulta = new List<atributo_subatributo>();

            if (id > 0)
                consulta.AddRange(db.atributo_subatributo.Where(m => m.ID_PADRE == id));
            else
                consulta.Add( db.atributo_subatributo.FirstOrDefault(m => m.ID == comodin));


            var seleccioonado = db.formulacion_comodin.Where(m => m.ID_PRODUCTO == prod)
                .Where(m => m.ID_PIEZA == pieza)
                .Where(m => m.ID_COMODIN == comodin)
                .FirstOrDefault().atributo_subatributo.ToList();



            var jsonData = new
            {
                callback = (
                        from c in consulta
                        select new
                        {
                            c.ID,
                            NOMBRE = c.NOMBRE + " (cod: " + c.CODIGO + ")",
                            NIVEL = c.NIVEL,
                            ID_PADRE = c.ID_PADRE,

                            hasChildren = c.TIENE_HIJOS,
                            expanded = c.TIENE_HIJOS,
                            Checked = (seleccioonado.Contains(c)),
                            //c.formulacion_comodin
                        }).ToArray()
             };
        

            return Json(jsonData.callback, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GuardarConfiguracion(List<ConfiguracionDTO> configuracion)
        {
            var rm = new ResponseModel();
            int idProd = configuracion[0].idProducto;
            AtributosBLL atributosBLL = new AtributosBLL();

            try
            {

                var prod = db.producto.FirstOrDefault(m => m.ID == idProd);
                int count = 0;
                foreach (ConfiguracionDTO config in configuracion)
                {
                    var comodin = db.formulacion_comodin.FirstOrDefault(m => m.ID == config.idFormulacion);

                    comodin.atributo_subatributo.Clear();

                    if(config.seleccionados!=null)
                        foreach (AtributosSeleccionadosDTO selec in config.seleccionados)
                        {
                            //obtenemos todos los padres del atributo hasta llegar al comodin padre 
                            var t = atributosBLL.ListarPadres(selec.id, ref db);

                            t.ForEach(m => { comodin.atributo_subatributo.Add(m);  });
                            // comodin.atributo_subatributo.Concat(t);
                            count++;
                        }



                }
                prod.ES_PERSONALIZABLE = count > 0;

                if (db.SaveChanges() > 0)
                {
                    rm.response = true;
                    rm.message = "Sus datos se guardaron correctamente";
                    rm.function = "reloadModalMp(true,'" + rm.message + "')";
                }
            }
            catch(Exception e)
            {

            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
       /* public ActionResult getpadre(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var rm = new ResponseModel();
            AtributosBLL atributosBLL = new AtributosBLL();

            var t = atributosBLL.ListarPadre(id, ref db);



            return Json(t, JsonRequestBehavior.AllowGet);
        }*/
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}