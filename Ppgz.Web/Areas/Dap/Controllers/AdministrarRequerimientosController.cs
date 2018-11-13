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
    public class AdministrarRequerimientosController : Controller
    {
        private readonly RequerimientosManager _requerimientosManager = new RequerimientosManager();
        private readonly ObrasManager _obrasManager = new ObrasManager();
        private readonly ComponentesMecanicosManager _componentesmecanicosManager = new ComponentesMecanicosManager();
        private readonly ComponentesElectricosManager _componentesElectricos_Manager = new ComponentesElectricosManager();

        //asignar los roles para esta funcionalidad
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARREQUERIMIENTOS-CONSULTAR")]
        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);

            ViewBag.ComponentesMecanicosCount = _componentesmecanicosManager.GetSustituciones();
            ViewBag.ComponentesMecanicos = _componentesmecanicosManager.GetSustituciones();
            TempData["sustitucionesmecanicas"] = ViewBag.ComponentesMecanicosCount.Count;
            TempData.Keep();

            ViewBag.ComponentesElectricosCount = _componentesElectricos_Manager.GetSustituciones();
            ViewBag.ComponentesElectricos = _componentesElectricos_Manager.GetSustituciones();
            TempData["sustitucioneselectronicas"] = ViewBag.ComponentesElectricosCount.Count;
            TempData.Keep();
            ViewBag.Requerimientos = _requerimientosManager.GetRequerimientos();

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
        public ActionResult Historico(int id)
        {

            ViewBag.Requerimientos = _requerimientosManager.GetHistorico(id);
            ViewBag.id = id;
            return View("Index");
        }
        public ActionResult HistoricoTodos()
        {
            ViewBag.Requerimientos = _requerimientosManager.GetHistoricoAll();
            return View("Index");
        }
        public ActionResult Requerimientos(int id)
        {
            TempData["requerimiento_id"] = id;
            TempData["requerimientos"] = id;
            TempData.Keep();
            
            ViewBag.Requerimientos = _requerimientosManager.GetRequerimientos(id);


            TempData["obra"] = id;
                TempData.Keep();
            ViewBag.id = id;
            return View("Index");
        }

        public ActionResult ReporteRequerimientos(int id)
        {
            TempData["requerimiento_id"] = id;
            TempData["requerimientos"] = id;
            TempData.Keep();

            ViewBag.Requerimientos = _requerimientosManager.GetRequerimientos(id);


            TempData["obra"] = id;
            TempData.Keep();
            ViewBag.id = id;
            return PartialView();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARREQUERIMIENTOS-CREAR")]
        public ActionResult Crear(int id)
        {
            var db = new EntitiesDap();
           
            var obra = _obrasManager.Find(id);
                TempData["obra"] = obra.Id;
                TempData.Keep();
                ViewBag.Obras =
             new SelectList(_obrasManager.FindObras(obra.Id), "nombre", "nombre", obra.Nombre);
           
            
            /*var requerimientoModel = new RequerimientoViewModel()
            {
                Obra = requerimientoModel.Obra,
                Tipo = requerimiento.Tipo,
                FechaSolicitud = requerimiento.FechaSolicitud.ToString(),
                GerenciaResponsable = requerimiento.GerenciaResponsable,
                Tecnicos = requerimiento.Tecnicos,
                StatusRequerimientos = requerimiento.StatusRequerimiento,
                FechaCumplimientoSolicitud = requerimiento.FechaCumplimientoSolicitud.ToString(),
                Descripcion = requerimiento.Descripcion,
                AccionesEjecutadas = requerimiento.AccionesEjecutadas,
                obra_id = requerimiento.obra_id,
                Id = requerimiento.Id
            };*/
            /*
            ViewBag.Obras =
             new SelectList(_requerimientosManager.FindObras(), "id", "nombre", obra.Nombre);
            */


           
            
            /*
            ViewBag.Tipo =
             new SelectList(_requerimientosManager.FindTipos(), "id", "descripcion");
            */
            ViewBag.GerenciaResponsable = new SelectList(db.gerenciaresponsable.ToList(), "descripcion", "descripcion");

            ViewBag.StatusRequerimiento =
               new SelectList(_requerimientosManager.FindStatus(), "descripcion", "descripcion");
            
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARREQUERIMIENTOS-CREAR")]
        public ActionResult CrearPorDefecto()
        {
            var db = new EntitiesDap();
           
                ViewBag.Obras =
             new SelectList(_obrasManager.FindAll(), "nombre", "nombre");

            ViewBag.GerenciaResponsable = new SelectList(db.gerenciaresponsable.ToList(), "descripcion", "descripcion");

            ViewBag.StatusRequerimiento =
               new SelectList(_requerimientosManager.FindStatus(), "descripcion", "descripcion");

            return View();
        }
        /*[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARREQUERIMIENTOS-MODIFICAR")]
        public ActionResult Crear()
        {
            return View();
        }*/
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARREQUERIMIENTOS-CONSULTAR")]
        public ActionResult Reporte(int id)
        {
            var db = new EntitiesDap();

            var requerimiento = _requerimientosManager.Find(id);

            TempData["requerimientos"] = id;
            TempData.Keep();
            ViewBag.GerenciaResponsable2 =
               new SelectList(_requerimientosManager.FindGerencias(), "descripcion", "descripcion", requerimiento.GerenciaResponsable);


            ViewBag.Obras =
             new SelectList(_requerimientosManager.FindObras(), "nombre", "nombre", requerimiento.Obra);
            /*
            ViewBag.Tipo =
             new SelectList(_requerimientosManager.FindTipos(), "descripcion", "descripcion", requerimiento.Obra);
            */

            //  ViewBag.GerenciaResponsable = new SelectList(db.gerenciaresponsable.ToList(), "descripcion", "descripcion", requerimiento.GerenciaResponsable);

            ViewBag.StatusRequerimiento2 =
               new SelectList(_requerimientosManager.FindStatus(), "descripcion", "descripcion", requerimiento.StatusRequerimiento);

            ViewBag.ArchivosCorreo = _obrasManager.FindCorreosRequerimientos(id);

            if (requerimiento == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            ViewBag.ComponentesMecanicosCount = _componentesmecanicosManager.GetSustituciones();
            ViewBag.ComponentesMecanicos = _componentesmecanicosManager.GetSustituciones();
            TempData["sustitucionesmecanicas"] = ViewBag.ComponentesMecanicosCount.Count;
            TempData.Keep();

            ViewBag.ComponentesElectricosCount = _componentesElectricos_Manager.GetSustituciones();
            ViewBag.ComponentesElectricos = _componentesElectricos_Manager.GetSustituciones();
            TempData["sustitucioneselectronicas"] = ViewBag.ComponentesElectricosCount.Count;
            TempData.Keep();
            ViewBag.Requerimientos = _requerimientosManager.GetRequerimientos();

            var requerimientoModel = new RequerimientoViewModel()
            {
                Obra = requerimiento.Obra,
                Tipo = requerimiento.Tipo,
                FechaSolicitud = requerimiento.FechaSolicitud.ToString(),
                //GerenciaResponsable = requerimiento.GerenciaResponsable,
                Tecnicos = requerimiento.Tecnicos,
                StatusRequerimientos = requerimiento.StatusRequerimiento,
                FechaCumplimientoSolicitud = requerimiento.FechaCumplimientoSolicitud.ToString(),
                Descripcion = requerimiento.Descripcion,
                AccionesEjecutadas = requerimiento.AccionesEjecutadas,
                obra_id = requerimiento.obra_id,
                Id = requerimiento.Id
            };

            return PartialView(requerimientoModel);
        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARREQUERIMIENTOS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var db = new EntitiesDap();

            var requerimiento = _requerimientosManager.Find(id);

            TempData["requerimientos"] = id;
            TempData.Keep();
            TempData["obra_id"] = requerimiento.obra_id;
            TempData.Keep();
            ViewBag.GerenciaResponsable2 =
               new SelectList(_requerimientosManager.FindGerencias(), "descripcion", "descripcion", requerimiento.GerenciaResponsable);


            ViewBag.Obras =
             new SelectList(_requerimientosManager.FindObras(), "nombre", "nombre", requerimiento.Obra);
            /*
            ViewBag.Tipo =
             new SelectList(_requerimientosManager.FindTipos(), "descripcion", "descripcion", requerimiento.Obra);
            */

          //  ViewBag.GerenciaResponsable = new SelectList(db.gerenciaresponsable.ToList(), "descripcion", "descripcion", requerimiento.GerenciaResponsable);
           
            ViewBag.StatusRequerimiento2 =
               new SelectList(_requerimientosManager.FindStatus(), "descripcion", "descripcion", requerimiento.StatusRequerimiento);

            ViewBag.ArchivosCorreo = _obrasManager.FindCorreosRequerimientos(id);

            if (requerimiento == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var requerimientoModel = new RequerimientoViewModel()
            {
                Obra = requerimiento.Obra,
                Tipo = requerimiento.Tipo,
                FechaSolicitud = requerimiento.FechaSolicitud.ToString(),
                //GerenciaResponsable = requerimiento.GerenciaResponsable,
                Tecnicos = requerimiento.Tecnicos,
                StatusRequerimientos = requerimiento.StatusRequerimiento,
                FechaCumplimientoSolicitud = requerimiento.FechaCumplimientoSolicitud.ToString(),
                Descripcion = requerimiento.Descripcion,
                AccionesEjecutadas = requerimiento.AccionesEjecutadas,
                obra_id = requerimiento.obra_id,
                Id = requerimiento.Id
            };

            return View(requerimientoModel);
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARREQUERIMIENTOS-CONSULTAR")]
        public ActionResult Detalle(int id)
        {
            var db = new EntitiesDap();

            var requerimiento = _requerimientosManager.Find(id);

            ViewBag.Obras =
             new SelectList(_requerimientosManager.FindObras(), "nombre", "nombre", requerimiento.Obra);
            /*
            ViewBag.Tipo =
             new SelectList(_requerimientosManager.FindTipos(), "descripcion", "descripcion", requerimiento.Obra);
            */

            ViewBag.GerenciaResponsableDetalle = new SelectList(db.gerenciaresponsable.ToList(), "descripcion", "descripcion", requerimiento.GerenciaResponsable);

            ViewBag.StatusRequerimientoDetalle =
               new SelectList(_requerimientosManager.FindStatus(), "descripcion", "descripcion", requerimiento.StatusRequerimiento);

            ViewBag.ArchivosCorreo = _obrasManager.FindCorreosRequerimientos(id);

            if (requerimiento == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var requerimientoModel = new RequerimientoViewModel()
            {
                Obra = requerimiento.Obra,
                Tipo = requerimiento.Tipo,
                FechaSolicitud = requerimiento.FechaSolicitud.ToString(),
                GerenciaResponsable = requerimiento.GerenciaResponsable,
                Tecnicos = requerimiento.Tecnicos,
                StatusRequerimientos = requerimiento.StatusRequerimiento,
                FechaCumplimientoSolicitud = requerimiento.FechaCumplimientoSolicitud.ToString(),
                Descripcion = requerimiento.Descripcion,
                AccionesEjecutadas = requerimiento.AccionesEjecutadas,
                obra_id = requerimiento.obra_id,
                Id = requerimiento.Id
            };

            return View(requerimientoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARREQUERIMIENTOS-MODIFICAR")]
        public ActionResult Editar(int id, RequerimientoViewModel model)
        {
            var db = new EntitiesDap();
            var Url = "";
            var requerimiento = _requerimientosManager.Find(id);
           
            var obra = _obrasManager.Find_by_name(model.Obra);

            ViewBag.Obras =
             new SelectList(_requerimientosManager.FindObras(), "nombre", "nombre", requerimiento.Obra);

            ViewBag.GerenciaResponsable = new SelectList(db.gerenciaresponsable.ToList(), "descripcion", "descripcion", requerimiento.GerenciaResponsable);

            ViewBag.StatusRequerimiento2 =
               new SelectList(_requerimientosManager.FindStatus(), "descripcion", "descripcion", requerimiento.StatusRequerimiento);

            ViewBag.ArchivosCorreo = _obrasManager.FindCorreosRequerimientos(id);

            if (requerimiento == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {
                DateTime? fechacumplimientoSolicitud = null;
                if (model.FechaCumplimientoSolicitud != null)
                {
                    _requerimientosManager.Actualizar(DateTime.Parse(model.FechaSolicitud),
                        DateTime.Parse(model.FechaCumplimientoSolicitud),
                    Convert.ToInt32(TempData["obra_id"]),
                      id,
                      requerimiento.Obra,
                      model.Tipo,

                      model.GerenciaResponsable,
                      model.Tecnicos,
                      model.StatusRequerimientos,

                      model.Descripcion,
                      model.AccionesEjecutadas
                      );
                }else
                {
                    _requerimientosManager.Actualizar(DateTime.Parse(model.FechaSolicitud),
                       fechacumplimientoSolicitud,
                    Convert.ToInt32(TempData["obra_id"]),
                      id,
                      requerimiento.Obra,
                      model.Tipo,

                      model.GerenciaResponsable,
                      model.Tecnicos,
                      model.StatusRequerimientos,

                      model.Descripcion,
                      model.AccionesEjecutadas
                      );
                }
                    

                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf-Fotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(id, Url, "requerimientos", "correo");
                    }


                }

                TempData["FlashSuccess"] = MensajesResource.INFO_Requerimientos_ActualizadoCorrectamente;
                //  return RedirectToAction("Editar", "AdministrarRequerimientos", id);
                if (obra != null)
                {
                    return RedirectToAction("Fallas", "AdministrarFallas", new { @id = TempData["obra_id"] });
                }
                else
                {
                    return RedirectToAction("Index", "AdministrarRequerimientos");
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

                return View(model);
            }

        }



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARREQUERIMIENTOS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(RequerimientoViewModel model, FormCollection collection)
        {
            var Url = "";
            var db = new EntitiesDap();
            var obra = _obrasManager.Find(Convert.ToInt32(TempData["obra"]));

            var obranombre = _obrasManager.FindObrasPorNombre(model.Obra);

            ViewBag.Obras =
             new SelectList(_requerimientosManager.FindObras(), "nombre", "nombre");
            
           ViewBag.StatusRequerimiento =
              new SelectList(_requerimientosManager.FindStatus(), "descripcion", "descripcion");


            ViewBag.GerenciaResponsable = new SelectList(db.gerenciaresponsable.ToList(), "descripcion", "descripcion");
            // if (!ModelState.IsValid) return View(model);

            requerimientos re = new requerimientos();
            DateTime? fechacumplimientoSolicitud = null;
            if (model.FechaCumplimientoSolicitud != null)
            {
                if (TempData["obra"] != null)
                {
                    re = _requerimientosManager.Crear(DateTime.Parse(model.FechaSolicitud),
                        DateTime.Parse(model.FechaCumplimientoSolicitud),
                        Convert.ToInt32(TempData["obra"]),
                         obra.Nombre,
                         model.Tipo,


                         model.GerenciaResponsable,
                         model.Tecnicos,
                         model.StatusRequerimientos,


                         model.Descripcion,
                         model.AccionesEjecutadas
                   );
                }
                else
                {
                    re = _requerimientosManager.Crear(DateTime.Parse(model.FechaSolicitud),
                        DateTime.Parse(model.FechaCumplimientoSolicitud),
                        obranombre.Id,
                         model.Obra,
                         model.Tipo,

                         model.GerenciaResponsable,
                         model.Tecnicos,
                         model.StatusRequerimientos,

                         model.Descripcion,
                         model.AccionesEjecutadas
                   );
                }
            }else
            {
                if (TempData["obra"] != null)
                {
                    re = _requerimientosManager.Crear(DateTime.Parse(model.FechaSolicitud),
                        fechacumplimientoSolicitud,
                        Convert.ToInt32(TempData["obra"]),
                         obra.Nombre,
                         model.Tipo,


                         model.GerenciaResponsable,
                         model.Tecnicos,
                         model.StatusRequerimientos,


                         model.Descripcion,
                         model.AccionesEjecutadas
                   );
                }
                else
                {
                    re = _requerimientosManager.Crear(DateTime.Parse(model.FechaSolicitud),
                       fechacumplimientoSolicitud,
                        obranombre.Id,
                         model.Obra,
                         model.Tipo,

                         model.GerenciaResponsable,
                         model.Tecnicos,
                         model.StatusRequerimientos,

                         model.Descripcion,
                         model.AccionesEjecutadas
                   );
                }
            }
                try
            {
                
                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Correo" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(re.Id, Url, "requerimientos", "correo");
                    }


                }

                TempData["FlashSuccess"] = MensajesResource.INFO_Requerimientos_CreadoCorrectamente;
                if(TempData["obra"] != null) { 
                    return RedirectToAction("Requerimientos", "AdministrarRequerimientos", new { @id = TempData["obra"] });
                }else
                {
                    return RedirectToAction("Index", "AdministrarRequerimientos");
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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARREQUERIMIENTOS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _requerimientosManager.Eliminar(id);
                TempData["FlashSuccess"] = MensajesResource.INFO_Requerimientos_EliminadoCorrectamente;
                if (TempData["obra"] != null)
                {
                    return RedirectToAction("Requerimientos", "AdministrarRequerimientos", new { @id = TempData["obra"] });
                }
                else
                {
                    return RedirectToAction("Index", "AdministrarRequerimientos");
                }
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                if (TempData["obra"] != null)
                {
                    return RedirectToAction("Requerimientos", "AdministrarRequerimientos", new { @id = TempData["obra"] });
                }
                else
                {
                    return RedirectToAction("Index", "AdministrarRequerimientos");
                }

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
                if (TempData["obra"] != null)
                {
                    return RedirectToAction("Requerimientos", "AdministrarRequerimientos", new { @id = TempData["obra"] });
                }
                else
                {
                    return RedirectToAction("Index", "AdministrarRequerimientos");
                }
            }

        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARREQUERIMIENTOS-MODIFICAR")]
        public ActionResult EliminarArchivo(string archivo, string tipo, string caracteristica, int id)
        {
            try
            {
                _obrasManager.EliminarArchivo(archivo, tipo, caracteristica);
                TempData["FlashSuccess"] = MensajesResource.INFO_Obras_ActualizadoCorrectamente;
                return RedirectToAction("Editar", "AdministrarRequerimientos", new { @id = TempData["requerimientos"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Editar", "AdministrarRequerimientos", new { @id = TempData["requerimientos"] });

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
                return RedirectToAction("Editar", "AdministrarRequerimientos", new { @id = TempData["requerimientos"] });
            }

        }
    }
}