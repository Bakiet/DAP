using System.ComponentModel.DataAnnotations;
using System;
namespace Ppgz.Web.Areas.Dap.Models
{
    public class ComponenteMecanicoViewModel
    {

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }    

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Caracteristicas")]
        public string Caracteristicas { get; set; }

        //[Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Marca")]
        public string Marca { get; set; }

        //[Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Modelo")]
        public string Modelo { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Serial")]
        public string Serial { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Fabricado")]
        public DateTime? FechaFabricado { get; set; }

        //[Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Duración")]
        public string Duracion { get; set; }

        //[Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Sustitución")]
        public string Sustitucion { get; set; }

        //[Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fotografia")]
        public string Fotografia { get; set; }

        [Display(Name = "Fecha Sustitución")]
        public DateTime? FechaSustitucion { get; set; }

    }
}