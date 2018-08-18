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
    public class AdministrarEquiposController : Controller
    {
        private readonly EquiposManager _equiposManager = new EquiposManager();

        private readonly FallasManager _fallasManager = new FallasManager();

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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAREQUIPOS-CONSULTAR")]

        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);


            ViewBag.obra = db.obras.ToList();

            ViewBag.Equipos = db.equipos.ToList();

            ViewBag.previos = db.previos.ToList();

            ViewBag.componentesmecanicos = db.componentesmecanicos.ToList();

            ViewBag.componenteselectronicos = db.componenteselectricos.ToList();

           


            //ViewBag.EstatusCita = db.estatuscitas.ToList();

            return View();
        }

        public ActionResult Equipos(int id)
        {
            var db = new EntitiesDap();

            TempData["obra_equipo_id"] = id;

            TempData.Keep();

            ViewBag.id = id;

            ViewBag.obra = _obrasManager.Find(id);

            ViewBag.Equipos = _equiposManager.GetEquipos(id);

            ViewBag.previos = db.previos.ToList();

            ViewBag.componentesmecanicos = db.componentesmecanicos.ToList();

            ViewBag.componenteselectronicos = db.componenteselectricos.ToList();

            return View("Index");
        }

        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-LISTAR,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-MODIFICAR")]
        //public ActionResult Index()
        //{
        //    ViewBag.mensajes = _mensajesInstitucionalesManager.FindAll();
        //    return View();
        //}
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAREQUIPOS-CREAR")]
        public ActionResult Crear(int id)
        {
            var obra = _obrasManager.Find(id);

            
            ViewBag.Obras =
             new SelectList(_fallasManager.FindObras(), "id", "nombre", obra.Id);

            return View();
        }
       // [HttpGet]
        public ActionResult Detalle(int id)
        {
            var equipo = _equiposManager.Find(id);

            ViewBag.Obras =
              new SelectList(_equiposManager.FindObras(id), "id", "nombre", equipo.obra_id);

            var db = new EntitiesDap();

            

            ViewBag.ArchivosFotografia = _obrasManager.FindFotografiasEquipos(id);

            ViewBag.ArchivosPlano = _obrasManager.FindPlanosEquipos(id);

            if (equipo == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var equipoModel = new EquipoViewModel()
            {
                Nombre = equipo.Nombre,
                Marca = equipo.Marca,
                Modelo = equipo.Modelo,
                Referencia = equipo.Referencia,
                DimensionesCabina = equipo.DimensionesCabina,
                DimensionesHueco = equipo.DimensionesHueco,
                CargaNominal = equipo.CargaNominal,
                Velocidad = equipo.Velocidad,
                Recorrido = equipo.Recorrido,
                Paradas = equipo.Paradas,
                Accesos = equipo.Accesos,
                VoltajeDeRed = equipo.VoltajeDeRed,
                PotenciaDeMaquina = equipo.PotenciaDeMaquina,
                TipoDeManiobra = equipo.TipoDeManiobra,
                NumeroDeGuayas = equipo.NumeroDeGuayas,
                Fotografia = equipo.Fotografia,
                Plano = equipo.Plano,
                obra_id = equipo.obra_id,
                CantidadPersonas = Convert.ToString(equipo.CantidadPersonas),
                obra = "Obra",
                FechaGarantia = equipo.FechaGarantia.ToString()
            };

            return PartialView(equipoModel);
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAREQUIPOS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var equipo = _equiposManager.Find(id);
            TempData["equipo"] = id;
            TempData.Keep();
            var obra = _obrasManager.Find(equipo.obra_id);           

            ViewBag.ObrasEditar =
             new SelectList(_equiposManager.FindObras(), "id", "nombre", obra.Id);
            /*
            ViewBag.Obras2 =
              new SelectList(_obrasManager.FindAll(), "nombre", "nombre", obra.Nombre);

            */

            var db = new EntitiesDap();
            

            ViewBag.ArchivosFotografia = _obrasManager.FindFotografiasEquipos(id);

            ViewBag.ArchivosPlano = _obrasManager.FindPlanosEquipos(id);

            if (equipo == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var equipoModel = new EquipoViewModel()
            {
                Nombre = equipo.Nombre,
                Marca = equipo.Marca,
                Modelo = equipo.Modelo,
                Referencia = equipo.Referencia,
                DimensionesCabina = equipo.DimensionesCabina,
                DimensionesHueco = equipo.DimensionesHueco,
                CargaNominal = equipo.CargaNominal,
                Velocidad = equipo.Velocidad,
                Recorrido = equipo.Recorrido,
                Paradas = equipo.Paradas,
                Accesos = equipo.Accesos,
                VoltajeDeRed = equipo.VoltajeDeRed,
                PotenciaDeMaquina = equipo.PotenciaDeMaquina,
                TipoDeManiobra = equipo.TipoDeManiobra,
                NumeroDeGuayas = equipo.NumeroDeGuayas,
                Fotografia = equipo.Fotografia,
                Plano = equipo.Plano,
                obra_id = equipo.obra_id,
                CantidadPersonas = Convert.ToString(equipo.CantidadPersonas),
                obra = "Obra",
                FechaGarantia = equipo.FechaGarantia.ToString()
            };

            return View(equipoModel);
        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAREQUIPOS-MODIFICAR")]
        public ActionResult DetalleEquipo(int id)
        {
            var equipo = _equiposManager.Find(id);

            var obra = _obrasManager.Find(equipo.obra_id);

            ViewBag.ObrasEditar =
             new SelectList(_equiposManager.FindObras(), "id", "nombre", obra.Id);
            /*
            ViewBag.Obras2 =
              new SelectList(_obrasManager.FindAll(), "nombre", "nombre", obra.Nombre);

            */

            var db = new EntitiesDap();


            ViewBag.ArchivosFotografia = _obrasManager.FindFotografiasEquipos(id);

            ViewBag.ArchivosPlano = _obrasManager.FindPlanosEquipos(id);

            if (equipo == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var equipoModel = new EquipoViewModel()
            {
                Nombre = equipo.Nombre,
                Marca = equipo.Marca,
                Modelo = equipo.Modelo,
                Referencia = equipo.Referencia,
                DimensionesCabina = equipo.DimensionesCabina,
                DimensionesHueco = equipo.DimensionesHueco,
                CargaNominal = equipo.CargaNominal,
                Velocidad = equipo.Velocidad,
                Recorrido = equipo.Recorrido,
                Paradas = equipo.Paradas,
                Accesos = equipo.Accesos,
                VoltajeDeRed = equipo.VoltajeDeRed,
                PotenciaDeMaquina = equipo.PotenciaDeMaquina,
                TipoDeManiobra = equipo.TipoDeManiobra,
                NumeroDeGuayas = equipo.NumeroDeGuayas,
                Fotografia = equipo.Fotografia,
                Plano = equipo.Plano,
                obra_id = equipo.obra_id,
                CantidadPersonas = Convert.ToString(equipo.CantidadPersonas),
                obra = "Obra",
                FechaGarantia = equipo.FechaGarantia.ToString()
            };

            return View(equipoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAREQUIPOS-MODIFICAR")]
        public ActionResult Editar(int id, EquipoViewModel model)
        {
            var equipo = _equiposManager.Find(id);
            var fotografia = "";
            var plano = "";

            ViewBag.ArchivosFotografia = _obrasManager.FindFotografiasEquipos(id);

            ViewBag.ArchivosPlano = _obrasManager.FindPlanosEquipos(id);

            var Url = "";
            /*
            ViewBag.Obras =
              new SelectList(_equiposManager.FindObras(id), "id", "nombre", equipo.obra_id);
            */
            var obra = _obrasManager.Find(equipo.obra_id);

            ViewBag.ObrasEditar =
             new SelectList(_equiposManager.FindObras(), "id", "nombre", obra.Id);

            if (equipo == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {
                /* HttpPostedFileBase filefotografia = Request.Files["Pdf-Fotografia"];

                 if (filefotografia != null && filefotografia.ContentLength > 0)
                 {
                     fotografia = CargarPdf(filefotografia);
                 }
                 else { fotografia = equipo.Fotografia; }

                 HttpPostedFileBase fileplano = Request.Files["Pdf-Plano"];

                 if (fileplano != null && fileplano.ContentLength > 0)
                 {
                     plano = CargarPdf(fileplano);
                 }
                 else { plano = equipo.Plano; }
                  */
                DateTime? fechagarantia = null;
                if(model.FechaGarantia != null) { 
                _equiposManager.Actualizar(Convert.ToInt32(TempData["obra_equipo_id"]),
                      id,
                      model.Nombre,
                      model.Marca,
                      model.Modelo,
                      model.Referencia,
                      model.DimensionesCabina,
                      model.DimensionesHueco,
                      model.CargaNominal,
                      model.Velocidad,
                      model.Recorrido,
                      model.Paradas,
                      model.Accesos,
                      model.VoltajeDeRed,
                      model.PotenciaDeMaquina,
                      model.TipoDeManiobra,
                      model.NumeroDeGuayas,
                      model.CantidadPersonas,
                      fotografia.Trim(),
                      plano.Trim(),DateTime.Parse(model.FechaGarantia));
                }else
                {
                    _equiposManager.Actualizar(Convert.ToInt32(TempData["obra_equipo_id"]),
                     id,
                     model.Nombre,
                     model.Marca,
                     model.Modelo,
                     model.Referencia,
                     model.DimensionesCabina,
                     model.DimensionesHueco,
                     model.CargaNominal,
                     model.Velocidad,
                     model.Recorrido,
                     model.Paradas,
                     model.Accesos,
                     model.VoltajeDeRed,
                     model.PotenciaDeMaquina,
                     model.TipoDeManiobra,
                     model.NumeroDeGuayas,
                     model.CantidadPersonas,
                     fotografia.Trim(),
                     plano.Trim(), fechagarantia);
                }
                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf-Fotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(equipo.Id, Url, "equipos", "fotografia");
                    }
                    if (d == "Pdf-Plano" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(equipo.Id, Url, "equipos", "plano");
                    }

                }

                TempData["FlashSuccess"] = MensajesResource.INFO_Equipos_ActualizadoCorrectamente;
                return RedirectToAction("Equipos", "AdministrarEquipos", new { @id = TempData["obra_equipo_id"] });
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



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAREQUIPOS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(EquipoViewModel model, FormCollection collection)
        {
            var fotografia = "";
            var plano = "";
            var Url = "";

            ViewBag.Obras =
             new SelectList(_fallasManager.FindObras(), "id", "nombre");

           

            try
            {
                /*HttpPostedFileBase filefotografia = Request.Files["Pdf-Fotografia"];

                if (filefotografia != null && filefotografia.ContentLength > 0)
                {
                    fotografia = CargarPdf(filefotografia);
                }
                else { fotografia = ""; }

                HttpPostedFileBase fileplano = Request.Files["Pdf-Plano"];

                if (fileplano != null && fileplano.ContentLength > 0)
                {
                    plano = CargarPdf(fileplano);
                }
                else { plano = ""; }*/
                equipos equipo = new equipos();
                DateTime? fechagarantia = null;
                if(model.FechaGarantia != null) { 
                equipo =_equiposManager.Crear(
                        Convert.ToInt32(TempData["obra_equipo_id"]),
                      model.Nombre,
                      model.Marca,
                      model.Modelo,
                      model.Referencia,
                      model.DimensionesCabina,
                      model.DimensionesHueco,
                      model.CargaNominal,
                      model.Velocidad,
                      model.Recorrido,
                      model.Paradas,
                      model.Accesos,
                      model.VoltajeDeRed,
                      model.PotenciaDeMaquina,
                      model.TipoDeManiobra,
                      model.NumeroDeGuayas,
                      model.CantidadPersonas,
                      fotografia.Trim(),
                      plano.Trim(), DateTime.Parse(model.FechaGarantia));
                }
                else
                {
                    equipo = _equiposManager.Crear(
                        Convert.ToInt32(TempData["obra_equipo_id"]),
                      model.Nombre,
                      model.Marca,
                      model.Modelo,
                      model.Referencia,
                      model.DimensionesCabina,
                      model.DimensionesHueco,
                      model.CargaNominal,
                      model.Velocidad,
                      model.Recorrido,
                      model.Paradas,
                      model.Accesos,
                      model.VoltajeDeRed,
                      model.PotenciaDeMaquina,
                      model.TipoDeManiobra,
                      model.NumeroDeGuayas,
                      model.CantidadPersonas,
                      fotografia.Trim(),
                      plano.Trim(), fechagarantia);
                }
                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf-Fotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(equipo.Id, Url, "equipos", "fotografia");
                    }
                    if (d == "Pdf-Plano" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(equipo.Id, Url, "equipos", "plano");
                    }

                }

                TempData["FlashSuccess"] = MensajesResource.INFO_Equipos_CreadoCorrectamente;
                return RedirectToAction("Equipos","AdministrarEquipos", new { @id = TempData["obra_equipo_id"] } );
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

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAREQUIPOS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _equiposManager.Eliminar(id);
                TempData["FlashSuccess"] = MensajesResource.INFO_Equipos_EliminadoCorrectamente;
                return RedirectToAction("Equipos", "AdministrarEquipos", new { @id = TempData["obra_equipo_id"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Equipos", "AdministrarEquipos", new { @id = TempData["obra_equipo_id"] });

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
                return RedirectToAction("Equipos", "AdministrarEquipos", new { @id = TempData["obra_equipo_id"] });
            }

        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAROBRAS-MODIFICAR")]
        public ActionResult EliminarArchivo(string archivo, string tipo, string caracteristica, int id)
        {
            try
            {
                _obrasManager.EliminarArchivo(archivo, tipo, caracteristica);
                TempData["FlashSuccess"] = MensajesResource.INFO_Obras_ActualizadoCorrectamente;
                return RedirectToAction("Editar", "AdministrarEquipos", new { @id = TempData["equipo"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Editar", "AdministrarEquipos", new { @id = TempData["equipo"] });

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
                return RedirectToAction("Editar", "AdministrarEquipos", new { @id = TempData["equipo"] });
            }

        }
    }
}