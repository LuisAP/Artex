using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using System.Data.Entity;

namespace Artex.Models.DAL.DAO
{
    public class ConceptoProductoDAO
    {
        public List<concepto_producto> GetAlls(ArtexConnection dbContext = null)
        {
            List<concepto_producto> list = null;
            try
            {
                using (dbContext = dbContext != null ? dbContext : new ArtexConnection())
                {
                    list = dbContext.concepto_producto.OrderBy(e => e.ID).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return list;
        }

        public List<concepto_producto> GetActive(ArtexConnection dbContext = null)
        {
            List<concepto_producto> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                list = dbContext.concepto_producto.Where(m => m.ACTIVO == true).OrderBy(e => e.ID).ToList();

            }
            catch (Exception e)
            {

            }
            return list;
        }
        public concepto_producto GetById(int id, ArtexConnection dbContext = null)
        {
            concepto_producto consulta = null;

            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                consulta = dbContext.concepto_producto.Where(e => e.ID == id).FirstOrDefault();

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
                    var consulta = dbContext.concepto_producto.Where(m => m.ID == id).FirstOrDefault();
                    if (consulta != null)
                    {
                        if (consulta.ACTIVO == true) {
                            consulta.ACTIVO = false;

                            result = dbContext.SaveChanges() > 0 || dbContext.Entry(consulta).State == EntityState.Unchanged;
                        }

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