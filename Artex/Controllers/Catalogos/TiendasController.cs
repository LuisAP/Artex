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
    public class TiendasController : Controller
    {
        private const string ABSOLUTE_PATH = "~/Views/Catalogos/Tiendas.cshtml";
        private ArtexConnection db = new ArtexConnection();
        // GET: Tiendas
        public ActionResult Index()
        {
            TiendaModel model = new TiendaModel();
            model.permisos = PermisosModulo.ObtenerPermisos(Modulo.TIENDAS);

            if (model.permisos == null) {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }

            return View(ABSOLUTE_PATH,model);
        }

        [HttpPost]
        public ActionResult GetById(int id) {
            TiendaDAO dao = new TiendaDAO();
            tienda c = dao.GetById(id);
            
            DireccionDAO daod = new DireccionDAO();
            direccion d = daod.GetById(Convert.ToInt32(c.ID_DIRECCION));
            

            var jsnResult = new
            {
                ID = c.ID,
                IDD = d.ID,
                NOMBRE = c.NOMBRE,
                RESPONSABLE = c.ID_RESPONSABLE,
                CREDITO_FM = c.CREDITO_FABRICACION_MAX,
                CREDITO_F = c.CREDITO_FABRICACION,
                CREDITO_C = c.CREDITO_COMERCIALIZACION,
                CREDITO_CM = c.CREDITO_COMERCIALIZACION_MAX,                
                ACTIVO = c.ACTIVO,
                CALLE = d.CALLE,
                NUM_EXT = d.NUM_EXTERIOR,
                NUM_INT = d.NUM_INTERIOR,
                CIUDAD = d.CIUDAD,
                COLONIA = d.COLONIA,
                MUNICIPIO = d.MUNICIPIO,
                CP = d.CP,
                PAIS = d.ID_PAIS,
                ESTADO = d.ID_ESTADO,
                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAlls() {
            var consulta = db.tienda;
            int j = 0;
            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE,
                            RESPONSABLE = c.empleado.persona_fisica.NOMBRE + " " + c.empleado.persona_fisica.APELLIDO_PATERNO + " " + c.empleado.persona_fisica.APELLIDO_MATERNO,
                            DIRECCION = c.direccion.CALLE + " #"+c.direccion.NUM_EXTERIOR,
                            CREDITO_FM = c.CREDITO_FABRICACION_MAX,
                            CREDITO_F = c.CREDITO_FABRICACION,
                            CREDITO_CM = c.CREDITO_COMERCIALIZACION_MAX,
                            CREDITO_C = c.CREDITO_COMERCIALIZACION,
                            ACTIVO = c.ACTIVO,                           
                        }).ToArray()
            };

            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(TiendaModel model) {
            var rm = new ResponseModel();
            if (!ModelState.IsValid)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }
            using (ArtexConnection db = new ArtexConnection())
            {
                TiendaDAO dao = new TiendaDAO();
                var entity = dao.GetById(model.Id, db);
               /* DireccionDAO dir = new DireccionDAO();
                var entityD = dir.GetById(Convert.ToInt32(entity.ID_DIRECCION), db);*/

                if (entity == null)
                {
                    
                    entity = new tienda();
                    entity.direccion = new direccion();
                    entity.NOMBRE = model.Nombre;
                    entity.ID_RESPONSABLE = model.Responsable;                    
                    entity.CREDITO_FABRICACION_MAX = model.Credito_FM;
                    entity.CREDITO_FABRICACION = model.Credito_F;
                    entity.CREDITO_COMERCIALIZACION_MAX = model.Credito_CM;
                    entity.CREDITO_COMERCIALIZACION = model.Credito_C;
                    entity.ACTIVO = model.Activo;
                    entity.direccion.CALLE = model.Calle;
                    entity.direccion.NUM_EXTERIOR = model.Num_Ext;
                    entity.direccion.NUM_INTERIOR = model.Num_Int;
                    entity.direccion.COLONIA = model.Colonia;
                    entity.direccion.CIUDAD = model.Ciudad;
                    entity.direccion.MUNICIPIO = model.Municipio;
                    entity.direccion.CP = model.CP;
                    entity.direccion.ID_PAIS = model.Pais;
                    entity.direccion.ID_ESTADO = model.Estado;

                    db.tienda.Add(entity);
                }
                else
                {
                    entity.direccion.CALLE = model.Calle;
                    entity.direccion.NUM_EXTERIOR = model.Num_Ext;
                    entity.direccion.NUM_INTERIOR = model.Num_Int;
                    entity.direccion.COLONIA = model.Colonia;
                    entity.direccion.CIUDAD = model.Ciudad;
                    entity.direccion.MUNICIPIO = model.Municipio;
                    entity.direccion.CP = model.CP;
                    entity.direccion.ID_PAIS = model.Pais;
                    entity.direccion.ID_ESTADO = model.Estado;
                    entity.NOMBRE = model.Nombre;
                    entity.ID_RESPONSABLE = model.Responsable;
                    entity.CREDITO_FABRICACION_MAX = model.Credito_FM;
                    entity.CREDITO_FABRICACION = model.Credito_F;
                    entity.CREDITO_COMERCIALIZACION_MAX = model.Credito_CM;
                    entity.CREDITO_COMERCIALIZACION = model.Credito_C;
                    entity.ACTIVO = model.Activo;
                }

                if (db.SaveChanges() > 0 || db.Entry(entity).State == EntityState.Unchanged)
                {
                    rm.response = true;
                    rm.message = "Sus datos se guardaron correctamente";
                    rm.function = "reload(true,'" +rm.message + "')";
                }
            }
                return Json(rm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerEmpleados()
        {
            var consulta = db.empleado.Where(m => m.ACTIVO).Include(m=>m.persona_fisica);

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.persona_fisica.NOMBRE,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerPais()
        {
            var consulta = db.pais.OrderBy(e => e.ID).ToList();

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

        public ActionResult GetEstadosByPais(int idPais)
        {
            var consulta = db.estado.Where(m => m.ID_PAIS ==idPais).OrderBy(e => e.NOMBRE).ToList();

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


            TiendaDAO dao = new TiendaDAO();
            rm.response = dao.DeleteById(id);

            if (rm.response)
            {
                rm.message = "El registro  se elimino correctamente";

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