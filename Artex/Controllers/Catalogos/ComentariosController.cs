using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artex.Models.DAL.DAO;
using Artex.Models.DAL.DAO.Views;
using Artex.DB;
using Artex.Models.ViewModels.Catalogos;
using Artex.Util;
using Artex.Util.Sistema;
using System.Data.Entity;

namespace Artex.Controllers.Catalogos
{
    public class ComentariosController : Controller
    {
        private const string ABSOLUTE_PATH = "~/Views/Catalogos/Comentarios.cshtml";
        private ArtexConnection db = new ArtexConnection();
        // GET: Comentarios
        public ActionResult Index()
        {
            ComentariosModel model = new ComentariosModel();
            model.permisos = PermisosModulo.ObtenerPermisos(Modulo.COMENTARIOS);

            if (model.permisos == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult GetPrograma() {
            var consulta = db.tipo_programa.Where(m => m.ACTIVO == true).OrderBy(e => e.NOMBRE).ToList();

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
            var consulta = db.comentarios;
            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            COMENTARIO =c.COMENTARIO,
                            PROGRAMA = c.tipo_programa.NOMBRE,
                        }).ToArray()
            };

            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Guardar(ComentariosModel model) {
            var rm = new ResponseModel();
            var consulta = db.comentarios.Where(m => m.ID == model.Id).FirstOrDefault();
            if (consulta == null)
            {
                consulta = new comentarios();
                consulta.ID_TIPO_PROGRAMA = model.Programa;
                consulta.COMENTARIO = model.Comentario;
                db.comentarios.Add(consulta);
            }
            else
            {
                consulta.ID_TIPO_PROGRAMA = model.Programa;
                consulta.COMENTARIO = model.Comentario;
            }

            if (db.SaveChanges() > 0) {
                rm.response = true;
                rm.message = "Se guardaron los datos correctamente";
                rm.result = true;
                rm.function = "reload(true,'" + rm.message + "')";
            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetById(int id) {
            var c = db.comentarios.Where(m => m.ID == id).FirstOrDefault();
            var jsnResult = new
            {
                ID = c.ID,
                COMENTARIO = c.COMENTARIO,
                PROGRAMA = c.ID_TIPO_PROGRAMA,
                PROGRAMAN = c.tipo_programa.NOMBRE,
                Success = true
            };
            return Json(jsnResult, JsonRequestBehavior.AllowGet);

        }
    }
}