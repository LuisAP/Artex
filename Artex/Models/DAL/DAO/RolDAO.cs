using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using Artex.Models.DAL.DTO.RecursosHumanos;
using System.Data.Entity;

namespace Artex.Models.DAL.DAO
{
    public class RolDAO
    {
        public List<rol> GetAlls(ArtexConnection dbContext = null)
        {
            List<rol> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                list = dbContext.rol.OrderBy(e => e.ID).ToList();

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public List<rol> GetActive(ArtexConnection dbContext = null)
        {
            List<rol> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                list = dbContext.rol.Where(m => m.ACTIVO == true).OrderBy(e => e.ID).ToList();

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public bool DeleteFromId(int id, ArtexConnection artexContext)
        {
            rol rol = GetFromId(id, artexContext);
            rol.ACTIVO = false;
            return artexContext.SaveChanges() > 0;
        }

        public rol GetFromId(int id, ArtexConnection artexContext)
        {
            return artexContext.rol.FirstOrDefault(x => x.ID == id);
        }
        public List<rol> GetRoles(ArtexConnection artexContext = null)
        {
            artexContext = artexContext == null ? artexContext = new ArtexConnection() : artexContext;
            return artexContext.rol.OrderBy(x => x.ID).ToList();
        }
    }
}