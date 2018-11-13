using System.ComponentModel.DataAnnotations;

namespace Ppgz.Web.Areas.Dap.Models
{
    public class ObraContactoViewModel
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "NOMBRE")]
        public string Nombre { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "EMAIL")]
        public string Email { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "TELEFONO")]
        public string Telefono { get; set; }
    }
}