using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.DB;
using Artex.Util;

namespace Artex.Models.ViewModels.Catalogos
{
    public class EmpleadoModel
    {
        

        public int id { get; set; }
        public int idPersona { get; set; }
        public int idDirection { get; set; }
        public int idDirTelefonico { get; set; }
        #region Datos Personales



        [Required(ErrorMessage = "El Nombre es requerido")]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El Apellido Paterno es requerido")]
        [Display(Name = "Apellido Paterno")]
        public string apellidoPaterno { get; set; }

        [Required(ErrorMessage = "El Apellido Materno es requerido")]
        [Display(Name = "Apellido Materno")]
        public string apellidoMaterno { get; set; }

        [RegularExpression(RegularExpressionsUtil.DATE, ErrorMessage = RegularExpressionsUtil.ERRORMESSAGE_DATE)]
        [Required(ErrorMessage = "la fecha de nacimiento es requerida")]
        [Display(Name = "Fecha de nacimiento")]
        public string fechaNacimiento { get; set; }

        [RegularExpression(RegularExpressionsUtil.RFC, ErrorMessage = RegularExpressionsUtil.ERRORMESSAGE_RFC)]
        [Required(ErrorMessage = "El RFC es requerido")]
        [Display(Name = "RFC")]
        public string rfc { get; set; }

        //[RegularExpression(RegularExpressionsUtil.RFC, ErrorMessage = RegularExpressionsUtil.ERRORMESSAGE_RFC)]
      //  [Required(ErrorMessage = "El RFC es requerido")]
        [Display(Name = "CURP")]
        public string curp { get; set; }

        [Display(Name = "Sexo")]
        public string sexo { get; set; }

        [Required(ErrorMessage = "La Calle es requerida")]
        [Display(Name = "Calle")]
        public string calle { get; set; }

        [Required(ErrorMessage = "El Número Exterior es requerido")]
        [Display(Name = "Número Exterior")]
        public string numExterior { get; set; }

        [Display(Name = "Número Interior")]
        public string numInterior { get; set; }

        [Required(ErrorMessage = "La Colonia es requerida")]
        [Display(Name = "Colonia")]
        public string colonia { get; set; }

        [Required(ErrorMessage = "La Ciudad es requerida")]
        [Display(Name = "Ciudad")]
        public string ciudad { get; set; }

        [Required(ErrorMessage = "El Municipio es requerido")]
        [Display(Name = "Municipio/Delegación")]
        public string municipio { get; set; }

      
        [Display(Name = "Estado")]
        public int? estado { get; set; }
        public IEnumerable<estado> estadoList { get; set; }

        [Display(Name = "País")]
        public int? pais { get; set; }
        public IEnumerable<pais> paisList { get; set; }

        [Required(ErrorMessage = "El Codigo Postal es requerido")]
        [StringLength(5, ErrorMessage = "Escribe 5 dígitos", MinimumLength = 5)]
        [Display(Name = "C.P.")]
        public string codigoPostal { get; set; }

 

        [Required(ErrorMessage = "El correo es requerido")]
        [Display(Name = "Correo personal")]
        [RegularExpression(RegularExpressionsUtil.EMAIL, ErrorMessage = RegularExpressionsUtil.ERRORMESSAGE_EMAIL)]
        public string correo { get; set; }

        [Display(Name = "Telefono")]
        public string telefono { get; set; }

        [Display(Name = "Celular")]
        public string celular { get; set; }


        #endregion
        [Display(Name = "NSS")]
        public string nss { get; set; }


        [Required(ErrorMessage = "El Puesto es requerido")]
        [Display(Name = "Puesto")]
        public string puesto { get; set; }

        [RegularExpression(RegularExpressionsUtil.DATE, ErrorMessage = RegularExpressionsUtil.ERRORMESSAGE_DATE)]
        [Required(ErrorMessage = "La Fecha de Ingreso es requerida")]
        [Display(Name = "Fecha Ingreso")]
        public string fechaIngreso { get; set; }

        [RegularExpression(RegularExpressionsUtil.DATE, ErrorMessage = RegularExpressionsUtil.ERRORMESSAGE_DATE)]
        [Display(Name = "Fecha Baja")]
        public string fechaBaja { get; set; }

        [Display(Name = "Correo Empresa")]
        [RegularExpression(RegularExpressionsUtil.EMAIL, ErrorMessage = RegularExpressionsUtil.ERRORMESSAGE_EMAIL)]
        public string correoEmpresa { get; set; }

        [RegularExpression(RegularExpressionsUtil.MONEY, ErrorMessage = RegularExpressionsUtil.ERRORMESSAGE_MONEY)]
        [Display(Name = "Salario")]
        public string salario { get; set; }

        [Display(Name = "Área de trabajo")]
        public int? areaTrabajo { get; set; }
        public IEnumerable<area_trabajo> areaTrabajoList { get; set; }

        [Display(Name = "Sucursal")]
        public int? sucursal { get; set; }
        public IEnumerable<tienda> sucursalList { get; set; }


        [Display(Name = "Activo")]
        public bool activo { get; set; }


    }
}