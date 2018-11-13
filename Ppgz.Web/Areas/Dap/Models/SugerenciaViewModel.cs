using System.ComponentModel.DataAnnotations;

namespace Ppgz.Web.Areas.Dap.Models
{
    public class SugerenciaViewModel
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "DESCRIPCIÓN")]
        public string Descripcion { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "NUMERO")]
        public string Numero { get; set; }
     //   [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "FECHA")]
        public string Fecha { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "CARACTERISTICA")]
        public string Caracteristica { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "ACCIONES TOMADAS")]
        public string AccionesTomadas { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "ACCIONES RECOMENDADAS")]
        public string AccionesRecomendadas { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Gerencia Responsable")]
        public string GerenciaResponsable { get; set; }
        
        
    }
}