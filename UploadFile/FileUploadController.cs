using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using VirusTotalNET;
using VirusTotalNET.ResponseCodes;
using VirusTotalNET.Results;

namespace UploadFile
{
    public class FileUploadController : ApiController
    {
        [HttpPost]
        public async Task<KeyValuePair<bool, string>> UploadFile()
        {

            try
            {
                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var httpPostedFile = HttpContext.Current.Request?.Files[0];
                    ScanResult fileResult=new ScanResult();
                    var fileSavePath = HostingEnvironment.MapPath("~/UploadedFiles");
                    var destinationFolder = "C:\\DestinationFolder\\";

                    if (httpPostedFile != null)
                    {
                        //TODO: Scan the file
                        VirusTotal virusTotal = new VirusTotal("1cc302b2a9d28a98df644cb215330e13003ea1d3bcf4bc7eaf453484d8e337a8");
                        //Use HTTPS instead of HTTP
                        virusTotal.UseTLS = true;

                        //Get the byte array
                        byte[] fileByteArray = new byte[httpPostedFile.ContentLength];
                        httpPostedFile.InputStream.Read(fileByteArray, 0, httpPostedFile.ContentLength);

                        var fileName = GetFileName(httpPostedFile.FileName);

                        //Check if the file has been scanned before.
                        FileReport fileReport = await virusTotal.GetFileReportAsync(fileByteArray);
                        bool hasFileBeenScannedBefore = fileReport.ResponseCode == FileReportResponseCode.Present;

                        if (hasFileBeenScannedBefore)
                        {
                            //Move it to the Destination Folder
                            switch (fileReport?.Positives)
                            {
                                case 0:
                                    SaveFile(destinationFolder, fileName, httpPostedFile);
                                    break;
                                default:
                                    break;
                            }

                        }
                        else
                        {
                            fileResult = await virusTotal.ScanFileAsync(fileByteArray, fileName);
                            if (fileResult.ResponseCode == ScanFileResponseCode.Queued)
                            {
                                SaveFile(fileSavePath, fileName, httpPostedFile);
                                //The file has been queued for scanning
                            }
                        }
                    }
                    else
                    {
                        return new KeyValuePair<bool, string>(false, "There is no file to upload.");
                    }
                }

                return new KeyValuePair<bool, string>(false, "No file found to upload.");
            }
            catch (Exception ex)
            {
                return new KeyValuePair<bool, string>(false, "An error occurred while uploading the file. Error Message: " + ex.Message);
            }
        }

        private string GetFileName(string postedFile)
        {
            string fileName;

            var lastIndex = postedFile.LastIndexOf('\\');

            fileName = postedFile.Remove(0, lastIndex+1);

            return fileName;
        }

        private KeyValuePair<bool, string> SaveFile(string fileSavePath, string fileName, HttpPostedFile httpPostedFile)
        {
            try
            {
                if (fileSavePath != null && !string.IsNullOrEmpty(fileName))
                {
                    httpPostedFile.SaveAs(fileSavePath + "\\" + fileName);

                    return new KeyValuePair<bool, string>(true, "File uploaded successfully.");
                }

                return new KeyValuePair<bool, string>(false, "Missing the folder to save");
            }
            catch (Exception e)
            {
                return new KeyValuePair<bool, string>(false, "Error saving the file" + fileName + e.Message);
            }
        }
    }
}