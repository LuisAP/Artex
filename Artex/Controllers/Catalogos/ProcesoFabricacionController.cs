﻿using System;
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
    public class ProcesoFabricacionController : Controller
    {
        private const string ABSOLUTE_PATH = "~/Views/Catalogos/ProcesosFabricacion.cshtml";

        public ActionResult Index()
        {
            ProcesoFabricacionModel model = new ProcesoFabricacionModel();
            model.permisos = PermisosModulo.ObtenerPermisos(Modulo.PROCESO_FABRICACION);
            if (model.permisos == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult GetAlls()

        {

            ProcesoFabricacionDAO dao = new ProcesoFabricacionDAO();
            List<procesos_de_fabricacion> consulta = dao.GetAlls();

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
            ProcesoFabricacionDAO dao = new ProcesoFabricacionDAO();
            procesos_de_fabricacion c = dao.GetById(id);

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
        public JsonResult Guardar(ProcesoFabricacionModel model)
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
                ProcesoFabricacionDAO dao = new ProcesoFabricacionDAO();
                var entity = dao.GetById(model.Id, db);

                if (entity == null)
                {
                    entity = new procesos_de_fabricacion();
                    entity.NOMBRE = model.Nombre;
                    entity.DESCRIPCION = model.Descripcion;
                    entity.ACTIVO = model.Activo;
                    db.procesos_de_fabricacion.Add(entity);
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


            ProcesoFabricacionDAO dao = new ProcesoFabricacionDAO();
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
    }
}