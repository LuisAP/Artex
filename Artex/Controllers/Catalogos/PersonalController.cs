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

namespace Artex.Controllers.RecursosHumanos
{
    [Authorize]
    public class PersonalController : Controller
    {
        private ArtexConnection db = new ArtexConnection();

        private const string ABSOLUTE_PATH = "~/Views/RecursosHumanos/Empleados/ListaEmpleados.cshtml";
        private const string VER_ABSOLUTE_PATH = "~/Views/RecursosHumanos/Empleados/CrearActualizarEmpleado.cshtml";
        private const string CREAR_EDITAR_ABSOLUTE_PATH = "~/Views/RecursosHumanos/Empleados/CrearActualizarEmpleado.cshtml";

        public ActionResult Index() 
        {
            PermisosModel model = PermisosModulo.ObtenerPermisos(Modulo.PERSONAL,db);
            if (model == null)
            {
                TempData["messaje"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH,model);
        }
        public ActionResult GetAlls()
        {


            var consulta = db.empleados_v;

            
            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID_EMPLEADO = c.ID_EMPLEADO,
                            EMPLEADO = c.NOMBRE_COMPLETO,
                            PUESTO = c.PUESTO,
                            RFC = c.RFC,
                            CORREO_PERSONAL = c.CORREO_PERSONAL,
                            ACTIVO = c.ACTIVO

                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Crear()
        {
            EmpleadoModel model = new EmpleadoModel();

            try
            {
                EmpleadoDAO CLDAO = new EmpleadoDAO();


                model.activo = true;

                model.paisList = PaisDAO.GetAlls(db);
                model.pais = model.paisList.FirstOrDefault(m => m.NOMBRE.Contains("Mex")).ID;
                model.estadoList = EstadoDAO.GetByIdPais((int)model.pais, db);
                model.estado = model.estadoList.FirstOrDefault(m => m.NOMBRE.Contains("Jalis")).ID;
                model.areaTrabajoList = AreaTrabajoDAO.GetAlls(db);
                model.sucursalList = db.tienda;

            }
            catch (Exception e)
            {
                LogUtil.ExceptionLog(e);
            }
            ViewBag.Editar = true;
            return View(CREAR_EDITAR_ABSOLUTE_PATH, model);
        }
        public ActionResult Ver(int id)
        {
            EmpleadoModel model = new EmpleadoModel();

            try
            {
                EmpleadoDAO CLDAO = new EmpleadoDAO();
                var entity = CLDAO.GetById(id, db);

                if (entity == null)
                {
                    return RedirectToAction("Index");
                   
                }
                copiarEntidadmodelo(ref model, ref entity);

            }
            catch (Exception e)
            {
                LogUtil.ExceptionLog(e);
            }
            ViewBag.Editar = false;
            return View(CREAR_EDITAR_ABSOLUTE_PATH, model);
        }

        public ActionResult Editar(int id)
        {
            EmpleadoModel model = new EmpleadoModel();

            try
                {
                    EmpleadoDAO CLDAO = new EmpleadoDAO();
                    var entity = CLDAO.GetById(id, db);

                    if (entity == null)
                    {
                    return RedirectToAction("Index");
                    }
                   copiarEntidadmodelo(ref model,ref entity);

                }
                catch (Exception e)
                {
                    LogUtil.ExceptionLog(e);
                }
            ViewBag.Editar = true;
            return View(CREAR_EDITAR_ABSOLUTE_PATH, model);
        }

      public EmpleadoModel copiarEntidadmodelo( ref EmpleadoModel model, ref empleado entity)
        {

            model.idPersona = entity.ID_PERSONA_FISICA;
            model.id = entity.ID;

            model.areaTrabajo = entity.ID_AREA_TRABAJO != null ? (int)entity.ID_AREA_TRABAJO : entity.ID_AREA_TRABAJO;
            model.areaTrabajoList = AreaTrabajoDAO.GetAlls(db);
            model.sucursalList = db.tienda;
            model.puesto = entity.PUESTO;
            model.fechaIngreso = entity.FECHA_INGRESO != null ? ExtensionMethods.DateFormat((DateTime)entity.FECHA_INGRESO) : null;
            model.fechaBaja = entity.FECHA_BAJA != null ? ExtensionMethods.DateFormat((DateTime)entity.FECHA_BAJA) : null;
            model.nss = entity.IMSS;
            model.salario = ExtensionMethods.ToMoneyFormat(entity.SALARIO);
            model.correoEmpresa = entity.EMAIL;
            model.activo = entity.ACTIVO;

            model.sucursal = entity.tienda.Count > 0 ? entity.tienda.FirstOrDefault().ID:0;

            PersonaFisicaDAO personaFisicaDAO = new PersonaFisicaDAO();
                    var persona = personaFisicaDAO.GetById(entity.ID_PERSONA_FISICA);
                    model.nombre = persona.NOMBRE;
                    model.apellidoPaterno = persona.APELLIDO_PATERNO;
                    model.apellidoMaterno = persona.APELLIDO_MATERNO;
                    model.fechaNacimiento = persona.FECHA_NACIMIENTO != null ? ExtensionMethods.DateFormat((DateTime)persona.FECHA_NACIMIENTO) : null;
                    model.curp = persona.CURP;
                    model.rfc = persona.RFC;
                    model.sexo = persona.SEXO;
                    model.correo = persona.EMAIL;
                    model.telefono = persona.TELEFONO;
                    model.celular = persona.CELULAR;

                    DireccionDAO direccionDAO = new DireccionDAO();
                    var direccion = direccionDAO.GetById((int)persona.ID_DIRECCION);
                    model.calle = direccion.CALLE;
                    model.numInterior = direccion.NUM_INTERIOR;
                    model.numExterior = direccion.NUM_EXTERIOR;
                    model.colonia = direccion.COLONIA;
                    model.ciudad = direccion.CIUDAD;
                    model.municipio = direccion.MUNICIPIO;
                    model.codigoPostal = direccion.CP;
                    model.estado = direccion.ID_ESTADO;
                    model.pais = direccion.ID_PAIS;
                    model.paisList = PaisDAO.GetAlls(db);
                    model.estadoList = EstadoDAO.GetByIdPais((int)model.pais, db);



            return model;
        }


        [HttpPost]
        public JsonResult Guardar(EmpleadoModel model)
        {
            var rm = new ResponseModel();
            if (!ModelState.IsValid)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }


           
                try
                {
                    EmpleadoDAO dao = new EmpleadoDAO();
                    var entity = dao.GetById(model.id, db);
                    bool nuevo = false;

                    if (entity == null)
                    {
                        entity = new empleado();
                        entity.persona_fisica = new persona_fisica();
                        entity.persona_fisica.direccion = new direccion();
                        nuevo = true;
                    }
                    //datos empleado
                    entity.ID_AREA_TRABAJO = model.areaTrabajo;
                    entity.PUESTO = model.puesto;
                    entity.FECHA_INGRESO = (model.fechaIngreso != null && model.fechaIngreso != "") ? ExtensionMethods.ToDateFormat(model.fechaIngreso) : entity.FECHA_INGRESO;
                    entity.FECHA_BAJA = (model.fechaBaja != null && model.fechaBaja != "") ? ExtensionMethods.ToDateFormat(model.fechaBaja) : entity.FECHA_BAJA;
                    entity.IMSS = model.nss;
                    entity.EMAIL = model.correoEmpresa;
                    entity.SALARIO = ExtensionMethods.ConverToDecimalFormat(model.salario);
                    entity.ACTIVO = model.activo;

                if (model.areaTrabajo == 1){
                    var tienda = db.tienda.Find(model.sucursal);
                    entity.tienda.Clear();
                    if (tienda != null)
                        entity.tienda.Add(tienda);

                }
                else
                {
                    entity.tienda.Clear();
                }
                if (model.activo == false && entity.AspNetUsers!=null)
                {
                    AspNetUsers usuario = db.AspNetUsers.FirstOrDefault(m => m.PersonalId == entity.ID);
                    usuario.LockoutEnabled = false;
                }

                    //datos persona
                    entity.persona_fisica.NOMBRE = model.nombre;
                    entity.persona_fisica.APELLIDO_PATERNO = model.apellidoPaterno;
                    entity.persona_fisica.APELLIDO_MATERNO = model.apellidoMaterno;
                    entity.persona_fisica.FECHA_NACIMIENTO = ExtensionMethods.ToDateFormat(model.fechaNacimiento);
                    entity.persona_fisica.CURP = model.curp;
                    entity.persona_fisica.RFC = model.rfc;
                    entity.persona_fisica.SEXO = model.sexo;
                    entity.persona_fisica.EMAIL = model.correo;
                    entity.persona_fisica.TELEFONO = model.telefono;
                    entity.persona_fisica.CELULAR = model.celular;

                    //datos direccion
                    entity.persona_fisica.direccion.CALLE = model.calle;
                    entity.persona_fisica.direccion.NUM_INTERIOR = model.numInterior;
                    entity.persona_fisica.direccion.NUM_EXTERIOR = model.numExterior;
                    entity.persona_fisica.direccion.COLONIA = model.colonia;
                    entity.persona_fisica.direccion.CIUDAD = model.ciudad;
                    entity.persona_fisica.direccion.MUNICIPIO = model.municipio;
                    entity.persona_fisica.direccion.CP = model.codigoPostal;
                    entity.persona_fisica.direccion.ID_ESTADO = model.estado;
                    entity.persona_fisica.direccion.ID_PAIS = model.pais;


                    if (nuevo)
                        db.empleado.Add(entity);


                if (db.SaveChanges() > 0 || db.Entry(entity).State == EntityState.Unchanged)
                {
                        rm.response = true;
                        rm.message = "Sus datos se guardaron correctamente";
                        rm.href = "Index";
                    }
                }catch(Exception e)
                {

                }
            


            return Json(rm, JsonRequestBehavior.AllowGet);
        }





        public JsonResult Delete(int id)
        {
            bool success = false;
            string msj = "Hubo un problema verifique su conexion e intente de nuevo.";

            var entity = db.empleado.Find(id);
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
                Success = success,
                msj = msj
            };
            return Json(result, JsonRequestBehavior.AllowGet);

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