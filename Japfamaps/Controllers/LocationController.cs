using BLL;
using DLL.DTO;
using DLL.DTOJAPFAMAP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace VersionAPI.Controllers
{
    public class LocationController : ApiController
    {

        private JapfaMap japfaMap;

        public LocationController(JapfaMap japfaMap)
        {
            this.japfaMap = japfaMap;
        }

        [HttpGet]
        public IEnumerable<Location> GetLocations(string divisionID = null)
        {
            return japfaMap.GetLocations(divisionID);
        }

        [HttpGet]
        public IEnumerable<Location> GetLocationsV1(string divisionID = null)
        {
            return japfaMap.GetLocations(divisionID);
        }

        [HttpGet]
        public IEnumerable<Object> GetLocationsFromPoultry()
        {
            return japfaMap.GetLocationsFromPoultry();
        }

        [HttpGet]
        public object GetLocationWithIDLink(string idLink)
        {
            return japfaMap.GetLocationWithIDLink(idLink);
        }

        [Authorize]
        [HttpGet]
        public object GetLocationWithIDLinkV1(string idLink)
        {
            return japfaMap.GetLocationWithIDLink(idLink);
        }


        [HttpGet]
        public List<ComboboxDTO> GetRegions()
        {

            return japfaMap.GetRegions();
        }

        [HttpGet]
        public List<ComboboxDTO> GetDivisions()
        {
            return japfaMap.GetDivisions();
        }

        [HttpGet]
        public List<ComboboxDTO> GetUnits()
        {
            return japfaMap.GetUnits();
        }

         [HttpGet]
        public IHttpActionResult GetLocationStatus()
        {
            return Ok(japfaMap.GetLocationStatus());
        }

        #region picture
        [HttpGet]
        public IHttpActionResult getPicture(int locationID)
        {
            try
            {
                return Ok(japfaMap.getPicture(locationID));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [HttpPost]
        public IHttpActionResult deletePicture(string pictureName, int locationID)
        {
            try
            {
                return Ok(japfaMap.deletePicture(pictureName,locationID));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        //[HttpPost]
        //public async Task<IHttpActionResult> UploadPictureForLocation(int locationID)
        //{
        //    // Check if the request contains multipart/form-data.
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    string rootFolder = HttpContext.Current.Server.MapPath("~/pictures/"+locationID);
        //    if (!Directory.Exists(rootFolder))
        //    {
        //        Directory.CreateDirectory(rootFolder);
        //    }

        //    var provider = new MultipartFormDataStreamProvider(rootFolder);

        //    // Read the form data and return an async task.
        //    var task =await Request.Content.ReadAsMultipartAsync(provider);

        //        //This illustrates how to get the file names.
        //        Random rnd = new Random();

        //        int i = japfaMap.getPictureCount(locationID);
        //        foreach (MultipartFileData file in provider.FileData)
        //        {
        //           var uploadingFileName = file.LocalFileName;
        //            var filename = rnd.Next(100000000)+ "Image" + ++i +".jpg";
        //            string originalFileName = String.Concat(rootFolder, "\\" + filename);
        //            if (File.Exists(originalFileName))
        //            {
        //                File.Delete(originalFileName);
        //            }
        //            File.Move(uploadingFileName, originalFileName);
        //            japfaMap.installPicture(filename, locationID);
        //        }

        //    return Ok(japfaMap.getPicture(locationID));
        //}

        //[Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> UploadPictureForLocation(int locationID)
        {
            try
            {
                // Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                string rootFolder = HttpContext.Current.Server.MapPath("~/pictures/" + locationID);
                if (!Directory.Exists(rootFolder))
                {
                    Directory.CreateDirectory(rootFolder);
                }

                var provider = new MultipartFormDataStreamProvider(rootFolder);

                // Read the form data and return an async task.
                var task = await Request.Content.ReadAsMultipartAsync(provider);

                //This illustrates how to get the file names.
                var id1 = 1;
                foreach (MultipartFileData file in provider.FileData)
                {
                    var uploadingFileNamePath = file.LocalFileName;
                    var fileName = file.Headers.ContentDisposition.FileName;
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }
                    string originalFileNamePath = String.Concat(rootFolder, "\\" + fileName);
                    if (File.Exists(originalFileNamePath))
                    {
                        File.Delete(originalFileNamePath);
                    }
                    File.Move(uploadingFileNamePath, originalFileNamePath);
                    //them vao table
                    japfaMap.installPicture(fileName, locationID);
                }

                return Ok(japfaMap.getPicture(locationID));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        #endregion


        #region location control
        [HttpPost]
        public IHttpActionResult save(string email, [FromBody] Location location)
        {
            try
            {
                return Ok(japfaMap.save(email, location));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [HttpPost]
        public IHttpActionResult approval(string email, int id, string locationName="")
        {
            try
            {
                return Ok(japfaMap.approval(email,  id));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [HttpPost]
        public IHttpActionResult reject(string email, int id, string locationName="")
        {
            try
            {
                return Ok(japfaMap.reject(email,  id));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [HttpGet]
        public IHttpActionResult GetLocationsForControl()
        {
            try
            {
                return Ok(japfaMap.GetLocationsForControl());
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [HttpGet]
        public IHttpActionResult UpdateLocationWithIDLink(string idLink, decimal dLat, decimal dLong, string updateEmail,string updateLocationKey)
        {
            try
            {
                return Ok(japfaMap.UpdateLocationWithIDLink(idLink, dLat,dLong,updateEmail, updateLocationKey));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult UpdateLocationWithIDLinkV1(string idLink, decimal dLat, decimal dLong, string locationName, string locationAddress, int locationStatus, string updateEmail, string updateLocationKey)
        {
            try
            {
                return Ok(japfaMap.UpdateLocationWithIDLinkV1(idLink, dLat, dLong, locationName, locationAddress, locationStatus, updateEmail, updateLocationKey));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [HttpPost]
        public IHttpActionResult delete(string email, string locationName, [FromBody] int id)
        {
            try
            {
                return Ok(japfaMap.delete(email, locationName, id));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        //[HttpPost]
        //public IHttpActionResult deleteLocationLinhTinh()
        //{
        //    try
        //    {
        //        return Ok(japfaMap.deleteLocationLinhTinh());
        //    }
        //    catch (Exception ex)
        //    {
        //        return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
        //    }
        //}

        [HttpPost]
        public IHttpActionResult UpdateStatusForJapfaMapFromEApproval(DocumentInfo documentInfo)
        {
            try
            {
                return Ok(japfaMap.UpdateStatusForJapfaMapFromEApproval(documentInfo));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        } 
        
        [HttpPost]
        public IHttpActionResult unlockLocation(string email, int id)
        {
            try
            {
                return Ok(japfaMap.unlockLocation(email, id));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [HttpPost]
        public IHttpActionResult lockLocation(string email, int id)
        {
            try
            {
                return Ok(japfaMap.lockLocation(email, id));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }


        [HttpPost]
        public IHttpActionResult UpdateLocation()
        {
            try
            {
                return Ok(japfaMap.UpdateLocation());
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }


        #endregion

    }
}
