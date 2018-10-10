﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.DB;
using System.Data.Entity;

namespace Artex.Models.DAL.DAO
{
    public class MateriaPrimaDAO
    {
        public List<materia_prima> GetAlls(ArtexConnection dbContext = null)
        {
            List<materia_prima> list = null;
            try
            {
                using (dbContext = dbContext != null ? dbContext : new ArtexConnection())
                {
                    list = dbContext.materia_prima.OrderBy(e => e.ID).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return list;
        }
        public List<materia_prima> GetActive(ArtexConnection dbContext = null)
        {
            List<materia_prima> list = null;
            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                list = dbContext.materia_prima.Where(m => m.ACTIVO == true).OrderBy(e => e.ID).ToList();

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public materia_prima GetById(int id, ArtexConnection dbContext = null)
        {
            materia_prima consulta = null;

            try
            {
                dbContext = dbContext != null ? dbContext : new ArtexConnection();

                consulta = dbContext.materia_prima.Where(e => e.ID == id).FirstOrDefault();

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
                    var consulta = dbContext.materia_prima.Where(m => m.ID == id).FirstOrDefault();
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