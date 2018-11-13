using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class ComponentesMecanicosManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();

        public List<componentesmecanicos> GetHistorico(int Id)
        {
            return _db.componentesmecanicos.Where(f => f.IdEquipos == Id && f.Sustitucion == "SI").ToList();

        }
        public List<componentesmecanicos> GetSustituciones()
        {
            var lastmonth = DateTime.Today;
            return _db.componentesmecanicos.Where(f => f.FechaAlerta <= lastmonth).ToList();

        }

        public List<componentesmecanicos_tipos> FindTipos()
        {
            return _db.componentesmecanicos_tipos.ToList();
        }

        public List<componentesmecanicos> FindAll()
		{
			return _db.componentesmecanicos.ToList();
		}

	
		public componentesmecanicos Find(int id)
		{
			return _db.componentesmecanicos.FirstOrDefault(e => e.Id ==id);
		}

	   
		public List<componentesmecanicos> GetComponentesMecanicos(int equipoId)
		{
			return _db.componentesmecanicos.Where(s => s.IdEquipos == equipoId && s.Sustitucion == "NO").ToList();

		}

		public componentesmecanicos Actualizar(int id, string Tipo = null, string Caracteristicas = null, string Descripcion = null,
			string Marca = null, string Modelo = null, string Serial = null, string FechaFabricado = null, string Duracion = null
			, string Sustitucion = null, string Fotografia = null, string FechaSustitucion = null, string FechaAlerta = null,string obra =null, string equipo =null)
		{

			var componentemecanico = _db.componentesmecanicos.Find(id);

			if (componentemecanico == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
          //  if (Tipo != null)
          //  {

                componentemecanico.Tipo = Tipo;
          //  }
           // if (Descripcion != null)
           // {

                componentemecanico.Descripcion = Descripcion;
           // }
           // if (Caracteristicas != null)
           // {

                componentemecanico.Caracteristicas = Caracteristicas;
          //  }
            //if (Descripcion != null)
          //  {

                componentemecanico.Descripcion = Descripcion;
          //  }
          //  if (Marca != null)
           // {

                componentemecanico.Marca = Marca;
          //  }
          //  if (Modelo != null)
          //  {

                componentemecanico.Modelo = Modelo;
          //  }
          //  if (Serial != null)
           // {

                componentemecanico.Serial = Serial;
           // }

            if (FechaFabricado != null)
            {

                componentemecanico.FechaFabricado = DateTime.Parse(FechaFabricado);
            }
            if (FechaSustitucion != null)
            {
            
                componentemecanico.FechaSustitucion = DateTime.Parse(FechaSustitucion);

            
            }
            if (FechaAlerta != null)
            {
                componentemecanico.FechaAlerta = DateTime.Parse(FechaAlerta);
                //  if (Duracion != null)
                //  {
            }
            if(Duracion != null) { 
            componentemecanico.Duracion = DateTime.Parse(Duracion);
            }
            //  }
            if (Sustitucion != null)
            {

                componentemecanico.Sustitucion = Sustitucion;
            }
           // if (Fotografia != null)
           // {

                componentemecanico.Fotografia = Fotografia;
            // }

            componentemecanico.obra = obra;
            componentemecanico.equipo = equipo;
            _db.Entry(componentemecanico).State = EntityState.Modified;
			_db.SaveChanges();

			return componentemecanico;
		}


		public componentesmecanicos Crear(int equipoid,string Tipo = null, string Caracteristicas = null, string Descripcion = null,
			string Marca = null, string Modelo = null, string Serial = null, string FechaFabricado = null, string Duracion = null
			,string Sustitucion = null, string Fotografia = null, string FechaSustitucion = null, string FechaAlerta = null,string obra = null,string equipo = null)
		{
            //if(Caracteristicas == null)
            //{
            //    Caracteristicas = "null";
            //}
            //if(Descripcion == null)
            //{
            //    Descripcion = "null";
            //}
            //if(Marca == null)
            //{
            //    Marca = "null";
            //}
            //if(Modelo == null)
            //{
            //    Modelo = "null";
            //}if(Serial == null)
            //{
            //    Serial = "null";
            //}
            //if(FechaFabricado == null)
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
            DateTime? FechaFabricadoD = null;
            DateTime? FechaSustitucionD = null;
            DateTime? FechaAlertaD = null;
            DateTime? SustitucionD = null;
            DateTime? DuracionD = null;
            if (FechaFabricado != null)
            {

                FechaFabricadoD = DateTime.Parse(FechaFabricado);
            }
            if (FechaSustitucion != null)
            {

                FechaSustitucionD = DateTime.Parse(FechaSustitucion);


            }
            if (FechaAlerta != null)
            {
                FechaAlertaD = DateTime.Parse(FechaAlerta);
                //  if (Duracion != null)
                //  {
            }
            if (Duracion != null)
            {
                DuracionD = DateTime.Parse(Duracion);
            }
            //  }
           
            try
			{
				var componentemecanico = new componentesmecanicos()
				{

                    IdEquipos = equipoid,
					Tipo = Tipo,
					Caracteristicas = Caracteristicas,
					Descripcion = Descripcion,
					FechaFabricado = FechaFabricadoD,
					Marca = Marca,
					Modelo  = Modelo,
					Serial = Serial,
					Duracion = DuracionD,
					Sustitucion = Sustitucion,
					Fotografia = Fotografia,
                    FechaSustitucion = FechaSustitucionD,
                    FechaAlerta = FechaAlertaD,
                    obra = obra,
                    equipo = equipo

				};

				_db.componentesmecanicos.Add(componentemecanico);
				_db.SaveChanges();

                return componentemecanico;

            }
			catch (Exception e)
			{
			   

				throw e;
			}


		}

        public void Eliminar(int id)
        {

            var componentemecanico = Find(id);

            // componenteelectrico.cuentasmensajes.Clear();

            _db.componentesmecanicos.Remove(componentemecanico);
            _db.SaveChanges();
        }
    }
}
