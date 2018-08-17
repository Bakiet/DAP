using System.ComponentModel.DataAnnotations;

namespace Ppgz.Web.Areas.Dap.Models
{
    public class SugerenciaViewModel
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Numero")]
        public string Numero { get; set; }
     //   [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha")]
        public string Fecha { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Caracteristica")]
        public string Caracteristica { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Acciones Tomadas")]
        public string AccionesTomadas { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Acciones Recomendadas")]
        public string AccionesRecomendadas { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Gerencia Responsable")]
        public string GerenciaResponsable { get; set; }
        
        
    }
}