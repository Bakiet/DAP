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
    public class AdministrarInformesController : Controller
    {
        private readonly ComponentesMecanicosManager _componentesmecanicosManager = new ComponentesMecanicosManager();
        private readonly ComponentesElectricosManager _componentesElectricos_Manager = new ComponentesElectricosManager();
        private readonly InformesManager _informesManager = new InformesManager();

        private readonly VentasManager _ventasManager = new VentasManager();

        private readonly ObrasManager _obrasManager = new ObrasManager();

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

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARINFORMES-CONSULTAR")]

        public ActionResult Index(int id)
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);

            TempData["venta_id"] = id;
            TempData.Keep();

            //var informe = _informesManager.Find(id);
            ViewBag.ComponentesMecanicosCount = _componentesmecanicosManager.GetSustituciones();
            ViewBag.ComponentesMecanicos = _componentesmecanicosManager.GetSustituciones();
            TempData["sustitucionesmecanicas"] = ViewBag.ComponentesMecanicosCount.Count;
            TempData.Keep();

            ViewBag.ComponentesElectricosCount = _componentesElectricos_Manager.GetSustituciones();
            ViewBag.ComponentesElectricos = _componentesElectricos_Manager.GetSustituciones();
            TempData["sustitucioneselectronicas"] = ViewBag.ComponentesElectricosCount.Count;
            TempData.Keep();

            var mantenimiento = _informesManager.GetInformeVenta(id);

            if (mantenimiento != null)
            {
                var venta = _ventasManager.Find(mantenimiento.IdVenta);


                TempData["IDVENTA"] = venta.Id;
                TempData.Keep();
                ViewBag.obra = _obrasManager.Find(venta.IdObra);

                TempData["IDOBRA"] = venta.IdObra;
                TempData.Keep();
                ViewBag.Informes = db.informes.ToList();

                if (id != 0)
                {
                    ViewBag.Informes = _informesManager.GetInformes(venta.Id);
                }
                else
                {
                    ViewBag.Informes = db.informes.ToList();
                };
            }
            else
            {

                ViewBag.venta = null;
                ViewBag.obra = null;


                ViewBag.Informes = null;

                // ViewBag.obra = _obrasManager.Find(venta.IdObra);
                //TempData["IDOBRA"] = venta.IdObra;
                // TempData.Keep();
            }

            //ViewBag.EstatusCita = db.estatuscitas.ToList();

            return View();
        }

        public ActionResult Informes(int id)
        {
            // var informe = _informesManager.Find(id);

            var mantenimiento = _informesManager.GetInforme(id);

            var venta = _ventasManager.Find(id);

            ViewBag.obra = _obrasManager.Find(venta.IdObra);

            ViewBag.Informes = _informesManager.GetInformes(id);
            return View("Index");
        }

        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-LISTAR,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-MODIFICAR")]
        //public ActionResult Index()
        //{
        //    ViewBag.mensajes = _mensajesInstitucionalesManager.FindAll();
        //    return View();
        //}
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARINFORMES-CREAR")]
        public ActionResult Crear()
        {
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARINFORMES-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var informes = _informesManager.Find(id);

            var mantenimiento = _informesManager.GetInforme(id);

            var venta = _ventasManager.Find(id);

            ViewBag.obra = _obrasManager.Find(venta.IdObra);

            ViewBag.Informes = _informesManager.GetInformes(id);

            if (informes == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            if(informes.Nombre == null)
            {
                informes.Nombre = "null";
            }
            if(informes.Url == null)
            {
                informes.Url = "null";
            }
            if(informes.Fecha == null)
            {
                informes.Fecha = DateTime.Now;
            }
            var informeModel = new InformeViewModel()
            {
                Nombre = informes.Nombre,
                Url = informes.Url,
                Fecha = informes.Fecha.ToString(),
            };

            return View(informeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARINFORMES-MODIFICAR")]
        public ActionResult Editar(int id, InformeViewModel model)
        {
            var informe = _informesManager.Find(id);
            var pdfUrl = "";
            var mantenimiento = _informesManager.GetInforme(id);

            var venta = _ventasManager.Find(id);

            ViewBag.obra = _obrasManager.Find(venta.IdObra);

            ViewBag.Informes = _informesManager.GetInformes(id);

            if (informe.Nombre == null)
            {
                informe.Nombre = "null";
            }
            if (informe.Url == null)
            {
                informe.Url = "null";
            }
            if (informe.Fecha == null)
            {
                informe.Fecha = DateTime.Now;
            }

            if (informe == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {
                HttpPostedFileBase pdf = Request.Files["Pdf"];

                if (pdf != null && pdf.ContentLength > 0)
                {
                    pdfUrl = CargarPdf(pdf);
                }
                else { pdfUrl = informe.Url; }

                _informesManager.Actualizar(Convert.ToInt32(TempData["IDVENTA"]),
                      id,
                      model.Nombre,
                      pdfUrl.Trim(),
                      model.Fecha);
                TempData.Keep();
                TempData["FlashSuccess"] = MensajesResource.INFO_Informes_ActualizadoCorrectamente;
                return RedirectToAction("Index", "AdministrarInformes", new { @id = TempData["IDVENTA"] });
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



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARINFORMES-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(InformeViewModel model, FormCollection collection)
        {
            var pdfUrl = "";

           

            try
            {
                HttpPostedFileBase pdf = Request.Files["Pdf"];

                if (pdf != null && pdf.ContentLength > 0)
                {
                    pdfUrl = CargarPdf(pdf);
                }
                else { pdfUrl = ""; }
                _informesManager.Crear(Convert.ToInt32(TempData["venta_id"]),
                      model.Nombre,
                      pdfUrl.Trim(),
                      model.Fecha);
                TempData.Keep();

                TempData["FlashSuccess"] = MensajesResource.INFO_Informes_CreadoCorrectamente;
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "AdministrarInformes", new { @id = TempData["venta_id"] });
            }
            catch (BusinessException businessEx)
            {
                ModelState.AddModelError(string.Empty, businessEx.Message);

                return RedirectToAction("Index", "AdministrarInformes", new { @id = TempData["venta_id"] });
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
                return RedirectToAction("Index", "AdministrarInformes", new { @id = TempData["venta_id"] });
            }
        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARINFORMES-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _informesManager.Eliminar(id);
                // TempData["FlashSuccess"] = MensajesResource.INFO_MensajesInstitucionales_EliminadoCorrectamente;
                return RedirectToAction("Index", "AdministrarInformes", new { @id = TempData["IDVENTA"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Index", "AdministrarInformes", new { @id = TempData["IDVENTA"] });

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
                return RedirectToAction("Index", "AdministrarInformes", new { @id = TempData["IDVENTA"] });
            }

        }
    }
}