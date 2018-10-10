using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using System.Data.Entity;

namespace Artex.Models.DAL.DAO
{
    public class PersonaFisicaDAO
    {
        public List<persona_fisica> GetAlls(ArtexConnection dbContext = null)
        {
            List<persona_fisica> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();
               
                    list = dbContext.persona_fisica.OrderBy(e => e.ID).ToList();
                
            }
            catch (Exception e)
            {

            }
            return list;
        }
     
        public persona_fisica GetById(int id, ArtexConnection dbContext = null)
        {
            persona_fisica consulta = null;

            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                consulta = dbContext.persona_fisica.Where(e => e.ID == id).FirstOrDefault();

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
                    var consulta = dbContext.persona_fisica.Where(m => m.ID == id).FirstOrDefault();
                    if (consulta != null)
                    {
                        dbContext.persona_fisica.Remove(consulta);

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