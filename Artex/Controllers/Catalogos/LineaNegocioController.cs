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
    public class LineaNegocioController : Controller
    {
        private ArtexConnection db = new ArtexConnection();

        private const string ABSOLUTE_PATH = "~/Views/Catalogos/LineaNegocio.cshtml";

        public ActionResult Index()
        {
            LineaNegocioModel model = new LineaNegocioModel();
            model.permisos = PermisosModulo.ObtenerPermisos(Modulo.LINEA_NEGOCIO);
            if (model.permisos == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult GetAlls()
        {

            //  LineaNegocioDAO dao = new LineaNegocioDAO();
            // List<linea_negocio> consulta2 = dao.GetAlls(db);

            var consulta = db.linea_negocio.Include(m => m.unidad_de_negocio);
            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE,
                            DESCRIPCION = c.DESCRIPCION,
                            UNIDAD_NEGOCIO=c.unidad_de_negocio.NOMBRE,
                            ACTIVO = c.ACTIVO,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetById(int id)
        {
            LineaNegocioDAO dao = new LineaNegocioDAO();
            linea_negocio c = dao.GetById(id,db);

            var jsnResult = new
            {
                ID = c.ID,
                NOMBRE = c.NOMBRE,
                DESCRIPCION = c.DESCRIPCION,
                ID_UNIDAD_NEGOCIO = c.ID_UNIDAD_NEGOCIO,
                ACTIVO = c.ACTIVO,
                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult Guardar(LineaNegocioModel model)
        {
            var rm = new ResponseModel();
            if (!ModelState.IsValid)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }


              LineaNegocioDAO dao = new LineaNegocioDAO();
                var entity = dao.GetById(model.Id, db);

                if (entity == null)
                {
                    entity = new linea_negocio();
                    entity.NOMBRE = model.Nombre;
                    entity.DESCRIPCION = model.Descripcion;
                    entity.ID_UNIDAD_NEGOCIO = model.UnidadNegocio;
                    entity.ACTIVO = model.Activo;
                    db.linea_negocio.Add(entity);
                }
                else
                {
                    entity.NOMBRE = model.Nombre;
                    entity.DESCRIPCION = model.Descripcion;
                    entity.ID_UNIDAD_NEGOCIO = model.UnidadNegocio;
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
            var rm = new ResponseModel();


            LineaNegocioDAO dao = new LineaNegocioDAO();
            rm.response = dao.DeleteById(id);

            if (rm.response)
            {
                rm.message = "El registro  se elimino correctamente";

            }

            return Json(rm, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetUnidadNegocio()
        {
            UnidadNegocioDAO dao = new UnidadNegocioDAO();

            List<unidad_de_negocio> consulta = dao.GetAlls();

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