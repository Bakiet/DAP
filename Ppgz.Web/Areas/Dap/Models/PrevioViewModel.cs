using System.ComponentModel.DataAnnotations;

namespace Ppgz.Web.Areas.Dap.Models
{
    public class PrevioViewModel
    {

        //  [Required(ErrorMessage = "El campo es obligatorio.")] NUEVO
        [Display(Name = "Carta de presentación")]
        public string CartaPresentacion { get; set; }

        //  [Required(ErrorMessage = "El campo es obligatorio.")] NUEVO
        [Display(Name = "Carta de presentación Documento")]
        public string CartaPresentacionFotografia { get; set; }

        [Display(Name = "Fecha Firma de Contrato")]
        public string FechaFirmaContrato { get; set; }

        [Display(Name = "Fecha Firma de Contrato Documento ")]
        public string FechaFirmaContratoFotografia { get; set; }

        //  [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Pago Inicial de Fabrica")]
        public string FechaPagoInicialFabrica { get; set; }

        [Display(Name = "Fecha Pago Inicial de Fabrica Documento ")]
        public string FechaPagoInicialFabricaFotografia { get; set; }

        //  [Required(ErrorMessage = "El campo es obligatorio.")]  NUEVO
        [Display(Name = "Fecha de ingreso en producción")]
        public string FechaIngresoProduccion { get; set; }

        //  [Required(ErrorMessage = "El campo es obligatorio.")]  NUEVO
        [Display(Name = "Fecha de ingreso en producción Documento")]
        public string FechaIngresoProduccionFotografia { get; set; }

        [Display(Name = "Fecha Pago Inicial de Equipo")]
        public string FechaPagoInicialEquipo { get; set; }

        [Display(Name = "Fecha Pago Inicial de Equipo Documento")]
        public string FechaPagoInicialEquipoFotografia { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de inicio de Construccion")]
        public string FechaConstruccion { get; set; }

        [Display(Name = "Fecha de inicio de Construccion Documento")]
        public string FechaConstruccionFotografia { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")] NUEVO
        [Display(Name = "Estatus de construcción")]
        public string EstatusConstruccion { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")] NUEVO
        [Display(Name = "Estatus de construcción Documento")]
        public string EstatusConstruccionFotografia { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")] NUEVO
        [Display(Name = "Ingreso al departamento de ingeniería y maniobras electrónicas")]
        public string IngresoDepartamentoIngMan { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")] NUEVO
        [Display(Name = "Ingreso al departamento de ingeniería y maniobras electrónicas Documento")]
        public string IngresoDepartamentoIngManFotografia { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")] NUEVO
        [Display(Name = "Solicitud de pago final a la fábrica")]
        public string SolicitudPagoInicialFabrica { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")] NUEVO
        [Display(Name = "Solicitud de pago final a la fábrica Documento")]
        public string SolicitudPagoInicialFabricaFotografia { get; set; }


        // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Salida de Fabrica a puerto")]
        public string FechaSalidaFabrica { get; set; }

        [Display(Name = "Fecha Salida de Fabrica Documento ")]
        public string FechaSalidaFabricaFotografia { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Salida del Buque a Venezuela")]
        public string FechaSalidaBuque { get; set; }

        [Display(Name = "Fecha Salida del Buque a Venezuela Documento ")]
        public string FechaSalidaBuqueFotografia { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Llegada del Buque")]
        public string FechaLlegadaBuque { get; set; }

        [Display(Name = "Fecha Llegada del Buque Documento ")]
        public string FechaLlegadaBuqueFotografia { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Periodo de Nacionalizacion")]
        public string FechaPeriodoNacionalizacion { get; set; }

        [Display(Name = "Fecha Periodo de Nacionalizacion Documento ")]
        public string FechaPeriodoNacionalizacionFotografia { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Salida de Puerto a Obra")]
        public string FechaSalidaPuertoObra { get; set; }

        [Display(Name = "Fecha Salida Puerto de Obra Documento ")]
        public string FechaSalidaPuertoObraFotografia { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Descarga y Resguardo de componentes")]
        public string FechaDescargaResguardo { get; set; }


       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Activacion Protocolo de Garantia de Fabrica")]
        public string ActivacionProtocoloGarantiaFabrica { get; set; }

        [Display(Name = "Activacion Protocolo de Garantia de Fabrica Documento ")]
        public string ActivacionProtocoloGarantiaFabricaFotografia { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Inicio de Montaje")]
        public string FechaInicioMontaje { get; set; }

        [Display(Name = "Fecha Inicio de Montaje Documento ")]
        public string FechaInicioMontajeFotografia { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Entrega de Foso")]
        public string FechaEntregaSoso { get; set; }

        [Display(Name = "Fecha Entrega de Foso Documento ")]
        public string FechaEntregaSosoFotografia { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Culminacion de Montaje")]
        public string FechaCulminacionMontaje { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de Configuracion")]
        public string FechaConfiguracion { get; set; }

        [Display(Name = "Fecha de Configuracion Documento ")]
        public string FechaConfiguracionFotografia { get; set; }

        //[Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Periodo de Prueba")]
        public string FechaPeriodoPrueba { get; set; }

        [Display(Name = "Fecha Periodo de Prueba Documento ")]
        public string FechaPeriodoPruebaFotografia { get; set; }

        // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de Inspeccion")]
        public string FechaInspeccion { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Tecnico Instalador")]
        public string TecnicoInstalador { get; set; }

       
        // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Informe Entrega Final")]
        public string InformeEntregaFinal { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Descarga de Resguardo Documento ")]
        public string FechaDescargaResguardoFotografia { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha Culminacion de Montaje Documento ")]
        public string FechaCulminacionMontajeFotografia { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de Inspeccion Documento ")]
        public string FechaInspeccionFotografia { get; set; }

       // [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Fecha de Inspeccion Video")]
        public string FechaInspeccionVideo { get; set; }

        public int previo_id { get; set; }

    }
}