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
    public class insumosController : Controller
    {
        private ArtexConnection db = new ArtexConnection();

        private const string ABSOLUTE_PATH = "~/Views/Catalogos/Insumos/ListaInsumos.cshtml";
        private const string CREAR_EDITAR_ABSOLUTE_PATH = "~/Views/Catalogos/Insumos/CrearEditarInsumo.cshtml";

        public ActionResult Index()
        {
            PermisosModel model = PermisosModulo.ObtenerPermisos(Modulo.INSUMOS);
            if (model == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult GetAlls()
        {

            var consulta = db.insumo;

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

        public ActionResult Ver(int id )
        {
            InsumoMpModel model = new InsumoMpModel();

            var consulta = db.insumo.Find(id);

            if (consulta != null)
            {
                EntityToModel(ref model, ref consulta);
                ViewBag.Editar = false;
                return View(CREAR_EDITAR_ABSOLUTE_PATH, model);
            }

            TempData["message"] = "danger, No fue posible cargar sus datos";
            return RedirectToAction("Index");
        }


        public ActionResult Editar(int id = -1)
        {
            InsumoMpModel model = new InsumoMpModel();

            var consulta = db.insumo.Find(id);

            if (consulta != null)
            {
                EntityToModel(ref model, ref consulta);

            }
            else
            {
                model.PresentacionCompra = 0;
                model.PresentacionCompraList = UnidadMedidaDAO.GetAlls();
                model.PresentacionEntrega = 0;
                model.PresentacionEntregaList = UnidadMedidaDAO.GetAlls();
               // model.AtributoList = new List<atributos_configuracion>();
               // model.tipoAtributoList = db.tipo_atributo.Where(m => m.ACTIVO);

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


            var entity = db.insumo.Find(model.Id);
                bool nuevo = false;

                if (entity == null)
                {
                if (db.insumo.Any(m => m.CODIGO == model.Codigo))
                {
                    rm.message = "Ese ya ha sido asignado";
                    return Json(rm, JsonRequestBehavior.AllowGet);
                }
                entity = new insumo();
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
                    db.insumo.Add(entity);


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

            var entity = db.insumo.Find(id);
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

        void EntityToModel(ref InsumoMpModel model, ref insumo consulta)
        {
          //  var tipoAtributo = consulta.atributos_configuracion != null ? consulta.atributos_configuracion.ID_TIPO_ATRIBUTO : 0;

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

        /*    model.tipoAtributo = consulta.atributos_configuracion != null ? consulta.atributos_configuracion.ID_TIPO_ATRIBUTO : 0;
            model.Atributo = consulta.atributos_configuracion != null ? (int)consulta.ID_ATRIBUTO : 0;
            model.tipoAtributoList = db.tipo_atributo.ToList();
            model.AtributoList = model.Atributo > 0 ? db.atributos_configuracion.Where(m => m.ID_TIPO_ATRIBUTO == tipoAtributo).ToList() : new List<atributos_configuracion>();
            */


            model.Activo = consulta.ACTIVO;

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