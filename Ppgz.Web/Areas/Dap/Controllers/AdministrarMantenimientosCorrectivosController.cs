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
    public class AdministrarMantenimientosCorrectivosController : Controller
    {
        private readonly ComponentesMecanicosManager _componentesmecanicosManager = new ComponentesMecanicosManager();
        private readonly ComponentesElectricosManager _componentesElectricos_Manager = new ComponentesElectricosManager();
        private readonly FallasManager _fallasManager = new FallasManager();

        private readonly EquiposManager _equiposManager = new EquiposManager();

        private readonly MantenimientosCorrectivosManager _mantenimientoscorrectivosManager = new MantenimientosCorrectivosManager();

        private readonly VentasManager _ventasManager = new VentasManager();

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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSCORRECTIVOS-CONSULTAR")]

        public ActionResult Index(int id)
        {
            var db = new EntitiesDap();
            TempData["venta_id"] = id;
            TempData.Keep();
            var fecha = DateTime.Today.AddMonths(-3);
            ViewBag.ComponentesMecanicosCount = _componentesmecanicosManager.GetSustituciones();
            ViewBag.ComponentesMecanicos = _componentesmecanicosManager.GetSustituciones();
            TempData["sustitucionesmecanicas"] = ViewBag.ComponentesMecanicosCount.Count;
            TempData.Keep();

            ViewBag.ComponentesElectricosCount = _componentesElectricos_Manager.GetSustituciones();
            ViewBag.ComponentesElectricos = _componentesElectricos_Manager.GetSustituciones();
            TempData["sustitucioneselectronicas"] = ViewBag.ComponentesElectricosCount.Count;
            TempData.Keep();
            var mantenimientocorrectivo = _mantenimientoscorrectivosManager.GetMantenimientoCorrectivo(id);

            if(mantenimientocorrectivo != null) { 
            var venta = _ventasManager.Find(mantenimientocorrectivo.IdVenta);
                TempData["IDVENTA"] = venta.Id;
                TempData.Keep();
                ViewBag.obra = _obrasManager.Find(venta.IdObra);

                TempData["IDOBRA"] = venta.IdObra;
                TempData.Keep();
                ViewBag.MantenimientosCorrectivos = db.mantenimientocorrectivo.ToList();


                if (id != 0)
                {
                    ViewBag.MantenimientosCorrectivos = _mantenimientoscorrectivosManager.GetMantenimientosCorrectivos(venta.Id);
                }
                else
                {
                    ViewBag.MantenimientosCorrectivos = db.mantenimientopreventivo.ToList();
                };
            }

            else
            {

                ViewBag.venta = null;
                ViewBag.obra = null;
                ViewBag.MantenimientosCorrectivos = null;
                // ViewBag.obra = _obrasManager.Find(venta.IdObra);
                //TempData["IDOBRA"] = venta.IdObra;
                // TempData.Keep();
            }

            ViewBag.StatusMantenimiento =
               new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name");



            //ViewBag.EstatusCita = db.estatuscitas.ToList();

            return View();
        }

        public ActionResult MantenimientosCorrectivos(int id)
        {

            //var mantenimiento = _mantenimientoscorrectivosManager.Find(id);

            var venta = _ventasManager.Find(id);

            ViewBag.obra = _obrasManager.Find(venta.IdObra);

            var obra = _obrasManager.Find(venta.IdObra);
            TempData["IDVENTA"] = id;
            TempData.Keep();
            ViewBag.Equipo_Tipo =
                  new SelectList(_fallasManager.FindEquiposPorObra(obra.Id), "nombre", "nombre");

            ViewBag.MantenimientosCorrectivos = _mantenimientoscorrectivosManager.GetMantenimientosCorrectivos(id);

            ViewBag.StatusMantenimiento =
             new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name");

            return View("Index");
        }

        public ActionResult ReporteMantenimientosCorrectivos(int id)
        {

            //var mantenimiento = _mantenimientoscorrectivosManager.Find(id);

            var venta = _ventasManager.Find(id);

            ViewBag.obra = _obrasManager.Find(venta.IdObra);

            var obra = _obrasManager.Find(venta.IdObra);

            ViewBag.Equipo_Tipo =
                  new SelectList(_fallasManager.FindEquiposPorObra(obra.Id), "nombre", "nombre");

            ViewBag.MantenimientosCorrectivos = _mantenimientoscorrectivosManager.GetMantenimientosCorrectivos(id);

            ViewBag.StatusMantenimiento =
             new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name");

            return PartialView();
        }

        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-LISTAR,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-MODIFICAR")]
        //public ActionResult Index()
        //{
        //    ViewBag.mensajes = _mensajesInstitucionalesManager.FindAll();
        //    return View();
        //}
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSCORRECTIVOS-CREAR")]
        public ActionResult Crear()
        {

 
            var idventa = TempData["venta_id"];
            TempData.Keep();
            TempData["venta"] = idventa;
            TempData.Keep();
            //TempData["IDVENTA"] = idventa;
            var venta = _ventasManager.Find(Convert.ToInt32(idventa));
            var obra = _obrasManager.Find(venta.IdObra);
            if (obra != null)
            {
                ViewBag.Equipo_Tipo =
                 new SelectList(_fallasManager.FindEquiposPorObra(obra.Id), "nombre", "nombre");
            }
            else
            {
                ViewBag.Equipo_Tipo =
                new SelectList(_fallasManager.FindEquipos(), "nombre", "nombre");
            }
          
            /*
            ViewBag.Equipo_Tipo =
                  new SelectList(_mantenimientoscorrectivosManager.FindTipos(), "id", "descripcion");
            */
            ViewBag.StatusMantenimiento =
             new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name");

            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSCORRECTIVOS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var mantenimientocorrectivo = _mantenimientoscorrectivosManager.Find(id);
            TempData["IDVENTA"] = mantenimientocorrectivo.IdVenta;
            TempData["mantenimientopreventivo"] = id;
            TempData.Keep();
            //if (mantenimientocorrectivo.Descripcion == null)
            //{
            //    mantenimientocorrectivo.Descripcion = "null";
            //}
            //if (mantenimientocorrectivo.Tipo == null)
            //{
            //    mantenimientocorrectivo.Tipo = "null";
            //}
            //if (mantenimientocorrectivo.NombreDocumento == null)
            //{
            //    mantenimientocorrectivo.NombreDocumento = "null";
            //}
            //if (mantenimientocorrectivo.Tecnico == null)
            //{
            //    mantenimientocorrectivo.Tecnico = "null";
            //}
            //if (mantenimientocorrectivo.EquiposAtendidos == null)
            //{
            //    mantenimientocorrectivo.EquiposAtendidos = "null";
            //}
            //if (mantenimientocorrectivo.StatusMantenimiento == null)
            //{
            //    mantenimientocorrectivo.StatusMantenimiento = "null";
            //}
            //if (mantenimientocorrectivo.Archivo == null)
            //{
            //    mantenimientocorrectivo.Archivo = "null";
            //}
            //if (mantenimientocorrectivo.Evaluacion == null)
            //{
            //    mantenimientocorrectivo.Evaluacion = "null";
            //}
            //if (mantenimientocorrectivo.HoraLlegada == null)
            //{
            //    mantenimientocorrectivo.HoraLlegada = "null";
            //}
            //if (mantenimientocorrectivo.HoraSalida == null)
            //{
            //    mantenimientocorrectivo.HoraSalida = "null";
            //}


            var venta = _ventasManager.Find(mantenimientocorrectivo.IdVenta);
            var obra = _obrasManager.Find(venta.IdObra);

            if (obra != null)
            {
                ViewBag.Equipo_Tipo =
                      new SelectList(_fallasManager.FindEquiposPorObra(obra.Id), "nombre", "nombre", mantenimientocorrectivo.Tipo);
            }
            else
            {
                ViewBag.Equipo_Tipo =
                  new SelectList(_fallasManager.FindEquipos(), "nombre", "nombre", mantenimientocorrectivo.Tipo);
            }

            ViewBag.StatusMantenimiento3 =
              new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name",mantenimientocorrectivo.StatusMantenimiento);

            ViewBag.Evaluaciones = _obrasManager.FindArchivos(id,"evaluacion", "mantenimientocorrectivo");
            ViewBag.Archivos = _obrasManager.FindArchivos(id, "archivo", "mantenimientocorrectivo");

            if (mantenimientocorrectivo == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var matenimientocorrectivoModel = new MantenimientoCorrectivoViewModel()
            {
               
                Tipo = mantenimientocorrectivo.Tipo,
                Descripcion = mantenimientocorrectivo.Descripcion,
                PersonaJuridica = mantenimientocorrectivo.PersonaJuridica,
                FechaVisita = mantenimientocorrectivo.FechaVisita.ToString(),
                FechaPublicacion = mantenimientocorrectivo.FechaPublicacion.ToString(),
                NombreDocumento = mantenimientocorrectivo.NombreDocumento,
                Tecnico = mantenimientocorrectivo.Tecnico,
                EquiposAtendidos = mantenimientocorrectivo.EquiposAtendidos,
                StatusMantenimiento = mantenimientocorrectivo.StatusMantenimiento,
                Archivo = mantenimientocorrectivo.Archivo,
                Evaluacion = mantenimientocorrectivo.Evaluacion,
                HoraLlegada = mantenimientocorrectivo.HoraLlegada,
                HoraSalida = mantenimientocorrectivo.HoraSalida,
                

            };

           
            return View(matenimientocorrectivoModel);
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSCORRECTIVOS-CONSULTAR")]
        public ActionResult Reporte(int id)
        {
            var mantenimientocorrectivo = _mantenimientoscorrectivosManager.Find(id);

            //if (mantenimientocorrectivo.Descripcion == null)
            //{
            //    mantenimientocorrectivo.Descripcion = "null";
            //}
            //if (mantenimientocorrectivo.Tipo == null)
            //{
            //    mantenimientocorrectivo.Tipo = "null";
            //}
            //if (mantenimientocorrectivo.NombreDocumento == null)
            //{
            //    mantenimientocorrectivo.NombreDocumento = "null";
            //}
            //if (mantenimientocorrectivo.Tecnico == null)
            //{
            //    mantenimientocorrectivo.Tecnico = "null";
            //}
            //if (mantenimientocorrectivo.EquiposAtendidos == null)
            //{
            //    mantenimientocorrectivo.EquiposAtendidos = "null";
            //}
            //if (mantenimientocorrectivo.StatusMantenimiento == null)
            //{
            //    mantenimientocorrectivo.StatusMantenimiento = "null";
            //}
            //if (mantenimientocorrectivo.Archivo == null)
            //{
            //    mantenimientocorrectivo.Archivo = "null";
            //}
            //if (mantenimientocorrectivo.Evaluacion == null)
            //{
            //    mantenimientocorrectivo.Evaluacion = "null";
            //}
            //if (mantenimientocorrectivo.HoraLlegada == null)
            //{
            //    mantenimientocorrectivo.HoraLlegada = "null";
            //}
            //if (mantenimientocorrectivo.HoraSalida == null)
            //{
            //    mantenimientocorrectivo.HoraSalida = "null";
            //}


            var venta = _ventasManager.Find(mantenimientocorrectivo.IdVenta);
            var obra = _obrasManager.Find(venta.IdObra);


            ViewBag.Equipo_Tipo =
                 new SelectList(_fallasManager.FindEquiposPorObra(obra.Id), "nombre", "nombre", mantenimientocorrectivo.Tipo);

            ViewBag.StatusMantenimiento3 =
              new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name", mantenimientocorrectivo.StatusMantenimiento);

            ViewBag.Evaluaciones = _obrasManager.FindArchivos(id, "evaluacion", "mantenimientocorrectivo");
            ViewBag.Archivos = _obrasManager.FindArchivos(id, "archivo", "mantenimientocorrectivo");

            if (mantenimientocorrectivo == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var matenimientocorrectivoModel = new MantenimientoCorrectivoViewModel()
            {

                Tipo = mantenimientocorrectivo.Tipo,
                Descripcion = mantenimientocorrectivo.Descripcion,
                PersonaJuridica = mantenimientocorrectivo.PersonaJuridica,
                FechaVisita = mantenimientocorrectivo.FechaVisita.ToString(),
                FechaPublicacion = mantenimientocorrectivo.FechaPublicacion.ToString(),
                NombreDocumento = mantenimientocorrectivo.NombreDocumento,
                Tecnico = mantenimientocorrectivo.Tecnico,
                EquiposAtendidos = mantenimientocorrectivo.EquiposAtendidos,
                StatusMantenimiento = mantenimientocorrectivo.StatusMantenimiento,
                Archivo = mantenimientocorrectivo.Archivo,
                Evaluacion = mantenimientocorrectivo.Evaluacion,
                HoraLlegada = mantenimientocorrectivo.HoraLlegada,
                HoraSalida = mantenimientocorrectivo.HoraSalida,


            };


            return PartialView(matenimientocorrectivoModel);
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSCORRECTIVOS-CONSULTAR")]
        public ActionResult Detalle(int id)
        {
            var mantenimientocorrectivo = _mantenimientoscorrectivosManager.Find(id);

            //if (mantenimientocorrectivo.Descripcion == null)
            //{
            //    mantenimientocorrectivo.Descripcion = "null";
            //}
            //if (mantenimientocorrectivo.Tipo == null)
            //{
            //    mantenimientocorrectivo.Tipo = "null";
            //}
            //if (mantenimientocorrectivo.NombreDocumento == null)
            //{
            //    mantenimientocorrectivo.NombreDocumento = "null";
            //}
            //if (mantenimientocorrectivo.Tecnico == null)
            //{
            //    mantenimientocorrectivo.Tecnico = "null";
            //}
            //if (mantenimientocorrectivo.EquiposAtendidos == null)
            //{
            //    mantenimientocorrectivo.EquiposAtendidos = "null";
            //}
            //if (mantenimientocorrectivo.StatusMantenimiento == null)
            //{
            //    mantenimientocorrectivo.StatusMantenimiento = "null";
            //}
            //if (mantenimientocorrectivo.Archivo == null)
            //{
            //    mantenimientocorrectivo.Archivo = "null";
            //}
            //if (mantenimientocorrectivo.Evaluacion == null)
            //{
            //    mantenimientocorrectivo.Evaluacion = "null";
            //}
            //if (mantenimientocorrectivo.HoraLlegada == null)
            //{
            //    mantenimientocorrectivo.HoraLlegada = "null";
            //}
            //if (mantenimientocorrectivo.HoraSalida == null)
            //{
            //    mantenimientocorrectivo.HoraSalida = "null";
            //}


            var venta = _ventasManager.Find(mantenimientocorrectivo.IdVenta);
            var obra = _obrasManager.Find(venta.IdObra);


            ViewBag.Equipo_Tipo =
                 new SelectList(_fallasManager.FindEquiposPorObra(obra.Id), "nombre", "nombre", mantenimientocorrectivo.Tipo);

            ViewBag.StatusMantenimiento3 =
              new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name", mantenimientocorrectivo.StatusMantenimiento);

            ViewBag.Evaluaciones = _obrasManager.FindArchivos(id, "evaluacion", "mantenimientocorrectivo");
            ViewBag.Archivos = _obrasManager.FindArchivos(id, "archivo", "mantenimientocorrectivo");

            if (mantenimientocorrectivo == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var matenimientocorrectivoModel = new MantenimientoCorrectivoViewModel()
            {

                Tipo = mantenimientocorrectivo.Tipo,
                Descripcion = mantenimientocorrectivo.Descripcion,
                PersonaJuridica = mantenimientocorrectivo.PersonaJuridica,
                FechaVisita = mantenimientocorrectivo.FechaVisita.ToString(),
                FechaPublicacion = mantenimientocorrectivo.FechaPublicacion.ToString(),
                NombreDocumento = mantenimientocorrectivo.NombreDocumento,
                Tecnico = mantenimientocorrectivo.Tecnico,
                EquiposAtendidos = mantenimientocorrectivo.EquiposAtendidos,
                StatusMantenimiento = mantenimientocorrectivo.StatusMantenimiento,
                Archivo = mantenimientocorrectivo.Archivo,
                Evaluacion = mantenimientocorrectivo.Evaluacion,
                HoraLlegada = mantenimientocorrectivo.HoraLlegada,
                HoraSalida = mantenimientocorrectivo.HoraSalida,


            };


            return View(matenimientocorrectivoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSCORRECTIVOS-MODIFICAR")]
        public ActionResult Editar(int id, MantenimientoCorrectivoViewModel model)
        {
            var mantenimientocorrectivo = _mantenimientoscorrectivosManager.Find(id);
            var pdfUrl = "";
            var pdfUrlevaluacion = "";
            int ventaid = Convert.ToInt32(TempData["IDVENTA"]);
            var venta = _ventasManager.Find(ventaid);
            var Url = "";


            var obra = _obrasManager.Find(venta.IdObra);
            if (obra != null)
            {
                ViewBag.Equipo_Tipo =
                      new SelectList(_fallasManager.FindEquiposPorObra(obra.Id), "nombre", "nombre", mantenimientocorrectivo.Tipo);
            }
            else
            {
                ViewBag.Equipo_Tipo =
                  new SelectList(_fallasManager.FindEquipos(), "nombre", "nombre", mantenimientocorrectivo.Tipo);
            }
          

            ViewBag.StatusMantenimiento =
            new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name", mantenimientocorrectivo.StatusMantenimiento);

            if (mantenimientocorrectivo == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {
                HttpPostedFileBase pdf = Request.Files["Pdf"];
                HttpPostedFileBase pdfevaluacion = Request.Files["Pdf-evaluacion"];

                if (pdf != null && pdf.ContentLength > 0)
                {
                    pdfUrl = CargarPdf(pdf);
                }
                else { pdfUrl = mantenimientocorrectivo.Archivo; }

                if (pdfevaluacion != null && pdfevaluacion.ContentLength > 0)
                {
                    pdfUrlevaluacion = CargarPdf(pdfevaluacion);
                }
                else { pdfUrlevaluacion = mantenimientocorrectivo.Evaluacion; }

                //if (model.Descripcion == null)
                //{ model.Descripcion = "null"; }
               
                //if (model.PersonaJuridica == null)
                //{
                //    model.PersonaJuridica = "null";
                //}
               
                //if (model.NombreDocumento == null)
                //{
                //    model.NombreDocumento = "null";
                //}
                //if (model.Tecnico == null)
                //{
                //    model.Tecnico = "null";
                //}
                //if (model.EquiposAtendidos == null)
                //{
                //    model.EquiposAtendidos = "null";
                //}
                //if (model.StatusMantenimiento == null)
                //{
                //    model.StatusMantenimiento = "NO ATENDIDO";
                //}
                //if (model.HoraLlegada == null)
                //{
                //    model.HoraLlegada = "null";
                //}
                //if (model.HoraSalida == null)
                //{
                //    model.HoraSalida = "null";
                //}
                //if (pdfUrl == null)
                //{
                //    pdfUrl = "null";
                //}
                //if (pdfUrlevaluacion == null)
                //{
                //    pdfUrlevaluacion = "null";
                //}
                _mantenimientoscorrectivosManager.Actualizar(Convert.ToInt32(TempData["IDVENTA"]),
                      id,
                      model.Tipo,
                      model.Descripcion,
                      model.PersonaJuridica,
                      DateTime.Parse(model.FechaVisita),
                      DateTime.Parse(model.FechaPublicacion),
                      model.NombreDocumento,
                      model.Tecnico,
                      model.EquiposAtendidos,
                      model.StatusMantenimiento,
                      pdfUrl.Trim(),
                      pdfUrlevaluacion.Trim(),
                      model.HoraLlegada,
                      model.HoraSalida);


                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf-evaluacion" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(id, Url, "mantenimientocorrectivo", "evaluacion");
                    }
                    if (d == "Pdf" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(id, Url, "mantenimientocorrectivo", "archivo");
                    }

                }

                TempData["FlashSuccess"] = MensajesResource.INFO_MantenimientosCorrectivos_ActualizadoCorrectamente;
                return RedirectToAction("Index", "AdministrarMantenimientosCorrectivos", new { @id = TempData["IDVENTA"] });
              //  return RedirectToAction("Index", "AdministrarMantenimientosPreventivos", new { @id = id });

                // TempData["FlashSuccess"] = MensajesResource.INFO_UsuarioNazan_ActualizadoCorrectamente;
                //return RedirectToAction("Index");
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



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSCORRECTIVOS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(MantenimientoCorrectivoViewModel model, FormCollection collection)
        {
           
            var pdfUrl = "";
            var pdfUrlevaluacion = "";
            var Url = "";

            //if (model.Descripcion == null)
            //{ model.Descripcion = "null"; }

            //if (model.PersonaJuridica == null)
            //{
            //    model.PersonaJuridica = "null";
            //}

            //if (model.NombreDocumento == null)
            //{
            //    model.NombreDocumento = "null";
            //}
            //if (model.Tecnico == null)
            //{
            //    model.Tecnico = "null";
            //}
            //if (model.EquiposAtendidos == null)
            //{
            //    model.EquiposAtendidos = "null";
            //}
            //if (model.StatusMantenimiento == null)
            //{
            //    model.StatusMantenimiento = "NO ATENDIDO";
            //}
            //if (model.HoraLlegada == null)
            //{
            //    model.HoraLlegada = "null";
            //}
            //if (model.HoraSalida == null)
            //{
            //    model.HoraSalida = "null";
            //}
            //if (pdfUrl == null)
            //{
            //    pdfUrl = "null";
            //}
            //if (pdfUrlevaluacion == null)
            //{
            //    pdfUrlevaluacion = "null";
            //}

            var venta = _ventasManager.Find(Convert.ToInt32(TempData["venta"]));
            TempData.Keep();
            var obra = _obrasManager.Find(venta.IdObra);

            if (obra != null)
            {
                ViewBag.Equipo_Tipo =
                new SelectList(_fallasManager.FindEquiposPorObra(obra.Id), "nombre", "nombre");
            }
            else
            {
                ViewBag.Equipo_Tipo =
                 new SelectList(_fallasManager.FindEquipos(), "nombre", "nombre");
            }
          

            ViewBag.StatusMantenimiento =
             new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name", 1);


            try
            {
                HttpPostedFileBase pdf = Request.Files["Pdf"];
                HttpPostedFileBase pdfevaluacion = Request.Files["Pdf-evaluacion"];

                if (pdf != null && pdf.ContentLength > 0)
                {
                    pdfUrl = CargarPdf(pdf);
                }
                else { pdfUrl = ""; }

                if (pdfevaluacion != null && pdfevaluacion.ContentLength > 0)
                {
                    pdfUrlevaluacion = CargarPdf(pdfevaluacion);
                }
                else { pdfUrlevaluacion = ""; }

                mantenimientocorrectivo mc = _mantenimientoscorrectivosManager.Crear(Convert.ToInt32(TempData["venta"]),
                       model.Tipo,
                      model.Descripcion,
                      model.PersonaJuridica,
                      DateTime.Parse(model.FechaVisita),
                      DateTime.Parse(model.FechaPublicacion),
                      model.NombreDocumento,
                      model.Tecnico,
                      model.EquiposAtendidos,
                      model.StatusMantenimiento,
                      pdfUrl.Trim(),
                      pdfUrlevaluacion.Trim(),
                      model.HoraLlegada,
                      model.HoraSalida);

                TempData.Keep();

                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf-evaluacion" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(mc.Id, Url, "mantenimientocorrectivo", "evaluacion");
                    }
                    if (d == "Pdf" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(mc.Id, Url, "mantenimientocorrectivo", "archivo");
                    }

                }

                TempData["FlashSuccess"] = MensajesResource.INFO_MantenimientosCorrectivos_CreadoCorrectamente;
                return RedirectToAction("Index", "AdministrarMantenimientosCorrectivos", new { @id = TempData["venta"] });
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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSCORRECTIVOS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _mantenimientoscorrectivosManager.Eliminar(id);
                TempData["FlashSuccess"] = MensajesResource.INFO_MantenimientosCorrectivos_EliminadoCorrectamente;
                return RedirectToAction("Index", "AdministrarMantenimientosCorrectivos", new { @id = TempData["IDVENTA"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Index", "AdministrarMantenimientosCorrectivos", new { @id = TempData["IDVENTA"] });

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
                return RedirectToAction("Index", "AdministrarMantenimientosCorrectivos", new { @id = TempData["IDVENTA"] });
            }

        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSCORRECTIVOS-MODIFICAR")]
        public ActionResult EliminarArchivo(string archivo, string tipo, string caracteristica, int id)
        {
            try
            {
                _obrasManager.EliminarArchivo(archivo, tipo, caracteristica);
                TempData["FlashSuccess"] = MensajesResource.INFO_Obras_ActualizadoCorrectamente;
                return RedirectToAction("Editar", "AdministrarMantenimientosCorrectivos", new { @id = TempData["mantenimientopreventivo"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Editar", "AdministrarMantenimientosCorrectivos", new { @id = TempData["mantenimientopreventivo"] });

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
                return RedirectToAction("Editar", "AdministrarMantenimientosCorrectivos", new { @id = TempData["mantenimientopreventivo"] });
            }

        }
    }
}