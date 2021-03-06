﻿using System;
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
        private readonly ComponentesMecanicosManager _componentesmecanicosManager = new ComponentesMecanicosManager();
        private readonly ComponentesElectricosManager _componentesElectricos_Manager = new ComponentesElectricosManager();
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


            ViewBag.Herramientas = _herramientasManager.GetHerramientasAll();
            ViewBag.ComponentesMecanicosCount = _componentesmecanicosManager.GetSustituciones();
            ViewBag.ComponentesMecanicos = _componentesmecanicosManager.GetSustituciones();
            TempData["sustitucionesmecanicas"] = ViewBag.ComponentesMecanicosCount.Count;
            TempData.Keep();

            ViewBag.ComponentesElectricosCount = _componentesElectricos_Manager.GetSustituciones();
            ViewBag.ComponentesElectricos = _componentesElectricos_Manager.GetSustituciones();
            TempData["sustitucioneselectronicas"] = ViewBag.ComponentesElectricosCount.Count;
            TempData.Keep();

            ViewBag.Obras =
         new SelectList(_obrasManager.FindAll(), "nombre", "nombre");

            ViewBag.Archivos = "";

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
            ViewBag.Archivos = "";
            ViewBag.obra = _obrasManager.Find(id);

            ViewBag.Herramientas = _herramientasManager.GetHistorico(id);
            return View("Index");
        }
        public ActionResult HistoricoTodos()
        {
            ViewBag.Archivos = "";
            ViewBag.Herramientas = _herramientasManager.GetHistoricoAll();
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
            ViewBag.Archivos = _obrasManager.FindArchivos(id, "archivo", "herramientas");
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
            ViewBag.Archivos = _obrasManager.FindArchivos(equipo.obra_id, "archivo", "herramientas");
            return View("Index");
        }

        public ActionResult ReporteHerramientas(int id)
        {
            var previo = _previosManager.Find(id);
            var db = new EntitiesDap();
            TempData["previo_id"] = id;
            TempData["previo"] = id;
            TempData.Keep();
            var equipo = _equiposManager.Find(previo.equipo_id);

            ViewBag.obra = _obrasManager.Find(equipo.obra_id);
            if (id != 0) { 
            ViewBag.Herramientas = _herramientasManager.GetHerramientas(id);
            }
            else
            {
                ViewBag.Herramientas = _herramientasManager.GetHerramientasAll();
            }
            return PartialView("Index");
        }

        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-LISTAR,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-MODIFICAR")]
        //public ActionResult Index()
        //{
        //    ViewBag.mensajes = _mensajesInstitucionalesManager.FindAll();
        //    return View();
        //}
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARREQUERIMIENTOS-CREAR")]
        public ActionResult CrearPorDefecto()
        {
            var db = new EntitiesDap();

            ViewBag.Obras =
         new SelectList(_obrasManager.FindAll(), "nombre", "nombre");

            return View();
        }
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
            if(obra != null)
            {
                ViewBag.Obras =
             new SelectList(_obrasManager.FindObras(obra.Id), "id", "nombre", obra.Id);

                TempData["OBRA_ID"] = obra.Id;
                TempData.Keep();


            }
            else
            {
                ViewBag.Obras = new SelectList(_obrasManager.FindAll(), "id", "nombre");
            }

            ViewBag.Archivos = _obrasManager.FindArchivos(id,"archivo", "herramientas");

          
            if (herramienta == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var herramientaModel = new HerramientaViewModel()
            {
                Descripcion = herramienta.Descripcion,
                Cantidad = herramienta.Cantidad.ToString(),
                FechaSalida = herramienta.FechaSalida.ToString(),
                Propiedad = herramienta.Propiedad,
                FechaCulminacion = herramienta.FechaCulminacion.ToString(),
                CantidadDeposito = herramienta.CantidadDeposito,
                FechaEntrada = herramienta.FechaEntrada.ToString(),
                SupervisorObra = herramienta.SupervisorObra,
                TecnicoResponsable = herramienta.TecnicoResponsable,
                Observaciones = herramienta.Observaciones,
                obra_id = herramienta.obra_id,
                obra = "Obra",
                Archivo = herramienta.Archivo
            };

            return View(herramientaModel);
        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARHERRAMIENTAS-CONSULTAR")]
        public ActionResult Reporte(int id)
        {
            var herramienta = _herramientasManager.Find(id);
            TempData["HERRAMIENTA_ID"] = id;
            TempData.Keep();
            var obra = _obrasManager.Find(herramienta.obra_id);
            if(obra != null) { 
            ViewBag.Obras =
             new SelectList(_obrasManager.FindObras(obra.Id), "id", "nombre", obra.Id);

                TempData["OBRA_ID"] = obra.Id;
                TempData.Keep();
            }
            else
            {
                ViewBag.Obras = "";
            }
            ViewBag.Archivos = _obrasManager.FindArchivos(id, "archivo", "herramientas");

            

            if (herramienta == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var herramientaModel = new HerramientaViewModel()
            {
                Descripcion = herramienta.Descripcion,
                Cantidad = herramienta.Cantidad.ToString(),
                FechaSalida = herramienta.FechaSalida.ToString(),
                Propiedad = herramienta.Propiedad,
                FechaCulminacion = herramienta.FechaCulminacion.ToString(),
                CantidadDeposito = herramienta.CantidadDeposito,
                FechaEntrada = herramienta.FechaEntrada.ToString(),
                SupervisorObra = herramienta.SupervisorObra,
                TecnicoResponsable = herramienta.TecnicoResponsable,
                Observaciones = herramienta.Observaciones,
                obra_id = herramienta.obra_id,
                obra = herramienta.obra,
                Archivo = herramienta.Archivo
            };

            return PartialView(herramientaModel);
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
                FechaSalida = herramienta.FechaSalida.ToString(),
                Propiedad = herramienta.Propiedad,
                FechaCulminacion = herramienta.FechaCulminacion.ToString(),
                CantidadDeposito = herramienta.CantidadDeposito,
                FechaEntrada = herramienta.FechaEntrada.ToString(),
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

            ViewBag.Archivos = _obrasManager.FindArchivos(id, "archivo", "herramientas");
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

                //if(model.FechaSalida == null)
                //{
                //    model.FechaSalida = "01/01/0001";
                //}
                //if(model.FechaCulminacion == null)
                //{
                //    model.FechaCulminacion = "01/01/0001";
                //}
                //if(model.FechaEntrada == null)
                //{
                //    model.FechaEntrada = "01/01/0001";
                //}
                if(model.Cantidad == null)
                {
                    model.Cantidad = "0";
                }
                DateTime? fechasalida = null;
                DateTime? fechaentrada = null;
                DateTime? fechaculminacion = null;

               

                if (model.FechaSalida != null && model.FechaEntrada == null && model.FechaCulminacion == null)
                {
                     _herramientasManager.Actualizar(Convert.ToInt32(TempData["OBRA_ID"]), 
                         id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                       DateTime.Parse(model.FechaSalida),
                      model.Propiedad,
                      fechaculminacion,
                      model.CantidadDeposito,
                      fechaentrada,
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaEntrada != null && model.FechaSalida == null && model.FechaCulminacion == null)
                {
                     _herramientasManager.Actualizar(Convert.ToInt32(TempData["OBRA_ID"]), 
                         id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      fechasalida,
                      model.Propiedad,
                      fechaculminacion,
                      model.CantidadDeposito,
                       DateTime.Parse(model.FechaEntrada),
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion != null && model.FechaSalida == null && model.FechaEntrada == null)
                {
                    _herramientasManager.Actualizar(Convert.ToInt32(TempData["OBRA_ID"]), 
                        id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      fechasalida,
                      model.Propiedad,
                       DateTime.Parse(model.FechaCulminacion),
                      model.CantidadDeposito,
                      fechaentrada,
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion != null && model.FechaSalida != null && model.FechaEntrada != null)
                {
                    _herramientasManager.Actualizar(Convert.ToInt32(TempData["OBRA_ID"]), 
                        id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      DateTime.Parse(model.FechaSalida),
                      model.Propiedad,
                       DateTime.Parse(model.FechaCulminacion),
                      model.CantidadDeposito,
                      DateTime.Parse(model.FechaEntrada),
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion != null && model.FechaSalida != null && model.FechaEntrada == null)
                {
                     _herramientasManager.Actualizar(Convert.ToInt32(TempData["OBRA_ID"]), 
                         id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      DateTime.Parse(model.FechaSalida),
                      model.Propiedad,
                       DateTime.Parse(model.FechaCulminacion),
                      model.CantidadDeposito,
                      fechaentrada,
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion != null && model.FechaSalida == null && model.FechaEntrada != null)
                {
                     _herramientasManager.Actualizar(Convert.ToInt32(TempData["OBRA_ID"]),
                         id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      fechasalida,
                      model.Propiedad,
                       DateTime.Parse(model.FechaCulminacion),
                      model.CantidadDeposito,
                      DateTime.Parse(model.FechaEntrada),
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion == null && model.FechaSalida != null && model.FechaEntrada != null)
                {
                   _herramientasManager.Actualizar(Convert.ToInt32(TempData["OBRA_ID"]), 
                       id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      DateTime.Parse(model.FechaSalida),
                      model.Propiedad,
                      fechaculminacion,
                      model.CantidadDeposito,
                      DateTime.Parse(model.FechaEntrada),
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }

                if (model.FechaCulminacion == null && model.FechaSalida == null && model.FechaEntrada == null)
                {
                    _herramientasManager.Actualizar(Convert.ToInt32(TempData["OBRA_ID"]), 
                        id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      fechasalida,
                      model.Propiedad,
                      fechaculminacion,
                      model.CantidadDeposito,
                      fechaentrada,
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
               
             
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
               // TempData.Keep();
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
                
                herramientas herramienta = new herramientas();
                
                //if (model.FechaSalida == null)
                //{
                //    model.FechaSalida = "01/01/0001";
                //}
                //if (model.FechaCulminacion == null)
                //{
                //    model.FechaCulminacion = "01/01/0001";
                //}
                //if (model.FechaEntrada == null)
                //{
                //    model.FechaEntrada = "01/01/0001";
                //}
                if (model.Cantidad == null)
                {
                    model.Cantidad = "0";
                }
                DateTime? fechasalida = null;
                DateTime? fechaentrada = null;
                DateTime? fechaculminacion = null;

                obras obra = _obrasManager.Find(Convert.ToInt32(TempData["idobra"]));

                if (model.FechaSalida != null && model.FechaEntrada == null && model.FechaCulminacion == null)
                {
                    herramienta = _herramientasManager.Crear(Convert.ToInt32(TempData["idobra"]),obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                       DateTime.Parse(model.FechaSalida),
                      model.Propiedad,
                      fechaculminacion,
                      model.CantidadDeposito,
                      fechaentrada,
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaEntrada != null && model.FechaSalida == null && model.FechaCulminacion == null)
                {
                    herramienta = _herramientasManager.Crear(Convert.ToInt32(TempData["idobra"]), obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      fechasalida,
                      model.Propiedad,
                      fechaculminacion,
                      model.CantidadDeposito,
                       DateTime.Parse(model.FechaEntrada),
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion != null && model.FechaSalida == null && model.FechaEntrada == null)
                {
                    herramienta = _herramientasManager.Crear(Convert.ToInt32(TempData["idobra"]), obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      fechasalida,
                      model.Propiedad,
                       DateTime.Parse(model.FechaCulminacion),
                      model.CantidadDeposito,
                      fechaentrada,
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion != null && model.FechaSalida != null && model.FechaEntrada != null)
                {
                    herramienta = _herramientasManager.Crear(Convert.ToInt32(TempData["idobra"]), obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      DateTime.Parse(model.FechaSalida),
                      model.Propiedad,
                       DateTime.Parse(model.FechaCulminacion),
                      model.CantidadDeposito,
                      DateTime.Parse(model.FechaEntrada),
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion != null && model.FechaSalida != null && model.FechaEntrada == null)
                {
                    herramienta = _herramientasManager.Crear(Convert.ToInt32(TempData["idobra"]), obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      DateTime.Parse(model.FechaSalida),
                      model.Propiedad,
                       DateTime.Parse(model.FechaCulminacion),
                      model.CantidadDeposito,
                      fechaentrada,
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion != null && model.FechaSalida == null && model.FechaEntrada != null)
                {
                    herramienta = _herramientasManager.Crear(Convert.ToInt32(TempData["idobra"]), obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      fechasalida,
                      model.Propiedad,
                       DateTime.Parse(model.FechaCulminacion),
                      model.CantidadDeposito,
                      DateTime.Parse(model.FechaEntrada),
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion == null && model.FechaSalida != null && model.FechaEntrada != null)
                {
                    herramienta = _herramientasManager.Crear(Convert.ToInt32(TempData["idobra"]), obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      DateTime.Parse(model.FechaSalida),
                      model.Propiedad,
                      fechaculminacion,
                      model.CantidadDeposito,
                      DateTime.Parse(model.FechaEntrada),
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }

                if (model.FechaCulminacion == null && model.FechaSalida == null && model.FechaEntrada == null)
                {
                    herramienta = _herramientasManager.Crear(Convert.ToInt32(TempData["idobra"]), obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      fechasalida,
                      model.Propiedad,
                      fechaculminacion,
                      model.CantidadDeposito,
                      fechaentrada,
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }


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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARHERRAMIENTAS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearPorDefecto(HerramientaViewModel model, FormCollection collection)
        {
            var Url = "";
            var pdfArchivo = "";
            if (!ModelState.IsValid) return View(model);

            try
            {
                HttpPostedFileBase pdf = Request.Files["Pdf"];

                var obra = _obrasManager.FindObrasPorNombre(model.obra);

                ViewBag.Obras =
             new SelectList(_obrasManager.FindObras(Convert.ToInt32(TempData["idobra"])), "id", "nombre", obra.Id);
                TempData.Keep();
                model.obra_id = obra.Id;
                TempData.Keep();

                herramientas herramienta = new herramientas();

               
                if (model.Cantidad == null)
                {
                    model.Cantidad = "0";
                }
                DateTime? fechasalida = null;
                DateTime? fechaentrada = null;
                DateTime? fechaculminacion = null;

               

                if (model.FechaSalida != null && model.FechaEntrada == null && model.FechaCulminacion == null)
                {
                    herramienta = _herramientasManager.Crear(obra.Id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                       DateTime.Parse(model.FechaSalida),
                      model.Propiedad,
                      fechaculminacion,
                      model.CantidadDeposito,
                      fechaentrada,
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaEntrada != null && model.FechaSalida == null && model.FechaCulminacion == null)
                {
                    herramienta = _herramientasManager.Crear(obra.Id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      fechasalida,
                      model.Propiedad,
                      fechaculminacion,
                      model.CantidadDeposito,
                       DateTime.Parse(model.FechaEntrada),
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion != null && model.FechaSalida == null && model.FechaEntrada == null)
                {
                    herramienta = _herramientasManager.Crear(obra.Id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      fechasalida,
                      model.Propiedad,
                       DateTime.Parse(model.FechaCulminacion),
                      model.CantidadDeposito,
                      fechaentrada,
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion != null && model.FechaSalida != null && model.FechaEntrada != null)
                {
                    herramienta = _herramientasManager.Crear(obra.Id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      DateTime.Parse(model.FechaSalida),
                      model.Propiedad,
                       DateTime.Parse(model.FechaCulminacion),
                      model.CantidadDeposito,
                      DateTime.Parse(model.FechaEntrada),
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion != null && model.FechaSalida != null && model.FechaEntrada == null)
                {
                    herramienta = _herramientasManager.Crear(obra.Id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      DateTime.Parse(model.FechaSalida),
                      model.Propiedad,
                       DateTime.Parse(model.FechaCulminacion),
                      model.CantidadDeposito,
                      fechaentrada,
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion != null && model.FechaSalida == null && model.FechaEntrada != null)
                {
                    herramienta = _herramientasManager.Crear(obra.Id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      fechasalida,
                      model.Propiedad,
                       DateTime.Parse(model.FechaCulminacion),
                      model.CantidadDeposito,
                      DateTime.Parse(model.FechaEntrada),
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }
                if (model.FechaCulminacion == null && model.FechaSalida != null && model.FechaEntrada != null)
                {
                    herramienta = _herramientasManager.Crear(obra.Id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      DateTime.Parse(model.FechaSalida),
                      model.Propiedad,
                      fechaculminacion,
                      model.CantidadDeposito,
                      DateTime.Parse(model.FechaEntrada),
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }

                if (model.FechaCulminacion == null && model.FechaSalida == null && model.FechaEntrada == null)
                {
                    herramienta = _herramientasManager.Crear(obra.Id, obra.Nombre,
                      model.Descripcion,
                      model.Cantidad,
                      fechasalida,
                      model.Propiedad,
                      fechaculminacion,
                      model.CantidadDeposito,
                      fechaentrada,
                      model.SupervisorObra,
                      model.TecnicoResponsable,
                      model.Observaciones);
                }


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
                return RedirectToAction("Index", "AdministrarHerramientas");
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
                if(TempData["idobra"] != null) { 
                    _herramientasManager.Eliminar(id);
                    TempData["FlashSuccess"] = MensajesResource.INFO_Herramientas_EliminadoCorrectamente;
                    return RedirectToAction("HerramientasDesdeObra", "AdministrarHerramientas", new { @id = TempData["idobra"] });
                }
                else
                {
                    _herramientasManager.Eliminar(id);
                    TempData["FlashSuccess"] = MensajesResource.INFO_Herramientas_EliminadoCorrectamente;
                    return RedirectToAction("Index", "AdministrarHerramientas");
                }
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