using DLL;
using DLL.DTO;
using DLL.DTOJAPFAMAP;
using DLL.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BLL
{
    public class JapfaMap
    {

        private UnitOfWorkJapfaMap unitOfWorkJapfaMap;
        private UnitOfWorkLBCosting unitOfWorkLBCosting;
        public JapfaMap(UnitOfWorkJapfaMap unitOfWorkJapfaMap, UnitOfWorkLBCosting unitOfWorkLBCosting)
        {
            this.unitOfWorkJapfaMap = unitOfWorkJapfaMap;
            this.unitOfWorkLBCosting = unitOfWorkLBCosting;
        }


        public IEnumerable<Location> GetLocations(string divisionID)
        {
            if (divisionID == null)
            {
                return unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().Where(x => x.LOCATIONSTATUS != 2 && x.LOCATIONSTATUS != 0)// khong phai la add hoặc lock thi lay ra
                          .Select(x => new Location
                          {
                              id = (int)x.IDLOCATION,
                              mLocation = x.MLOCATION,
                              mLat = (double)x.MLAT,
                              mLong = (double)x.MLONG,
                              mUnit = x.MSUBDIVISION,
                              mRegion = x.MREGION,
                              mDivision = x.MDIVISION,
                              mContact = x.MCONTACT,
                              mNumberPhone = x.MNUMBERPHONE,
                              mAddress = x.MADDRESS,
                              mLocationLink = x.IDLOCATIONLINK,
                              selected = true,
                          }).OrderByDescending(x => x.id);
            }
            else
            {
                return unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().Where(x => x.MDIVISION == divisionID && x.LOCATIONSTATUS != 2 && x.LOCATIONSTATUS !=0)// khong phai la add hoặc lock thi lay ra
                        .Select(x => new Location
                        {
                            id = (int)x.IDLOCATION,
                            mLocation = x.MLOCATION,
                            mLat = (double)x.MLAT,
                            mLong = (double)x.MLONG,
                            mUnit = x.MSUBDIVISION,
                            mRegion = x.MREGION,
                            mDivision = x.MDIVISION,
                            mContact = x.MCONTACT,
                            mNumberPhone = x.MNUMBERPHONE,
                            mAddress = x.MADDRESS,
                            mLocationLink = x.IDLOCATIONLINK,
                            selected = true,
                        }).OrderByDescending(x => x.id);
            }
           
        }

        //for Escale, poultry...
        public object GetLocationWithIDLink(string idLink)
        {
            var location = unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().FirstOrDefault(x => x.IDLOCATIONLINK == idLink);
            if (location != null)
            {
                return new
                {
                    mLocation = location.MLOCATION,
                    mAddress = location.MADDRESS,
                    mLat = location.MLAT,
                    mLong = location.MLONG,
                };
            }
            else
            {
                return new
                {
                    mLocation = "",
                    mAddress = "",
                    mLat = 0.0,
                    mLong = 0.0
                };
            }

        }

        //de chon khi link
        public IEnumerable<Object> GetLocationsFromPoultry()
        {
            return unitOfWorkLBCosting.V_FARMER_GETLIST.GetAll()
                              .Select(x => new
                              {
                                  id = x.FARMERCODE,
                                  name = x.FARMERNAMER,
                              });

        }

        public List<ComboboxDTO> GetRegions()
        {
            var regions = new List<ComboboxDTO>();
            regions.Add(new ComboboxDTO { id = "All", name = "All" });
            regions.Add(new ComboboxDTO { id = "North", name = "North" });
            regions.Add(new ComboboxDTO { id = "South", name = "South" });

            return regions;
        }

        public List<ComboboxDTO> GetDivisions()
        {
            var division = new List<ComboboxDTO>();
            division.Add(new ComboboxDTO { id = "All", name = "All" });
            division.Add(new ComboboxDTO { id = "Feed", name = "Feed" });
            division.Add(new ComboboxDTO { id = "Swine", name = "Swine" });
            division.Add(new ComboboxDTO { id = "Poultry", name = "Poultry" });
            division.Add(new ComboboxDTO { id = "Go Direct", name = "Go Direct" });
            division.Add(new ComboboxDTO { id = "HO", name = "HO" });

            return division;
        }

        public List<ComboboxDTO> GetUnits()
        {
            var unit = new List<ComboboxDTO>();
            //All
            unit.Add(new ComboboxDTO { id = "All", name = "All", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "GGP", name = "GGP", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "GP", name = "GP", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "PS", name = "PS", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "CGF", name = "CGF", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "RF", name = "RF", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "HTC", name = "HTC", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "LAB", name = "LAB", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "Processing", name = "Processing", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "Fresh Meat", name = "Fresh Meat", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "FM", name = "Feed Mill", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "CGC", name = "CGC", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "CGD", name = "CGD", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "CGL", name = "CGL", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "FAT", name = "FAT", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "Office", name = "Office", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "PUL", name = "PUL", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "GILT", name = "GILT", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "SELLING", name = "SELLING", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "WASHINGBAY", name = "WASHINGBAY", groupID = "All" });
            unit.Add(new ComboboxDTO { id = "NUR", name = "NUR", groupID = "All" });

            //Swine
            unit.Add(new ComboboxDTO { id = "All", name = "All", groupID = "Swine" });
            unit.Add(new ComboboxDTO { id = "GGP", name = "GGP", groupID = "Swine" });
            unit.Add(new ComboboxDTO { id = "GP", name = "GP", groupID = "Swine" });
            unit.Add(new ComboboxDTO { id = "PS", name = "PS", groupID = "Swine" });
            unit.Add(new ComboboxDTO { id = "CGF", name = "CGF", groupID = "Swine" });
            unit.Add(new ComboboxDTO { id = "RF", name = "RF", groupID = "Swine" });
            unit.Add(new ComboboxDTO { id = "FAT", name = "FAT", groupID = "Swine" });
            unit.Add(new ComboboxDTO { id = "GILT", name = "GILT", groupID = "Swine" });
            unit.Add(new ComboboxDTO { id = "SELLING", name = "SELLING", groupID = "Swine" });
            unit.Add(new ComboboxDTO { id = "WASHINGBAY", name = "WASHINGBAY", groupID = "Swine" });
            unit.Add(new ComboboxDTO { id = "NUR", name = "NUR", groupID = "Swine" });

            //Poultry
            unit.Add(new ComboboxDTO { id = "All", name = "All", groupID = "Poultry" });
            unit.Add(new ComboboxDTO { id = "GP", name = "GP", groupID = "Poultry" });
            unit.Add(new ComboboxDTO { id = "PS", name = "PS", groupID = "Poultry" });
            unit.Add(new ComboboxDTO { id = "CGF", name = "CGF", groupID = "Poultry" });
            unit.Add(new ComboboxDTO { id = "CGC", name = "CGC", groupID = "Poultry" });
            unit.Add(new ComboboxDTO { id = "CGD", name = "CGD", groupID = "Poultry" });
            unit.Add(new ComboboxDTO { id = "CGL", name = "CGL", groupID = "Poultry" });
            unit.Add(new ComboboxDTO { id = "RF", name = "RF", groupID = "Poultry" });
            unit.Add(new ComboboxDTO { id = "HTC", name = "HTC", groupID = "Poultry" });
            unit.Add(new ComboboxDTO { id = "LAB", name = "LAB", groupID = "Poultry" });
            unit.Add(new ComboboxDTO { id = "PUL", name = "PUL", groupID = "Poultry" });


            //GoDirect
            unit.Add(new ComboboxDTO { id = "All", name = "All", groupID = "Go Direct" });
            unit.Add(new ComboboxDTO { id = "Processing", name = "Processing", groupID = "Go Direct" });
            unit.Add(new ComboboxDTO { id = "Fresh Meat", name = "Fresh Meat", groupID = "Go Direct" });

            //Feed
            unit.Add(new ComboboxDTO { id = "All", name = "All", groupID = "Feed" });
            unit.Add(new ComboboxDTO { id = "Feed Mill", name = "Feed Mill", groupID = "Feed" });

            //HO
            unit.Add(new ComboboxDTO { id = "All", name = "All", groupID = "HO" });
            unit.Add(new ComboboxDTO { id = "Office", name = "Office", groupID = "HO" });

            return unit;
        }

        public IEnumerable<ComboboxDTO> GetLocationStatus()
        {
            return unitOfWorkJapfaMap.TBLOCATIONSTATU_Repository.GetAll()
                .Select(x => new ComboboxDTO { id = x.ID.ToString(), name = x.NAME, idNum = (int)x.ID }).OrderBy(x=>x.idNum);
        }

        #region picture
        public int getPictureCount(int locationID)
        {
            return unitOfWorkJapfaMap.TBPICTUREFORLOCATION_Repository.GetAll().Where(x => x.TBLOCATIONID == locationID).Count();
        }

        public IEnumerable<Common> getPicture(int locationID)
        {
            return unitOfWorkJapfaMap.TBPICTUREFORLOCATION_Repository.GetAll().Where(x => x.TBLOCATIONID == locationID)
                .Select(x => new Common
                {
                    idNum = (int)x.ID,
                    link = "https://e-approval.japfa.com.vn:62071/pictures/" + locationID + "/" + x.PICTURENAME,
                    name = x.PICTURENAME

                }).OrderBy(x => x.idNum).ToList();
        }

        public int installPicture(string pictureName, int locationID)
        {

            var tbLocation = unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().FirstOrDefault(x => x.IDLOCATION == locationID);
            //truong hop = 1 hoac = 4 la editting, hoac = 3 dang la delete thi co the chuyen trang thai sang editting.
            if (tbLocation.LOCATIONSTATUS != 2)
            {
                tbLocation.LOCATIONSTATUS = 4;//neu dang la delete thi co the chuyen trang thai sang update
                unitOfWorkJapfaMap.TBLLOCATION_Repository.Update(tbLocation);

            }

            int id = (int)unitOfWorkJapfaMap.TBPICTUREFORLOCATION_Repository.GetAll().DefaultIfEmpty().Max(x => x == null ? 0 : x.ID) + 1;
            TBPICTUREFORLOCATION tBPICTUREFORLOCATION = new TBPICTUREFORLOCATION
            {
                ID = id,
                PICTURENAME = pictureName,
                TBLOCATIONID = locationID,
                UPDATETIME = DateTime.Now
            };
            unitOfWorkJapfaMap.TBPICTUREFORLOCATION_Repository.Add(tBPICTUREFORLOCATION);
            return unitOfWorkJapfaMap.Save();
        }

        public int deletePicture(string pictureName, int locationID)
        {

            TBPICTUREFORLOCATION tBPICTUREFORLOCATION = unitOfWorkJapfaMap.TBPICTUREFORLOCATION_Repository.GetAll()
                .FirstOrDefault(x => x.TBLOCATIONID == locationID && x.PICTURENAME == pictureName);
            ;
            if (tBPICTUREFORLOCATION != null)
            {
                //xoa tren webserver
                string rootFolder = HttpContext.Current.Server.MapPath("~/pictures/" + locationID);
                string originalFileName = String.Concat(rootFolder, "\\" + pictureName);
                if (File.Exists(originalFileName))
                {
                    File.Delete(originalFileName);
                }

                //xoa trong db
                unitOfWorkJapfaMap.TBPICTUREFORLOCATION_Repository.Delete(tBPICTUREFORLOCATION);
            }
            return unitOfWorkJapfaMap.Save();
        }
        #endregion


        #region location control
        public IEnumerable<object> GetLocationsForControl()
        {
            return unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().AsEnumerable()
                     .Select(x =>
                     {
                         if (x.TBLOCATIONUPDATING != null)
                         {
                             return new Location
                             {
                                 id = (int)x.IDLOCATION,
                                 mLocation = x.TBLOCATIONUPDATING.MLOCATION,
                                 mLat = (double)x.TBLOCATIONUPDATING.MLAT,
                                 mLong = (double)x.TBLOCATIONUPDATING.MLONG,
                                 mUnit = x.TBLOCATIONUPDATING.MSUBDIVISION,
                                 mRegion = x.TBLOCATIONUPDATING.MREGION,
                                 mDivision = x.TBLOCATIONUPDATING.MDIVISION,
                                 mContact = x.TBLOCATIONUPDATING.MCONTACT,
                                 mNumberPhone = x.TBLOCATIONUPDATING.MNUMBERPHONE,
                                 mAddress = x.TBLOCATIONUPDATING.MADDRESS,
                                 mLocationLink = x.TBLOCATIONUPDATING.IDLOCATIONLINK,
                                 selected = true,
                                 LOCATIONSTATUS = (int)x.LOCATIONSTATUS,
                                 NOTE = x.TBLOCATIONUPDATING.NOTE,
                                 locationStatusName = x.TBLOCATIONSTATU.NAME,
                                 uPDATEEMAIL = x.UPDATEEMAIL,
                                 UPDATETIME = x.UPDATETIME
                             };

                         }
                         else
                             return new Location
                             {
                                 id = (int)x.IDLOCATION,
                                 mLocation = x.MLOCATION,
                                 mLat = (double)x.MLAT,
                                 mLong = (double)x.MLONG,
                                 mUnit = x.MSUBDIVISION,
                                 mRegion = x.MREGION,
                                 mDivision = x.MDIVISION,
                                 mContact = x.MCONTACT,
                                 mNumberPhone = x.MNUMBERPHONE,
                                 mAddress = x.MADDRESS,
                                 mLocationLink = x.IDLOCATIONLINK,
                                 selected = true,
                                 LOCATIONSTATUS = (int)x.LOCATIONSTATUS,
                                 NOTE = x.NOTE,
                                 locationStatusName = x.TBLOCATIONSTATU.NAME,
                                 uPDATEEMAIL = x.UPDATEEMAIL,
                                 UPDATETIME = x.UPDATETIME
                             };

                     }).OrderByDescending(x => x.id);


        }

        //khi send sang approval se call de ham nay de update them E-SigningID
        public int save(string email, Location location)
        {
            //new
            if (location.id == -2)
            {
                var id = unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().DefaultIfEmpty().Max(x => x == null ? 0 : x.IDLOCATION) + 1;
                var tbLocation = new TBLLOCATION
                {
                    IDLOCATION = id,
                    MLOCATION = location.mLocation,
                    MADDRESS = location.mAddress,
                    MCONTACT = location.mContact,
                    MSUBDIVISION = location.mUnit,
                    MDIVISION = location.mDivision,
                    MREGION = location.mRegion,
                    MLAT = (decimal)location.mLat,
                    MLONG = (decimal)location.mLong,
                    IDLOCATIONLINK = location.mLocationLink,
                    MNUMBERPHONE = location.mNumberPhone,
                    UPDATETIME = DateTime.Now,
                    UPDATEEMAIL = email,
                    NOTE = location.NOTE,
                    LOCATIONSTATUS = 2,
                    ESIGNINGID = location.esigningID,
                };
                unitOfWorkJapfaMap.TBLLOCATION_Repository.Add(tbLocation);
                unitOfWorkJapfaMap.Save();
                return (int)id;


            }
            else
            {
                var tbLocation = unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().FirstOrDefault(x => x.IDLOCATION == location.id);
                //truong hop add thi update vao chinh tblocation
                if (location.LOCATIONSTATUS == 2)
                {
                    tbLocation.MLOCATION = location.mLocation;
                    tbLocation.MADDRESS = location.mAddress;
                    tbLocation.MCONTACT = location.mContact;
                    tbLocation.MSUBDIVISION = location.mUnit;
                    tbLocation.MDIVISION = location.mDivision;
                    tbLocation.MREGION = location.mRegion;
                    tbLocation.MLAT = (decimal)location.mLat;
                    tbLocation.MLONG = (decimal)location.mLong;
                    tbLocation.IDLOCATIONLINK = location.mLocationLink;
                    tbLocation.MNUMBERPHONE = location.mNumberPhone;
                    tbLocation.UPDATETIME = DateTime.Now;
                    tbLocation.UPDATEEMAIL = email;
                    tbLocation.NOTE = location.NOTE;
                    tbLocation.ESIGNINGID = location.esigningID;
                }
                //truong hop = 1 hoac = 4 la editting, hoac = 3 dang la delete thi co the chuyen trang thai sang editting.
                else
                {
                    //chua co thi add
                    if (tbLocation.TBLOCATIONUPDATING == null)
                    {
                        var id = unitOfWorkJapfaMap.TBLOCATIONUPDATING_Repository.GetAll().DefaultIfEmpty().Max(x => x == null ? 0 : x.ID) + 1;
                        var tBLOCATIONUPDATING = new TBLOCATIONUPDATING
                        {
                            MLOCATION = location.mLocation,
                            MADDRESS = location.mAddress,
                            MCONTACT = location.mContact,
                            MSUBDIVISION = location.mUnit,
                            MDIVISION = location.mDivision,
                            MREGION = location.mRegion,
                            MLAT = (decimal)location.mLat,
                            MLONG = (decimal)location.mLong,
                            IDLOCATIONLINK = location.mLocationLink,
                            MNUMBERPHONE = location.mNumberPhone,
                            UPDATETIME = DateTime.Now,
                            UPDATEEMAIL = email,
                            ID = id,
                            NOTE = location.NOTE,
                        };
                        unitOfWorkJapfaMap.TBLOCATIONUPDATING_Repository.Add(tBLOCATIONUPDATING);

                        tbLocation.IDLOCATIONUPDATE = id;
                        tbLocation.LOCATIONSTATUS = 4;
                        tbLocation.ESIGNINGID = location.esigningID;
                        unitOfWorkJapfaMap.TBLLOCATION_Repository.Update(tbLocation);
                    }
                    //co roi thi update
                    else
                    {
                        tbLocation.TBLOCATIONUPDATING.MLOCATION = location.mLocation;
                        tbLocation.TBLOCATIONUPDATING.MADDRESS = location.mAddress;
                        tbLocation.TBLOCATIONUPDATING.MCONTACT = location.mContact;
                        tbLocation.TBLOCATIONUPDATING.MSUBDIVISION = location.mUnit;
                        tbLocation.TBLOCATIONUPDATING.MDIVISION = location.mDivision;
                        tbLocation.TBLOCATIONUPDATING.MREGION = location.mRegion;
                        tbLocation.TBLOCATIONUPDATING.MLAT = (decimal)location.mLat;
                        tbLocation.TBLOCATIONUPDATING.MLONG = (decimal)location.mLong;
                        tbLocation.TBLOCATIONUPDATING.IDLOCATIONLINK = location.mLocationLink;
                        tbLocation.TBLOCATIONUPDATING.MNUMBERPHONE = location.mNumberPhone;
                        tbLocation.TBLOCATIONUPDATING.UPDATETIME = DateTime.Now;
                        tbLocation.TBLOCATIONUPDATING.UPDATEEMAIL = email;

                        unitOfWorkJapfaMap.TBLOCATIONUPDATING_Repository.Update(tbLocation.TBLOCATIONUPDATING);

                        tbLocation.LOCATIONSTATUS = 4;//neu dang la delete thi co the chuyen trang thai sang update
                        tbLocation.ESIGNINGID = location.esigningID;
                        unitOfWorkJapfaMap.TBLLOCATION_Repository.Update(tbLocation);
                    }
                }


                unitOfWorkJapfaMap.Save();
                return location.id;
            }


        }

        //tren giao dien E-Signing hoac khi, cap tren phe duyet se call sang
        public int approval(string email, int id)
        {
            var location = unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().FirstOrDefault(x => x.IDLOCATION == id);
            if (location != null)
            {
                if (location.LOCATIONSTATUS == 2)
                {
                    location.LOCATIONSTATUS = 1;
                    location.UPDATEEMAIL = email;
                    unitOfWorkJapfaMap.TBLLOCATION_Repository.Update(location);
                }
                else if (location.LOCATIONSTATUS == 3)
                {
                    if (location.TBLOCATIONUPDATING != null)
                    {
                        unitOfWorkJapfaMap.TBLOCATIONUPDATING_Repository.Delete(location.TBLOCATIONUPDATING);
                    }
                    unitOfWorkJapfaMap.TBLLOCATION_Repository.Delete(location);
                }
                else if (location.LOCATIONSTATUS == 4)
                {
                    location.MLOCATION = location.TBLOCATIONUPDATING.MLOCATION;
                    location.MADDRESS = location.TBLOCATIONUPDATING.MADDRESS;
                    location.MCONTACT = location.TBLOCATIONUPDATING.MCONTACT;
                    location.MSUBDIVISION = location.TBLOCATIONUPDATING.MSUBDIVISION;
                    location.MDIVISION = location.TBLOCATIONUPDATING.MDIVISION;
                    location.MREGION = location.TBLOCATIONUPDATING.MREGION;
                    location.MLAT = location.TBLOCATIONUPDATING.MLAT;
                    location.MLONG = location.TBLOCATIONUPDATING.MLONG;
                    location.IDLOCATIONLINK = location.TBLOCATIONUPDATING.IDLOCATIONLINK;
                    location.MNUMBERPHONE = location.TBLOCATIONUPDATING.MNUMBERPHONE;
                    location.UPDATETIME = location.TBLOCATIONUPDATING.UPDATETIME;
                    location.UPDATEEMAIL = location.UPDATEEMAIL;
                    location.LOCATIONSTATUS = 1;

                    unitOfWorkJapfaMap.TBLLOCATION_Repository.Update(location);

                    if (location.TBLOCATIONUPDATING != null)
                    {
                        unitOfWorkJapfaMap.TBLOCATIONUPDATING_Repository.Delete(location.TBLOCATIONUPDATING);
                    }

                }
            }



            unitOfWorkJapfaMap.SaveLog(email);
            return unitOfWorkJapfaMap.Save();

        }

        //tren giao dien E-Signing hoac khi, cap tren phe duyet se call sang
        public int reject(string email,  int id)
        {

            var location = unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().FirstOrDefault(x => x.IDLOCATION == id);
            if (location != null)
            {
                if (location.LOCATIONSTATUS == 2)
                {
                    unitOfWorkJapfaMap.TBLLOCATION_Repository.Delete(location);
                }
                else if (location.LOCATIONSTATUS == 3|| location.LOCATIONSTATUS == 4)
                {
                    if (location.TBLOCATIONUPDATING != null)
                    {
                        unitOfWorkJapfaMap.TBLOCATIONUPDATING_Repository.Delete(location.TBLOCATIONUPDATING);
                    }
                    location.LOCATIONSTATUS = 1;
                    location.UPDATEEMAIL = email;
                    unitOfWorkJapfaMap.TBLLOCATION_Repository.Update(location);
                }
            }

            unitOfWorkJapfaMap.SaveLog(email);
            return unitOfWorkJapfaMap.Save();

        }

        //chi tren japfamaps
        public int delete(string email, string locationName, int id)
        {
            var location = unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().FirstOrDefault(x => x.IDLOCATION == id);
            if (location != null)
            {
                location.LOCATIONSTATUS = 3;
            }
            unitOfWorkJapfaMap.SaveLog(email);
            return unitOfWorkJapfaMap.Save();
        }

        //recheck voi trung, vi day la update truc tiep tu poultry
        public int UpdateLocationWithIDLink(string idLink, decimal dLat, decimal dLong, string updateEmail, string updateLocationKey)
        {
            if (updateLocationKey != "thankyou")
            {
                throw new Exception("Vui lòng kiểm tra lại updateLocationKey");
            }

            var location = unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().FirstOrDefault(x => x.IDLOCATIONLINK == idLink);
            if (location != null)
            {
                location.MLAT = dLat;
                location.MLONG = dLong;
                location.UPDATEEMAIL = updateEmail;
            }
            else
            {
                throw new Exception("Không tìm thấy location ứng với ID link này!");
            }
            return unitOfWorkJapfaMap.Save();
        }

        //recheck voi trung, vi day la update truc tiep tu poultry
        public int UpdateLocationWithIDLinkV1(string idLink, decimal dLat, decimal dLong,string locationName,string locationAddress,int locationStatus,  string updateEmail, string updateLocationKey)
        {
            if (updateLocationKey != "thankyou")
            {
                throw new Exception("Vui lòng kiểm tra lại updateLocationKey");
            }
            if (locationStatus != 0 || locationStatus != 1)
            {
                throw new Exception("locationStatus chi co the la 0 hoac 1");
            }
            var location = unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().FirstOrDefault(x => x.IDLOCATIONLINK == idLink);
            if (location != null)
            {
                location.MLAT = dLat;
                location.MLONG = dLong;
                location.UPDATEEMAIL = updateEmail;
                location.MLOCATION = locationName;
                location.MADDRESS = locationAddress;
                location.LOCATIONSTATUS = locationStatus;
            }
            else
            {
                throw new Exception("Không tìm thấy location ứng với ID link này!");
            }
            return unitOfWorkJapfaMap.Save();
        }

        //khi ben E-Approval reject hoa approval thi se update lai japfamaps
        public int UpdateStatusForJapfaMapFromEApproval(DocumentInfo documentInfo)
        {
            int i = -1;
          
            //Request 1
            //Request 2
            var locationUpdating = unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().FirstOrDefault(x => x.ESIGNINGID == documentInfo.DocumentId);
            if (locationUpdating != null)
            {
                if (documentInfo.DocumentStatusId == 3)//complate
                {
                    if (locationUpdating != null)
                    {
                        i = i + approval(locationUpdating.UPDATEEMAIL, (int)locationUpdating.IDLOCATION);
                    }
                }
                else if (documentInfo.DocumentStatusId == 4) //reject
                {
                    if (locationUpdating != null)
                    {
                        i = i + reject(locationUpdating.UPDATEEMAIL, (int)locationUpdating.IDLOCATION);
                    }
                }
            }
            else
            {
                // khong can, vi tren he thong japfa maps chi nhan yeu cau cuoi cung. vi esigningID hien chi luu request cuoi cung.
                //va location update dang tim theo E-SigningID
                //neu muon approval tat qua request thi E-Signing tra ve them OriginID. va japfamaps se tim theo ID nay va update theo
                //nhuoc diem la Ai approval truoc se update truoc, ai approval sau se update sau va khong theo thu tu request
                //throw new Exception("");
            }
            return i;
        }

        //send dang E-Approval se thuc hien tu client


        public int unlockLocation(string email, int id)
        {
                var location = unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().FirstOrDefault(x => x.IDLOCATION == id);
                if (location != null)
                {
                location.LOCATIONSTATUS = 1;
                location.UPDATEEMAIL = email;
                }
          return  unitOfWorkJapfaMap.Save();
        }

        public int lockLocation(string email, int id)
        {
            var location = unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().FirstOrDefault(x => x.IDLOCATION == id);
            if (location != null)
            {
                location.LOCATIONSTATUS = 0;
                location.UPDATEEMAIL = email;
            }
            return unitOfWorkJapfaMap.Save();
        }

        #endregion


        //public int UpdateLocation()
        //{
        //    unitOfWorkJapfaMap.TBLOCATIONTEMP_Repository.GetAll().ToList().ForEach(location =>
        //    {
        //        var check = unitOfWorkJapfaMap.TBLLOCATION_Repository.GetAll().FirstOrDefault(x => x.IDLOCATION == location.IDLOCATION);
        //        if (check != null)
        //        {
        //            check.IDLOCATIONLINK = location.IDLOCATIONLINK;
        //            check.MLOCATION = location.MLOCATION;
        //            unitOfWorkJapfaMap.TBLLOCATION_Repository.Update(check);
        //        }
        //    });
        //    return unitOfWorkJapfaMap.Save();
        //}


    }
}


public class DocumentInfo
{
    public int DocumentStatusId { get; set; }
    public int DocumentType { get; set; }
    public int DocumentId { get; set; }
    public int OriginalId { get; set; }
}
