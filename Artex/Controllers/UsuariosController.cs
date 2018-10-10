using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artex.DB;
using Artex.Models.DAL.DAO;
using System.Text;
using System.Security.Cryptography;
using Artex.Models.ViewModels.RecursosHumanos;
using Artex.Models.ViewModels.Email;
using Artex.Util;
using Artex.Util.Sistema;
using System.Data.Entity;
using Artex.Util.Email;

namespace Artex.Controllers

{
    public class UsuariosController : Controller
    {
        private ArtexConnection db = new ArtexConnection();
        
        private const string ABSOLUTE_PATH = "~/Views/RecursosHumanos/Usuarios/ListaUsuarios.cshtml";
        private const string CREATE_PATH = "~/Views/RecursosHumanos/Usuarios/CrearUsuario.cshtml";
        private const string UPDATE_PATH = "~/Views/RecursosHumanos/Usuarios/EditarUsuario.cshtml";

        private const string VERYFY_CODE = "~/Views/RecursosHumanos/Usuarios/VerificarCodigo.cshtml";
        private const string RESET_PASS = "~/Views/RecursosHumanos/Usuarios/ResetPassword.cshtml";
        private const string SEND_CODE = "~/Views/RecursosHumanos/Usuarios/EnviarCodigo.cshtml";
        private const string CHANGE_PASS = "~/Views/RecursosHumanos/Usuarios/CambiarPassword.cshtml";


        public ActionResult Index()
        {
            PermisosModel model = PermisosModulo.ObtenerPermisos(Modulo.USUARIOS,db);
            if (model == null)
            {
                TempData["message"] = "danger,No tiene pemisos";
                return Redirect("~/Home");
            }
            return View(ABSOLUTE_PATH, model);
        }
        public ActionResult GetAlls()
        {

            UsuarioDAO userDAO = new UsuarioDAO();
            ArtexConnection artexContext = new ArtexConnection();
            List<usuarios_v> consulta = userDAO.GetAlls(artexContext);

            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID = c.ID,
                            USUARIO = c.USUARIO,
                            EMPLEADO = c.NOMBRE_COMPLETO,
                            ROL = c.ROL,
                            ID_ROL = c.ID_ROL,
                            ACTIVO = c.ACTIVO

                        }).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Crear()
        {
            if (!PermisosModulo.ObtenerPermiso(Modulo.USUARIOS, Permiso.CREAR,db))
            {
                TempData["message"] = "danger,No tiene permisos.";
                return RedirectToAction("Index");
            }
            UsuariosModel model = new UsuariosModel();
            try
            {

                model.Activo = true;
                model.esEmpleado = true;
                model.rolList = db.rol.Where(m => m.ACTIVO == true);
                model.personalList = new List<empleados_v>();



            }
            catch (Exception e)
            {
                LogUtil.ExceptionLog(e);
                model = null;
            }
            return View(CREATE_PATH, model);
        }

        public ActionResult editar(string id)
        {           
            AspNetUsers entity = db.AspNetUsers.Find(id);
            UpdateUsuariosModel model = new UpdateUsuariosModel();


            if (entity != null)
            {
                model.rolList = db.rol.Where(m => m.ACTIVO == true);
                model.personalList = null;

                model.Id = entity.Id;
                model.Email = entity.Email;
                model.Activo = entity.LockoutEnabled;
                model.rol = entity.RolId;
                model.personal = entity.PersonalId;
                model.UserName = entity.UserName;
                

                if (entity.persona_contacto!=null)
                {
                    model.esEmpleado = false;
                    model.nombre = entity.persona_contacto.NOMBRE;
                    model.apellidoMaterno = entity.persona_contacto.APELLIDO_MATERNO;
                    model.apellidoPaterno = entity.persona_contacto.APELLIDO_PATERNO;
                }
                else
                {
                    model.esEmpleado = true;
                    model.personalList = db.empleados_v.Where(m => m.ID_EMPLEADO == entity.PersonalId);
                }
                if (model.personalList == null)
                    model.personalList = new List<empleados_v>();
            }

            return View(UPDATE_PATH, model);

        }


        [HttpPost]
        public JsonResult Crear(UsuariosModel model)
        {
            var rm = new ResponseModel();
            if (!ModelState.IsValid)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }
            AspNetUsers entity = db.AspNetUsers.Find(model.Id);


            if (entity == null)
            {
                entity = new AspNetUsers();
                entity.Id = Token();
                entity.Email = model.Email;
                entity.PasswordHash = HashPassword(model.Password);
                entity.SecurityStamp = Guid.NewGuid().ToString();
                entity.LockoutEnabled = model.Activo;
                entity.UserName = model.UserName;
                entity.RolId = model.rol;
                entity.PersonalId = model.personal;


                entity.EmailConfirmed = false;
                entity.PhoneNumberConfirmed = false;
                entity.TwoFactorEnabled = false;
                entity.AccessFailedCount = 0;

                if (!model.esEmpleado)
                {
                    entity.persona_contacto = new persona_contacto();
                    entity.persona_contacto.NOMBRE = model.nombre;
                    entity.persona_contacto.APELLIDO_MATERNO = model.apellidoMaterno;
                    entity.persona_contacto.APELLIDO_PATERNO = model.apellidoPaterno;
                }
                db.AspNetUsers.Add(entity);
            }
            else
            {
                entity.Email = model.Email;
                
                entity.LockoutEnabled = model.Activo;
                entity.UserName = model.UserName;
                entity.RolId = model.rol;
            }

            if (db.SaveChanges() > 0 || db.Entry(entity).State == EntityState.Unchanged)

