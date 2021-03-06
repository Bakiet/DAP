﻿using System.ComponentModel.DataAnnotations;

namespace Ppgz.Web.Areas.Dap.Models
{
    public class ComponenteElectricoSustitucionViewModel
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

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Modelo")]
        public string Modelo { get; set; }

      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Serial")]
        public string Serial { get; set; }

      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Fabricado")]
        public string FechaFabricado { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Vencimiento")]
        public string Duracion { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Sustitución")]
        public string Sustitucion { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fotografia")]
        public string Fotografia { get; set; }


    }
}