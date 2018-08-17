using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MySql.Data.MySqlClient;
using Ppgz.Repository;

namespace Ppgz.Services
{
	public class EnviosManager
	{
		private readonly EntitiesDap _db = new EntitiesDap();

        public List<envios_status> FindStatus()
        {
           /* envios_status eq = new envios_status();

            eq = _db.envios_status.Where(e => e.tipo == "envio").FirstOrDefault();*/

            List<envios_status> eq = new List<envios_status>();

            eq = _db.envios_status.Where(e => e.tipo == "envio").ToList();

            return eq; // _db.envios_status.ToList();
        }

        public List<envios> FindStatusPagosPorEnvio(int id)
        {
            List<envios> eq = new List<envios>();

            eq = _db.envios.Where(e => e.Id == id).ToList();

            return eq; // _db.envios_status.ToList();
        }

        public List<envios_status> FindStatusPagos()
        {
            List <envios_status> eq = new List<envios_status>();

            eq = _db.envios_status.Where(e => e.tipo == "pago").ToList();

            return eq; // _db.envios_status.ToList();
        }
        public List<envios_status> FindStatusEntrega()
        {
            /*envios_status eq = new envios_status();

            eq = _db.envios_status.Where(e => e.tipo == "entrega").FirstOrDefault();*/

            List<envios_status> eq = new List<envios_status>();

            eq = _db.envios_status.Where(e => e.tipo == "entrega").ToList();

            return eq; // _db.envios_status.ToList();

           
        }

        public List<componenteselectronicos_tipos> FindComponentes()
        {

            return _db.componenteselectronicos_tipos.ToList();
            
        }

        public List<obras> FindObras(int id)
        {
            envios eq = new envios();

            eq = _db.envios.Where(e => e.Id == id).FirstOrDefault();

            return _db.obras.ToList();


        }

        public List<obras> FindObras()
        {

           // return _db.obras.ToList();
            return _db.obras.Where(c => c.oficina != "SI").ToList();

        }
        public List<obras> FindObrasWithOficina()
        {

            // return _db.obras.ToList();
            return _db.obras.ToList();

        }

        public List<envios> FindAll()
		{
			return _db.envios.ToList();
		}

	
		public envios Find(int id)
		{
			return _db.envios.FirstOrDefault(e => e.Id ==id);
		}
        public List<envios> GetEnvios()
        {
            return _db.envios.Where(f => f.StatusEnvio != "RECIBIDO").ToList();

        }

        public List<envios> GetEnvios(int obraId)
		{
            return _db.envios.Where(f => f.obra_id == obraId && f.StatusEnvio != "RECIBIDO").ToList();

        }
        public List<envios> GetHistorico(int obraId)
        {
            return _db.envios.Where(f => f.obra_id == obraId && f.StatusEnvio == "RECIBIDO").ToList();

        }
        public List<envios> GetHistoricoAll()
        {
            return _db.envios.Where(f => f.StatusEnvio == "RECIBIDO").ToList();

        }

