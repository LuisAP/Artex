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
    public class UnidadNegocioController : Controller
    {
        private ArtexConnection db = new ArtexConnection();


        private const string ABSOLUTE_PATH = "~/Views/Catalogos/UnidadNegocio.cshtml";

        public ActionResult Index()
        {
            UnidadNegocioModel model = new UnidadNegocioModel();
            model.permisos = PermisosModulo.ObtenerPermisos(Modulo.UNIDAD_NEGOCIO);
            if (model.permisos == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult GetAlls()
        {
            
            var consulta = db.unidad_de_negocio.Include(c => c.empresa);

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE,
                            DESCRIPCION = c.DESCRIPCION,
                            EMPRESA=c.empresa.NOMBRE,
                              ACTIVO = c.ACTIVO,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetById(int id)
        {
         
            var c = db.unidad_de_negocio.Find(id);

            var jsnResult = new
            {
                ID = c.ID,
                NOMBRE = c.NOMBRE,
                DESCRIPCION = c.DESCRIPCION,
                ID_EMPRESA = c.ID_EMPRESA,
                ACTIVO = c.ACTIVO,
                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult Guardar(UnidadNegocioModel model)
        {
            var rm = new ResponseModel();
            if (!ModelState.IsValid)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }
          
                var entity = db.unidad_de_negocio.Find(model.Id);

                if (entity == null)
                {
                    entity = new unidad_de_negocio();
                    entity.NOMBRE = model.Nombre;
                    entity.DESCRIPCION = model.Descripcion;
                    entity.ID_EMPRESA = model.Empresa;
                    db.unidad_de_negocio.Add(entity);
                entity.ACTIVO = model.Activo;
            }
                else
                {
                    entity.NOMBRE = model.Nombre;
                    entity.DESCRIPCION = model.Descripcion;
                    entity.ID_EMPRESA = model.Empresa;
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

             
        public ActionResult GetEmpresas()
        {
            var consulta = db.empresa.Where(m=> m.ACTIVO);

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
        public JsonResult Delete(int id)
        {
            var rm = new ResponseModel();

            var entity = db.unidad_de_negocio.Find(id);
            if (entity != null)
            {
                entity.ACTIVO = false;

                if (db.SaveChanges() > 0)
                {
                    rm.result = true;
                    rm.response = true;
                    rm.message = "El registro  se elimino correctamente";
                } else if (db.Entry(entity).State == EntityState.Unchanged) {
                    rm.result = false;
                    rm.message = "El registro  ya esta deasctivado";
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