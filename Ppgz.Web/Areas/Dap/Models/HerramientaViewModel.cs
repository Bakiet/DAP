using System.ComponentModel.DataAnnotations;

namespace Ppgz.Web.Areas.Dap.Models
{
    public class HerramientaViewModel
    {

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Cantidad")]
        public string Cantidad { get; set; }

      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de Salida")]
        public string FechaSalida { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Propiedad")]
        public string Propiedad { get; set; }

      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de Culminacion")]
        public string FechaCulminacion { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Cantidad Deposito")]
        public string CantidadDeposito { get; set; }

      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Entrada")]
        public string FechaEntrada { get; set; }

      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Supervisor de Obra")]
        public string SupervisorObra { get; set; }

      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Tecnico Responsable")]
        public string TecnicoResponsable { get; set; }

      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [Display(Name = "Archivo")]
        public string Archivo { get; set; }

        public int obra_id { get; set; }

        public string obra { get; set; }

    }
}