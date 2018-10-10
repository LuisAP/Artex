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
using Artex.Models.BLL.Catalogos;

namespace Artex.Controllers.Catalogos
{
    [Authorize]
    public class AtributosController : Controller
    {
        private ArtexConnection db = new ArtexConnection();

        private const string ABSOLUTE_PATH = "~/Views/Catalogos/AtributosDeProducto/ListaAtributos.cshtml";
        private const string CREATE_UPDATE_ABSOLUTE_PATH = "~/Views/Catalogos/AtributosDeProducto/CrearActualizarAtributo.cshtml";

        public ActionResult Index()
        {
            PermisosModel model = PermisosModulo.ObtenerPermisos(Modulo.ATRIBUTOS);
            if (model == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(CREATE_UPDATE_ABSOLUTE_PATH);
        }
       /*ublic ActionResult GetAlls()
        {

            var consulta = db.atributo_subatributo.Where(m=> m.NIVEL==0);
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
                             //ACTIVO = c.ACTIVO,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
*/
      /*public ActionResult ver(int id)
        {

            AtributoProductoModel model = new AtributoProductoModel();
           // AtributoProductoDAO dao = new AtributoProductoDAO();
            MateriaPrimaDAO insumoMpDAO = new MateriaPrimaDAO();

            var consulta = db.atributo_subatributo.Find(id);

            if (consulta != null)
            {
           //     EntityTomodel(ref model, ref consulta);
                ViewBag.Editar = false;
                return View(CREATE_UPDATE_ABSOLUTE_PATH, model);
            }
            TempData["message"] = "danger, No fue posible cargar sus datos";
            return RedirectToAction("Index");
        }
        */
      /*  public ActionResult Editar(int id = -1)
        {

            AtributoProductoModel model = new AtributoProductoModel();
            //AtributoProductoDAO dao = new AtributoProductoDAO();
            MateriaPrimaDAO insumoMpDAO = new MateriaPrimaDAO();

            var consulta = db.atributo_subatributo.Find(id);

            if (consulta != null)
            {
                model.Id = consulta.ID;
                model.Nombre = consulta.NOMBRE;
                model.Descripcion = consulta.DESCRIPCION;
                model.Codigo = consulta.CODIGO;
            }
            else
            {
             //   model.TipoAtributoList = TipoAtributoProductoDAO.GetActive(db);

               // model.gradoList = db.atributo_grado.Where(m => m.ACTIVO);
                //  model.MateriaPrimaList = insumoMpDAO.GetAlls();


                model.Activo = true;
            }
            ViewBag.Editar = true;
            return View(CREATE_UPDATE_ABSOLUTE_PATH, model);
        }
        */
        [HttpPost]
        public JsonResult GetById(int id)
        {
            var  c = db.atributo_subatributo.Find(id);

            var jsnResult = new
            {
                ID = c.ID,
                NOMBRE = c.NOMBRE,
                CODIGO = c.CODIGO,
                DESCRIPCION = c.DESCRIPCION,

                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarAtributo(AddSubAtributoModel model)
        {
            var rm = new ResponseModel();
            if (!ModelState.IsValid)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }

            if (model.id > 0)
            {
                //Modificar atributo
                var atributo = db.atributo_subatributo.Find(model.id);
                atributo.NOMBRE = model.Nombre;
                atributo.DESCRIPCION = model.Descripcion;
                atributo.CODIGO = model.Codigo;
            }
            else if (model.IdPadre > 0)
            {

                //Agregar atributo hijo
                var atributoPadre = db.atributo_subatributo.Find(model.IdPadre);
                atributo_subatributo atributoHijo = new atributo_subatributo();


                atributoHijo.NOMBRE = model.Nombre;
                atributoHijo.DESCRIPCION = model.Descripcion;
                atributoHijo.CODIGO = model.Codigo;
                atributoHijo.NIVEL = atributoPadre.NIVEL + 1;
                atributoHijo.ID_PADRE = atributoPadre.ID;
                atributoHijo.TIENE_HIJOS = false;
                atributoPadre.TIENE_HIJOS = true;
                db.atributo_subatributo.Add(atributoHijo);
            }

            else
            { //Agregar atributo al nivel 0
                atributo_subatributo newAtributo = new atributo_subatributo();


                newAtributo.NOMBRE = model.Nombre;
                newAtributo.DESCRIPCION = model.Descripcion;
                newAtributo.CODIGO = model.Codigo;
                newAtributo.NIVEL = 0;
                newAtributo.TIENE_HIJOS = false;
                db.atributo_subatributo.Add(newAtributo);

            }

            
            //atributoHijo.ACTIVO = model.Activo;
            // entity.CODIGO = model.Codigo;
            //  entity.PRECIO = ExtensionMethods.ConverToDecimalFormat(model.Precio);
            //  entity.ID_AREA = model.AreaFabricacion;
            //  entity.ID_PROCESO = model.ProcesoFabricacion;
            //  entity.ID_TIPO_ATRIBUTO = model.TipoAtributo;
            //  entity.ID_UNIDAD_MEDIDA = model.UnidadMedida;
            //  entity.GRADO = null;
            //  entity.GRADO = model.TipoAtributo == 2 ? model.grado : entity.GRADO;



            if (db.SaveChanges() > 0)
            {
                rm.response = true;
                rm.message = "Sus datos se guardaron correctamente";
               // rm.href = "self";
                rm.function = "reload(true,'" + rm.message + "')";
            }
            
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        //Metodo que muestra todas las mp de un atributo
        public ActionResult GetMpAtributo(int idAtributo)
        {

            var entity = db.atributo_subatributo.Find(idAtributo);

            var mp = entity.materia_prima;

            //var consulta = db.atributo_subatributo.Where(m => m.ID_PRODUCTO == idProd).Where(m => m.TIPO == "Mp");
            var jsonData = new
            {
                rows = (
                      from c in mp
                      select new
                      {
                          ID = c.ID,
                          ID_ATRIBUTO= entity.ID,
                          NOMBRE = c.NOMBRE,
                          CODIGO = c.CODIGO,
                          DESCRIPCION = c.DESCRIPCION,
                          PRESENTACION_ENTREGA = c.unidad_medida.NOMBRE,
                          TIPO_COMPRA = c.TIPO_COMPRA,
                          TIPO = c.TIPO,
                      }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        //Metodo que muestra todas las mp activas que no pertenecen a un atributo especificado
        public ActionResult GetMp(int idAtributo)
        {

            var entityAtributo = db.atributo_subatributo.Find(idAtributo);
            HashSet<int> idMpAtributo = new HashSet<int>(from x in entityAtributo.materia_prima.Select(x => (int)x.ID) select x);

            var mp = db.materia_prima.Where(m=> m.ACTIVO).Where(m=> !idMpAtributo.Contains(m.ID));


            var jsonData = new
            {
                rows = (
                      from c in mp
                      select new
                      {
                          ID = c.ID,
                          NOMBRE = c.NOMBRE,
                          DESCRIPCION = c.NOMBRE+" ("+ c.CODIGO+")",
                      }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddMpAtributo(int idAtributo, int idMp)
        {
            var rm = new ResponseModel();

            var entityAtributo = db.atributo_subatributo.Find(idAtributo);
            var entityMp = db.materia_prima.Find(idMp);

            entityAtributo.materia_prima.Add(entityMp);
            
            if (db.SaveChanges() > 0)
            {
                rm.response = true;
                rm.message =null;
                rm.function = "reloadModalMp(true,'Sus datos se guardaron correctamente')";
            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteMpAtributo(int idAtributo, int idMp)
        {
            var rm = new ResponseModel();

            var entityAtributo = db.atributo_subatributo.Find(idAtributo);
            var entityMp = db.materia_prima.Find(idMp);

            entityAtributo.materia_prima.Remove(entityMp);

            if (db.SaveChanges() > 0)
            {
                rm.response = true;
                rm.message = "Sus datos se guardaron correctamente";
                rm.function = "reloadModalMp(true,'" + rm.message + "')";
            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }


       

        public JsonResult DeleteAtributo(int id)
        {
            var rm = new ResponseModel();
            AtributosBLL atributosBLL = new AtributosBLL();

            /*

                Agregar verificacion: si atributo pertenece a un producto no se permite eliminar
                */


            //Obtener todos los atributos 
            var listaAtributos = db.atributo_subatributo.Where(m => m.ID == id).ToList();           // listaAtributos.Add(padre);
            listaAtributos.AddRange(atributosBLL.ListarHijos(id, ref db));

            //Eliminar atributo y subatributos si tiene incliuyedno la relacion con la mp
            listaAtributos.ForEach(m => { m.materia_prima.Clear();  db.atributo_subatributo.Remove(m); });

            if (db.SaveChanges() > 0)
            {
                rm.response = true;
                rm.message = "Sus datos se guardaron correctamente";
              //  rm.function = "reload(true,'" + rm.message + "')";
            }

            return Json(rm, JsonRequestBehavior.AllowGet);

        }




        /*
            List<atributo_subatributo> ListarHijos(int idPadre)
        {
            List<atributo_subatributo> lista = new List<atributo_subatributo>();

            var padre = db.atributo_subatributo.Find(idPadre);
            lista.Add(padre);
            var subAtributos = db.atributo_subatributo.Where(m => m.ID_PADRE == idPadre).ToList();

            foreach (var atributo in subAtributos)
            {
                lista.AddRange(ListarHijos(atributo.ID));
            }
            return lista;
        }

             */
        /*    public ActionResult GetTipoAtributo()
            {
               // TipoAtributoProductoDAO dao = new TipoAtributoProductoDAO();

               / List<tipo_atributo> consulta = TipoAtributoProductoDAO.GetAlls();

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
            }*/
        public ActionResult GetUnidadMedida()
        {

            List<unidad_medida> consulta = UnidadMedidaDAO.GetAlls();

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

        public ActionResult GetSubAtributos(int? id = null)
        {

           //var consulta = db.atributo_subatributo ;
            var consulta = db.atributo_subatributo.Where(m => m.ID_PADRE == id).OrderBy(m=> m.CODIGO);

            var jsonData = new
            {
                callback = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE,
                            CODIGO = c.CODIGO,
                            DESCRIPCION = c.DESCRIPCION,
                            NIVEL=c.NIVEL,
                            ID_PADRE = c.ID_PADRE,
                            hasChildren = c.TIENE_HIJOS,
                            //COLOR=color(c.NIVEL)
                        }).ToArray()
            };
            return Json(jsonData.callback, JsonRequestBehavior.AllowGet);
        }


      /*  void EntityTomodel(ref AtributoProductoModel model,ref  consulta)
        {
            model.Id = consulta.ID;
            model.Nombre = consulta.NOMBRE;
            model.Descripcion = consulta.DESCRIPCION;
            model.Codigo = consulta.CODIGO;
            // model.Precio = ExtensionMethods.ToMoneyFormat(consulta.PRECIO);
            model.TipoAtributoList = TipoAtributoProductoDAO.GetActive(db);
            model.TipoAtributo = (int)consulta.ID_TIPO_ATRIBUTO;
            model.gradoList = db.atributo_grado.Where(m => m.ACTIVO);// UnidadMedidaDAO.GetAlls();
            model.grado = consulta.GRADO != null ? (int)consulta.GRADO : 0;
            //model.MateriaPrimaList = insumoMpDAO.GetAlls();
            //  model.MateriaPrima = consulta.ID_INSUMO_MP;
            model.Activo = consulta.ACTIVO;
        }*/

        /*   public ActionResult GetAreaFabricacion()
           {
               AreaFabricacionDAO dao = new AreaFabricacionDAO();

               List<area_de_fabricacion> consulta = dao.GetAlls();

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
           public ActionResult GetProcesoFabricacion()
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
                           }).ToArray()
               };
               return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
           }* /
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}