using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;

namespace Artex.Models.DAL.DAO.Views
{
    public class V_ClientesDAO
    {
        public List<clientes_v> GetAlls(ArtexConnection dbContext = null)
        {
            List<clientes_v> list = null;
            try
            {
                using (dbContext = dbContext != null ? dbContext : new ArtexConnection())
                {
                    list = dbContext.clientes_v.OrderBy(e => e.ID).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return list;
        }
    }
}