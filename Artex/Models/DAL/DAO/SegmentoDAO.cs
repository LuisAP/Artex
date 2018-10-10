using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using System.Data.Entity;

namespace Artex.Models.DAL.DAO
{
    public class SegmentoDAO
    {
        public  List<segmento> GetAlls(ArtexConnection dbContext = null)
        {
            List<segmento> list = null;
            try
            {
                using (dbContext = dbContext != null ? dbContext : new ArtexConnection())
                {
                    list = dbContext.segmento.OrderBy(e => e.ID).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return list;
        }
        public List<segmento> GetActive(ArtexConnection dbContext = null)
        {
            List<segmento> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                list = dbContext.segmento.Where(m => m.ACTIVO == true).OrderBy(e => e.ID).ToList();

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public segmento GetById(int id, ArtexConnection dbContext = null)
        {
            segmento consulta = null;

            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                consulta = dbContext.segmento.Where(e => e.ID == id).FirstOrDefault();

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
                    var consulta = dbContext.segmento.Where(m => m.ID == id).FirstOrDefault();
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