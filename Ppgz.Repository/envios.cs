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
    
    public partial class envios
    {
        public int Id { get; set; }
        public string Obra { get; set; }
        public string TipoComponente { get; set; }
        public Nullable<System.DateTime> FechaPedido { get; set; }
        public Nullable<System.DateTime> FechaSalida { get; set; }
        public string Tracking { get; set; }
        public string EmpresaEnvio { get; set; }
        public string StatusEnvio { get; set; }
        public string NumeroBulto { get; set; }
        public string PesoTotal { get; set; }
        public string CodigoArancelario { get; set; }
        public string Solicitado { get; set; }
        public string CostoProducto { get; set; }
        public string StatusPago { get; set; }
        public string NumeroFactura { get; set; }
        public string StatusEntrega { get; set; }
        public string EntregaProducto { get; set; }
        public Nullable<System.DateTime> FechaEntrega { get; set; }
        public Nullable<int> obra_id { get; set; }
    }
}
