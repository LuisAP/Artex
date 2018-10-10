using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using System.Data.Entity;

namespace Artex.Models.DAL.DAO
{
    public class PaisDAO
    {
        public static List<pais> GetAlls(ArtexConnection dbContext = null)
        {
            List<pais> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();
                
                    list = dbContext.pais.OrderBy(e => e.ID).ToList();
                
            }
            catch (Exception e)
            {

            }
            return list;
        }


        public pais GetById(int id, ArtexConnection dbContext = null)
        {
            pais consulta = null;

            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                consulta = dbContext.pais.Where(e => e.ID == id).FirstOrDefault();

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
                    var consulta = dbContext.pais.Where(m => m.ID == id).FirstOrDefault();
                    if (consulta != null)
                    {
                        dbContext.pais.Remove(consulta);

                        result = dbContext.SaveChanges() > 0 || dbContext.Entry(consulta).State == EntityState.Unchanged;

                    }
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }
    }
}