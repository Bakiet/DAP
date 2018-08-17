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
    public class AdministrarDatosObrasController : Controller
    {
        private readonly DatoObrasManager _datoobrasManager = new DatoObrasManager();


        //asignar los roles para esta funcionalidad
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARGESTIONOBRAS-CONSULTAR")]
        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);

            ViewBag.DatoObras = db.obrasdato.ToList();

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
            var datoobra = _datoobrasManager.Find(id);

            if (datoobra == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var datoobraModel = new DatoObraViewModel()
            {
                primercontacto = datoobra.primercontacto,
                contactotelefonicoobra = datoobra.contactotelefonicoobra,
                correoelectronicoobra = datoobra.correoelectronicoobra,
                cargoobra = datoobra.cargoobra,
                nombreconstructora = datoobra.nombreconstructora,
                contactotelefonicoempresa = datoobra.contactotelefonicoempresa,
                correoelectronicoempresa = datoobra.correoelectronicoempresa,
                cargoempresa = datoobra.cargoempresa,
                direccionoficina = datoobra.direccionoficina,
                ubicacionobra = datoobra.ubicacionobra,
                tipoobra = datoobra.tipoobra,
                numeroequipos = datoobra.numeroequipos,
                tipoequipo = datoobra.tipoequipo,
                vendidasclimb = datoobra.vendidasclimb,
                vendidasotros = datoobra.vendidasotros,
                novendidas = datoobra.novendidas,
                paralizadas = datoobra.paralizadas
            };

            return View(datoobraModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARGESTIONOBRAS-MODIFICAR")]
        public ActionResult Editar(int id, DatoObraViewModel model)
        {
            var obra = _datoobrasManager.Find(id);

            if (obra == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {
                _datoobrasManager.Actualizar(
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
        public ActionResult Crear(DatoObraViewModel model, FormCollection collection)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {

                _datoobrasManager.Crear(
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
                _datoobrasManager.Eliminar(id);
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