using System.ComponentModel.DataAnnotations;
using System;
namespace Ppgz.Web.Areas.Dap.Models
{
    public class EquipoViewModel
    {

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "NOMBRE")]
        public string Nombre { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "MARCA")]
        public string Marca { get; set; }

      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "MODELO")]
        public string Modelo { get; set; }

      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "PARADAS")]
        public string Paradas { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "REFERENCIA")]
        public string Referencia { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Dimensiones de Cabina")]
        public string DimensionesCabina { get; set; }

      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Dimensiones de Hueco")]
        public string DimensionesHueco { get; set; }

      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Carga Nominal")]
        public string CargaNominal { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "VELOCIDAD")]
        public string Velocidad { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "RECORRIDO")]
        public string Recorrido { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "PARADAS")]
        public string TipoDeManiobraParadas { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "ACCESOS")]
        public string Accesos { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Voltaje De Red")]
        public string VoltajeDeRed { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Potencia De Maquina")]
        public string PotenciaDeMaquina { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Tipo De Maniobra")]
        public string TipoDeManiobra { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Numero De Guayas")]
        public string NumeroDeGuayas { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "FOTOGRAFIA")]
        public string Fotografia { get; set; }

        [Display(Name = "Cantidad de Personas")]
        public string CantidadPersonas { get; set; }


        [Display(Name = "Fecha Activación de Garantía")]
        public string FechaGarantia { get; set; }

        [Display(Name = "Fecha Vencimiento")]
        public string FechaVencimiento { get; set; }
        // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "PLANO")]
        public string Plano { get; set; }

        public int obra_id { get; set; }

        public string obra { get; set; }


    }
}