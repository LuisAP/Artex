using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.Models.DAL.DTO.Catalogos;
using Artex.DB;
using System.Data.Entity;
using Artex.Util;
using Artex.Models.ViewModels.Catalogos;

namespace Artex.Models.BLL.Catalogos
{
    public class AtributosBLL
    {      
        //Metodo que obtiene los atributos  hijos
        public  List<atributo_subatributo> ListarHijos(int idPadre, ref ArtexConnection db)
        {
            List<atributo_subatributo> lista = new List<atributo_subatributo>();

            //var padre = db.atributo_subatributo.Find(idPadre);

            var subAtributos = db.atributo_subatributo.Where(m => m.ID_PADRE == idPadre);

            foreach (var atributo in subAtributos)
            {
                lista.Add(atributo);
                lista.AddRange(ListarHijos(atributo.ID, ref db));
            }
            return lista;
        }



        //Metodo que obtiene los atributos padre 
        public List<atributo_subatributo> ListarPadres(int idHijo,   ref ArtexConnection db)
        {
            List<atributo_subatributo> lista = new List<atributo_subatributo>();
            var padre = db.atributo_subatributo.FirstOrDefault(m => m.ID == idHijo);

            lista.Add(padre);

            if (padre.ID_PADRE != null)
                lista.AddRange(ListarPadres((int)padre.ID_PADRE, ref db));
            
            return lista;
        }


        public String GetCode(ref formulacion_comodin formulacion, int seleccionado,string code="")
        {
            var atributo = formulacion.atributo_subatributo.FirstOrDefault(m => m.ID == seleccionado);
            

            if (atributo.ID_PADRE != null)
            code = GetCode(ref formulacion,(int)atributo.ID_PADRE, atributo.CODIGO)+ code;

            else
            code = atributo.CODIGO+ code;
            return code;
        }
        public String GetDescripcionConfig(ref formulacion_comodin formulacion, int seleccionado, string descriptcion = "")
        {
            var atributo = formulacion.atributo_subatributo.FirstOrDefault(m => m.ID == seleccionado);


            if (atributo.ID_PADRE != null)
                descriptcion = GetDescripcionConfig(ref formulacion, (int)atributo.ID_PADRE, atributo.NOMBRE) +" "+ descriptcion;

           // else
              //  descriptcion = atributo.NOMBRE + descriptcion;
            return descriptcion;
        }
    }
}