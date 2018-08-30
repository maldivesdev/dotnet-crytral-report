using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DS.Report.Controllers.Connection
{
    public class ConnectionController : Controller
    {
        // GET: Connection
        [HttpGet]
        public string Get()
        {
            return "OK";
        }
    }
}