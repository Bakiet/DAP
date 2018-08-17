using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class VentasManager
    {
		private readonly EntitiesDap _db = new EntitiesDap();


		public List<ventas> FindAll()
		{
			return _db.ventas.ToList();
		}

	
		public ventas Find(int? id)
		{
			return _db.ventas.FirstOrDefault(e => e.Id ==id);
		}

	   
		public List<ventas> GetVentas(int obraId)
		{
			return _db.ventas.Where(v => v.IdObra == obraId).ToList();

		}

		public ventas Actualizar(int id, string descripcion = null)
		{

			var ventas = _db.ventas.Find(id);

			if (ventas == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			if (descripcion != null)
			{
				
				ventas.Descripcion = descripcion;
			}
			

			_db.Entry(ventas).State = EntityState.Modified;
			_db.SaveChanges();

			return ventas;
		}


		public void Crear(string descripcion = null)
		{
		   

			try
			{
				var venta = new ventas()
				{

					Descripcion = descripcion
				};

				_db.ventas.Add(venta);
				_db.SaveChanges();

			}
			catch (Exception)
			{
			   

				throw;
			}


		}


	}
}
