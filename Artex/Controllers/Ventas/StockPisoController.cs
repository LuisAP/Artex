using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artex.DB;
using Artex.Models.ViewModels.Catalogos;
using Artex.Models.DAL.DAO;
using Artex.Util;
using Artex.Util.Sistema;
using System.Data.Entity;

namespace Artex.Controllers.Ventas
{
    [Authorize]
    public class StockPisoController : Controller
    {
        private ArtexConnection db = new ArtexConnection();

        private const string ABSOLUTE_PATH = "~/Views/Ventas/Piso/StockPiso.cshtml";

        public ActionResult Index()
        {
            BancoModel model = new BancoModel();
            model.permisos = PermisosModulo.ObtenerPermisos(Modulo.BANCOS);
            if (model == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }

        public ActionResult GetAlls()
        {

            var consulta = db.stock_tienda_v.Where(m=> m.CANTIDAD>0);

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID_PRODUCTO,
                            PRODUCTO = c.PRODUCTO,
                            DESCRIPCION = c.DESCRIPCION,
                            CODIGO = c.CODIGO_CONFIGURADO,
                            CANTIDAD = c.CANTIDAD,
                            DISPONIBLES = c.DISPONIBLES,
                            FOTO = c.FOTO,
                            ID_TIENDA = c.ID_TIENDA,
                            TIENDA = c.TIENDA,
                            PROGRAMA = c.UNIDAD_NEGOCIO,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCatalogoProductos()
        {
            int idTienda = 1;
            Boolean mayoreo = false;

            var consulta = db.producto_v.Where(m => m.ID_STATUS_SKU==2);
            /*
            if (nombre != "")
            {
                consulta = consulta.Where(m => m.NOMBRE.Contains(nombre));
            }
            if (codigo != "")
            {
                consulta = consulta.Where(m => m.CODIGO.Contains(codigo));

            }*/

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID_PRODUCTO,
                            PRODUCTO = c.NOMBRE,
                            DESCRIPCION = c.DESCRIPCION,
                            CODIGO = c.CODIGO,
                            ES_PERSONALIZABLE=c.ES_PERSONALIZABLE,
                            //CANTIDAD = c,
                            // PRECIO = mayoreo ? c.PRECIO_MAYOREO : c.PRECIO_RETAIL,
                            // DESCUENTO = mayoreo ? c.DESCUENTO_CADENAS : c.DESCUENTO_RETAIL,
                            FOTO = "/Content/img/silla-medina.jpg",
                           // PROGRAMA = uNegocio == 1 ? "fabricacion" : "externo",
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTiendas()
        {
            var consulta = db.tienda.Where(m => m.ACTIVO==true);

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
       /* public ActionResult GetConfigurar(int idProd, String codigo)
        {
            producto entityProd = db.producto.Where(m=> m.ID==idProd).Include(m=> m.piezas_configurables).FirstOrDefault();
           // var entityPiezas = entityProd.piezas_configurables;



            return PartialView("~/Views/Ventas/Piso/Partials/_SeleccionarAtributos.cshtml", entityProd);
        }*/
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