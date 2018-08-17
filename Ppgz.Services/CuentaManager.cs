using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class CuentaManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();

		/// <summary>
		/// Tipos de cuenta de acuerdo al modelo de datos
		/// </summary>
		public static class Tipo
		{
			public const string Mercaderia = "MERCADERIA";
			public const string  Servicio  = "SERVICIO";

		}

		internal void ValidarTipo(string tipo)
		{
			var tipos = new[]
			{
				Tipo.Mercaderia,
				Tipo.Servicio
			};

			if (!tipos.Contains(tipo))
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Cuenta_Tipo);
			}
		}

		internal perfiles GetPerfilMaestroByTipo(string tipo)
		{
			switch (tipo)
			{
				case Tipo.Mercaderia:
					return PerfilManager.MaestroMercaderia;
					
				case Tipo.Servicio:
					return PerfilManager.MaestroServicio;
			}

		   throw new BusinessException(CommonMensajesResource.ERROR_Perfil_Tipo);
		}

		public cuentas FindByNombre(string nombreProveedor)
		{
			return _db.cuentas.FirstOrDefault(c => c.NombreCuenta == nombreProveedor && c.Borrado == false);
		}

		public cuentas FindByResponsableUsuarioId(string id)
		{
			return _db.cuentas
				.FirstOrDefault(c => c.aspnetusers1.Any(u=>u.Id == id) && c.Borrado == false);
		}

		public List<cuentas> FinAll()
		{
			return _db.cuentas.Where(c=> c.Borrado == false).ToList();
		}

		public List<CuentaConUsuarioMaestro> FindAllWithUsuarioMaestro()
		{
			var query = (from c in _db.cuentas
				from u in c.aspnetusers1
				where u.Tipo == UsuarioManager.Tipo.MaestroProveedor
				select new CuentaConUsuarioMaestro{ Cuenta = c, UsuarioMaestro = u});

			return  query.ToList();
		} 
		public cuentas Find(int id)
		{
			return _db.cuentas.FirstOrDefault(c => c.Id ==id && c.Borrado == false );
		}
		public CuentaConUsuarioMaestro FindWithUsuarioMaestro(int id)
		{
			var query = (from c in _db.cuentas
						 from u in c.aspnetusers1
                         where u.Tipo == UsuarioManager.Tipo.MaestroProveedor
						 && c.Id == id
						 select new CuentaConUsuarioMaestro { Cuenta = c, UsuarioMaestro = u });

			return query.FirstOrDefault();
		} 
		
		public void Crear(string tipo, string nombreProveedor, string responsableLogin,
			string reponsableNombre, string reponsableApellido, string responsableCargo,
			string responsableEmail, string responableTelefono, string responsablePassword,
			bool esEspecial)
		{
			ValidarTipo(tipo);

			nombreProveedor = nombreProveedor.Trim();

			var cuenta = FindByNombre(nombreProveedor);

			// Valida si existe una cuenta con el mismo nombre
			if (cuenta != null)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Cuenta_NombreProveedorDuplicado);
			}

			var usuarioManager = new UsuarioManager();

			if (usuarioManager.FindByUsername(responsableLogin) != null)
			{
				throw new BusinessException(MensajesResource.ERROR_CuentaManager_Crear_LoginExistente);
			}

			cuenta = new cuentas
			{
				CodigoProveedor = Guid.NewGuid().ToString("D"),
				NombreCuenta = nombreProveedor,
				Activo = true,
				Tipo = tipo,
				EsEspecial = esEspecial
			};

			_db.cuentas.Add(cuenta);
			_db.SaveChanges();
			try
			{
				var usuarioMaestro = usuarioManager.CrearMaestroProveedor(
					responsableLogin,
					reponsableNombre,
					reponsableApellido,
					responsableEmail,
					responableTelefono,
					responsableCargo,
					true,
					responsablePassword,
					cuenta.Id
					);

				AsociarUsuarioEnCuenta(usuarioMaestro.Id, cuenta.Id);

			}
			catch (Exception)
			{
				// TODO SIMULAR
				Eliminar(cuenta.Id);

				var usuario = usuarioManager.FindByUsername(responsableLogin);
				if (usuario!= null)
				{
					usuarioManager.Eliminar(usuario.Id);
				}

				throw;
			}


		}

		public void AsociarUsuarioEnCuenta(string usuarioId, int cuentaId)
		{
			// TODO PASAR LOS QUERIES A UN ARCHIVO DE RECURSO
			const string sql = @"
						INSERT INTO  cuentasusuarios (UsuarioId, CuentaId)
						VALUES ({0},{1})";
			_db.Database.ExecuteSqlCommand(sql, usuarioId, cuentaId);
			_db.SaveChanges();
		}

		public void DesAsociarUsuarioEnCuenta(string usuarioId)
		{
			// TODO PASAR LOS QUERIES A UN ARCHIVO DE RECURSO O STORE PROCEDURE
			const string sql = @"
						DELETE FROM cuentasusuarios 
						WHERE  UsuarioId = {0}";
			_db.Database.ExecuteSqlCommand(sql, usuarioId);
			_db.SaveChanges();
		}

		//TODO
		public void Eliminar(int id)
		{

			var cuenta = _db.cuentas.Find(id);

			
			var messageError = new MySqlParameter
			{
				ParameterName = "messageError",
				Direction = ParameterDirection.Output,
				MySqlDbType = MySqlDbType.VarChar
			};

			var parameters = new List<MySqlParameter>()
			{
				new MySqlParameter
				{
					ParameterName = "messageError",
					Direction = ParameterDirection.Output,
					MySqlDbType = MySqlDbType.VarChar
				},
				new MySqlParameter("p_cuentaId", id)
			};

			Db.ExecuteProcedureOut(parameters, "delete_account");
		   
			if (messageError.Value != null)
			{
				throw new BusinessException(messageError.Value.ToString());
			}
  
		}


		


	

		public void EstablecerCuentaEspecial(int cuentaId, bool esEspecial)
		{
			var cuenta = _db.cuentas.Find(cuentaId);

			if (cuenta == null)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Cuenta_Id);
			}
			cuenta.EsEspecial = esEspecial;
			_db.Entry(cuenta).State = EntityState.Modified;
			_db.SaveChanges();
		}
	}
}
