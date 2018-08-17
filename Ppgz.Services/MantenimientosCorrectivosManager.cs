using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class MantenimientosCorrectivosManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();


        public List<equipo_tipo> FindTipos()
        {
            return _db.equipo_tipo.ToList();
        }

		public List<mantenimientocorrectivo> FindAll()
		{
			return _db.mantenimientocorrectivo.ToList();
		}

	
		public mantenimientocorrectivo Find(int id)
		{
			return _db.mantenimientocorrectivo.FirstOrDefault(e => e.Id ==id);
		}

        public mantenimientocorrectivo GetMantenimientoCorrectivo(int id)
        {
            return _db.mantenimientocorrectivo.FirstOrDefault(s => s.IdVenta == id);
        }

        public List<mantenimientocorrectivo> GetMantenimientosCorrectivos(int ventaId)
		{
			return _db.mantenimientocorrectivo.Where(s => s.IdVenta == ventaId).ToList();

		}

		public mantenimientocorrectivo Actualizar(int idventa, int id, string Tipo = null, string Descripcion = null, string PersonaJuridica = null,
			DateTime? FechaVisita = null, DateTime? FechaPublicacion = null, string NombreDocumento = null, string Tecnico = null, string EquiposAtendidos = null
			, string StatusMantenimiento = null, string Archivo = null, string Evaluacion = null, string HoraLlegada = null
			, string HoraSalida = null)
		{

			var mantenimientocorrectivo = _db.mantenimientocorrectivo.Find(id);

           // if (idventa != 0)
            //{
                mantenimientocorrectivo.IdVenta = idventa;
           // }
            
			//if (mantenimientocorrectivo == null)
			//{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			//}
			//if (Tipo != null)
			//{

				mantenimientocorrectivo.Tipo = Tipo;
			//}
			//if (Descripcion != null)
			//{

				mantenimientocorrectivo.Descripcion = Descripcion;
			//}
			//if (PersonaJuridica != null)
			//{

				mantenimientocorrectivo.PersonaJuridica = PersonaJuridica;
			//}
			//if (FechaVisita != null)
			//

				mantenimientocorrectivo.FechaVisita = FechaVisita;
			//}
			//if (FechaPublicacion != null)
			//{

				mantenimientocorrectivo.FechaPublicacion = FechaPublicacion;
			//}
			//if (NombreDocumento != null)
			//{

				mantenimientocorrectivo.NombreDocumento = NombreDocumento;
			//}
			//if (Tecnico != null)
			//{

				mantenimientocorrectivo.Tecnico = Tecnico;
			//}

			//if (EquiposAtendidos != null)
			//{

				mantenimientocorrectivo.EquiposAtendidos = EquiposAtendidos;
			//}


			//if (StatusMantenimiento != null)
			//{

				mantenimientocorrectivo.StatusMantenimiento = StatusMantenimiento;
			//}
			//if (Archivo != null)
			//{

				mantenimientocorrectivo.Archivo = Archivo;
			//}
			//if (Evaluacion != null)
			//{

				mantenimientocorrectivo.Evaluacion = Evaluacion;
			//}
			//if (HoraLlegada != null)
			//{

				mantenimientocorrectivo.HoraLlegada = HoraLlegada;
			//}
			////if (HoraSalida != null)
			//{

				mantenimientocorrectivo.HoraSalida = HoraSalida;
			//}
			

			_db.Entry(mantenimientocorrectivo).State = EntityState.Modified;
			_db.SaveChanges();

			return mantenimientocorrectivo;
		}


		public mantenimientocorrectivo Crear(int idventa, string Tipo = null, string Descripcion = null, string PersonaJuridica = null,
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
				var mantenimientocorrectivo = new mantenimientocorrectivo()
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
                    IdVenta = idventa

				};

				_db.mantenimientocorrectivo.Add(mantenimientocorrectivo);
				_db.SaveChanges();

                return mantenimientocorrectivo;

            }
			catch (Exception)
			{
			   

				throw;
			}


		}
        public void Eliminar(int id)
        {

            var mantenimientocorrectivo = Find(id);

            // componenteelectrico.cuentasmensajes.Clear();

            _db.mantenimientocorrectivo.Remove(mantenimientocorrectivo);
            _db.SaveChanges();
        }

    }
}
