using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System;

namespace Ppgz.Web.Areas.Dap.Models
{
    public class FallaViewModel
    {

       
        [Display(Name = "Fecha de la falla")]
        public string FechaFalla { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Obra")]
        public string Obra { get; set; }
        public IEnumerable<SelectListItem> ObraList { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Equipo")]
        public string Equipo { get; set; }
        public IEnumerable<SelectListItem> EquipoList { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Tipo de Falla")]
        public string Tipo { get; set; }
        public IEnumerable<SelectListItem> FallaList { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Componente")]
        public string Componente { get; set; }
        public IEnumerable<SelectListItem> ComponenteElectronicoList { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Técnico")]
        public string Personal { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Estatus de la falla")]
        public string StatusFalla { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }


      
        [Display(Name = "Fecha de Soluciòn")]
        public string FechaSolucion { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Nùmero de Reporte")]
        public string NumeroReporte { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }


       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Condiciòn")]
        public string Condicion { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Acciones Tomadas")]
        public string AccionesTomadas { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Acciones Recomendadas")]
        public string AccionesRecomendadas { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Duración")]
        public string Duracion { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Persona Reporte")]
        public string PersonaReporte { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Gerencia Responsable")]
        public string GerenciaResponsable { get; set; }
        public string Componenete { get; internal set; }
        
        public int id { get; set; }

        public List<FallaViewModel> Fallas { get; set; }

        

        

        public int? obraid { get; set; }

        //public IEnumerable<SelectListItem> ComponenteMecanicoList { get; set; }

        
         


        /*
                public List<ComponenteElectricoViewModel> componenteselectricos { get; set; }

                public List<ComponenteMecanicoViewModel> componentesmecanicos { get; set; }*/
    }
}