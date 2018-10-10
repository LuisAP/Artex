using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artex.Models.DAL.DAO;
using Artex.Models.DAL.DTO.Ventas;
using Artex.DB;
using Artex.Models.ViewModels.Catalogos;
using Artex.Models.ViewModels.Cotizacion;
using Artex.Util;
using Artex.Util.Sistema;
using System.Data.Entity;
using Artex.Util;
using Artex.Models.BLL.Productos;
using Artex.Models.BLL.Catalogos;
using Artex.Models.DAL.DTO.Catalogos;



namespace Artex.Controllers.Cotizacion
{
    public class CotizacionController : Controller
    {
        private const string ABSOLUTE_PATH = "~/Views/Cotizacion/Cotizaciones/Cotizaciones.cshtml";
        private const string CREAR_ABSOLUTE_PATH = "~/Views/Cotizacion/Cotizaciones/NuevaCotizacion.cshtml";
        private const string EDITAR_ABSOLUTE_PATH = "~/Views/Cotizacion/Cotizaciones/NuevaCotizacion.cshtml";
        private ArtexConnection db = new ArtexConnection();
        // GET: Cotizacion
        public ActionResult Index()
        {
            CotizacionModel model = new CotizacionModel();
            model.permisos = PermisosModulo.ObtenerPermisos(Modulo.TIENDAS);

            if (model.permisos == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult Crear()
        {
            if (!PermisosModulo.ObtenerPermiso(Modulo.COTIZACIONES, Permiso.CREAR))
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            CotizacionModel model = new CotizacionModel();

            var usuario = UsuarioDAO.GetUserLogged(db);

            var tienda = usuario.empleado != null && usuario.empleado.tienda.Count >0 ? usuario.empleado.tienda.First() : null;

            if (tienda == null)
            {
                TempData["message"] = "danger, El usuario no a sido asignado a ninguna tieda";
                return RedirectToAction("Index");
            }
            model.Nombre = usuario.empleado != null ? db.empleados_v.FirstOrDefault(m => m.ID_EMPLEADO == usuario.PersonalId).NOMBRE_COMPLETO : "";

            model.Tienda = tienda.NOMBRE;
                model.Fecha = ExtensionMethods.DateFormat(DateTime.Now);

            
            return View(CREAR_EDITAR_ABSOLUTE_PATH, model);
        }



        public ActionResult Editar(int id)
        {
            CotizacionModel model = new CotizacionModel();

            var entity = db.cotizacion.Where(m => m.ID == id).Include(m=> m.cotizacion_producto).FirstOrDefault();


            return View(EDITAR_ABSOLUTE_PATH, model);

        }

        #region Select by cotizacion
        public ActionResult GetCotizacion()
        {
            var consulta = db.cotizacion.OrderBy(e => e.ID).ToList();

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetcotizacionByid(int id = 0)
        {
            var c = db.cotizacion.Where(m => m.ID == id).FirstOrDefault();
            var jsonData = new
            {
                ID = c.ID,
                COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                TIENDA = c.tienda.NOMBRE,
                FECHA = c.FECHA_COTIZACION,
                ACTIVO = c.VIGENCIA,
                TOTAL = c.TOTAL,
                CLIENTE = c.cliente.NOMBRE + " " + c.cliente.APELLIDO_PATERNO + " " + c.cliente.APELLIDO_MATERNO + c.cliente.EMPRESA,
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult NuevoCliente(CotizacionModel model)
        {
            var rm = new ResponseModel();
            if (model.Nombrec == null || model.Apellido_pc == null)
            {
                rm.message = "Ingrese un Nombre y Apellido para continuar";
                rm.function = "reload(false)";
            }
            else
            {
                using (ArtexConnection db = new ArtexConnection())
                {
                    var entity = new cliente();
                    entity.direccion = new direccion();
                    entity.NOMBRE = model.Nombrec;
                    entity.APELLIDO_PATERNO = model.Apellido_pc;
                    entity.APELLIDO_MATERNO = model.Apellido_mc;
                    entity.TIPO_PERSONA = "Fisica";
                    entity.direccion.ID_PAIS = 1;
                    entity.ES_CLIENTE_CREDITO = true;
                    entity.ACTIVO = true;
                    entity.RFC = "XAXX010101000";
                    db.cliente.Add(entity);

                    if (db.SaveChanges() > 0)
                    {
                        rm.response = true;
                        rm.message = "Sus datos se guardaron correctamente";
                        rm.function = "reload(true,'" + rm.message + "','" + entity.ID + "')";

                    }

                }
            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region BUSCAR

        //NUEVO
        public ActionResult Getcliente()
        {
            var consulta = db.cliente.Where(m => m.ACTIVO == true).OrderBy(e => e.NOMBRE).ToList();

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE + " " + c.APELLIDO_PATERNO + " " + c.APELLIDO_MATERNO + " " + c.EMPRESA,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Getvendedor()
        {
            var consulta = db.empleado.Where(m => m.PUESTO == "vendedor").OrderBy(m => m.ID_PERSONA_FISICA).ToList();

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.persona_fisica.NOMBRE + " " + c.persona_fisica.APELLIDO_PATERNO + " " + c.persona_fisica.APELLIDO_PATERNO,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAlls()
        {
            var consulta = db.cotizacion;
            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            COTIZACION = c.ID,
                            TIENDA = c.tienda.NOMBRE,
                            FECHA = c.FECHA_COTIZACION,
                            ACTIVO = c.VIGENCIA,

                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        /*BUSCAR*/
        public class buscar
        {
            public int vendedor { get; set; }
            public int cliente { get; set; }
            //public string fecha { get; set; }
            //public string fechaF { get; set; }
        }
        [HttpPost]
        public JsonResult BuscarC(buscar Buscar, string fecha = "", string fechaF = "")
        {
            var rm = new ResponseModel();
            rm.result = true;
            rm.message = "OK";

            if (fechaF == "")
            {
                if (Buscar.vendedor > 0 && Buscar.cliente > 0)
                {
                    var consulta = db.cotizacion.Where(m => (m.ID_VENDEDOR == Buscar.vendedor) && (m.ID_CLIENTE == Buscar.cliente));
                    if (consulta.Count() > 0)
                    {
                        var jsonData = new
                        {
                            result = true,
                            response = true,
                            message = "NULL",

                            rows = (
                               from c in consulta
                               select new
                               {
                                   ID = c.ID,
                                   COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                                   TIENDA = c.tienda.NOMBRE,
                                   FECHA = c.FECHA_COTIZACION,
                                   ACTIVO = c.VIGENCIA,

                               }).ToArray()
                        };

                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        rm.result = false;
                        rm.response = true;
                        rm.message = "No se encontro ninguna cotizacion con los parametros establecidos";

                    }
                }
                else if (Buscar.vendedor > 0 && Buscar.cliente <= 0)
                {
                    var consulta = db.cotizacion.Where(m => (m.ID_VENDEDOR == Buscar.vendedor));
                    if (consulta.Count() > 0)
                    {
                        var jsonData = new
                        {
                            result = true,
                            response = true,
                            message = "NULL",

                            rows = (
                               from c in consulta
                               select new
                               {
                                   ID = c.ID,
                                   COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                                   TIENDA = c.tienda.NOMBRE,
                                   FECHA = c.FECHA_COTIZACION,
                                   ACTIVO = c.VIGENCIA,

                               }).ToArray()
                        };

                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        rm.result = false;
                        rm.response = true;
                        rm.message = "No se encontro ninguna cotizacion con los parametros establecidos";

                    }
                }
                else
                {
                    if (Buscar.vendedor <= 0 && Buscar.cliente > 0)
                    {
                        var consulta = db.cotizacion.Where(m => (m.ID_CLIENTE == Buscar.cliente));
                        if (consulta.Count() > 0)
                        {
                            var jsonData = new
                            {
                                result = true,
                                response = true,
                                message = "NULL",

                                rows = (
                                   from c in consulta
                                   select new
                                   {
                                       ID = c.ID,
                                       COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                                       TIENDA = c.tienda.NOMBRE,
                                       FECHA = c.FECHA_COTIZACION,
                                       ACTIVO = c.VIGENCIA,

                                   }).ToArray()
                            };

                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            rm.result = false;
                            rm.response = true;
                            rm.message = "No se encontro ninguna cotizacion con los parametros establecidos";

                        }
                    }
                }
                return Json(rm, JsonRequestBehavior.AllowGet);

            }
            else
            {

                DateTime FechaI = DateTime.Parse(fecha);
                DateTime FechaF = DateTime.Parse(fechaF);
                if (Buscar.vendedor > 0 && Buscar.cliente > 0)
                {
                    var consulta = db.cotizacion.Where(m => (m.ID_VENDEDOR == Buscar.vendedor) && (m.ID_CLIENTE == Buscar.cliente) && (m.FECHA_COTIZACION >= FechaI && m.FECHA_COTIZACION <= FechaF));
                    if (consulta.Count() > 0)
                    {
                        var jsonData = new
                        {
                            result = true,
                            response = true,
                            message = "NULL",

                            rows = (
                               from c in consulta
                               select new
                               {
                                   ID = c.ID,
                                   COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                                   TIENDA = c.tienda.NOMBRE,
                                   FECHA = c.FECHA_COTIZACION,
                                   ACTIVO = c.VIGENCIA,

                               }).ToArray()
                        };

                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        rm.result = false;
                        rm.response = true;
                        rm.message = "No se encontro ninguna cotizacion con los parametros establecidos";

                    }
                }
                else if (Buscar.vendedor > 0 && Buscar.cliente <= 0)
                {
                    var consulta = db.cotizacion.Where(m => (m.ID_VENDEDOR == Buscar.vendedor) && (m.FECHA_COTIZACION >= FechaI && m.FECHA_COTIZACION <= FechaF));
                    if (consulta.Count() > 0)
                    {
                        var jsonData = new
                        {
                            result = true,
                            response = true,
                            message = "NULL",

                            rows = (
                               from c in consulta
                               select new
                               {
                                   ID = c.ID,
                                   COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                                   TIENDA = c.tienda.NOMBRE,
                                   FECHA = c.FECHA_COTIZACION,
                                   ACTIVO = c.VIGENCIA,

                               }).ToArray()
                        };

                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        rm.result = false;
                        rm.response = true;
                        rm.message = "No se encontro ninguna cotizacion con los parametros establecidos";

                    }
                }
                else if (Buscar.vendedor <= 0 && Buscar.cliente > 0)
                {
                    var consulta = db.cotizacion.Where(m => (m.ID_CLIENTE == Buscar.cliente) && (m.FECHA_COTIZACION >= FechaI && m.FECHA_COTIZACION <= FechaF));
                    if (consulta.Count() > 0)
                    {
                        var jsonData = new
                        {
                            result = true,
                            response = true,
                            message = "NULL",

                            rows = (
                               from c in consulta
                               select new
                               {
                                   ID = c.ID,
                                   COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                                   TIENDA = c.tienda.NOMBRE,
                                   FECHA = c.FECHA_COTIZACION,
                                   ACTIVO = c.VIGENCIA,

                               }).ToArray()
                        };

                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        rm.result = false;
                        rm.response = true;
                        rm.message = "No se encontro ninguna cotizacion con los parametros establecidos";

                    }
                }
                else
                {
                    var consulta = db.cotizacion.Where(m => (m.FECHA_COTIZACION >= FechaI && m.FECHA_COTIZACION <= FechaF));
                    if (consulta.Count() > 0)
                    {
                        var jsonData = new
                        {
                            result = true,
                            response = true,
                            message = "NULL",

                            rows = (
                               from c in consulta
                               select new
                               {
                                   ID = c.ID,
                                   COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                                   TIENDA = c.tienda.NOMBRE,
                                   FECHA = c.FECHA_COTIZACION,
                                   ACTIVO = c.VIGENCIA,

                               }).ToArray()
                        };

                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        rm.result = false;
                        rm.response = true;
                        rm.message = "No se encontro ninguna cotizacion con los parametros establecidos";

                    }
                }

                // || (m.FECHA_COTIZACION >= FechaI && m.FECHA_COTIZACION <= FechaF)
                return Json(rm, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult Getdatos(int id)
        {

            var consulta = db.cotizacion.Where(m => m.ID == id).FirstOrDefault();

            var jsnResult = new
            {
                VENDEDOR = consulta.empleado.persona_fisica.NOMBRE + " " + consulta.empleado.persona_fisica.APELLIDO_PATERNO + " " + consulta.empleado.persona_fisica.APELLIDO_MATERNO,
                CLIENTE = consulta.cliente.NOMBRE + " " + consulta.cliente.APELLIDO_PATERNO + " " + consulta.cliente.APELLIDO_MATERNO + " " + consulta.cliente.EMPRESA,
                TIENDA = consulta.tienda.NOMBRE,
                CORREO = consulta.cliente.EMAIL,
                CELULAR = consulta.cliente.TELEFONO,
                ID = consulta.ID,
                Success = true
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Getproductos(int id)
        {

            var consulta = db.cotizacion_producto.Where(m => m.ID_COTIZACION == id).Distinct();
            var suma = db.cotizacion_producto.Where(m => m.ID_COTIZACION == id).Sum(m => m.PRECIO);

            var jsnResult = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            PRODUCTO = c.producto.NOMBRE,
                            CODIGO = c.producto.CODIGO,
                            CANTIDAD = c.CANTIDAD,
                          // PRECIOUNITARIO = c.producto.PRECIO_RETAIL,
                           // SUBTOTAL = c.CANTIDAD * c.producto.PRECIO_RETAIL
                        }).ToArray()

            };

            return Json(jsnResult.rows, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BuscarD(int vendedor = 0, int cliente = 0, string fecha = "", string fechaF = "")
        {
            var rm = new ResponseModel();
            rm.result = true;
            rm.message = "OK";


            if (fechaF == "")
            {
                if (vendedor > 0 && cliente > 0)
                {
                    var consulta = db.cotizacion.Where(m => (m.ID_VENDEDOR == vendedor) && (m.ID_CLIENTE == cliente));


                    var jsonData = new
                    {
                        result = true,
                        response = true,
                        message = "NULL",

                        rows = (
                           from c in consulta
                           select new
                           {
                               ID = c.ID,
                               COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                               TIENDA = c.tienda.NOMBRE,
                               FECHA = c.FECHA_COTIZACION,
                               ACTIVO = c.VIGENCIA,
                               TOTAL = c.TOTAL,
                               CLIENTE = c.cliente.NOMBRE + " " + c.cliente.APELLIDO_PATERNO + " " + c.cliente.APELLIDO_MATERNO + c.cliente.EMPRESA,

                           }).ToArray()
                    };

                    return Json(jsonData.rows, JsonRequestBehavior.AllowGet);

                }
                else if (vendedor > 0 && cliente <= 0)
                {
                    var consulta = db.cotizacion.Where(m => (m.ID_VENDEDOR == vendedor));

                    var jsonData = new
                    {
                        result = true,
                        response = true,
                        message = "NULL",

                        rows = (
                           from c in consulta
                           select new
                           {
                               ID = c.ID,
                               COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                               TIENDA = c.tienda.NOMBRE,
                               FECHA = c.FECHA_COTIZACION,
                               ACTIVO = c.VIGENCIA,
                               TOTAL = c.TOTAL,
                               CLIENTE = c.cliente.NOMBRE + " " + c.cliente.APELLIDO_PATERNO + " " + c.cliente.APELLIDO_MATERNO + c.cliente.EMPRESA,

                           }).ToArray()
                    };

                    return Json(jsonData.rows, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    if (vendedor <= 0 && cliente > 0)
                    {
                        var consulta = db.cotizacion.Where(m => (m.ID_CLIENTE == cliente));

                        var jsonData = new
                        {
                            result = true,
                            response = true,
                            message = "NULL",

                            rows = (
                               from c in consulta
                               select new
                               {
                                   ID = c.ID,
                                   COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                                   TIENDA = c.tienda.NOMBRE,
                                   FECHA = c.FECHA_COTIZACION,
                                   ACTIVO = c.VIGENCIA,
                                   TOTAL = c.TOTAL,
                                   CLIENTE = c.cliente.NOMBRE + " " + c.cliente.APELLIDO_PATERNO + " " + c.cliente.APELLIDO_MATERNO + c.cliente.EMPRESA,

                               }).ToArray()
                        };

                        return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(rm, JsonRequestBehavior.AllowGet);
            }
            else
            {
                DateTime FechaI = DateTime.Parse(fecha);
                DateTime FechaF = DateTime.Parse(fechaF);
                if (vendedor > 0 && cliente > 0)
                {
                    var consulta = db.cotizacion.Where(m => (m.ID_VENDEDOR == vendedor) && (m.ID_CLIENTE == cliente) && (m.FECHA_COTIZACION >= FechaI && m.FECHA_COTIZACION <= FechaF));
                    if (consulta.Count() > 0)
                    {
                        var jsonData = new
                        {
                            result = true,
                            response = true,
                            message = "NULL",

                            rows = (
                               from c in consulta
                               select new
                               {
                                   ID = c.ID,
                                   COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                                   TIENDA = c.tienda.NOMBRE,
                                   FECHA = c.FECHA_COTIZACION,
                                   ACTIVO = c.VIGENCIA,
                                   TOTAL = c.TOTAL,
                                   CLIENTE = c.cliente.NOMBRE + " " + c.cliente.APELLIDO_PATERNO + " " + c.cliente.APELLIDO_MATERNO + c.cliente.EMPRESA,

                               }).ToArray()
                        };

                        return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        rm.result = false;
                        rm.response = true;
                        rm.message = "No se encontro ninguna cotizacion con los parametros establecidos";

                    }
                }
                else if (vendedor > 0 && cliente <= 0)
                {
                    var consulta = db.cotizacion.Where(m => (m.ID_VENDEDOR == vendedor) && (m.FECHA_COTIZACION >= FechaI && m.FECHA_COTIZACION <= FechaF));
                    if (consulta.Count() > 0)
                    {
                        var jsonData = new
                        {
                            result = true,
                            response = true,
                            message = "NULL",

                            rows = (
                               from c in consulta
                               select new
                               {
                                   ID = c.ID,
                                   COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                                   TIENDA = c.tienda.NOMBRE,
                                   FECHA = c.FECHA_COTIZACION,
                                   ACTIVO = c.VIGENCIA,
                                   TOTAL = c.TOTAL,
                                   CLIENTE = c.cliente.NOMBRE + " " + c.cliente.APELLIDO_PATERNO + " " + c.cliente.APELLIDO_MATERNO + c.cliente.EMPRESA,

                               }).ToArray()
                        };

                        return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        rm.result = false;
                        rm.response = true;
                        rm.message = "No se encontro ninguna cotizacion con los parametros establecidos";

                    }
                }
                else if (vendedor <= 0 && cliente > 0)
                {
                    var consulta = db.cotizacion.Where(m => (m.ID_CLIENTE == cliente) && (m.FECHA_COTIZACION >= FechaI && m.FECHA_COTIZACION <= FechaF));

                    var jsonData = new
                    {
                        result = true,
                        response = true,
                        message = "NULL",

                        rows = (
                           from c in consulta
                           select new
                           {
                               ID = c.ID,
                               COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                               TIENDA = c.tienda.NOMBRE,
                               FECHA = c.FECHA_COTIZACION,
                               ACTIVO = c.VIGENCIA,
                               TOTAL = c.TOTAL,
                               CLIENTE = c.cliente.NOMBRE + " " + c.cliente.APELLIDO_PATERNO + " " + c.cliente.APELLIDO_MATERNO + c.cliente.EMPRESA,

                           }).ToArray()
                    };

                    return Json(jsonData, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var consulta = db.cotizacion.Where(m => (m.FECHA_COTIZACION >= FechaI && m.FECHA_COTIZACION <= FechaF));

                    var jsonData = new
                    {
                        result = true,
                        response = true,
                        message = "NULL",

                        rows = (
                           from c in consulta
                           select new
                           {
                               ID = c.ID,
                               COTIZACION = "ARTEX-" + c.tienda.NOMBRE + "-COT-00" + c.ID,
                               TIENDA = c.tienda.NOMBRE,
                               FECHA = c.FECHA_COTIZACION,
                               ACTIVO = c.VIGENCIA,
                               TOTAL = c.TOTAL,
                               CLIENTE = c.cliente.NOMBRE + " " + c.cliente.APELLIDO_PATERNO + " " + c.cliente.APELLIDO_MATERNO + c.cliente.EMPRESA,

                           }).ToArray()
                    };

                    return Json(jsonData.rows, JsonRequestBehavior.AllowGet);

                }

                // || (m.FECHA_COTIZACION >= FechaI && m.FECHA_COTIZACION <= FechaF)
                return Json(rm, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(int id)
        {
            var rm = new ResponseModel();
            var consulta = db.cotizacion.Where(m => m.ID == id).FirstOrDefault();
            if (consulta != null)
            {
                db.cotizacion.Remove(consulta);
                rm.result = db.SaveChanges() > 0;
                if (rm.result)
                {
                    rm.message = "registro eliminado exitosamente";
                    rm.response = true;
                }
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult getdatacliente(int id)
        {
            ClienteDAO dao = new ClienteDAO();
            cliente c = dao.GetById(id);

            DireccionDAO daod = new DireccionDAO();
            direccion d = daod.GetById(Convert.ToInt32(c.ID_DIRECCION_ENTREGA));

            var jsnResult = new
            {
                CIUDAD = d.CIUDAD,
                CALLE = d.CALLE,
                CP = d.CP,
                NUM_EXT = d.NUM_EXTERIOR,
                CORREO = c.CELULAR,
                CELULAR = c.EMAIL,
                NOMBRE = c.NOMBRE + " " + c.APELLIDO_PATERNO + " " + c.APELLIDO_MATERNO + " " + c.EMPRESA
            };

            return Json(jsnResult, JsonRequestBehavior.AllowGet);
        }
        #endregion
     
        #region Agregar Productos
        public ActionResult GetPrograma()
        {
            var consulta = db.tipo_programa.Where(m => m.ACTIVO);

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            NOMBRE = c.NOMBRE
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPiso(String nombre = "", string codigo = "")
        {
            int idTienda = 1;
            Boolean mayoreo = false;

            var consulta = db.stock_tienda_v.Where(m => m.ID_TIENDA == idTienda);

            if (nombre != "")
            {
                consulta = consulta.Where(m => m.PRODUCTO.Contains(nombre));
            }
            if (codigo != "")
            {
                consulta = consulta.Where(m => m.CODIGO_CONFIGURADO.Contains(codigo));

            }

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            c.ID_PRODUCTO,
                            PRODUCTO = c.PRODUCTO,
                            c.DESCRIPCION,
                            CODIGO = c.CODIGO_CONFIGURADO,
                            CANTIDAD = c.DISPONIBLES,
                            PRECIO = 0,// mayoreo ? c.PRECIO_MAYOREO : c.PRECIO_RETAIL,
                            DESCUENTO_MAX = 0,// mayoreo ? c.DESCUENTO_CADENAS : c.DESCUENTO_RETAIL,
                            FOTO = "/Content/img/productos/" + c.FOTO,
                            PROGRAMA = "piso",
                            CONFIGURABLE= false,
                            CONFIGURADO = false,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetIntranet(String nombre = "", string codigo = "")
        {
            int idTienda = 1;
            Boolean mayoreo = false;

            var consulta = db.stock_intranet_v.Where(m => m.DISPONIBLES > 0);

            if (nombre != "")
            {
                consulta = consulta.Where(m => m.PRODUCTO.Contains(nombre));
            }
            if (codigo != "")
            {
                consulta = consulta.Where(m => m.CODIGO_CONFIGURADO.Contains(codigo));

            }

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            c.ID_PRODUCTO,
                            PRODUCTO = c.PRODUCTO,
                            c.DESCRIPCION,
                            CODIGO = c.CODIGO_CONFIGURADO,
                            CANTIDAD = c.DISPONIBLES,
                            PRECIO =0,// mayoreo ? c.PRECIO_MAYOREO : c.PRECIO_RETAIL,
                            DESCUENTO_MAX = 0,//mayoreo ? c.DESCUENTO_CADENAS : c.DESCUENTO_RETAIL,
                            FOTO = "/Content/img/productos/" + c.FOTO,
                            PROGRAMA = "intranet",
                            CONFIGURABLE = false,
                            CONFIGURADO = false,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCatalogoProductos(int uNegocio, String nombre = "", string codigo = "")
        {
            String ListaPrecios = "POP";


            var consulta = db.producto_v.Where(m => m.ID_STATUS_SKU==2).Where(m => m.ID_UNIDAD_NEGOCIO == uNegocio);

            if (nombre != "")
            {
                consulta = consulta.Where(m => m.NOMBRE.Contains(nombre));
            }
            if (codigo != "")
            {
                consulta = consulta.Where(m => m.CODIGO.Contains(codigo));

            }

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID_PRODUCTO,
                            c.ID_PRODUCTO,
                            PRODUCTO = c.NOMBRE,
                            c.DESCRIPCION,
                            CODIGO = c.CODIGO,
                            CANTIDAD = 0,
                            PRECIO =c.PRECIO_POP,// mayoreo ? c.PRECIO_MAYOREO : c.PRECIO_RETAIL,
                            DESCUENTO_MAX = 10,//mayoreo ? c.DESCUENTO_CADENAS : c.DESCUENTO_RETAIL,
                            FOTO = "/Content/img/productos/" + c.FOTO,
                            PROGRAMA = uNegocio == 1 ? "fabricacion" : "externo",
                            CONFIGURABLE = c.ES_PERSONALIZABLE,
                            CONFIGURADO = false,
                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Configuracion

        public ActionResult GetConfiguracion(int idProducto, string codigo = "")
        {
            ConfiguracionesDTO model = new ConfiguracionesDTO();
            var configuracion = db.formulacion_comodin.Where(m => m.ID_PRODUCTO == idProducto).Include(m => m.atributo_subatributo);
            var carasDistintas = configuracion.Select(m => m.ID_PIEZA).Distinct().ToList();
            model.ListaCaras = new List<caraDTO>();
            model.idProducto = idProducto;

            string[] ArrayCode = { };
            string[] ArrayM = { };
            string[] ArrayT = { };
            string[] ArrayA = { };
            Boolean configurado = false;

            try
            {


                if (codigo != "")
                {
                    var selecionados = new atributoSeleccionadosDTO();
                    configurado = true;
                    ArrayCode = codigo.Split('/');
                    ArrayM = ArrayCode[0].Split('M').Where(m => m != "").ToArray();
                    ArrayT = ArrayCode[1].Split('T').Where(m => m != "").ToArray();
                    ArrayA = ArrayCode[2].Split('A').Where(m=> m!="").ToArray();

                }


                int numCara = 0;
                foreach (int idCara in carasDistintas)
                {
                    caraDTO cara = new caraDTO();
                    cara.ListaComodines = new List<comodinDTO>();
                    cara.idProducto = idProducto;
                    cara.idCara = idCara;
                    cara.cara = db.piezas_configurables.FirstOrDefault(m => m.ID == idCara).NOMBRE;

                    var comodinesporCara = configuracion.Where(m => m.ID_PIEZA == idCara);

                    foreach (formulacion_comodin comodin in comodinesporCara)
                    {
                        var atributoByComodin = comodin.atributo_subatributo;
                        if (atributoByComodin.Count > 0)
                        {
                            comodinDTO cmdnDTO = new comodinDTO();
                            cmdnDTO.listaCombos = new List<combosDTO>();
                            cmdnDTO.idFormulacion = comodin.ID;
                            cmdnDTO.idAtributo = comodin.ID_COMODIN;
                            cmdnDTO.startLevel = atributoByComodin.FirstOrDefault(m => m.ID == comodin.ID_COMODIN).NIVEL;
                            cmdnDTO.endLevel = atributoByComodin.Max(m => m.NIVEL);

                            var atributoIncial = atributoByComodin.FirstOrDefault(m => m.NIVEL == cmdnDTO.startLevel); //indica el contenido del primer combo


                            var CodigotipoAtributo = atributoByComodin.FirstOrDefault(m => m.NIVEL == 0).CODIGO;

                            int? idPadre = null;
                            int numCombo = 0;

                            for (int i = 0; i < cmdnDTO.endLevel; i++)
                            {
                                int nivel = i + 1;
                                combosDTO combo = new combosDTO();
                                combo.nombre = atributoIncial.NOMBRE;
                                combo.nivel = nivel;
                                combo.numCmbo = numCombo;
                                if (i == cmdnDTO.endLevel - 1)
                                    combo.ultimoCmbo = true;

                                if (configurado)
                                {
                                    String cod = "";
                                    switch (CodigotipoAtributo)
                                    {
                                        case "M":
                                            if (i == 0)
                                                idPadre = 2;
                                            cod = ArrayM[numCara].Substring(i * 2, 2);
                                            break;
                                        case "T":
                                            if (i == 0)
                                                idPadre = 3;
                                            if (i == 2)
                                                cod = ArrayT[numCara].Substring(i * 2, 3);
                                            else
                                                cod = ArrayT[numCara].Substring(i * 2, 2);

                                            break;
                                        case "A":
                                            if (i == 0)
                                                idPadre = 4;
                                            cod = ArrayA[numCara].Substring(i * 2, 2);
                                            break;
                                    }

                                    combo.seleccionado = atributoByComodin.FirstOrDefault(m => m.NIVEL == nivel && m.CODIGO == cod && m.ID_PADRE == idPadre).ID;
                                    idPadre = combo.seleccionado;

                                }

                                if (i >= cmdnDTO.startLevel)
                                {
                                    cmdnDTO.listaCombos.Add(combo);
                                    numCombo++;
                                }
                            }


                            cara.ListaComodines.Add(cmdnDTO);

                        }
                    }
                    model.ListaCaras.Add(cara);
                    numCara++;
                }
            }
            catch (Exception e)
            {
                var err = e;
            }
            return PartialView("~/Views/Cotizacion/Cotizaciones/Partials/_SeleccionarConfiguracion.cshtml", model);
            //return Json(ListaCaras, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOpcionesByCombo(int idFormulacion, int numCmbo, int nivel)
        {
            var comodin = db.formulacion_comodin.FirstOrDefault(m => m.ID == idFormulacion);

            var lista = comodin.atributo_subatributo.Where(m => m.NIVEL == nivel);


            if (numCmbo == 0)
            {
                var jsonResult = new { rows = (from c in lista select new { COMBO0_ID = c.ID, c.NOMBRE,  c.ID_PADRE, c.CODIGO }).ToArray() };
                return Json(jsonResult.rows, JsonRequestBehavior.AllowGet);
            }
            else if (numCmbo == 1)
            {
                var jsonResult = new { rows = (from c in lista select new { COMBO1_ID = c.ID, c.NOMBRE, COMBO0_ID = c.ID_PADRE, c.CODIGO }).ToArray() };
                return Json(jsonResult.rows, JsonRequestBehavior.AllowGet);

            }
            else if (numCmbo == 2)
            {
                var jsonResult = new  {  rows = (from c in lista  select new { COMBO2_ID = c.ID, c.NOMBRE, COMBO1_ID = c.ID_PADRE,  c.CODIGO }).ToArray() };
                return Json(jsonResult.rows, JsonRequestBehavior.AllowGet);
            }
            else if (numCmbo == 3)
            {
                var jsonResult = new { rows = (from c in lista select new { COMBO3_ID = c.ID, c.NOMBRE, COMBO2_ID = c.ID_PADRE, c.CODIGO }).ToArray() };
                return Json(jsonResult.rows, JsonRequestBehavior.AllowGet);
            }



            return Json("[]",JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GenerarCodigo(SeleccionProductoDTO selectProd)
        {
            AtributosBLL atributosBLL = new AtributosBLL();

            var configuracion = db.formulacion_comodin.Where(m => m.ID_PRODUCTO == selectProd.idProducto).Include(m => m.atributo_subatributo);
            String M = "";
            String T = "";
            String A = "";
            String code = "";

            String descriptM = "";
            String descriptT = "";
            String descriptA = "";
            String descript = "";
            foreach (atributoSeleccionadosDTO s in selectProd.listaseleccionados)
            {
                var comodin = configuracion.FirstOrDefault(m => m.ID == s.idFormulacion);

                String temp = atributosBLL.GetCode(ref comodin, s.idSeleccionado);
                String descripcion= atributosBLL.GetDescripcionConfig(ref comodin, s.idSeleccionado);
                char tipoAtributo = temp[0];

                switch (tipoAtributo)
                {
                    case 'M':
                        M += temp;
                        descriptM += descripcion;
                        break;
                    case 'T':
                        T += temp;
                        descriptT += descripcion;
                        break;
                    case 'A':
                        A += temp;
                        descriptA += descripcion;
                        break;
                }
            }

            code = M == "" ? "M0" : M;
            code += T == "" ? "/T0" : "/"+T;
            code += A == "" ? "/A0" : "/" + A;

            descript = descriptM + descriptT + descriptA;

            var jsonResponse = new { code = code, descripcion = descript, precio=0 };

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult guardaCotizacion(int cliente,string total, List<ListaProductosDTO> productos)
        {
            var entity = new cotizacion();
                var user = UsuarioDAO.GetUserLogged(db);
            try
            {
             

                entity.ID_CLIENTE = cliente;
                entity.ID_TIENDA = user.empleado.tienda.FirstOrDefault().ID;
                entity.ID_VENDEDOR = user.empleado.ID;
                entity.FECHA_COTIZACION = DateTime.Now;
                entity.TOTAL = ExtensionMethods.ConverToDecimalFormat(total);
                entity.VIGENCIA = DateTime.Now.AddDays(15);
                entity.ID_CANAL_VENTA = 1;

                entity.cotizacion_producto = new List<cotizacion_producto>();

                foreach (ListaProductosDTO prod in productos)
                {
                    var p = new cotizacion_producto();
                    decimal precio = ExtensionMethods.ConverToDecimalFormat(prod.precio);
                    float descuento = (float)ExtensionMethods.ConverToDecimalFormat(prod.descuento);

                    p.ID_PRODUCTO = prod.idProducto;
                    p.CODIGO = prod.codigoConfig;
                    p.CANTIDAD = prod.cantidad;
                    p.PRECIO = precio;
                    p.DESCUENTO_UNITARIO = descuento;
                    p.IMPORTE = prod.cantidad * (precio - (precio * (decimal)descuento / 100));
   
                    p.ORIGEN_DEL_PRODUCTO = prod.origen;
                    p.ID_ORIGEN_DEL_PRODUCTO = prod.idByOrigen;
                    entity.cotizacion_producto.Add(p);
                }

                db.cotizacion.Add(entity);

                db.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("GetcotizacionByid", new { id=entity.ID});
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