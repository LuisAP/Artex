using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using System.Data.Entity;

namespace Artex.Models.DAL.DAO
{
    public class UsuarioDAO
    {
        public static AspNetUsers GetUserLogged(ArtexConnection artexContext)
        {
            string userName = (HttpContext.Current.User.Identity.Name);
            return artexContext.AspNetUsers.Include("empleado").FirstOrDefault(m => m.UserName == userName);
        }



        public List<usuarios_v> GetAlls(ArtexConnection dbContext = null)
        {
            List<usuarios_v> list = null;
            dbContext = dbContext == null ? new ArtexConnection() : dbContext;
            try
            {

                list = dbContext.usuarios_v.OrderBy(e => e.ID).ToList();
            }
            catch (Exception e)
            {

            }
            return list;
        }
        public List<usuarios_v> GetActive(ArtexConnection dbContext = null)
        {
            List<usuarios_v> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                list = dbContext.usuarios_v.Where(m => m.ACTIVO == true).OrderBy(e => e.ID).ToList();

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public AspNetUsers GetById(String id, ArtexConnection db = null)
        {
            AspNetUsers evento = null;

            try
            {
                db = db != null ? db : new ArtexConnection();


                evento = db.AspNetUsers.Where(e => e.Id == id).FirstOrDefault();

            }
            catch (Exception e)
            {
            }

            return evento;
        }
        public usuarios_v GetById_v(String id, ArtexConnection db = null)
        {
            usuarios_v evento = null;

            try
            {
                db = db != null ? db : new ArtexConnection();


                evento = db.usuarios_v.Where(e => e.ID == id).FirstOrDefault();

            }
            catch (Exception e)
            {
            }

            return evento;
        }

        public bool DeleteById(String id)
        {
            bool result = false;

            try
            {
                ArtexConnection db = new ArtexConnection();
                AspNetUsers consulta = db.AspNetUsers.Where(m => m.Id == id).FirstOrDefault();
                if (consulta != null)
                {
                    consulta.LockoutEnabled = false;


                    result = db.SaveChanges() > 0 || db.Entry(consulta).State == EntityState.Unchanged;

                }
            }
            catch (Exception e)
            {

            }
            return result;
        }

    }
}