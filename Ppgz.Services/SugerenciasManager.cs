using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class SugerenciasManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();


		public List<sugerencias> FindAll()
		{
			return _db.sugerencias.ToList();
		}

	
		public sugerencias Find(int id)
		{
			return _db.sugerencias.FirstOrDefault(e => e.Id ==id);
		}

        public sugerencias GetSugerencia(int id)
        {
            return _db.sugerencias.FirstOrDefault(s => s.IdVenta == id);
        }

        public List<sugerencias> GetSugerencias(int ventaId)
		{
			return _db.sugerencias.Where(s => s.IdVenta == ventaId).ToList();

		}

		public sugerencias Actualizar(int idventa, int id, string descripcion = null, string caracteristica = null, string numero = null,
			string fecha = null, string accionestomadas = null, string accionesrecomendadas = null, string gerenciaresponsable = null)
		{

			var sugerencias = _db.sugerencias.Find(id);

            if (idventa != 0)
            {
                sugerencias.IdVenta = idventa;
            }

            if (sugerencias == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			if (descripcion != null)
			{

				sugerencias.Descripcion = descripcion;
			}
			if (caracteristica != null)
			{

				sugerencias.Caracteristica = caracteristica;
			}
			if (numero != null)
			{

				sugerencias.Numero = numero;
			}
			if (fecha != null)
			{

				sugerencias.Fecha = DateTime.Parse(fecha);
			}
			if (accionestomadas != null)
			{

				sugerencias.AccionesTomadas = accionestomadas;
			}
			if (accionesrecomendadas != null)
			{

				sugerencias.AccionesRecomendadas = accionesrecomendadas;
			}
			if (gerenciaresponsable != null)
			{

				sugerencias.GerenciaResponsable = gerenciaresponsable;
			}
			
		

			_db.Entry(sugerencias).State = EntityState.Modified;
			_db.SaveChanges();

			return sugerencias;
		}


		public void Crear(int ventaid, string descripcion = null, string caracteristica = null, string numero = null,
			string fecha = null, string accionestomadas = null, string accionesrecomendadas = null, string gerenciaresponsable = null)
		{
		   

	 
			try
			{
				var sugerencia = new sugerencias()
				{

					Descripcion = descripcion,
					Caracteristica = caracteristica,
					Numero = numero,
					Fecha = DateTime.Parse(fecha),
					AccionesTomadas = accionestomadas,
					AccionesRecomendadas = accionesrecomendadas,
					GerenciaResponsable = gerenciaresponsable,
                    IdVenta = ventaid
				};

				_db.sugerencias.Add(sugerencia);
				_db.SaveChanges();

			}
			catch (Exception)
			{
			   

				throw;
			}


		}
		public void Eliminar(int id)
		{

			var sugerencia = Find(id);

			// componenteelectrico.cuentasmensajes.Clear();

			_db.sugerencias.Remove(sugerencia);
			_db.SaveChanges();
		}

	}
}