        public envios Actualizar(int? obraid,int id, string obra = null, string tipocomponente = null, DateTime? fechapedido = null,
			DateTime? fechasalida = null, string tracking = null, string empresaenvio = null, string statusenvio = null, string numerobulto = null
			, string pesototal = null, string codigoarancelario = null, string solicitado = null, string costoproducto = null, string statuspago = null, 
			string numerofactura = null, string statusentrega = null, string entregaproducto = null, DateTime? fechaentrega = null)
		{

			var envio = _db.envios.Find(id);

			if (obra == null)
			{
			   // throw new BusinessException(CommonMensajesResource.ERROR_Usuario_Id);
				
			}
			//if (tipocomponente != null)
			//{
				// ValidarNombreApellido(nombre);
				envio.TipoComponente = tipocomponente;
			//}
			//if (fechapedido != null)
			//{
			  //  ValidarNombreApellido(apellido);
				envio.FechaPedido = fechapedido;
			//}
			//if (fechasalida != null)
			//{
			   // ValidarEmail(email);
				envio.FechaSalida = fechasalida;

			//}
			//if (tracking != null)
				envio.Tracking= tracking;

			//if (empresaenvio != null)
				envio.EmpresaEnvio = empresaenvio;
			//if (statusenvio != null)
				envio.StatusEnvio = statusenvio;

			//if (numerobulto != null)
				envio.NumeroBulto = numerobulto;
			//if (pesototal != null)
				envio.PesoTotal = pesototal;


			//if (codigoarancelario != null)
				envio.CodigoArancelario = codigoarancelario;

			//if (solicitado != null)
				envio.Solicitado = solicitado;

			//if (solicitado != null)
				envio.Solicitado = solicitado;

			//if (costoproducto != null)
				envio.CostoProducto = costoproducto;

			//if (statuspago != null)
				envio.StatusPago = statuspago;

			//if (numerofactura != null)
				envio.NumeroFactura = numerofactura;

			//if (solicitado != null)
				envio.Solicitado = solicitado;

			//if (statusentrega != null)
				envio.StatusEntrega = statusentrega;

			//if (entregaproducto != null)
				envio.EntregaProducto = entregaproducto;

			//if (fechaentrega != null)
				envio.FechaEntrega = fechaentrega;

            envio.obra_id = obraid;

			_db.Entry(envio).State = EntityState.Modified;
			_db.SaveChanges();

			return envio;
		}

	   

		public void Crear(int? obraid, string obra = null, string tipocomponente = null, DateTime? fechapedido = null,
			DateTime? fechasalida = null, string tracking = null, string empresaenvio = null, string statusenvio = null, string numerobulto = null
			, string pesototal = null, string codigoarancelario = null, string solicitado = null, string costoproducto = null, string statuspago = null,
			string numerofactura = null, string statusentrega = null, string entregaproducto = null, DateTime? fechaentrega = null)
		{
		   
            //if(obra == null)
            //{
            //    obra = "null";
            //}if(tipocomponente == null)
            //{
            //    tipocomponente = "null";
            //}if(fechapedido == null)
            //{
            //    fechapedido = DateTime.Now.ToString();
            //}if(fechasalida == null)
            //{
            //    fechasalida = DateTime.Now.ToString();
            //}
            //if (tracking == null)
            //{
            //    tracking = "null";
            //}if(empresaenvio == null)
            //{
            //    empresaenvio = "null";
            //}if(statusenvio == null)
            //{
            //    statusenvio = "EN TRANSITO";
            //}if(numerobulto == null)
            //{
            //    numerobulto = "null";
            //}if(pesototal == null)
            //{
            //    pesototal = "null";
            //}if(codigoarancelario == null)
            //{
            //    codigoarancelario = "null";
            //}if(solicitado == null)
            //{
            //    solicitado = "null";
            //}
            //if (costoproducto == null)
            //{
            //    costoproducto = "null";
            //}if(statuspago == null)
            //{
            //    statuspago = "NO CANCELADO";
            //}
            //if (numerofactura == null)
            //{
            //    numerofactura = "null";
            //}
            //if (entregaproducto == null)
            //{
            //    entregaproducto = "null";
            //}
            //if (fechaentrega == null)
            //{
            //    fechaentrega = DateTime.Now.ToString();
            //}
	        
			try
			{
				var envio = new envios()
				{

					Obra = obra,
					TipoComponente = tipocomponente,
					FechaPedido = fechapedido,
					FechaSalida = fechasalida,
					Tracking = tracking,
					EmpresaEnvio = empresaenvio,
					StatusEnvio = statusenvio,
					NumeroBulto = numerobulto,
					PesoTotal = pesototal,
					CodigoArancelario = codigoarancelario,
					Solicitado = solicitado,
					CostoProducto = costoproducto,
					StatusPago = statuspago,
					NumeroFactura = numerofactura,
					StatusEntrega = statusentrega,
					EntregaProducto = entregaproducto,
					FechaEntrega = fechaentrega,
                    obra_id = obraid
				};

				_db.envios.Add(envio);
				_db.SaveChanges();

			}
			catch (Exception)
			{
			   

				throw;
			}


		}


        public void Eliminar(int id)
        {

            var envio = Find(id);

            // componenteelectrico.cuentasmensajes.Clear();

            _db.envios.Remove(envio);
            _db.SaveChanges();
        }



    }
}
