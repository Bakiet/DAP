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
    public class AdministrarSugerenciasController : Controller
    {
        private readonly SugerenciasManager _sugerenciasManager = new SugerenciasManager();

        private readonly VentasManager _ventasManager = new VentasManager();

        private readonly ObrasManager _obrasManager = new ObrasManager();

        private readonly ComponentesMecanicosManager _componentesmecanicosManager = new ComponentesMecanicosManager();
        private readonly ComponentesElectricosManager _componentesElectricos_Manager = new ComponentesElectricosManager();

        //asignar los roles para esta funcionalidad
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARSUGERENCIAS-CONSULTAR")]
        public ActionResult Index(int id)
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);

            var sugerencia = _sugerenciasManager.GetSugerencia(id);
            ViewBag.ComponentesMecanicosCount = _componentesmecanicosManager.GetSustituciones();
            ViewBag.ComponentesMecanicos = _componentesmecanicosManager.GetSustituciones();
            TempData["sustitucionesmecanicas"] = ViewBag.ComponentesMecanicosCount.Count;
            TempData.Keep();

            ViewBag.ComponentesElectricosCount = _componentesElectricos_Manager.GetSustituciones();
            ViewBag.ComponentesElectricos = _componentesElectricos_Manager.GetSustituciones();
            TempData["sustitucioneselectronicas"] = ViewBag.ComponentesElectricosCount.Count;
            TempData.Keep();

            TempData["venta_id"] = id;
            TempData.Keep();


            if (sugerencia != null)
            {
                var venta = _ventasManager.Find(sugerencia.IdVenta);

                ViewBag.obra = _obrasManager.Find(venta.IdObra);

                TempData["IDVENTA"] = venta.Id;

                ViewBag.venta = venta.Id;

                ViewBag.obra = _obrasManager.Find(venta.IdObra);

                TempData["IDOBRA"] = venta.IdObra;

                if (id != 0)
                {
                    ViewBag.Sugerencias = _sugerenciasManager.GetSugerencias(venta.Id);
                }
                else
                {
                    ViewBag.Sugerencias = db.mantenimientopreventivo.ToList();
                };
            }
            else
            {

                ViewBag.venta = null;
                ViewBag.obra = null;


                ViewBag.Sugerencias = null;

              
            }
            //ViewBag.Sugerencias = db.sugerencias.ToList();

            //ViewBag.EstatusCita = db.estatuscitas.ToList();

            return View();
        }

        public ActionResult Sugerencias(int id)
        {
            var sugerencia = _sugerenciasManager.GetSugerencia(id);

            var venta = _ventasManager.Find(sugerencia.IdVenta);

            ViewBag.obra = _obrasManager.Find(venta.IdObra);
            ViewBag.Sugerencias = _sugerenciasManager.GetSugerencias(id);
            return View("Index");
        }

        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-LISTAR,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-MODIFICAR")]
        //public ActionResult Index()
        //{
        //    ViewBag.mensajes = _mensajesInstitucionalesManager.FindAll();
        //    return View();
        //}
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARSUGERENCIAS-CREAR")]
        public ActionResult Crear()
        {
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARSUGERENCIAS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var sugerencia = _sugerenciasManager.Find(id);

            if (sugerencia == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var sugerenciaModel = new SugerenciaViewModel()
            {
                Descripcion = sugerencia.Descripcion,
                Caracteristica = sugerencia.Caracteristica,
                Fecha = sugerencia.Fecha.ToString(),
                Numero = sugerencia.Numero,
                AccionesTomadas = sugerencia.AccionesTomadas,
                AccionesRecomendadas = sugerencia.AccionesRecomendadas,
                GerenciaResponsable = sugerencia.GerenciaResponsable,
            };

            return View(sugerenciaModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARSUGERENCIAS-MODIFICAR")]
        public ActionResult Editar(int id, SugerenciaViewModel model)
        {
            var sugerencia = _sugerenciasManager.Find(id);

            if (sugerencia == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {
                _sugerenciasManager.Actualizar(Convert.ToInt32(TempData["IDVENTA"]),
                      id,
                      model.Descripcion,
                      model.Caracteristica,
                      model.Numero,
                      model.Fecha,
                      model.AccionesTomadas,
                      model.AccionesRecomendadas,
                      model.GerenciaResponsable);

                 TempData["FlashSuccess"] = MensajesResource.INFO_Sugerencias_ActualizadoCorrectamente;
              
                return RedirectToAction("Index", "AdministrarSugerencias", new { @id = TempData["IDVENTA"] });
                
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



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARSUGERENCIAS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(SugerenciaViewModel model, FormCollection collection)
        {
            

            try
            {

                _sugerenciasManager.Crear(Convert.ToInt32(TempData["venta_id"]),
                       model.Descripcion,
                      model.Caracteristica,
                      model.Numero,
                      model.Fecha,
                      model.AccionesTomadas,
                      model.AccionesRecomendadas,
                      model.GerenciaResponsable);


                TempData["FlashSuccess"] = MensajesResource.INFO_Sugerencias_CreadoCorrectamente;
                return RedirectToAction("Index", "AdministrarSugerencias", new { @id = TempData["venta_id"] });
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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARSUGERENCIAS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _sugerenciasManager.Eliminar(id);
                // TempData["FlashSuccess"] = MensajesResource.INFO_MensajesInstitucionales_EliminadoCorrectamente;
                return RedirectToAction("Index", "AdministrarSugerencias", new { @id = TempData["IDVENTA"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Index", "AdministrarSugerencias", new { @id = TempData["IDVENTA"] });

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