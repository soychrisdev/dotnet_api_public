using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SF_Form_CEDEM.BL
{
    class PlataformaSalesForce
    {
        private string TOKEN_EXPIRE_MINUTES = ConfigurationManager.AppSettings["TOKEN_EXPIRE_MINUTES"];
        private string nombreFormulario = ConfigurationManager.AppSettings["nameForm"];


        public string UserName { get; set; }
        public string Password { get; set; }
        //public string Token { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string LoginEndpoint { get; set; }
        public string ApiEndpoint { get; set; }
        public string ApiRest { get; set; }
        public string AuthToken { get; set; }
        public string InstanceURL { get; set; }

        static PlataformaSalesForce()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }
        public string GetToken()
        {

            var db = new PlataformaOracle();
            var accessToken = db.GetAccessToken(nombreFormulario);
            String jsonResponse;
            if (string.IsNullOrEmpty(accessToken))
            {

                using (var client = new HttpClient())
                {
                    var request = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type","password"},
                    { "client_id",ClientId},
                    { "client_secret",ClientSecret},
                    { "username",UserName},
                    { "password",Password}
                });

                    request.Headers.Add("X-PreetyPrint", "1");
                    var response = client.PostAsync(LoginEndpoint, request).Result;
                    jsonResponse = response.Content.ReadAsStringAsync().Result;


                }

                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
                accessToken = values["access_token"];
                AuthToken = values["access_token"];
                InstanceURL = values["instance_url"];
                var storedAt = DateTime.Now;
                var expiresAt = DateTime.Now.AddMinutes(Convert.ToDouble(TOKEN_EXPIRE_MINUTES));
                db.GuardarAccessToken(accessToken, nombreFormulario, storedAt, expiresAt, InstanceURL);
                var tokenRetorno = db.GetAccessToken(nombreFormulario);
                accessToken = tokenRetorno;

            }
            return accessToken;

        }

        public string GetData(string nombreMetodo, string AuthToken, string InstanceURL, string ApiRest)
        {
            using (var client = new HttpClient())
            {
                string restRequest = InstanceURL + ApiRest + nombreMetodo;
                var request = new HttpRequestMessage(HttpMethod.Get, restRequest);
                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("X-PreetyPrint", "1");
                var response = client.SendAsync(request).Result;

                return response.Content.ReadAsStringAsync().Result;
            }
        }
        public string Query(string InstanceURL, string ApiEndpoint, string soqlQuery, string AuthToken)
        {
            using (var client = new HttpClient())
            {
                string restRequest = InstanceURL + ApiEndpoint + "query?q=" + soqlQuery;
                var request = new HttpRequestMessage(HttpMethod.Get, restRequest);
                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("X-PreetyPrint", "1");
                var response = client.SendAsync(request).Result;

                return response.Content.ReadAsStringAsync().Result;
            }
        }
        public async Task<string> CreateRecordRest(string jsonObj, string recordType, string InstanceURL, string ApiEndpoint, string AuthToken)
        {
            try
            {
                string response = string.Empty;
                HttpResponseMessage httpResponse;
                ///Se indica la instacia de Salesforce a la que apunta, se indica el objeto a crear y envia el JSON Serializado string.
                string uri = $"{InstanceURL}{ApiEndpoint}{recordType}";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                HttpContent content = new StringContent(jsonObj, Encoding.UTF8, "application/json");

                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = content;

                httpResponse = client.SendAsync(request).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    var db = new PlataformaOracle();
                    response = await httpResponse.Content.ReadAsStringAsync();
                }
                else
                {
                    response = await httpResponse.Content.ReadAsStringAsync();
                    var db = new PlataformaOracle();
                    db.Insertar_log_error(response, "CRM_PORTAL_ARANCELES");
                    throw new Exception("Error HttpClient status:" + httpResponse.StatusCode.ToString());
                }

                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<string> CreateRecord(string jsonObj, string recordType)
        {
            try
            {
                ///Se indica la instacia de Salesforce a la que apunta, se indica el objeto a crear y envia el JSON Serializado string.
                string uri = $"{InstanceURL}{ApiEndpoint}sobjects/{recordType}";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                HttpContent content = new StringContent(jsonObj, Encoding.UTF8, "application/json");

                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = content;

                var response = client.SendAsync(request).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }
        public async Task<string> UpdatedRecord(string url, string jsonObj)
        {
            try
            {
                #region [ Método de Actualización ]
                ///Se genera URL con la Instacia a la que apunta, se agrega la ruta y versión del servicio, se indica el objeto de Salesforce y el ID del registro a modificar
                string uri = InstanceURL + url + "?_HttpMethod=PATCH";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                HttpContent content = new StringContent(jsonObj, Encoding.UTF8, "application/json");

                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = content;

                var response = client.SendAsync(request).Result;
                return response.Content.ReadAsStringAsync().Result;
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<string> DescribeObject(string url)
        {
            try
            {
                string descriptionContent = string.Empty;
                HttpClient queryClient = new HttpClient();
                string restQuery = InstanceURL + ApiEndpoint + url;

                // Create the request
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, restQuery);

                // Add Token to the header
                request.Headers.Add("Authorization", "Bearer " + AuthToken);

                //return JSON to the caller
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Call the endpoint async
                try
                {
                    //HttpResponseMessage response = await queryClient.SendAsync(request);
                    var response = queryClient.SendAsync(request).Result;

                    //descriptionContent = await response.Content.ReadAsStringAsync();
                    descriptionContent = response.Content.ReadAsStringAsync().Result;
                }
                catch (Exception e)
                {
                    descriptionContent = e.Message;
                }

                queryClient.Dispose();

                return descriptionContent;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
