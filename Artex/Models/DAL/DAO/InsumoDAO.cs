using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using System.Data.Entity;

namespace Artex.Models.DAL.DAO
{
    public class InsumoDAO
    {
        public List<insumo> GetAlls(ArtexConnection dbContext = null)
        {
            List<insumo> list = null;
            try
            {
                using (dbContext = dbContext != null ? dbContext : new ArtexConnection())
                {
                    list = dbContext.insumo.OrderBy(e => e.ID).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return list;
        }
        public List<insumo> GetActive(ArtexConnection dbContext = null)
        {
            List<insumo> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                list = dbContext.insumo.Where(m => m.ACTIVO == true).OrderBy(e => e.ID).ToList();

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public insumo GetById(int id, ArtexConnection dbContext = null)
        {
            insumo consulta = null;

            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                consulta = dbContext.insumo.Where(e => e.ID == id).FirstOrDefault();

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
                    var consulta = dbContext.insumo.Where(m => m.ID == id).FirstOrDefault();
                    if (consulta != null)
                    {
                        dbContext.insumo.Remove(consulta);

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