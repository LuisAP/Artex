using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.DB;
using Artex.Models.DAL.DTO.Catalogos;

namespace Artex.Models.ViewModels.Catalogos
{
    public class ProductoModel
    {

        #region  Datos Generales
        public int Id { get; set; }
        public Boolean Editable { get; set; }
        
        [Display(Name = "Nombre:")]
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción:")]
        public string Descripcion { get; set; }

        [Display(Name = "Código:")]
        public string Codigo { get; set; }

        [Display(Name = "Unidad de negocio:")]
        [Required(ErrorMessage = "Debe seleccionar una unidad de negocio")]
        public int UnidadNegocio { get; set; }
        public IEnumerable<unidad_de_negocio> UnidadNegocioList { get; set; }


        [Display(Name = "Linea de negocio:")]
        [Required(ErrorMessage = "Debe seleccionar una linea de negocio")]
        public int LineaNegocio { get; set; }
        public IEnumerable<linea_negocio> LineaNegocioList { get; set; }

        [Display(Name = "Diseño:")]
        [Required(ErrorMessage = "Debe seleccionar una opcion")]
        public int disenio { get; set; }
        public IEnumerable<disenio> disenioList { get; set; }


        [Display(Name = "Familia de producto:")]
        [Required(ErrorMessage = "Debe seleccionar una familia de producto")]
        public int FamilaProducto { get; set; }
        public IEnumerable<familia_producto> FamilaProductoList { get; set; }

        [Display(Name = "Estatus sku:")]
        [Required(ErrorMessage = "Debe seleccionar un estatus de sku")]
        public int EstatusSku { get; set; }
        public IEnumerable<estatus_sku> EstatusSkuList { get; set; }

        [Display(Name = "Linea de producto:")]
        [Required(ErrorMessage = "Debe seleccionar una linea de producto")]
        public int lineaProducto { get; set; }
        public IEnumerable<linea_producto> lineaProductoList { get; set; }

        [Display(Name = "Concepto de producto:")]
        [Required(ErrorMessage = "Debe seleccionar un concepto de producto")]
        public int conceptoProducto { get; set; }
        public IEnumerable<concepto_producto> conceptoProductoList { get; set; }

        [Display(Name = "Segmento:")]
        [Required(ErrorMessage = "Debe seleccionar un dsegmento")]
        public int segmento { get; set; }
        public IEnumerable<segmento> segmentoList { get; set; }

        [Display(Name = "Estilo:")]
        [Required(ErrorMessage = "Debe seleccionar un estilo")]
        public int estilo { get; set; }
        public IEnumerable<estilo_producto> estiloList { get; set; }


        [Display(Name = "Tiene Exclusividad:")]
        public bool Exclusividad { get; set; }

        [Display(Name = "Activo:")]
        public bool Activo { get; set; }

        //Faactores
        [Display(Name = "Precio de Fasbrica: $")]
        public string PrecioFabrica { get; set; }

        [Display(Name = "Precio POP: $")]
        public string PrecioPop { get; set; }

        [Display(Name = "Precio Caenas: $")]
        public string PrecioCadenas { get; set; }

        [Display(Name = "Precio Franquicias: $")]
        public string PrecioFranquicia { get; set; }

        [Display(Name = "Precio Precio Proyectos: $")]
        public string PrecioProyectos { get; set; }


       // [Required(ErrorMessage = "Please select file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Solo puede selecionar imagenes")]
        public HttpPostedFileBase foto { get; set; }

        #endregion


        #region Fabricacion
        [Display(Name = "Puntuacion:")]
        public string Puntuacion { get; set; }

       // [Display(Name = "Configurar producto:")]
        public bool EsPersonalizable { get; set; }

      //  public ConfiguracionProductoModel configuracion { get; set; }

      public List<piezasDTO> PiezasSeleccionadas { get; set; }

        #endregion


        #region Externo



        #endregion



        /*
               [Display(Name = "Atributo:")]
               public int Atributo { get; set; }
               public IEnumerable<atributos_configuracion> AtributoList { get; set; }




                       [Display(Name = "Area de fabricacion:")]
                       public int AreaFabricacion { get; set; }
                       public IEnumerable<area_de_fabricacion> AreaFabricacionList { get { return new List<area_de_fabricacion>(); } set { } }


                       [Display(Name = "Proceso de fabricacion:")]
                       public int ProcesoFabricacion { get; set; }
                       public IEnumerable<procesos_de_fabricacion> ProcesoFabricacionList { get { return new List<procesos_de_fabricacion>(); } set { } }
                       */
    }
  /*  public class ReconfigurarPiezasModel
    {
        public int IdProducto { get; set; }

        //public int IdPieza { get; set; }
        public List<PiezasDto> Piezas { get; set; }

    }
    public class ConfiguracionProductoModel
    {
        public int Id { get; set; }
        public String Nombre { get; set; }

        public int IdPieza { get; set; }
        public List<PiezasDto> PiezasSeleccionadas { get; set; }

        public ListaTipoAtributosDTO listaTipoAtributos { get; set; }


    }*/
    public class FormulacionProductoModel
    {
        public int Id { get; set; }
        public String Nombre { get; set; }

        public FormulacionDTO ListaMp { get; set; }


    }

}