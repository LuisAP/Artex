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

namespace Artex.Controllers.Catalogos
{
    [Authorize]
    public class EstatusSkuController : Controller
    {

        private ArtexConnection db = new ArtexConnection();

        private const string ABSOLUTE_PATH = "~/Views/Catalogos/EstatusSku.cshtml";
        public ActionResult Index()
        {
            EstatusSkuModel model = new EstatusSkuModel();
             model.permisos = PermisosModulo.ObtenerPermisos(Modulo.ESTATUS_SKU);

            if (model.permisos == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult GetAlls()
        {
            var consulta = db.estatus_sku;

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE,
                            CODIGO = c.CODIGO,
                            ACTIVO = c.ACTIVO,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetById(int id)
        {
            var c = db.estatus_sku.Find(id);

            var jsnResult = new
            {
                ID = c.ID,
                NOMBRE = c.NOMBRE,
                CODIGO = c.CODIGO,
                ACTIVO = c.ACTIVO,
                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult Guardar(EstatusSkuModel model)
        {
            var rm = new ResponseModel();
            if (!ModelState.IsValid)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }


            var entity = db.estatus_sku.Find(model.Id);

            if (entity == null)
            {
                entity = new estatus_sku();
                entity.NOMBRE = model.Nombre;
                entity.CODIGO = model.Codigo;
                entity.ACTIVO = model.Activo;
                db.estatus_sku.Add(entity);
            }
            else
            {
                entity.NOMBRE = model.Nombre;
                entity.CODIGO = model.Codigo;
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

            var entity = db.estatus_sku.Find(id);
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
                Success = success,
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