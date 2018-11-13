using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ppgz.Web.Areas.Dap.Models
{
    public class ObraViewModel
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "NOMBRE")]
        public string Nombre { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "PERSONA JURÍDICA")]
        public string PersonaJuridica { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "DIRECCION DE FACTURACIÓN")]
        public string DireccionFacturacion { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "DIRECCION DE OFICINA")]
        public string DireccionOficina { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "DIRECCION DE OBRA")]
        public string DireccionObra { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "TELÉFONO DE OFICINA")]
        public string TelefonoOficina { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [DataType(DataType.Text)]
        [Display(Name = "CONTACTO")]
        [StringLength(40, ErrorMessage = "Debe tener de 3 a 40 Carácteres.", MinimumLength = 3)]
        public string Contacto { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [DataType(DataType.Text)]
        [Display(Name = "CONTACTO 2")]
        [StringLength(40, ErrorMessage = "Debe tener de 3 a 40 Carácteres.", MinimumLength = 3)]
        public string Contacto2 { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico valído.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Debe ingresar un correo electrónico valído.")]
        [Display(Name = "EMAIL")]
        public string Email { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico valído.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Debe ingresar un correo electrónico valído.")]
        [Display(Name = "EMAIL 2")]
        public string Email2 { get; set; }

        [Display(Name = "FOTO")]
        public string foto { get; set; }

        [Display(Name = "MAPA")]
        public string mapa { get; set; }

        public int? Id { get; set; }

    }
}