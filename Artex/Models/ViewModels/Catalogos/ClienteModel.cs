using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Artex.DB;
using Artex.Util;

namespace Artex.Models.ViewModels.Catalogos
{
    public class ClienteModel
    {
        public int idCliente { get; set; }
        public bool esPersonaFisica { get; set; }

    
        [Display(Name = "Nombre de la empresa")]
        public string nombreEmpresa { get; set; }

        [Display(Name = "Nombre")]
        public string nombrePersona { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string apellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        public string apellidoMaterno { get; set; }

        [Display(Name = "Clabe Interbancaria")]
        public string Clabe { get; set; }

        [Display(Name = "Razón social")]
        public string razonSocial { get; set; }

        [Display(Name = "No. de cuenta")]
        [Required (ErrorMessage = "Se requiere el No. de cuenta")]
        public string cuenta { get; set; }

        [Display(Name = "Tipo de cuenta")]
        public string tipoCuenta { get; set; }

        [Display(Name = "Referencia Bancomer")]
        [Required(ErrorMessage = "Ingresé una referencia Bancaria")]
        public string referencia { get; set; }

        [Display(Name = "Referencia Banamex")]
        [Required(ErrorMessage = "Ingresé una referencia Bancaria")]
        public string referencia2 { get; set; }

        [Display(Name = "Banco")]
        [Required(ErrorMessage ="Seleccioné un Banco")]
        public int banco { get; set; }
        public IEnumerable<bancos> bancosList { get; set; }

        [Display(Name = "Saldo")]
        [RegularExpression(RegularExpressionsUtil.MONEY, ErrorMessage = RegularExpressionsUtil.ERRORMESSAGE_MONEY)]
        public string saldo { get; set; }

        [Display(Name = "Credito máximo")]
        [RegularExpression(RegularExpressionsUtil.MONEY, ErrorMessage = RegularExpressionsUtil.ERRORMESSAGE_MONEY)]
        public string creditoMaximo { get; set; }

        [Display(Name = "Tipo de cliente")]
        public Boolean esClienteCredito { get; set; }


       // [Required(ErrorMessage = "El RFC es requerido")] 
        [Display(Name = "RFC")]
        [RegularExpression(RegularExpressionsUtil.RFC, ErrorMessage = RegularExpressionsUtil.ERRORMESSAGE_RFC)]
        [Required(ErrorMessage = "Ingrese un RFC con formato valido")]
        public string rfc { get; set; }

        [Display(Name = "Teléfono")]
        public string telefono { get; set; }

        [Display(Name = "Celular")]
        [Required(ErrorMessage ="El Número de Celular es requerido")]
        public string celular { get; set; }

        [Display(Name = "Correo")]
        [RegularExpression(RegularExpressionsUtil.EMAIL, ErrorMessage = RegularExpressionsUtil.ERRORMESSAGE_EMAIL)]
        [Required(ErrorMessage = "El Correo es requerido")]
        public string correo { get; set; }


        [Display(Name = "Activo")]
        public bool Activo { get; set; }

       

        [Display(Name = "Calle")]
        public string calle { get; set; }

        [Display(Name = "Número Exterior")]
        public string numExterior { get; set; }

        [Display(Name = "Número Interior")]
        public string numInterior { get; set; }

        [Display(Name = "Colonia")]
        public string colonia { get; set; }

        [Display(Name = "Ciudad")]
        public string ciudad { get; set; }

        [Display(Name = "Municipio/Delegación")]
        public string municipio { get; set; }
       
        public IEnumerable<pais> paisList { get; set; }
        [Display(Name = "País")]
        public int? pais { get; set; }

        [Display(Name = "Estado")]
        public int? estado { get; set; }
        public IEnumerable<estado> estadoList { get; set; }

        [Display(Name = "Código Postal")]
        public string codigoPostal { get; set; }



       

//datos de contacto

        [Display(Name = "Nombre")]
        public string nombreContacto { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string apellidoPaternoContacto { get; set; }

        [Display(Name = "Apellido Materno")]
        public string apellidoMaternoContacto { get; set; }

        [Display(Name = "Telefono")]
        public string telefonoContacto { get; set; }

        [Display(Name = "Celular")]
        public string celularContacto { get; set; }

        [Display(Name = "Correo")]
        [RegularExpression(RegularExpressionsUtil.EMAIL, ErrorMessage = RegularExpressionsUtil.ERRORMESSAGE_EMAIL)]
        public string correoContacto { get; set; }

// Datos 2do Contacto
        [Display(Name = "Nombre")]
        public string nombreContacto2 { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string apellidoPaternoContacto2 { get; set; }

        [Display(Name = "Apellido Materno")]
        public string apellidoMaternoContacto2 { get; set; }

        [Display(Name = "Telefono")]
        public string telefonoContacto2 { get; set; }

        [Display(Name = "Celular")]
        public string celularContacto2 { get; set; }

        [Display(Name = "Correo")]
        [RegularExpression(RegularExpressionsUtil.EMAIL, ErrorMessage = RegularExpressionsUtil.ERRORMESSAGE_EMAIL)]
        public string correoContacto2 { get; set; }

    }

}