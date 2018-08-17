using System.ComponentModel.DataAnnotations;

namespace Ppgz.Web.Areas.Dap.Models
{
    public class DatoObraViewModel
    {
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Primer Contacto en obra")]
        public string primercontacto { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Contacto telefónico")]
        public string contactotelefonicoobra { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Correo Electrónico")]
        public string correoelectronicoobra { get; set; }

      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Cargo")]
        public string cargoobra { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Nombre de la constructora")]
        public string nombreconstructora { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Contacto Telefónico")]
        public string contactotelefonicoempresa { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Correo Electrónico")]
        public string correoelectronicoempresa { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Responsable Cargo")]
        public string cargoempresa { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Dirección de Oficina")]
        public string direccionoficina { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Ubicación de Obra")]
        public string ubicacionobra { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Tipo de obra")]
        public string tipoobra { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Número de quipos")]
        public string numeroequipos { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Tipo de Equipo")]
        public string tipoequipo { get; set; }

        //[Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Vendidas por climb")]
        public string vendidasclimb { get; set; }

        //[Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Vendidas por otros")]
        public string vendidasotros { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "No vendidas")]
        public string novendidas { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Paralizadas")]
        public string paralizadas { get; set; }


    }
}