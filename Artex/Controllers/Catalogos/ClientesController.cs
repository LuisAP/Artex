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


namespace Dorantes.Controllers.Catalogue
{
    [Authorize]
    public class ClientesController : Controller
    {
        private ArtexConnection db = new ArtexConnection();

        private const string ABSOLUTE_PATH = "~/Views/Catalogos/Clientes/ListaClientes.cshtml";
        private const string VER_ABSOLUTE_PATH = "~/Views/Catalogos/Clientes/CrearActualizarCliente.cshtml";
        private const string CREAR_EDITAR_ABSOLUTE_PATH = "~/Views/Catalogos/Clientes/CrearActualizarCliente.cshtml";

        //
        // GET: /Cliente/
        public ActionResult Index()
        {
            PermisosModel model = PermisosModulo.ObtenerPermisos(Modulo.CLIENTES);
            if (model == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }

        //
        // GET: /CatalogoCliente/NuevoCliente/
        public ActionResult Crear()
        {
            if (!PermisosModulo.ObtenerPermiso(Modulo.CLIENTES, Permiso.CREAR))
            {
                TempData["message"] = "danger,No tiene permisos.";
                return RedirectToAction("Index");
            }
            ClienteModel model = null;
            try
            {
                using (ArtexConnection db = new ArtexConnection())
                {
                    model = new ClienteModel();
                    model.esPersonaFisica = true;
                    model.Activo = true;

                    model.paisList = PaisDAO.GetAlls(db);
                    model.pais = model.paisList.FirstOrDefault(m => m.NOMBRE.Contains("Mex")).ID;
                    model.estadoList = EstadoDAO.GetByIdPais((int)model.pais, db);
                    model.estado = model.estadoList.FirstOrDefault(m => m.NOMBRE.Contains("Jalis")).ID;

                    model.bancosList = BancoDAO.GetAlls();

                }
            }
            catch (Exception e)
            {
                LogUtil.ExceptionLog(e);
                model = null;
            }
            ViewBag.Editar = true;
            return View(CREAR_EDITAR_ABSOLUTE_PATH, model);
        }
        
        public ActionResult Ver(int id = -1)
        {
            if (!PermisosModulo.ObtenerPermiso(Modulo.CLIENTES, Permiso.VER))
            {
                TempData["message"] = "danger,No tiene permisos.";
                return RedirectToAction("Index");
            }
            ClienteModel model = null;
            try
            {
                ClienteDAO CLDAO = new ClienteDAO();
                var entity = CLDAO.GetById(id, db);
                if (entity != null)
                {
                    model = new ClienteModel();
                    CopiarEntidadModelo(ref model, entity, db);
                }
                ViewBag.Editar = false;
                return View(CREAR_EDITAR_ABSOLUTE_PATH, model);

            }
            catch (Exception e)
            {
                LogUtil.ExceptionLog(e);
            }
            return View(ABSOLUTE_PATH);
        }
        public ActionResult rfc() {
            var c = db.configuracion_sistema.Where(m => m.ID == 7).FirstOrDefault();
            var jsonData = new
            {
                RFC = c.VALOR_CADENA
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //
        public ActionResult Editar(int id = -1)
        {
            if (!PermisosModulo.ObtenerPermiso(Modulo.CLIENTES, Permiso.EDITAR))
            {
                TempData["message"] = "danger,No tiene permisos.";
                return RedirectToAction("Index");
            }

            ClienteModel model = null;
            if (id > 0)
            {
                try
                {

                    ClienteDAO CLDAO = new ClienteDAO();
                    var entity = CLDAO.GetById(id, db);

                    var rol = db.rol.First();
                    // permisos_sistema permiso = rol.permisos_sistema.FirstOrDefault(m => m.ID_MODULO == 2);
                    // bool edit = permiso.EDITAR;

                    if (entity != null)
                    {
                        model = new ClienteModel();
                        CopiarEntidadModelo(ref model, entity, db);
                        ViewBag.Editar = true;
                        return View(CREAR_EDITAR_ABSOLUTE_PATH, model);
                    }

                }
                catch (Exception e)
                {
                    LogUtil.ExceptionLog(e);
                }
            }
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public JsonResult Guardar(ClienteModel model)
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

                ClienteDAO dao = new ClienteDAO();
                var entity = dao.GetById(model.idCliente, db);
                bool nuevo = false;
                string error = "";

                if (entity == null)
                {
                    if (!validarInsersion(ref model, ref error))
                    {
                        rm.message = error;
                        return Json(rm, JsonRequestBehavior.AllowGet);
                    }

                    entity = new cliente();
                    entity.direccion = new direccion();
                    nuevo = true;
                }
                else
                {
                    if (!validarModificacion(ref model, ref entity, ref error))
                    {
                        rm.message = error;
                        return Json(rm, JsonRequestBehavior.AllowGet);
                    }
                }

                if (model.esPersonaFisica)
                {
                    entity.TIPO_PERSONA = "Fisica";
                    entity.NOMBRE = ExtensionMethods.Trim( model.nombrePersona);
                    entity.APELLIDO_PATERNO = ExtensionMethods.Trim(model.apellidoPaterno);
                    entity.APELLIDO_MATERNO = ExtensionMethods.Trim(model.apellidoMaterno);

                    entity.EMPRESA = null;
                    entity.RAZON_SOCIAL = null;
                }
                else
                {
                    entity.TIPO_PERSONA = "Moral";
                    entity.EMPRESA = ExtensionMethods.Trim(model.nombreEmpresa);
                    entity.RAZON_SOCIAL = ExtensionMethods.Trim(model.razonSocial);

                    entity.NOMBRE = null;
                    entity.APELLIDO_PATERNO = null;
                    entity.APELLIDO_MATERNO = null;
                }

                entity.TELEFONO = model.telefono;
                entity.CELULAR = model.celular;
                entity.EMAIL = model.correo;
                entity.RFC = model.rfc;
                entity.REFERENCIA_BANCARIA = model.referencia;
                entity.ACTIVO = model.Activo;
                entity.CLABE = model.Clabe;
                entity.REFERENCIA_BANCARIA_2 = model.referencia2;

                entity.ES_CLIENTE_CREDITO = model.esClienteCredito;

                if (model.esClienteCredito)
                {
                    entity.SALDO = ExtensionMethods.ConverToDecimalFormat(model.saldo);
                    entity.MONTO_CREDITO = ExtensionMethods.ConverToDecimalFormat(model.creditoMaximo);

                }
                entity.ACTIVO = model.Activo;

                //Agregar persona contacto si la hay
                var contacto = entity.persona_contacto != null ? entity.persona_contacto : new persona_contacto();
                if (model.nombreContacto != null && model.nombreContacto != string.Empty)
                {
                    contacto.NOMBRE = model.nombreContacto;
                    contacto.EMAIL = model.correoContacto;
                    contacto.TELEFONO = model.telefonoContacto;

                    entity.persona_contacto = contacto;

                }
                else
                {
                    entity.persona_contacto = null;
                }
                //Agregar persona contacto #2 si la hay
                var contacto2 = entity.persona_contacto1 != null ? entity.persona_contacto1: new persona_contacto();
                if (model.nombreContacto2 != null && model.nombreContacto2 != string.Empty)
                {
                    contacto2.NOMBRE = model.nombreContacto2;
                    contacto2.EMAIL = model.correoContacto2;
                    contacto2.TELEFONO = model.telefonoContacto2;

                    entity.persona_contacto1 = contacto2;
                }
                else
                {
                    entity.persona_contacto1 = null;
                }

                //agregar direccion
                entity.direccion.CALLE = model.calle;
                entity.direccion.NUM_EXTERIOR = model.numExterior;
                entity.direccion.NUM_INTERIOR = model.numInterior;
                entity.direccion.COLONIA = model.colonia;
                entity.direccion.CIUDAD = model.ciudad;
                entity.direccion.MUNICIPIO = model.municipio;
                entity.direccion.ID_ESTADO = model.estado;
                entity.direccion.ID_PAIS = model.pais;
                entity.direccion.CP = model.codigoPostal;

                //datos de cuenta
                if (model.cuenta != null && model.cuenta != "")
                {
                    entity.cuenta = entity.cuenta == null ? new cuenta() : entity.cuenta;
                    entity.cuenta.NUMERO_CUENTA = model.cuenta;
                    entity.cuenta.ID_BANCO = model.banco;
                    entity.cuenta.TIPO = model.tipoCuenta;

                }
                else
                {
                    entity.cuenta = null;
                }

                if (nuevo)
                    db.cliente.Add(entity);

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
            }
            catch (Exception e)
            {
                rm.SetResponse(false, e.Message);
               // LogUtil.ExceptionLog(e);
            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }


        Boolean validarModificacion(ref ClienteModel model, ref cliente entity, ref string mensaje)
        {
            var rfc = model.rfc;
            var id = model.idCliente;
            if ((!model.esClienteCredito && entity.ES_CLIENTE_CREDITO) && entity.SALDO > 0)
            {
                mensaje += " No puede cambiar la forma de pago porque existe un adeudo";
                return false;
            }

            var clienteExistente = db.cliente.Where(m => m.ID != id).FirstOrDefault(m => m.RFC == rfc);
            if (clienteExistente!=null)
            {
                mensaje += "Ya existe un cliente con el rfc '" + rfc + "' <a href='/Clientes/ver?id=" + clienteExistente.ID + "'>Ver<a/>";

                return false;
            }

            return true;
        }
        Boolean validarInsersion(ref ClienteModel model, ref string mensaje)
        {
            var rfc = model.rfc;
            //validar rfc
            var clienteExistente = db.cliente.FirstOrDefault(m => m.RFC == rfc);
            if (clienteExistente != null && rfc!=null && rfc!="")
            {
                mensaje += "Ya existe un cliente con el rfc '" + rfc + "' <a href='/Clientes/ver?id=" + clienteExistente.ID + "'>Ver<a/>";

                return false;
            }




            return true;
        }
        public ActionResult BorrarCliente(int id = -1)
        {
            if (id > 0)
            {
                String errorMsg = string.Empty;
                ClienteDAO CLDAO = new ClienteDAO();
                bool result = CLDAO.DeleteById(id);

                if (!result)
                {
                    errorMsg = "Error al borrar cliente";
                }
            }
            return View(ABSOLUTE_PATH);
        }



        public ActionResult GetAlls()
        {
            List<clientes_v> clientesList = null;

            V_ClientesDAO CLDAO = new V_ClientesDAO();
            clientesList = CLDAO.GetAlls(db); /// obtener todos


            var jsonData = new
            {

                rows = (
                        from c in clientesList
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE_CLIENTE,
                            RFC = c.RFC,
                            CORREO = c.EMAIL,
                            TIPO = c.TIPO_PERSONA,
                            ACTIVO = c.ACTIVO

                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetEstadosByPais(int idPais)
        {
            List<estado> estadosLst = null;
            using (ArtexConnection db = new ArtexConnection())
            {
                estadosLst = EstadoDAO.GetByIdPais(idPais, db);
            }
            var resultado = new
            {
                rows = (
                        from c in estadosLst
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE
                        }).ToArray()
            };
            return Json(resultado.rows, JsonRequestBehavior.AllowGet);
        }



        private void CopiarEntidadModelo(ref ClienteModel model, cliente entity, ArtexConnection db)
        {
            try
            {
                model = new ClienteModel();
                model.idCliente = entity.ID;

                model.bancosList = BancoDAO.GetAlls();

                model.saldo = ExtensionMethods.ToMoneyFormat(entity.SALDO);
                model.creditoMaximo = ExtensionMethods.ToMoneyFormat(entity.MONTO_CREDITO);
                model.esClienteCredito = entity.ES_CLIENTE_CREDITO;
                model.rfc = entity.RFC;
                model.referencia = entity.REFERENCIA_BANCARIA;
                model.correo = entity.EMAIL;
                model.celular = entity.CELULAR;
                model.telefono = entity.TELEFONO;
                model.Activo = entity.ACTIVO;
                model.referencia2 = entity.REFERENCIA_BANCARIA_2;
                model.Clabe = entity.CLABE;


                if (entity.TIPO_PERSONA == "Fisica")
                {
                    model.esPersonaFisica = true;
                    model.nombrePersona = entity.NOMBRE;
                    model.apellidoPaterno = entity.APELLIDO_PATERNO;
                    model.apellidoMaterno = entity.APELLIDO_MATERNO;

                }
                else
                {
                    model.esPersonaFisica = false;
                    model.nombreEmpresa = entity.EMPRESA;
                    model.razonSocial = entity.RAZON_SOCIAL;

                }
                //datos de cuenta
                var cuenta = entity.cuenta;
                if (cuenta != null)
                {
                    model.cuenta = cuenta.NUMERO_CUENTA;
                    model.tipoCuenta = cuenta.TIPO;
                    model.banco = cuenta.ID_BANCO;
                }
                //datos de contacto
                var contacto = entity.persona_contacto;
                if (contacto != null)
                {
                    model.nombreContacto = contacto.NOMBRE;
                    model.apellidoPaternoContacto = contacto.APELLIDO_PATERNO;
                    model.apellidoMaternoContacto = contacto.APELLIDO_MATERNO;
                    model.correoContacto = contacto.EMAIL;
                    model.telefonoContacto = contacto.TELEFONO;
                }
                var contacto2 = entity.persona_contacto1;
                if (contacto2 != null)
                {
                    model.nombreContacto2 = contacto2.NOMBRE;
                    model.apellidoPaternoContacto2 = contacto2.APELLIDO_PATERNO;
                    model.apellidoMaternoContacto2 = contacto2.APELLIDO_MATERNO;
                    model.correoContacto2 = contacto2.EMAIL;
                    model.telefonoContacto2 = contacto2.TELEFONO;
                }

                //datos de direccion
                var direccion = entity.direccion;
                if (direccion != null)
                {
                    model.calle = direccion.CALLE;
                    model.numExterior = direccion.NUM_EXTERIOR;
                    model.numInterior = direccion.NUM_INTERIOR;
                    model.colonia = direccion.COLONIA;
                    model.ciudad = direccion.CIUDAD;
                    model.municipio = direccion.MUNICIPIO;
                    model.estado = direccion.ID_ESTADO;
                    model.pais = direccion.ID_PAIS;
                    model.paisList = PaisDAO.GetAlls(db);
                    model.estadoList = direccion.ID_PAIS != null ? EstadoDAO.GetByIdPais((int)direccion.ID_PAIS, db) : EstadoDAO.GetAlls(db);
                    model.codigoPostal = direccion.CP;
                }


            }
            catch (Exception e)
            {
                LogUtil.ExceptionLog(e);
                model = null;
            }
        }


        public JsonResult Delete(int id)
        {
            var rm = new ResponseModel();

            var entity = db.cliente.Find(id);
            if (entity != null)
            {
                entity.ACTIVO = false;

                if (db.SaveChanges() > 0)
                {
                    rm.result = true;
                    rm.message = "El registro  se elimino correctamente";
                } else if (db.Entry(entity).State == EntityState.Unchanged) {
                    rm.result = false;
                    rm.message = "El registro ya esta desactivado";
                }

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