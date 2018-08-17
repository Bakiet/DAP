using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class FallasManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();


		public List<obras> FindObras()
		{
			//return _db.obras.ToList();
            return _db.obras.Where(c => c.oficina != "SI").ToList();
        }

        public obras FindIdObra(string obra)
        {
            obras ob = new obras();
            ob = _db.obras.Where(o => o.Nombre == obra).FirstOrDefault();
            return ob;
        }

        public List<equipos> FindEquiposPorObra(int id)
		{
			obras ob = new obras();
            fallas fa = new fallas();

            //fa = _db.fallas.Where(f => f.Id == id).FirstOrDefault();

            //ob = _db.obras.Where(o => o.Nombre == fa.Obra).FirstOrDefault();

            //if (ob != null) { 
			    return _db.equipos.Where(o => o.obra_id == id).ToList();
            //}
           // else
            //{
               // return null;
            //}
        }

        public List<equipos> FindEquipos()
        {
            
            return _db.equipos.ToList();
        }
        public List<equipo_tipo> FindEquiposTipo()
        {

            return _db.equipo_tipo.ToList();
        }

        public List<fallas_tipo> FindTipoFallas()
		{
			return _db.fallas_tipo.ToList();
		}

        public List<componenteselectronicos_tipos> FindComponentes()
        { 
            return _db.componenteselectronicos_tipos.ToList();
        }

        public List<componenteselectronicos_tipos> FindComponentesElectronicos()
		{
            return _db.componenteselectronicos_tipos.ToList();
        }

		public List<componentesmecanicos_tipos> FindComponentesMecanicos()
		{
			return _db.componentesmecanicos_tipos.ToList();
		}
		public List<fallas_status> FindStatus()
		{
			return _db.fallas_status.ToList();
		}
        /*public List<condicion> FindCondicion()
        {
            return _db.condicion.ToList();
        }*/

        public List<fallas> FindAll()
		{
			return _db.fallas.ToList();
		}

	
		public fallas Find(int id)
		{
			return _db.fallas.FirstOrDefault(f => f.Id ==id);
		}
        public List<fallas> GetFallas()
        {
            return _db.fallas.Where(f => f.StatusFalla != "FALLA SOLUCIONADA").ToList();

        }

        public List<fallas> GetFallas(int obraId)
		{
			return _db.fallas.Where(f => f.obra_id == obraId && f.StatusFalla != "FALLA SOLUCIONADA").ToList();

		}

        public List<fallas> GetHistorico(int obraId)
        {
            return _db.fallas.Where(f => f.obra_id == obraId && f.StatusFalla == "FALLA SOLUCIONADA").ToList();

        }
        public List<fallas> GetHistoricoAll()
        {
            return _db.fallas.Where(f => f.StatusFalla == "FALLA SOLUCIONADA").ToList();

        }

        public fallas Actualizar(int id, DateTime? fechafalla, DateTime? fechaSolucion, string obra = null, string equipo = null,
			string tipo = null, string componente = null, string personal = null, string statusFalla = null, string numeroReporte = null, string descripcion = null, string condicion = null, string accionesTomadas = null, string accionesRecomendadas = null, string duracion = null
			, string personaReporte = null, string gerenciaResponsable = null)
		{

			var fallas = _db.fallas.Find(id);

			if (fallas == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			if (fechafalla != null)
			{
				// ValidarNombreApellido(nombre);
				fallas.FechaFalla = fechafalla;
			}
			if (obra != null)
			{
				//  ValidarNombreApellido(apellido);
				fallas.Obra = obra;
			}
			if (equipo != null)
			{
				// ValidarEmail(email);
				fallas.Equipo = equipo;
			}
			if (tipo != null)
			{
				//  ValidarNombreApellido(apellido);
				fallas.Tipo = tipo;
			}
			if (componente != null)
			{
				//  ValidarNombreApellido(apellido);
				fallas.Componente = componente;
			}
			if (personal != null)
			{
				//  ValidarNombreApellido(apellido);
				fallas.Personal = personal;
			}
			if (statusFalla != null)
			{
				//  ValidarNombreApellido(apellido);
				fallas.StatusFalla = statusFalla;
			}
			if (fechaSolucion != null)
			{
				//  ValidarNombreApellido(apellido);
				fallas.FechaSolucion = fechaSolucion;
			}
			if (numeroReporte != null)
			{
				//  ValidarNombreApellido(apellido);
				fallas.NumeroReporte = numeroReporte;
			}
			if (descripcion != null)
			{
				//  ValidarNombreApellido(apellido);
				fallas.Descripcion = descripcion;
			}
			if (condicion != null)
			{
				//  ValidarNombreApellido(apellido);
				fallas.Condicion = condicion;
			}
			if (accionesTomadas != null)
			{
				//  ValidarNombreApellido(apellido);
				fallas.AccionesTomadas = accionesTomadas;
			}
			if (duracion != null)
			{
				//  ValidarNombreApellido(apellido);
				fallas.Duracion = duracion;
			}
			if (personaReporte != null)
			{
				//  ValidarNombreApellido(apellido);
				fallas.PersonaReporte = personaReporte;
			}
			if (gerenciaResponsable != null)
			{
				//  ValidarNombreApellido(apellido);
				fallas.GerenciaResponsable = gerenciaResponsable;
			}
			
			

			if (fechafalla != null)
				fallas.FechaFalla = fechafalla;

			if (obra != null)
				fallas.Obra = obra;

			if (equipo != null)
				fallas.Equipo = equipo;

			if (tipo != null)
				fallas.Tipo = tipo;

			if (componente != null)
				fallas.Componente = componente;

			if (personal != null)
				fallas.Personal = personal;

			if (statusFalla != null)
				fallas.StatusFalla = statusFalla;

			if (fechaSolucion != null)
				fallas.FechaSolucion = fechaSolucion;

			if (numeroReporte != null)
				fallas.NumeroReporte = numeroReporte;

			if (descripcion != null)
				fallas.Descripcion = descripcion;

			if (condicion != null)
				fallas.Condicion = condicion;

			if (accionesTomadas != null)
				fallas.AccionesTomadas = accionesTomadas;

			if (duracion != null)
				fallas.Duracion = duracion;

			if (personaReporte != null)
				fallas.PersonaReporte = personaReporte;

			if (gerenciaResponsable != null)
				fallas.PersonaReporte = gerenciaResponsable;


			_db.Entry(fallas).State = EntityState.Modified;
			_db.SaveChanges();

			return fallas;
		}


		public fallas Crear(int obraid, DateTime? fechafalla, DateTime? fechaSolucion, string obra = null, string equipo = null,
			string tipo = null, string componente = null, string personal = null, string statusFalla = null, string numeroReporte = null, string descripcion = null, string condicion = null, string accionesTomadas = null, string accionesRecomendadas = null, string duracion = null
			, string personaReporte = null, string gerenciaResponsable = null)
		{
		   
            //if(fechafalla == null)
            //{
            //    fechafalla = DateTime.Now;
            //}
         //   if(obra == null)
         //   {
         //       obra = "null";
         //   }
         //   if(equipo == null)
         //   {
         //       equipo = "null";
         //   }
         //   if(tipo == null)
         //   {
         //       tipo = "null";
         //   }
         //   if(componente == null)
         //   {
         //       componente = "null";
         //   }
         //   if(personal == null)
         //   {
         //       personal = "null";
         //   }
         //   if(statusFalla == null)
         //   {

         //       statusFalla = "null";
         //   }
         //   //if(fechaSolucion == null)
         //   //{
         //   //    fechaSolucion = DateTime.Now.ToString();
         //   //}
	        //if(numeroReporte == null)
         //   {
         //       numeroReporte = "null";
         //   }
         //   if(descripcion == null)
         //   {
         //       descripcion = "null";
         //   }
         //   if(condicion == null)
         //   {
         //       condicion = "null";
         //   }
         //   if(accionesTomadas == null)
         //   {
         //       accionesTomadas = "null";
         //   }
         //   if(accionesRecomendadas == null)
         //   {
         //       accionesRecomendadas = "null";
         //   }
         //   if(duracion == null)
         //   {
         //       duracion = "null";
         //   }
         //   if(personaReporte == null)
         //   {
         //       personaReporte = "null";
         //   }
         //   if(gerenciaResponsable == null)
         //   {
         //       gerenciaResponsable = "null";
         //   }
			try
			{
				var falla = new fallas()
				{

					FechaFalla = fechafalla,
					Obra = obra,
					Equipo = equipo,
					Tipo = tipo,
					Componente = componente,
					Personal = personal,
					StatusFalla = statusFalla,
					FechaSolucion = fechaSolucion,
					NumeroReporte = numeroReporte,
					Descripcion = descripcion,
					Condicion = condicion,
					AccionesTomadas = accionesTomadas,
					AccionesRecomendadas = accionesRecomendadas,
					Duracion = duracion,
					PersonaReporte = personaReporte,
					GerenciaResponsable = gerenciaResponsable,
                    obra_id = obraid
				};

				_db.fallas.Add(falla);
				_db.SaveChanges();

                return falla;
			}
			catch (Exception)
			{
			   

				throw;
			}


		}



		//TODO
		public void Eliminar(int id)
		{

			var falla = Find(id);

			// componenteelectrico.cuentasmensajes.Clear();

			_db.fallas.Remove(falla);
			_db.SaveChanges();
		}



	}
}
