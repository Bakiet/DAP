using System.ComponentModel.DataAnnotations;
using System;
namespace Ppgz.Web.Areas.Dap.Models
{
    public class RequerimientoViewModel
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Obra")]
        public string Obra { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de Solicitud")]
        public DateTime? FechaSolicitud { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Gerencia Responsable")]
        public string GerenciaResponsable { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Técnicos")]
        public string Tecnicos { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Estatus de Requerimiento")]
        public string StatusRequerimientos { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de Cumplimiento de Solicitud")]
        public DateTime? FechaCumplimientoSolicitud { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Acciones Ejecutadas")]
        public string AccionesEjecutadas { get; set; }
        
        public int? Id { get; set; }

        public int? obra_id { get; set; }
    }
}