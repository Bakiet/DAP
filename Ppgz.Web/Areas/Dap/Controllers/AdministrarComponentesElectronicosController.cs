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
    public class AdministrarComponentesElectronicosController : Controller
    {
        private readonly ComponentesElectricosManager _componentesElectricos_Manager = new ComponentesElectricosManager();

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
        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRONICOS-LISTAR,NAZAN-ADMINISTRARCOMPONENTESELECTRONICOS-MODIFICAR")]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRONICOS-CONSULTAR")]
        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);


            ViewBag.ComponentesElectricos = db.componenteselectricos.ToList();

           


            //ViewBag.EstatusCita = db.estatuscitas.ToList();

            return View();
        }
        public ActionResult Historico(int id)
        {
            var equipo = _equiposManager.Find(id);

            ViewBag.Equipo = equipo;

            ViewBag.obra = _obrasManager.Find(equipo.obra_id);

            TempData["obra_id"] = equipo.obra_id;

            ViewBag.ComponentesElectricos = _componentesElectricos_Manager.GetHistorico(id);
            ViewBag.id = id;
            return View("Index");
        }
        public ActionResult ComponentesElectronicos(int id)
        {
            var equipo = _equiposManager.Find(id);

            TempData["equipo_id"] = id;

            ViewBag.Equipo = equipo;

            ViewBag.obra = _obrasManager.Find(equipo.obra_id);

            ViewBag.ComponentesElectricos = _componentesElectricos_Manager.GetComponentesElectricos(id);

            ViewBag.id = id;

            TempData["obra_id"] = equipo.obra_id;

            return View("Index");
        }
        public ActionResult ReporteComponentesElectronicos(int id)
        {
            var equipo = _equiposManager.Find(id);

            TempData["equipo_id"] = id;

            ViewBag.Equipo = equipo;

            ViewBag.obra = _obrasManager.Find(equipo.obra_id);

            ViewBag.ComponentesElectricos = _componentesElectricos_Manager.GetComponentesElectricos(id);

            ViewBag.id = id;

            TempData["obra_id"] = equipo.obra_id;

            return PartialView("Index");
        }


        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRONICOS-CREAR")]
        public ActionResult Crear()
        {
            ViewBag.ComponentesElectronicos_Tipos =
                new SelectList(_componentesElectricos_Manager.FindTipos(), "descripcion", "descripcion");

            TempData["obra"] = TempData["obra_id"];
            TempData["equipo"] = TempData["equipo_id"];

            ViewBag.obra = _obrasManager.Find(Convert.ToInt32(TempData["obra_id"]));


            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRONICOS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            TempData["componente"] = id;
            TempData.Keep();
            var componenteelectrico = _componentesElectricos_Manager.Find(id);
            ViewBag.ComponentesElectronicos_Tipos =
               new SelectList(_componentesElectricos_Manager.FindTipos(), "descripcion", "descripcion");

            ViewBag.ArchivosFotografia = _obrasManager.FindFotografiasComponentesElectronicos(id);

            ViewBag.Sustitucion2 =
              new SelectList(new[] { new { ID = "SI", Name = "SI" }, new { ID = "NO", Name = "NO" }, }, "ID", "Name", componenteelectrico.Sustitucion);

            TempData["obra"] = TempData["obra_id"];

            ViewBag.obra = _obrasManager.Find(Convert.ToInt32(TempData["obra_id"]));

            if (componenteelectrico == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var componenteelectricoModel = new ComponenteElectricoViewModel()
            {
                Tipo = componenteelectrico.Tipo,
                Caracteristicas = componenteelectrico.Caracteristicas,
                Descripcion = componenteelectrico.Descripcion,
                Marca = componenteelectrico.Marca,
                Modelo = componenteelectrico.Modelo,
                Serial = componenteelectrico.Serial,
                FechaFabricado = componenteelectrico.FechaFabricado,
                Duracion = componenteelectrico.Duracion,
                Sustitucion = componenteelectrico.Sustitucion,
                Fotografia = componenteelectrico.Fotografia,

            };

            return View(componenteelectricoModel);
        }

        public ActionResult Reporte(int id)
        {
            TempData["componente"] = id;
            TempData.Keep();
            var componenteelectrico = _componentesElectricos_Manager.Find(id);
            ViewBag.ComponentesElectronicos_Tipos =
               new SelectList(_componentesElectricos_Manager.FindTipos(), "descripcion", "descripcion");

            ViewBag.ArchivosFotografia = _obrasManager.FindFotografiasComponentesElectronicos(id);

            ViewBag.Sustitucion2 =
              new SelectList(new[] { new { ID = "SI", Name = "SI" }, new { ID = "NO", Name = "NO" }, }, "ID", "Name", componenteelectrico.Sustitucion);

            TempData["obra"] = TempData["obra_id"];

            ViewBag.obra = _obrasManager.Find(Convert.ToInt32(TempData["obra_id"]));

            if (componenteelectrico == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var componenteelectricoModel = new ComponenteElectricoViewModel()
            {
                Tipo = componenteelectrico.Tipo,
                Caracteristicas = componenteelectrico.Caracteristicas,
                Descripcion = componenteelectrico.Descripcion,
                Marca = componenteelectrico.Marca,
                Modelo = componenteelectrico.Modelo,
                Serial = componenteelectrico.Serial,
                FechaFabricado = componenteelectrico.FechaFabricado,
                Duracion = componenteelectrico.Duracion,
                Sustitucion = componenteelectrico.Sustitucion,
                Fotografia = componenteelectrico.Fotografia,

            };

            return PartialView(componenteelectricoModel);
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRONICOS-CONSULTAR")]
        public ActionResult Detalle(int id)
        {
            var componenteelectrico = _componentesElectricos_Manager.Find(id);
            ViewBag.ComponentesElectronicos_Tipos =
               new SelectList(_componentesElectricos_Manager.FindTipos(), "descripcion", "descripcion");

            ViewBag.ArchivosFotografia = _obrasManager.FindFotografiasComponentesElectronicos(id);

            ViewBag.Sustitucion2 =
              new SelectList(new[] { new { ID = "SI", Name = "SI" }, new { ID = "NO", Name = "NO" }, }, "ID", "Name", componenteelectrico.Sustitucion);

            TempData["obra"] = TempData["obra_id"];

            ViewBag.obra = _obrasManager.Find(Convert.ToInt32(TempData["obra_id"]));

            if (componenteelectrico == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var componenteelectricoModel = new ComponenteElectricoViewModel()
            {
                Tipo = componenteelectrico.Tipo,
                Caracteristicas = componenteelectrico.Caracteristicas,
                Descripcion = componenteelectrico.Descripcion,
                Marca = componenteelectrico.Marca,
                Modelo = componenteelectrico.Modelo,
                Serial = componenteelectrico.Serial,
                FechaFabricado = componenteelectrico.FechaFabricado,
                Duracion = componenteelectrico.Duracion,
                Sustitucion = componenteelectrico.Sustitucion,
                Fotografia = componenteelectrico.Fotografia,

            };

            return View(componenteelectricoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRONICOS-MODIFICAR")]
        public ActionResult Editar(int id, ComponenteElectricoViewModel model)
        {
            var Url = "";
            var pdfUrl = "";

            var componenteelectrico = _componentesElectricos_Manager.Find(id);

            ViewBag.obra = _obrasManager.Find(Convert.ToInt32(TempData["obra_id"]));

            ViewBag.Sustitucion2 =
              new SelectList(new[] { new { ID = "SI", Name = "SI" }, new { ID = "NO", Name = "NO" }, }, "ID", "Name", componenteelectrico.Sustitucion);

            if (componenteelectrico == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {/*
                HttpPostedFileBase pdf = Request.Files["Pdf"];

                if (pdf != null && pdf.ContentLength > 0)
                {
                    pdfUrl = CargarPdf(pdf);
                }
                else { pdfUrl = componenteelectrico.Fotografia; }
                */
                _componentesElectricos_Manager.Actualizar(
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

                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(id, Url, "componenteselectricos", "fotografia");
                    }


                }
                // TempData["FlashSuccess"] = MensajesResource.INFO_UsuarioNazan_ActualizadoCorrectamente;
                // return RedirectToAction("Editar", "AdministrarComponentesElectronicos", new { @id = id });
                return RedirectToAction("ComponentesElectronicos", "AdministrarComponentesElectronicos", new { @id = TempData["equipo_id"] });
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
        public ActionResult Crear(ComponenteElectricoViewModel model, FormCollection collection)
        {
            var Url = "";
            var pdfUrl = "";

            ViewBag.obra = _obrasManager.Find(Convert.ToInt32(TempData["obra"]));



            if (!ModelState.IsValid) return View(model);

            try
            {/*
                HttpPostedFileBase pdf = Request.Files["Pdf"];

                if (pdf != null && pdf.ContentLength > 0)
                {
                    pdfUrl = CargarPdf(pdf);
                }
                else { pdfUrl = ""; }
                */
                componenteselectricos ce =_componentesElectricos_Manager.Crear(Convert.ToInt32(TempData["equipo"]),
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

                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(ce.Id, Url, "componenteselectricos", "fotografia");
                    }


                }
                //TempData["FlashSuccess"] = MensajesResource.INFO_MensajesInstitucionales_CreadoCorrectamente;
                return RedirectToAction("ComponentesElectronicos", "AdministrarComponentesElectronicos", new { @id = TempData["equipo"] });
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

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRONICOS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _componentesElectricos_Manager.Eliminar(id);
                // TempData["FlashSuccess"] = MensajesResource.INFO_MensajesInstitucionales_EliminadoCorrectamente;
                return RedirectToAction("ComponentesElectronicos", "AdministrarComponentesElectronicos", new { @id = TempData["equipo_id"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("ComponentesElectronicos", "AdministrarComponentesElectronicos", new { @id = TempData["equipo_id"] });

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
                return RedirectToAction("ComponentesElectronicos", "AdministrarComponentesElectronicos", new { @id = TempData["equipo_id"] });
            }

        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCOMPONENTESELECTRONICOS-MODIFICAR")]
        public ActionResult EliminarArchivo(string archivo, string tipo, string caracteristica, int id)
        {
            try
            {
                _obrasManager.EliminarArchivo(archivo, tipo, caracteristica);
                TempData["FlashSuccess"] = MensajesResource.INFO_Obras_ActualizadoCorrectamente;
                return RedirectToAction("Editar", "AdministrarComponentesElectronicos", new { @id = TempData["componente"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Editar", "AdministrarComponentesElectronicos", new { @id = TempData["componente"] });

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
                return RedirectToAction("Editar", "AdministrarComponentesElectronicos", new { @id = TempData["componente"] });
            }

        }

    }
}