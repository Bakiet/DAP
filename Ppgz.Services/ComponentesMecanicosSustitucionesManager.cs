using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class ComponentesMecanicosSustitucionesManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();


		public List<componentesmecanicos_sustituciones> FindAll()
		{
			return _db.componentesmecanicos_sustituciones.ToList();
		}

	
		public componentesmecanicos_sustituciones Find(int id)
		{
			return _db.componentesmecanicos_sustituciones.FirstOrDefault(e => e.Id ==id);
		}

	   
		public List<componentesmecanicos_sustituciones> GetComponentesMecanicosSustituciones(int equipoId)
		{
			return _db.componentesmecanicos_sustituciones.Where(s => s.IdEquipos == equipoId).ToList();

		}

		public componentesmecanicos_sustituciones Actualizar(int id, string Tipo = null, string Caracteristicas = null, string Descripcion = null,
			string Marca = null, string Modelo = null, string Serial = null, string FechaFabricado = null, string Duracion = null
			, string Sustitucion = null, string Fotografia = null)
		{

			var componentemecanico_sustitucion = _db.componentesmecanicos_sustituciones.Find(id);

			if (componentemecanico_sustitucion == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			if (Tipo != null)
			{

				componentemecanico_sustitucion.Tipo = Tipo;
			}
			if (Descripcion != null)
			{

				componentemecanico_sustitucion.Descripcion = Descripcion;
			}
			if (Caracteristicas != null)
			{

				componentemecanico_sustitucion.Caracteristicas = Caracteristicas;
			}
			if (Descripcion != null)
			{

				componentemecanico_sustitucion.Descripcion = Descripcion;
			}
			if (Marca != null)
			{

				componentemecanico_sustitucion.Marca = Marca;
			}
			if (Modelo != null)
			{

				componentemecanico_sustitucion.Modelo = Modelo;
			}
			if (Serial != null)
			{

				componentemecanico_sustitucion.Serial = Serial;
			}

			if (FechaFabricado != null)
			{

				componentemecanico_sustitucion.FechaFabricado = DateTime.Parse(FechaFabricado);
			}


			if (Duracion != null)
			{

				componentemecanico_sustitucion.Duracion = Duracion;
			}
			if (Sustitucion != null)
			{

				componentemecanico_sustitucion.Sustitucion = Sustitucion;
			}
			if (Fotografia != null)
			{

				componentemecanico_sustitucion.Fotografia = Fotografia;
			}
			

			_db.Entry(componentemecanico_sustitucion).State = EntityState.Modified;
			_db.SaveChanges();

			return componentemecanico_sustitucion;
		}


		public void Crear(string Tipo = null, string Caracteristicas = null, string Descripcion = null,
			string Marca = null, string Modelo = null, string Serial = null, string FechaFabricado = null, string Duracion = null
			,string Sustitucion = null, string Fotografia = null)
		{
		   

	 
			try
			{
				var componentemecanico_sustitucion = new componentesmecanicos_sustituciones()
				{

					Tipo = Tipo,
					Caracteristicas = Caracteristicas,
					Descripcion = Descripcion,
					FechaFabricado = DateTime.Parse(FechaFabricado),
					Marca = Marca,
					Modelo  = Modelo,
					Serial = Serial,
					Duracion = Duracion,
					Sustitucion = Sustitucion,
					Fotografia = Fotografia

				};

				_db.componentesmecanicos_sustituciones.Add(componentemecanico_sustitucion);
				_db.SaveChanges();

			}
			catch (Exception)
			{
			   

				throw;
			}


		}


	}
}
