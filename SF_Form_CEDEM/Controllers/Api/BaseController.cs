using Inacap.Security.Access;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SF_Form_CEDEM.BL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SF_Form_CEDEM.Controllers.Api
{
    [RoutePrefix("api")]

    public class BaseController : ApiController
    {
        public PlataformaOracle db;
        public BaseController() => db = new PlataformaOracle();

        public string getUrl(int tipo_url)
        {
            //tipo_url = 1;
            //tipo_url = 2; //JWT_AUDIENCE_TOKEN --JWT_ISSUER_TOKEN 
            string baseUrl;

            string dns = HttpContext.Current.Request.Url.DnsSafeHost;
            if (dns == "localhost" && tipo_url == 1)
            {
                return baseUrl = "http://" + HttpContext.Current.Request.Url.Authority + "/";

            }
            else if (dns == "localhost" && tipo_url == 2)
            {
                return baseUrl = "http://localhost:52111/CRM_SF/API_PUBLIC";
            }
            else
            {
                //return baseUrl = "https://" + HttpContext.Current.Request.Url.Authority;
                return baseUrl = "https://" + HttpContext.Current.Request.Url.Authority + "/CRM_SF/API_PUBLIC/";
            }

        }

        [HttpGet]
        [Route("health")]
        public async Task<HttpResponseMessage> IsWorking()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Endpoint funcionando correctamente!");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


    }
}
