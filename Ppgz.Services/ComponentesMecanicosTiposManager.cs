using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class ComponentesMecanicosTiposManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();

        
        public List<componentesmecanicos_tipos> FindAll()
		{
			return _db.componentesmecanicos_tipos.ToList();
		}

	
		public componentesmecanicos_tipos Find(int id)
		{
			return _db.componentesmecanicos_tipos.FirstOrDefault(e => e.id ==id);
		}

	   
		

		public componentesmecanicos_tipos Actualizar(int id, string Descripcion = null)
		{

			var componentemecanicotipo = _db.componentesmecanicos_tipos.Find(id);

			if (componentemecanicotipo == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			
			if (Descripcion != null)
			{

                componentemecanicotipo.descripcion = Descripcion;
			}
			

			_db.Entry(componentemecanicotipo).State = EntityState.Modified;
			_db.SaveChanges();

			return componentemecanicotipo;
		}


		public componentesmecanicos_tipos Crear(string Descripcion = null)
		{

			try
			{
				var componentemecanicotipo = new componentesmecanicos_tipos()
				{

					descripcion = Descripcion

				};

				_db.componentesmecanicos_tipos.Add(componentemecanicotipo);
				_db.SaveChanges();

                return componentemecanicotipo;

            }
			catch (Exception)
			{
			   

				throw;
			}


		}

        public void Eliminar(int id)
        {

            var componentemecanico_tipos = Find(id);

           
            _db.componentesmecanicos_tipos.Remove(componentemecanico_tipos);
            _db.SaveChanges();
        }
    }
}
