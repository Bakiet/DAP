using System.ComponentModel.DataAnnotations;
using System;
namespace Ppgz.Web.Areas.Dap.Models
{
    public class RequerimientoViewModel
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "OBRA")]
        public string Obra { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "TIPO")]
        public string Tipo { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de Solicitud")]
        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}", ApplyFormatInEditMode = true)]
        public string FechaSolicitud { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Gerencia Responsable")]
        public string GerenciaResponsable { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "TECNICOS")]
        public string Tecnicos { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Estatus de Requerimiento")]
        public string StatusRequerimientos { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de Cumplimiento de Solicitud")]
        public string FechaCumplimientoSolicitud { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "DESCRIPCIÓN")]
        public string Descripcion { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Acciones Ejecutadas")]
        public string AccionesEjecutadas { get; set; }
        
        public int? Id { get; set; }

        public int? obra_id { get; set; }
    }
}