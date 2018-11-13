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
    public class AdministrarComponentesMecanicosTiposController : Controller
    {
        private readonly ComponentesMecanicosTiposManager _componentesmecanicostiposManager = new ComponentesMecanicosTiposManager();

        private readonly ComponentesMecanicosManager _componentesmecanicosManager = new ComponentesMecanicosManager();

        private readonly EquiposManager _equiposManager = new EquiposManager();

        private readonly ObrasManager _obrasManager = new ObrasManager();

        //asignar los roles para esta funcionalidad
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOS-CONSULTAR")]
        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);


            ViewBag.ComponentesMecanicosTipos = db.componentesmecanicos_tipos.ToList();



            return View();
        }

       

      
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOS-CREAR")]
        public ActionResult Crear()
        {
            
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var componentemecanicotipo = _componentesmecanicostiposManager.Find(id);


            if (componentemecanicotipo == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var componentemecanicotipoModel = new ComponenteMecanicoTipoViewModel()
            {
                Descripcion = componentemecanicotipo.descripcion

            };

            return View(componentemecanicotipoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOS-MODIFICAR")]
        public ActionResult Editar(int id, ComponenteMecanicoTipoViewModel model)
        {
            var componentemecanicotipo = _componentesmecanicostiposManager.Find(id);
           
            if (componentemecanicotipo == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {
               
                _componentesmecanicostiposManager.Actualizar(
                      id,
                      model.Descripcion);

                // TempData["FlashSuccess"] = MensajesResource.INFO_UsuarioNazan_ActualizadoCorrectamente;
                // return RedirectToAction("Editar", "AdministrarComponentesMecanicosTipos", new { @id = id });
                return RedirectToAction("Index", "AdministrarComponentesMecanicos");
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



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(ComponenteMecanicoTipoViewModel model, FormCollection collection)
        {
            
            var equipo = _equiposManager.Find(Convert.ToInt32(TempData["equipo_id"]));
            if(equipo == null)
            {
                equipo = _equiposManager.Find(Convert.ToInt32(TempData["equipoid"]));
            }
            
            TempData.Keep();
            ViewBag.Equipo = equipo;
            if(equipo != null) { 
            ViewBag.obra = _obrasManager.Find(equipo.obra_id);
                TempData["obra_id"] = equipo.obra_id;
                TempData.Keep();
                ViewBag.ComponentesMecanicos = _componentesmecanicosManager.GetComponentesMecanicos(equipo.Id);
            }
           

            if (!ModelState.IsValid) return View(model);

            try
            {
             
                _componentesmecanicostiposManager.Crear(
                      model.Descripcion);
                if(TempData["componente"] != null) { 
                //TempData["FlashSuccess"] = MensajesResource.INFO_MensajesInstitucionales_CreadoCorrectamente;
                return RedirectToAction("Editar","AdministrarComponentesMecanicos", new { @id = TempData["componente"] });
                }
                else
                {
                    if(TempData["fallaid"] != null) { 
                    return RedirectToAction("Editar", "AdministrarFallas", new { @id = TempData["fallaid"] });
                    }else
                    {
                        return RedirectToAction("CrearPorDefecto", "AdministrarFallas");
                    }
                }
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