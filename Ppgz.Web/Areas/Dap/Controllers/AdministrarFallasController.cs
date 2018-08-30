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
    public class AdministrarFallasController : Controller
    {
        private readonly FallasManager _fallasManager = new FallasManager();

        private readonly ObrasManager _obrasManager = new ObrasManager();


        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARFALLAS-CONSULTAR")]
        public ActionResult Index()
        {
            var db = new EntitiesDap();
            var fecha = DateTime.Today.AddMonths(-3);
            //ViewBag.Fallas = db.fallas.ToList();

            ViewBag.Fallas = _fallasManager.GetFallas();

            ViewBag.FallasCount = _fallasManager.GetSustituciones();
            TempData["sustituciones"] = ViewBag.FallasCount.Count;
            TempData.Keep();

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
           ViewBag.Fallas = _fallasManager.GetHistorico(id);
           ViewBag.id = id;
           return View("Index");
        }
       
        public ActionResult Sustituciones()
        {
            ViewBag.FallasCount = _fallasManager.GetSustituciones();
            ViewBag.Fallas = _fallasManager.GetSustituciones();
            TempData["sustituciones"] = ViewBag.FallasCount.Count;
            TempData.Keep();
            return View();
        }
        public ActionResult HistoricoTodos()
        {
           ViewBag.Fallas = _fallasManager.GetHistoricoAll();
           return View("Index");
        }

        public ActionResult Fallas(int id)
        {
            
            ViewBag.Fallas = _fallasManager.GetFallas(id);
            
            ViewBag.id = id;

            var obra = _obrasManager.Find(id);
            TempData["OBRA_ID"] = id;
            TempData.Keep();
            return View("Index");
        }
        public ActionResult ReporteFallas(int id)
        {

            ViewBag.Fallas = _fallasManager.GetFallas(id);

            ViewBag.id = id;

            var obra = _obrasManager.Find(id);
            TempData["OBRA_ID"] = id;
            TempData.Keep();
            return PartialView();
        }
        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-LISTAR,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-MODIFICAR")]
        //public ActionResult Index()
        //{
        //    ViewBag.mensajes = _mensajesInstitucionalesManager.FindAll();
        //    return View();
        //}


        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARFALLAS-MODIFICAR")]
        public ActionResult Editar(int id, string tipo)
        {

            TempData["fallaid"] = id;
            TempData.Keep();
            var falla = _fallasManager.Find(id);

           // var statusfalla = status;
            ViewBag.Obras =
               new SelectList(_fallasManager.FindObras(), "nombre", "nombre",falla.Obra);

            var fallaequiposobra = _fallasManager.FindEquiposPorObra(id);

            ViewBag.ArchivosCorreo = _obrasManager.FindCorreosFallas(id);


            if (fallaequiposobra.Count > 0) { 
            ViewBag.Equipos =
               new SelectList(fallaequiposobra, "nombre", "nombre",falla.Equipo);
            }else
            {
                ViewBag.Equipos =
               new SelectList(_fallasManager.FindEquiposTipo(), "Descripcion", "Descripcion");
            }

            if(tipo != null)
            {
                ViewBag.TipoFallas =
               new SelectList(_fallasManager.FindTipoFallas(), "descripcion", "descripcion", tipo);

                if (tipo == "Electrónica")
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentesElectronicos(), "descripcion", "descripcion", falla.Componente);
                }
                if (tipo == "Mecánica")
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentesMecanicos(), "descripcion", "descripcion", falla.Componente);
                }
                else
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentes(), "descripcion", "descripcion", falla.Componente);
                }
            }
            else
            {
                ViewBag.TipoFallas =
               new SelectList(_fallasManager.FindTipoFallas(), "descripcion", "descripcion", falla.Tipo);

                if (falla.Tipo == "Electrónica")
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentesElectronicos(), "descripcion", "descripcion", falla.Componente);
                }
                if (falla.Tipo == "Mecánica")
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentesMecanicos(), "descripcion", "descripcion", falla.Componente);
                }
                else
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentes(), "descripcion", "descripcion", falla.Componente);
                }
            }
            
            

            
            /*

             ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentesMecanicos(), "descripcion", "descripcion",falla.Componente);
            */
            ViewBag.Status =
               new SelectList(_fallasManager.FindStatus(), "descripcion", "descripcion",falla.StatusFalla);
            
            ViewBag.Condicion2 =
               new SelectList(new[] { new { ID = "OBRA", Name = "OBRA" }, new { ID = "EQUIPO", Name = "EQUIPO" }, }, "ID", "Name",falla.Condicion);
            /*
            ViewBag.Condicion =
               new SelectList(_fallasManager.FindCondicion(), "descripcion", "descripcion", falla.Condicion);
            */
            if (falla == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var fallaModel = new FallaViewModel()
            {
                id = id,
                FechaFalla = falla.FechaFalla.ToString("dd/MM/YYYY"),
                //Obra = falla.Obra.ToList().,
                Equipo = falla.Equipo,
                Tipo = falla.Tipo,
                Componente = falla.Componente,
                Personal = falla.Personal,
                StatusFalla = falla.StatusFalla,
                FechaSolucion = falla.FechaSolucion.ToString(),
                NumeroReporte = falla.NumeroReporte,
                Descripcion = falla.Descripcion,
                //Condicion = falla.Condicion,
                AccionesTomadas = falla.AccionesTomadas,
                AccionesRecomendadas = falla.AccionesRecomendadas,
                Duracion = falla.Duracion.ToString(),
                PersonaReporte = falla.PersonaReporte,
                GerenciaResponsable = falla.GerenciaResponsable,
                obraid = falla.obra_id
               // Fallas = falla.Fallas
                
            };

            return View(fallaModel);
        }


        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARFALLAS-CONSULTAR")]
        public ActionResult Detalle(int id, string status)
        {


            var falla = _fallasManager.Find(id);

            var statusfalla = status;
            ViewBag.Obras =
               new SelectList(_fallasManager.FindObras(), "nombre", "nombre", falla.Obra);

            var fallaequiposobra = _fallasManager.FindEquiposPorObra(falla.obra_id);

            ViewBag.ArchivosCorreo = _obrasManager.FindCorreosFallas(id);


          //  if (fallaequiposobra.Count > 0)
         //   {
                ViewBag.Equipos =
                   new SelectList(fallaequiposobra, "nombre", "nombre", falla.Equipo);
          //  }
           // else
          //  {
                ViewBag.Equipos =
               new SelectList(_fallasManager.FindEquiposTipo(), "Descripcion", "Descripcion", falla.Equipo);
           // }

            // ViewBag.TipoFallas =
            //   new SelectList(_fallasManager.FindTipoFallas(), "descripcion", "descripcion", falla.Tipo);


            
                ViewBag.TipoFallas =
               new SelectList(_fallasManager.FindTipoFallas(), "descripcion", "descripcion", falla.Tipo);

                if (falla.Tipo == "Electrónica")
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentesElectronicos(), "descripcion", "descripcion", falla.Componente);
                }
                if (falla.Tipo == "Mecánica")
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentesMecanicos(), "descripcion", "descripcion", falla.Componente);
                }
                else
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentes(), "descripcion", "descripcion", falla.Componente);
                }
           
            /*

             ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentesMecanicos(), "descripcion", "descripcion",falla.Componente);
            */
            ViewBag.Status =
               new SelectList(_fallasManager.FindStatus(), "descripcion", "descripcion", falla.StatusFalla);

            ViewBag.Condicion =
               new SelectList(new[] { new { ID = "OBRA", Name = "OBRA" }, new { ID = "EQUIPO", Name = "EQUIPO" }, }, "ID", "Name", falla.Condicion);

            if (falla == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var fallaModel = new FallaViewModel()
            {
                id = id,
                FechaFalla = falla.FechaFalla.ToString("dd/MM/YYYY"),
                //Obra = falla.Obra.ToList().,
                Equipo = falla.Equipo,
                Tipo = falla.Tipo,
                Componente = falla.Componente,
                Personal = falla.Personal,
                StatusFalla = falla.StatusFalla,
                FechaSolucion = falla.FechaSolucion.ToString(),
                NumeroReporte = falla.NumeroReporte,
                Descripcion = falla.Descripcion,
                Condicion = falla.Condicion,
                AccionesTomadas = falla.AccionesTomadas,
                AccionesRecomendadas = falla.AccionesRecomendadas,
                Duracion = falla.Duracion.ToString(),
                PersonaReporte = falla.PersonaReporte,
                GerenciaResponsable = falla.GerenciaResponsable,
                obraid = falla.obra_id
                // Fallas = falla.Fallas

            };

            return View(fallaModel);
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARFALLAS-CONSULTAR")]
        public ActionResult Reporte(int id, string status)
        {


            var falla = _fallasManager.Find(id);

            var statusfalla = status;
            ViewBag.Obras =
               new SelectList(_fallasManager.FindObras(), "nombre", "nombre", falla.Obra);

            var fallaequiposobra = _fallasManager.FindEquiposPorObra(id);

            ViewBag.ArchivosCorreo = _obrasManager.FindCorreosFallas(id);


            if (fallaequiposobra != null)
            {
                ViewBag.Equipos =
                   new SelectList(fallaequiposobra, "nombre", "nombre", falla.Equipo);
            }
            else
            {
                ViewBag.Equipos =
               new SelectList(_fallasManager.FindEquipos(), "nombre", "nombre");
            }

            ViewBag.TipoFallas =
               new SelectList(_fallasManager.FindTipoFallas(), "descripcion", "descripcion", falla.Tipo);


            if (falla.Tipo == "Electrónica")
            {
                ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentesElectronicos(), "descripcion", "descripcion", falla.Componente);
            }
            if (falla.Tipo == "Mecánica")
            {
                ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentesMecanicos(), "descripcion", "descripcion", falla.Componente);
            }
            else
            {
                ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentes(), "descripcion", "descripcion", falla.Componente);
            }
            /*

             ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentesMecanicos(), "descripcion", "descripcion",falla.Componente);
            */
            ViewBag.Status =
               new SelectList(_fallasManager.FindStatus(), "descripcion", "descripcion", falla.StatusFalla);

            ViewBag.Condicion =
               new SelectList(new[] { new { ID = "OBRA", Name = "OBRA" }, new { ID = "EQUIPO", Name = "EQUIPO" }, }, "ID", "Name", falla.Condicion);

            if (falla == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var fallaModel = new FallaViewModel()
            {
                id = id,
                FechaFalla = falla.FechaFalla.ToString("dd/MM/yyyy"),
                //Obra = falla.Obra.ToList().,
                Equipo = falla.Equipo,
                Tipo = falla.Tipo,
                Componente = falla.Componente,
                Personal = falla.Personal,
                StatusFalla = falla.StatusFalla,
                FechaSolucion = falla.FechaSolucion.ToString(),
                NumeroReporte = falla.NumeroReporte,
                Descripcion = falla.Descripcion,
                Condicion = falla.Condicion,
                AccionesTomadas = falla.AccionesTomadas,
                AccionesRecomendadas = falla.AccionesRecomendadas,
                Duracion = falla.Duracion.ToString(),
                PersonaReporte = falla.PersonaReporte,
                GerenciaResponsable = falla.GerenciaResponsable,
                obraid = falla.obra_id
                // Fallas = falla.Fallas

            };

            return PartialView(fallaModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARFALLAS-MODIFICAR")]
        public ActionResult Editar(int id, FallaViewModel model)
        {
            var Url = "";
            var falla = _fallasManager.Find(id);

            var obra = _obrasManager.Find_by_name(falla.Obra);

            ViewBag.Obras =
              new SelectList(_fallasManager.FindObras(), "nombre", "nombre", falla.Obra);


            ViewBag.Equipos =
               new SelectList(_fallasManager.FindEquiposPorObra(id), "nombre", "nombre", falla.Equipo);

            ViewBag.ArchivosCorreo = _obrasManager.FindCorreosFallas(id);

            ViewBag.TipoFallas =
               new SelectList(_fallasManager.FindTipoFallas(), "descripcion", "descripcion", falla.Tipo);

            if(model.Tipo == "Electrónica")
            {
                ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentesElectronicos(), "descripcion", "descripcion", falla.Componente);
            }
            if(model.Tipo == "Mecánica")
            {
                ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentesMecanicos(), "descripcion", "descripcion", falla.Componente);
            }
            else{
                ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentes(), "descripcion", "descripcion", falla.Componente);
            }

            ViewBag.Condicion =
               new SelectList(new[] { new { ID = "OBRA", Name = "OBRA" }, new { ID = "EQUIPO", Name = "EQUIPO" }, }, "ID", "Name", falla.Condicion);

            /*
            ViewBag.Componentes =
               new SelectList(_fallasManager.FindComponentesMecanicos(), "descripcion", "descripcion", falla.Componente);
            */
            ViewBag.Status =
               new SelectList(_fallasManager.FindStatus(), "descripcion", "descripcion", falla.StatusFalla);

           
            if (falla == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            
            try
            {
                DateTime? fallasolucion = null;
                DateTime? duracion = null;

                if (model.FechaSolucion == null)
                {
                    if (falla.Obra == null)
                    {
                        _fallasManager.Actualizar(
                              id,
                               DateTime.Parse(model.FechaFalla),
                               fallasolucion,
                              model.Obra,
                              model.Equipo,
                              model.Tipo,
                              model.Componente,
                              model.Personal,
                              model.StatusFalla,
                              model.NumeroReporte,
                              model.Descripcion,
                              model.Condicion,
                              model.AccionesTomadas,
                              model.AccionesRecomendadas,
                              duracion,
                              model.PersonaReporte,
                              model.GerenciaResponsable
                              );
                    }
                    else
                    {
                        _fallasManager.Actualizar(
                         id,
                          DateTime.Parse(model.FechaFalla),
                          fallasolucion,
                         falla.Obra,
                         model.Equipo,
                         model.Tipo,
                         model.Componente,
                         model.Personal,
                         model.StatusFalla,
                         model.NumeroReporte,
                         model.Descripcion,
                         model.Condicion,
                         model.AccionesTomadas,
                         model.AccionesRecomendadas,
                         duracion,
                         model.PersonaReporte,
                         model.GerenciaResponsable
                         );
                    }
                }
                else
                {
                    if (falla.Obra == null)
                    {
                        _fallasManager.Actualizar(
                              id,
                               DateTime.Parse(model.FechaFalla),
                                  DateTime.Parse(model.FechaSolucion),
                              model.Obra,
                              model.Equipo,
                              model.Tipo,
                              model.Componente,
                              model.Personal,
                              model.StatusFalla,
                              model.NumeroReporte,
                              model.Descripcion,
                              model.Condicion,
                              model.AccionesTomadas,
                              model.AccionesRecomendadas,
                              duracion,
                              model.PersonaReporte,
                              model.GerenciaResponsable
                              );
                    }
                    else
                    {
                        _fallasManager.Actualizar(
                         id,
                          DateTime.Parse(model.FechaFalla),
                              DateTime.Parse(model.FechaSolucion),
                         falla.Obra,
                         model.Equipo,
                         model.Tipo,
                         model.Componente,
                         model.Personal,
                         model.StatusFalla,
                         model.NumeroReporte,
                         model.Descripcion,
                         model.Condicion,
                         model.AccionesTomadas,
                         model.AccionesRecomendadas,
                         duracion,
                         model.PersonaReporte,
                         model.GerenciaResponsable
                         );
                    }
                }
                  
                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Correo" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(falla.Id, Url, "fallas", "correo");
                    }


                }

                TempData["FlashSuccess"] = MensajesResource.INFO_Fallas_ActualizadoCorrectamente;
               // return RedirectToAction("Editar", "AdministrarFallas", new { @Id = id } );

                if (obra != null)
                {
                    return RedirectToAction("Fallas", "AdministrarFallas", new { @id = obra.Id });
                }
                else
                {
                    return RedirectToAction("Index", "AdministrarFallas");
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


        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARFALLAS-CREAR")]
        public ActionResult Crear(int id,string tipo,string defecto)
        {
           

           var db = new EntitiesDap();
           
                TempData["OBRA_ID"] = id;
            TempData.Keep();
               var obra = _obrasManager.Find(id);

                ViewBag.Obras =
             new SelectList(_fallasManager.FindObras(), "nombre", "nombre", obra.Nombre);
           
            
            TempData.Keep();



            ViewBag.Equipos =
               new SelectList(_fallasManager.FindEquiposPorObra(id), "nombre", "nombre");


            ViewBag.TipoFallas =
               new SelectList(db.fallas_tipo, "descripcion", "descripcion");

            // var tipo = this.Response.Headers["ddltipofalla"].ToString();

            ViewBag.Componentes =
                   new SelectList(_fallasManager.FindComponentes(), "descripcion", "descripcion");
            
            if (tipo != null)
            {

                if (tipo.ToString() == "Electrónica")
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentesElectronicos(), "descripcion", "descripcion");
                }
                else if (tipo.ToString() == "Mecánica")
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentesMecanicos(), "descripcion", "descripcion");
                }
                else
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentes(), "descripcion", "descripcion");
                }
            }
            else
            {
                ViewBag.Componentes =
                   new SelectList(_fallasManager.FindComponentes(), "descripcion", "descripcion");
            }
            /*
            ViewBag.Componentes =
                  new SelectList(_fallasManager.FindComponentesElectronicos(), "descripcion", "descripcion");
            */
            ViewBag.Status =
               new SelectList(db.fallas_status, "descripcion", "descripcion");

            ViewBag.Condicion =
               new SelectList(new[]{new {ID="OBRA",Name="OBRA"},new{ID= "EQUIPO", Name= "EQUIPO" },},"ID", "Name");

            
                return View();
           
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARFALLAS-CREAR")]
        public ActionResult CrearPorDefecto(string tipo, string obra)
        {

            TempData["tipo"] = tipo;
            TempData.Keep();
            var db = new EntitiesDap();

            var obraactual = _obrasManager.FindObrasPorNombre(obra);

            if(obraactual != null) { 
                TempData["obraidactual"] = obraactual.Id;
                TempData["obranombreactual"] = obraactual.Nombre;
            }
            TempData.Keep();

            if (TempData["obraidactual"] != null)
            {
                
                ViewBag.Obras =
                new SelectList(_fallasManager.FindObras(), "nombre", "nombre", TempData["obranombreactual"]);

                ViewBag.Equipos =
               new SelectList(_fallasManager.FindEquiposPorObra(Convert.ToInt32(TempData["obraidactual"])), "nombre", "nombre", Convert.ToInt32(TempData["obraidactual"]));

                ViewBag.TipoFallas =
                   new SelectList(db.fallas_tipo, "descripcion", "descripcion", TempData["tipo"]);
               
            }
            else
            {
                ViewBag.Obras =
               new SelectList(_fallasManager.FindObras(), "nombre", "nombre");

                ViewBag.Equipos =
               new SelectList(_fallasManager.FindEquiposPorObra(0), "nombre", "nombre");


                ViewBag.TipoFallas =
                   new SelectList(db.fallas_tipo, "descripcion", "descripcion");
            }
            // var tipo = this.Response.Headers["ddltipofalla"].ToString();

            if (tipo != null)
            {
                
                if (tipo.ToString() == "Electrónica")
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentesElectronicos(), "descripcion", "descripcion");
                   
                }
                else if (tipo.ToString() == "Mecánica")
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentesMecanicos(), "descripcion", "descripcion");
                }
                else
                {
                    ViewBag.Componentes =
                    new SelectList(_fallasManager.FindComponentes(), "descripcion", "descripcion");
                }
            }
            else
            {
                ViewBag.Componentes =
                   new SelectList(_fallasManager.FindComponentes(), "descripcion", "descripcion");
            }

            /*
            ViewBag.Componentes =
                  new SelectList(_fallasManager.FindComponentesElectronicos(), "descripcion", "descripcion");
            */
            ViewBag.Status =
               new SelectList(db.fallas_status, "descripcion", "descripcion");

            ViewBag.Condicion =
               new SelectList(new[] { new { ID = "OBRA", Name = "OBRA" }, new { ID = "EQUIPO", Name = "EQUIPO" }, }, "ID", "Name");

            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARFALLAS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearPorDefecto(FallaViewModel model, FormCollection collection)
        {
            var obra_id = _fallasManager.FindIdObra(model.Obra);
            var Url = "";
            var fechafalla = "";
            var fechasolucion = "";
            var obranombre = _obrasManager.Find(Convert.ToInt32(TempData["obraidactual"]));

            ViewBag.Obras =
              new SelectList(_fallasManager.FindObras(), "nombre", "nombre");


            ViewBag.Equipos =
               new SelectList(_fallasManager.FindEquiposPorObra(Convert.ToInt32(TempData["OBRA_ID"])), "nombre", "nombre");

            ViewBag.TipoFallas =
               new SelectList(_fallasManager.FindTipoFallas(), "descripcion", "descripcion");

            if (model.Tipo == "Electrónica")
            {
                ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentesElectronicos(), "descripcion", "descripcion");
            }
            if (model.Tipo == "Mecánica")
            {
                ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentesMecanicos(), "descripcion", "descripcion");
            }
            else
            {
                ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentes(), "descripcion", "descripcion");
            }

            ViewBag.Status =
               new SelectList(_fallasManager.FindStatus(), "descripcion", "descripcion");

            ViewBag.Condicion =
               new SelectList(new[] { new { ID = "OBRA", Name = "OBRA" }, new { ID = "EQUIPO", Name = "EQUIPO" }, }, "ID", "Name", model.Condicion);
            /*  var db = new EntitiesDap();
              model.ObraList = new SelectList(db.obras, "id", "nombre", "nombre");
              model.EquipoList = new SelectList(db.equipos, "id", "nombre");
              model.FallaList = new SelectList(db.fallas_tipo, "id", "descripcion");
              model.ComponenteElectronicoList = new SelectList(db.componenteselectronicos_tipos, "id", "descripcion");
              model.StatusList = new SelectList(db.fallas_status, "id", "descripcion");*/

            //  if (!ModelState.IsValid) return View(model);
           
           


            try
            {
                DateTime? fallasolucion = null;
                DateTime? duracion = null;
                fallas falla = new fallas();
                if (model.FechaSolucion == null)
                {
                    falla = _fallasManager.Crear(Convert.ToInt32(TempData["obraidactual"]),
                         DateTime.Parse(model.FechaFalla),
                        fallasolucion,
                        obranombre.Nombre,
                        model.Equipo,
                        model.Tipo,
                        model.Componente,
                        model.Personal,
                        model.StatusFalla,
                        model.NumeroReporte,
                        model.Descripcion,
                        model.Condicion,
                        model.AccionesTomadas,
                        model.AccionesRecomendadas,
                        duracion,
                        model.PersonaReporte,
                        model.GerenciaResponsable);
                }
                else
                {
                    falla = _fallasManager.Crear(Convert.ToInt32(TempData["obraidactual"]),
                         DateTime.Parse(model.FechaFalla),
                          DateTime.Parse(model.FechaSolucion),
                        obranombre.Nombre,
                        model.Equipo,
                        model.Tipo,
                        model.Componente,
                        model.Personal,
                        model.StatusFalla,
                        model.NumeroReporte,
                        model.Descripcion,
                        model.Condicion,
                        model.AccionesTomadas,
                        model.AccionesRecomendadas,
                        duracion,
                        model.PersonaReporte,
                        model.GerenciaResponsable);
                }
                //if (TempData["OBRA_ID"] != null)
                //{



                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Correo" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(falla.Id, Url, "fallas", "correo");
                    }


                }

                TempData["FlashSuccess"] = MensajesResource.INFO_Fallas_CreadoCorrectamente;
                if (TempData["OBRA_ID"] != null)
                {
                    return RedirectToAction("Fallas", "AdministrarFallas", new { @id = Convert.ToInt32(TempData["OBRA_ID"]) });
                }
                else
                {
                    return RedirectToAction("Index", "AdministrarFallas");
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

                // CommonManager.WriteAppLog(log, TipoMensaje.Error);

                //  ModelState.AddModelError(string.Empty, e.Message);
                return View(model);
            }
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARFALLAS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(FallaViewModel model, FormCollection collection)
        {
            var obra_id = _fallasManager.FindIdObra(model.Obra);
            var Url = "";

            var obranombre = _obrasManager.Find(Convert.ToInt32(TempData["OBRA_ID"]));
            TempData.Keep();
            ViewBag.Obras =
              new SelectList(_fallasManager.FindObras(), "nombre", "nombre");


            ViewBag.Equipos =
               new SelectList(_fallasManager.FindEquiposPorObra(Convert.ToInt32(TempData["OBRA_ID"])), "nombre", "nombre");
            TempData.Keep();
            ViewBag.TipoFallas =
               new SelectList(_fallasManager.FindTipoFallas(), "descripcion", "descripcion");

            if (model.Tipo == "Electrónica")
            {
                ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentesElectronicos(), "descripcion", "descripcion");
            }
            if (model.Tipo == "Mecánica")
            {
                ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentesMecanicos(), "descripcion", "descripcion");
            }
            else
            {
                ViewBag.Componentes =
                new SelectList(_fallasManager.FindComponentes(), "descripcion", "descripcion");
            }

            ViewBag.Status =
               new SelectList(_fallasManager.FindStatus(), "descripcion", "descripcion");

            ViewBag.Condicion =
               new SelectList(new[] { new { ID = "OBRA", Name = "OBRA" }, new { ID = "EQUIPO", Name = "EQUIPO" }, }, "ID", "Name", model.Condicion);
            /*  var db = new EntitiesDap();
              model.ObraList = new SelectList(db.obras, "id", "nombre", "nombre");
              model.EquipoList = new SelectList(db.equipos, "id", "nombre");
              model.FallaList = new SelectList(db.fallas_tipo, "id", "descripcion");
              model.ComponenteElectronicoList = new SelectList(db.componenteselectronicos_tipos, "id", "descripcion");
              model.StatusList = new SelectList(db.fallas_status, "id", "descripcion");*/

            //  if (!ModelState.IsValid) return View(model);
            fallas falla = new fallas();


            try
            {
                DateTime? fallasolucion = null;
                DateTime? duracion = null;

                if (model.FechaSolucion == null)
                {
                    falla = _fallasManager.Crear(Convert.ToInt32(TempData["OBRA_ID"]),
                         DateTime.Parse(model.FechaFalla),
                        fallasolucion,
                        obranombre.Nombre,
                        model.Equipo,
                        model.Tipo,
                        model.Componente,
                        model.Personal,
                        model.StatusFalla,
                        model.NumeroReporte,
                        model.Descripcion,
                        model.Condicion,
                        model.AccionesTomadas,
                        model.AccionesRecomendadas,
                        duracion,
                        model.PersonaReporte,
                        model.GerenciaResponsable);
                    TempData.Keep();
                }
                else
                {
                    falla = _fallasManager.Crear(Convert.ToInt32(TempData["OBRA_ID"]),
                         DateTime.Parse(model.FechaFalla),
                          DateTime.Parse(model.FechaSolucion),
                        obranombre.Nombre,
                        model.Equipo,
                        model.Tipo,
                        model.Componente,
                        model.Personal,
                        model.StatusFalla,
                        model.NumeroReporte,
                        model.Descripcion,
                        model.Condicion,
                        model.AccionesTomadas,
                        model.AccionesRecomendadas,
                        duracion,
                        model.PersonaReporte,
                        model.GerenciaResponsable);
                    TempData.Keep();
                }

                HttpPostedFileBase file;

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Correo" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(falla.Id, Url, "fallas", "correo");
                    }
                   

                }

                TempData["FlashSuccess"] = MensajesResource.INFO_Fallas_CreadoCorrectamente;
                if(TempData["OBRA_ID"] != null) { 
                    return RedirectToAction("Fallas", "AdministrarFallas", new { @id = Convert.ToInt32(TempData["OBRA_ID"]) } );
                }else
                {
                    return RedirectToAction("Index", "AdministrarFallas");
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

               // CommonManager.WriteAppLog(log, TipoMensaje.Error);

              //  ModelState.AddModelError(string.Empty, e.Message);
                return View(model);
            }
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARFALLAS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _fallasManager.Eliminar(id);
                TempData["FlashSuccess"] = MensajesResource.INFO_Fallas_EliminadoCorrectamente;
                if(TempData["OBRA_ID"] != null)
                {
                    return RedirectToAction("Fallas", "AdministrarFallas", new { @id = TempData["OBRA_ID"] });
                }else{
                    return RedirectToAction("Index", "AdministrarFallas");
                }

            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                if (TempData["OBRA_ID"] != null)
                {
                    return RedirectToAction("Fallas", "AdministrarFallas", new { @id = TempData["OBRA_ID"] });
                }
                else
                {
                    return RedirectToAction("Index", "AdministrarFallas");
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
                if (TempData["OBRA_ID"] != null)
                {
                    return RedirectToAction("Fallas", "AdministrarFallas", new { @id = TempData["OBRA_ID"] });
                }
                else
                {
                    return RedirectToAction("Index", "AdministrarFallas");
                }
            }

        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARFALLAS-MODIFICAR")]
        public ActionResult EliminarArchivo(string archivo, string tipo, string caracteristica, int id)
        {
            try
            {
                _obrasManager.EliminarArchivo(archivo, tipo, caracteristica);
                TempData["FlashSuccess"] = MensajesResource.INFO_Obras_ActualizadoCorrectamente;
                return RedirectToAction("Editar", "AdministrarFallas", new { @id = TempData["fallaid"] });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Editar", "AdministrarFallas", new { @id = TempData["fallaid"] });

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
                return RedirectToAction("Editar", "AdministrarFallas", new { @id = TempData["fallaid"] });
            }

        }

    }
}