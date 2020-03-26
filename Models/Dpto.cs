using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SyscafeAppPwa.Models
{
    [DataContract]
    public class Dpto
    {
        [DataMember]
        public int codigo { get; set; }
        [DataMember]
        public string codigoiso { get; set; }
        [DataMember]
        public string nombre { get; set; }
      
        public static async Task<string> GetResponse()
        {
            //-----https://cors-anywhere.herokuapp.com/
            string _url    = "https://wstest123.azurewebsites.net/Service1.svc";
            string _action = "http://tempuri.org/IWSPersonas/ObtenerPersona";
            string soapEnvelopeXml = GetSoapEnvelope();            
            string soapResult = "";
            Dictionary<string, string> DataResponse = new Dictionary<string, string>();

            try
            {
                using (HttpClient client = CreateHttpClient(_action))
                {
                    HttpContent content = new StringContent(soapEnvelopeXml, Encoding.UTF8, "text/xml");
                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, _url) {  Content = content  };
                    using (var response = await client.SendAsync(requestMessage))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            soapResult   = await response.Content.ReadAsStringAsync();
                            //DataResponse = FormatXmlResponse(soapResult);
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
            return soapResult;
        }
        #region
        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/plain;charset=\"utf-8\"";
            webRequest.Accept = "text/plain";
            webRequest.Method = "GET";
            webRequest.Proxy = null;
            webRequest.KeepAlive = true;
            return webRequest;
        }
        #endregion
        // HTTP CLient
        private static HttpClient CreateHttpClient(string action)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            client.DefaultRequestHeaders.Add("SOAPAction", action);
            client.DefaultRequestHeaders.Add("Host", "tbonline-webapp.azurewebsites.net");
            client.DefaultRequestHeaders.Host = "tbonline-webapp.azurewebsites.net";
            return client;
        }

        //private static XmlDocument CreateSoapEnvelope()
        private static string GetSoapEnvelope()
        {
            //return "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tem=\"http://tempuri.org/\">" +
            //            "<soapenv:Header/>" +
            //            "<soapenv:Body>" +
            //                "<tem:GetDpto/>" +
            //            "</soapenv:Body>" +
            //        "</soapenv:Envelope> ";

            return "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tem=\"http://tempuri.org/\">"+
                   "<soapenv:Header/>"+
                   "<soapenv:Body>"+
                      "<tem:ObtenerPersona>"+
                         "<tem:Identificacion>1</tem:Identificacion>" +
                      "</tem:ObtenerPersona>"+
                   "</soapenv:Body>"+
                "</soapenv:Envelope>";
        }

        /**
         * 
         */
        public static Dictionary<string, string> FormatXmlResponse(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            Dictionary<string, string> format = new Dictionary<string, string>();
            XmlNodeList ListNodes = xmlDoc.SelectNodes("a:Mciudad");
            string list = "";
            foreach (XmlNode Node in ListNodes)
            {
                list += Node["codigo"].InnerText;
                list += Node["codigoiso"].InnerText;
                list += Node["nombre"].InnerText;
                format.Add(Node["codigo"].InnerText, list);   
            }
            return format;
        }
    }
}
