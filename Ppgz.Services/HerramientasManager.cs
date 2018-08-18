using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class HerramientasManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();


		public List<herramientas> FindAll()
		{
			return _db.herramientas.ToList();
		}

	
		public herramientas Find(int id)
		{
			return _db.herramientas.FirstOrDefault(e => e.Id ==id);
		}

	   
		public List<herramientas> GetHerramientas(int obraId)
		{
			return _db.herramientas.Where(s => s.obra_id == obraId && s.FechaEntrada == null).ToList();

		}

        public List<herramientas> GetHistorico(int obraId)
        {
            return _db.herramientas.Where(f => f.obra_id == obraId && f.FechaEntrada != null).ToList();

        }

        public herramientas Actualizar(int obra_id,int id, string Descripcion = null, string Cantidad = null, DateTime? FechaSalida = null,
			string Propiedad = null, DateTime? FechaCulminacion = null, string CantidadDeposito = null, DateTime? FechaEntrada = null, string SupervisorObra = null
			, string TecnicoResponsable = null, string Observaciones = null)
		{

			var herramienta = _db.herramientas.Find(id);

			if (herramienta == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			

				herramienta.Descripcion = Descripcion;
			

				herramienta.Cantidad = int.Parse(Cantidad);
			

				herramienta.FechaSalida = FechaSalida;
			

				herramienta.Propiedad = Propiedad;
			

				herramienta.FechaCulminacion = FechaCulminacion;
			

				herramienta.CantidadDeposito = CantidadDeposito;
			

				herramienta.FechaEntrada =FechaEntrada;
			

				herramienta.SupervisorObra = SupervisorObra;
			

				herramienta.TecnicoResponsable = TecnicoResponsable;
			
				herramienta.Observaciones = Observaciones;
			
            

            herramienta.obra_id = obra_id;
			_db.Entry(herramienta).State = EntityState.Modified;
			_db.SaveChanges();

			return herramienta;
		}


		public herramientas Crear(int obra_id,string Descripcion = null, string Cantidad = null, DateTime? FechaSalida = null,
			string Propiedad = null, DateTime? FechaCulminacion = null, string CantidadDeposito = null, DateTime? FechaEntrada = null, string SupervisorObra = null
			, string TecnicoResponsable = null, string Observaciones = null)
		{
		   

	        if(Cantidad == "null")
            {
                Cantidad = "0";
            }
			try
			{
                var herramienta = new herramientas()
                {
                   
                    Descripcion = Descripcion,
                    Cantidad = int.Parse(Cantidad),
                    FechaSalida = FechaSalida,
                    Propiedad = Propiedad,
                    FechaCulminacion = FechaCulminacion,
                    CantidadDeposito = CantidadDeposito,
                    FechaEntrada = FechaEntrada,
                    SupervisorObra = SupervisorObra,
                    TecnicoResponsable = TecnicoResponsable,
                    Observaciones = Observaciones,
                    obra_id = obra_id
                   

				};

				_db.herramientas.Add(herramienta);
				_db.SaveChanges();

                return herramienta;

			}
			catch (Exception)
			{
			   

				throw;
			}


		}
        public void Eliminar(int id)
        {

            var herramienta = Find(id);

            // componenteelectrico.cuentasmensajes.Clear();

            _db.herramientas.Remove(herramienta);
            _db.SaveChanges();
        }

    }
}
