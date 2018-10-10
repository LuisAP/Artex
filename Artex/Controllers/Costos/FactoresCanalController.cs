using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artex.Models.ViewModels.Catalogos;
using Artex.DB;
using Artex.Models.DAL.DAO;
using Artex.Util;
using Artex.Util.Sistema;
using System.Data.Entity;
using Artex.Models.ViewModels.Costos;
using Artex.Models.BLL.Costos;


namespace Artex.Controllers.Catalogos
{
    [Authorize]
    public class FactoresCanalController : Controller
    {
        private ArtexConnection db = new ArtexConnection();

        private const string ABSOLUTE_PATH = "~/Views/Costos/FactoresCanal/Factores.cshtml";

        public ActionResult Index()
        {
            LineaNegocioModel model = new LineaNegocioModel();
            model.permisos = PermisosModulo.ObtenerPermisos(Modulo.LINEA_NEGOCIO);
            if (model.permisos == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH);
        }


        public ActionResult FactorByLinea()
        {
            FactorCanalBLL bll = new FactorCanalBLL();
            var factorLinea = db.factor_canal_linea.ToList();
            var lineas = db.linea_negocio.Where(m=> m.ACTIVO).ToList();

            var lista_linea = bll.ListLineaNegocioDTO(lineas,factorLinea);
      
            return Json(lista_linea, JsonRequestBehavior.AllowGet);
        }

    
        public ActionResult FactorByFamily(int idLinea)
        {

            var descuentoFamily = db.factor_canal_familia.Where(m => m.ID_LINEA == idLinea).Include(m => m.familia_producto);


            var jsonData = new
            {
                rows = (
                        from c in descuentoFamily
                        select new
                        {
                            c.ID_LINEA,
                            c.ID_FAMILIA,
                            FAMILIA = c.familia_producto.NOMBRE,

                            FACTOR_POP = c.FACTOR_POP,
                            FACTOR_FRANQUICIA = c.FACTOR_FRANQUICIA,
                            FACTOR_PROYECTOS = c.FACTOR_PROYECTOS,
                            FACTOR_CADENAS = c.FACTOR_CADENAS,

                         
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductosConDescuento()
        {
            var descuentoProd = db.factor_canal_producto.Include(m => m.producto);

            var jsonData = new
            {
                rows = (
                        from c in descuentoProd
                        select new
                        {
                            c.ID_PRODUCTO,
                            PRODUCTO = c.producto.NOMBRE,
                            CODIGO = c.producto.CODIGO,

                            FACTOR_POP = c.FACTOR_POP,
                            FACTOR_FRANQUICIA = c.FACTOR_FRANQUICIA,
                            FACTOR_PROYECTOS = c.FACTOR_PROYECTOS,
                            FACTOR_CADENAS = c.FACTOR_CADENAS,

                        
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetByIdLinea(int idLinea)
        {
            FactorCanalBLL bll = new FactorCanalBLL();

            var factorLinea = db.factor_canal_linea.FirstOrDefault(m => m.ID_LINEA_NEGOCIO == idLinea);
            var linea = db.linea_negocio.FirstOrDefault(m => m.ID == idLinea);

            var result = bll.LineaNegocioDTO(linea, factorLinea);



            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetByIdFamily(int idLinea, int idFamilia)
        {
            var c = db.factor_canal_familia.Where(m => m.ID_LINEA==idLinea && m.ID_FAMILIA == idFamilia).Include(m => m.familia_producto).FirstOrDefault();

            var jsnResult = new
            {
                ID = c.ID_FAMILIA,
                ID_LINEA = c.ID_LINEA,
                NOMBRE = c.familia_producto.NOMBRE,

                FACTOR_POP = c.FACTOR_POP,
                FACTOR_FRANQUICIA = c.FACTOR_FRANQUICIA,
                FACTOR_PROYECTOS = c.FACTOR_PROYECTOS,
                FACTOR_CADENAS = c.FACTOR_CADENAS,

                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetByIdProducto(int idProducto)
        {
            var c = db.factor_canal_producto.Where(m => m.ID_PRODUCTO == idProducto).Include(m => m.producto).FirstOrDefault();

            var jsnResult = new
            {
                ID = c.ID_PRODUCTO,
                NOMBRE = c.producto.NOMBRE,

                FACTOR_POP = c.FACTOR_POP,
                FACTOR_FRANQUICIA = c.FACTOR_FRANQUICIA,
                FACTOR_PROYECTOS = c.FACTOR_PROYECTOS,
                FACTOR_CADENAS = c.FACTOR_CADENAS,

                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarProductos(int idLinea, int idFamily, String nombre = "", string codigo = "")
        {
           

            var consulta = db.producto.Include(m=> m.ID_STATUS_SKU);

            if (idLinea > 0)
                consulta = consulta.Where(m => m.ID_LINEA_NEGOCIO == idLinea);

            if (idFamily > 0)
                consulta = consulta.Where(m => m.ID_FAMILIA_PRODUCTO == idFamily);

            if (nombre != "")
                consulta = consulta.Where(m => m.NOMBRE.Contains(nombre));
            
            if (codigo != "")
                consulta = consulta.Where(m => m.CODIGO.Contains(codigo));

            
            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            PRODUCTO = c.NOMBRE,
                            DESCRIPCION = c.DESCRIPCION,
                            CODIGO = c.CODIGO,
                          
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarFactores(FactorCanalModel model)
        {
            var rm = new ResponseModel();
            if (!ModelState.IsValid)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }

            switch (model.indicador)
            {
                case "producto":
                    {
                        Boolean nuevo = false;
                        var entity = db.factor_canal_producto.FirstOrDefault(m => m.ID_PRODUCTO == model.idProducto);

                        if (entity == null)
                        {
                            entity = new factor_canal_producto();
                            nuevo = true;

                            entity.ID_PRODUCTO = model.idProducto;
                        }
                        entity.FACTOR_POP = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorPOP);
                        entity.FACTOR_CADENAS = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorCadenas);
                        entity.FACTOR_FRANQUICIA = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorFranquicia);
                        entity.FACTOR_PROYECTOS = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorProyectos);

                        if (nuevo)
                            db.factor_canal_producto.Add(entity);
                    }
                    break;
                case "familia":
                    {
                        var entity = db.factor_canal_familia.FirstOrDefault(m => m.ID_LINEA == model.idLinea && m.ID_FAMILIA == model.idFamilia);

                        entity.FACTOR_POP = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorPOP);
                        entity.FACTOR_CADENAS = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorCadenas);
                        entity.FACTOR_FRANQUICIA = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorFranquicia);
                        entity.FACTOR_PROYECTOS = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorProyectos);
                       
                    }
                    break;
                case "addFamilia":
                    {
                        if (model.idAddFamilia == null)
                        {
                            rm.SetResponse(false, "Seleccione una Familia");
                            break;
                        }
                            

                        var entity = new factor_canal_familia();

                        entity.ID_FAMILIA = (int)model.idAddFamilia;
                        entity.ID_LINEA = model.idLinea;

                        entity.FACTOR_POP = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorPOP);
                        entity.FACTOR_CADENAS = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorCadenas);
                        entity.FACTOR_FRANQUICIA = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorFranquicia);
                        entity.FACTOR_PROYECTOS = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorProyectos);


                        db.factor_canal_familia.Add(entity);
                    }
                    break;
                case "linea":
                    {

                        Boolean nuevo = false;
                        var entity = db.factor_canal_linea.FirstOrDefault(m => m.ID_LINEA_NEGOCIO == model.idLinea);

                        if (entity == null)
                        {
                            entity = new factor_canal_linea();
                            nuevo = true;
                            entity.ID_LINEA_NEGOCIO = model.idLinea;
                        }
                        entity.FACTOR_POP = (float)ExtensionMethods.ConverToDecimalFormat(model.factorPOP);
                        entity.FACTOR_CADENAS = (float)ExtensionMethods.ConverToDecimalFormat(model.factorCadenas);
                        entity.FACTOR_FRANQUICIA = (float)ExtensionMethods.ConverToDecimalFormat(model.factorFranquicia);
                        entity.FACTOR_PROYECTOS = (float)ExtensionMethods.ConverToDecimalFormat(model.factorProyectos);

                        if (nuevo)
                            db.factor_canal_linea.Add(entity);
                    }
                    break;

            }

            
            if (db.SaveChanges() > 0)
            {
                rm.response = true;
                rm.message = "Sus datos se guardaron correctamente";
                rm.function = "reloadAll(true,'"+rm.message+"')";
            }



            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult ListaPreciosPop()
        {
            var consulta = db.lista_precios_pop_v;
            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID_PRODUCTO,
                            c.PRODUCTO,
                            c.CODIGO,
                            c.COSTO,
                            PRECIO_SUJERIDO = c.PRECIO_SUGERIDO,
                            c.PRECIO_POP,
                            c.FACTOR_POP,
                            c.INDICADOR
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
}
        public JsonResult ListaPreciosCadenas()
        {
            var consulta = db.lista_precios_cadenas_v;
            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID_PRODUCTO,
                            c.PRODUCTO,
                            c.CODIGO,
                            c.COSTO,
                            PRECIO_SUJERIDO = c.PRECIO_SUGERIDO,
                            c.FACTOR_CADENAS,
                            c.PRECIO_CADENAS,
                            c.INDICADOR
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListaPreciosfranquicias()
        {
            var consulta = db.lista_precios_franquicias_v;
            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID_PRODUCTO,
                            c.PRODUCTO,
                            c.CODIGO,
                            c.COSTO,
                            PRECIO_SUJERIDO = c.PRECIO_SUGERIDO,
                            c.FACTOR_FRANQUICIA,
                            c.PRECIO_FRANQUICIA,
                            c.INDICADOR
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListaPreciosProyectos()
        {
            var consulta = db.lista_precios_proyectos_v;
            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID_PRODUCTO,
                            c.PRODUCTO,
                            c.CODIGO,
                            c.COSTO,
                            PRECIO_SUJERIDO = c.PRECIO_SUGERIDO,
                            c.FACTOR_PROYECTOS,
                            c.PRECIO_PROYECTOS,
                            c.INDICADOR
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
      
        public ActionResult ActualizarPrecios(String canal)
        {
            var listaPrecios = db.lista_precios.ToList();
            String message = "Error al actualizar precios";
            switch (canal)
            {
                case "pop":
                    listaPrecios.ForEach(m => {
                        lista_precios_pop_v precioSujerido = db.lista_precios_pop_v.FirstOrDefault(x => x.ID_PRODUCTO == m.ID_PRODUCTO);
                        if (precioSujerido != null)
                            m.PRECIO_POP = (decimal)precioSujerido.PRECIO_SUGERIDO;
                    });
                    message = "Los precios de POP se actualizaron corectamente";
                    break;
                case "cadenas":
                    listaPrecios.ForEach(m => {
                        lista_precios_cadenas_v precioSujerido = db.lista_precios_cadenas_v.FirstOrDefault(x => x.ID_PRODUCTO == m.ID_PRODUCTO);
                        if (precioSujerido != null)
                            m.PRECIO_CADENAS = (decimal)precioSujerido.PRECIO_SUGERIDO;
                    });
                    message = "Los precios de CADENAS se actualizaron corectamente";

                    break;
                case "franquicias":
                    listaPrecios.ForEach(m => {
                        lista_precios_franquicias_v precioSujerido = db.lista_precios_franquicias_v.FirstOrDefault(x => x.ID_PRODUCTO == m.ID_PRODUCTO);
                        if (precioSujerido != null)
                            m.PRECIO_FRANQUICIA = (decimal)precioSujerido.PRECIO_SUGERIDO;
                    });
                    message = "Los precios de FRANQUICIAS se actualizaron corectamente";

                    break;
                case "proyectos":
                    listaPrecios.ForEach(m => {
                        lista_precios_proyectos_v precioSujerido = db.lista_precios_proyectos_v.FirstOrDefault(x => x.ID_PRODUCTO == m.ID_PRODUCTO);
                        if (precioSujerido != null)
                            m.PRECIO_PROYECTOS = (decimal)precioSujerido.PRECIO_SUGERIDO;
                    });
                    message = "Los precios de PROYECTOS se actualizaron corectamente";

                    break;
            }

            if (db.SaveChanges() > 0)
            {
                TempData["message"] = "success," + message;
            }
            else
            {
                TempData["message"] = "danger, Ocurio un error al actualizar precios";

            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public JsonResult GetPrecioPop(int idProducto)
        {
            var c = db.lista_precios_pop_v.FirstOrDefault(m => m.ID_PRODUCTO == idProducto);

            var jsnResult = new
            {
                ID = c.ID_PRODUCTO,
                PRODUCTO = c.PRODUCTO,
                CODIGO = c.CODIGO,
                c.PRECIO_SUGERIDO,
                PRECIO = c.PRECIO_POP,

                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPrecioCadenas(int idProducto)
        {
            var c = db.lista_precios_cadenas_v.FirstOrDefault(m => m.ID_PRODUCTO == idProducto);

            var jsnResult = new
            {
                ID = c.ID_PRODUCTO,
                PRODUCTO = c.PRODUCTO,
                CODIGO = c.CODIGO,
                c.PRECIO_SUGERIDO,
                PRECIO = c.PRECIO_CADENAS,

                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPrecioFranquicias(int idProducto)
        {
            var c = db.lista_precios_franquicias_v.FirstOrDefault(m => m.ID_PRODUCTO == idProducto);

            var jsnResult = new
            {
                ID = c.ID_PRODUCTO,
                PRODUCTO = c.PRODUCTO,
                CODIGO = c.CODIGO,
                c.PRECIO_SUGERIDO,
                PRECIO = c.PRECIO_FRANQUICIA,

                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPrecioProyectos(int idProducto)
        {
            var c = db.lista_precios_proyectos_v.FirstOrDefault(m => m.ID_PRODUCTO == idProducto);

            var jsnResult = new
            {
                ID = c.ID_PRODUCTO,
                PRODUCTO = c.PRODUCTO,
                CODIGO = c.CODIGO,
                c.PRECIO_SUGERIDO,
                PRECIO = c.PRECIO_PROYECTOS,

                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFamiliasByLinea(int idLinea)
        {
            HashSet<int> idDescuentoByFamily = new HashSet<int>(from x in db.factor_canal_familia.Where(m => m.ID_LINEA == idLinea).Select(x => (int)x.ID_FAMILIA) select x);
            var consulta = db.familia_producto.Where(m => m.ACTIVO);

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE
                        }).Where(x => !idDescuentoByFamily.Contains(x.ID)).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult CambiarPrecio(int id_producto, string precio,string canal)
        {
            ResponseModel rm = new ResponseModel();

            var entity = db.lista_precios.FirstOrDefault(m => m.ID_PRODUCTO == id_producto);
            decimal cantidad = ExtensionMethods.ConverToDecimalFormat(precio);

            switch (canal)
            {
                case "pop":
                    entity.PRECIO_POP = cantidad;
                    break;
                case "cadenas":
                    entity.PRECIO_CADENAS = cantidad;
                    break;
                case "franquicias":
                    entity.PRECIO_FRANQUICIA = cantidad;
                    break;
                case "proyectos":
                    entity.PRECIO_PROYECTOS = cantidad;
                    break;
            }


            if (db.SaveChanges() > 0)
            {
                rm.response = true;
                rm.message = "Sus datos se guardaron correctamente";
                rm.function = "reloadAll(true,'" + rm.message + "')";

            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteFactor(int idLinea, int idFamilia, int idProducto, string factor)
        {
            var rm = new ResponseModel();


            switch (factor)
            {
                case "producto":
                    {
                        var entity = db.factor_canal_producto.FirstOrDefault(m => m.ID_PRODUCTO == idProducto);
                        if (entity != null)
                            db.factor_canal_producto.Remove(entity);
                    }
                    break;
                case "familia":
                    {
                        var entity = db.factor_canal_familia.FirstOrDefault(m => m.ID_LINEA == idLinea && m.ID_FAMILIA == idFamilia);
                        if (entity != null)
                            db.factor_canal_familia.Remove(entity);
                    }
                    break;
            }
            if (db.SaveChanges() > 0)
            {
                rm.response = true;
                rm.message = "Sus datos se guardaron correctamente";
                rm.function = "reloadAll(true,'" + rm.message + "')";

            }
            return Json(rm, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetLineaNegocio()
        {
            var consulta = db.linea_negocio;

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFamilia()
        {
            var consulta = db.familia_producto;

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }


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