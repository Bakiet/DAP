﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Ppgz.Repository;
using SapWrapper;
using SatWrapper;

namespace Ppgz.Services
{
    public class FacturaManager
    {
       /* private readonly EntitiesDap _db = new EntitiesDap();
        public List<facturas>GetFacturas(int proveedorId)
        {
            return _db.facturas.Where(f => f.proveedor_id == proveedorId).ToList();

        }
        public facturas Find(int facturaId, int proveedorId)
        {
            return _db.facturas.FirstOrDefault(f => f.Id == facturaId && f.proveedor_id == proveedorId);
        }

        public void ValidarFactura(Stream xmlFileStream)
        {
            var serializer = new XmlSerializer(typeof(Comprobante));

            try
            {

                serializer.Deserialize(xmlFileStream);
            }
            catch (Exception )
            {
                
                throw new BusinessException("Xml incorrecto");
            }
        }

        public void CargarFactura(int proveedorId, int cuentaId, string xmlFilePath, string pdfFilePath)
        {
            // Validaciones de los archivos
            if (!File.Exists(xmlFilePath))
            {
                throw new BusinessException("Xml incorrecto");
            }
            if(Path.GetExtension(xmlFilePath).ToLower() != ".xml")
            {
                throw new BusinessException("Xml incorrecto");
            }
            if (!File.Exists(pdfFilePath))
            {
                throw new BusinessException("Pdf incorrecto");
            }
            if(Path.GetExtension(pdfFilePath).ToLower() != ".pdf")
            {
                throw new BusinessException("Pdf incorrecto");
            }

            // Validacion de la factura 
            var serializer = new XmlSerializer(typeof(Comprobante));
            
            var xmlFileStream = new FileStream(xmlFilePath, FileMode.Open);
            

            var comprobante = (Comprobante)serializer.Deserialize(xmlFileStream);
            
            if (_db.facturas.FirstOrDefault(fa => fa.Uuid == comprobante.Complemento.TimbreFiscalDigital.UUID) != null)
            {
                throw new BusinessException("La factura ya esta registrada en el sistema");
            }

            var proveedor = _db.proveedores.FirstOrDefault(p => p.Rfc == comprobante.Emisor.Rfc);
            
            if (proveedor == null)
            {
                throw new BusinessException("Proveedor incorrecto en la factura");
            }

            if (proveedor.Id != proveedorId)
            {
                throw new BusinessException("Proveedor incorrecto en la factura");
            }

            if (_db.facturas.FirstOrDefault(fa => fa.Uuid == comprobante.Complemento.TimbreFiscalDigital.UUID) != null)
            {
                throw new BusinessException("La factura ya esta registrada en el sistema");
            }
            xmlFileStream.Close();
            xmlFileStream.Dispose();

            var contenido = File.ReadAllText(xmlFilePath);
            
            // Validación ante el sat

            var configuraciones = _db.configuraciones.ToList();
            var cfdiToken = configuraciones.Single(c => c.Clave == "cfdi.token").Valor;
            var cfdiPassword = configuraciones.Single(c => c.Clave == "cfdi.password").Valor;
            var cfdiUser = configuraciones.Single(c => c.Clave == "cfdi.user").Valor;
            var cfdiCuenta = configuraciones.Single(c => c.Clave == "cfdi.cuenta").Valor;

            CfdiServiceConsulta.Validar(contenido, cfdiToken, cfdiPassword, cfdiUser, cfdiCuenta, comprobante.Receptor.Rfc);
            
            //Creacion de la factura para la miro
            var cantidad = comprobante.Conceptos.Concepto.Aggregate<Concepto, decimal>(0, (current, concepto) => current + Convert.ToDecimal(concepto.Cantidad));

            var sapFacturaManager = new SapFacturaManager();
            
            var facturaSap = sapFacturaManager.CrearFactura(
                proveedor.NumeroProveedor,
                comprobante.Serie  ?? string.Empty + comprobante.Folio,
                DateTime.ParseExact(comprobante.Fecha, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture),
                comprobante.SubTotal,
                comprobante.Total,
                cantidad.ToString(CultureInfo.InvariantCulture),
                 comprobante.Complemento.TimbreFiscalDigital.UUID,
                 comprobante.Emisor.Rfc);

            // Se crea el directorio de acuerdo a la fecha del comprobante
            var fecha = DateTime.ParseExact(comprobante.Fecha.Substring(0, 10), "yyyy-MM-dd",
                CultureInfo.InvariantCulture);
            var directory = GetFacturaDirectory(fecha);
           
            // Movemos el xml
            var xmlFileName = comprobante.Complemento.TimbreFiscalDigital.UUID + ".xml";
            var newXmlPath = Path.Combine(directory.FullName, xmlFileName);
            File.Copy(xmlFilePath, newXmlPath);

            // Movemos el pdf
            var pdfFileName = comprobante.Complemento.TimbreFiscalDigital.UUID + ".pdf";
            var newPdfPath = Path.Combine(directory.FullName, pdfFileName);
            File.Copy(pdfFilePath, newPdfPath);

            var facturas = new facturas
            {
                Serie = comprobante.Serie ?? string.Empty,
                Folio = comprobante.Folio,
                Fecha =  fecha,
                Total = decimal.Parse(comprobante.Total, CultureInfo.InvariantCulture),
                proveedor_id = proveedor.Id,
                Uuid = comprobante.Complemento.TimbreFiscalDigital.UUID,
                XmlRuta = newXmlPath,
                PdfRuta = newPdfPath,
                Estatus = facturaSap.Estatus
            };

            if (facturas.Estatus != "S")
            {
                facturas.Comentario = string.Format
                    ("Tipo:{1} {0}Mensaje:{2}", 
                    Environment.NewLine,
                    facturaSap.ErrorTable.Rows[0]["TYPE"],
                    facturaSap.ErrorTable.Rows[0]["MESSAGE"]);
            }
            else
            {
                facturas.NumeroGenerado = facturaSap.FacturaNumero;
            }

            _db.facturas.Add(facturas);

            _db.SaveChanges();
        }


        public DirectoryInfo GetFacturaDirectory(DateTime fecha)
        {
            var facturaRootDirectory = new DirectoryInfo(_db.configuraciones.Single(c => c.Clave == "facturas.rootdirectory").Valor);

            var yearPath = facturaRootDirectory.GetDirectories(fecha.Year.ToString()).Length == 0 ?
                facturaRootDirectory.CreateSubdirectory(fecha.Year.ToString()) :
                facturaRootDirectory.GetDirectories(fecha.Year.ToString())[0];

            var monthPath = yearPath.GetDirectories(fecha.Month.ToString()).Length == 0 ?
                yearPath.CreateSubdirectory(fecha.Month.ToString()) :
                yearPath.GetDirectories(fecha.Month.ToString())[0];

            return monthPath;
        }
        */
    }
}
