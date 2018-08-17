using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class ObrasManager
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
       
        //internal void ValidarTipo(string tipo)
        //{
        //	var tipos = new[]
        //	{
        //		Tipo.Mercaderia,
        //		Tipo.Servicio
        //	};

        //	if (!tipos.Contains(tipo))
        //	{
        //		throw new BusinessException(CommonMensajesResource.ERROR_Cuenta_Tipo);
        //	}
        //}

        //internal perfiles GetPerfilMaestroByTipo(string tipo)
        //{
        //	switch (tipo)
        //	{
        //		case Tipo.Mercaderia:
        //			return PerfilManager.MaestroMercaderia;

        //		case Tipo.Servicio:
        //			return PerfilManager.MaestroServicio;
        //	}

        //   throw new BusinessException(CommonMensajesResource.ERROR_Perfil_Tipo);
        //}

        //public cuentas FindByNombre(string nombreProveedor)
        //{
        //	return _db.cuentas.FirstOrDefault(c => c.NombreCuenta == nombreProveedor && c.Borrado == false);
        //}

        //public cuentas FindByResponsableUsuarioId(string id)
        //{
        //	return _db.cuentas
        //		.FirstOrDefault(c => c.aspnetusers1.Any(u=>u.Id == id) && c.Borrado == false);
        //}

        public List<obras> FindAll()
		{
			return _db.obras.Where(c => c.oficina != "SI").ToList();
		}

        public List<obras> FindAllWithOficina()
        {
            return _db.obras.ToList();
        }

        public List<obras_contactos> FindContactosObras(int? id)
        {
            return _db.obras_contactos.Where(c => c.obra_id == id).ToList();
        }

        public List<obras> FindObras(int? id)
        {
            return _db.obras.ToList();
        }
        public obras FindObrasPorNombre(string nombre)
        {
            return _db.obras.FirstOrDefault(o => o.Nombre == nombre);
        }
        public List<archivos> FindCorreosFallas(int? id)
        {
            return _db.archivos.Where(c => c.instancia_id == id && c.caracteristica == "correo" && c.tipo == "fallas").ToList();
        }
        public List<archivos> FindCorreosRequerimientos(int? id)
        {
            return _db.archivos.Where(c => c.instancia_id == id && c.caracteristica == "correo" && c.tipo == "requerimientos").ToList();
        }
        public List<archivos> FindFotografiasObras(int? id)
        {
            return _db.archivos.Where(c => c.instancia_id == id && c.caracteristica == "fotografia" && c.tipo == "obras").ToList();
        }
        public List<archivos> FindMapasObras(int? id)
        {
            return _db.archivos.Where(c => c.instancia_id == id && c.caracteristica == "mapa" && c.tipo == "obras").ToList();
        }

        public List<archivos> FindFotografiasComponentesMecanicos(int? id)
        {
            return _db.archivos.Where(c => c.instancia_id == id && c.caracteristica == "fotografia" && c.tipo == "componentesmecanicos").ToList();
        }
       

        public List<archivos> FindFotografiasComponentesElectronicos(int? id)
        {
            return _db.archivos.Where(c => c.instancia_id == id && c.caracteristica == "fotografia" && c.tipo == "componenteselectricos").ToList();
        }
       

        public List<archivos> FindFotografiasEquipos(int? id)
        {
            return _db.archivos.Where(c => c.instancia_id == id && c.caracteristica == "fotografia" && c.tipo == "equipos").ToList();
        }

        public List<archivos> FindArchivos(int? id, string caracteristica, string tipo)
        {
            return _db.archivos.Where(c => c.instancia_id == id && c.caracteristica == caracteristica && c.tipo == tipo).ToList();
        }

        public List<archivos> FindPlanosEquipos(int? id)
        {
            return _db.archivos.Where(c => c.instancia_id == id && c.caracteristica == "plano" && c.tipo == "equipos").ToList();
        }
        //public List<CuentaConUsuarioMaestro> FindAllWithUsuarioMaestro()
        //{
        //	var query = (from c in _db.cuentas
        //		from u in c.aspnetusers1
        //				 where u.Tipo == UsuarioManager.Tipo.MaestroProveedor
        //		select new CuentaConUsuarioMaestro{ Cuenta = c, UsuarioMaestro = u});

        //	return  query.ToList();
        //} 
        public obras Find(int? id)
		{
			return _db.obras.FirstOrDefault(o => o.Id ==id);
		}
        public obras Find_by_name(string name)
        {
            return _db.obras.FirstOrDefault(o => o.Nombre == name);
        }

        public void AgregarArchivos(int instancia_id, string url = null, string tipo = null, string caracteristica = null)
        {
            try
            {
                var archivo = new archivos()
                {
                    tipo = tipo,
                    url = url,
                    caracteristica = caracteristica,
                    instancia_id = instancia_id
                };

                var id = _db.archivos.Add(archivo);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }


        }
        /// <summary>
        /// Actualizacion de los campos básico y genericos del usuario
        /// es valido para todos los tipos Proveedor, MaestroProveedor y Nazan
        /// </summary>
        public obras Actualizar(int id, string nombre = null, string personajuridica = null, string direccionfacturacion = null,
			string direccionoficina = null, string direccionobra = null, string telefonooficina = null, string contacto = null, string contacto2 = null
			, string email = null, string email2 = null, string foto = null, string mapa = null)
		{

			var obra = _db.obras.Find(id);

			if (obra == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			//if (nombre != null)
			//{
				// ValidarNombreApellido(nombre);
				obra.Nombre = nombre;
			//}
			//if (personajuridica != null)
			//{
			  //  ValidarNombreApellido(apellido);
				obra.PersonaJuridica = personajuridica;
			//}
			//if (direccionfacturacion != null)
			//{
			   // ValidarEmail(email);
				obra.DireccionFacturacion = direccionfacturacion;

			//}
			//if (direccionoficina != null)
				obra.DireccionOficina= direccionoficina;

			//if (direccionobra != null)
				obra.DireccionObra = direccionobra;
			//if (telefonooficina != null)
				obra.TelefonoOficina = telefonooficina;

			//if (contacto != null)
				obra.Contacto = contacto;
			//if (contacto2 != null)
				obra.Contacto2 = contacto2;

			//if (email != null)
				obra.Email = email;

			//if (email2 != null)
				obra.Email2 = email2;

           // if (foto != null)
                obra.foto = foto;

           // if (mapa != null)
                obra.mapa = mapa;

            _db.Entry(obra).State = EntityState.Modified;
			_db.SaveChanges();

			return obra;
		}

		//public CuentaConUsuarioMaestro FindWithUsuarioMaestro(int id)
		//{
		//    var query = (from c in _db.cuentas
		//                 from u in c.aspnetusers1
		//                 where u.Tipo == UsuarioManager.Tipo.MaestroProveedor
		//                 && c.Id == id
		//                 select new CuentaConUsuarioMaestro { Cuenta = c, UsuarioMaestro = u });

		//    return query.FirstOrDefault();
		//}

		public obras Crear(string nombre = null, string personajuridica = null, string direccionfacturacion = null,
			string direccionoficina = null, string direccionobra = null, string telefonooficina = null, string contacto = null, string contacto2 = null
			, string email = null, string email2 = null, string foto = null, string mapa = null)
		{
		   

	 
			try
			{
				var obra = new obras()
				{

					Nombre = nombre,
					PersonaJuridica = personajuridica,
					DireccionFacturacion = direccionfacturacion,
					DireccionOficina = direccionoficina,
					DireccionObra = direccionobra,
					TelefonoOficina = telefonooficina,
					Contacto = contacto,
					Contacto2 = contacto2,
					Email = email,
					Email2 = email2,
                    foto = foto,
                    mapa = mapa
				};

				var id = _db.obras.Add(obra);
				_db.SaveChanges();

				
				var ascensorprivado = new equipos()
				{

					Nombre = "ASCENSOR PRIVADO",
					obra_id = id.Id

				};
                var idequipoap = _db.equipos.Add(ascensorprivado);
				_db.SaveChanges();

                var previo = new previos()
                {
                    FechaFirmaContrato = DateTime.Now,
                    FechaPagoInicialFabrica = DateTime.Now,
                    FechaPagoInicialEquipo = DateTime.Now,
                    FechaConstruccion = DateTime.Now,
                    FechaSalidaFabrica = DateTime.Now,
                    FechaSalidaBuque = DateTime.Now,
                    FechaLlegadaBuque = DateTime.Now,
                    FechaPeriodoNacionalizacion = DateTime.Now,
                    FechaSalidaPuertoObra = DateTime.Now,
                    FechaDescargaResguardo = DateTime.Now,
                    ActivacionProtocoloGarantiaFabrica = "no",
                    FechaInicioMontaje = DateTime.Now,
                    FechaEntregaSoso = DateTime.Now,
                    FechaCulminacionMontaje = DateTime.Now,
                    FechaConfiguracion = DateTime.Now,
                    FechaPeriodoPrueba = DateTime.Now,
                    FechaInspeccion = DateTime.Now,
                    equipo_id = idequipoap.Id
                };
                _db.previos.Add(previo);
                _db.SaveChanges();

               /* var ascensorservicio = new equipos()
				{

					Nombre = "ASCENSOR DE SERVICIO",
					obra_id = id.Id

				};
                var idequipoas = _db.equipos.Add(ascensorservicio);
				_db.SaveChanges();

                var previoas = new previos()
                {
                    equipo_id = idequipoas.Id
                };
                var previo_id = _db.previos.Add(previoas);
                _db.SaveChanges();*/

                var protocolo = new protocolos()
                {
                    obra_id = id.Id,
                    IdPrevio = previo.Id
                };
                _db.protocolos.Add(protocolo);
                _db.SaveChanges();

                var herramienta = new herramientas()
                {
                    obra_id = id.Id,
                    previo_id = previo.Id
                };
                _db.herramientas.Add(herramienta);
                _db.SaveChanges();
/*
                var montacarro = new equipos()
				{

					Nombre = "MONTA CARRO",
					obra_id = id.Id

				};
                var idequipomc = _db.equipos.Add(montacarro);
				_db.SaveChanges();

                var previomc = new previos()
                {
                    equipo_id = idequipomc.Id
                };
                _db.previos.Add(previomc);
                _db.SaveChanges();
                */
                var ventamanprev = new ventas()
                {
                    Descripcion = "MANTENIMIENTOS PREVENTIVOS",
                    IdObra = id.Id
                };
                var idventamp = _db.ventas.Add(ventamanprev);
                _db.SaveChanges();

                var ventamancorrec = new ventas()
                {
                    Descripcion = "MANTENIMIENTOS CORRECTIVOS",
                    IdObra = id.Id
                };
                var idventamc =_db.ventas.Add(ventamancorrec);
                _db.SaveChanges();

                var ventainfor = new ventas()
                {
                    Descripcion = "INFORMES GENERALES",
                    IdObra = id.Id
                };
                var idventainforme = _db.ventas.Add(ventainfor);
                _db.SaveChanges();

                var ventarepo = new ventas()
                {
                    Descripcion = "REPORTES DE SUGERENCIAS",
                    IdObra = id.Id
                };
                var idventasugerencia = _db.ventas.Add(ventarepo);
                _db.SaveChanges();

                var mp = new mantenimientopreventivo()
                {
                    IdVenta = idventamp.Id
                };
                _db.mantenimientopreventivo.Add(mp);
                _db.SaveChanges();

                var mc = new mantenimientocorrectivo()
                {
                    IdVenta = idventamc.Id
                };
                _db.mantenimientocorrectivo.Add(mc);
                _db.SaveChanges();

                
                var informe = new informes()
                {
                    IdVenta = idventainforme.Id,
                    Url = "null",Nombre = "null" ,
                    Fecha = DateTime.Now
                };
                _db.informes.Add(informe);
                _db.SaveChanges();

                var sugerencia = new sugerencias()
                {
                    IdVenta = idventasugerencia.Id,
                    Descripcion = "null",
                    Caracteristica = "null",
                    Numero = "null",
                    AccionesRecomendadas = "null",
                    AccionesTomadas = "null",
                    GerenciaResponsable = "null",
                    Fecha=DateTime.Now
                };
                _db.sugerencias.Add(sugerencia);
                _db.SaveChanges();

                return id;
            }
			catch (Exception)
			{
			   

				throw;
			}


		}

        public void CrearContacto(int obra_id, string nombre = null, string email = null, string telefono = null)
        {
            try
            {
                var obra = new obras_contactos()
                {
                    nombre = nombre,
                    email = email,
                    telefono = telefono,
                    obra_id = obra_id
                };

                var id = _db.obras_contactos.Add(obra);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }


        }
        public void EditarContacto(int obra_id, string nombre = null, string email = null, string telefono = null)
        {
            try
            {
                var obracontacto = _db.obras_contactos.FirstOrDefault(o => o.id == obra_id);

                obracontacto.nombre = nombre;
                obracontacto.email = email;
                obracontacto.telefono = telefono;
                _db.Entry(obracontacto).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void EliminarContacto(int id)
        {

            var obracontacto = _db.obras_contactos.Find(id);

            if (obracontacto != null)
            {
                _db.obras_contactos.Remove(obracontacto);

                _db.SaveChanges();

            }


        }
        public void EliminarArchivo(string archivourl, string tipo, string caracteristica)
        {

            var archivo = _db.archivos.FirstOrDefault(o => o.url == archivourl && o.tipo == tipo && o.caracteristica == caracteristica);

            if (archivo != null)
            {
                _db.archivos.Remove(archivo);

                _db.SaveChanges();

            }


        }
        //      public void AsociarUsuarioEnCuenta(string usuarioId, int cuentaId)
        //{
        //	// TODO PASAR LOS QUERIES A UN ARCHIVO DE RECURSO
        //	const string sql = @"
        //				INSERT INTO  cuentasusuarios (UsuarioId, CuentaId)
        //				VALUES ({0},{1})";
        //	_db.Database.ExecuteSqlCommand(sql, usuarioId, cuentaId);
        //	_db.SaveChanges();
        //}

        //public void DesAsociarUsuarioEnCuenta(string usuarioId)
        //{
        //	// TODO PASAR LOS QUERIES A UN ARCHIVO DE RECURSO O STORE PROCEDURE
        //	const string sql = @"
        //				DELETE FROM cuentasusuarios 
        //				WHERE  UsuarioId = {0}";
        //	_db.Database.ExecuteSqlCommand(sql, usuarioId);
        //	_db.SaveChanges();
        //}

        //TODO
        public void Eliminar(int id)
		{

			var obra = _db.obras.Find(id);

            

            // componenteelectrico.cuentasmensajes.Clear();

            _db.obras.Remove(obra);
            _db.SaveChanges();

            // if (obra != null)
			//{
			//	//OJO REALIZAR ELIMINACION DE DATOS RELACIONADOS
			//	/*
			//	foreach (var proveedor in obra.proveedores.ToList())
			//	{
			//		var prvoeProveedorManager = new ProveedorManager();
			//		prvoeProveedorManager.Eliminar(proveedor.Id);
			//	}*/

			//}

			//var messageError = new MySqlParameter
			//{
			//	ParameterName = "messageError",
			//	Direction = ParameterDirection.Output,
			//	MySqlDbType = MySqlDbType.VarChar
			//};

			//var parameters = new List<MySqlParameter>()
			//{
			//	new MySqlParameter
			//	{
			//		ParameterName = "messageError",
			//		Direction = ParameterDirection.Output,
			//		MySqlDbType = MySqlDbType.VarChar
			//	},
			//	new MySqlParameter("p_cuentaId", id)
			//};

			//Db.ExecuteProcedureOut(parameters, "delete_account");
		   
			//if (messageError.Value != null)
			//{
			//	throw new BusinessException(messageError.Value.ToString());
			//}
  
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
