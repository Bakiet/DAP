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
    public class AdministrarObrasController : Controller
    {
        private readonly ObrasManager _obrasManager = new ObrasManager();
        private readonly FallasManager _fallasManager = new FallasManager();
        /*
        public ActionResult EditarContacto(int id, ObraContactoViewModel model)
        {
            var obracontacto = _obrasManager.FindContacto(id);

            try
            {

                _obrasManager.ActualizarContacto(
                      id,
                      model.Nombre,
                      model.Email,
                      model.Telefono);

                TempData["FlashSuccess"] = MensajesResource.INFO_Obras_ActualizadoCorrectamente;
                return RedirectToAction("Index");
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

        }*/

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAROBRAS-CREAR")]
        [HttpPost]
        public ActionResult CrearContacto(int id, FormCollection collection)
        {
            string nombre = Convert.ToString(collection["txtaddnombre"]);
            string email = Convert.ToString(collection["txtaddemail"]);
            string telefono = Convert.ToString(collection["txtaddtelefono"]);
            int obra_id = id;
            try
            {

                _obrasManager.CrearContacto(obra_id,nombre, email,telefono);

                TempData["FlashSuccess"] = MensajesResource.INFO_ObrasContacto_CreadoCorrectamente;
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var log = CommonManager.BuildMessageLog(
                    TipoMensaje.Error,
                    ControllerContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString(),
                    ControllerContext.Controller.ValueProvider.GetValue("action").RawValue.ToString(),
                    e.ToString(), Request);

                CommonManager.WriteAppLog(log, TipoMensaje.Error);
                TempData["FlashError"] = MensajesResource.ERROR_ObrasContacto_CreadoIncorrectamente;
                ModelState.AddModelError(string.Empty, e.Message);
                return View("Index");
            }
        }
        //asignar los roles para esta funcionalidad
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAROBRAS-CONSULTAR")]
        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);

            ViewBag.Obras = db.obras.Where(c => c.oficina != "SI").ToList();

            ViewBag.Equipos = db.equipos.ToList();

            ViewBag.Fallas = db.fallas.ToList();

            ViewBag.Envios = db.envios.ToList();

            ViewBag.Requerimientos = db.requerimientos.ToList();

            ViewBag.Herramientas = db.herramientas.ToList();

            ViewBag.Ventas = db.ventas.ToList();

            ViewBag.contactos = db.obras_contactos.ToList();

            ViewBag.ArchivosFotografia = "";

            ViewBag.ArchivosMapa = "";

            ViewBag.FallasCount = _fallasManager.GetSustituciones();
            TempData["sustituciones"] = ViewBag.FallasCount.Count;
            TempData.Keep();
            //ViewBag.EstatusCita = db.estatuscitas.ToList();

            return View();
        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAROBRAS-CONSULTAR")]
        public ActionResult Reporte(int id)
        {
            var obra = _obrasManager.Find(id);
            var db = new EntitiesDap();

            //ViewBag.ArchivosFotografia = 

            var contacto = _obrasManager.FindContactosObras(id);

            if (contacto != null)
            {
                ViewBag.contactos = contacto;
            }
            else
            {
                ViewBag.contactos = null;
            }


            var foto = _obrasManager.FindFotografiasObras(id);

            if (foto != null)
            {
                ViewBag.ArchivosFotografia = foto;
            }
            else
            {
                ViewBag.ArchivosFotografia = null;
            }

            var mapas = _obrasManager.FindMapasObras(id);

            if (mapas != null)
            {
                ViewBag.ArchivosMapa = mapas;
            }
            else
            {
                ViewBag.ArchivosMapa = null;
            }

            if (obra == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var obraModel = new ObraViewModel()
            {
                Nombre = obra.Nombre,
                PersonaJuridica = obra.PersonaJuridica,
                TelefonoOficina = obra.TelefonoOficina,
                DireccionOficina = obra.DireccionOficina,
                DireccionObra = obra.DireccionObra,
                DireccionFacturacion = obra.DireccionFacturacion,
                Contacto = obra.Contacto,
                Contacto2 = obra.Contacto2,
                Email = obra.Email,
                Email2 = obra.Email2,
                foto = obra.foto,
                mapa = obra.mapa
            };

            return PartialView(obraModel);
        }
        //[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-LISTAR,NAZAN-ADMINISTRARMENSAJESINSTITUCIONALES-MODIFICAR")]
        //public ActionResult Index()
        //{
        //    ViewBag.mensajes = _mensajesInstitucionalesManager.FindAll();
        //    return View();
        //}
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAROBRAS-CREAR")]
        public ActionResult Crear()
        {
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAROBRAS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var db = new EntitiesDap();

            var obra = _obrasManager.Find(id);
            
            TempData.Keep();
            ViewBag.contactos = _obrasManager.FindContactosObras(id);

            ViewBag.ArchivosFotografia = _obrasManager.FindFotografiasObras(id);

            ViewBag.ArchivosMapa = _obrasManager.FindMapasObras(id);

            //db.obras_contactos.ToList();

            if (obra == null)
            {
               // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var obraModel = new ObraViewModel()
            {
                Id = id,
                Nombre = obra.Nombre,
                PersonaJuridica = obra.PersonaJuridica,
                TelefonoOficina = obra.TelefonoOficina,
                DireccionOficina = obra.DireccionOficina,
                DireccionObra = obra.DireccionObra,
                DireccionFacturacion = obra.DireccionFacturacion,
                Contacto = obra.Contacto,
                Contacto2 = obra.Contacto2,
                Email = obra.Email,
                Email2 = obra.Email2,
                foto = obra.foto,
                mapa = obra.mapa
            };

            return View(obraModel);
        }

        public ActionResult Detalle(int id)
        {
            var obra = _obrasManager.Find(id);
            var db = new EntitiesDap();

       

            //ViewBag.ArchivosFotografia = 

            var contacto = _obrasManager.FindContactosObras(id);

            if (contacto != null)
            {
                ViewBag.contactos = contacto;
            }
            else
            {
                ViewBag.contactos = null;
            }


            var foto = _obrasManager.FindFotografiasObras(id);

            if (foto != null)
            {
                ViewBag.ArchivosFotografia = foto;
            }
            else
            {
                ViewBag.ArchivosFotografia = null;
            }

            var mapas = _obrasManager.FindMapasObras(id);

            if (mapas != null) {
                ViewBag.ArchivosMapa = mapas;
            }else
            {
                ViewBag.ArchivosMapa = null;
            }

            if (obra == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var obraModel = new ObraViewModel()
            {
                Nombre = obra.Nombre,
                PersonaJuridica = obra.PersonaJuridica,
                TelefonoOficina = obra.TelefonoOficina,
                DireccionOficina = obra.DireccionOficina,
                DireccionObra = obra.DireccionObra,
                DireccionFacturacion = obra.DireccionFacturacion,
                Contacto = obra.Contacto,
                Contacto2 = obra.Contacto2,
                Email = obra.Email,
                Email2 = obra.Email2,
                foto = obra.foto,
                mapa = obra.mapa
            };

            return View(obraModel);
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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAROBRAS-MODIFICAR")]
        public ActionResult Editar(int id, ObraViewModel model, FormCollection collection)
        {
            var obra = _obrasManager.Find(id);
            var fotoUrl = "";
            var fotoMapa = "";
            var Url = "";

            var db = new EntitiesDap();

            ViewBag.contactos = _obrasManager.FindContactosObras(id);

            ViewBag.ArchivosFotografia = _obrasManager.FindFotografiasObras(id);

            ViewBag.ArchivosMapa = _obrasManager.FindMapasObras(id);

            if (obra == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {

                //HttpPostedFileBase file_foto = Request.Files["file-foto"];
                /*
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file_foto = Request.Files[i];

                    _obrasManager.AgregarArchivos();

                }


                HttpPostedFileBase file_mapa = Request.Files["file-mapa"];

                if (file_foto != null && file_foto.ContentLength > 0)
                {
                    fotoUrl = CargarPdf(file_foto);
                }
                else { fotoUrl = obra.foto; }
                if (file_mapa != null && file_mapa.ContentLength > 0)
                {
                    fotoMapa = CargarPdf(file_mapa);
                }
                else { fotoMapa = obra.mapa; }
                */
                if (model.Nombre != null) { 
                _obrasManager.Actualizar(
                      id,
                      model.Nombre,
                      model.PersonaJuridica,
                      model.DireccionFacturacion,
                      model.DireccionOficina,
                      model.DireccionObra,
                      obra.TelefonoOficina,
                      obra.Contacto,
                      obra.Contacto2,
                      obra.Email,
                      obra.Email2, fotoUrl.Trim(), fotoMapa.Trim());
                }
                if(model.mapa != null) { 
                _obrasManager.AgregarArchivos(obra.Id, model.mapa, "obras", "mapa");
                }
                TempData["FlashSuccess"] = MensajesResource.INFO_Obras_ActualizadoCorrectamente;

                string nombre = Convert.ToString(collection["txtaddnombre"]);
                string email = Convert.ToString(collection["txtaddemail"]);
                string telefono = Convert.ToString(collection["txtaddtelefono"]);

                string editnombre = Convert.ToString(collection["txteditnombre"]);
                string editemail = Convert.ToString(collection["txteditemail"]);
                string edittelefono = Convert.ToString(collection["txtedittelefono"]);

                string deletenombre = Convert.ToString(collection["txtdeletenombre"]);

                int obra_id = id;
                string obracontacto_id = Convert.ToString(collection["txteditid"]); ;
                string obracontactodelete_id = Convert.ToString(collection["txtdeleteid"]);

                HttpPostedFileBase file;


                for (int i = 0; i < Request.Files.Count; i++)
                {

                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf-Fotografia" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(obra.Id, Url, "obras", "fotografia");
                    }
                }



                if (nombre != null) { 
                  _obrasManager.CrearContacto(obra_id, nombre, email, telefono);  
                }
                if (editnombre != null)
                {
                _obrasManager.EditarContacto(Convert.ToInt32(obracontacto_id), editnombre, editemail, edittelefono);
                }
                if (deletenombre != null)
                {
                    _obrasManager.EliminarContacto(Convert.ToInt32(obracontactodelete_id));
                }

                return RedirectToAction("Index","AdministrarObras");
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

       

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAROBRAS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(ObraViewModel model, FormCollection collection)
        {
            var Url = "";
            var fotoMapa = "";

            if (!ModelState.IsValid) return View(model);

            try
            {
                

                /*HttpPostedFileBase file_foto = Request.Files["file-foto"];

                HttpPostedFileBase file_mapa = Request.Files["file-mapa"];

                if (file_foto != null && file_foto.ContentLength > 0)
                {
                    fotoUrl = CargarPdf(file_foto);
                }
                else { fotoUrl = ""; }
                if (file_mapa != null && file_mapa.ContentLength > 0)
                {
                    fotoMapa = CargarPdf(file_mapa);
                }
                else { fotoMapa = ""; }
                */
                obras obra = _obrasManager.Crear(
                       model.Nombre,
                       model.PersonaJuridica,
                       model.DireccionFacturacion,
                       model.DireccionOficina,
                       model.DireccionObra,
                       model.TelefonoOficina,
                       model.Contacto,
                       model.Contacto2,
                       model.Email,
                       model.Email2, "", "");


                _obrasManager.AgregarArchivos(obra.Id, model.mapa, "obras", "mapa");

                HttpPostedFileBase file;


                for (int i = 0; i < Request.Files.Count; i++)
                {
                    
                    file = Request.Files[i];
                    var d = Request.Files.AllKeys[i].ToString();

                    if (d == "Pdf-Fotografia" && file.FileName != "") { 
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(obra.Id, Url, "obras", "fotografia");
                    }
                    if (d == "Pdf-Mapa" && file.FileName != "")
                    {
                        Url = CargarPdf(file);
                        _obrasManager.AgregarArchivos(obra.Id, Url, "obras", "mapa");
                    }

                }

                TempData["FlashSuccess"] = MensajesResource.INFO_Obras_CreadoCorrectamente;
                return RedirectToAction("Index");
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


        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAROBRAS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _obrasManager.Eliminar(id);
                TempData["FlashSuccess"] = MensajesResource.INFO_Obras_EliminadoCorrectamente;
                return RedirectToAction("Index");
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Index");

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
                return RedirectToAction("Index");
            }

        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRAROBRAS-MODIFICAR")]
        public ActionResult EliminarArchivo(string archivo,string tipo, string caracteristica,int id)
        {
            try
            {
                _obrasManager.EliminarArchivo(archivo, tipo,caracteristica);
                 TempData["FlashSuccess"] = MensajesResource.INFO_Obras_ActualizadoCorrectamente;
                return RedirectToAction("Editar", "AdministrarObras", new { @id = id });
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                return RedirectToAction("Editar", "AdministrarObras", new { @id = id });

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
                return RedirectToAction("Editar", "AdministrarObras", new { @id = id });
            }

        }
    }
}