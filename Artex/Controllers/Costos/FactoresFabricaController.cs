using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artex.Models.ViewModels.Catalogos;
using Artex.DB;
using Artex.Models.ViewModels.Costos;
using Artex.Util;
using Artex.Util.Sistema;
using System.Data.Entity;
using Artex.Models.BLL.Costos;

namespace Artex.Controllers.Catalogos
{
    public class FactoresFabricaController : Controller
    {
        private ArtexConnection db = new ArtexConnection();

        private const string ABSOLUTE_PATH = "~/Views/Costos/FactoresFabrica/Factores.cshtml";

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


            FactorFabricaBLL bll = new FactorFabricaBLL();
            var factorLinea = db.factor_fabrica_linea.ToList();
            var lineas = db.linea_negocio.Where(m => m.ACTIVO).ToList();

            var lista_linea = bll.ListLineaNegocioDTO(lineas, factorLinea);

            return Json(lista_linea, JsonRequestBehavior.AllowGet);

          

        }


        public ActionResult FactorByFamily(int idLinea)
        {

            var descuentoFamily = db.factor_fabrica_familia.Where(m => m.ID_LINEA == idLinea).Include(m => m.familia_producto);
            

            var jsonData = new
            {
                rows = (
                        from c in descuentoFamily
                        select new
                        {
                            c.ID_LINEA,
                            c.ID_FAMILIA,
                            FAMILIA = c.familia_producto.NOMBRE,
                            FACTOR_FABRICA = c.FACTOR_FABRICA,

                            DESCUENTO_POP = c.DESCUENTO_POP,
                            DESCUENTO_FRANQUICIA = c.DESCUENTO_FRANQUICIA,
                            DESCUENTO_PROYECTOS = c.DESCUENTO_PROYECTOS,
                            DESCUENTO_CADENAS = c.DESCUENTO_CADENAS,

                         
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductosConDescuento()
        {
            var descuentoProd = db.factor_fabrica_producto.Include(m => m.producto);

            var jsonData = new
            {
                rows = (
                        from c in descuentoProd
                        select new
                        {
                            c.ID_PRODUCTO,
                            PRODUCTO = c.producto.NOMBRE,
                            CODIGO = c.producto.CODIGO,
                            FACTOR_FABRICA = c.FACTOR_FABRICA,

                            DESCUENTO_POP = c.DESCUENTO_POP,
                            DESCUENTO_FRANQUICIA = c.DESCUENTO_FRANQUICIA,
                            DESCUENTO_PROYECTOS = c.DESCUENTO_PROYECTOS,
                            DESCUENTO_CADENAS = c.DESCUENTO_CADENAS,

                        
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetByIdLinea(int idLinea)
        {
            var c = db.factor_fabrica_linea.Where(m => m.ID_LINEA_NEGOCIO == idLinea).Include(m => m.linea_negocio).FirstOrDefault();

            var jsnResult = new
            {
                ID = c.ID_LINEA_NEGOCIO,
                NOMBRE = c.linea_negocio.NOMBRE,

                FACTOR_FABRICA = c.FACTOR_FABRICA,

                DESCUENTO_POP = c.DESCUENTO_POP,
                DESCUENTO_FRANQUICIA = c.DESCUENTO_FRANQUICIA,
                DESCUENTO_PROYECTOS = c.DESCUENTO_PROYECTOS,
                DESCUENTO_CADENAS = c.DESCUENTO_CADENAS,

                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetByIdFamily(int idLinea, int idFamilia)
        {
            var c = db.factor_fabrica_familia.Where(m => m.ID_LINEA==idLinea && m.ID_FAMILIA == idFamilia).Include(m => m.familia_producto).FirstOrDefault();

            var jsnResult = new
            {
                ID = c.ID_FAMILIA,
                ID_LINEA = c.ID_LINEA,
                NOMBRE = c.familia_producto.NOMBRE,

                FACTOR_FABRICA = c.FACTOR_FABRICA,

                DESCUENTO_POP = c.DESCUENTO_POP,
                DESCUENTO_FRANQUICIA = c.DESCUENTO_FRANQUICIA,
                DESCUENTO_PROYECTOS = c.DESCUENTO_PROYECTOS,
                DESCUENTO_CADENAS = c.DESCUENTO_CADENAS,

                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetByIdProducto(int idProducto)
        {
            var c = db.factor_fabrica_producto.Where(m => m.ID_PRODUCTO == idProducto).Include(m => m.producto).FirstOrDefault();

            var jsnResult = new
            {
                ID = c.ID_PRODUCTO,
                NOMBRE = c.producto.NOMBRE,

                FACTOR_FABRICA = c.FACTOR_FABRICA,

                DESCUENTO_POP = c.DESCUENTO_POP,
                DESCUENTO_FRANQUICIA = c.DESCUENTO_FRANQUICIA,
                DESCUENTO_PROYECTOS = c.DESCUENTO_PROYECTOS,
                DESCUENTO_CADENAS = c.DESCUENTO_CADENAS,

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
        public JsonResult GuardarFactorFabrica(FactorFabricaModel model)
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
                        var entity = db.factor_fabrica_producto.FirstOrDefault(m => m.ID_PRODUCTO == model.idProducto);

                        if (entity == null)
                        {
                            entity = new factor_fabrica_producto();
                            nuevo = true;

                            entity.ID_PRODUCTO = model.idProducto;
                        }
                        entity.FACTOR_FABRICA = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorFabrica);
                        entity.DESCUENTO_POP = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorPOP);
                        entity.DESCUENTO_CADENAS = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorCadenas);
                        entity.DESCUENTO_FRANQUICIA = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorFranquicia);
                        entity.DESCUENTO_PROYECTOS = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorProyectos);

                        if (nuevo)
                            db.factor_fabrica_producto.Add(entity);
                    }
                    break;
                case "familia":
                    {
                        var entity = db.factor_fabrica_familia.FirstOrDefault(m => m.ID_LINEA == model.idLinea && m.ID_FAMILIA == model.idFamilia);

                        entity.FACTOR_FABRICA = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorFabrica);
                        entity.DESCUENTO_POP = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorPOP);
                        entity.DESCUENTO_CADENAS = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorCadenas);
                        entity.DESCUENTO_FRANQUICIA = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorFranquicia);
                        entity.DESCUENTO_PROYECTOS = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorProyectos);

                     
                    }
                    break;
                case "addFamilia":
                    {
                        if (model.idAddFamilia == null)
                        {
                            rm.SetResponse(false, "Seleccione una Familia");
                            break;
                        }
                            

                        var entity = new factor_fabrica_familia();

                        entity.ID_FAMILIA = (int)model.idAddFamilia;
                        entity.ID_LINEA = model.idLinea;

                        entity.FACTOR_FABRICA = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorFabrica);
                        entity.DESCUENTO_POP = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorPOP);
                        entity.DESCUENTO_CADENAS = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorCadenas);
                        entity.DESCUENTO_FRANQUICIA = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorFranquicia);
                        entity.DESCUENTO_PROYECTOS = (float?)ExtensionMethods.ToDecimalFormatNullable(model.factorProyectos);


