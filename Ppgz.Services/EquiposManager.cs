﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class EquiposManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();

        public List<obras> FindObras(int id)
        {
            equipos eq = new equipos();

            eq = _db.equipos.Where(e => e.Id == id).FirstOrDefault();

            return _db.obras.ToList();


        }
        public List<equipo_tipo> FindEquiposTipo()
		{
			return _db.equipo_tipo.ToList();

		}

        public List<equipos> FindAll()
		{
			return _db.equipos.ToList();
		}

	
		public equipos Find(int id)
		{
			return _db.equipos.FirstOrDefault(e => e.Id ==id);
		}
        public equipos FindPorObra(int id)
        {
            return _db.equipos.FirstOrDefault(e => e.obra_id == id);
        }
        public List<obras> FindObras()
        {
            //return _db.obras.ToList();
            return _db.obras.Where(c => c.oficina != "SI").ToList();
        }

        public List<equipos> GetEquipos(int obraId)
		{
			return _db.equipos.Where(e => e.obra_id == obraId).ToList();

		}

        public List<previos> GetPrevios(int equipoId)
        {
            return _db.previos.Where(e => e.equipo_id == equipoId).ToList();

        }

        public List<componentesmecanicos> GetComponentesMecanicos(int equipoId)
        {
            return _db.componentesmecanicos.Where(e => e.IdEquipos == equipoId).ToList();

        }
        public List<componenteselectricos> GetComponentesElectricos(int equipoId)
        {
            return _db.componenteselectricos.Where(e => e.IdEquipos == equipoId).ToList();

        }

        public equipos Actualizar(int obraid,int id, string nombre = null, string marca = null, string modelo = null,
			string referencia = null, string dimensionescabina = null, string dimensioneshueco = null, string carganominal = null, string velocidad = null
			, string recorrido = null, string paradas = null, string accesos = null, string voltajedered = null, string potenciademaquina = null, string tipodemaniobra = null
			, string numerodeguayas = null, string cantidadpersonas = null, string fotografia = null, string plano = null)
		{

			var equipos = _db.equipos.Find(id);

			if (equipos == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			if (nombre != null)
			{
				
				equipos.Nombre = nombre;
			}
			if (marca != null)
			{
				
				equipos.Marca = marca;
			}
			if (modelo != null)
			{
				
				equipos.Modelo = modelo;
			}
			if (referencia != null)
			{
			
				equipos.Referencia = referencia;
			}
			if (dimensionescabina != null)
			{
				
				equipos.DimensionesCabina = dimensionescabina;
			}
			if (dimensioneshueco != null)
			{
				
				equipos.DimensionesHueco = dimensioneshueco;
			}
			if (carganominal != null)
			{
				
				equipos.CargaNominal = carganominal;
			}
			if (velocidad != null)
			{
				
				equipos.Velocidad = velocidad;
			}
			if (recorrido != null)
			{
				
				equipos.Recorrido = recorrido;
			}
			if (paradas != null)
			{
			
				equipos.Paradas = paradas;
			}
			if (accesos != null)
			{
				
				equipos.Accesos = accesos;
			}
			if (voltajedered != null)
			{
				
				equipos.VoltajeDeRed = voltajedered;
			}
			if (potenciademaquina != null)
			{
			
				equipos.PotenciaDeMaquina = potenciademaquina;
			}
			if (tipodemaniobra != null)
			{
			
				equipos.TipoDeManiobra = tipodemaniobra;
			}
			if (numerodeguayas != null)
			{
				equipos.NumeroDeGuayas = numerodeguayas;
			}

			if (fotografia != null)
				equipos.Fotografia = fotografia;

			if (plano != null)
				equipos.Plano = plano;

            equipos.CantidadPersonas = Convert.ToInt32(cantidadpersonas);

            equipos.obra_id = obraid;
			//equipos.obra_id = id;

			_db.Entry(equipos).State = EntityState.Modified;
			_db.SaveChanges();

			return equipos;
		}


		public equipos Crear(int obraid,string nombre = null, string marca = null, string modelo = null,
			string referencia = null, string dimensionescabina = null, string dimensioneshueco = null, string carganominal = null, string velocidad = null
			, string recorrido = null, string paradas = null, string accesos = null, string voltajedered = null, string potenciademaquina = null, string tipodemaniobra = null
			, string numerodeguayas = null, string cantidadpersonas = null, string fotografia = null, string plano = null)
		{
		   

	 
			try
			{
                var equipo = new equipos()
                {

                    Nombre = nombre,
                    Marca = marca,
                    Modelo = modelo,
                    Referencia = referencia,
                    DimensionesCabina = dimensionescabina,
                    DimensionesHueco = dimensioneshueco,
                    CargaNominal = carganominal,
                    Velocidad = velocidad,
                    Recorrido = recorrido,
                    Paradas = paradas,
                    Accesos = accesos,
                    VoltajeDeRed = voltajedered,
                    PotenciaDeMaquina = potenciademaquina,
                    TipoDeManiobra = tipodemaniobra,
                    NumeroDeGuayas = numerodeguayas,
                    CantidadPersonas = Convert.ToInt32(cantidadpersonas),
					Fotografia = fotografia,
					Plano = plano,
                    obra_id = obraid
				};

                var equipoid = _db.equipos.Add(equipo);
				_db.SaveChanges();

                var previo = new previos()
                {
                    equipo_id = equipoid.Id,
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
                   
                };
                var previoid= _db.previos.Add(previo);
                _db.SaveChanges();

                var protocolo = new protocolos()
                {
                    obra_id = obraid,
                    IdPrevio = previoid.Id
                };
                _db.protocolos.Add(protocolo);
                _db.SaveChanges();

                var herramienta = new herramientas()
                {
                    obra_id = obraid,
                    previo_id = previoid.Id
                };
                _db.herramientas.Add(herramienta);
                _db.SaveChanges();

                return equipo;

			}
			catch (Exception)
			{
			   

				throw;
			}


		}

        public void Eliminar(int id)
        {

            var equipo = Find(id);

            // componenteelectrico.cuentasmensajes.Clear();

            _db.equipos.Remove(equipo);
            _db.SaveChanges();
        }
    }
}