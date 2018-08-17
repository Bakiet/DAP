using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Ppgz.Services;
using Ppgz.Web.Areas.Dap.Models;
using Ppgz.Web.Infrastructure;
using Ppgz.Web.Infrastructure.Dap;
using System.Data;
using System.Linq;
using Ppgz.Repository;


namespace Ppgz.Web.Areas.Dap.Controllers
{
    [Authorize]
    public class AdministrarPreviosController : Controller
    {
        private readonly PreviosManager _previosManager = new PreviosManager();

        private readonly EquiposManager _equiposManager = new EquiposManager();

        private readonly ObrasManager _obrasManager = new ObrasManager();


        //asignar los roles para esta funcionalidad
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARPREVIOS-CONSULTAR")]

        internal string CargarPdf(HttpPostedFileBase file)
        {
            if (file != null && file.ContentType != "application/pdf")
            {
                //throw new BusinessException(MensajesResource.ERROR_MensajesInstitucionales_PdfInvalido);
            }

            if (file == null)
            {
               // throw new BusinessException(MensajesResource.ERROR_MensajesInstitucionales_PdfInvalido);
            }

            var fileName = Path.GetFileName(file.FileName);

            if (fileName == null)
            {
                //throw new BusinessException(MensajesResource.ERROR_MensajesInstitucionales_PdfInvalido);
            }

            var path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);

            file.SaveAs(path);

            return "~/Uploads/" + fileName;
        }
        /*
         *  
                public ActionResult Index()
                {
                    var db = new EntitiesDap();

                    var fecha = DateTime.Today.AddMonths(-3);


                    ViewBag.Previos = db.previos.ToList();




                    //ViewBag.EstatusCita = db.estatuscitas.ToList();

                    return View();
                }*/
        
