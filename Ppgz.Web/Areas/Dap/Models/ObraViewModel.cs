using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ppgz.Web.Areas.Dap.Models
{
    public class ObraViewModel
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Persona Jurídica")]
        public string PersonaJuridica { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Direccion de Facturación")]
        public string DireccionFacturacion { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Direccion de Oficina")]
        public string DireccionOficina { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Direccion de Obra")]
        public string DireccionObra { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Teléfono de Oficina")]
        public string TelefonoOficina { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [DataType(DataType.Text)]
        [Display(Name = "Contacto")]
        [StringLength(40, ErrorMessage = "Debe tener de 3 a 40 Carácteres.", MinimumLength = 3)]
        public string Contacto { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [DataType(DataType.Text)]
        [Display(Name = "Contacto 2")]
        [StringLength(40, ErrorMessage = "Debe tener de 3 a 40 Carácteres.", MinimumLength = 3)]
        public string Contacto2 { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico valído.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Debe ingresar un correo electrónico valído.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico valído.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Debe ingresar un correo electrónico valído.")]
        [Display(Name = "Email 2")]
        public string Email2 { get; set; }

        [Display(Name = "Foto")]
        public string foto { get; set; }

        [Display(Name = "Mapa")]
        public string mapa { get; set; }

        public int? Id { get; set; }

    }
}