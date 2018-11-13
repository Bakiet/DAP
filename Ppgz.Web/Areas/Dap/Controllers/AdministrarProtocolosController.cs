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
    public class AdministrarProtocolosController : Controller
    {
        private readonly ProtocolosManager _protocolosManager = new ProtocolosManager();
        private readonly ObrasManager _obrasManager = new ObrasManager();
        private readonly PreviosManager _previosManager = new PreviosManager();
        private readonly EquiposManager _equiposManager = new EquiposManager();
        private readonly ComponentesMecanicosManager _componentesmecanicosManager = new ComponentesMecanicosManager();
        private readonly ComponentesElectricosManager _componentesElectricos_Manager = new ComponentesElectricosManager();

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
        //asignar los roles para esta funcionalidad
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARPROTOCOLOS-CONSULTAR")]
        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);


            ViewBag.Protocolos = db.protocolos.ToList();


            ViewBag.ComponentesMecanicosCount = _componentesmecanicosManager.GetSustituciones();
            ViewBag.ComponentesMecanicos = _componentesmecanicosManager.GetSustituciones();
            TempData["sustitucionesmecanicas"] = ViewBag.ComponentesMecanicosCount.Count;
            TempData.Keep();

            ViewBag.ComponentesElectricosCount = _componentesElectricos_Manager.GetSustituciones();
            ViewBag.ComponentesElectricos = _componentesElectricos_Manager.GetSustituciones();
            TempData["sustitucioneselectronicas"] = ViewBag.ComponentesElectricosCount.Count;
            TempData.Keep();

            //ViewBag.EstatusCita = db.estatuscitas.ToList();

            return View();
        }

        public ActionResult Protocolos(int id)
        {
            var previo = _previosManager.Find(id);

            TempData["previo_id"] = id;
            TempData.Keep();
            var equipo = _equiposManager.Find(previo.equipo_id);

            ViewBag.obra = _obrasManager.Find(equipo.obra_id);
            var obra = ViewBag.obra = _obrasManager.Find(equipo.obra_id);
            TempData["obra_id"] = equipo.obra_id;
            TempData.Keep();
            ViewBag.Protocolos = _protocolosManager.GetProtocolos(id);
            return View("Index");
        }

      
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARPROTOCOLOS-CREAR")]
        public ActionResult Crear()
        {
            ViewBag.Obras =
             new SelectList(_obrasManager.FindObras(Convert.ToInt32(TempData["obra_id"])), "id", "nombre", TempData["obra_id"]);
            TempData.Keep();
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARPROTOCOLOS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var protocolo = _protocolosManager.Find(id);

            if(protocolo.Nombre == null)
            {
                protocolo.Nombre = "null";
            }
            if (protocolo.Url == null)
            {
                protocolo.Url = "null";
            }
            ViewBag.Obras =
              new SelectList(_obrasManager.FindObras(Convert.ToInt32(TempData["obra_id"])), "id", "nombre", Convert.ToInt32(TempData["obra_id"]));
            TempData.Keep();
            if (protocolo == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var protocoloModel = new ProtocoloViewModel()
            {
                Nombre = protocolo.Nombre,
                Url = protocolo.Url,
                obra_id = protocolo.obra_id
               

            };

            return View(protocoloModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARPROTOCOLOS-MODIFICAR")]
        public ActionResult Editar(int id, ProtocoloViewModel model)
        {
            var protocolo = _protocolosManager.Find(id);
            var pdfUrl = "";
            ViewBag.Obras =
            new SelectList(_obrasManager.FindObras(protocolo.obra_id), "id", "nombre", protocolo.obra_id);
            if (protocolo == null)
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
                else { pdfUrl = protocolo.Url; }

                if(pdfUrl == null)
                {
                    pdfUrl = "null.jpg";
                }
                

                _protocolosManager.Actualizar(protocolo.obra_id,
                      id,
                      model.Nombre,
                      pdfUrl.Trim());

                 TempData["FlashSuccess"] = MensajesResource.INFO_Protocolos_ActualizadoCorrectamente;
                //return RedirectToAction("Editar", "AdministrarProtocolos", new { @id = id });
                return RedirectToAction("Protocolos", "AdministrarProtocolos", new { @id = TempData["previo_id"] });
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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARPROTOCOLOS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(ProtocoloViewModel model, FormCollection collection)
        {
            var pdfUrl = "";
            //ViewBag.Obras =
            //new SelectList(_obrasManager.FindAll(), "id", "nombre");
            ViewBag.Obras =
              new SelectList(_obrasManager.FindObras(model.obra_id), "id", "nombre", model.obra_id);

            if (!ModelState.IsValid) return View(model);

            try
            {
                HttpPostedFileBase pdf = Request.Files["Pdf"];

                if (pdf != null && pdf.ContentLength > 0)
                {
                    pdfUrl = CargarPdf(pdf);
                }
                else { pdfUrl = "null"; }
                _protocolosManager.Crear(Convert.ToInt32(TempData["previo_id"]),model.obra_id,
                       model.Nombre,
                       pdfUrl.Trim());

                TempData["FlashSuccess"] = MensajesResource.INFO_Protocolos_CreadoCorrectamente;
                return RedirectToAction("Protocolos", "AdministrarProtocolos", new { @id = TempData["previo_id"] });
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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARPROTOCOLOS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _protocolosManager.Eliminar(id);
                TempData["FlashSuccess"] = MensajesResource.INFO_Protocolos_EliminadoCorrectamente;
                return RedirectToAction("Protocolos", "AdministrarProtocolos", new { @id = TempData["previo_id"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Protocolos", "AdministrarProtocolos", new { @id = TempData["previo_id"] });

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
                return RedirectToAction("Protocolos", "AdministrarProtocolos", new { @id = TempData["previo_id"] });
            }

        }
    }
}