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
using System.Collections.Generic;

namespace Ppgz.Web.Areas.Dap.Controllers
{
    [Authorize]
    public class AdministrarEnviosController : Controller
    {
        private readonly EnviosManager _enviosManager = new EnviosManager();

        private readonly ObrasManager _obrasManager = new ObrasManager();


        //asignar los roles para esta funcionalidad
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARENVIOS-CONSULTAR")]
        public ActionResult Index()
        {
            var db = new EntitiesDap();

            var fecha = DateTime.Today.AddMonths(-3);


            ViewBag.Envios = db.envios.ToList();

            ViewBag.Envios = _enviosManager.GetEnvios();


            //ViewBag.EstatusCita = db.estatuscitas.ToList();

            return View();
        }
        public ActionResult Historico(int id)
        {

            ViewBag.obra = _obrasManager.Find(id);
            ViewBag.id = id;
            ViewBag.Envios = _enviosManager.GetHistorico(id);
            return View("Index");
        }
        public ActionResult HistoricoTodos()
        {
            ViewBag.Envios = _enviosManager.GetHistoricoAll();
            return View("Index");
        }

        public ActionResult Envios(int id)
        {
            ViewBag.obra = _obrasManager.Find(id);
            ViewBag.id = id;
            TempData["obra"] = id;
            TempData.Keep();
            ViewBag.Envios = _enviosManager.GetEnvios(id);
            return View("Index");
        }

