using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using SpartacusAPI.Filters;
using System.IO;
using Google.Cloud.Storage.V1;
namespace SpartacusAPI.Controllers
{
    [ApiController]    
    [Route("spartacusapi/GetImage")]
    [ApiKeyAuth]
    public class GetImageController : ControllerBase
    {
        /// <summary>
        /// To get Image by ImageId
        /// </summary>
        [HttpPost]
        public string GetImage([FromBody] String ImageId)
        {
            string message = "Fail";
            DataTable dt = new DataTable();            
            SqlConnection con = new SqlConnection(Startup.dbConn);
            SqlCommand cmd = new SqlCommand("SELECT * FROM IMAGE_REQUEST WHERE IMAGE_ID=" + ImageId, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\ApprovedImages\\");
                    bool basePathExists = System.IO.Directory.Exists(basePath);
                    if (!basePathExists) Directory.CreateDirectory(basePath);
                    var fileName = Path.GetFileNameWithoutExtension(dr["IMAGE_ID"].ToString().Trim() + ".png".Trim());
                    var FilePath = Path.Combine(basePath, dr["IMAGE_ID"].ToString().Trim() + ".png".Trim());
                    var extension = Path.GetExtension(dr["IMAGE_ID"].ToString().Trim() + ".png".Trim());
                    byte[] img = (byte[])dr["Image"];
                    if (!System.IO.File.Exists(FilePath))
                    {
                        FileStream fs = new FileStream(FilePath, FileMode.CreateNew, FileAccess.ReadWrite);
                        fs.Write(img, 0, img.Length);
                        fs.Flush();
                        fs.Close();
                    }
                    var bucketName = "ingka-recovery-sparepart-prod-images";
                    var localpath = FilePath;
                    var objectName = fileName;

                    //For bulk upload, this method will execute in loop foreach images available in the Spartacus ON-Prem storage
                    //For single upload from Spartacus website File Upload should be called once in Async
                     message = UploadFile(bucketName, localpath, string.IsNullOrEmpty(objectName) ? null : objectName);
                    System.IO.File.Delete(localpath);
                    
                }
               
            }
            con.Close();
            return message;
        }

     [NonAction]
        public string UploadFile(string bucketName, string localPath, string objectName = null)
        {
            try
            {
                string path = Path.GetFullPath("ingka-recovery-sparepart-prod-8bb5b4967152.json");
                //var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Service Account Of Spartacus\\");
                System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                var storage = StorageClient.Create();
                using (var file =   System.IO.File.OpenRead(localPath))
                {
                    objectName = objectName ?? Path.GetFileName(localPath);
                    storage.UploadObject(bucketName, objectName, null, file);
                    Console.WriteLine($"Uploaded {objectName}.");
                }
               
            }
            catch 
            {
                return "fail";
            }
            return "success";
        }
    }
}