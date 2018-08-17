using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class ProtocolosManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();


		public List<protocolos> FindAll()
		{
			return _db.protocolos.ToList();
		}

	
		public protocolos Find(int id)
		{
			return _db.protocolos.FirstOrDefault(e => e.Id ==id);
		}

	   
		public List<protocolos> GetProtocolos(int previoId)
		{
			return _db.protocolos.Where(s => s.IdPrevio == previoId).ToList();

		}

		public protocolos Actualizar(int obra_id,int id, string nombre = null, string url = null)
		{

			var protocolo = _db.protocolos.Find(id);

			if (protocolo == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			if (nombre != null)
			{

				protocolo.Nombre = nombre;
			}
			if (url != null)
			{

				protocolo.Url = url;
			}

            protocolo.obra_id = obra_id;
		

			_db.Entry(protocolo).State = EntityState.Modified;
			_db.SaveChanges();

			return protocolo;
		}


		public void Crear(int previo_id, int obra_id,string nombre = null, string url = null)
		{
		   

	 
			try
			{
				var protocolo = new protocolos()
				{
                    IdPrevio = previo_id,
                    obra_id = obra_id,
					Nombre = nombre,
					Url = url
				};

				_db.protocolos.Add(protocolo);
				_db.SaveChanges();

			}
			catch (Exception)
			{
			   

				throw;
			}


		}
        public void Eliminar(int id)
        {

            var protocolo = Find(id);

            // componenteelectrico.cuentasmensajes.Clear();

            _db.protocolos.Remove(protocolo);
            _db.SaveChanges();
        }

    }
}
