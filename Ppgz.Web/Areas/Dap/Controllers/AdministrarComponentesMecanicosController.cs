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
    public class AdministrarComponentesMecanicosController : Controller
    {
        private readonly ComponentesMecanicosManager _componentesmecanicosManager = new ComponentesMecanicosManager();

        private readonly EquiposManager _equiposManager = new EquiposManager();

        private readonly ObrasManager _obrasManager = new ObrasManager();

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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOS-CONSULTAR")]
        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);

            ViewBag.ComponentesMecanicos = db.componentesmecanicos.ToList();

            ViewBag.obra = _obrasManager.Find(Convert.ToInt32(TempData["obra_id"]));
            //ViewBag.EstatusCita = db.estatuscitas.ToList();

            return View();
        }
        public ActionResult Historico(int id)
        {
            var equipo = _equiposManager.Find(id);

            ViewBag.Equipo = equipo;

            ViewBag.obra = _obrasManager.Find(equipo.obra_id);

            TempData["obra_id"] = equipo.obra_id;
            TempData.Keep();
            ViewBag.ComponentesMecanicos = _componentesmecanicosManager.GetHistorico(id);
            ViewBag.id = id;
            return View("Index");
        }
        public ActionResult ComponentesMecanicos(int id)
        {
            var equipo = _equiposManager.Find(id);

            TempData["equipo_id"] = id;
            TempData.Keep();
            ViewBag.Equipo = equipo;

            ViewBag.obra = _obrasManager.Find(equipo.obra_id);

            TempData["obra_id"] = equipo.obra_id;
            TempData.Keep();
            ViewBag.ComponentesMecanicos = _componentesmecanicosManager.GetComponentesMecanicos(id);

            ViewBag.id = id;

            return View("Index");
        }

        public ActionResult ReporteComponentesMecanicos(int id)
        {
            var equipo = _equiposManager.Find(id);

            TempData["equipo_id"] = id;
            TempData.Keep();
            ViewBag.Equipo = equipo;

            ViewBag.obra = _obrasManager.Find(equipo.obra_id);

            TempData["obra_id"] = equipo.obra_id;
            TempData.Keep();
            ViewBag.ComponentesMecanicos = _componentesmecanicosManager.GetComponentesMecanicos(id);

            ViewBag.id = id;

            return PartialView("Index");
        }

        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-LISTAR,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-MODIFICAR")]
        //public ActionResult Index()
        //{
        //    ViewBag.mensajes = _mensajesInstitucionalesManager.FindAll();
        //    return View();
        //}
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOS-CREAR")]
        public ActionResult Crear()
        {
            ViewBag.ComponentesMecanicos_Tipos =
                 new SelectList(_componentesmecanicosManager.FindTipos(), "descripcion", "descripcion");

            ViewBag.Sustitucion =
               new SelectList(new[] { new { ID = "SI", Name = "SI" }, new { ID = "NO", Name = "NO" }, }, "ID", "Name", 1);

            TempData["equipo"] = TempData["equipo_id"];
            TempData["obra"] = TempData["obra_id"];
            TempData.Keep();
            ViewBag.obra = _obrasManager.Find(Convert.ToInt32(TempData["obra_id"]));


            return View();
        }

      

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            TempData["componente"] = id;
            TempData.Keep();
            var componentemecanico = _componentesmecanicosManager.Find(id);

            ViewBag.ComponentesMecanicos_Tipos =
                  new SelectList(_componentesmecanicosManager.FindTipos(), "descripcion", "descripcion");

            ViewBag.ArchivosFotografia = _obrasManager.FindFotografiasComponentesMecanicos(id);

            ViewBag.Sustitucion2 =
               new SelectList(new[] { new { ID = "SI", Name = "SI" }, new { ID = "NO", Name = "NO" }, }, "Name", "Name", componentemecanico.Sustitucion);

            TempData["obra"] = TempData["obra_id"];
            TempData.Keep();
            ViewBag.obra = _obrasManager.Find(Convert.ToInt32(TempData["obra_id"]));

            
            

            if (componentemecanico == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var componentemecanicoModel = new ComponenteMecanicoViewModel()
            {
                Tipo = componentemecanico.Tipo,
                Caracteristicas = componentemecanico.Caracteristicas,
                Descripcion = componentemecanico.Descripcion,
                Marca = componentemecanico.Marca,
                Modelo = componentemecanico.Modelo,
                Serial = componentemecanico.Serial,
                FechaFabricado = componentemecanico.FechaFabricado,
                Duracion = componentemecanico.Duracion,
                Sustitucion = componentemecanico.Sustitucion,
                Fotografia = componentemecanico.Fotografia,

            };

            return View(componentemecanicoModel);
        }

        public ActionResult Reporte(int id)
        {
            TempData["componente"] = id;
            TempData.Keep();
            var componentemecanico = _componentesmecanicosManager.Find(id);

            ViewBag.ComponentesMecanicos_Tipos =
                  new SelectList(_componentesmecanicosManager.FindTipos(), "descripcion", "descripcion");

            ViewBag.ArchivosFotografia = _obrasManager.FindFotografiasComponentesMecanicos(id);

            ViewBag.Sustitucion2 =
               new SelectList(new[] { new { ID = "SI", Name = "SI" }, new { ID = "NO", Name = "NO" }, }, "Name", "Name", componentemecanico.Sustitucion);

            TempData["obra"] = TempData["obra_id"];
            TempData.Keep();
            ViewBag.obra = _obrasManager.Find(Convert.ToInt32(TempData["obra_id"]));




            if (componentemecanico == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var componentemecanicoModel = new ComponenteMecanicoViewModel()
            {
                Tipo = componentemecanico.Tipo,
                Caracteristicas = componentemecanico.Caracteristicas,
                Descripcion = componentemecanico.Descripcion,
                Marca = componentemecanico.Marca,
                Modelo = componentemecanico.Modelo,
                Serial = componentemecanico.Serial,
                FechaFabricado = componentemecanico.FechaFabricado,
                Duracion = componentemecanico.Duracion,
                Sustitucion = componentemecanico.Sustitucion,
                Fotografia = componentemecanico.Fotografia,

            };

            return PartialView(componentemecanicoModel);
        }


        public ActionResult Detalle(int id)
        {
            var componentemecanico = _componentesmecanicosManager.Find(id);

            ViewBag.ComponentesMecanicos_Tipos =
                  new SelectList(_componentesmecanicosManager.FindTipos(), "descripcion", "descripcion");

            ViewBag.ArchivosFotografia = _obrasManager.FindFotografiasComponentesMecanicos(id);

            ViewBag.Sustitucion2 =
               new SelectList(new[] { new { ID = "SI", Name = "SI" }, new { ID = "NO", Name = "NO" }, }, "ID", "Name", componentemecanico.Sustitucion);

            TempData["obra"] = TempData["obra_id"];
            TempData.Keep();
            ViewBag.obra = _obrasManager.Find(Convert.ToInt32(TempData["obra_id"]));




            if (componentemecanico == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var componentemecanicoModel = new ComponenteMecanicoViewModel()
            {
                Tipo = componentemecanico.Tipo,
                Caracteristicas = componentemecanico.Caracteristicas,
                Descripcion = componentemecanico.Descripcion,
                Marca = componentemecanico.Marca,
                Modelo = componentemecanico.Modelo,
                Serial = componentemecanico.Serial,
                FechaFabricado = componentemecanico.FechaFabricado,
                Duracion = componentemecanico.Duracion,
                Sustitucion = componentemecanico.Sustitucion,
                Fotografia = componentemecanico.Fotografia,

            };

            return View(componentemecanicoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOS-MODIFICAR")]
        public ActionResult Editar(int id, ComponenteMecanicoViewModel model)
        {
            var Url = "";
            var pdfUrl = "";

            var componentemecanico = _componentesmecanicosManager.Find(id);

            ViewBag.obra = _obrasManager.Find(Convert.ToInt32(TempData["obra_id"]));
            TempData.Keep();

            ViewBag.Sustitucion2 =
              new SelectList(new[] { new { ID = "SI", Name = "SI" }, new { ID = "NO", Name = "NO" }, }, "ID", "Name", componentemecanico.Sustitucion);

            if (componentemecanico == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {
                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(componentemecanico.Id, Url, "componentesmecanicos", "fotografia");
                    }

                }
                /*HttpPostedFileBase pdf = Request.Files["Pdf"];

                if (pdf != null && pdf.ContentLength > 0)
                {
                    pdfUrl = CargarPdf(pdf);
                }
                else { pdfUrl = componentemecanico.Fotografia; }
                */
                _componentesmecanicosManager.Actualizar(
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
                      pdfUrl.Trim(), model.FechaSustitucion);

                // TempData["FlashSuccess"] = MensajesResource.INFO_UsuarioNazan_ActualizadoCorrectamente;
                return RedirectToAction("ComponentesMecanicos", "AdministrarComponentesMecanicos", new { @id = TempData["equipo_id"] });
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
        public ActionResult Crear(ComponenteMecanicoViewModel model, FormCollection collection)
        {
            var Url = "";
            var pdfUrl = "";

            ViewBag.obra = _obrasManager.Find(Convert.ToInt32(TempData["obra"]));
            TempData.Keep();
            if (!ModelState.IsValid) return View(model);

            try
            {
               /* HttpPostedFileBase pdf = Request.Files["Pdf"];

                if (pdf != null && pdf.ContentLength > 0)
                {
                    pdfUrl = CargarPdf(pdf);
                }
                else { pdfUrl = ""; }
                */
                componentesmecanicos cm = _componentesmecanicosManager.Crear(Convert.ToInt32(TempData["equipo"]),
                     model.Tipo,
                      model.Caracteristicas,
                      model.Descripcion,
                      model.Marca,
                      model.Modelo,
                      model.Serial,
                      model.FechaFabricado,
                      model.Duracion,
                      model.Sustitucion,
                      pdfUrl.Trim(), model.FechaSustitucion);

                TempData.Keep();
                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(cm.Id, Url, "componentesmecanicos", "fotografia");
                    }
                   

                }

                //TempData["FlashSuccess"] = MensajesResource.INFO_MensajesInstitucionales_CreadoCorrectamente;
                return RedirectToAction("ComponentesMecanicos", "AdministrarComponentesMecanicos", new { @id = TempData["equipo"] });
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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _componentesmecanicosManager.Eliminar(id);
                //TempData["FlashSuccess"] = MensajesResource.inINFO_ComponentesMecanicos_EliminadoCorrectamente;
                return RedirectToAction("ComponentesMecanicos", "AdministrarComponentesMecanicos", new { @id = TempData["equipo_id"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("ComponentesMecanicos", "AdministrarComponentesMecanicos", new { @id = TempData["equipo_id"] });

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
                return RedirectToAction("ComponentesMecanicos", "AdministrarComponentesMecanicos", new { @id = TempData["equipo_id"] });
            }

        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESMECANICOS-MODIFICAR")]
        public ActionResult EliminarArchivo(string archivo, string tipo, string caracteristica, int id)
        {
            try
            {
                _obrasManager.EliminarArchivo(archivo, tipo, caracteristica);
                TempData["FlashSuccess"] = MensajesResource.INFO_Obras_ActualizadoCorrectamente;
                return RedirectToAction("Editar", "AdministrarComponentesMecanicos", new { @id = TempData["componente"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Editar", "AdministrarComponentesMecanicos", new { @id = TempData["componente"] });

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
                return RedirectToAction("Editar", "AdministrarComponentesMecanicos", new { @id = TempData["componente"] });
            }

        }
    }
}