        public ActionResult ReporteEnvios(int id)
        {
            ViewBag.obra = _obrasManager.Find(id);
            ViewBag.id = id;
            TempData["obra"] = id;
            TempData.Keep();
            ViewBag.Envios = _enviosManager.GetEnvios(id);
            return PartialView();
        }

       
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARENVIOS-CREAR")]
        public ActionResult Crear(int id)
        {
            
            var obra = _obrasManager.Find(id);
            TempData["obra"] = obra.Id;
            TempData.Keep();

            ViewBag.Obras =
              new SelectList(_enviosManager.FindObras(), "nombre", "nombre", obra.Nombre);
           
 
            
            ViewBag.Componentes =
             new SelectList(_enviosManager.FindComponentes(), "descripcion", "descripcion");
            
            ViewBag.StatusEnvio =
               new SelectList(_enviosManager.FindStatus(), "descripcion", "descripcion");

            ViewBag.StatusPago =
               new SelectList(_enviosManager.FindStatusPagos(), "descripcion", "descripcion");
            /*
            ViewBag.StatusEntrega =
               new SelectList(_enviosManager.FindStatusEntrega(), "descripcion", "descripcion");*/
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARENVIOS-MODIFICAR")]
        public ActionResult CrearPorDefecto()
        {
           
                ViewBag.Obras =
              new SelectList(_enviosManager.FindObrasWithOficina(), "nombre", "nombre");
            


            ViewBag.Componentes =
             new SelectList(_enviosManager.FindComponentes(), "descripcion", "descripcion");

            ViewBag.StatusEnvio =
               new SelectList(_enviosManager.FindStatus(), "descripcion", "descripcion");

            ViewBag.StatusPago =
               new SelectList(_enviosManager.FindStatusPagos(), "descripcion", "descripcion");
            /*
            ViewBag.StatusEntrega =
               new SelectList(_enviosManager.FindStatusEntrega(), "descripcion", "descripcion");*/
            return View();
        }

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARENVIOS-MODIFICAR")]
        public ActionResult Editar(int id)
        {
            var envio = _enviosManager.Find(id);

            var obra = _obrasManager.Find(envio.obra_id);

            TempData["obra"] = obra.Id;

            ViewBag.StatusPago2 =
               new SelectList(_enviosManager.FindStatusPagos(), "descripcion", "descripcion", envio.StatusPago);

            ViewBag.StatusEnvio2 =
            new SelectList(_enviosManager.FindStatus(), "descripcion", "descripcion", envio.StatusEnvio);

            if (obra != null)
            {
                ViewBag.Obras =
                  new SelectList(_enviosManager.FindObras(), "nombre", "nombre", obra.Nombre);
            }else
            {
                ViewBag.Obras =
                  new SelectList(_enviosManager.FindObras(), "nombre", "nombre");
            }
                /*}
                            ViewBag.Componentes =
                             new SelectList(_enviosManager.FindComponentes(), "descripcion", "descripcion", envio.TipoComponente);
                           */
                /*  ViewBag.StatusEnvio =
                     new SelectList(_enviosManager.FindStatus(), "descripcion", "descripcion", envio.StatusEnvio)
      */


                // new SelectList(new[] { new { ID = "OBRA", Name = "OBRA" }, new { ID = "EQUIPO", Name = "EQUIPO" }, }, "ID", "Name", falla.Condicion);
                /*
                ViewBag.StatusEntrega =
                   new SelectList(_enviosManager.FindStatusEntrega(), "descripcion", "descripcion", envio.StatusEntrega);
                */
                if (envio == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var envioModel = new EnvioViewModel()
            {
              Obra = envio.Obra,
                TipoComponente = envio.TipoComponente,
                FechaPedido = envio.FechaPedido,
                FechaSalida = envio.FechaSalida,
                Tracking = envio.Tracking,
                EmpresaEnvio = envio.EmpresaEnvio,
                StatusEnvio = envio.StatusEnvio,
                NumeroBulto = envio.NumeroBulto,
                PesoTotal = envio.PesoTotal,
                CodigoArancelario = envio.CodigoArancelario,
                Solicitado = envio.Solicitado,
                CostoProducto = envio.CostoProducto,
                StatusPago = envio.StatusPago,
                NumeroFactura = envio.NumeroFactura,
                StatusEntrega = envio.StatusEntrega,
                EntregaProducto = envio.EntregaProducto,
                FechaEntrega = envio.FechaEntrega,
                obra_id = envio.obra_id,
                id = envio.Id

            };

            return View(envioModel);
        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARENVIOS-CONSULTAR")]
        public ActionResult Reporte(int id)
        {
            var envio = _enviosManager.Find(id);

            var obra = _obrasManager.Find(envio.obra_id);

            ViewBag.StatusPago2 =
               new SelectList(_enviosManager.FindStatusPagos(), "descripcion", "descripcion", envio.StatusPago);

            ViewBag.StatusEnvio2 =
            new SelectList(_enviosManager.FindStatus(), "descripcion", "descripcion", envio.StatusEnvio);

            if(obra != null)
            {
                ViewBag.Obras =
                new SelectList(_enviosManager.FindObras(), "nombre", "nombre", obra.Nombre);
            }
            
            
            if (envio == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var envioModel = new EnvioViewModel()
            {
                Obra = envio.Obra,
                TipoComponente = envio.TipoComponente,
                FechaPedido = envio.FechaPedido,
                FechaSalida = envio.FechaSalida,
                Tracking = envio.Tracking,
                EmpresaEnvio = envio.EmpresaEnvio,
                StatusEnvio = envio.StatusEnvio,
                NumeroBulto = envio.NumeroBulto,
                PesoTotal = envio.PesoTotal,
                CodigoArancelario = envio.CodigoArancelario,
                Solicitado = envio.Solicitado,
                CostoProducto = envio.CostoProducto,
                StatusPago = envio.StatusPago,
                NumeroFactura = envio.NumeroFactura,
                StatusEntrega = envio.StatusEntrega,
                EntregaProducto = envio.EntregaProducto,
                FechaEntrega = envio.FechaEntrega,
                obra_id = envio.obra_id,
                id = envio.Id

            };

            return PartialView(envioModel);
        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARENVIOS-CONSULTAR")]
        public ActionResult Detalle(int id)
        {
            var envio = _enviosManager.Find(id);

            var obra = _obrasManager.Find(envio.obra_id);

            if(obra != null) { 
            ViewBag.Obras =
              new SelectList(_enviosManager.FindObras(), "nombre", "nombre", obra.Nombre);
            }
            else
            {
                ViewBag.Obras =
                  new SelectList(_enviosManager.FindObras(), "nombre", "nombre");
            }
            ViewBag.Componentes =
             new SelectList(_enviosManager.FindComponentes(), "descripcion", "descripcion", envio.TipoComponente);

            ViewBag.StatusEnvio =
               new SelectList(_enviosManager.FindStatus(), "descripcion", "descripcion", envio.StatusEnvio);

            ViewBag.StatusPago =
               new SelectList(_enviosManager.FindStatusPagos(), "descripcion", "descripcion", envio.StatusPago);
            /*
            ViewBag.StatusEntrega =
               new SelectList(_enviosManager.FindStatusEntrega(), "descripcion", "descripcion", envio.StatusEntrega);
            */
            if (envio == null)
            {
                // TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            var envioModel = new EnvioViewModel()
            {
                Obra = envio.Obra,
                TipoComponente = envio.TipoComponente,
                FechaPedido = envio.FechaPedido,
                FechaSalida = envio.FechaSalida,
                Tracking = envio.Tracking,
                EmpresaEnvio = envio.EmpresaEnvio,
                StatusEnvio = envio.StatusEnvio,
                NumeroBulto = envio.NumeroBulto,
                PesoTotal = envio.PesoTotal,
                CodigoArancelario = envio.CodigoArancelario,
                Solicitado = envio.Solicitado,
                CostoProducto = envio.CostoProducto,
                StatusPago = envio.StatusPago,
                NumeroFactura = envio.NumeroFactura,
                StatusEntrega = envio.StatusEntrega,
                EntregaProducto = envio.EntregaProducto,
                FechaEntrega = envio.FechaEntrega,
                obra_id = envio.obra_id,
                id = envio.Id

            };

            return View(envioModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARENVIOS-MODIFICAR")]
        public ActionResult Editar(int id, EnvioViewModel model)
        {
            var envio = _enviosManager.Find(id);

            var obra = _obrasManager.Find_by_name(envio.Obra);

            ViewBag.Obras =
              new SelectList(_enviosManager.FindObras(id), "nombre", "nombre", obra.Nombre);
            
            ViewBag.Componentes =
             new SelectList(_enviosManager.FindComponentes(), "descripcion", "descripcion", envio.TipoComponente);
            
            ViewBag.StatusEnvio =
               new SelectList(_enviosManager.FindStatus(), "descripcion", "descripcion", envio.StatusEnvio);

            ViewBag.StatusPago =
               new SelectList(_enviosManager.FindStatusPagos(), "descripcion", "descripcion", envio.StatusPago);
/*
            ViewBag.StatusEntrega =
               new SelectList(_enviosManager.FindStatusEntrega(), "descripcion", "descripcion", envio.StatusEntrega);

           */

            if (envio == null)
            {
                //TempData["FlashError"] = MensajesResource.ERROR_MensajesInstitucionales_IdIncorrecto;
                return RedirectToAction("Index");
            }

            try
            {
                _enviosManager.Actualizar(Convert.ToInt32(TempData["obra"]),
                      id,
                      model.Obra,
                      model.TipoComponente,
                      model.FechaPedido,
                      model.FechaSalida,
                      model.Tracking,
                      model.EmpresaEnvio,
                      model.StatusEnvio,
                      model.NumeroBulto,
                      model.PesoTotal,
                      model.CodigoArancelario,
                      model.Solicitado,
                      model.CostoProducto,
                      model.StatusPago,
                      model.NumeroFactura,
                      model.StatusEntrega,
                      model.EntregaProducto,
                      model.FechaEntrega);

                TempData.Keep();
                TempData["FlashSuccess"] = MensajesResource.INFO_Envios_ActualizadoCorrectamente;
              //  return RedirectToAction("Index");
                //return RedirectToAction("Editar", "AdministrarEnvios", id);
                return RedirectToAction("Envios", "AdministrarEnvios", new { @Id = TempData["obra"] });

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



        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARENVIOS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(EnvioViewModel model, FormCollection collection)
        {

            var obra = _obrasManager.Find(Convert.ToInt32(TempData["obra"]));

            var obranombre = _obrasManager.FindObrasPorNombre(obra.Nombre);

            ViewBag.Obras =
              new SelectList(_enviosManager.FindObras(), "nombre", "nombre");

            ViewBag.Componentes =
             new SelectList(_enviosManager.FindComponentes(), "descripcion", "descripcion");

            ViewBag.StatusEnvio =
               new SelectList(_enviosManager.FindStatus(), "descripcion", "descripcion");

            ViewBag.StatusPago =
               new SelectList(_enviosManager.FindStatusPagos(), "descripcion", "descripcion");

            ViewBag.StatusEntrega =
               new SelectList(_enviosManager.FindStatusEntrega(), "descripcion", "descripcion");

            //if (!ModelState.IsValid) return View(model);

            try
            {
                if(TempData["obra"] != null) { 
                _enviosManager.Crear(Convert.ToInt32(TempData["obra"]),
                       obra.Nombre,
                       model.TipoComponente,
                       model.FechaPedido,
                       model.FechaSalida,
                       model.Tracking,
                       model.EmpresaEnvio,
                       model.StatusEnvio,
                       model.NumeroBulto,
                       model.PesoTotal,
                       model.CodigoArancelario,
                       model.Solicitado,
                       model.CostoProducto,
                       model.StatusPago,
                       model.NumeroFactura,
                       model.StatusEntrega,
                       model.EntregaProducto,
                       model.FechaEntrega);
                }
                //else if(model.obra_id != null)
                //{
                //    _enviosManager.Crear(obranombre.Id,
                //       model.Obra,
                //       model.TipoComponente,
                //       model.FechaPedido,
                //       model.FechaSalida,
                //       model.Tracking,
                //       model.EmpresaEnvio,
                //       model.StatusEnvio,
                //       model.NumeroBulto,
                //       model.PesoTotal,
                //       model.CodigoArancelario,
                //       model.Solicitado,
                //       model.CostoProducto,
                //       model.StatusPago,
                //       model.NumeroFactura,
                //       model.StatusEntrega,
                //       model.EntregaProducto,
                //       model.FechaEntrega);
                //}
                TempData.Keep();
                TempData["FlashSuccess"] = MensajesResource.INFO_Envios_CreadoCorrectamente;
                if (TempData["obra"] != null)
                {
                    return RedirectToAction("Envios", "AdministrarEnvios", new { @id = TempData["obra"] });
                }else
                {
                    return RedirectToAction("Index", "AdministrarEnvios");
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

        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARENVIOS-CREAR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearPorDefecto(EnvioViewModel model, FormCollection collection)
        {

            //var obra = _obrasManager.Find(model.Obra);

            var obra = _obrasManager.FindObrasPorNombre(model.Obra);

            ViewBag.Obras =
              new SelectList(_enviosManager.FindObras(), "nombre", "nombre");

            ViewBag.Componentes =
             new SelectList(_enviosManager.FindComponentes(), "descripcion", "descripcion");

            ViewBag.StatusEnvio =
               new SelectList(_enviosManager.FindStatus(), "descripcion", "descripcion");

            ViewBag.StatusPago =
               new SelectList(_enviosManager.FindStatusPagos(), "descripcion", "descripcion");

            ViewBag.StatusEntrega =
               new SelectList(_enviosManager.FindStatusEntrega(), "descripcion", "descripcion");

            //if (!ModelState.IsValid) return View(model);

            try
            {

                _enviosManager.Crear(obra.Id,
                           obra.Nombre,
                           model.TipoComponente,
                           model.FechaPedido,
                           model.FechaSalida,
                           model.Tracking,
                           model.EmpresaEnvio,
                           model.StatusEnvio,
                           model.NumeroBulto,
                           model.PesoTotal,
                           model.CodigoArancelario,
                           model.Solicitado,
                           model.CostoProducto,
                           model.StatusPago,
                           model.NumeroFactura,
                           model.StatusEntrega,
                           model.EntregaProducto,
                           model.FechaEntrega);
                   
               
                //else if(model.obra_id != null)
                //{
                //    _enviosManager.Crear(obranombre.Id,
                //       model.Obra,
                //       model.TipoComponente,
                //       model.FechaPedido,
                //       model.FechaSalida,
                //       model.Tracking,
                //       model.EmpresaEnvio,
                //       model.StatusEnvio,
                //       model.NumeroBulto,
                //       model.PesoTotal,
                //       model.CodigoArancelario,
                //       model.Solicitado,
                //       model.CostoProducto,
                //       model.StatusPago,
                //       model.NumeroFactura,
                //       model.StatusEntrega,
                //       model.EntregaProducto,
                //       model.FechaEntrega);
                //}
                TempData.Keep();
                TempData["FlashSuccess"] = MensajesResource.INFO_Envios_CreadoCorrectamente;
                if(obra.Nombre == "OFICINA")
                {
                    return RedirectToAction("Index", "AdministrarEnvios");
                }
                if (TempData["obra"] != null)
                {
                    return RedirectToAction("Envios", "AdministrarEnvios", new { @id = TempData["obra"] });
                }
                else
                {
                    return RedirectToAction("Index", "AdministrarEnvios");
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
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARENVIOS-ELIMINAR")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                _enviosManager.Eliminar(id);
                TempData["FlashSuccess"] = MensajesResource.INFO_Envios_EliminadoCorrectamente;
                if (TempData["obra"] != null)
                {
                    return RedirectToAction("Envios", "AdministrarEnvios", new { @id = TempData["obra"] });
                }
                else
                {
                    return RedirectToAction("Index", "AdministrarEnvios");
                }
            }
            catch (BusinessException businessEx)
            {
                TempData["FlashError"] = businessEx.Message;
                
                if (TempData["obra"] != null)
                {
                    return RedirectToAction("Envios", "AdministrarEnvios", new { @id = TempData["obra"] });
                }
                else
                {
                    return RedirectToAction("Index", "AdministrarEnvios");
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
                    return RedirectToAction("Envios", "AdministrarEnvios", new { @id = TempData["obra"] });
                }
                else
                {
                    return RedirectToAction("Index", "AdministrarEnvios");
                }
            }

        }
    }
}