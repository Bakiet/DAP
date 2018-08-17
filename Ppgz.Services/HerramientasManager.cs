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
			return _db.herramientas.Where(s => s.obra_id == obraId).ToList();

		}

        public List<herramientas> GetHistorico(int previoId)
        {
            return _db.herramientas.Where(f => f.previo_id == previoId && f.FechaEntrada != null).ToList();

        }

        public herramientas Actualizar(int obra_id,int id, string Descripcion = null, string Cantidad = null, string FechaSalida = null,
			string Propiedad = null, string FechaCulminacion = null, string CantidadDeposito = null, string FechaEntrada = null, string SupervisorObra = null
			, string TecnicoResponsable = null, string Observaciones = null)
		{

			var herramienta = _db.herramientas.Find(id);

			if (herramienta == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			if (Descripcion != null)
			{

				herramienta.Descripcion = Descripcion;
			}
			if (Cantidad != null)
			{

				herramienta.Cantidad = int.Parse(Cantidad);
			}
			if (FechaSalida != null)
			{

				herramienta.FechaSalida = DateTime.Parse(FechaSalida);
			}
			if (Propiedad != null)
			{

				herramienta.Propiedad = Propiedad;
			}
			if (FechaCulminacion != null)
			{

				herramienta.FechaCulminacion = DateTime.Parse(FechaCulminacion);
			}
			if (CantidadDeposito != null)
			{

				herramienta.CantidadDeposito = CantidadDeposito;
			}
			if (FechaEntrada != null)
			{

				herramienta.FechaEntrada = DateTime.Parse(FechaEntrada);
			}

			if (SupervisorObra != null)
			{

				herramienta.SupervisorObra = SupervisorObra;
			}


			if (TecnicoResponsable != null)
			{

				herramienta.TecnicoResponsable = TecnicoResponsable;
			}
			if (Observaciones != null)
			{

				herramienta.Observaciones = Observaciones;
			}
            

            herramienta.obra_id = obra_id;
			_db.Entry(herramienta).State = EntityState.Modified;
			_db.SaveChanges();

			return herramienta;
		}


		public herramientas Crear(int previo_id, int obra_id,string Descripcion = null, string Cantidad = null, string FechaSalida = null,
			string Propiedad = null, string FechaCulminacion = null, string CantidadDeposito = null, string FechaEntrada = null, string SupervisorObra = null
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
                    previo_id = previo_id,
                    Descripcion = Descripcion,
                    Cantidad = int.Parse(Cantidad),
                    FechaSalida = DateTime.Parse(FechaSalida),
                    Propiedad = Propiedad,
                    FechaCulminacion = DateTime.Parse(FechaCulminacion),
                    CantidadDeposito = CantidadDeposito,
                    FechaEntrada = DateTime.Parse(FechaEntrada),
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
