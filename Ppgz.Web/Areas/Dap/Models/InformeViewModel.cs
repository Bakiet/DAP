using System.ComponentModel.DataAnnotations;

namespace Ppgz.Web.Areas.Dap.Models
{
    public class InformeViewModel
    {

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "NOMBRE")]
        public string Nombre { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "URL")]
        public string Url { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "FECHA")]
        public string Fecha { get; set; }


    }
}