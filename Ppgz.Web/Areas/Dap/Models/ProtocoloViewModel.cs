﻿using System.ComponentModel.DataAnnotations;

namespace Ppgz.Web.Areas.Dap.Models
{
    public class ProtocoloViewModel
    {

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Url")]
        public string Url { get; set; }

        public int obra_id { get; set; }

        public string obra { get; set; }

    }
}