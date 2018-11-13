using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ppgz.Repository;
using Ppgz.Web.Infrastructure;
using Ppgz.Web.Infrastructure.Dap;
using Ppgz.Web.Models;
using System.Threading.Tasks;
using Ppgz.Services;

namespace Ppgz.Web.Controllers
{
    
    [Authorize]
    public class HomeController : Controller
    {
        private readonly FallasManager _fallasManager = new FallasManager();
        private readonly ComponentesMecanicosManager _componentesmecanicosManager = new ComponentesMecanicosManager();
        private readonly ComponentesElectricosManager _componentesElectricos_Manager = new ComponentesElectricosManager();
        [TerminosCondiciones]
        public ActionResult Index()
        {
            ViewBag.FallasCount = _fallasManager.GetSustituciones();
            TempData["sustituciones"] = ViewBag.FallasCount.Count;
            TempData.Keep();

            ViewBag.ComponentesMecanicosCount = _componentesmecanicosManager.GetSustituciones();
            ViewBag.ComponentesMecanicos = _componentesmecanicosManager.GetSustituciones();
            TempData["sustitucionesmecanicas"] = ViewBag.ComponentesMecanicosCount.Count;
            TempData.Keep();

            ViewBag.ComponentesElectricosCount = _componentesElectricos_Manager.GetSustituciones();
            ViewBag.ComponentesElectricos = _componentesElectricos_Manager.GetSustituciones();
            TempData["sustitucioneselectronicas"] = ViewBag.ComponentesElectricosCount.Count;
            TempData.Keep();
            return View();
        }
       
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public void CreateRole()
        {
           CommonManager commonManager= new CommonManager();

           var table = commonManager.QueryToTable("SELECT * FROM cuentas;");
           


        }

        public void AddToRole()
        {/*
            var context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            userManager.AddToRole(User.Identity.GetUserId(), "NAZAN-ADMINSITRARUSUARIOSNAZAN-LISTAR");*/
        }

        [Authorize(Roles = "MAESTRO")]
        public void TestRole()
        {
            /*var context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            userManager.AddToRole(User.Identity.GetUserId(), "MAESTRO");
            Response.Write("TENGO ACCESO MAESTRO");*/
        }

        public void Terminos()
        {
            Response.Write("DEBE ACEPTAR");
        }

        public async Task<ActionResult> TestMail()
        {
            var commonManager = new CommonManager();
            await commonManager.SendHtmlMail(
                "Prueba","hola<br>juan<br>godoy async","g.juanch14@gmail.com");
            return  Content("Correo enviado correctamente");
        }

    }
}