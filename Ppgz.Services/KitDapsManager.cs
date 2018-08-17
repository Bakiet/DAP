using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class KitDapsManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();


        public List<obras> FindObras(int id)
        {
            equipos eq = new equipos();

            eq = _db.equipos.Where(e => e.Id == id).FirstOrDefault();

            return _db.obras.ToList();


        }

        public List<kitdap> FindAll()
		{
			return _db.kitdap.ToList();
		}

	
		public kitdap Find(int id)
		{
			return _db.kitdap.FirstOrDefault(e => e.Id ==id);
		}

	   
		public List<kitdap> GetKitDaps(int previoId)
		{
			return _db.kitdap.Where(s => s.IdPrevio == previoId).ToList();

		}

		public kitdap Actualizar(int obra_id, int id, string Nombre = null, string Url = null)
		{

			var kitdap = _db.kitdap.Find(id);

			if (kitdap == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			if (Nombre != null)
			{

				kitdap.Nombre = Nombre;
			}
			if (Url != null)
			{

				kitdap.Url = Url;
			}
           

                kitdap.obra_id = obra_id;
            


            _db.Entry(kitdap).State = EntityState.Modified;
			_db.SaveChanges();

			return kitdap;
		}


		public void Crear(int previo_id, int obra_id,string Nombre = null, string Url = null)
		{
		   

	        if(Url == "")
            {
                Url = "null";
            }
			try
			{
				var kitdap = new kitdap()
				{
                    IdPrevio = previo_id,
                    obra_id = obra_id,
					Nombre = Nombre,
					Url = Url

				};

				_db.kitdap.Add(kitdap);
				_db.SaveChanges();

			}
			catch (Exception)
			{
			   

				throw;
			}


		}
        public void Eliminar(int id)
        {

            var kitdap = Find(id);

            // componenteelectrico.cuentasmensajes.Clear();

            _db.kitdap.Remove(kitdap);
            _db.SaveChanges();
        }

    }
}
