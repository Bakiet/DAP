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
    public class AdministrarKitDapsController : Controller
    {
        private readonly KitDapsManager _kitdapsManager = new KitDapsManager();
        private readonly ObrasManager _obrasManager = new ObrasManager();
        private readonly PreviosManager _previosManager = new PreviosManager();
        private readonly EquiposManager _equiposManager = new EquiposManager();
        private readonly ComponentesMecanicosManager _componentesmecanicosManager = new ComponentesMecanicosManager();
        private readonly ComponentesElectricosManager _componentesElectricos_Manager = new ComponentesElectricosManager();

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARKITDAPS-CONSULTAR")]
        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);


            ViewBag.KitDaps = db.kitdap.ToList();


            //ViewBag.EstatusCita = db.estatuscitas.ToList();
            ViewBag.ComponentesMecanicosCount = _componentesmecanicosManager.GetSustituciones();
            ViewBag.ComponentesMecanicos = _componentesmecanicosManager.GetSustituciones();
            TempData["sustitucionesmecanicas"] = ViewBag.ComponentesMecanicosCount.Count;
            TempData.Keep();

            ViewBag.ComponentesElectricosCount = _componentesElectricos_Manager.GetSustituciones();
            ViewBag.ComponentesElectricos = _componentesElectricos_Manager.GetSustituciones();
            TempData["sustitucioneselectronicas"] = ViewBag.ComponentesElectricosCount.Count;
            TempData.Keep();
            return View();
        }

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

        public ActionResult KitDaps(int id)
        {
            var previo = _previosManager.Find(id);

            TempData["previo_id"] = id;
            TempData.Keep();

            var equipo = _equiposManager.Find(previo.equipo_id);

            ViewBag.obra = _obrasManager.Find(equipo.obra_id);

            TempData["obra_id"] = equipo.obra_id;
            TempData.Keep();
            ViewBag.KitDaps = _kitdapsManager.GetKitDaps(id);
            return View("Index");
        }

        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-LISTAR,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-MODIFICAR")]
        //public ActionResult Index()
        //{
        //    ViewBag.mensajes = _mensajesInstitucionalesManager.FindAll();
        //    return View();
        //}
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARKITDAPS-CREAR")]
        public ActionResult Crear()
        {
            if(TempData["obra_id"] != null)
            {
                ViewBag.Obras =
                new SelectList(_obrasManager.FindAll(), "id", "nombre", TempData["obra_id"]);
                TempData.Keep();
            }
            else
            {
                ViewBag.Obras =
                new SelectList(_obrasManager.FindAll(), "id", "nombre");
            }
            
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARKITDAPS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var kitdap = _kitdapsManager.Find(id);
            TempData["id_previo"] = kitdap.IdPrevio;
            TempData.Keep();
            var previo = _previosManager.Find(Convert.ToInt32(TempData["id_previo"]));
            var equipo = _equiposManager.Find(previo.Id);

            ViewBag.Obras2 =
               new SelectList(_obrasManager.FindAll(), "id", "nombre", TempData["obra_id"]);

            //ViewBag.Obras =
            //  new SelectList(_obrasManager.FindObras(kitdap.obra_id), "id", "nombre", kitdap.obra_id);
            // ViewBag.equipo =new SelectList(_fallasManager.FindEquiposPorObra(id), "nombre", "nombre", falla.Equipo);


            if (kitdap == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var kitdapModel = new KitDapViewModel()
            {
                Nombre = kitdap.Nombre,
                Url = kitdap.Url,
                obra_id = kitdap.obra_id
               
            };

            return View(kitdapModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARKITDAPS-MODIFICAR")]
        public ActionResult Editar(int id, KitDapViewModel model)
        {
            var kitdap = _kitdapsManager.Find(id);
            var pdfUrl = "";
            //ViewBag.Obras =
            // new SelectList(_obrasManager.FindObras(kitdap.obra_id), "id", "nombre", kitdap.obra_id);

            TempData["id_previo"] = kitdap.IdPrevio;
            TempData.Keep();
            var previo = _previosManager.Find(Convert.ToInt32(TempData["id_previo"]));
            var equipo = _equiposManager.Find(previo.Id);

            ViewBag.Obras2 =
               new SelectList(_obrasManager.FindAll(), "id", "nombre", equipo.obra_id);
            if (kitdap == null)
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
                else { pdfUrl = kitdap.Url; }

                _kitdapsManager.Actualizar(equipo.obra_id,
                      id,
                      model.Nombre,
                      pdfUrl.Trim());

                TempData["FlashSuccess"] = MensajesResource.INFO_Kitdaps_ActualizadoCorrectamente;
                //return RedirectToAction("Editar", "AdministrarKitDaps", new { @id = id });
                return RedirectToAction("Kitdaps", "AdministrarKitDaps", new { @id = TempData["previo_id"] });
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



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARKITDAPS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(KitDapViewModel model, FormCollection collection)
        {
            var pdfUrl = "";

            ViewBag.Obras =
            new SelectList(_obrasManager.FindAll(), "id", "nombre", TempData["obra_id"]);
            TempData.Keep();

            if (!ModelState.IsValid) return View(model);

            try
            {
                HttpPostedFileBase pdf = Request.Files["Pdf"];

                if (pdf != null && pdf.ContentLength > 0)
                {
                    pdfUrl = CargarPdf(pdf);
                }
                else { pdfUrl = ""; }

                _kitdapsManager.Crear(Convert.ToInt32(TempData["previo_id"]),model.obra_id,
                      model.Nombre,
                      pdfUrl.Trim());
                TempData.Keep();

               TempData["FlashSuccess"] = MensajesResource.INFO_Kitdaps_CreadoCorrectamente;
                return RedirectToAction("Kitdaps","AdministrarKitDaps", new { @id = TempData["previo_id"] } );


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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARKITDAPS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _kitdapsManager.Eliminar(id);
                TempData["FlashSuccess"] = MensajesResource.INFO_Kitdaps_EliminadoCorrectamente;
                return RedirectToAction("Kitdaps", "AdministrarKitDaps", new { @id = TempData["previo_id"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Kitdaps", "AdministrarKitDaps", new { @id = TempData["previo_id"] });

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
                return RedirectToAction("Kitdaps", "AdministrarKitDaps", new { @id = TempData["previo_id"] });
            }

        }
    }
}