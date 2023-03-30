using DLL.DTOJAPFAMAP;
using DLL.Repositories;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BLL.Reports
{
    public class Reports
    {
        private UnitOfWorkJapfaMap unitOfWorkJapfaMap;
        public Reports(UnitOfWorkJapfaMap unitOfWorkJapfaMap)
        {
            this.unitOfWorkJapfaMap = unitOfWorkJapfaMap;
        }

        public byte[] Locations(string fileType, List<Location> locations)
        {
            //header

            //body
            //var Body = unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll()
            //.Select(x =>
            //         new Location
            //         {
            //             id =(int) x.IDLOCATION,
            //             mLocationLink = x.IDLOCATIONLINK,
            //             mLocation = x.MLOCATION,
            //             mLat =(double) x.MLAT,
            //             mLong =(double) x.MLONG,
            //             mUnit = x.MSUBDIVISION,
            //             mRegion = x.MDIVISION,
            //             mDivision = x.MREGION,
            //             mContact = x.MCONTACT,
            //             mNumberPhone = x.MNUMBERPHONE,
            //             mAddress = x.MADDRESS,
            //             UPDATETIME = (System.DateTime)x.UPDATETIME,
            //         });


            ReportViewer report = new ReportViewer();
            report.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/bin/Reports/ReportLocations.rdlc");
            if (fileType == "excel")
            {
                report.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/bin/Reports/ReportLocations.rdlc");
            }
            report.LocalReport.DataSources.Add(new ReportDataSource() { Name = "Body", Value = locations });

            report.LocalReport.EnableExternalImages = true;
            string mimeType;
            string encoding;
            string filenameExtension;
            string[] streamids;
            Warning[] warnings;
            string extension = "PDF";
            if (fileType == "excel") extension = "EXCELOPENXML";
            byte[] bytesFile = report.LocalReport.Render(extension, null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

            return bytesFile;
        }




    }
}
