using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Artex.Models.DAL.DTO.Catalogos
{
    public class CotizacionDTO
    {
        public int cliente { get; set; }
        public List<ListaProductosDTO> prooductos { get; set; }
    }
    public class ListaProductosDTO
    {
        public string origen { get; set; }
        public int idByOrigen { get; set; }
        public int idProducto { get; set; }
        public string codigoConfig { get; set; }
        public string configuracion { get; set; }
        public int cantidad { get; set; }
        public string descuento { get; set; }
        public string precio { get; set; }
        public Boolean configurado { get; set; }
       

    }
}