//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ppgz.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class componentesmecanicos_sustituciones
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Serial { get; set; }
        public Nullable<System.DateTime> FechaFabricado { get; set; }
        public string Duracion { get; set; }
        public string Sustitucion { get; set; }
        public string Fotografia { get; set; }
        public Nullable<int> IdEquipos { get; set; }
        public string Tipo { get; set; }
        public string Caracteristicas { get; set; }
    }
}
