﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SatWrapper.CFDIService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://validadorws.cfdi.mx.konesh.com", ConfigurationName="CFDIService.ValidadorIntegradoresPortType")]
    public interface ValidadorIntegradoresPortType {
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="urn:validarDocumentos", ReplyAction="urn:validarDocumentosResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        SatWrapper.CFDIService.validarDocumentosResponse validarDocumentos(SatWrapper.CFDIService.validarDocumentosRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:validarDocumentos", ReplyAction="urn:validarDocumentosResponse")]
        System.Threading.Tasks.Task<SatWrapper.CFDIService.validarDocumentosResponse> validarDocumentosAsync(SatWrapper.CFDIService.validarDocumentosRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="validarDocumentos", WrapperNamespace="http://validadorws.cfdi.mx.konesh.com", IsWrapped=true)]
    public partial class validarDocumentosRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://validadorws.cfdi.mx.konesh.com", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("cad", IsNullable=true)]
        public string[] cad;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://validadorws.cfdi.mx.konesh.com", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string token;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://validadorws.cfdi.mx.konesh.com", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string password;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://validadorws.cfdi.mx.konesh.com", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string user;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://validadorws.cfdi.mx.konesh.com", Order=4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string cuenta;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://validadorws.cfdi.mx.konesh.com", Order=5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string rfcReceptor;
        
        public validarDocumentosRequest() {
        }
        
        public validarDocumentosRequest(string[] cad, string token, string password, string user, string cuenta, string rfcReceptor) {
            this.cad = cad;
            this.token = token;
            this.password = password;
            this.user = user;
            this.cuenta = cuenta;
            this.rfcReceptor = rfcReceptor;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="validarDocumentosResponse", WrapperNamespace="http://validadorws.cfdi.mx.konesh.com", IsWrapped=true)]
    public partial class validarDocumentosResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://validadorws.cfdi.mx.konesh.com", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string @return;
        
        public validarDocumentosResponse() {
        }
        
        public validarDocumentosResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ValidadorIntegradoresPortTypeChannel : SatWrapper.CFDIService.ValidadorIntegradoresPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ValidadorIntegradoresPortTypeClient : System.ServiceModel.ClientBase<SatWrapper.CFDIService.ValidadorIntegradoresPortType>, SatWrapper.CFDIService.ValidadorIntegradoresPortType {
        
        public ValidadorIntegradoresPortTypeClient() {
        }
        
        public ValidadorIntegradoresPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ValidadorIntegradoresPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ValidadorIntegradoresPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ValidadorIntegradoresPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SatWrapper.CFDIService.validarDocumentosResponse SatWrapper.CFDIService.ValidadorIntegradoresPortType.validarDocumentos(SatWrapper.CFDIService.validarDocumentosRequest request) {
            return base.Channel.validarDocumentos(request);
        }
        
        public string validarDocumentos(string[] cad, string token, string password, string user, string cuenta, string rfcReceptor) {
            SatWrapper.CFDIService.validarDocumentosRequest inValue = new SatWrapper.CFDIService.validarDocumentosRequest();
            inValue.cad = cad;
            inValue.token = token;
            inValue.password = password;
            inValue.user = user;
            inValue.cuenta = cuenta;
            inValue.rfcReceptor = rfcReceptor;
            SatWrapper.CFDIService.validarDocumentosResponse retVal = ((SatWrapper.CFDIService.ValidadorIntegradoresPortType)(this)).validarDocumentos(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SatWrapper.CFDIService.validarDocumentosResponse> SatWrapper.CFDIService.ValidadorIntegradoresPortType.validarDocumentosAsync(SatWrapper.CFDIService.validarDocumentosRequest request) {
            return base.Channel.validarDocumentosAsync(request);
        }
        
        public System.Threading.Tasks.Task<SatWrapper.CFDIService.validarDocumentosResponse> validarDocumentosAsync(string[] cad, string token, string password, string user, string cuenta, string rfcReceptor) {
            SatWrapper.CFDIService.validarDocumentosRequest inValue = new SatWrapper.CFDIService.validarDocumentosRequest();
            inValue.cad = cad;
            inValue.token = token;
            inValue.password = password;
            inValue.user = user;
            inValue.cuenta = cuenta;
            inValue.rfcReceptor = rfcReceptor;
            return ((SatWrapper.CFDIService.ValidadorIntegradoresPortType)(this)).validarDocumentosAsync(inValue);
        }
    }
}