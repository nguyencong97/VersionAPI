using BLL.Reports;
using DLL.DTOJAPFAMAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace VersionAPI.Controllers
{
    public class ReportsController : ApiController
    {

        private Reports reports;

        public ReportsController(Reports reports)
        {
            this.reports = reports;
        }

        [HttpPost]
        public IHttpActionResult Locations(string fileType,[FromBody] List<Location> locations)
        {
            try
            {
                return Ok(reports.Locations(fileType, locations));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

    }
}
