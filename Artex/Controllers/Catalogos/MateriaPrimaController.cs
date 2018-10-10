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
    public class MateriaPrimaController : Controller
    {
        private ArtexConnection db = new ArtexConnection();

        private const string ABSOLUTE_PATH = "~/Views/Catalogos/MateriaPrima/ListaMateriaPrima.cshtml";
        private const string CREAR_EDITAR_ABSOLUTE_PATH = "~/Views/Catalogos/MateriaPrima/CrearEditarMateriaPrima.cshtml";

        public ActionResult Index()
        {
            PermisosModel model = PermisosModulo.ObtenerPermisos(Modulo.MATERIA_PRIMA,db);
            if (model == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult GetAlls()
        {

            MateriaPrimaDAO dao = new MateriaPrimaDAO();
            List<materia_prima> consulta = dao.GetAlls(db);

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

        
        public ActionResult Ver(int id)
        {
            InsumoMpModel model = new InsumoMpModel();
            MateriaPrimaDAO dao = new MateriaPrimaDAO();

            var consulta = dao.GetById(id, db);

            if (consulta != null)
            {
                EntityToModel(ref model, ref consulta);
                ViewBag.Editar = false;
                return View(CREAR_EDITAR_ABSOLUTE_PATH, model);
            }
            TempData["message"] = "danger, No fue posible cargar sus datos";
            return RedirectToAction("Index");
        }

        public ActionResult Editar(int id=-1)
        {
            InsumoMpModel model = new InsumoMpModel();
            MateriaPrimaDAO dao = new MateriaPrimaDAO();


            var consulta = dao.GetById(id, db);

            if (consulta != null)
            {
                EntityToModel(ref model, ref consulta);
            }
            else
            {
                model.PresentacionCompra = 0;
                model.PresentacionCompraList = db.unidad_medida.Where(m => m.ACTIVO);
                model.PresentacionEntrega = 0;
                model.PresentacionEntregaList = db.unidad_medida.Where(m => m.ACTIVO);
              //  model.AtributoList = new List<atributos_configuracion>();
              //  model.tipoAtributoList = db.tipo_atributo.Where(m=> m.ACTIVO);

                model.Activo = true;
            }
            ViewBag.Editar = true;
            return View(CREAR_EDITAR_ABSOLUTE_PATH, model);
        }


        [HttpPost]
        public JsonResult Guardar(InsumoMpModel model)
        {
            var rm = new ResponseModel();
            if (!ModelState.IsValid)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }
            
                MateriaPrimaDAO dao = new MateriaPrimaDAO();
                var entity = dao.GetById(model.Id, db);
                bool nuevo = false;

            if (entity == null)
            {
                if (db.materia_prima.Any(m => m.CODIGO == model.Codigo))
                {
                    rm.message = "Ese codigo de materia prima ya ha sido asignado";
                    return Json(rm, JsonRequestBehavior.AllowGet);
                }
                entity = new materia_prima();
                nuevo = true;
            }


            entity.NOMBRE = model.Nombre;
            entity.DESCRIPCION = model.Descripcion;
            entity.CODIGO = model.Codigo;
            entity.PRECIO_COMPRA = ExtensionMethods.ConverToDecimalFormat(model.PrecioCompra);
            entity.PRECIO_ENTREGA = model.PrecioEntrega != null ? ExtensionMethods.ConverToDecimalFormat(model.PrecioEntrega) : 0;
            entity.TIPO = model.Tipo;
            entity.PRESENTACION_COMPRA = model.PresentacionCompra;
            entity.PRESENTACION_ENTREGA = model.PresentacionEntrega;
            entity.TIPO_COMPRA = model.TipoCompra;
            entity.TIPO_EXPLOSION = model.TipoExplosion;
            entity.ID_ATRIBUTO = null;
            entity.ID_ATRIBUTO = model.Atributo > 0 ? model.Atributo : entity.ID_ATRIBUTO;


            entity.ACTIVO = model.Activo;



            if (nuevo)
                db.materia_prima.Add(entity);


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
            var rm = new ResponseModel();


            MateriaPrimaDAO dao = new MateriaPrimaDAO();
            rm.response = dao.DeleteById(id);

            if (rm.response)
            {
                rm.message = "El registro  se elimino correctamente";

            }

            return Json(rm, JsonRequestBehavior.AllowGet);

        }

        void EntityToModel(ref InsumoMpModel model, ref materia_prima consulta)
        {
         //   var tipoAtributo = consulta.atributos_configuracion != null ? consulta.atributos_configuracion.ID_TIPO_ATRIBUTO : 0;
            model.Id = consulta.ID;
            model.Nombre = consulta.NOMBRE;
            model.Descripcion = consulta.DESCRIPCION;
            model.Codigo = consulta.CODIGO;
            model.PrecioCompra = ExtensionMethods.ToMoneyFormat(consulta.PRECIO_COMPRA);
            model.PrecioEntrega = ExtensionMethods.ToMoneyFormat(consulta.PRECIO_ENTREGA);
            model.Tipo = consulta.TIPO;
            model.PresentacionCompra = (int)consulta.PRESENTACION_COMPRA;
            model.PresentacionCompraList = UnidadMedidaDAO.GetAlls();
            model.PresentacionEntrega = (int)consulta.PRESENTACION_ENTREGA;
            model.PresentacionEntregaList = UnidadMedidaDAO.GetAlls();
            model.TipoCompra = consulta.TIPO_COMPRA;
            model.TipoExplosion = consulta.TIPO_EXPLOSION;
/*
            model.tipoAtributo = consulta.atributos_configuracion != null ? consulta.atributos_configuracion.ID_TIPO_ATRIBUTO : 0;
            model.Atributo = consulta.atributos_configuracion != null ? (int)consulta.ID_ATRIBUTO : 0;
            model.tipoAtributoList = db.tipo_atributo.ToList();
            model.AtributoList = model.Atributo > 0 ? db.atributos_configuracion.Where(m => m.ID_TIPO_ATRIBUTO == tipoAtributo).ToList() : new List<atributos_configuracion>();

            */

            model.Activo = consulta.ACTIVO;
        }
   /*     public ActionResult GetAtributos(int idTipo)
        {
            var entity = db.atributos_configuracion.Where(m => m.ACTIVO).Where(m => m.ID_TIPO_ATRIBUTO == idTipo);
            
            var resultado = new
            {
                rows = (
                        from c in entity
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE
                        }).ToArray()
            };
            return Json(resultado.rows, JsonRequestBehavior.AllowGet);
        }*/
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