            {
                rm.response = true;
                //rm.message = "Sus datos se guardaron correctamente";
                rm.href = "Index";
                TempData["message"] = "success,Sus datos se guardaron correctamente";

            }



            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Actualizar(UpdateUsuariosModel model)
        {
            var rm = new ResponseModel();
            if (!ModelState.IsValid)
            {
                rm.message = "Hubo un problema verifique sus datos e intente de nuevo.";
                rm.message += ExtensionMethods.GetAllErrorsFromModelState(this);
                return Json(rm, JsonRequestBehavior.AllowGet);

            }
            AspNetUsers entity = db.AspNetUsers.Find(model.Id);


            if (entity != null)
            {
                entity.Email = model.Email;
                entity.LockoutEnabled = model.Activo;
                entity.RolId = model.rol;
                entity.PersonalId = model.personal;


                if (entity.persona_contacto!=null)
                {
                    entity.persona_contacto.NOMBRE = model.nombre;
                    entity.persona_contacto.APELLIDO_MATERNO = model.apellidoMaterno;
                    entity.persona_contacto.APELLIDO_PATERNO = model.apellidoPaterno;
                }
            }


            if (db.SaveChanges() > 0 || db.Entry(entity).State == EntityState.Unchanged)
            {
                rm.response = true;
                //rm.message = "Sus datos se guardaron correctamente";
                rm.href = "Index";
                TempData["message"] = "success,Sus datos se guardaron correctamente";

            }

            return Json(rm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Delete(String id)
        {
            bool success = false;
            string msj = "Hubo un problema verifique su conexion e intente de nuevo.";

            using (ArtexConnection db = new ArtexConnection())
            {
                UsuarioDAO userDAO = new UsuarioDAO();
                success = userDAO.DeleteById(id);

                if (success)
                {
                    msj = "El registro  se elimino correctamente";

                }
                var result = new
                {
                    Success = success,
                    msj = msj
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckUserValid(String user)
        {
            bool success = false;
            string msj = "El usuario "+user+" no esta disponible";

            using (ArtexConnection db = new ArtexConnection())
            {
                success = db.AspNetUsers.Any(x => x.UserName == user);

                var result = new
                {
                    Success = success,
                    msj = msj
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetRol()
        {
            RolDAO rol = new RolDAO();

            List<rol> consulta = rol.GetRoles();

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

        public ActionResult GetEmpleados()
        {
            ArtexConnection db = new ArtexConnection();


            HashSet<int> personalIds = new HashSet<int>(from x in db.AspNetUsers.Where(m => m.PersonalId!=null).Select(x =>(int) x.PersonalId) select x);
            IEnumerable<empleados_v> consulta = db.empleados_v;


            var jsonData = new
            {
                rows = (
                        from c in consulta
                        select new
                        {
                            ID_EMPLEADO = c.ID_EMPLEADO,
                            NOMBRE_COMPLETO = c.NOMBRE_COMPLETO
                     //   }).ToArray()
            }).Where(x => !personalIds.Contains(x.ID_EMPLEADO)).ToArray()
            };
            return Json(jsonData.rows, JsonRequestBehavior.AllowGet);
        }
        
        
        // POST: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View(SEND_CODE);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.AspNetUsers.FirstOrDefault(m => m.UserName == model.Email);
                if (user != null && user.LockoutEnabled)
                {
                    Random rnd = new Random();
                    user.VerificationCode = rnd.Next(100000, 999999).ToString();
                    db.SaveChanges();
                    // Enviar correo electrónico
                    EmailModel email = new EmailModel();
                    email.To = user.Email;
                    email.Subject = "Codigo de recuperacion";
                    email.Name = "Artex";
                    email.Message = "Codigo de recuperacion para la cuenta "
                        +user.UserName+ "\n Codigo: "+user.VerificationCode;

                    EmailUtil.EnviarMeil(email,db);
                   
                    return RedirectToAction("checkCode",new { id=user.Id});
                }

                

            }

            return View(SEND_CODE,model);
        }
        public ActionResult CheckCode(string id)
        {
            CheckCodeViewModel model = new CheckCodeViewModel();
               
                model.id = id;

                    return View(VERYFY_CODE,model);
    
        }
        public ActionResult ChangePass()
        {

            var user = UsuarioDAO.GetUserLogged(db);

            ChangePasswordViewModel resetModel = new ChangePasswordViewModel();
            resetModel.Id = user.Id;
            resetModel.Email = user.Email;
            resetModel.UserName = user.UserName;
            return View(CHANGE_PASS, resetModel);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePass(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(CHANGE_PASS, model);
            }
            var user = db.AspNetUsers.FirstOrDefault(m => m.Id == model.Id);
            if (user != null && user.LockoutEnabled)
            {
                user.Email = model.Email;
                user.PasswordHash = HashPassword(model.Password);
                user.VerificationCode = null;
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(CHANGE_PASS, model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CheckCode(CheckCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.AspNetUsers.FirstOrDefault(m => m.Id == model.id);
                if (user != null && user.LockoutEnabled && user.VerificationCode==model.Codigo)
                {
                    ResetPassViewModel resetModel = new ResetPassViewModel();
                    resetModel.id = user.Id;
                    resetModel.Codigo = user.VerificationCode;
                    return View(RESET_PASS,resetModel);
                }


            }

            return View(VERYFY_CODE, model);

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = db.AspNetUsers.FirstOrDefault(m => m.Id == model.id);
            if (user != null && user.LockoutEnabled && user.VerificationCode == model.Codigo)
            {
                user.PasswordHash = HashPassword(model.Password);
                user.VerificationCode = null;
                db.SaveChanges();
              
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
        //******************* Encriptacion y generacion de ids

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static string MD5(string word)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(word));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
        public static string Token()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray()) i *= ((int)b + 1);
            return MD5(string.Format("{0:x}", i - DateTime.Now.Ticks));
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