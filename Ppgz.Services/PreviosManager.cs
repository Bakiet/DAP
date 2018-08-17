using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class PreviosManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();


		public List<previos> FindAll()
		{
			return _db.previos.ToList();
		}

	
		public previos Find(int id)
		{
			return _db.previos.FirstOrDefault(p => p.Id == id);
		}


        public previos FindPorEquipo(int id)
        {
            return _db.previos.FirstOrDefault(p => p.equipo_id == id);
        }

        public List<previos> GetPrevios(int equipoId)
		{
			return _db.previos.Where(p => p.equipo_id == equipoId).ToList();

		}

        public previos GetPrevio(int equipoId)
        {
            return _db.previos.FirstOrDefault(p => p.equipo_id == equipoId);

        }

        public previos Actualizar(int id, string FechaFirmaContrato = null, string FechaPagoInicialFabrica = null, string FechaPagoInicialEquipo = null,
            string FechaConstruccion = null, string FechaSalidaFabrica = null, string FechaSalidaBuque = null, string FechaLlegadaBuque = null, string FechaPeriodoNacionalizacion = null
            , string FechaSalidaPuertoObra = null, string FechaDescargaResguardo = null, string ActivacionProtocoloGarantiaFabrica = null, string FechaInicioMontaje = null, string FechaEntregaSoso = null
            , string FechaCulminacionMontaje = null, string FechaConfiguracion = null, string FechaPeriodoPrueba = null, string FechaInspeccion = null, string TecnicoInstalador = null
            , string CartaPresentacion = null, string FechaIngresoProduccion = null, string EstatusConstruccion = null, string IngresoDepartamentoIngMan = null, string SolicitudPagoInicialFabrica = null
           )
        {

			var previo = _db.previos.Find(id);

			if (previo == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
            if(FechaFirmaContrato != null)
                previo.FechaFirmaContrato = DateTime.Parse(FechaFirmaContrato);
            if (FechaPagoInicialFabrica != null)
                previo.FechaPagoInicialFabrica = DateTime.Parse(FechaPagoInicialFabrica);
            if (FechaPagoInicialEquipo != null)
                previo.FechaPagoInicialEquipo = DateTime.Parse(FechaPagoInicialEquipo);
            if (FechaConstruccion != null)
                previo.FechaConstruccion = DateTime.Parse(FechaConstruccion);
            if (FechaSalidaFabrica != null)
                previo.FechaSalidaFabrica = DateTime.Parse(FechaSalidaFabrica);
            if (FechaSalidaBuque != null)
                previo.FechaSalidaBuque = DateTime.Parse(FechaSalidaBuque);
            if (FechaLlegadaBuque != null)
                previo.FechaLlegadaBuque = DateTime.Parse(FechaLlegadaBuque);
            if (FechaPeriodoNacionalizacion != null)
                previo.FechaPeriodoNacionalizacion = DateTime.Parse(FechaPeriodoNacionalizacion);
            if (FechaSalidaPuertoObra != null)
                previo.FechaEntregaSoso = DateTime.Parse(FechaSalidaPuertoObra);
            if (FechaDescargaResguardo != null)
                previo.FechaConfiguracion = DateTime.Parse(FechaDescargaResguardo);
            if (ActivacionProtocoloGarantiaFabrica != null)
                previo.ActivacionProtocoloGarantiaFabrica = ActivacionProtocoloGarantiaFabrica;
            if (FechaCulminacionMontaje != null)
                previo.FechaInicioMontaje = DateTime.Parse(FechaCulminacionMontaje);
            if (FechaEntregaSoso != null)
                previo.FechaEntregaSoso = DateTime.Parse(FechaEntregaSoso);
            if (FechaConfiguracion != null)
                previo.FechaConfiguracion = DateTime.Parse(FechaConfiguracion);
            if (FechaPeriodoPrueba != null)
                previo.FechaPeriodoPrueba = DateTime.Parse(FechaPeriodoPrueba);
            if (FechaInspeccion != null)
                previo.FechaInspeccion = DateTime.Parse(FechaInspeccion);
            if (FechaInicioMontaje != null)
                previo.FechaInicioMontaje = DateTime.Parse(FechaInicioMontaje);
            if (TecnicoInstalador != null)
                previo.TecnicoInstalador = TecnicoInstalador;
            if (CartaPresentacion != null)
                previo.CartaPresentacion = DateTime.Parse(CartaPresentacion);
            if (FechaIngresoProduccion != null)
                previo.FechaIngresoProduccion = DateTime.Parse(FechaIngresoProduccion);
            if (EstatusConstruccion != null)
                previo.EstatusConstruccion = DateTime.Parse(EstatusConstruccion);
            if (IngresoDepartamentoIngMan != null)
                previo.IngresoDepartamentoIngMan = DateTime.Parse(IngresoDepartamentoIngMan);
            if (SolicitudPagoInicialFabrica != null)
                previo.SolicitudPagoInicialFabrica = DateTime.Parse(SolicitudPagoInicialFabrica);

            //if (FechaFirmaContrato != null)
            //{

            //	previo.FechaFirmaContrato = DateTime.Parse(FechaFirmaContrato);
            //}
            //if (FechaPagoInicialFabrica != null)
            //{

            //	previo.FechaPagoInicialFabrica = DateTime.Parse(FechaPagoInicialFabrica);
            //}
            //if (FechaPagoInicialEquipo != null)
            //{

            //	previo.FechaPagoInicialEquipo = DateTime.Parse(FechaPagoInicialEquipo);
            //}
            //if (FechaConstruccion != null)
            //{

            //	previo.FechaConstruccion = DateTime.Parse(FechaConstruccion);
            //}
            //if (FechaSalidaFabrica != null)
            //{

            //	previo.FechaSalidaFabrica = DateTime.Parse(FechaSalidaFabrica);
            //}
            //if (FechaSalidaBuque != null)
            //{

            //	previo.FechaSalidaBuque = DateTime.Parse(FechaSalidaBuque);
            //}
            //if (FechaLlegadaBuque != null)
            //{

            //	previo.FechaLlegadaBuque = DateTime.Parse(FechaLlegadaBuque);
            //}

            //if (FechaPeriodoNacionalizacion != null)
            //{

            //	previo.FechaPeriodoNacionalizacion = DateTime.Parse(FechaPeriodoNacionalizacion);
            //}


            //if (FechaSalidaPuertoObra != null)
            //{

            //	previo.FechaSalidaPuertoObra = DateTime.Parse(FechaSalidaPuertoObra);
            //}
            //if (FechaDescargaResguardo != null)
            //{

            //	previo.FechaDescargaResguardo = DateTime.Parse(FechaDescargaResguardo);
            //}
            //if (ActivacionProtocoloGarantiaFabrica != null)
            //{

            //	previo.ActivacionProtocoloGarantiaFabrica = ActivacionProtocoloGarantiaFabrica;
            //}
            //if (ActivacionProtocoloGarantiaFabrica != null)
            //{

            //	previo.ActivacionProtocoloGarantiaFabrica = ActivacionProtocoloGarantiaFabrica;
            //}
            //if (FechaEntregaSoso != null)
            //{

            //	previo.FechaEntregaSoso = DateTime.Parse(FechaEntregaSoso);
            //}
            //if (FechaCulminacionMontaje != null)
            //{

            //	previo.FechaCulminacionMontaje = DateTime.Parse(FechaCulminacionMontaje);
            //}
            //if (FechaConfiguracion != null)
            //{

            //	previo.FechaConfiguracion = DateTime.Parse(FechaConfiguracion);
            //}
            //if (FechaPeriodoPrueba != null)
            //{

            //	previo.FechaPeriodoPrueba = DateTime.Parse(FechaPeriodoPrueba);
            //}
            //if (FechaInspeccion != null)
            //{

            //	previo.FechaInspeccion = DateTime.Parse(FechaInspeccion);
            //}
            //if (TecnicoInstalador != null)
            //{

            //	previo.TecnicoInstalador = TecnicoInstalador;
            //}
            //if (InformeEntregaFinal != null)
            //{

            //	previo.InformeEntregaFinal = InformeEntregaFinal;
            //}

            //if (FechaDescargaResguardoFotografia != null)
            //{

            //	previo.FechaDescargaResguardoFotografia = FechaDescargaResguardoFotografia;
            //}
            //if (FechaCulminacionMontajeFotografia != null)
            //{

            //	previo.FechaCulminacionMontajeFotografia = FechaCulminacionMontajeFotografia;
            //}
            //if (FechaInspeccionFotografia != null)
            //{

            //	previo.FechaInspeccionFotografia = FechaInspeccionFotografia;
            //}
            //if (FechaInspeccionVideo != null)
            //{

            //	previo.FechaInspeccionVideo = FechaInspeccionVideo;
            //}

            _db.Entry(previo).State = EntityState.Modified;
			_db.SaveChanges();

			return previo;
		}


		public previos Crear(string FechaFirmaContrato = null, string FechaPagoInicialFabrica = null, string FechaPagoInicialEquipo = null, 
            string FechaConstruccion = null, string FechaSalidaFabrica = null,  string FechaSalidaBuque = null,  string FechaLlegadaBuque = null,  string FechaPeriodoNacionalizacion = null
            , string FechaSalidaPuertoObra = null,  string FechaDescargaResguardo = null, string ActivacionProtocoloGarantiaFabrica = null, string FechaInicioMontaje = null,  string FechaEntregaSoso = null
            , string FechaCulminacionMontaje = null, string FechaConfiguracion = null,  string FechaPeriodoPrueba = null,  string FechaInspeccion = null, string TecnicoInstalador = null
            , string CartaPresentacion = null, string FechaIngresoProduccion = null, string EstatusConstruccion = null, string IngresoDepartamentoIngMan = null, string SolicitudPagoInicialFabrica = null)
		{
		   

	 
			try
			{
				var previo = new previos()
				{

					FechaFirmaContrato = DateTime.Parse(FechaFirmaContrato),
                   
					FechaPagoInicialFabrica = DateTime.Parse(FechaPagoInicialFabrica),
                    
					FechaPagoInicialEquipo = DateTime.Parse(FechaPagoInicialEquipo),
                    
					FechaConstruccion = DateTime.Parse(FechaConstruccion),
                   
					FechaSalidaFabrica = DateTime.Parse(FechaSalidaFabrica),
                   
					FechaSalidaBuque = DateTime.Parse(FechaSalidaBuque),
                   
					FechaLlegadaBuque = DateTime.Parse(FechaLlegadaBuque),
                  
					FechaPeriodoNacionalizacion = DateTime.Parse(FechaPeriodoNacionalizacion),
                  
					FechaSalidaPuertoObra = DateTime.Parse(FechaSalidaPuertoObra),
               
					FechaDescargaResguardo = DateTime.Parse(FechaDescargaResguardo),
                    
					ActivacionProtocoloGarantiaFabrica = ActivacionProtocoloGarantiaFabrica,
					FechaCulminacionMontaje = DateTime.Parse(FechaCulminacionMontaje),
                    FechaEntregaSoso = DateTime.Parse(FechaEntregaSoso),
                   
					FechaConfiguracion = DateTime.Parse(FechaConfiguracion),
					FechaPeriodoPrueba = DateTime.Parse(FechaPeriodoPrueba),
                    
					FechaInspeccion = DateTime.Parse(FechaInspeccion),
                    FechaInicioMontaje = DateTime.Parse(FechaInicioMontaje),
                   
					TecnicoInstalador = TecnicoInstalador,

                    CartaPresentacion = DateTime.Parse(CartaPresentacion),
                    FechaIngresoProduccion = DateTime.Parse(FechaIngresoProduccion),
                    EstatusConstruccion = DateTime.Parse(EstatusConstruccion),
                    IngresoDepartamentoIngMan = DateTime.Parse(IngresoDepartamentoIngMan),
                    SolicitudPagoInicialFabrica = DateTime.Parse(SolicitudPagoInicialFabrica)
                    //InformeEntregaFinal = InformeEntregaFinal,

                    //FechaInspeccionVideo = FechaInspeccionVideo


                };

				var previoid = _db.previos.Add(previo);
				_db.SaveChanges();
                return previo;

                /*
                var protocolo = new protocolos()
                {
                    obra_id = id.Id,
                    IdPrevio = previoid.Id
                };
                _db.protocolos.Add(protocolo);
                _db.SaveChanges();

                var herramienta = new herramientas()
                {
                    obra_id = id.Id,
                    previo_id = previoid.Id
                };
                _db.herramientas.Add(herramienta);
                _db.SaveChanges();
                */

            }
			catch (Exception)
			{
			   

				throw;
			}


		}
        public void Eliminar(int id)
        {

            var previo = Find(id);

            // componenteelectrico.cuentasmensajes.Clear();

            _db.previos.Remove(previo);
            _db.SaveChanges();
        }

    }
}
