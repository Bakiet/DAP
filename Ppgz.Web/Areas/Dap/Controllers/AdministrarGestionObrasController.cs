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
    public class AdministrarGestionObrasController : Controller
    {
        private readonly GestionObrasManager _gestionobrasManager = new GestionObrasManager();


        //asignar los roles para esta funcionalidad
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARGESTIONOBRAS-CONSULTAR")]
        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);

            ViewBag.GestionObras = db.obrasgestion.ToList();

            return View();
        }
       
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARGESTIONOBRAS-CREAR")]
        public ActionResult Crear()
        {
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARGESTIONOBRAS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var gestionobra = _gestionobrasManager.Find(id);

            if (gestionobra == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var gestionobraModel = new GestionObraViewModel()
            {
                primercontacto = gestionobra.primercontacto,
                contactotelefonicoobra = gestionobra.contactotelefonicoobra,
                correoelectronicoobra = gestionobra.correoelectronicoobra,
                cargoobra = gestionobra.cargoobra,
                nombreconstructora = gestionobra.nombreconstructora,
                contactotelefonicoempresa = gestionobra.contactotelefonicoempresa,
                correoelectronicoempresa = gestionobra.correoelectronicoempresa,
                cargoempresa = gestionobra.cargoempresa,
                direccionoficina = gestionobra.direccionoficina,
                ubicacionobra = gestionobra.ubicacionobra,
                tipoobra = gestionobra.tipoobra,
                numeroequipos = gestionobra.numeroequipos,
                tipoequipo = gestionobra.tipoequipo,
                vendidasclimb = gestionobra.vendidasclimb,
                vendidasotros = gestionobra.vendidasotros,
                novendidas = gestionobra.novendidas,
                paralizadas = gestionobra.paralizadas
            };

            return View(gestionobraModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARGESTIONOBRAS-MODIFICAR")]
        public ActionResult Editar(int id, GestionObraViewModel model)
        {
            var obra = _gestionobrasManager.Find(id);

            if (obra == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {
                _gestionobrasManager.Actualizar(
                      id,
                      model.primercontacto,
                       model.contactotelefonicoobra,
                       model.correoelectronicoobra,
                       model.cargoobra,
                       model.nombreconstructora,
                       model.contactotelefonicoempresa,
                       model.correoelectronicoempresa,
                       model.cargoempresa,
                       model.direccionoficina,
                       model.ubicacionobra,
                       model.tipoobra,
                       model.numeroequipos,
                       model.tipoequipo,
                       model.vendidasclimb,
                       model.vendidasotros,
                       model.novendidas,
                       model.paralizadas);

                 TempData["FlashSuccess"] = MensajesResource.INFO_GestionObras_ActualizadoCorrectamente;
                return RedirectToAction("Index");
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



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARGESTIONOBRAS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(GestionObraViewModel model, FormCollection collection)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {

                _gestionobrasManager.Crear(
                       model.primercontacto,
                       model.contactotelefonicoobra,
                       model.correoelectronicoobra,
                       model.cargoobra,
                       model.nombreconstructora,
                       model.contactotelefonicoempresa,
                       model.correoelectronicoempresa,
                       model.cargoempresa,
                       model.direccionoficina,
                       model.ubicacionobra,
                       model.tipoobra,
                       model.numeroequipos,
                       model.tipoequipo,
                       model.vendidasclimb,
                       model.vendidasotros,
                       model.novendidas,
                       model.paralizadas);


                TempData["FlashSuccess"] = MensajesResource.INFO_GestionObras_CreadoCorrectamente;
                return RedirectToAction("Index");
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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARGESTIONOBRAS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _gestionobrasManager.Eliminar(id);
                TempData["FlashSuccess"] = MensajesResource.INFO_GestionObras_EliminadoCorrectamente;
                return RedirectToAction("Index");
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Index");

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
                return RedirectToAction("Index");
            }

        }
    }
}