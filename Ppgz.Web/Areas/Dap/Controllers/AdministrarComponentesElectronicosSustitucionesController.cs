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
    public class AdministrarComponentesElectronicosSustitucionesController : Controller
    {
        private readonly ComponentesElectricosSustitucionesManager _componentesElectricos_sustitucionesManager = new ComponentesElectricosSustitucionesManager();


        //asignar los roles para esta funcionalidad
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRICOSSUSTITUCIONES-LISTAR,NAZAN-ADMINISTRARCOMPONENTESELECTRICOSSUSTITUCIONES-MODIFICAR")]

        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);


            ViewBag.ComponentesElectricosSustituciones = db.componenteselectricos_sustituciones.ToList();

           


            //ViewBag.EstatusCita = db.estatuscitas.ToList();

            return View();
        }

        public ActionResult ComponentesElectricosSustituciones(int id)
        {

            ViewBag.ComponentesElectricosSustituciones = _componentesElectricos_sustitucionesManager.GetComponentesElectricosSustituciones(id);
            return View("Index");
        }

        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-LISTAR,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-MODIFICAR")]
        //public ActionResult Index()
        //{
        //    ViewBag.mensajes = _mensajesInstitucionalesManager.FindAll();
        //    return View();
        //}
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRICOSSUSTITUCIONES-MODIFICAR")]
        public ActionResult Crear()
        {
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRICOSSUSTITUCIONES-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var componenteelectrico_sustitucion = _componentesElectricos_sustitucionesManager.Find(id);

            if (componenteelectrico_sustitucion == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var componentemecanico_sustitucionModel = new ComponenteElectricoSustitucionViewModel()
            {
                Tipo = componenteelectrico_sustitucion.Tipo,
                Caracteristicas = componenteelectrico_sustitucion.Caracteristicas,
                Descripcion = componenteelectrico_sustitucion.Descripcion,
                Marca = componenteelectrico_sustitucion.Marca,
                Modelo = componenteelectrico_sustitucion.Modelo,
                Serial = componenteelectrico_sustitucion.Serial,
                FechaFabricado = componenteelectrico_sustitucion.FechaFabricado.ToString(),
                Duracion = componenteelectrico_sustitucion.Duracion,
                Sustitucion = componenteelectrico_sustitucion.Sustitucion,
                Fotografia = componenteelectrico_sustitucion.Fotografia,

            };

            return View(componentemecanico_sustitucionModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRICOSSUSTITUCIONES-MODIFICAR")]
        public ActionResult Editar(int id, ComponenteElectricoSustitucionViewModel model)
        {
            var componenteelectrico_sustitucion = _componentesElectricos_sustitucionesManager.Find(id);

            if (componenteelectrico_sustitucion == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {
                _componentesElectricos_sustitucionesManager.Actualizar(
                      id,
                      model.Tipo,
                      model.Caracteristicas,
                      model.Descripcion,
                      model.Marca,
                      model.Modelo,
                      model.Serial,
                      model.FechaFabricado,
                      model.Duracion,
                      model.Sustitucion,
                      model.Fotografia);

                // TempData["FlashSuccess"] = MensajesResource.INFO_UsuarioNazan_ActualizadoCorrectamente;
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



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRICOSSUSTITUCIONES-MODIFICAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(ComponenteElectricoSustitucionViewModel model, FormCollection collection)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {

                _componentesElectricos_sustitucionesManager.Crear(
                     model.Tipo,
                      model.Caracteristicas,
                      model.Descripcion,
                      model.Marca,
                      model.Modelo,
                      model.Serial,
                      model.FechaFabricado,
                      model.Duracion,
                      model.Sustitucion,
                      model.Fotografia);


                //TempData["FlashSuccess"] = MensajesResource.INFO_MensajesInstitucionales_CreadoCorrectamente;
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

    }
}