                        db.factor_fabrica_familia.Add(entity);
                    }
                    break;
                case "linea":
                    {

                        Boolean nuevo = false;
                        var entity = db.factor_fabrica_linea.FirstOrDefault(m => m.ID_LINEA_NEGOCIO == model.idLinea);

                        if (entity == null)
                        {
                            entity = new factor_fabrica_linea();
                            nuevo = true;
                            entity.ID_LINEA_NEGOCIO = model.idLinea;
                        }
                        entity.FACTOR_FABRICA = (float)ExtensionMethods.ConverToDecimalFormat(model.factorFabrica);
                        entity.DESCUENTO_POP = (float)ExtensionMethods.ConverToDecimalFormat(model.factorPOP);
                        entity.DESCUENTO_CADENAS = (float)ExtensionMethods.ConverToDecimalFormat(model.factorCadenas);
                        entity.DESCUENTO_FRANQUICIA = (float)ExtensionMethods.ConverToDecimalFormat(model.factorFranquicia);
                        entity.DESCUENTO_PROYECTOS = (float)ExtensionMethods.ConverToDecimalFormat(model.factorProyectos);

                        if (nuevo)
                            db.factor_fabrica_linea.Add(entity);
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

        
        public JsonResult ListaPrecios()
        {
            var consulta = db.lista_precios_fabrica_v;
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
                            PRECIO_SUJERIDO = c.PRECIO_SUJERIDO_FABRICA,
                            c.PRECIO_FABRICA,
                            c.PRECIO_POP,
                            c.PRECIO_CADENAS,
                            c.PRECIO_FRANQUICIA,
                            c.PRECIO_PROYECTOS,
                            c.INDICADOR
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
}

        public ActionResult ActualizarPrecios()
        {
            var listaPrecios = db.lista_precios.ToList();

            listaPrecios.ForEach(m => {
                lista_precios_fabrica_v precioSujerido = db.lista_precios_fabrica_v.FirstOrDefault(x => x.ID_PRODUCTO == m.ID_PRODUCTO);
                if(precioSujerido!=null)
                    m.PRECIO_FABRICA = (decimal)precioSujerido.PRECIO_SUJERIDO_FABRICA;
            });
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public JsonResult GetPrecioById(int idProducto)
        {
            var c = db.lista_precios_fabrica_v.FirstOrDefault(m => m.ID_PRODUCTO == idProducto);

            var jsnResult = new
            {
                ID = c.ID_PRODUCTO,
                PRODUCTO = c.PRODUCTO,
                CODIGO = c.CODIGO,
                c.PRECIO_SUJERIDO_FABRICA,
                c.PRECIO_FABRICA,

                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFamiliasByLinea(int idLinea)
        {
            HashSet<int> idDescuentoByFamily = new HashSet<int>(from x in db.factor_fabrica_familia.Where(m => m.ID_LINEA == idLinea).Select(x => (int)x.ID_FAMILIA) select x);
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
        public JsonResult CambiarPrecio(int id_producto, string precio)
        {
            ResponseModel rm = new ResponseModel();

            var entity = db.lista_precios.FirstOrDefault(m => m.ID_PRODUCTO == id_producto);
            decimal cantidad = ExtensionMethods.ConverToDecimalFormat(precio);

            entity.PRECIO_FABRICA = cantidad;


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
                        var entity = db.factor_fabrica_producto.FirstOrDefault(m => m.ID_PRODUCTO == idProducto);
                        if (entity != null)
                            db.factor_fabrica_producto.Remove(entity);
                    }
                    break;
                case "familia":
                    {
                        var entity = db.factor_fabrica_familia.FirstOrDefault(m => m.ID_LINEA == idLinea && m.ID_FAMILIA == idFamilia);
                        if (entity != null)
                            db.factor_fabrica_familia.Remove(entity);
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