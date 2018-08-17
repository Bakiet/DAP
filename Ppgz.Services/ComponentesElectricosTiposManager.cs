using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class ComponentesElectricosTiposManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();

        
        public List<componenteselectronicos_tipos> FindAll()
		{
			return _db.componenteselectronicos_tipos.ToList();
		}

	
		public componenteselectronicos_tipos Find(int id)
		{
			return _db.componenteselectronicos_tipos.FirstOrDefault(e => e.id ==id);
		}

	   
		

		public componenteselectronicos_tipos Actualizar(int id, string Descripcion = null)
		{

			var componenteelectronicotipo = _db.componenteselectronicos_tipos.Find(id);

			if (componenteelectronicotipo == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			
			if (Descripcion != null)
			{

                componenteelectronicotipo.descripcion = Descripcion;
			}
			

			_db.Entry(componenteelectronicotipo).State = EntityState.Modified;
			_db.SaveChanges();

			return componenteelectronicotipo;
		}


		public componenteselectronicos_tipos Crear(string Descripcion = null)
		{

			try
			{
				var componenteelectronicotipo = new componenteselectronicos_tipos()
				{

					descripcion = Descripcion

				};

				_db.componenteselectronicos_tipos.Add(componenteelectronicotipo);
				_db.SaveChanges();

                return componenteelectronicotipo;

            }
			catch (Exception)
			{
			   

				throw;
			}


		}

        public void Eliminar(int id)
        {

            var componenteelectronico_tipos = Find(id);

           
            _db.componenteselectronicos_tipos.Remove(componenteelectronico_tipos);
            _db.SaveChanges();
        }
    }
}
