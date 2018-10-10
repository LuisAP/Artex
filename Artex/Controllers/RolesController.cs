using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artex.Util;
using Artex.DB;
using Artex.Models.DAL.DAO;
using Artex.Models.DAL.DTO.RecursosHumanos;
using Artex.Models.ViewModels.RecursosHumanos;
using Artex.Models.BLL.RecursosHumanos;
using Artex.Util.Sistema;
using System.Data.Entity;

namespace Artex.Controllers
{
    public class RolesController : Controller
    {
        private ArtexConnection db = new ArtexConnection();
        private  string  ABSOLUTE_PATH = "~/Views/RecursosHumanos/Roles/ListaRoles.cshtml";
        private string CREATE_UPDATE_ABSOLUTE_PATH = "~/Views/RecursosHumanos/Roles/CrearEditarRol.cshtml";

        
        // GET: Roles
        public ActionResult Index()
        {
            PermisosModel model = PermisosModulo.ObtenerPermisos(Modulo.ROLES);
            if (model == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult GetAlls()
        {
            List<rol> consulta = null;

            RolDAO CLDAO = new RolDAO();
            consulta = CLDAO.GetAlls(db); /// obtener todos


            var jsonData = new
            {

                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE,
                            DESCRIPCION = c.DESCRIPCION,
                            ACTIVO = c.ACTIVO

                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Ver(int id)
        {
            RolModel rolModel = new RolModel();
            RolDAO roleDAO = new RolDAO();


            ModuloBLL moduleBuilder = new ModuloBLL();

            rol rolEntity = roleDAO.GetFromId(id, db);
            if (rolEntity != null)
            {
                rolModel.idRol = rolEntity.ID;
                rolModel.nombre = rolEntity.NOMBRE;
                rolModel.descripcion = rolEntity.DESCRIPCION;
                rolModel.habilitado = rolEntity.ACTIVO;
                rolModel.listaModulosSubmodulos = moduleBuilder.ObtenerModeloModuloSubmodulos(rolEntity, db);
                rolModel.listaPermisosEspecilaes = moduleBuilder.ObtenerPermisosEspeciales(rolEntity, db);

                ViewBag.Editar = false;
                return View(CREATE_UPDATE_ABSOLUTE_PATH, rolModel);
            }

            TempData["message"] = "danger, No fue posible cargar sus datos";
            return RedirectToAction("Index");

        }

        public ActionResult Editar(int id = -1)
        {
            
            
            RolModel rolModel = new RolModel();
            RolDAO roleDAO = new RolDAO();
            rolModel.habilitado = true;


            ModuloBLL moduleBuilder = new ModuloBLL();

            rol rolEntity = roleDAO.GetFromId(id, db);
            if (rolEntity != null)
            {
                rolModel.idRol = rolEntity.ID;
                rolModel.nombre = rolEntity.NOMBRE;
                rolModel.descripcion = rolEntity.DESCRIPCION;
                rolModel.habilitado = rolEntity.ACTIVO;
            }


            rolModel.listaModulosSubmodulos = moduleBuilder.ObtenerModeloModuloSubmodulos(rolEntity, db);
            rolModel.listaPermisosEspecilaes = moduleBuilder.ObtenerPermisosEspeciales(rolEntity, db);

            ViewBag.Editar = true;
            return View(CREATE_UPDATE_ABSOLUTE_PATH, rolModel);
        }
        [HttpPost]
        public ActionResult Guardar(RolModel model)
        {
            var rm = new ResponseModel();
            bool nuevo = false;
            if (!ModelState.IsValid)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }

             try
                {

                RolDAO roleDAO = new RolDAO();
                rol rol = roleDAO.GetFromId(model.idRol, db);

                if (rol == null)
                {
                    rol = new rol();
                    nuevo = true;
                }
                else
                {
                    //setear rol
                    if (rol.modulo != null)
                        rol.modulo.Clear();

                    if (rol.permisos_modulo != null)
                        rol.permisos_modulo.Clear();

                    if (rol.permisos_especiales != null)
                        rol.permisos_especiales.Clear();

                    db.SaveChanges();
                }

                rol.NOMBRE = model.nombre;
                rol.DESCRIPCION = model.descripcion;
                rol.ACTIVO = model.habilitado;

                // Añadir modulos seleccionados
                List<ModuloDTO> modulosSeleccionados = new List<ModuloDTO>();

                List<ModuloDTO> submodulosSeleccionados = new List<ModuloDTO>();

                // Añadir submodulos seleccionados
                foreach (ModuloDTO modulo in model.listaModulosSubmodulos)
                {
                    if (modulo.listaSubmodulo != null && modulo.listaSubmodulo.Any())
                    {
                        List<ModuloDTO> listaSubmodulos = modulo.listaSubmodulo.Where(m => m.habilitado == true).ToList();
                        if (listaSubmodulos.Any())
                        {
                            modulosSeleccionados.Add(modulo);
                            foreach (ModuloDTO submodulo in listaSubmodulos)
                            {
                                submodulosSeleccionados.Add(submodulo);
                            }
                        }

                    }
                }

                foreach (ModuloDTO submodulo in submodulosSeleccionados)
                {
                    modulosSeleccionados.Add(submodulo);
                }



                foreach (ModuloDTO moduloSeleccionado in modulosSeleccionados)
                {
                    modulo modulo = db.modulo.Where(m => m.ID == moduloSeleccionado.id).First();
                    rol.modulo.Add(modulo);

                    if (!moduloSeleccionado.esRaiz)
                    {
                        var permisos = new permisos_modulo();
                        permisos.ID_ROL = rol.ID;
                        permisos.ID_MODULO = modulo.ID;

                        permisos.VER = moduloSeleccionado.ver;
                        permisos.CREAR = moduloSeleccionado.crear;
                        permisos.EDITAR = moduloSeleccionado.editar;
                        permisos.ELIMINAR = moduloSeleccionado.eliminar;
                        permisos.REPORTE = moduloSeleccionado.reportes;


                        rol.permisos_modulo.Add(permisos);
                    }
                }

                //Agregar permisos especiales habilitados
                foreach (PermisosEspecialesDTO permisoDTO in model.listaPermisosEspecilaes)
                {
                    if (permisoDTO.habilitado)
                    {
                        permisos_especiales permisosEntity = db.permisos_especiales.Where(m => m.ID == permisoDTO.id).FirstOrDefault();
                        rol.permisos_especiales.Add(permisosEntity);
                    }


                }
                if (nuevo)
                    db.rol.Add(rol);

                if (db.SaveChanges() > 0 || db.Entry(rol).State == EntityState.Unchanged)

                {
                    rm.response = true;
                    rm.href = "Index";
                    TempData["message"] = "success,Sus datos se guardaron correctamente";
                }
            }
                catch (Exception e)
                {

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