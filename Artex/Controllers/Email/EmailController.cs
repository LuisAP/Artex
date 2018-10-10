using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artex.DB;
using Artex.Models.DAL.DAO;
using Artex.Util;
using Artex.Util.Sistema;
using System.Data.Entity;
using System.Net.Mail;
using Artex.Models.ViewModels.Email;

namespace Artex.Controllers.Email
{
    public class EmailController : Controller
    {
        private const string ABSOLUTE_PATH = "~/Views/Email/Email.cshtml";

        public ActionResult Index()
        {
            
            return View(ABSOLUTE_PATH);
        }
        public JsonResult EnviarCorreo(ContactoViewModel model)
        {
            var rm = new ResponseModel();

            if (ModelState.IsValid)
            {
                try
                {
                    //var _usuario = usuario.Obtener(FrontOfficeStartUp.UsuarioVisualizando());

                    var mail = new MailMessage();
                    mail.From = new MailAddress(model.Correo, model.Nombre);
                    mail.To.Add("elihusantiago@gmail.com");
                    mail.Subject = "Correo desde contacto";
                    mail.IsBodyHtml = true;
                    mail.Body = model.Mensaje;

                    var SmtpServer = new SmtpClient("smtp.gmail.com"); // or "smtp.gmail.com"
                    SmtpServer.Port = 587;
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    SmtpServer.UseDefaultCredentials = false;

                    // Agrega tu correo y tu contraseña, hemos usado el servidor de Outlook.
                    SmtpServer.Credentials = new System.Net.NetworkCredential("esantiago@stratia.net", "l-u131190");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
                catch (Exception e)
                {
                    rm.SetResponse(false, e.Message);
                    return Json(rm);
                    throw;
                }

                rm.SetResponse(true);
                rm.function = "CerrarContacto();";
            }

            return Json(rm);
        }



    }
}