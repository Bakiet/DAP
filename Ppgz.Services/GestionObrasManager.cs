using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class GestionObrasManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();

	
		public List<obrasgestion> FindAll()
		{
			return _db.obrasgestion.ToList();
		}

		public List<obrasgestion> FindObras(int? id)
		{

			return _db.obrasgestion.ToList();


		}
	  
		public obrasgestion Find(int? id)
		{
			return _db.obrasgestion.FirstOrDefault(o => o.Id ==id);
		}

	   

	  
		public obrasgestion Actualizar(int id, string primercontacto = null, string contactotelefonicoobra = null, string correoelectronicoobra = null,
            string cargoobra = null, string nombreconstructora = null, string contactotelefonicoempresa = null, string correoelectronicoempresa = null, string cargoempresa = null
            , string direccionoficina = null, string ubicacionobra = null, string tipoobra = null, string numeroequipos = null, string tipoequipo = null
            , string vendidasclimb = null, string vendidasotros = null, string novendidas = null, string paralizadas = null)
		{

			var obrasgestion = _db.obrasgestion.Find(id);

			if (obrasgestion == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
            obrasgestion.primercontacto = primercontacto;
            obrasgestion.contactotelefonicoobra = contactotelefonicoobra;
            obrasgestion.correoelectronicoobra = correoelectronicoobra;
            obrasgestion.cargoobra = cargoobra;
            obrasgestion.nombreconstructora = nombreconstructora;
            obrasgestion.contactotelefonicoempresa = contactotelefonicoempresa;
            obrasgestion.correoelectronicoempresa = correoelectronicoempresa;
            obrasgestion.cargoempresa = cargoempresa;
            obrasgestion.direccionoficina = direccionoficina;
            obrasgestion.ubicacionobra = ubicacionobra;
            obrasgestion.tipoobra = tipoobra;
            obrasgestion.numeroequipos = numeroequipos;
            obrasgestion.tipoequipo = tipoequipo;
            obrasgestion.vendidasclimb = vendidasclimb;
            obrasgestion.vendidasotros = vendidasotros;
            obrasgestion.novendidas = novendidas;
            obrasgestion.paralizadas = paralizadas;


            _db.Entry(obrasgestion).State = EntityState.Modified;
			_db.SaveChanges();

			return obrasgestion;
		}

		

		public void Crear(string primercontacto = null, string contactotelefonicoobra = null, string correoelectronicoobra = null,
			string cargoobra = null, string nombreconstructora = null, string contactotelefonicoempresa = null, string correoelectronicoempresa = null, string cargoempresa = null
			, string direccionoficina = null, string ubicacionobra = null, string tipoobra = null, string numeroequipos = null, string tipoequipo = null
            , string vendidasclimb = null, string vendidasotros = null, string novendidas = null, string paralizadas = null)
		{
            //if (primercontacto == null){primercontacto = "null";}
            //if (contactotelefonicoobra == null) { contactotelefonicoobra = "null"; }
            //if (correoelectronicoobra == null) { correoelectronicoobra = "null"; }
            //if (cargoobra == null) { cargoobra = "null"; }
            //if (nombreconstructora == null) { nombreconstructora = "null"; }
            //if (contactotelefonicoempresa == null) { contactotelefonicoempresa = "null"; }
            //if (correoelectronicoempresa == null) { correoelectronicoempresa = "null"; }
            //if (cargoempresa == null) { cargoempresa = "null"; }
            //if (direccionoficina == null) { direccionoficina = "null"; }
            //if (ubicacionobra == null) { ubicacionobra = "null"; }
            //if (tipoobra == null) { tipoobra = "null"; }
            //if (numeroequipos == null) { numeroequipos = "null"; }
            //if (tipoequipo == null) { tipoequipo = "null"; }
            //if (vendidasclimb == null) { vendidasclimb = "null"; }
            //if (vendidasotros == null) { vendidasotros = "null"; }
            //if (novendidas == null) { novendidas = "null"; }
            //if (paralizadas == null) { paralizadas = "null"; }
            try
			{
                var gestionobra = new obrasgestion()
                {

                    primercontacto = primercontacto,
                    contactotelefonicoobra = contactotelefonicoobra,
                    correoelectronicoobra = correoelectronicoobra,
                    cargoobra = cargoobra,
                    nombreconstructora = nombreconstructora,
                    contactotelefonicoempresa = contactotelefonicoempresa,
                    correoelectronicoempresa = correoelectronicoempresa,
                    cargoempresa = cargoempresa,
                    direccionoficina = direccionoficina,
                    ubicacionobra = ubicacionobra,
                    tipoobra = tipoobra,
                    numeroequipos = numeroequipos,
                    tipoequipo = tipoequipo,
                    vendidasclimb = vendidasclimb,
                    vendidasotros = vendidasotros,
                    novendidas = novendidas,
                    paralizadas = paralizadas,
                    Descripcion = ""
                };

				var id = _db.obrasgestion.Add(gestionobra);
				_db.SaveChanges();

				/*
				var ascensorprivado = new equipos()
				{

					Nombre = "ASCENSOR PRIVADO",
					obra_id = id.Id

				};
				_db.equipos.Add(ascensorprivado);
				_db.SaveChanges();

				var ascensorservicio = new equipos()
				{

					Nombre = "ASCENSOR DE SERVICIO",
					obra_id = id.Id

				};
				_db.equipos.Add(ascensorservicio);
				_db.SaveChanges();

				var montacarro = new equipos()
				{

					Nombre = "MONTA CARRO",
					obra_id = id.Id

				};
				_db.obrasgestion.Add(montacarro);
				_db.SaveChanges();*/
			}
			catch (Exception)
			{
			   

				throw;
			}


		}

	
		//TODO
		public void Eliminar(int id)
		{

			var obragestion = _db.obrasgestion.Find(id);

            _db.obrasgestion.Remove(obragestion);
            _db.SaveChanges();

        }


	}
}
