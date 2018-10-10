using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using System.Data.Entity;

namespace Artex.Models.DAL.DAO
{
    public class ProcesoFabricacionDAO
    {
        public List<procesos_de_fabricacion> GetAlls(ArtexConnection dbContext = null)
        {
            List<procesos_de_fabricacion> list = null;
            try
            {
                using (dbContext = dbContext != null ? dbContext : new ArtexConnection())
                {
                    list = dbContext.procesos_de_fabricacion.OrderBy(e => e.ID).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return list;
        }
        public List<procesos_de_fabricacion> GetActive(ArtexConnection dbContext = null)
        {
            List<procesos_de_fabricacion> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                list = dbContext.procesos_de_fabricacion.Where(m => m.ACTIVO == true).OrderBy(e => e.ID).ToList();

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public procesos_de_fabricacion GetById(int id, ArtexConnection dbContext = null)
        {
            procesos_de_fabricacion consulta = null;

            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                consulta = dbContext.procesos_de_fabricacion.Where(e => e.ID == id).FirstOrDefault();

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
                    var consulta = dbContext.procesos_de_fabricacion.Where(m => m.ID == id).FirstOrDefault();
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