using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artex.Util.Sistema;
using Artex.Models.ViewModels.Catalogos;
using Artex.DB;
using Artex.Models.DAL.DAO;
using Artex.Util;
using System.Data.Entity;

namespace Artex.Controllers.Catalogos
{
    public class LineaProductoController : Controller
    {
        private ArtexConnection db = new ArtexConnection();


        private const string ABSOLUTE_PATH = "~/Views/Catalogos/LineaProducto.cshtml";

        public ActionResult Index()
        {
            LineaProductoModel model = new LineaProductoModel();
            model.permisos = PermisosModulo.ObtenerPermisos(Modulo.LINEA_PRODUCTO,db);
            if (model == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult GetAlls()
        {

            var consulta = db.linea_producto;
            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE,
                            DESCRIPCION = c.DESCRIPCION,
                            ACTIVO = c.ACTIVO,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetById(int id)
        {
            var c = db.linea_producto.Find(id);

            var jsnResult = new
            {
                ID = c.ID,
                NOMBRE = c.NOMBRE,
                DESCRIPCION = c.DESCRIPCION,
                ACTIVO = c.ACTIVO,
                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult Guardar(LineaProductoModel model)
        {
            var rm = new ResponseModel();
            if (!ModelState.IsValid)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }


            
                var entity = db.linea_producto.Find(model.Id);

                if (entity == null)
                {
                    entity = new linea_producto();
                    entity.NOMBRE = model.Nombre;
                    entity.DESCRIPCION = model.Descripcion;
                    entity.ACTIVO = model.Activo;
                    db.linea_producto.Add(entity);
                }
                else
                {
                    entity.NOMBRE = model.Nombre;
                    entity.DESCRIPCION = model.Descripcion;
                    entity.ACTIVO = model.Activo;
                }

            if (db.SaveChanges() > 0 || db.Entry(entity).State == EntityState.Unchanged)
            {
                    rm.response = true;
                    rm.message = "Sus datos se guardaron correctamente";
                    rm.function = "reload(true,'" + rm.message + "')";
                }

            


            return Json(rm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Delete(int id)
        {
            bool success = false;
            string msj = "Hubo un problema verifique su conexion e intente de nuevo.";

            var entity = db.linea_producto.Find(id);
            if (entity != null)
            {
                entity.ACTIVO = false;

                if (db.SaveChanges() > 0 || db.Entry(entity).State == EntityState.Unchanged)
                {
                    success = true;
                    msj = "El registro  se elimino correctamente";
                }

            }

            var result = new
            {
                response = success,
                msj = msj
            };
            return Json(result, JsonRequestBehavior.AllowGet);

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