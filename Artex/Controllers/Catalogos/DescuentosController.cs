using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artex.Models.DAL.DAO;
using Artex.Models.DAL.DAO.Views;
using Artex.DB;
using Artex.Models.ViewModels.Catalogos;
using Artex.Models.ViewModels.Cotizacion;
using Artex.Util;
using Artex.Util.Sistema;
using System.Data.Entity;

namespace Artex.Controllers.Catalogos
{
    public class DescuentosController : Controller
    {/*
        private const string ABSOLUTE_PATH = "~/Views/Catalogos/Descuentos.cshtml";
        private ArtexConnection db = new ArtexConnection();
        // GET: Gastos
        public ActionResult Index()
        {
            DescuentosModel model = new DescuentosModel();
            model.permisos = PermisosModulo.ObtenerPermisos(Modulo.TIENDAS);

            if (model.permisos == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }

            return View(ABSOLUTE_PATH, model);
        }

        [HttpPost]
        public JsonResult Guardar(DescuentosModel model)
        {
            var rm = new ResponseModel();
            if (!ModelState.IsValid)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }
            using (ArtexConnection db = new ArtexConnection())
            {
                var entity = db.descuentos.Where(e => e.ID == model.Id).FirstOrDefault();
                
                if (entity == null)
                {
                    var entityD = new descuentos();
                    entityD.PRODUCT_ID = model.Producto;
                    entityD.FAMILIA_ID = model.Familia;
                    entityD.LINEA_DE_NEGOCIO_ID = model.Linea;
                    
                    

                    db.descuentos.Add(entityD);
                }
                else
                {
                   
                }

                if (db.SaveChanges() > 0 || db.Entry(entity).State == EntityState.Unchanged)
                {
                    rm.response = true;
                    rm.message = "Sus datos se guardaron correctamente";
                    rm.function = "reload(true,'" + rm.message + "')";
                }
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProducto()
        {
            var consulta = db.producto.OrderBy(e => e.NOMBRE).ToList();

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
            var consulta = db.familia_producto.OrderBy(e => e.NOMBRE).ToList();

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

        public ActionResult GetLinea()
        {
            var consulta = db.linea_negocio.OrderBy(e => e.NOMBRE).ToList();

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

        public ActionResult GetAlls() {
            var consulta = db.descuentos.OrderBy(e => e.ID);

            var jsonData = new {

                rows = (
                    from c in consulta
                    select new {
                        TIPO = c.ID,
                        PRODUCTO = c.PRODUCT_ID,
                        FAMILIA = c.FAMILIA_ID,
                        LINEA = c.LINEA_DE_NEGOCIO_ID,
                        NOMBRE = c.producto.NOMBRE+c.familia_producto.NOMBRE+c.linea_negocio.NOMBRE,                        
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
        }*/
    }
}