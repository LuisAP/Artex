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
    public class ColorMezclaController : Controller
    {

        private ArtexConnection db = new ArtexConnection();

        private const string ABSOLUTE_PATH = "~/Views/Catalogos/ColorMezcla/ListaColores.cshtml";
        private const string CREATE_UPDATE_ABSOLUTE_PATH = "~/Views/Catalogos/ColorMezcla/EditarColor.cshtml";

        public ActionResult Index()
        {
            PermisosModel model = PermisosModulo.ObtenerPermisos(Modulo.COLORES);
            if (model == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult GetAlls()
        {

            var consulta = db.color_mezcla.Where(m => m.ACTIVO);
            var jsonData = new
            {
                rows = (
                      from c in consulta
                      select new
                      {
                          ID = c.ID,
                          NOMBRE = c.NOMBRE,
                          DESCRIPCION = c.DESCRIPCION,
                          // PRECIO = c.PRECIO,
                          // TIPO = c.tipo_atributo.NOMBRE,
                          ACTIVO = c.ACTIVO,
                      }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ver(int id)
        {
            ColorMezclaModel model = new ColorMezclaModel();

            var consulta = db.color_mezcla.Find(id);

            if (consulta != null)
            {
                model.Id = consulta.ID;
                model.Nombre = consulta.NOMBRE;
                model.Descripcion = consulta.DESCRIPCION;
                model.Activo = consulta.ACTIVO;

                ViewBag.Editar = false;
                return View(CREATE_UPDATE_ABSOLUTE_PATH, model);
            }

            TempData["message"] = "danger, No fue posible cargar sus datos";
            return RedirectToAction("Index");
        }


        public ActionResult Editar(int id = -1)
        {
            ColorMezclaModel model = new ColorMezclaModel();

            var consulta = db.color_mezcla.Find(id);

            if (consulta != null)
            {
                model.Id = consulta.ID;
                model.Nombre = consulta.NOMBRE;
                model.Descripcion = consulta.DESCRIPCION;
                model.Activo = consulta.ACTIVO;
            }
            
            ViewBag.Editar = true;
            return View(CREATE_UPDATE_ABSOLUTE_PATH, model);
        }


        [HttpPost]
        public JsonResult Guardar(ColorMezclaModel model)
        {
            var rm = new ResponseModel();
            if (!ModelState.IsValid)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }


            var entity = db.color_mezcla.Find(model.Id);
            bool nuevo = false;

            if (entity == null)
            {                
                entity = new color_mezcla();
                nuevo = true;
            }


            entity.NOMBRE = model.Nombre;
            entity.DESCRIPCION = model.Descripcion;
          

            entity.ACTIVO = model.Activo;



            if (nuevo)
                db.color_mezcla.Add(entity);


            if (db.SaveChanges() > 0 || db.Entry(entity).State == EntityState.Unchanged)
            {
                rm.response = true;
                rm.message = null; //"Sus datos se guardaron correctamente";
                if (nuevo)
                    rm.href = "Index";
                else
                    rm.href = "self";
                TempData["message"] = "success,Sus datos se guardaron correctamente";
            }




            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            bool success = false;
            string msj = "Hubo un problema verifique su conexion e intente de nuevo.";

            var entity = db.color_mezcla.Find(id);
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
        public ActionResult GetFormula(int id)
        {

            var consulta = db.colores_v.Where(m => m.ID_COLOR==id);
            var jsonData = new
            {
                rows = (
                      from c in consulta
                      select new
                      {
                          ID_COLOR = c.ID_COLOR,
                          COLOR = c.COLOR,
                          // DESCRIPCION = c.DESCRIPCION,
                          // PRECIO = c.PRECIO,
                          // TIPO = c.tipo_atributo.NOMBRE,
                          ID_MP = c.ID_MP,
                          MP = c.MP,
                          CODIGO_MP = c.CODIGO_MP,
                          DESCRIPCION_MP = c.DESCRIPCION_MP,
                          PORCENTAJE = c.PORCENTAJE,
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