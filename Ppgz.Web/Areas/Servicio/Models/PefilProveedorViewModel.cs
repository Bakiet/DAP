using System.Web.Mvc;
using Ppgz.Services;
using Ppgz.Web.Areas.Dap.Models;

namespace Ppgz.Web.Areas.Servicio.Models
{
    public class PefilProveedorViewModel : PefilNazanViewModel
    {
        public PefilProveedorViewModel()
        {
            var perfilrManager = new PerfilManager();

            var roles = perfilrManager.GetRolesServicio();

            Roles = new MultiSelectList(roles, "Id", "Description");
        }
    }
}