using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class DatoObrasManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();

	
		public List<obrasdato> FindAll()
		{
			return _db.obrasdato.ToList();
		}

		public List<obrasdato> FindObras(int? id)
		{

			return _db.obrasdato.ToList();


		}
	  
		public obrasdato Find(int? id)
		{
			return _db.obrasdato.FirstOrDefault(o => o.Id ==id);
		}

	   

	  
		public obrasdato Actualizar(int id, string primercontacto = null, string contactotelefonicoobra = null, string correoelectronicoobra = null,
            string cargoobra = null, string nombreconstructora = null, string contactotelefonicoempresa = null, string correoelectronicoempresa = null, string cargoempresa = null
            , string direccionoficina = null, string ubicacionobra = null, string tipoobra = null, string numeroequipos = null, string tipoequipo = null
            , string vendidasclimb = null, string vendidasotros = null, string novendidas = null, string paralizadas = null)
		{

			var obrasdato = _db.obrasdato.Find(id);

			if (obrasdato == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
            //if(primercontacto == null)
            //{
            //    primercontacto = "null";
            //}
            //if (contactotelefonicoobra == null)
            //{
            //    contactotelefonicoobra = "null";
            //}
            //if (correoelectronicoobra == null)
            //{
            //    correoelectronicoobra = "null";
            //}
            //if (cargoobra == null)
            //{
            //    cargoobra = "null";
            //}
            //if (nombreconstructora == null)
            //{
            //    nombreconstructora = "null";
            //}
            //if (contactotelefonicoempresa == null)
            //{
            //    contactotelefonicoempresa = "null";
            //}
            //if (correoelectronicoempresa == null)
            //{
            //    correoelectronicoempresa = "null";
            //}
            //if (cargoempresa == null)
            //{
            //    cargoempresa = "null";
            //}
            //if (direccionoficina == null)
            //{
            //    direccionoficina = "null";
            //}
            //if (ubicacionobra == null)
            //{
            //    ubicacionobra = "null";
            //}
            //if (tipoobra == null)
            //{
            //    tipoobra = "null";
            //}
            //if (numeroequipos == null)
            //{
            //    numeroequipos = "null";
            //}
            //if (tipoequipo == null)
            //{
            //    tipoequipo = "null";
            //}
            //if (vendidasclimb == null)
            //{
            //    vendidasclimb = "null";
            //}
            //if (vendidasotros == null)
            //{
            //    vendidasotros = "null";
            //}
            //if (novendidas == null)
            //{
            //    novendidas = "null";
            //}
            //if (paralizadas == null)
            //{
            //    paralizadas = "null";
            //}
            if(primercontacto != null)
                obrasdato.primercontacto = primercontacto;
            if (contactotelefonicoobra != null)
                obrasdato.contactotelefonicoobra = contactotelefonicoobra;
            if (correoelectronicoobra != null)
                obrasdato.correoelectronicoobra = correoelectronicoobra;
            if (cargoobra != null)
                obrasdato.cargoobra = cargoobra;
            if (nombreconstructora != null)
                obrasdato.nombreconstructora = nombreconstructora;
            if (contactotelefonicoempresa != null)
                obrasdato.contactotelefonicoempresa = contactotelefonicoempresa;
            if (correoelectronicoempresa != null)
                obrasdato.correoelectronicoempresa = correoelectronicoempresa;
            if (cargoempresa != null)
                obrasdato.cargoempresa = cargoempresa;
            if (direccionoficina != null)
                obrasdato.direccionoficina = direccionoficina;
            if (ubicacionobra != null)
                obrasdato.ubicacionobra = ubicacionobra;
            if (tipoobra != null)
                obrasdato.tipoobra = tipoobra;
            if (numeroequipos != null)
                obrasdato.numeroequipos = numeroequipos;
            if (tipoequipo != null)
                obrasdato.tipoequipo = tipoequipo;
            if (vendidasclimb != null)
                obrasdato.vendidasclimb = vendidasclimb;
            if (vendidasotros != null)
                obrasdato.vendidasotros = vendidasotros;
            if (novendidas != null)
                obrasdato.novendidas = novendidas;
            if (paralizadas != null)
                obrasdato.paralizadas = paralizadas;


            _db.Entry(obrasdato).State = EntityState.Modified;
			_db.SaveChanges();

			return obrasdato;
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
                var datoobra = new obrasdato()
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

				var id = _db.obrasdato.Add(datoobra);
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

			var obradato = _db.obrasdato.Find(id);

            _db.obrasdato.Remove(obradato);
            _db.SaveChanges();

        }


	}
}
