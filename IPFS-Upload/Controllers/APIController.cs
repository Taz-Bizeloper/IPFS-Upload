using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace IPFS_Upload.Controllers
{

    public class APIController : ApiController
    {

        private string UploadNFTStorage(string filePath, string fileName, string fileDescr)
        {
            //use cmd command to execute the nodeJS upload script that uses the NFT.Storage npm package
            var proc = new System.Diagnostics.Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.UseShellExecute = false;

            proc.StartInfo.FileName = "cmd";
            string command = "/C node " + System.Web.HttpContext.Current.Request.MapPath("~/upload-ipfs") + " ";
            command += "\"" + System.Web.HttpContext.Current.Request.MapPath("~/uploads/" + filePath) + "\"" + " ";
            command += "\"" + fileName + "\"" + " ";
            command += "\"" + fileDescr + "\"" + " ";
            proc.StartInfo.Verb = "runas";
            proc.StartInfo.Arguments = command;
            proc.Start();
            proc.WaitForExit();
            string result = proc.StandardOutput.ReadToEnd();
            string resultErr = proc.StandardError.ReadToEnd();
            if (!String.IsNullOrEmpty(resultErr))
            {
                File.AppendAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/DevLog.txt"), resultErr + Environment.NewLine + Environment.NewLine);
            }
            return result;
        }

        [HttpPost]
        [Route("api/UploadFile")]
        [AcceptVerbs("OPTIONS")]
        public string UploadFile()
        {
            if (HttpContext.Current.Request.HttpMethod != "POST") //this is used to ignore OPTIONS requests
                return "";
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Get the uploaded image from the Files collection
                string tmpLocationToSave = "";
                try
                {
                    var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];

                    if (httpPostedFile != null)
                    {
                        string filePath = HttpContext.Current.Request["filePath"];
                        string fileName = HttpContext.Current.Request["fileName"];
                        string fileDescr = HttpContext.Current.Request["fileDescr"];


                        string tmpStoreFolder = System.Web.HttpContext.Current.Request.MapPath("~/uploads/");
                        if (!Directory.Exists(tmpStoreFolder))
                        {
                            Directory.CreateDirectory(tmpStoreFolder);
                        }

                         tmpLocationToSave = tmpStoreFolder + "/" + filePath;
                        
                        if (File.Exists(tmpLocationToSave))
                        {
                            File.Delete(tmpLocationToSave);
                        }
                        string[] checkDir = filePath.Split('/');
                        string tempDir = tmpStoreFolder;
                        for (int i = 0; i < checkDir.Count() - 1; i++) //do not check last element that is the file name
                        {
                            tempDir += "/" + checkDir[i];
                            if (!Directory.Exists(tempDir))
                            {
                                Directory.CreateDirectory(tempDir);
                            }
                        }

                        httpPostedFile.SaveAs(tmpLocationToSave); //save image temporarily

                        string ipfs = UploadNFTStorage(filePath, fileName, fileDescr);
                        File.Delete(tmpLocationToSave);
                        return ipfs.Trim();
                    }
                    else
                    {
                        return "bad";
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/DevLog.txt"), ex.ToString() + Environment.NewLine + Environment.NewLine);
                    try
                    {
                        File.Delete(tmpLocationToSave);
                    }
                    catch (Exception e) { 
                    }

                    return "bad";
                }
            }
            else
            {
                return "bad";
            }
        }


    }
}
