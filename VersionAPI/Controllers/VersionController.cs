using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLL;

namespace VersionAPI.Controllers
{
    public class VersionController : ApiController
    {

        [HttpGet]
        public string phanquyen()
        {
            return "1.7";
        }

        [HttpGet]
        public string crm()
        {
            return "2.9";
        }

        [HttpGet]
        public string crmit()
        {
            return "2.0";
        }

        [HttpGet]
        public string farmic()
        {
            return "1.5";
        }

        [HttpGet]
        public string jauthen()
        {
            return "1.4";
        }
    }
}
