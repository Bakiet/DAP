using System.ComponentModel.DataAnnotations;
using System;
namespace Ppgz.Web.Areas.Dap.Models
{
    public class EnvioViewModel
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Obra")]
        public string Obra { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Tipo de Componente")]
        public string TipoComponente { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de Pedido")]
        public DateTime? FechaPedido { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "fecha de Salida")]
        public DateTime? FechaSalida { get; set; }
     //   [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Tracking")]
        public string Tracking { get; set; }
     //   [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Empresa de Envio")]       
        public string EmpresaEnvio { get; set; }
      //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [DataType(DataType.Text)]
        [Display(Name = "Status de Envío")]
        public string StatusEnvio { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [DataType(DataType.Text)]
        [Display(Name = "Numero de Bulto")]
        public string NumeroBulto { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Peso Total")]
        public string PesoTotal { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "CodigoArancelario")]
        public string CodigoArancelario { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Solicitado")]
        public string Solicitado { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Costo del Producto")]
        public string CostoProducto { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Status de Pago")]
        public string StatusPago { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Numero de Factura")]
        public string NumeroFactura { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Status de Entrega")]
        public string StatusEntrega { get; set; }
        //[Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Entrega de Producto")]
        public string EntregaProducto { get; set; }
       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de Entrega")]
        public DateTime? FechaEntrega { get; set; }

        public int? obra_id { get; set; }

        public int id { get; set; }
    }
}