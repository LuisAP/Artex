using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using System.Data.Entity;

namespace Artex.Models.DAL.DAO
{
   /* public class AtributoProductoDAO
    {
        public List<atributos_configuracion> GetAlls(ArtexConnection dbContext = null)
        {
            List<atributos_configuracion> list = null;
            try
            {
                using (dbContext = dbContext != null ? dbContext : new ArtexConnection())
                {
                    list = dbContext.atributos_configuracion.OrderBy(e => e.ID).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return list;
        }

        public List<atributos_configuracion> GetActive(ArtexConnection dbContext = null)
        {
            List<atributos_configuracion> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                list = dbContext.atributos_configuracion.Where(m => m.ACTIVO == true).OrderBy(e => e.ID).ToList();

            }
            catch (Exception e)
            {

            }
            return list;
        }
        public atributos_configuracion GetById(int id, ArtexConnection dbContext = null)
        {
            atributos_configuracion consulta = null;

            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                consulta = dbContext.atributos_configuracion.Where(e => e.ID == id).FirstOrDefault();

            }
            catch (Exception e)
            {
            }

            return consulta;
        }


        public bool DeleteById(int id)
        {
            bool result = false;


            try
            {
                using (var dbContext = new ArtexConnection())
                {
                    var consulta = dbContext.atributos_configuracion.Where(m => m.ID == id).FirstOrDefault();
                    if (consulta != null)
                    {
                        consulta.ACTIVO = false;

                        result = dbContext.SaveChanges() > 0 || dbContext.Entry(consulta).State == EntityState.Unchanged;

                    }
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }
    }*/
}