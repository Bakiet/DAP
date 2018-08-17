using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.IO;
using Ppgz.Web.Areas.Dap.Models;
using System.Reflection;
using Ppgz.Repository;

namespace Ppgz.Web.Areas.Dap.Controllers
{
	public class AdministrarCalendarioController : Controller
	{

		
		[Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCALENDARIO-CONSULTAR")]
		public ActionResult Index()
		{

			return View();
		}

        public JsonResult GetEvents()
        {
            
            using (EntitiesDap db = new EntitiesDap())
            {
                var events = db.events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCALENDARIO-CREAR")]
        [HttpPost]
        public JsonResult SaveEvent(events e)
        {
            var status = false;
            using (EntitiesDap db = new EntitiesDap())
            {
                if (e.Id > 0)
                {
                    //Update the event
                    var v = db.events.Where(a => a.Id == e.Id).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    db.events.Add(e);
                }

                db.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }
        [Authorize(Roles = "MAESTRO-NAZAN,NAZAN-ADMINISTRARCALENDARIO-ELIMINAR")]
        [HttpPost]
        public JsonResult DeleteEvent(int Id)
        {
            var status = false;
            using (EntitiesDap db = new EntitiesDap())
            {
                var v = db.events.Where(a => a.Id == Id).FirstOrDefault();
                if (v != null)
                {
                    db.events.Remove(v);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}