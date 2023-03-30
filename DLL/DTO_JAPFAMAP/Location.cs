using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DTOJAPFAMAP
{
    public class Location
    {
        public int id { get; set; }
        public string mLocation { get; set; }
        public double mLat { get; set; }
        public double mLong { get; set; }
        public string mUnit { get; set; }
        public string mRegion { get; set; }
        public string mDivision { get; set; }
        public string mContact { get; set; }
        public string mNumberPhone { get; set; }
        public string mAddress { get; set; }
        public bool selected { get; set; }
        public string mLocationLink { get; set; }
        public int LOCATIONSTATUS { get; set; }
        public string NOTE { get; set; }
        public string uPDATEEMAIL { get; set; }
        public string locationStatusName { get; set; }
        public int esigningID { get; set; }

        public Nullable<System.DateTime> UPDATETIME { get; set; }



    }
}
