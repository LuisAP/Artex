using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using System.Data.Entity;

namespace Artex.Models.DAL.DAO
{
    public class AreaTrabajoDAO
    {
        public static List<area_trabajo> GetAlls(ArtexConnection dbContext = null)
        {
            List<area_trabajo> list = null;
            try
            {
              dbContext = dbContext != null ? dbContext : new ArtexConnection();
                
                    list = dbContext.area_trabajo.OrderBy(e => e.ID).ToList();
                
            }
            catch (Exception e)
            {

            }
            return list;
        }
        public List<area_trabajo> GetActive(ArtexConnection dbContext = null)
        {
            List<area_trabajo> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                list = dbContext.area_trabajo.Where(m => m.ACTIVO == true).OrderBy(e => e.ID).ToList();

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public area_trabajo GetById(int id, ArtexConnection dbContext = null)
        {
            area_trabajo consulta = null;

            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                consulta = dbContext.area_trabajo.Where(e => e.ID == id).FirstOrDefault();

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
                    var consulta = dbContext.area_trabajo.Where(m => m.ID == id).FirstOrDefault();
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
    }
}