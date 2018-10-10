using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using System.Data.Entity;

namespace Artex.Models.DAL.DAO
{
    public class EmpleadoDAO
    {
        

        public List<empleados_v> GetAlls(ArtexConnection dbContext = null)
        {
            List<empleados_v> list = null;
            dbContext = dbContext == null ? new ArtexConnection() : dbContext;
            try
            {

                list = dbContext.empleados_v.OrderBy(e => e.ID_EMPLEADO).ToList();
            }
            catch (Exception e)
            {

            }
            return list;
        }
        public List<empleados_v> GetActive(ArtexConnection dbContext = null)
        {
            List<empleados_v> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                list = dbContext.empleados_v.Where(m => m.ACTIVO == true).OrderBy(e => e.ID_EMPLEADO).ToList();

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public empleado GetById(int id, ArtexConnection db = null)
        {
            empleado evento = null;

            try
            {
                db = db != null ? db : new ArtexConnection();


                evento = db.empleado.Where(e => e.ID == id).FirstOrDefault();

            }
            catch (Exception e)
            {
            }

            return evento;
        }
        public empleados_v GetById_v(int id, ArtexConnection db = null)
        {
            empleados_v evento = null;

            try
            {
                db = db != null ? db : new ArtexConnection();


                evento = db.empleados_v.Where(e => e.ID_EMPLEADO == id).FirstOrDefault();

            }
            catch (Exception e)
            {
            }

            return evento;
        }

        public bool DeleteById(int id)
        {
            bool result = false;

            try
            {
                ArtexConnection db = new ArtexConnection();
                empleado consulta = db.empleado.Where(m => m.ID == id).FirstOrDefault();
                if (consulta != null)
                {
                    consulta.ACTIVO = false;

                    result = db.SaveChanges() > 0 || db.Entry(consulta).State == EntityState.Unchanged;

                }
            }
            catch (Exception e)
            {

            }
            return result;
        }
        
    }
}