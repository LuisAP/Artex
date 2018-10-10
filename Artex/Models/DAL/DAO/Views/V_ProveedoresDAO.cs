using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;

namespace Artex.Models.DAL.DAO.Views
{
    public class V_ProveedoresDAO
    {
        public List<proveedores_v> GetAlls(ArtexConnection dbContext = null)
        {
            List<proveedores_v> list = null;
            try
            {
                using (dbContext = dbContext != null ? dbContext : new ArtexConnection())
                {
                    list = dbContext.proveedores_v.OrderBy(e => e.ID).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return list;
        }
    }
}