using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace PrestaZic
{
    public class ClientController : ApiController
    {
        [HttpGet]
        public IHttpActionResult OpenMenu()
        {
            Task.Run( () => { transferToDrive transferToDrive = new transferToDrive(); });
            return Ok("Sending files to drive...");
        }
    }
}
