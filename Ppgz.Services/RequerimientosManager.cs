using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class RequerimientosManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();
       
        public List<requerimientos_status> FindStatus()
        {
            requerimientos_status eq = new requerimientos_status();

            //eq = _db.requerimientos_status.Where(e => e.tipo == "envio").FirstOrDefault();

            return _db.requerimientos_status.ToList();
        }
      

        public List<obras> FindObras()
        {
           // return _db.obras.ToList();
            return _db.obras.Where(c => c.oficina != "SI").ToList();
        }
        public List<gerenciaresponsable> FindGerencias()
        {
            return _db.gerenciaresponsable.ToList();

        }
        public List<requerimientos_tipo> FindTipos()
        {
            return _db.requerimientos_tipo.ToList();
        }

        public List<requerimientos> GetHistorico(int obraId)
        {
            return _db.requerimientos.Where(f => f.obra_id == obraId && f.StatusRequerimiento == "REQUERIMIENTO EJECUTADO").ToList();

        }
        public List<requerimientos> GetHistoricoAll()
        {
            return _db.requerimientos.Where(f => f.StatusRequerimiento == "REQUERIMIENTO EJECUTADO").ToList();

        }

        public List<requerimientos> FindAll()
		{
			return _db.requerimientos.ToList();
		}

	
		public requerimientos Find(int id)
		{
			return _db.requerimientos.FirstOrDefault(r => r.Id ==id);
		}
        public List<requerimientos> GetRequerimientos()
        {
            //return _db.requerimientos.Where(r => r.obra_id == obraId).ToList();
            return _db.requerimientos.Where(f => f.StatusRequerimiento != "REQUERIMIENTO EJECUTADO").ToList();

        }

        public List<requerimientos> GetRequerimientos(int obraId)
		{
			//return _db.requerimientos.Where(r => r.obra_id == obraId).ToList();
            return _db.requerimientos.Where(f => f.obra_id == obraId && f.StatusRequerimiento != "REQUERIMIENTO EJECUTADO").ToList();

        }

	
		public void Actualizar(DateTime fechasolicitud, DateTime? fechacumplimientosolicitud, int? obraid, int id, string obra = null, string tipo = null, 
			string gerenciaresponsable = null, string tecnicos = null, string statusrequerimiento = null, string descripcion = null
			, string accionesejecutadas = null)
		{

			var requerimiento = _db.requerimientos.Find(id);

            requerimiento.obra_id = obraid;

            if (requerimiento == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			//if (obra != null)
			//{
				// ValidarNombreApellido(nombre);
				requerimiento.Obra = obra;
			//}
			//if (tipo != null)
			//{
			  //  ValidarNombreApellido(apellido);
				requerimiento.Tipo = tipo;
			//}
			//if (fechasolicitud != null)
			//{
			   // ValidarEmail(email);
				requerimiento.FechaSolicitud = fechasolicitud;

			//}
			//if (gerenciaresponsable != null)
				requerimiento.GerenciaResponsable= gerenciaresponsable;

			//if (tecnicos != null)
				requerimiento.Tecnicos = tecnicos;
			//if (statusrequerimiento != null)
				requerimiento.StatusRequerimiento = statusrequerimiento;

			//if (fechacumplimientosolicitud != null)
				requerimiento.FechaCumplimientoSolicitud = fechacumplimientosolicitud;
			//if (descripcion != null)
				requerimiento.Descripcion = descripcion;

			//if (accionesejecutadas != null)
				requerimiento.AccionesEjecutadas = accionesejecutadas;

			_db.Entry(requerimiento).State = EntityState.Modified;
			_db.SaveChanges();

			
		}

	

		public requerimientos Crear(DateTime fechasolicitud, DateTime? fechacumplimientosolicitud, int? obraid, string obra = null, string tipo = null, 
			string gerenciaresponsable = null, string tecnicos = null, string statusrequerimiento = null,  string descripcion = null
			, string accionesejecutadas = null)
		{
		   

	        //if(obra == null)
         //   {
         //       obra = "null";
         //   }if(tipo == null)
         //   {
         //       tipo = "null";
         //   }if(fechasolicitud == null)
         //   {
         //       fechasolicitud = DateTime.Now.ToString();
         //   }
         //   if (gerenciaresponsable == null)
         //   {
         //       gerenciaresponsable = "null";
         //   }
         //   if (tecnicos == null){
         //       tecnicos = "null";
         //   }
         //   if (statusrequerimiento == null)
         //   {
         //       statusrequerimiento = "null";
         //   }
         //   if (fechacumplimientosolicitud == null) {
         //       fechacumplimientosolicitud = DateTime.Now.ToString();
         //   }
         //   if (descripcion == null)
         //   {
         //       descripcion = "null";
         //   }
         //   if (accionesejecutadas == null)
         //   {
         //       accionesejecutadas = "null";
         //   }
			try
			{
				var requerimiento = new requerimientos()
				{

					Obra = obra,
					Tipo = tipo,
					FechaSolicitud = fechasolicitud,                    
					GerenciaResponsable = gerenciaresponsable,
					Tecnicos = tecnicos,
					StatusRequerimiento = statusrequerimiento,
					FechaCumplimientoSolicitud = fechacumplimientosolicitud,
					Descripcion = descripcion,
					AccionesEjecutadas = accionesejecutadas,
                    obra_id = obraid
				};

				_db.requerimientos.Add(requerimiento);
				_db.SaveChanges();
                return requerimiento;
			}
			catch (Exception e)
			{
			   

				throw e;
			}


		}
        public void Eliminar(int id)
        {

            var requerimiento = Find(id);

            // componenteelectrico.cuentasmensajes.Clear();

            _db.requerimientos.Remove(requerimiento);
            _db.SaveChanges();
        }
    }
}
