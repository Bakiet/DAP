using System.Web.Mvc;
using Ppgz.Services;
using Ppgz.Web.Areas.Dap.Models;

namespace Ppgz.Web.Areas.Mercaderia.Models
{
    public class PefilProveedorViewModel : PefilNazanViewModel
    {
        public PefilProveedorViewModel()
        {
            var perfilManager = new PerfilManager();

            var roles =perfilManager.GetRolesMercaderia();

            Roles = new MultiSelectList(roles, "Id", "Description");
        }
    }
}