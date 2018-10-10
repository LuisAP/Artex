using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artex.Models.DAL.DAO;
using Artex.Models.DAL.DAO.Views;
using Artex.DB;
using Artex.Models.ViewModels.Catalogos;
using Artex.Models.ViewModels.Fabricacion;
using Artex.Util;
using Artex.Util.Sistema;
using System.Data.Entity;

namespace Artex.Controllers.Fabricacion
{
    public class SubprocesosController : Controller
    {
        private const string ABSOLUTE_PATH = "~/Views/Fabricacion/Subprocesos.cshtml";
        private ArtexConnection db = new ArtexConnection();
        // GET: Procesos
        public ActionResult Index()
        {
            SubprocesosModel model = new SubprocesosModel();
            model.permisos = PermisosModulo.ObtenerPermisos(Modulo.TIENDAS);

            if (model.permisos == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }

            return View(ABSOLUTE_PATH, model);
        }

        public ActionResult Getalls()
        {
            var consulta = db.subprocesos_de_fabricacion.OrderBy(x => x.CLAVE);

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE,
                            DESCRIPCION = c.DESCRIPCION,
                            CLAVE = c.CLAVE,
                            PROCESO = c.procesos_de_fabricacion.NOMBRE,
                            ACTIVO = c.ACTIVO

                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(int id = 0)
        {
            var consulta = db.subprocesos_de_fabricacion.Where(x => x.ID == id).FirstOrDefault();

            var JsnResult = new
            {
                ID = consulta.ID,
                NOMBRE = consulta.NOMBRE,
                DESCRIPCION = consulta.DESCRIPCION,
                CLAVE = consulta.CLAVE,
                PROCESO = consulta.ID_PROCESO__DE_FABRICACION,
                ACTIVO = consulta.ACTIVO,
                Success = true
            };

            return Json(JsnResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var rm = new ResponseModel();

            var consulta = db.subprocesos_de_fabricacion.Where(x => x.ID == id).FirstOrDefault();
            if (consulta != null)
            {
                consulta.ACTIVO = false;
            }
            if (db.SaveChanges() > 0)
            {
                rm.message = "El registro  se elimino correctamente";
                rm.response = true;
            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(SubprocesosModel model)
        {
            var rm = new ResponseModel();
            var entity = db.subprocesos_de_fabricacion.Where(x => x.ID == model.Id).FirstOrDefault();

            if (entity == null)
            {
                entity = new subprocesos_de_fabricacion();
                entity.NOMBRE = model.Nombre;
                entity.DESCRIPCION = model.Descripcion;
                entity.ACTIVO = model.Activo;
                entity.ID_PROCESO__DE_FABRICACION = model.Proceso;
                entity.CLAVE = model.Clave;
                db.subprocesos_de_fabricacion.Add(entity);
            }
            else
            {
                entity.NOMBRE = model.Nombre;
                entity.DESCRIPCION = model.Descripcion;
                entity.ACTIVO = model.Activo;
                entity.ID_PROCESO__DE_FABRICACION = model.Proceso;
                entity.CLAVE = model.Clave;
            }
            if (db.SaveChanges() > 0)
            {
                rm.result = true;
                rm.response = true;
                rm.message = "Sus datos se guardaron correctamente";
                rm.function = "reload(true,'" + rm.message + "')";
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Getproceso() {
            var consulta = db.procesos_de_fabricacion.OrderBy(x => x.NOMBRE);
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