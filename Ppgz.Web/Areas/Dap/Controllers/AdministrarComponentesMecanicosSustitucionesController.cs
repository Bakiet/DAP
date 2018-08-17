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
    public class AdministrarComponentesMecanicosSustitucionesController : Controller
    {
        private readonly ComponentesMecanicosSustitucionesManager _componentesmecanicos_sustitucionesManager = new ComponentesMecanicosSustitucionesManager();


        //asignar los roles para esta funcionalidad
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOSSUSTITUCIONES-LISTAR,NAZAN-ADMINISTRARCOMPONENTESMECANICOSSUSTITUCIONES-MODIFICAR")]

        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);


            ViewBag.ComponentesMecanicosSustituciones = db.componentesmecanicos_sustituciones.ToList();

           


            //ViewBag.EstatusCita = db.estatuscitas.ToList();

            return View();
        }

        public ActionResult ComponentesMecanicosSustituciones(int id)
        {

            ViewBag.ComponentesMecanicosSustituciones = _componentesmecanicos_sustitucionesManager.GetComponentesMecanicosSustituciones(id);
            return View("Index");
        }

        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-LISTAR,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-MODIFICAR")]
        //public ActionResult Index()
        //{
        //    ViewBag.mensajes = _mensajesInstitucionalesManager.FindAll();
        //    return View();
        //}
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOSSUSTITUCIONES-MODIFICAR")]
        public ActionResult Crear()
        {
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOSSUSTITUCIONES-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var componentemecanico_sustitucion = _componentesmecanicos_sustitucionesManager.Find(id);

            if (componentemecanico_sustitucion == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var componentemecanico_sustitucionModel = new ComponenteMecanicoSustitucionViewModel()
            {
                Tipo = componentemecanico_sustitucion.Tipo,
                Caracteristicas = componentemecanico_sustitucion.Caracteristicas,
                Descripcion = componentemecanico_sustitucion.Descripcion,
                Marca = componentemecanico_sustitucion.Marca,
                Modelo = componentemecanico_sustitucion.Modelo,
                Serial = componentemecanico_sustitucion.Serial,
                FechaFabricado = componentemecanico_sustitucion.FechaFabricado.ToString(),
                Duracion = componentemecanico_sustitucion.Duracion,
                Sustitucion = componentemecanico_sustitucion.Sustitucion,
                Fotografia = componentemecanico_sustitucion.Fotografia,

            };

            return View(componentemecanico_sustitucionModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOSSUSTITUCIONES-MODIFICAR")]
        public ActionResult Editar(int id, ComponenteMecanicoSustitucionViewModel model)
        {
            var componentemecanico_sustitucion = _componentesmecanicos_sustitucionesManager.Find(id);

            if (componentemecanico_sustitucion == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {
                _componentesmecanicos_sustitucionesManager.Actualizar(
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



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOSSUSTITUCIONES-MODIFICAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(ComponenteMecanicoSustitucionViewModel model, FormCollection collection)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {

                _componentesmecanicos_sustitucionesManager.Crear(
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