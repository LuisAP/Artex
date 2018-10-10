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
    public class UnidadMedidaController : Controller
    {
        private const string ABSOLUTE_PATH = "~/Views/Catalogos/UnidadMedida.cshtml";
        private ArtexConnection db = new ArtexConnection();

        public ActionResult Index()
        {
           UnidadMedidaModel model = new UnidadMedidaModel();
            model.permisos = PermisosModulo.ObtenerPermisos(Modulo.UNIDAD_MEDIDA);
            if (model.permisos == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult GetAlls()
        {

            List<unidad_medida> consulta = UnidadMedidaDAO.GetAlls(db);

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
            UnidadMedidaDAO dao = new UnidadMedidaDAO();
            unidad_medida c = dao.GetById(id);

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
        public JsonResult Guardar(UnidadMedidaModel model)
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
                UnidadMedidaDAO dao = new UnidadMedidaDAO();
                var entity = dao.GetById(model.Id, db);

                if (entity == null)
                {
                    entity = new unidad_medida();
                    entity.NOMBRE = model.Nombre;
                    entity.DESCRIPCION = model.Descripcion;
                    entity.ACTIVO = model.Activo;
                    db.unidad_medida.Add(entity);
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

            }


            return Json(rm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Delete(int id)
        {
            var rm = new ResponseModel();
            var entity = db.unidad_medida.Find(id);
            if (entity != null)
            {
                entity.ACTIVO = false;

                if (db.SaveChanges() > 0 )
                {
                    rm.response = true;
                    rm.message = "El registro  se elimino correctamente";
                } else if(db.Entry(entity).State == EntityState.Unchanged) {
                    rm.response = false;
                    rm.message = "El registro ya se encuentra eliminado";
                }

            }

            
            return Json(rm, JsonRequestBehavior.AllowGet);

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