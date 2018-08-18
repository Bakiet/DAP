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
    public class AdministrarHerramientasController : Controller
    {
        private readonly HerramientasManager _herramientasManager = new HerramientasManager();
        private readonly ObrasManager _obrasManager = new ObrasManager();
        private readonly PreviosManager _previosManager = new PreviosManager();
        private readonly EquiposManager _equiposManager = new EquiposManager();

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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARHERRAMIENTAS-CONSULTAR")]

        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);


            ViewBag.Herramientas = db.herramientas.ToList();

           


            //ViewBag.EstatusCita = db.estatuscitas.ToList();

            return View();
        }

        public ActionResult Historico(int id)
        {
            var previo = _previosManager.Find(id);

            TempData["previo_id"] = id;
            TempData["previo"] = id;
            TempData.Keep();
            //var equipo = _equiposManager.Find(previo.equipo_id);

            ViewBag.obra = _obrasManager.Find(id);

            ViewBag.Herramientas = _herramientasManager.GetHistorico(id);
            return View("Index");
        }
        public ActionResult HerramientasDesdeObra(int id)
        {
            //var equipo = _equiposManager.FindPorObra(id);
           // var previo = _previosManager.FindPorEquipo(equipo.Id);

            ViewBag.obra = _obrasManager.Find(id);
            TempData["idobra"] = id;
            TempData.Keep();
            ViewBag.Herramientas = _herramientasManager.GetHerramientas(id);
            return View("Index");
        }

        public ActionResult Herramientas(int id)
        {
            var previo = _previosManager.Find(id);

            TempData["previo_id"] = id;
            TempData["previo"] = id;
            TempData.Keep();
            var equipo = _equiposManager.Find(previo.equipo_id);

            ViewBag.obra = _obrasManager.Find(equipo.obra_id);

            ViewBag.Herramientas = _herramientasManager.GetHerramientas(id);
            return View("Index");
        }

        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-LISTAR,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-MODIFICAR")]
        //public ActionResult Index()
        //{
        //    ViewBag.mensajes = _mensajesInstitucionalesManager.FindAll();
        //    return View();
        //}
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARHERRAMIENTAS-CREAR")]
        public ActionResult Crear()
        {
            var previoid = TempData["previo_id"];
            TempData["previo"] = previoid;
            TempData.Keep();
            //var previo = _previosManager.Find(Convert.ToInt32(previoid));

           // var equipo = _equiposManager.Find(previo.equipo_id);

            var obra = _obrasManager.Find(Convert.ToInt32(TempData["idobra"]));
            TempData.Keep();
            //model.obra_id = Convert.ToInt32(TempData["OBRA_ID"]);

           // TempData["OBRA_ID"] = equipo.obra_id;
           // TempData.Keep();
            /*
            ViewBag.Obras =
             new SelectList(_obrasManager.FindAll(), "id", "nombre");
            */
            ViewBag.Obras =
            new SelectList(_obrasManager.FindObras(obra.Id), "id", "nombre", obra.Id);

            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARHERRAMIENTAS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var herramienta = _herramientasManager.Find(id);
            TempData["HERRAMIENTA_ID"] = id;
            TempData.Keep();
            var obra = _obrasManager.Find(herramienta.obra_id);
            ViewBag.Obras =
             new SelectList(_obrasManager.FindObras(obra.Id), "id", "nombre", obra.Id);

            ViewBag.Archivos = _obrasManager.FindArchivos(id,"archivo", "herramientas");

            TempData["OBRA_ID"] = obra.Id;
            TempData.Keep();

            if (herramienta == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var herramientaModel = new HerramientaViewModel()
            {
                Descripcion = herramienta.Descripcion,
                Cantidad = herramienta.Cantidad.ToString(),
                FechaSalida = herramienta.FechaSalida,
                Propiedad = herramienta.Propiedad,
                FechaCulminacion = herramienta.FechaCulminacion,
                CantidadDeposito = herramienta.CantidadDeposito,
                FechaEntrada = herramienta.FechaEntrada,
                SupervisorObra = herramienta.SupervisorObra,
                TecnicoResponsable = herramienta.TecnicoResponsable,
                Observaciones = herramienta.Observaciones,
                obra_id = herramienta.obra_id,
                obra = "Obra",
                Archivo = herramienta.Archivo
            };

            return View(herramientaModel);
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARHERRAMIENTAS-MODIFICAR")]
        public ActionResult Detalle(int id)
        {
            var herramienta = _herramientasManager.Find(id);

            var obra = _obrasManager.Find(herramienta.obra_id);
            ViewBag.Obras =
             new SelectList(_obrasManager.FindObras(obra.Id), "id", "nombre", obra.Id);

            ViewBag.Archivos = _obrasManager.FindArchivos(id, "herramientas", "archivo");

            TempData["OBRA_ID"] = obra.Id;
            TempData.Keep();

            if (herramienta == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var herramientaModel = new HerramientaViewModel()
            {
                Descripcion = herramienta.Descripcion,
                Cantidad = herramienta.Cantidad.ToString(),
                FechaSalida = herramienta.FechaSalida,
                Propiedad = herramienta.Propiedad,
                FechaCulminacion = herramienta.FechaCulminacion,
                CantidadDeposito = herramienta.CantidadDeposito,
                FechaEntrada = herramienta.FechaEntrada,
                SupervisorObra = herramienta.SupervisorObra,
                TecnicoResponsable = herramienta.TecnicoResponsable,
                Observaciones = herramienta.Observaciones,
                obra_id = herramienta.obra_id,
                obra = "Obra",
                Archivo = herramienta.Archivo
            };

            return View(herramientaModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARHERRAMIENTAS-MODIFICAR")]
        public ActionResult Editar(int id, HerramientaViewModel model)
        {
            var Url = "";
            var pdfUrl = "";
            var herramienta = _herramientasManager.Find(id);
            var obra = _obrasManager.Find(herramienta.obra_id);

            ViewBag.Obras =
             new SelectList(_obrasManager.FindObras(obra.Id), "id", "nombre", obra.Id);
           /* ViewBag.Obras =
           new SelectList(_obrasManager.FindObras(obra.Id), "id", "nombre", obra.Id);
            */
            if (herramienta == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {
                HttpPostedFileBase pdf = Request.Files["Pdf"];

                if (pdf != null && pdf.ContentLength > 0)
                {
                    pdfUrl = CargarPdf(pdf);
                }
                else { pdfUrl = herramienta.Archivo; }

                
                _herramientasManager.Actualizar(Convert.ToInt32(TempData["OBRA_ID"]),
                      id,
                      model.Descripcion,
                      model.Cantidad,
                      model.FechaSalida,
                      model.Propiedad,
                      model.FechaCulminacion,
                      model.CantidadDeposito,
                      model.FechaEntrada,
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);

                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(id, Url, "herramientas", "archivo");
                    }


                }
                TempData.Keep();
                 TempData["FlashSuccess"] = MensajesResource.INFO_Herramientas_ActualizadoCorrectamente;
                return RedirectToAction("HerramientasDesdeObra", "AdministrarHerramientas", new { @id = TempData["OBRA_ID"] });
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



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARHERRAMIENTAS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(HerramientaViewModel model, FormCollection collection)
        {
            var Url = "";
            var pdfArchivo = "";
            if (!ModelState.IsValid) return View(model);

            try
            {
                HttpPostedFileBase pdf = Request.Files["Pdf"];

                ViewBag.Obras =
             new SelectList(_obrasManager.FindObras(Convert.ToInt32(TempData["idobra"])), "id", "nombre", TempData["idobra"]);
                TempData.Keep();
                model.obra_id = Convert.ToInt32(TempData["idobra"]);
                TempData.Keep();
                //if (model.Descripcion == null)
                //{
                //    model.Descripcion = "null";
                //}
                //if (model.Cantidad == null)
                //{
                //    model.Cantidad = "null";
                //}
                //if (model.FechaSalida == null)
                //{
                //    model.FechaSalida = DateTime.Now.ToString();
                //}
                //if (model.Propiedad == null)
                //{
                //    model.Propiedad = "null";
                //}
                //if (model.FechaCulminacion == null)
                //{
                //    model.FechaCulminacion = DateTime.Now.ToString();
                //}
                //if (model.CantidadDeposito == null)
                //{
                //    model.CantidadDeposito = "null";
                //}
                //if (model.FechaEntrada == null)
                //{
                //    model.FechaEntrada = DateTime.Now.ToString();
                //}
                //if (model.SupervisorObra == null)
                //{
                //    model.SupervisorObra = "null";
                //}
                //if (model.TecnicoResponsable == null)
                //{
                //    model.TecnicoResponsable = "null";
                //}
                //if (model.Observaciones == null)
                //{
                //    model.Observaciones = "null";
                //}
                herramientas herramienta = new herramientas();

                herramienta =  _herramientasManager.Crear(Convert.ToInt32(TempData["idobra"]),
                      model.Descripcion,
                      model.Cantidad,
                      model.FechaSalida,
                      model.Propiedad,
                      model.FechaCulminacion,
                      model.CantidadDeposito,
                      model.FechaEntrada,
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);

                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(herramienta.Id, Url, "herramientas", "archivo");
                    }


                }

                TempData.Keep();
                TempData["FlashSuccess"] = MensajesResource.INFO_Herramientas_CreadoCorrectamente;
                return RedirectToAction("HerramientasDesdeObra", "AdministrarHerramientas", new { @id = TempData["idobra"] });
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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARHERRAMIENTAS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _herramientasManager.Eliminar(id);
                TempData["FlashSuccess"] = MensajesResource.INFO_Herramientas_EliminadoCorrectamente;
                return RedirectToAction("HerramientasDesdeObra", "AdministrarHerramientas", new { @id = TempData["idobra"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("HerramientasDesdeObra", "AdministrarHerramientas", new { @id = TempData["idobra"] });

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
                return RedirectToAction("HerramientasDesdeObra", "AdministrarHerramientas", new { @id = TempData["idobra"] });
            }

        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARHERRAMIENTAS-MODIFICAR")]
        public ActionResult EliminarArchivo(string archivo, string tipo, string caracteristica, int id)
        {
            try
            {
                _obrasManager.EliminarArchivo(archivo, tipo, caracteristica);
                TempData["FlashSuccess"] = MensajesResource.INFO_Obras_ActualizadoCorrectamente;
                return RedirectToAction("Editar", "AdministrarHerramientas", new { @id = TempData["HERRAMIENTA_ID"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Editar", "AdministrarHerramientas", new { @id = TempData["HERRAMIENTA_ID"] });

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
                return RedirectToAction("Editar", "AdministrarHerramientas", new { @id = TempData["HERRAMIENTA_ID"] });
            }

        }
    }
}