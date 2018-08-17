using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class ComponentesElectricosSustitucionesManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();


		public List<componenteselectricos_sustituciones> FindAll()
		{
			return _db.componenteselectricos_sustituciones.ToList();
		}

	
		public componenteselectricos_sustituciones Find(int id)
		{
			return _db.componenteselectricos_sustituciones.FirstOrDefault(e => e.Id ==id);
		}

	   
		public List<componenteselectricos_sustituciones> GetComponentesElectricosSustituciones(int equipoId)
		{
			return _db.componenteselectricos_sustituciones.Where(s => s.IdEquipos == equipoId).ToList();

		}

		public componenteselectricos_sustituciones Actualizar(int id, string Tipo = null, string Caracteristicas = null, string Descripcion = null,
			string Marca = null, string Modelo = null, string Serial = null, string FechaFabricado = null, string Duracion = null
			, string Sustitucion = null, string Fotografia = null)
		{

			var componenteelectrico_sustitucion = _db.componenteselectricos_sustituciones.Find(id);

			if (componenteelectrico_sustitucion == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			if (Tipo != null)
			{

				componenteelectrico_sustitucion.Tipo = Tipo;
			}
			if (Descripcion != null)
			{

				componenteelectrico_sustitucion.Descripcion = Descripcion;
			}
			if (Caracteristicas != null)
			{

				componenteelectrico_sustitucion.Caracteristicas = Caracteristicas;
			}
			if (Descripcion != null)
			{

				componenteelectrico_sustitucion.Descripcion = Descripcion;
			}
			if (Marca != null)
			{

				componenteelectrico_sustitucion.Marca = Marca;
			}
			if (Modelo != null)
			{

				componenteelectrico_sustitucion.Modelo = Modelo;
			}
			if (Serial != null)
			{

				componenteelectrico_sustitucion.Serial = Serial;
			}

			if (FechaFabricado != null)
			{

				componenteelectrico_sustitucion.FechaFabricado = DateTime.Parse(FechaFabricado);
			}


			if (Duracion != null)
			{

				componenteelectrico_sustitucion.Duracion = Duracion;
			}
			if (Sustitucion != null)
			{

				componenteelectrico_sustitucion.Sustitucion = Sustitucion;
			}
			if (Fotografia != null)
			{

				componenteelectrico_sustitucion.Fotografia = Fotografia;
			}
			

			_db.Entry(componenteelectrico_sustitucion).State = EntityState.Modified;
			_db.SaveChanges();

			return componenteelectrico_sustitucion;
		}


		public void Crear(string Tipo = null, string Caracteristicas = null, string Descripcion = null,
			string Marca = null, string Modelo = null, string Serial = null, string FechaFabricado = null, string Duracion = null
			,string Sustitucion = null, string Fotografia = null)
		{
		   

	 
			try
			{
				var componenteelectrico_sustitucion = new componenteselectricos_sustituciones()
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

				_db.componenteselectricos_sustituciones.Add(componenteelectrico_sustitucion);
				_db.SaveChanges();

			}
			catch (Exception)
			{
			   

				throw;
			}


		}


	}
}
