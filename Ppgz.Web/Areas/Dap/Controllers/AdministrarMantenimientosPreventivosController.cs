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
    public class AdministrarMantenimientosPreventivosController : Controller
    {
        private readonly FallasManager _fallasManager = new FallasManager();

        private readonly EquiposManager _equiposManager = new EquiposManager();

        private readonly MantenimientosPreventivosManager _mantenimientospreventivosManager = new MantenimientosPreventivosManager();

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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSPREVENTIVOS-CONSULTAR")]
        public ActionResult Index(int id)
        {
            var db = new EntitiesDap();
            TempData["venta_id"] = id;
            TempData.Keep();
            var fecha = DateTime.Today.AddMonths(-3);

            var mantenimiento = _mantenimientospreventivosManager.GetMantenimientoPreventivo(id);

            if(mantenimiento != null) { 
                var venta = _ventasManager.Find(mantenimiento.IdVenta);
                TempData["IDVENTA"] = venta.Id;
                TempData.Keep();
                TempData["venta"] = venta.Id;
                TempData.Keep();
                ViewBag.venta = venta.Id;

                ViewBag.obra = _obrasManager.Find(venta.IdObra);
                TempData["IDOBRA"] = venta.IdObra;
                TempData.Keep();

                if (id != 0)
                {
                    ViewBag.MantenimientosPreventivos = _mantenimientospreventivosManager.GetMantenimientosPreventivos(venta.Id);
                }
                else
                {
                    ViewBag.MantenimientosPreventivos = db.mantenimientopreventivo.ToList();
                };
            }
            else
            {

                ViewBag.venta = null;
                ViewBag.obra = null;

                
                ViewBag.MantenimientosPreventivos = null;
               
                // ViewBag.obra = _obrasManager.Find(venta.IdObra);
                //TempData["IDOBRA"] = venta.IdObra;
                // TempData.Keep();
            }


            ViewBag.StatusMantenimiento =
             new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name", 1);

           
            

            //ViewBag.EstatusCita = db.estatuscitas.ToList();

            return View();
        }

        public ActionResult MantenimientosPreventivos(int idventa)
        {
       
           // var mantenimiento = _mantenimientospreventivosManager.Find(id);
            var mantenimiento = _mantenimientospreventivosManager.GetMantenimientoPreventivo(idventa);
            var venta = _ventasManager.Find(mantenimiento.IdVenta);

            ViewBag.venta = venta.Id;
            TempData["venta"] = idventa;
            TempData.Keep();
            ViewBag.obra = _obrasManager.Find(venta.IdObra);

            var obra = _obrasManager.Find(venta.IdObra);

            ViewBag.Equipo_Tipo =
                  new SelectList(_fallasManager.FindEquiposPorObra(obra.Id), "nombre", "nombre");

            ViewBag.StatusMantenimiento =
             new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name", 1);

            ViewBag.Envios = _mantenimientospreventivosManager.GetMantenimientosPreventivos(idventa);
            return View("Index");
        }

        public ActionResult ReporteMantenimientosPreventivos(int idventa)
        {

            // var mantenimiento = _mantenimientospreventivosManager.Find(id);
            var mantenimiento = _mantenimientospreventivosManager.GetMantenimientoPreventivo(idventa);
            var venta = _ventasManager.Find(mantenimiento.IdVenta);

            ViewBag.venta = venta.Id;

            ViewBag.obra = _obrasManager.Find(venta.IdObra);

            var obra = _obrasManager.Find(venta.IdObra);

            ViewBag.Equipo_Tipo =
                  new SelectList(_fallasManager.FindEquiposPorObra(obra.Id), "nombre", "nombre");

            ViewBag.StatusMantenimiento =
             new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name", 1);

            ViewBag.Envios = _mantenimientospreventivosManager.GetMantenimientosPreventivos(idventa);
            return PartialView();
        }

        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-LISTAR,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-MODIFICAR")]
        //public ActionResult Index()
        //{
        //    ViewBag.mensajes = _mensajesInstitucionalesManager.FindAll();
        //    return View();
        //}
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSPREVENTIVOS-CREAR")]
        public ActionResult Crear()
        {

            // var mantenimientopreventivo = _mantenimientospreventivosManager.Find(id);
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

           

            ViewBag.StatusMantenimiento =
              new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name", 1);
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSPREVENTIVOS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var mantenimientopreventivo = _mantenimientospreventivosManager.Find(id);
            TempData["mantenimientopreventivo"] = id;
            TempData["IDVENTA"] = mantenimientopreventivo.IdVenta;
            TempData.Keep();
            //if (mantenimientopreventivo.Descripcion == null)
            //{
            //    mantenimientopreventivo.Descripcion = "null";
            //}
            //if (mantenimientopreventivo.Tipo == null)
            //{
            //    mantenimientopreventivo.Tipo = "null";
            //}
            //if (mantenimientopreventivo.NombreDocumento == null)
            //{
            //    mantenimientopreventivo.NombreDocumento = "null";
            //}
            //if (mantenimientopreventivo.Tecnico == null)
            //{
            //    mantenimientopreventivo.Tecnico = "null";
            //}
            //if (mantenimientopreventivo.EquiposAtendidos == null)
            //{
            //    mantenimientopreventivo.EquiposAtendidos = "null";
            //}
            //if (mantenimientopreventivo.StatusMantenimiento == null)
            //{
            //    mantenimientopreventivo.StatusMantenimiento = "null";
            //}
            //if (mantenimientopreventivo.Archivo == null)
            //{
            //    mantenimientopreventivo.Archivo = "null";
            //}
            //if (mantenimientopreventivo.Evaluacion == null)
            //{
            //    mantenimientopreventivo.Evaluacion = "null";
            //}
            //if (mantenimientopreventivo.HoraLlegada == null)
            //{
            //    mantenimientopreventivo.HoraLlegada = "null";
            //}
            //if (mantenimientopreventivo.HoraSalida == null)
            //{
            //    mantenimientopreventivo.HoraSalida = "null";
            //}

            //FechaVisita = mantenimientopreventivo.FechaVisita.ToString(),
            // FechaPublicacion = mantenimientopreventivo.FechaPublicacion.ToString(),


            var venta = _ventasManager.Find(mantenimientopreventivo.IdVenta);
            var obra = _obrasManager.Find(venta.IdObra);
            ViewBag.StatusMantenimiento2 =
             new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name", mantenimientopreventivo.StatusMantenimiento);
            /*
                ViewBag.Equipo_Tipo =
                     new SelectList(_mantenimientospreventivosManager.FindTipos(id), "Tipo", "Tipo", mantenimientopreventivo.Descripcion);
               *//*
               ViewBag.Equipo_Tipo =
                   new SelectList(_equiposManager.FindEquiposTipo(), "Descripcion", "Descripcion", mantenimientopreventivo.Tipo);
               */
                 /*
              ViewBag.Equipo_Tipo =
                     new SelectList(_equiposManager.FindEquiposTipo(), "Descripcion", "Descripcion", mantenimientopreventivo.Tipo);
              */
            if (obra != null)
            {
                ViewBag.Equipo_Tipo =
                      new SelectList(_fallasManager.FindEquiposPorObra(obra.Id), "nombre", "nombre", mantenimientopreventivo.Tipo);
            }
            else
            {
                ViewBag.Equipo_Tipo =
                  new SelectList(_fallasManager.FindEquipos(), "nombre", "nombre", mantenimientopreventivo.Tipo);
            }

           

            ViewBag.Evaluaciones = _obrasManager.FindArchivos(id, "evaluacion", "mantenimientopreventivo");
            ViewBag.Archivos = _obrasManager.FindArchivos(id, "archivo", "mantenimientopreventivo");

            if (mantenimientopreventivo == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var matenimientopreventivoModel = new MantenimientoPreventivoViewModel()
            {
                Tipo = mantenimientopreventivo.Tipo,
                Descripcion = mantenimientopreventivo.Descripcion,
                PersonaJuridica = mantenimientopreventivo.PersonaJuridica,
                FechaVisita = mantenimientopreventivo.FechaVisita.ToString(),
                FechaPublicacion = mantenimientopreventivo.FechaPublicacion.ToString(),
                NombreDocumento = mantenimientopreventivo.NombreDocumento,
                Tecnico = mantenimientopreventivo.Tecnico,
                EquiposAtendidos = mantenimientopreventivo.EquiposAtendidos,
                //StatusMantenimiento = mantenimientopreventivo.StatusMantenimiento,
                Archivo = mantenimientopreventivo.Archivo,
                Evaluacion = mantenimientopreventivo.Evaluacion,
                HoraLlegada = mantenimientopreventivo.HoraLlegada,
                HoraSalida = mantenimientopreventivo.HoraSalida,
                

            };

            return View(matenimientopreventivoModel);
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSPREVENTIVOS-CONSULTAR")]
        public ActionResult Reporte(int id)
        {
            var mantenimientopreventivo = _mantenimientospreventivosManager.Find(id);


            //if (mantenimientopreventivo.Descripcion == null)
            //{
            //    mantenimientopreventivo.Descripcion = "null";
            //}
            //if (mantenimientopreventivo.Tipo == null)
            //{
            //    mantenimientopreventivo.Tipo = "null";
            //}
            //if (mantenimientopreventivo.NombreDocumento == null)
            //{
            //    mantenimientopreventivo.NombreDocumento = "null";
            //}
            //if (mantenimientopreventivo.Tecnico == null)
            //{
            //    mantenimientopreventivo.Tecnico = "null";
            //}
            //if (mantenimientopreventivo.EquiposAtendidos == null)
            //{
            //    mantenimientopreventivo.EquiposAtendidos = "null";
            //}
            //if (mantenimientopreventivo.StatusMantenimiento == null)
            //{
            //    mantenimientopreventivo.StatusMantenimiento = "null";
            //}
            //if (mantenimientopreventivo.Archivo == null)
            //{
            //    mantenimientopreventivo.Archivo = "null";
            //}
            //if (mantenimientopreventivo.Evaluacion == null)
            //{
            //    mantenimientopreventivo.Evaluacion = "null";
            //}
            //if (mantenimientopreventivo.HoraLlegada == null)
            //{
            //    mantenimientopreventivo.HoraLlegada = "null";
            //}
            //if (mantenimientopreventivo.HoraSalida == null)
            //{
            //    mantenimientopreventivo.HoraSalida = "null";
            //}

            //FechaVisita = mantenimientopreventivo.FechaVisita.ToString(),
            // FechaPublicacion = mantenimientopreventivo.FechaPublicacion.ToString(),


            var venta = _ventasManager.Find(mantenimientopreventivo.IdVenta);
            var obra = _obrasManager.Find(venta.IdObra);
            ViewBag.StatusMantenimiento2 =
             new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name", mantenimientopreventivo.StatusMantenimiento);
            /*
                ViewBag.Equipo_Tipo =
                     new SelectList(_mantenimientospreventivosManager.FindTipos(id), "Tipo", "Tipo", mantenimientopreventivo.Descripcion);
               *//*
               ViewBag.Equipo_Tipo =
                   new SelectList(_equiposManager.FindEquiposTipo(), "Descripcion", "Descripcion", mantenimientopreventivo.Tipo);
               */
                 /*
              ViewBag.Equipo_Tipo =
                     new SelectList(_equiposManager.FindEquiposTipo(), "Descripcion", "Descripcion", mantenimientopreventivo.Tipo);
              */
            ViewBag.Equipo_Tipo =
                  new SelectList(_fallasManager.FindEquiposPorObra(obra.Id), "nombre", "nombre", mantenimientopreventivo.Tipo);

            ViewBag.Evaluaciones = _obrasManager.FindArchivos(id, "evaluacion", "mantenimientopreventivo");
            ViewBag.Archivos = _obrasManager.FindArchivos(id, "archivo", "mantenimientopreventivo");

            if (mantenimientopreventivo == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var matenimientopreventivoModel = new MantenimientoPreventivoViewModel()
            {
                Tipo = mantenimientopreventivo.Tipo,
                Descripcion = mantenimientopreventivo.Descripcion,
                PersonaJuridica = mantenimientopreventivo.PersonaJuridica,
                FechaVisita = mantenimientopreventivo.FechaVisita.ToString(),
                FechaPublicacion = mantenimientopreventivo.FechaPublicacion.ToString(),
                NombreDocumento = mantenimientopreventivo.NombreDocumento,
                Tecnico = mantenimientopreventivo.Tecnico,
                EquiposAtendidos = mantenimientopreventivo.EquiposAtendidos,
                //StatusMantenimiento = mantenimientopreventivo.StatusMantenimiento,
                Archivo = mantenimientopreventivo.Archivo,
                Evaluacion = mantenimientopreventivo.Evaluacion,
                HoraLlegada = mantenimientopreventivo.HoraLlegada,
                HoraSalida = mantenimientopreventivo.HoraSalida,


            };

            return PartialView(matenimientopreventivoModel);
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSPREVENTIVOS-CONSULTAR")]
        public ActionResult Detalle(int id)
        {
            var mantenimientopreventivo = _mantenimientospreventivosManager.Find(id);


            //if (mantenimientopreventivo.Descripcion == null)
            //{
            //    mantenimientopreventivo.Descripcion = "null";
            //}
            //if (mantenimientopreventivo.Tipo == null)
            //{
            //    mantenimientopreventivo.Tipo = "null";
            //}
            //if (mantenimientopreventivo.NombreDocumento == null)
            //{
            //    mantenimientopreventivo.NombreDocumento = "null";
            //}
            //if (mantenimientopreventivo.Tecnico == null)
            //{
            //    mantenimientopreventivo.Tecnico = "null";
            //}
            //if (mantenimientopreventivo.EquiposAtendidos == null)
            //{
            //    mantenimientopreventivo.EquiposAtendidos = "null";
            //}
            //if (mantenimientopreventivo.StatusMantenimiento == null)
            //{
            //    mantenimientopreventivo.StatusMantenimiento = "null";
            //}
            //if (mantenimientopreventivo.Archivo == null)
            //{
            //    mantenimientopreventivo.Archivo = "null";
            //}
            //if (mantenimientopreventivo.Evaluacion == null)
            //{
            //    mantenimientopreventivo.Evaluacion = "null";
            //}
            //if (mantenimientopreventivo.HoraLlegada == null)
            //{
            //    mantenimientopreventivo.HoraLlegada = "null";
            //}
            //if (mantenimientopreventivo.HoraSalida == null)
            //{
            //    mantenimientopreventivo.HoraSalida = "null";
            //}

            //FechaVisita = mantenimientopreventivo.FechaVisita.ToString(),
            // FechaPublicacion = mantenimientopreventivo.FechaPublicacion.ToString(),


            var venta = _ventasManager.Find(mantenimientopreventivo.IdVenta);
            var obra = _obrasManager.Find(venta.IdObra);
            ViewBag.StatusMantenimiento2 =
             new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name", mantenimientopreventivo.StatusMantenimiento);
           
            ViewBag.Equipo_Tipo =
                  new SelectList(_fallasManager.FindEquiposPorObra(obra.Id), "nombre", "nombre", mantenimientopreventivo.Tipo);

            ViewBag.Evaluaciones = _obrasManager.FindArchivos(id, "evaluacion", "mantenimientopreventivo");
            ViewBag.Archivos = _obrasManager.FindArchivos(id, "archivo", "mantenimientopreventivo");

            if (mantenimientopreventivo == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var matenimientopreventivoModel = new MantenimientoPreventivoViewModel()
            {
                Tipo = mantenimientopreventivo.Tipo,
                Descripcion = mantenimientopreventivo.Descripcion,
                PersonaJuridica = mantenimientopreventivo.PersonaJuridica,
                FechaVisita = mantenimientopreventivo.FechaVisita.ToString(),
                FechaPublicacion = mantenimientopreventivo.FechaPublicacion.ToString(),
                NombreDocumento = mantenimientopreventivo.NombreDocumento,
                Tecnico = mantenimientopreventivo.Tecnico,
                EquiposAtendidos = mantenimientopreventivo.EquiposAtendidos,
                //StatusMantenimiento = mantenimientopreventivo.StatusMantenimiento,
                Archivo = mantenimientopreventivo.Archivo,
                Evaluacion = mantenimientopreventivo.Evaluacion,
                HoraLlegada = mantenimientopreventivo.HoraLlegada,
                HoraSalida = mantenimientopreventivo.HoraSalida,


            };

            return View(matenimientopreventivoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSPREVENTIVOS-MODIFICAR")]
        public ActionResult Editar(int id, MantenimientoPreventivoViewModel model)
        {
            var mantenimientopreventivo = _mantenimientospreventivosManager.Find(id);
            var pdfUrl = "";
            var pdfUrlevaluacion = "";
            int ventaid = Convert.ToInt32(TempData["IDVENTA"]);
            var venta = _ventasManager.Find(ventaid);

            var obra = _obrasManager.Find(venta.IdObra);
            if(obra != null) { 
            ViewBag.Equipo_Tipo =
                  new SelectList(_fallasManager.FindEquiposPorObra(obra.Id), "nombre", "nombre", mantenimientopreventivo.Tipo);
            }else
            {
                ViewBag.Equipo_Tipo =
                  new SelectList(_fallasManager.FindEquipos(), "nombre", "nombre", mantenimientopreventivo.Tipo);
            }
            ViewBag.StatusMantenimiento =
             new SelectList(new[] { new { ID = "Culminado", Name = "Culminado" }, new { ID = "No culminado", Name = "No culminado" }, }, "ID", "Name", mantenimientopreventivo.StatusMantenimiento);
            var Url = "";
            if (mantenimientopreventivo == null)
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
                else { pdfUrl = mantenimientopreventivo.Archivo; }

                if (pdfevaluacion != null && pdfevaluacion.ContentLength > 0)
                {
                    pdfUrlevaluacion = CargarPdf(pdfevaluacion);
                }
                else { pdfUrlevaluacion = mantenimientopreventivo.Evaluacion; }

                //if(model.Descripcion == null)
                //{ model.Descripcion = "null"; }
                //if(model.FechaVisita == null)
                //{
                //    model.FechaVisita = "00/00/2018";
                //}
                //if (model.PersonaJuridica == null)
                //{
                //    model.PersonaJuridica = "null";
                //}
                //if(model.FechaPublicacion == null)
                //{
                //    model.FechaPublicacion = "00/00/2018";
                //}
                //if (model.NombreDocumento == null)
                //{
                //    model.NombreDocumento = "null";
                //}if(model.Tecnico == null)
                //{
                //    model.Tecnico = "null";
                //}if(model.EquiposAtendidos == null)
                //{
                //    model.EquiposAtendidos = "null";
                //}if(model.StatusMantenimiento == null)
                //{
                //    model.StatusMantenimiento = "NO ATENDIDO";
                //}
                //if (model.HoraLlegada == null)
                //{
                //    model.HoraLlegada = "null";
                //}if(model.HoraSalida == null)
                //{
                //    model.HoraSalida = "null";
                //}if(pdfUrl == null)
                //{
                //    pdfUrl = "null";
                //}if(pdfUrlevaluacion == null)
                //{
                //    pdfUrlevaluacion = "null";
                //}
                DateTime? fechavisita = null;
                DateTime? fechapublicacion = null;
                if(model.FechaVisita != null && model.FechaPublicacion == null) { 
                _mantenimientospreventivosManager.Actualizar(Convert.ToInt32(TempData["IDVENTA"]),
                      id,
                      model.Tipo,
                      model.Descripcion,
                      model.PersonaJuridica,
                      DateTime.Parse(model.FechaVisita),
                      fechapublicacion,
                      model.NombreDocumento,
                      model.Tecnico,
                      model.EquiposAtendidos,
                      model.StatusMantenimiento,
                      pdfUrl.Trim(),
                      pdfUrlevaluacion.Trim(),
                      model.HoraLlegada,
                      model.HoraSalida);
                }
                if (model.FechaVisita == null && model.FechaPublicacion != null)
                {
                    _mantenimientospreventivosManager.Actualizar(Convert.ToInt32(TempData["IDVENTA"]),
                          id,
                          model.Tipo,
                          model.Descripcion,
                          model.PersonaJuridica,
                          fechavisita,
                          DateTime.Parse(model.FechaPublicacion),
                          model.NombreDocumento,
                          model.Tecnico,
                          model.EquiposAtendidos,
                          model.StatusMantenimiento,
                          pdfUrl.Trim(),
                          pdfUrlevaluacion.Trim(),
                          model.HoraLlegada,
                          model.HoraSalida);
                }
                if (model.FechaVisita == null && model.FechaPublicacion == null)
                {
                    _mantenimientospreventivosManager.Actualizar(Convert.ToInt32(TempData["IDVENTA"]),
                          id,
                          model.Tipo,
                          model.Descripcion,
                          model.PersonaJuridica,
                          fechavisita,
                          fechapublicacion,
                          model.NombreDocumento,
                          model.Tecnico,
                          model.EquiposAtendidos,
                          model.StatusMantenimiento,
                          pdfUrl.Trim(),
                          pdfUrlevaluacion.Trim(),
                          model.HoraLlegada,
                          model.HoraSalida);
                }
                if (model.FechaVisita != null && model.FechaPublicacion != null)
                {
                    _mantenimientospreventivosManager.Actualizar(Convert.ToInt32(TempData["IDVENTA"]),
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
                }
                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf-evaluacion" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(id, Url, "mantenimientopreventivo", "evaluacion");
                    }
                    if (d == "Pdf" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(id, Url, "mantenimientopreventivo", "archivo");
                    }

                }

                TempData["FlashSuccess"] = MensajesResource.INFO_MantenimientosPreventivos_ActualizadoCorrectamente;

                
                return RedirectToAction("Index", "AdministrarMantenimientosPreventivos", new { @id = TempData["IDVENTA"] });
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



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSPREVENTIVOS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(MantenimientoPreventivoViewModel model, FormCollection collection)
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

            
            /*
            var idventa = TempData["IDVENTA"];
            TempData["venta"] = idventa;
            */
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

            // if (!ModelState.IsValid) return View(model);

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
                
                //mantenimientopreventivo mp = _mantenimientospreventivosManager.Crear(Convert.ToInt32(TempData["venta"]),
                //       model.Tipo,
                //      model.Descripcion,
                //      model.PersonaJuridica,
                //      model.FechaVisita,
                //      model.FechaPublicacion,
                //      model.NombreDocumento,
                //      model.Tecnico,
                //      model.EquiposAtendidos,
                //      model.StatusMantenimiento,
                //      pdfUrl.Trim(),
                //      pdfUrlevaluacion.Trim(),
                //      model.HoraLlegada,
                //      model.HoraSalida);


                DateTime? fechavisita = null;
                DateTime? fechapublicacion = null;
                mantenimientopreventivo mp = new mantenimientopreventivo();
                if (model.FechaVisita != null && model.FechaPublicacion == null)
                {
                    mp = _mantenimientospreventivosManager.Crear(Convert.ToInt32(TempData["venta"]),
                          model.Tipo,
                          model.Descripcion,
                          model.PersonaJuridica,
                          DateTime.Parse(model.FechaVisita),
                          fechapublicacion,
                          model.NombreDocumento,
                          model.Tecnico,
                          model.EquiposAtendidos,
                          model.StatusMantenimiento,
                          pdfUrl.Trim(),
                          pdfUrlevaluacion.Trim(),
                          model.HoraLlegada,
                          model.HoraSalida);
                }
                if (model.FechaVisita == null && model.FechaPublicacion != null)
                {
                    mp = _mantenimientospreventivosManager.Crear(Convert.ToInt32(TempData["venta"]),
                          model.Tipo,
                          model.Descripcion,
                          model.PersonaJuridica,
                          fechavisita,
                          DateTime.Parse(model.FechaPublicacion),
                          model.NombreDocumento,
                          model.Tecnico,
                          model.EquiposAtendidos,
                          model.StatusMantenimiento,
                          pdfUrl.Trim(),
                          pdfUrlevaluacion.Trim(),
                          model.HoraLlegada,
                          model.HoraSalida);
                }
                if (model.FechaVisita == null && model.FechaPublicacion == null)
                {
                    mp = _mantenimientospreventivosManager.Crear(Convert.ToInt32(TempData["venta"]),
                          model.Tipo,
                          model.Descripcion,
                          model.PersonaJuridica,
                          fechavisita,
                          fechapublicacion,
                          model.NombreDocumento,
                          model.Tecnico,
                          model.EquiposAtendidos,
                          model.StatusMantenimiento,
                          pdfUrl.Trim(),
                          pdfUrlevaluacion.Trim(),
                          model.HoraLlegada,
                          model.HoraSalida);
                }
                if (model.FechaVisita != null && model.FechaPublicacion != null)
                {
                     mp = _mantenimientospreventivosManager.Crear(Convert.ToInt32(TempData["venta"]),
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
                }

                TempData.Keep();

                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf-evaluacion" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(mp.Id, Url, "mantenimientopreventivo", "evaluacion");
                    }
                    if (d == "Pdf" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(mp.Id, Url, "mantenimientopreventivo", "archivo");
                    }

                }

                TempData["FlashSuccess"] = MensajesResource.INFO_MantenimientosPreventivos_CreadoCorrectamente;
                return RedirectToAction("Index", "AdministrarMantenimientosPreventivos", new { @id = TempData["venta"] });
               // return RedirectToAction("Index");
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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSPREVENTIVOS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _mantenimientospreventivosManager.Eliminar(id);
                TempData["FlashSuccess"] = MensajesResource.INFO_MantenimientosPreventivos_EliminadoCorrectamente;
                return RedirectToAction("Index", "AdministrarMantenimientosPreventivos", new { @id = TempData["venta"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Index", "AdministrarMantenimientosPreventivos", new { @id = TempData["venta"] });

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
                return RedirectToAction("Index", "AdministrarMantenimientosPreventivos", new { @id = TempData["venta"] });
            }

        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMANTENIMIENTOSPREVENTIVOS-MODIFICAR")]
        public ActionResult EliminarArchivo(string archivo, string tipo, string caracteristica, int id)
        {
            try
            {
                _obrasManager.EliminarArchivo(archivo, tipo, caracteristica);
                TempData["FlashSuccess"] = MensajesResource.INFO_Obras_ActualizadoCorrectamente;
                return RedirectToAction("Editar", "AdministrarMantenimientosPreventivos", new { @id = TempData["mantenimientopreventivo"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Editar", "AdministrarMantenimientosPreventivos", new { @id = TempData["mantenimientopreventivo"] });

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
                return RedirectToAction("Editar", "AdministrarMantenimientosPreventivos", new { @id = TempData["mantenimientopreventivo"] });
            }

        }
    }
}