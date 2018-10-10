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
    public class ProveedoresController : Controller
    {
        private const string ABSOLUTE_PATH = "~/Views/Catalogos/Proveedores/ListaProveedores.cshtml";
        private const string VER_ABSOLUTE_PATH = "~/Views/Catalogos/Proveedores/CrearActualizarProveedor.cshtml";
        private const string CREAR_EDITAR_ABSOLUTE_PATH = "~/Views/Catalogos/Proveedores/CrearActualizarProveedor.cshtml";

        //
        // GET: /Cliente/
        public ActionResult Index()
        {
            PermisosModel model = PermisosModulo.ObtenerPermisos(Modulo.PROVEEDORES);
            if (model == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }

        //
        // GET: /CatalogoCliente/NuevoCliente/
        public ActionResult Create()
        {
            ProveedorModel model = null;
            try
            {
                using (ArtexConnection db = new ArtexConnection())
                {
                    model = new ProveedorModel();
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
            ProveedorModel model = null;
            try
            {
                using (ArtexConnection db = new ArtexConnection())
                {
                    ProveedorDAO CLDAO = new ProveedorDAO();
                    var entity = CLDAO.GetById(id, db);
                    if (entity != null)
                    {
                        model = new ProveedorModel();
                        CopiarEntidadModelo(ref model, entity, db);
                    }
                    ViewBag.Editar = false;
                    return View(VER_ABSOLUTE_PATH, model);
                }
            }
            catch (Exception e)
            {
                LogUtil.ExceptionLog(e);
            }
            return View(ABSOLUTE_PATH);
        }

        //
        public ActionResult Editar(int id = -1)
        {
            ProveedorModel model = null;
            if (id > 0)
            {
                try
                {
                    using (ArtexConnection db = new ArtexConnection())
                    {
                        ProveedorDAO CLDAO = new ProveedorDAO();
                        var entity = CLDAO.GetById(id, db);

                        if (entity != null)
                        {
                            model = new ProveedorModel();
                            CopiarEntidadModelo(ref model, entity, db);
                            ViewBag.Editar = true;
                            return View(CREAR_EDITAR_ABSOLUTE_PATH, model);
                        }
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
        public JsonResult Guardar(ProveedorModel model)
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
                try
                {

                    ProveedorDAO dao = new ProveedorDAO();
                    var entity = dao.GetById(model.id, db);
                    bool nuevo = false;




                    if (entity == null)
                    {
                        entity = new proveedor();
                        entity.direccion = new direccion();
                        nuevo = true;
                    }
                   

                    if (model.esPersonaFisica)
                    {
                        entity.TIPO_PERSONA = "Fisica";
                        entity.NOMBRE = model.nombrePersona;
                        entity.APELLIDO_PATERNO = model.apellidoPaterno;
                        entity.APELLIDO_MATERNO = model.apellidoMaterno;

                        entity.EMPRESA = null;
                        entity.RAZON_SOCIAL = null;
                    }
                    else
                    {
                        entity.TIPO_PERSONA = "Moral";
                        entity.EMPRESA = model.nombreEmpresa;
                        entity.RAZON_SOCIAL = model.razonSocial;

                        entity.NOMBRE = null;
                        entity.APELLIDO_PATERNO = null;
                        entity.APELLIDO_MATERNO = null;
                    }

                    entity.TELEFONO = model.telefono;
                    entity.CELULAR = model.celular;
                    entity.EMAIL = model.correo;
                    entity.RFC = model.rfc;
                    entity.TIEMPO_CREDITO = model.tiempoCrdito;
                    entity.TIEMPO_ENTREGA = model.tiempoEntrega;
                    entity.CALIFICACION = model.calificacion;
                    entity.GIRO = model.giro;
                    entity.ACTIVO = model.Activo;

                    entity.ES_DE_CREDITO = model.esDeCredito;
                    entity.SALDO = ExtensionMethods.ConverToDecimalFormat(model.saldo);
                    entity.MONTO_CREDITO = ExtensionMethods.ConverToDecimalFormat(model.creditoMaximo);
                    entity.ACTIVO = model.Activo;

                    //Agregar persona contacto si la hay
                    var contacto = entity.persona_contacto != null ? entity.persona_contacto : new persona_contacto();
                    if (model.nombreContacto != null && model.nombreContacto != string.Empty)
                    {
                        contacto.NOMBRE = model.nombreContacto;
                        contacto.APELLIDO_PATERNO = model.apellidoPaternoContacto;
                        contacto.APELLIDO_MATERNO = model.apellidoMaternoContacto;
                        contacto.EMAIL = model.correoContacto;
                        contacto.TELEFONO = model.telefonoContacto;

                        entity.persona_contacto = contacto;

                    }
                    else
                    {
                        entity.persona_contacto = null;
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
                        db.proveedor.Add(entity);


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
                    LogUtil.ExceptionLog(e);
                }
            }


            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteById(int id = -1)
        {
            if (id > 0)
            {
                String errorMsg = string.Empty;
                ProveedorDAO CLDAO = new ProveedorDAO();
                bool result = CLDAO.DeleteById(id);

                if (!result)
                {
                    errorMsg = "Error al borrar proveedor";
                }
            }
            return View(ABSOLUTE_PATH);
        }



        public ActionResult GetAlls()
        {
            List<proveedores_v> proveedorsList = null;
            using (ArtexConnection dbContext = new ArtexConnection())
            {



                V_ProveedoresDAO CLDAO = new V_ProveedoresDAO();
                proveedorsList = CLDAO.GetAlls(dbContext); /// obtener todos
            }

            var jsonData = new
            {

                rows = (
                        from c in proveedorsList
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

        #region metodos utilitarios


        private void CopiarEntidadModelo(ref ProveedorModel model, proveedor entity, ArtexConnection db)
        {
            try
            {
                model = new ProveedorModel();
                model.id = entity.ID;

                model.bancosList = BancoDAO.GetAlls();

                model.saldo = ExtensionMethods.ToMoneyFormat(entity.SALDO);
                model.creditoMaximo = ExtensionMethods.ToMoneyFormat(entity.MONTO_CREDITO);
                model.esDeCredito = entity.ES_DE_CREDITO;
                model.rfc = entity.RFC;
                model.correo = entity.EMAIL;
                model.celular = entity.CELULAR;
                model.telefono = entity.TELEFONO;
                model.tiempoEntrega = entity.TIEMPO_ENTREGA;
                model.tiempoCrdito = entity.TIEMPO_CREDITO;
                model.calificacion = entity.CALIFICACION;
                model.giro = entity.GIRO;
                model.Activo = entity.ACTIVO;


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


        #endregion
    }
}