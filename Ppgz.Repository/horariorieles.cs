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
    
    public partial class horariorieles
    {
        public int Id { get; set; }
        public System.DateTime Fecha { get; set; }
        public bool Disponibilidad { get; set; }
        public int HorarioId { get; set; }
        public int AlmacenRielId { get; set; }
        public Nullable<int> CitaId { get; set; }
        public string ComentarioBloqueo { get; set; }
    
        public virtual citas citas { get; set; }
        public virtual horarios horarios { get; set; }
        public virtual rieles rieles { get; set; }
    }
}