        public ActionResult Previos(int id)
        {
            var equipo = _equiposManager.Find(id);

            TempData["previo_id_temp"] = id;
            ViewBag.Equipo = equipo;

            ViewBag.obra = _obrasManager.Find(equipo.obra_id);

            ViewBag.Previos = _previosManager.GetPrevios(id);

            ViewBag.FechaFirmaContratoFotografia = _obrasManager.FindArchivos(id, "FechaFirmaContratoFotografia", "previos");
            ViewBag.FechaPagoInicialFabricaFotografia = _obrasManager.FindArchivos(id, "FechaPagoInicialFabricaFotografia", "previos");
            ViewBag.FechaPagoInicialEquipoFotografia = _obrasManager.FindArchivos(id, "FechaPagoInicialEquipoFotografia", "previos");
            ViewBag.FechaConstruccionFotografia = _obrasManager.FindArchivos(id, "FechaConstruccionFotografia", "previos");
            ViewBag.FechaSalidaFabricaFotografia = _obrasManager.FindArchivos(id, "FechaSalidaFabricaFotografia", "previos");
            ViewBag.FechaSalidaBuqueFotografia = _obrasManager.FindArchivos(id, "FechaSalidaBuqueFotografia", "previos");
            ViewBag.FechaLlegadaBuqueFotografia = _obrasManager.FindArchivos(id, "FechaLlegadaBuqueFotografia", "previos");
            ViewBag.FechaPeriodoNacionalizacionFotografia = _obrasManager.FindArchivos(id, "FechaPeriodoNacionalizacionFotografia", "previos");
            ViewBag.FechaSalidaPuertoObraFotografia = _obrasManager.FindArchivos(id, "FechaSalidaPuertoObraFotografia", "previos");
            ViewBag.FechaDescargaResguardoFotografia = _obrasManager.FindArchivos(id, "FechaDescargaResguardoFotografia", "previos");
            ViewBag.FechaInicioMontajeFotografia = _obrasManager.FindArchivos(id, "FechaInicioMontajeFotografia", "previos");
            ViewBag.FechaEntregaSosoFotografia = _obrasManager.FindArchivos(id, "FechaEntregaSosoFotografia", "previos");
            ViewBag.FechaCulminacionMontajeFotografia = _obrasManager.FindArchivos(id, "FechaCulminacionMontajeFotografia", "previos");
            ViewBag.FechaConfiguracionFotografia = _obrasManager.FindArchivos(id, "FechaConfiguracionFotografia", "previos");
            ViewBag.FechaPeriodoPruebaFotografia = _obrasManager.FindArchivos(id, "FechaPeriodoPruebaFotografia", "previos");
            ViewBag.FechaInspeccionFotografia = _obrasManager.FindArchivos(id, "FechaInspeccionFotografia", "previos");
            ViewBag.FechaInspeccionVideo = _obrasManager.FindArchivos(id, "FechaInspeccionVideo", "previos");
            ViewBag.InformeEntregaFinal = _obrasManager.FindArchivos(id, "InformeEntregaFinal", "previos");

            ViewBag.CartaPresentacionFotografia = _obrasManager.FindArchivos(id, "CartaPresentacionFotografia", "previos");
            ViewBag.FechaIngresoProduccionFotografia = _obrasManager.FindArchivos(id, "FechaIngresoProduccionFotografia", "previos");
            ViewBag.EstatusConstruccionFotografia = _obrasManager.FindArchivos(id, "EstatusConstruccionFotografia", "previos");
            ViewBag.IngresoDepartamentoIngManFotografia = _obrasManager.FindArchivos(id, "IngresoDepartamentoIngManFotografia", "previos");
            ViewBag.SolicitudPagoInicialFabricaFotografia = _obrasManager.FindArchivos(id, "SolicitudPagoInicialFabricaFotografia", "previos");

            return View("Index");
        }
        
        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-LISTAR,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-MODIFICAR")]
        //public ActionResult Index()
        //{
        //    ViewBag.previos = _previosManager.FindAll();
        //    return View();
        //}
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARPREVIOS-CREAR")]
        public ActionResult Crear()
        {
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARPREVIOS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var equipo = _equiposManager.Find(id);
            TempData["equipo_id"] = id;
            TempData.Keep();

            ViewBag.Equipo = equipo;

            ViewBag.obra = _obrasManager.Find(equipo.obra_id);

            var previo = _previosManager.GetPrevio(id);

            ViewBag.FechaFirmaContratoFotografia = _obrasManager.FindArchivos(previo.Id, "FechaFirmaContratoFotografia" ,"previos");
            ViewBag.FechaPagoInicialFabricaFotografia = _obrasManager.FindArchivos(previo.Id, "FechaPagoInicialFabricaFotografia", "previos");
            ViewBag.FechaPagoInicialEquipoFotografia = _obrasManager.FindArchivos(previo.Id, "FechaPagoInicialEquipoFotografia", "previos");
            ViewBag.FechaConstruccionFotografia = _obrasManager.FindArchivos(previo.Id, "FechaConstruccionFotografia", "previos");
            ViewBag.FechaSalidaFabricaFotografia = _obrasManager.FindArchivos(previo.Id, "FechaSalidaFabricaFotografia", "previos");
            ViewBag.FechaSalidaBuqueFotografia = _obrasManager.FindArchivos(previo.Id, "FechaSalidaBuqueFotografia", "previos");
            ViewBag.FechaLlegadaBuqueFotografia = _obrasManager.FindArchivos(previo.Id, "FechaLlegadaBuqueFotografia", "previos");
            ViewBag.FechaPeriodoNacionalizacionFotografia = _obrasManager.FindArchivos(previo.Id, "FechaPeriodoNacionalizacionFotografia", "previos");
            ViewBag.FechaSalidaPuertoObraFotografia = _obrasManager.FindArchivos(previo.Id, "FechaSalidaPuertoObraFotografia", "previos");
            ViewBag.FechaDescargaResguardoFotografia = _obrasManager.FindArchivos(previo.Id, "FechaDescargaResguardoFotografia", "previos");
            ViewBag.FechaInicioMontajeFotografia = _obrasManager.FindArchivos(previo.Id, "FechaInicioMontajeFotografia", "previos");
            ViewBag.FechaEntregaSosoFotografia = _obrasManager.FindArchivos(previo.Id, "FechaEntregaSosoFotografia", "previos");
            ViewBag.FechaCulminacionMontajeFotografia = _obrasManager.FindArchivos(previo.Id, "FechaCulminacionMontajeFotografia", "previos");
            ViewBag.FechaConfiguracionFotografia = _obrasManager.FindArchivos(previo.Id, "FechaConfiguracionFotografia", "previos");
            ViewBag.FechaPeriodoPruebaFotografia = _obrasManager.FindArchivos(previo.Id, "FechaPeriodoPruebaFotografia", "previos");
            ViewBag.FechaInspeccionFotografia = _obrasManager.FindArchivos(previo.Id, "FechaInspeccionFotografia", "previos");
            ViewBag.FechaInspeccionVideo = _obrasManager.FindArchivos(previo.Id, "FechaInspeccionVideo", "previos");
            ViewBag.InformeEntregaFinal = _obrasManager.FindArchivos(previo.Id, "InformeEntregaFinal", "previos");

            ViewBag.CartaPresentacionFotografia = _obrasManager.FindArchivos(previo.Id, "CartaPresentacionFotografia", "previos");
            ViewBag.FechaIngresoProduccionFotografia = _obrasManager.FindArchivos(previo.Id, "FechaIngresoProduccionFotografia", "previos");
            ViewBag.EstatusConstruccionFotografia = _obrasManager.FindArchivos(previo.Id, "EstatusConstruccionFotografia", "previos");
            ViewBag.IngresoDepartamentoIngManFotografia = _obrasManager.FindArchivos(previo.Id, "IngresoDepartamentoIngManFotografia", "previos");
            ViewBag.SolicitudPagoInicialFabricaFotografia = _obrasManager.FindArchivos(previo.Id, "SolicitudPagoInicialFabricaFotografia", "previos");

            TempData["previo_id"] = previo.Id;
            TempData.Keep();
            if (previo == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Crear", "AdministrarPrevios", new { @id = id });
            }
            
            

            var previoModel = new PrevioViewModel()
            {
                FechaFirmaContrato = previo.FechaFirmaContrato.ToString("dd/MM/yyyy"),
                FechaFirmaContratoFotografia = previo.FechaFirmaContratoFotografia,
                FechaPagoInicialFabrica = previo.FechaPagoInicialFabrica.ToString("dd/MM/yyyy"),
                FechaPagoInicialFabricaFotografia = previo.FechaPagoInicialFabricaFotografia,
                FechaPagoInicialEquipo = previo.FechaPagoInicialEquipo.ToString("dd/MM/yyyy"),
                FechaPagoInicialEquipoFotografia = previo.FechaPagoInicialEquipoFotografia,
                FechaConstruccion = previo.FechaConstruccion.ToString("dd/MM/yyyy"),
                FechaConstruccionFotografia = previo.FechaConstruccionFotografia,
                FechaSalidaFabrica = previo.FechaSalidaFabrica.ToString("dd/MM/yyyy"),
                FechaSalidaFabricaFotografia = previo.FechaSalidaFabricaFotografia,
                FechaSalidaBuque = previo.FechaSalidaBuque.ToString("dd/MM/yyyy"),
                FechaSalidaBuqueFotografia = previo.FechaSalidaBuqueFotografia,
                FechaLlegadaBuque = previo.FechaLlegadaBuque.ToString("dd/MM/yyyy"),
                FechaLlegadaBuqueFotografia = previo.FechaLlegadaBuqueFotografia,
                FechaPeriodoNacionalizacion = previo.FechaPeriodoNacionalizacion.ToString("dd/MM/yyyy"),
                FechaPeriodoNacionalizacionFotografia = previo.FechaPeriodoNacionalizacionFotografia,
                FechaSalidaPuertoObra = previo.FechaSalidaPuertoObra.ToString("dd/MM/yyyy"),
                FechaSalidaPuertoObraFotografia = previo.FechaSalidaPuertoObraFotografia,
                FechaDescargaResguardo = previo.FechaDescargaResguardo.ToString("dd/MM/yyyy"),
                ActivacionProtocoloGarantiaFabrica = previo.ActivacionProtocoloGarantiaFabrica,
                FechaInicioMontaje = previo.FechaInicioMontaje.ToString("dd/MM/yyyy"),
                FechaInicioMontajeFotografia = previo.FechaInicioMontajeFotografia,
                FechaEntregaSoso = previo.FechaEntregaSoso.ToString("dd/MM/yyyy"),
                FechaEntregaSosoFotografia = previo.FechaEntregaSosoFotografia,
                FechaCulminacionMontaje = previo.FechaCulminacionMontaje.ToString("dd/MM/yyyy"),
                FechaConfiguracion = previo.FechaConfiguracion.ToString("dd/MM/yyyy"),
                FechaConfiguracionFotografia = previo.FechaConfiguracionFotografia,
                FechaPeriodoPrueba = previo.FechaPeriodoPrueba.ToString("dd/MM/yyyy"),
                FechaPeriodoPruebaFotografia = previo.FechaPeriodoPruebaFotografia,
                FechaInspeccion = previo.FechaInspeccion.ToString("dd/MM/yyyy"),
                TecnicoInstalador = previo.TecnicoInstalador,
                InformeEntregaFinal = previo.InformeEntregaFinal,
                FechaDescargaResguardoFotografia = previo.FechaDescargaResguardoFotografia,
                FechaCulminacionMontajeFotografia = previo.FechaCulminacionMontajeFotografia,
                FechaInspeccionFotografia = previo.FechaInspeccionFotografia,
                FechaInspeccionVideo = previo.FechaInspeccionVideo,
                previo_id = previo.Id,
                CartaPresentacion = previo.CartaPresentacion.ToString("dd/MM/yyyy"),
                FechaIngresoProduccion = previo.FechaIngresoProduccion.ToString("dd/MM/yyyy"),
                EstatusConstruccion = previo.EstatusConstruccion.ToString("dd/MM/yyyy"),
                IngresoDepartamentoIngMan = previo.IngresoDepartamentoIngMan.ToString("dd/MM/yyyy"),
                SolicitudPagoInicialFabrica = previo.SolicitudPagoInicialFabrica.ToString("dd/MM/yyyy")


            };

            return View(previoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARPREVIOS-MODIFICAR")]
        public ActionResult Editar(int id, PrevioViewModel model)
        {
            var equipo = _equiposManager.Find(id);
            var Url = "";
            ViewBag.Equipo = equipo;

            var idprevio = TempData["previo_id"];
            TempData.Keep();

            ViewBag.obra = _obrasManager.Find(equipo.obra_id);

            //var previo = _previosManager.Find(id);
            var previo = _previosManager.GetPrevio(id);


            try
            {
                //HttpPostedFileBase FechaDescargaResguardoFotografia = Request.Files["Pdf-FechaDescargaResguardoFotografia"];
                //HttpPostedFileBase FechaFirmaContratoFotografia = Request.Files["Pdf-FechaFirmaContratoFotografia"];
                //HttpPostedFileBase FechaPagoInicialFabricaFotografia = Request.Files["Pdf-FechaPagoInicialFabricaFotografia"];
                //HttpPostedFileBase FechaPagoInicialEquipoFotografia = Request.Files["Pdf-FechaPagoInicialEquipoFotografia"];
                //HttpPostedFileBase FechaConstruccionFotografia = Request.Files["Pdf-FechaConstruccionFotografia"];
                //HttpPostedFileBase FechaSalidaFabricaFotografia = Request.Files["Pdf-FechaSalidaFabricaFotografia"];
                //HttpPostedFileBase FechaSalidaBuqueFotografia = Request.Files["Pdf-FechaSalidaBuqueFotografia"];
                //HttpPostedFileBase FechaLlegadaBuqueFotografia = Request.Files["Pdf-FechaLlegadaBuqueFotografia"];
                //HttpPostedFileBase FechaPeriodoNacionalizacionFotografia = Request.Files["Pdf-FechaPeriodoNacionalizacionFotografia"];
                //HttpPostedFileBase FechaSalidaPuertoObraFotografia = Request.Files["Pdf-FechaSalidaPuertoObraFotografia"];
                ////HttpPostedFileBase ActivacionProtocoloGarantiaFabrica = Request.Files["Pdf-FechaDescargaResguardoFotografia"];
                //HttpPostedFileBase FechaInicioMontajeFotografia = Request.Files["Pdf-FechaInicioMontajeFotografia"];
                //HttpPostedFileBase FechaEntregaSosoFotografia = Request.Files["Pdf-FechaEntregaSosoFotografia"];
               
                //HttpPostedFileBase FechaConfiguracionFotografia = Request.Files["Pdf-FechaConfiguracionFotografia"];
                //HttpPostedFileBase FechaPeriodoPruebaFotografia = Request.Files["Pdf-FechaPeriodoPruebaFotografia"];
                //HttpPostedFileBase FechaCulminacionMontajeFotografia = Request.Files["Pdf-FechaCulminacionMontajeFotografia"];
                //HttpPostedFileBase fechainspeccion = Request.Files["Pdf"];
                //HttpPostedFileBase FechaInspeccionVideo = Request.Files["Pdf-FechaInspeccionVideo"];
                //HttpPostedFileBase InformeEntregaFinal = Request.Files["Pdf-InformeEntregaFinal"];


                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();


                    if (d == "Pdf-FechaDescargaResguardoFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaDescargaResguardoFotografia");
                    }
                    if (d == "Pdf-FechaFirmaContratoFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaFirmaContratoFotografia");
                    }
                    if (d == "Pdf-FechaPagoInicialFabricaFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaPagoInicialFabricaFotografia");
                    }
                    if (d == "Pdf-FechaPagoInicialEquipoFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaPagoInicialEquipoFotografia");
                    }
                    if (d == "Pdf-FechaConstruccionFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaConstruccionFotografia");
                    }
                    if (d == "Pdf-FechaSalidaBuqueFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaSalidaBuqueFotografia");
                    }
                    if (d == "Pdf-FechaLlegadaBuqueFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaLlegadaBuqueFotografia");
                    }
                    if (d == "Pdf-FechaPeriodoNacionalizacionFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaPeriodoNacionalizacionFotografia");
                    }


                    if (d == "Pdf-FechaSalidaPuertoObraFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaSalidaPuertoObraFotografia");
                    }
                    if (d == "Pdf-FechaInicioMontajeFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaInicioMontajeFotografia");
                    }

                    if (d == "Pdf-FechaEntregaSosoFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaEntregaSosoFotografia");
                    }



                    if (d == "Pdf-FechaConfiguracionFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaConfiguracionFotografia");
                    }
                    if (d == "Pdf-FechaPeriodoPruebaFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaPeriodoPruebaFotografia");
                    }
                    if (d == "Pdf-FechaCulminacionMontajeFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaCulminacionMontajeFotografia");
                    }
                    if (d == "Pdf-Pdf" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "Pdf");
                    }
                    if (d == "Pdf-FechaInspeccionVideo" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaInspeccionVideo");
                    }
                    if (d == "Pdf-InformeEntregaFinal" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "InformeEntregaFinal");
                    }

                }

                _previosManager.Actualizar(
                      Convert.ToInt32(idprevio),
                      model.FechaFirmaContrato,
                      model.FechaPagoInicialFabrica,
                      model.FechaPagoInicialEquipo,
                      model.FechaConstruccion,
                      model.FechaSalidaFabrica,
                      model.FechaSalidaBuque,
                      model.FechaLlegadaBuque,
                      model.FechaPeriodoNacionalizacion,
                      model.FechaSalidaPuertoObra,
                      model.FechaDescargaResguardo,
                      model.ActivacionProtocoloGarantiaFabrica,
                      model.FechaInicioMontaje,
                      model.FechaEntregaSoso,
                      model.FechaCulminacionMontaje,
                      model.FechaConfiguracion,
                      model.FechaPeriodoPrueba,
                      model.FechaInspeccion,
                      model.TecnicoInstalador,
                      model.CartaPresentacion, model.FechaIngresoProduccion, model.EstatusConstruccion, model.IngresoDepartamentoIngMan, model.SolicitudPagoInicialFabrica);

                 TempData["FlashSuccess"] = MensajesResource.INFO_Previos_ActualizadoCorrectamente;
                return RedirectToAction("Editar","AdministrarPrevios", new { @id = id });
            }
            catch (BusinessException businessEx)
            {
                ModelState.AddModelError(string.Empty, businessEx.Message);

                return View(model);
            }
            catch (Exception e)
            {
                var log = CommonManager.BuildMessageLog(
                    TipoMensaje.Error,
                    ControllerContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString(),
                    ControllerContext.Controller.ValueProvider.GetValue("action").RawValue.ToString(),
                    e.ToString(), Request);

                CommonManager.WriteAppLog(log, TipoMensaje.Error);

                return View(model);
            }

        }



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARPREVIOS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(PrevioViewModel model, FormCollection collection)
        {
            var Url = "";
            //var pdfUrlFechaInspeccion = "";
            //var pdfInformeEntregaFinal = "";
            //var pdfFechaDescargaResguardoFotografia = "";
            //var pdfFechaCulminacionMontajeFotografia = "";
            //var pdfFechaInspeccionVideo = "";


            //var pdfFechaFirmaContratoFotografia = "";
            //var pdfFechaPagoInicialFabricaFotografia = "";
            //var pdfFechaPagoInicialEquipoFotografia = "";
            //var pdfFechaConstruccionFotografia = "";
            //var pdfFechaSalidaFabricaFotografia = "";
            //var pdfFechaSalidaBuqueFotografia = "";
            //var pdfFechaLlegadaBuqueFotografia = "";
            //var pdfFechaPeriodoNacionalizacionFotografia = "";
            //var pdfFechaSalidaPuertoObraFotografia = "";

            //var pdfFechaInicioMontajeFotografia = "";
            //var pdfFechaEntregaSosoFotografia = "";
            //var pdfFechaCulminacionMontaje = "";
            //var pdfFechaConfiguracionFotografia = "";
            //var pdfFechaPeriodoPruebaFotografia = "";

            //var pdffechainspeccion = "";

            // if (!ModelState.IsValid) return View(model);

            try
            {

                

                HttpPostedFileBase FechaDescargaResguardoFotografia = Request.Files["Pdf-FechaDescargaResguardoFotografia"];
                HttpPostedFileBase FechaFirmaContratoFotografia = Request.Files["Pdf-FechaFirmaContratoFotografia"];
                HttpPostedFileBase FechaPagoInicialFabricaFotografia = Request.Files["Pdf-FechaPagoInicialFabricaFotografia"];
                HttpPostedFileBase FechaPagoInicialEquipoFotografia = Request.Files["Pdf-FechaPagoInicialEquipoFotografia"];
                HttpPostedFileBase FechaConstruccionFotografia = Request.Files["Pdf-FechaConstruccionFotografia"];
                HttpPostedFileBase FechaSalidaFabricaFotografia = Request.Files["Pdf-FechaSalidaFabricaFotografia"];
                HttpPostedFileBase FechaSalidaBuqueFotografia = Request.Files["Pdf-FechaSalidaBuqueFotografia"];
                HttpPostedFileBase FechaLlegadaBuqueFotografia = Request.Files["Pdf-FechaLlegadaBuqueFotografia"];
                HttpPostedFileBase FechaPeriodoNacionalizacionFotografia = Request.Files["Pdf-FechaPeriodoNacionalizacionFotografia"];
                HttpPostedFileBase FechaSalidaPuertoObraFotografia = Request.Files["Pdf-FechaSalidaPuertoObraFotografia"];
                //HttpPostedFileBase ActivacionProtocoloGarantiaFabrica = Request.Files["Pdf-FechaDescargaResguardoFotografia"];
                HttpPostedFileBase FechaInicioMontajeFotografia = Request.Files["Pdf-FechaInicioMontajeFotografia"];
                HttpPostedFileBase FechaEntregaSosoFotografia = Request.Files["Pdf-FechaEntregaSosoFotografia"];

                HttpPostedFileBase FechaConfiguracionFotografia = Request.Files["Pdf-FechaConfiguracionFotografia"];
                HttpPostedFileBase FechaPeriodoPruebaFotografia = Request.Files["Pdf-FechaPeriodoPruebaFotografia"];
                HttpPostedFileBase FechaCulminacionMontajeFotografia = Request.Files["Pdf-FechaCulminacionMontajeFotografia"];
                HttpPostedFileBase fechainspeccion = Request.Files["Pdf"];
                HttpPostedFileBase FechaInspeccionVideo = Request.Files["Pdf-FechaInspeccionVideo"];
                HttpPostedFileBase InformeEntregaFinal = Request.Files["Pdf-InformeEntregaFinal"];

               

                previos previo = _previosManager.Crear(model.FechaFirmaContrato,

                      model.FechaPagoInicialFabrica,

                      model.FechaPagoInicialEquipo,

                      model.FechaConstruccion,

                      model.FechaSalidaFabrica,

                      model.FechaSalidaBuque,

                      model.FechaLlegadaBuque,

                      model.FechaPeriodoNacionalizacion,

                      model.FechaSalidaPuertoObra,

                      model.FechaDescargaResguardo,
                      model.ActivacionProtocoloGarantiaFabrica,
                      model.FechaInicioMontaje,

                      model.FechaEntregaSoso,

                      model.FechaCulminacionMontaje,
                      model.FechaConfiguracion,

                      model.FechaPeriodoPrueba,

                      model.FechaInspeccion,
                      model.TecnicoInstalador,
                      model.CartaPresentacion,model.FechaIngresoProduccion,model.EstatusConstruccion,model.IngresoDepartamentoIngMan,model.SolicitudPagoInicialFabrica);
                      


                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();


                    if (d == "Pdf-FechaDescargaResguardoFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaDescargaResguardoFotografia");
                    }
                    if (d == "Pdf-FechaFirmaContratoFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaFirmaContratoFotografia");
                    }
                    if (d == "Pdf-FechaPagoInicialFabricaFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaPagoInicialFabricaFotografia");
                    }
                    if (d == "Pdf-FechaPagoInicialEquipoFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaPagoInicialEquipoFotografia");
                    }
                    if (d == "Pdf-FechaConstruccionFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaConstruccionFotografia");
                    }
                    if (d == "Pdf-FechaSalidaBuqueFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaSalidaBuqueFotografia");
                    }
                    if (d == "Pdf-FechaLlegadaBuqueFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaLlegadaBuqueFotografia");
                    }
                    if (d == "Pdf-FechaPeriodoNacionalizacionFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaPeriodoNacionalizacionFotografia");
                    }


                    if (d == "Pdf-FechaSalidaPuertoObraFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaSalidaPuertoObraFotografia");
                    }
                    if (d == "Pdf-FechaInicioMontajeFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaInicioMontajeFotografia");
                    }
                  
                    if (d == "Pdf-FechaEntregaSosoFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaEntregaSosoFotografia");
                    }



                    if (d == "Pdf-FechaConfiguracionFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaConfiguracionFotografia");
                    }
                    if (d == "Pdf-FechaPeriodoPruebaFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaPeriodoPruebaFotografia");
                    }
                    if (d == "Pdf-FechaCulminacionMontajeFotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaCulminacionMontajeFotografia");
                    }
                    if (d == "Pdf-Pdf" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "Pdf");
                    }
                    if (d == "Pdf-FechaInspeccionVideo" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaInspeccionVideo");
                    }
                    if (d == "Pdf-InformeEntregaFinal" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "InformeEntregaFinal");
                    }


                    if (d == "Pdf-CartaPresentacion" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "CartaPresentacion");
                    }
                    if (d == "Pdf-FechaIngresoProduccion" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "FechaIngresoProduccion");
                    }
                    if (d == "Pdf-EstatusConstruccion" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "EstatusConstruccion");
                    }
                    if (d == "Pdf-IngresoDepartamentoIngMan" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "IngresoDepartamentoIngMan");
                    }
                    if (d == "Pdf-SolicitudPagoInicialFabrica" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(previo.Id, Url, "previos", "SolicitudPagoInicialFabrica");
                    }

                }


                //HttpPostedFileBase FechaDescargaResguardoFotografia = Request.Files["Pdf-FechaDescargaResguardoFotografia"];
                //HttpPostedFileBase FechaFirmaContratoFotografia = Request.Files["Pdf-FechaFirmaContratoFotografia"];

                //HttpPostedFileBase FechaPagoInicialFabricaFotografia = Request.Files["Pdf-FechaPagoInicialFabricaFotografia"];
                //HttpPostedFileBase FechaPagoInicialEquipoFotografia = Request.Files["Pdf-FechaPagoInicialEquipoFotografia"];
                //HttpPostedFileBase FechaConstruccionFotografia = Request.Files["Pdf-FechaConstruccionFotografia"];
                //HttpPostedFileBase FechaSalidaFabricaFotografia = Request.Files["Pdf-FechaSalidaFabricaFotografia"];
                //HttpPostedFileBase FechaSalidaBuqueFotografia = Request.Files["Pdf-FechaSalidaBuqueFotografia"];
                //HttpPostedFileBase FechaLlegadaBuqueFotografia = Request.Files["Pdf-FechaLlegadaBuqueFotografia"];
                //HttpPostedFileBase FechaPeriodoNacionalizacionFotografia = Request.Files["Pdf-FechaPeriodoNacionalizacionFotografia"];
                //HttpPostedFileBase FechaSalidaPuertoObraFotografia = Request.Files["Pdf-FechaSalidaPuertoObraFotografia"];
                ////HttpPostedFileBase ActivacionProtocoloGarantiaFabrica = Request.Files["Pdf-FechaDescargaResguardoFotografia"];
                //HttpPostedFileBase FechaInicioMontajeFotografia = Request.Files["Pdf-FechaInicioMontajeFotografia"];
                //HttpPostedFileBase FechaEntregaSosoFotografia = Request.Files["Pdf-FechaEntregaSosoFotografia"];


                //HttpPostedFileBase FechaConfiguracionFotografia = Request.Files["Pdf-FechaConfiguracionFotografia"];
                //HttpPostedFileBase FechaPeriodoPruebaFotografia = Request.Files["Pdf-FechaPeriodoPruebaFotografia"];
                //HttpPostedFileBase FechaCulminacionMontajeFotografia = Request.Files["Pdf-FechaCulminacionMontajeFotografia"];


                //HttpPostedFileBase fechainspeccion = Request.Files["Pdf"];
                //HttpPostedFileBase FechaInspeccionVideo = Request.Files["Pdf-FechaInspeccionVideo"];
                //HttpPostedFileBase InformeEntregaFinal = Request.Files["Pdf-InformeEntregaFinal"];

                TempData["FlashSuccess"] = MensajesResource.INFO_Previos_CreadoCorrectamente;
                return RedirectToAction("Editar", "AdministrarPrevios", new { @id = TempData["previo_id"] });
            }
            catch (BusinessException businessEx)
            {
                ModelState.AddModelError(string.Empty, businessEx.Message);

                return View(model);
            }
            catch (Exception e)
            {
                var log = CommonManager.BuildMessageLog(
                    TipoMensaje.Error,
                    ControllerContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString(),
                    ControllerContext.Controller.ValueProvider.GetValue("action").RawValue.ToString(),
                    e.ToString(), Request);

                CommonManager.WriteAppLog(log, TipoMensaje.Error);

                ModelState.AddModelError(string.Empty, e.Message);
                return View(model);
            }
        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARPREVIOS-MODIFICAR")]
        public ActionResult EliminarArchivo(string archivo, string tipo, string caracteristica, int id)
        {
            try
            {
                _obrasManager.EliminarArchivo(archivo, tipo, caracteristica);
                TempData["FlashSuccess"] = MensajesResource.INFO_Obras_ActualizadoCorrectamente;
                return RedirectToAction("Editar", "AdministrarPrevios", new { @id = TempData["equipo_id"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Editar", "AdministrarPrevios", new { @id = TempData["equipo_id"] });

            }
            catch (Exception e)
            {
                var log = CommonManager.BuildMessageLog(
                    TipoMensaje.Error,
                    ControllerContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString(),
                    ControllerContext.Controller.ValueProvider.GetValue("action").RawValue.ToString(),
                    e.ToString(), Request);

                CommonManager.WriteAppLog(log, TipoMensaje.Error);

                TempData["FlashError"] = MensajesResource.ERROR_General;
                return RedirectToAction("Editar", "AdministrarEquipos", new { @id = TempData["equipo_id"] });
            }

        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARPREVIOS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _previosManager.Eliminar(id);
                TempData["FlashSuccess"] = MensajesResource.INFO_Previos_EliminadoCorrectamente;
                return RedirectToAction("Editar", "AdministrarPrevios", new { @id = TempData["previo_id"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Editar", "AdministrarPrevios", new { @id = TempData["previo_id"] });

            }
            catch (Exception e)
            {
                var log = CommonManager.BuildMessageLog(
                    TipoMensaje.Error,
                    ControllerContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString(),
                    ControllerContext.Controller.ValueProvider.GetValue("action").RawValue.ToString(),
                    e.ToString(), Request);

                CommonManager.WriteAppLog(log, TipoMensaje.Error);

                TempData["FlashError"] = MensajesResource.ERROR_General;
                return RedirectToAction("Editar", "AdministrarPrevios", new { @id = TempData["previo_id"] });
            }

        }
    }
}