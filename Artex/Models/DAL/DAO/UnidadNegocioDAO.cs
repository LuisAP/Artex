using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using System.Data.Entity;

namespace Artex.Models.DAL.DAO
{
    public class UnidadNegocioDAO
    {
        public List<unidad_de_negocio> GetAlls(ArtexConnection dbContext = null)
        {
            List<unidad_de_negocio> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();
                
                    list = dbContext.unidad_de_negocio.OrderBy(e => e.ID).ToList();
                
            }
            catch (Exception e)
            {

            }
            return list;
        }
        public List<unidad_de_negocio> GetActive(ArtexConnection dbContext = null)
        {
            List<unidad_de_negocio> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                list = dbContext.unidad_de_negocio.Where(m=> m.ACTIVO==true).OrderBy(e => e.ID).ToList();

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public unidad_de_negocio GetById(int id, ArtexConnection dbContext = null)
        {
            unidad_de_negocio consulta = null;

            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                consulta = dbContext.unidad_de_negocio.Where(e => e.ID == id).FirstOrDefault();

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
                    var consulta = dbContext.unidad_de_negocio.Where(m => m.ID == id).FirstOrDefault();
                    if (consulta != null)
                    {
                        dbContext.unidad_de_negocio.Remove(consulta);

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