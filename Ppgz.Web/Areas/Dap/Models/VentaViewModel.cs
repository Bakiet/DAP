using System.ComponentModel.DataAnnotations;

namespace Ppgz.Web.Areas.Dap.Models
{
    public class VentaViewModel
    {

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "DESCRIPCIÓN")]
        public string Descripcion { get; set; }

        

    }
}