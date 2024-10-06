using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows.Controls;
using System.Xml;
using System.Reflection;

namespace PrestaZic
{
    public class AdministrationController : ApiController
    {
        readonly static Log log = new Log();

        [HttpGet]
        public IHttpActionResult encrypt()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Request.Content.ReadAsStringAsync().Result);

            if (xmlDoc.InnerText == ConfigurationManager.AppSettings["MasterPassword"].ToString())
            {
                ConfigApp.encrypt();
                log.WriteToFile("Encryption of config app... Done");
                return Ok("Application is now secured !");
            }
            else
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error : Please identify you"));
            }
        }

        [HttpGet]
        public IHttpActionResult unencrypt()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Request.Content.ReadAsStringAsync().Result);

            if (xmlDoc.InnerText == ConfigurationManager.AppSettings["MasterPassword"].ToString())
            {
                ConfigApp.decrypt();
                log.WriteToFile("Unencryption of config app... Done");
                return Ok("Application is now editable !");
            }
            else
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error : Please identify you"));
            }
        }

        [HttpGet]
        public IHttpActionResult version()
        {
            return Ok("PrestaZic service, version " + Assembly.GetEntryAssembly().GetName().Version);
        }
    }
}
