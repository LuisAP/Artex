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
    public class EstatusPedidoController : Controller
    {
        private const string ABSOLUTE_PATH = "~/Views/Catalogos/EstatusPedido.cshtml";

        public ActionResult Index()
        {
            EstatusPedidoModel model = new EstatusPedidoModel();
            model.permisos = PermisosModulo.ObtenerPermisos(Modulo.ESTATUS_PEDIDO);
            if (model.permisos == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult GetAlls()
        {

            List<estatus_pedido> consulta = EstatusPedidoDAO.GetAlls();

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
            EstatusPedidoDAO dao = new EstatusPedidoDAO();
            estatus_pedido c = dao.GetById(id);

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
        public JsonResult Guardar(AreaTrabajoModel model)
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
                EstatusPedidoDAO dao = new EstatusPedidoDAO();
                var entity = dao.GetById(model.Id, db);

                if (entity == null)
                {
                    entity = new estatus_pedido();
                    entity.NOMBRE = model.Nombre;
                    entity.DESCRIPCION = model.Descripcion;
                    entity.ACTIVO = model.Activo;
                    db.estatus_pedido.Add(entity);
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


            EstatusPedidoDAO dao = new EstatusPedidoDAO();
            rm.response = dao.DeleteById(id);

            if (rm.response)
            {
                rm.message = "El registro  se elimino correctamente";

            }

            return Json(rm, JsonRequestBehavior.AllowGet);

        }

    }
}