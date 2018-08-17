using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class MantenimientosPreventivosManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();

/*
        public List<equipo_tipo> FindTipos()
        {
            return _db.equipo_tipo.ToList();
        }*/
        public List<mantenimientopreventivo> FindTipos(int id)
        {
            mantenimientopreventivo eq = new mantenimientopreventivo();

            eq = _db.mantenimientopreventivo.Where(e => e.IdVenta == id).FirstOrDefault();

            return _db.mantenimientopreventivo.ToList();

        }
        
		public List<mantenimientopreventivo> FindAll()
		{
			return _db.mantenimientopreventivo.ToList();
		}

	
		public mantenimientopreventivo Find(int id)
		{
			return _db.mantenimientopreventivo.FirstOrDefault(e => e.Id ==id);
		}

        public mantenimientopreventivo GetMantenimientoPreventivo(int id)
        {
            return _db.mantenimientopreventivo.FirstOrDefault(s => s.IdVenta == id);
        }

        public List<mantenimientopreventivo> GetMantenimientosPreventivos(int ventaId)
		{
			return _db.mantenimientopreventivo.Where(s => s.IdVenta == ventaId).ToList();

		}

		public mantenimientopreventivo Actualizar(int idventa, int id , string Tipo = null, string Descripcion = null, string PersonaJuridica = null,
			DateTime? FechaVisita = null, DateTime? FechaPublicacion = null, string NombreDocumento = null, string Tecnico = null, string EquiposAtendidos = null
			, string StatusMantenimiento = null, string Archivo = null, string Evaluacion = null, string HoraLlegada = null
			, string HoraSalida = null)
		{

			var mantenimientopreventivo = _db.mantenimientopreventivo.Find(id);

            if(idventa != 0) { 
                mantenimientopreventivo.IdVenta = idventa;
            }
            if (mantenimientopreventivo == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			if (Tipo != null)
			{

				mantenimientopreventivo.Tipo = Tipo;
			}
			if (Descripcion != null)
			{

				mantenimientopreventivo.Descripcion = Descripcion;
			}
			if (PersonaJuridica != null)
			{

				mantenimientopreventivo.PersonaJuridica = PersonaJuridica;
			}
			if (FechaVisita != null)
			{

				mantenimientopreventivo.FechaVisita = FechaVisita;
			}
			if (FechaPublicacion != null)
			{

				mantenimientopreventivo.FechaPublicacion = FechaPublicacion;
			}
			if (NombreDocumento != null)
			{

				mantenimientopreventivo.NombreDocumento = NombreDocumento;
			}
			if (Tecnico != null)
			{

				mantenimientopreventivo.Tecnico = Tecnico;
			}

			if (EquiposAtendidos != null)
			{

				mantenimientopreventivo.EquiposAtendidos = EquiposAtendidos;
			}


			if (StatusMantenimiento != null)
			{

				mantenimientopreventivo.StatusMantenimiento = StatusMantenimiento;
			}
			if (Archivo != null)
			{

				mantenimientopreventivo.Archivo = Archivo;
			}
			if (Evaluacion != null)
			{

				mantenimientopreventivo.Evaluacion = Evaluacion;
			}
			if (HoraLlegada != null)
			{

				mantenimientopreventivo.HoraLlegada = HoraLlegada;
			}
			if (HoraSalida != null)
			{

				mantenimientopreventivo.HoraSalida = HoraSalida;
			}
			

			_db.Entry(mantenimientopreventivo).State = EntityState.Modified;
			_db.SaveChanges();

			return mantenimientopreventivo;
		}


        public mantenimientopreventivo Crear(int ventaid,string Tipo = null, string Descripcion = null, string PersonaJuridica = null,
            DateTime? FechaVisita = null, DateTime? FechaPublicacion = null, string NombreDocumento = null, string Tecnico = null, string EquiposAtendidos = null
            , string StatusMantenimiento = null, string Archivo = null, string Evaluacion = null, string HoraLlegada = null
            , string HoraSalida = null)
		{

            //if (Tipo == null)
            //{
            //    Tipo = "null";
            //}
            //if (Descripcion == null)
            //{
            //    Descripcion = "null";
            //}
            //if (PersonaJuridica == null)
            //{
            //    PersonaJuridica = "null";
            //}
            //if (NombreDocumento == null)
            //{
            //    NombreDocumento = "null";
            //}
            //if (Tecnico == null)
            //{
            //    Tecnico = "null";
            //}
            //if (FechaVisita == null)
            //{
            //    FechaVisita = "22/07/2018";
            //}
            //if (FechaPublicacion == null)
            //{
            //    FechaPublicacion = "22/07/2018";
            //}
            //if (EquiposAtendidos == null)
            //{
            //    EquiposAtendidos = "null";
            //}
            //if (StatusMantenimiento == null)
            //{
            //    StatusMantenimiento = "NO CULMINADO";
            //}
            //if (Archivo == null || Archivo == "")
            //{
            //    Archivo = "null";
            //}
            //if (Evaluacion == null || Evaluacion == "")
            //{
            //    Evaluacion = "null";
            //}
            //if (HoraLlegada == null)
            //{
            //    HoraLlegada = "null";
            //}
            //if (HoraSalida == null)
            //{
            //    HoraSalida = "null";
            //}

            try
			{
				var mantenimientopreventivo = new  mantenimientopreventivo()
				{

					Tipo = Tipo,
					Descripcion = Descripcion,
					PersonaJuridica = PersonaJuridica,
					FechaVisita = FechaVisita,
					FechaPublicacion = FechaPublicacion,
					NombreDocumento = NombreDocumento,
					Tecnico = Tecnico,
					EquiposAtendidos = EquiposAtendidos,
					StatusMantenimiento = StatusMantenimiento,
					Archivo = Archivo,
					Evaluacion = Evaluacion,
					HoraLlegada = HoraLlegada,
					HoraSalida = HoraSalida,
                    IdVenta = ventaid

				};

				_db.mantenimientopreventivo.Add(mantenimientopreventivo);
				_db.SaveChanges();

                return mantenimientopreventivo;


            }
			catch (Exception)
			{
			   

				throw;
			}


		}
        public void Eliminar(int id)
        {

            var mantenimientopreventico = Find(id);

            // componenteelectrico.cuentasmensajes.Clear();

            _db.mantenimientopreventivo.Remove(mantenimientopreventico);
            _db.SaveChanges();
        }

    }
}
