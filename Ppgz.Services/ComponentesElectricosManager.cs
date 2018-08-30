using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class ComponentesElectricosManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();

        public List<componenteselectricos> GetHistorico(int Id)
        {
            return _db.componenteselectricos.Where(f => f.IdEquipos == Id && f.Sustitucion == "SI").ToList();

        }
        public List<componenteselectricos> GetSustituciones()
        {
            var lastmonth = DateTime.Today;
            return _db.componenteselectricos.Where(f => f.FechaAlerta <= lastmonth).ToList();

        }
        public List<componenteselectronicos_tipos> FindTipos()
        {
            return _db.componenteselectronicos_tipos.ToList();
        }

        public List<componenteselectricos> FindAll()
		{
			return _db.componenteselectricos.ToList();
		}

	
		public componenteselectricos Find(int id)
		{
			return _db.componenteselectricos.FirstOrDefault(e => e.Id ==id);
		}

	   
		public List<componenteselectricos> GetComponentesElectricos(int equipoId)
		{
			return _db.componenteselectricos.Where(s => s.IdEquipos == equipoId && s.Sustitucion == "NO").ToList();

		}

		public componenteselectricos Actualizar(int id, string Tipo = null, string Caracteristicas = null, string Descripcion = null,
			string Marca = null, string Modelo = null, string Serial = null, DateTime? FechaFabricado = null, DateTime? Duracion = null
			, string Sustitucion = null, string Fotografia = null, DateTime? FechaSustitucion = null, DateTime? FechaAlerta = null)
		{

			var componenteelectrico = _db.componenteselectricos.Find(id);

			if (componenteelectrico == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			//if (Tipo != null)
			//{

				componenteelectrico.Tipo = Tipo;
			//}
			//if (Descripcion != null)
			//{

				componenteelectrico.Descripcion = Descripcion;
			//}
			//if (Caracteristicas != null)
			//{

				componenteelectrico.Caracteristicas = Caracteristicas;
			//}
			//if (Descripcion != null)
			//{

				componenteelectrico.Descripcion = Descripcion;
			//}
			//if (Marca != null)
			//{

				componenteelectrico.Marca = Marca;
			//}
			//if (Modelo != null)
			//{

				componenteelectrico.Modelo = Modelo;
			//}
			//if (Serial != null)
			//{

				componenteelectrico.Serial = Serial;
			//}

			//if (FechaFabricado != null)
			//{

				componenteelectrico.FechaFabricado = FechaFabricado;
			//}
           // if (FechaSustitucion != null)
            //{

                componenteelectrico.FechaSustitucion = FechaSustitucion;
            // }
            componenteelectrico.FechaAlerta = FechaAlerta;

           // if (Duracion != null)
			//{

				componenteelectrico.Duracion = Duracion;
			//}
			if (Sustitucion != null)
			{

				componenteelectrico.Sustitucion = Sustitucion;
			}
			//if (Fotografia != null)
			//{

				componenteelectrico.Fotografia = Fotografia;
			//}
			

			_db.Entry(componenteelectrico).State = EntityState.Modified;
			_db.SaveChanges();

			return componenteelectrico;
		}


		public componenteselectricos Crear(int equipoid, string Tipo = null, string Caracteristicas = null, string Descripcion = null,
			string Marca = null, string Modelo = null, string Serial = null, DateTime? FechaFabricado = null, DateTime? Duracion = null
			,string Sustitucion = null, string Fotografia = null, DateTime? FechaSustitucion = null, DateTime? FechaAlerta = null)
		{

            //if (Caracteristicas == null)
            //{
            //    Caracteristicas = "null";
            //}
            //if (Descripcion == null)
            //{
            //    Descripcion = "null";
            //}
            //if (Marca == null)
            //{
            //    Marca = "null";
            //}
            //if (Modelo == null)
            //{
            //    Modelo = "null";
            //}
            //if (Serial == null)
            //{
            //    Serial = "null";
            //}
            //if (FechaFabricado == null)
            //{
            //    FechaFabricado = "22/07/2018";
            //}
            //if (FechaSustitucion == null)
            //{
            //    FechaSustitucion = "22/07/2018";
            //}
            //if (Duracion == null)
            //{
            //    Duracion = "null";
            //}
            if (Sustitucion == null)
            {
                Sustitucion = "NO";
            }
            //if (Fotografia == null)
            //{
            //    Fotografia = "null";
            //}

            try
			{
				var componenteelectrico = new componenteselectricos()
				{
                    IdEquipos = equipoid,
                    Tipo = Tipo,
					Caracteristicas = Caracteristicas,
					Descripcion = Descripcion,
					FechaFabricado = FechaFabricado,
					Marca = Marca,
					Modelo  = Modelo,
					Serial = Serial,
					Duracion = Duracion,
					Sustitucion = Sustitucion,
					Fotografia = Fotografia,
                    FechaSustitucion = FechaSustitucion,
                    FechaAlerta = FechaAlerta

				};

				_db.componenteselectricos.Add(componenteelectrico);
				_db.SaveChanges();

                return componenteelectrico;


            }
			catch (Exception)
			{
			   

				throw;
			}


		}
        public void Eliminar(int id)
        {
           
            var componenteelectrico = Find(id);

           // componenteelectrico.cuentasmensajes.Clear();

            _db.componenteselectricos.Remove(componenteelectrico);
            _db.SaveChanges();
        }

    }
}
