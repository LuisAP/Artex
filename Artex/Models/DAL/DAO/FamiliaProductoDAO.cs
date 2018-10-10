using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using System.Data.Entity;

namespace Artex.Models.DAL.DAO
{
    public class FamiliaProductoDAO
    {
        public List<familia_producto> GetAlls(ArtexConnection dbContext = null)
        {
            List<familia_producto> list = null;
            try
            {
                using (dbContext = dbContext != null ? dbContext : new ArtexConnection())
                {
                    list = dbContext.familia_producto.OrderBy(e => e.ID).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return list;
        }
        public List<familia_producto> GetActive(ArtexConnection dbContext = null)
        {
            List<familia_producto> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                list = dbContext.familia_producto.Where(m => m.ACTIVO == true).OrderBy(e => e.ID).ToList();

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public familia_producto GetById(int id, ArtexConnection dbContext = null)
        {
            familia_producto consulta = null;

            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();
                
                    consulta = dbContext.familia_producto.Where(e => e.ID == id).FirstOrDefault();
                
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
                    var consulta = dbContext.familia_producto.Where(m => m.ID == id).FirstOrDefault();
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