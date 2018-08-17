using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class InformesManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();


		public List<informes> FindAll()
		{
			return _db.informes.ToList();
		}

	
		public informes Find(int id)
		{
			return _db.informes.FirstOrDefault(e => e.Id ==id);
		}

        public informes GetInforme(int id)
        {
            return _db.informes.FirstOrDefault(s => s.Id == id);
        }

        public informes GetInformeVenta(int id)
        {
            return _db.informes.FirstOrDefault(s => s.IdVenta == id);
        }

        public List<informes> GetInformes(int ventaId)
		{
			return _db.informes.Where(s => s.IdVenta == ventaId).ToList();

		}

		public informes Actualizar(int ventaid, int id, string Nombre = null, string Url = null, string Fecha = null)
		{

			var informe = _db.informes.Find(id);

            if (ventaid != 0)
            {
                informe.IdVenta = ventaid;
            }
            if (informe == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			if (Nombre != null)
			{

				informe.Nombre = Nombre;
			}
			if (Url != null)
			{

				informe.Url = Url;
			}
			if (Fecha != null)
			{

				informe.Fecha = DateTime.Parse(Fecha);
			}


			_db.Entry(informe).State = EntityState.Modified;
			_db.SaveChanges();

			return informe;
		}


		public void Crear(int ventaid, string Nombre = null, string Url = null, string Fecha = null)
		{
		   

	 
			try
			{
				var informe = new informes()
				{

					Nombre = Nombre,
					Url = Url,
					Fecha = DateTime.Parse(Fecha),
                    IdVenta = ventaid

				};

				_db.informes.Add(informe);
				_db.SaveChanges();

			}
			catch (Exception)
			{
			   

				throw;
			}


		}

        public void Eliminar(int id)
        {

            var informe = Find(id);

            // componenteelectrico.cuentasmensajes.Clear();

            _db.informes.Remove(informe);
            _db.SaveChanges();
        }

    }
}
