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
    public class AdministrarComponentesElectronicosTiposController : Controller
    {
        private readonly ComponentesElectricosTiposManager _componenteselectricostiposManager = new ComponentesElectricosTiposManager();

        private readonly ComponentesMecanicosManager _componenteselectronicosManager = new ComponentesMecanicosManager();

        private readonly EquiposManager _equiposManager = new EquiposManager();

        private readonly ObrasManager _obrasManager = new ObrasManager();
        //asignar los roles para esta funcionalidad
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRONICOS-CONSULTAR")]

        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);


            ViewBag.ComponentesElectronicosTipos = db.componenteselectronicos_tipos.ToList();



            return View();
        }

       

      
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRONICOS-CREAR")]
        public ActionResult Crear()
        {
           
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRONICOS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var componenteElectronicotipo = _componenteselectricostiposManager.Find(id);


            if (componenteElectronicotipo == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var componenteElectronicotipoModel = new ComponenteElectricoTipoViewModel()
            {
                Descripcion = componenteElectronicotipo.descripcion

            };

            return View(componenteElectronicotipoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRONICOS-MODIFICAR")]
        public ActionResult Editar(int id, ComponenteElectricoTipoViewModel model)
        {
            var componenteElectronicotipo = _componenteselectricostiposManager.Find(id);
           
            if (componenteElectronicotipo == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index", "AdministrarComponentesElectronicos");
            }

            try
            {
               
                _componenteselectricostiposManager.Actualizar(
                      id,
                      model.Descripcion);

                // TempData["FlashSuccess"] = MensajesResource.INFO_UsuarioNazan_ActualizadoCorrectamente;
                return RedirectToAction("Index", "AdministrarComponentesElectronicos");
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



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRONICOS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(ComponenteElectricoTipoViewModel model, FormCollection collection)
        {
            var equipo = _equiposManager.Find(Convert.ToInt32(TempData["equipo_id"]));


            TempData.Keep();
            ViewBag.Equipo = equipo;

            ViewBag.obra = _obrasManager.Find(equipo.obra_id);

            TempData["obra_id"] = equipo.obra_id;
            TempData.Keep();
            ViewBag.ComponentesMecanicos = _componenteselectronicosManager.GetComponentesMecanicos(Convert.ToInt32(TempData["equipo_id"]));

            if (!ModelState.IsValid) return View(model);

            try
            {
             
                _componenteselectricostiposManager.Crear(
                      model.Descripcion);

                //TempData["FlashSuccess"] = MensajesResource.INFO_MensajesInstitucionales_CreadoCorrectamente;
                return RedirectToAction("Editar", "AdministrarComponentesElectronicos", new { @id = TempData["componente"] });
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