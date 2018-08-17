using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class PerfilManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();
		/// <summary>
		/// Tipos de perfil de acuerdo al modelo de datos
		/// </summary>
		public static class TipoPerfil
		{
			public static string Nazan { get { return "DAP"; } }
			public static string Proveedor { get { return "PROVEEDOR"; } }
		}

		/// <summary>
		/// Tipos de Roles de acuerdo al modelo de datos
		/// </summary>
		public static class TipoRole
		{
			public const string Nazan = "DAP";
			public const string Mercaderia = "MERCADERIA";
			public const string Servicio = "SERVICIO";
		}

		/// <summary>
		/// Estos perfiles se crean al incio del sistema y siempre estaran disponibles en la base de datos
		/// </summary>
		#region Prefilesmaestros
		public static perfiles MaestroMercaderia
		{
			get
			{
				var db = new EntitiesDap();
				var perfil = db.perfiles.FirstOrDefault(p => p.Nombre == "MAESTRO-MERCADERIA");
				if (perfil != null) return perfil;

				var role = db.aspnetroles.Find("MAESTRO-MERCADERIA");

				perfil = new perfiles()
				{
					Nombre = "MAESTRO-MERCADERIA",
					Tipo = TipoPerfil.Proveedor
				};
				perfil.aspnetroles.Add(role);
				db.perfiles.Add(perfil);
				db.SaveChanges();
				return perfil;
			}
		}
		public static perfiles MaestroServicio
		{
			get
			{
				var db = new EntitiesDap();
				var perfil = db.perfiles.FirstOrDefault(p => p.Nombre == "MAESTRO-SERVICIO");
				if (perfil != null) return perfil;

				var role = db.aspnetroles.Find("MAESTRO-SERVICIO");

				perfil = new perfiles()
				{
					Nombre = "MAESTRO-SERVICIO",
					Tipo = TipoPerfil.Proveedor
				};
				perfil.aspnetroles.Add(role);
				db.perfiles.Add(perfil);
				db.SaveChanges();
				return perfil;
			}
		}
		// PREFIL CREADO POR EL SISTEMA AL INICIO
		public static perfiles MaestroNazan
		{
			get
			{
				var db = new EntitiesDap();
				var perfil = db.perfiles.FirstOrDefault(p => p.Nombre == "MAESTRO-ADMINISTRADOR");

				if (perfil != null) return perfil;
				var role = db.aspnetroles.Find("MAESTRO-NAZAN");

				perfil = new perfiles()
				{
					Nombre = "MAESTRO-ADMINISTRADOR",
					Tipo = TipoPerfil.Nazan
				};
				perfil.aspnetroles.Add(role);
				db.perfiles.Add(perfil);
				db.SaveChanges();
				return perfil;
			}
		}
		#endregion

		public perfiles Find(int id)
		{
			return _db.perfiles.FirstOrDefault(p => p.Id == id);
		}

		public List<perfiles> FindPerfilProveedorByCuentaId(int cuentaId)
		{
			return _db.perfiles.Where(p => p.CuentaId == cuentaId).ToList();
		}
		public List<perfiles> FindPerfilesNazan()
		{
			return _db.perfiles.Where(p => p.Tipo == TipoPerfil.Nazan).ToList();
		}
		public perfiles FindPerfilProveedorByNombreAndCuentaId(string nombrePerfil, int? cuentaId)
		{
			return _db.perfiles
				.FirstOrDefault(p => p.Nombre == nombrePerfil && p.Tipo == TipoPerfil.Proveedor && p.CuentaId == cuentaId);
		}
		public perfiles FindPerfilNazan(int id)
		{
			return _db.perfiles
				.FirstOrDefault(p => p.Id == id && p.Tipo == TipoPerfil.Nazan);
		}

		internal perfiles Crear(string tipo, string nombre, string[] rolesIds, int? cuentaId = null)
		{
			var tipos = new[]
			{
				TipoPerfil.Nazan,
				TipoPerfil.Proveedor
			};

			if (!tipos.Contains(tipo))
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_Tipo);
			}

			// TODO VALIDACIONES
			nombre = nombre.Trim();

			if (!rolesIds.Any())
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_AccesosRequeridos);
			}
			var aspnetroles =
				_db.aspnetroles.Where(r => rolesIds.Contains(r.Name));

			if (!aspnetroles.Any())
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_AccesosRequeridos);
			}

			if (cuentaId != null)
			{
				if (_db.cuentas.Find((int)cuentaId) == null)
				{
					throw new BusinessException(CommonMensajesResource.ERROR_Cuenta_Id);
				}
			}

			var perfil = new perfiles { Nombre = nombre, Tipo = tipo, CuentaId = cuentaId };

			foreach (var role in aspnetroles)
			{
				perfil.aspnetroles.Add(role);
			}

			_db.perfiles.Add(perfil);
			_db.SaveChanges();

			return perfil;
		}
		internal void Actualizar(int id, string nombre, string[] rolesIds)
		{
			var perfil = _db.perfiles.Find(id);

			if (perfil == null)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_Id);
			}

			// TODO VALIDACIONES
			nombre = nombre.Trim();

			if (!rolesIds.Any())
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_AccesosRequeridos);
			}
			var aspnetroles =
				_db.aspnetroles.Where(r => rolesIds.Contains(r.Name));

			if (!aspnetroles.Any())
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_AccesosRequeridos);
			}

			perfil.Nombre = nombre;
			perfil.aspnetroles.Clear();

			foreach (var role in aspnetroles)
			{
				perfil.aspnetroles.Add(role);
			}

			_db.Entry(perfil).State = EntityState.Modified;
			_db.SaveChanges();

			ActualizarPermisosByPefilId(id);
		}

		internal void ValidarRolesMercaderia(string[] rolesIds)
		{
			var rolesMercaderia = GetRolesMercaderia().Select(r2 => r2.Id).ToArray();

			if (rolesIds.Intersect(rolesMercaderia).Count() != rolesIds.Length)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_AccesosIncorrectos);
			}

		}

		internal void ValidarRolesServicio(string[] rolesIds)
		{
			var rolesMercaderia = GetRolesServicio().Select(r2 => r2.Id).ToArray();

			if (rolesIds.Intersect(rolesMercaderia).Count() != rolesIds.Length)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_AccesosIncorrectos);
			}

		}
		public perfiles CrearProveedor(string nombre, string[] rolesIds, int cuentaId)
		{
			var perfil = _db.perfiles.FirstOrDefault(p => p.Nombre == nombre && p.CuentaId == cuentaId);

			if (perfil != null)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_NombreExistente);
			}

			var cuenta = _db.cuentas.Find(cuentaId);
			if (cuenta == null)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Cuenta_Id);
			}
			if (cuenta.Tipo == CuentaManager.Tipo.Mercaderia)
			{
				ValidarRolesMercaderia(rolesIds);
			}
			if (cuenta.Tipo == CuentaManager.Tipo.Servicio)
			{
				ValidarRolesServicio(rolesIds);
			}

			return Crear(TipoPerfil.Proveedor, nombre, rolesIds, cuentaId);
		}
		public void ActualizarProveedor(int id, string nombre, string[] rolesIds)
		{
			var perfil = Find(id);
			if (perfil == null)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_Id);
			}

			if (perfil.cuentas == null)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Cuenta_Id);
			}
			if (perfil.Tipo != TipoPerfil.Proveedor)
			{
				throw new Exception(CommonMensajesResource.ERROR_Perfil_Tipo);
			}


			if (_db.perfiles.FirstOrDefault(
				p => p.Nombre == nombre
					 && p.CuentaId == p.cuentas.Id
					 && p.Id != id) != null)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_NombreExistente);
			}


			if (perfil.cuentas.Tipo == CuentaManager.Tipo.Mercaderia)
			{
				ValidarRolesMercaderia(rolesIds);
			}
			if (perfil.cuentas.Tipo == CuentaManager.Tipo.Servicio)
			{
				ValidarRolesServicio(rolesIds);
			}

			Actualizar(id, nombre, rolesIds);
		}
		internal void ValidarRolesNazan(string[] rolesIds)
		{

			var rolesNazan = GetRolesNazan().Select(r2 => r2.Id).ToArray();

			if (rolesIds.Intersect(rolesNazan).Count() != rolesIds.Length)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_AccesosIncorrectos);
			}
		}
		public perfiles CrearNazan(string nombre, string[] rolesIds)
		{
			var perfil = _db.perfiles.FirstOrDefault(p => p.Nombre == nombre && p.Tipo == TipoPerfil.Nazan);

			if (perfil != null)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_NombreExistente);
			}

			ValidarRolesNazan(rolesIds);

			return Crear(TipoPerfil.Nazan, nombre, rolesIds);
		}
		public void ActualizarNazan(int id, string nombre, string[] rolesIds)
		{
			var perfil =
				_db.perfiles
				.FirstOrDefault(p => p.Nombre == nombre && p.Tipo == TipoPerfil.Nazan && p.Id != id);

			if (perfil != null)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_NombreExistente);
			}

			ValidarRolesNazan(rolesIds);

			Actualizar(id, nombre, rolesIds);
		}

		internal void ActualizarPermisosByPefilId(int perfilId)
		{
			// TODO PASAR A UN PROCEDURE    
			const string sql = @"

						DELETE FROM AspNetUserRoles 
						WHERE UserId IN (SELECT Id FROM AspNetUsers WHERE PerfilId = {0});
						INSERT INTO AspNetUserRoles 
							(UserId, RoleId)
						SELECT u.Id, r.RoleId
						FROM   AspNetUsers u
						JOIN   perfilesroles r ON r.PerfilId = {0}
						WHERE  u.PerfIlId = {0};";

			_db.Database.ExecuteSqlCommand(sql, perfilId);
			_db.SaveChanges();

		}

		// TODO CREARE UN ELIMINAR PERFIL CON LA CUENTA
		public void Eliminar(int id)
		{
			var perfil = Find(id);

			if (perfil == null)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_Id);
			}

			if (perfil.Id == MaestroNazan.Id
				|| perfil.Id == MaestroServicio.Id
				|| perfil.Id == MaestroNazan.Id)
			{
				throw new BusinessException(CommonMensajesResource.ERROR_Perfil_EliminarMaestro);
			}

			if (perfil.aspnetusers.Any())
			{
				throw new BusinessException(CommonMensajesResource.ERROR_PerfilProveedor_EliminarConUsuarios);
			}

			perfil.aspnetroles.Clear();
			_db.perfiles.Remove(perfil);
			_db.SaveChanges();
		}

		#region Roles
		public List<aspnetroles> GetRolesMercaderia()
		{
			return _db.aspnetroles
                .Where(r => r.Tipo == TipoRole.Mercaderia).ToList();
		}
		public List<aspnetroles> GetRolesServicio()
		{
			return _db.aspnetroles
                .Where(r => r.Tipo == TipoRole.Servicio).ToList();
		}
		public List<aspnetroles> GetRolesNazan()
		{
			return _db.aspnetroles
                .Where(r => r.Tipo == TipoRole.Nazan).ToList();
		}
		#endregion
	}
}
