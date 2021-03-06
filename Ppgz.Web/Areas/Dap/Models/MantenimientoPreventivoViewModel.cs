﻿using System.ComponentModel.DataAnnotations;
using System;
namespace Ppgz.Web.Areas.Dap.Models
{
    public class MantenimientoPreventivoViewModel
    {
        

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "EQUIPO")]
        public string Tipo { get; set; }

     //   [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "DESCRIPCIÓN")]
        public string Descripcion { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Persona Jurídica")]
        public string PersonaJuridica { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de Visita")]
        public string FechaVisita { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de Publicación")]
        public string FechaPublicacion { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Nombre de Documento")]
        public string NombreDocumento { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "TECNICO")]
        public string Tecnico { get; set; }

      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Equipos Atendidos")]
        public string EquiposAtendidos { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Estatus de Mantenimiento")]
        public string StatusMantenimiento { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "ARCHIVO")]
        public string Archivo { get; set; }


       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "EVALUACIÓN")]
        public string Evaluacion { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Hora de Llegada")]
        public string HoraLlegada { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Hora de Salida")]
        public string HoraSalida { get; set; }

        public int IdVenta { get; set; }

    }
}