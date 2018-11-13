using System.ComponentModel.DataAnnotations;
using System;
namespace Ppgz.Web.Areas.Dap.Models
{
    public class ComponenteMecanicoViewModel
    {

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "TIPO")]
        public string Tipo { get; set; }    

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "CARACTERISTICAS")]
        public string Caracteristicas { get; set; }

        //[Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "DESCRIPCIÓN")]
        public string Descripcion { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "MARCA")]
        public string Marca { get; set; }

        //[Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "MODELO")]
        public string Modelo { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "SERIAL")]
        public string Serial { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "FECHA FABRICADO")]
        public string FechaFabricado { get; set; }

        //[Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "FECHA VENCIMIENTO")]
        public string Duracion { get; set; }

        //[Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "SUSTITUCIONES")]
        public string Sustitucion { get; set; }

        //[Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "FOTOGRAFIA")]
        public string Fotografia { get; set; }

        [Display(Name = "FECHA SUSTITUCIÓN")]
        public string FechaSustitucion { get; set; }

        [Display(Name = "FECHA ALERTA")]
       
        public string FechaAlerta { get; set; }

        public string obra { get; set; }

        public string equipo { get; set; }

    }
}