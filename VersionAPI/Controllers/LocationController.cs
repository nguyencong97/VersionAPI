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
        public IEnumerable<Location> GetLocations(int id)
        {
            return japfaMap.GetLocations(id);
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
        public IHttpActionResult delete(string email, [FromBody] int id)
        {
            try
            {
                return Ok(japfaMap.delete(email, id));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
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

        [HttpPost]
        public async Task<IHttpActionResult> UploadPictureForLocation(int locationID)
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string rootFolder = HttpContext.Current.Server.MapPath("~/pictures/"+locationID);
            if (!Directory.Exists(rootFolder))
            {
                Directory.CreateDirectory(rootFolder);
            }

            var provider = new MultipartFormDataStreamProvider(rootFolder);

            // Read the form data and return an async task.
            var task =await Request.Content.ReadAsMultipartAsync(provider);

                //This illustrates how to get the file names.
                Random rnd = new Random();

                int i = japfaMap.getPictureCount(locationID);
                foreach (MultipartFileData file in provider.FileData)
                {
                   var uploadingFileName = file.LocalFileName;
                    var filename = rnd.Next(100000000)+ "Image" + ++i +".jpg";
                    string originalFileName = String.Concat(rootFolder, "\\" + filename);
                    if (File.Exists(originalFileName))
                    {
                        File.Delete(originalFileName);
                    }
                    File.Move(uploadingFileName, originalFileName);
                    japfaMap.installPicture(filename, locationID);
                }

            return Ok(japfaMap.getPicture(locationID));
        }

        #endregion


    }
}
