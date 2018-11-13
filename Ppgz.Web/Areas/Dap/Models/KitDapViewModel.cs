using System.ComponentModel.DataAnnotations;

namespace Ppgz.Web.Areas.Dap.Models
{
    public class KitDapViewModel
    {

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "NOMBRE")]
        public string Nombre { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "URL")]
        public string Url { get; set; }

        public int obra_id { get; set; }

        public string obra { get; set; }
    }
}