using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace INTSOF.Utils
{
    public class FPFileApplicantImage
    {
        public static List<string> SaveImagesFromFingerPrintFiles(string newTempFilePath, int tenantId, string FileName,string ApplicantFingerPrintImages, string temporaryFileLocation, bool aWSUseS3)
        {
            List<string> applicantImagesPath = new List<string>();
            try
            {                
                Regex re = new Regex(@"10.999:");
                var OriginalfileBytes = File.ReadAllBytes(newTempFilePath);
                Encoding ascii = Encoding.ASCII;
                string instr = ascii.GetString(OriginalfileBytes);
                Match match = re.Match(instr);
                List<List<byte>> applicantFiles = new List<List<byte>>();
                // newbytes = new List<byte>();
                if (match.Success)
                {
                    var index = match.Index;
                    Match nmatch = match.NextMatch();
                    int end = (nmatch.Success) ? nmatch.Index : OriginalfileBytes.Length;
                    var newbytes = new List<byte>();
                    for (var i = match.Index + match.Length; i < end; i++)
                    {
                        newbytes.Add(OriginalfileBytes[i]);
                    }
                    applicantFiles.Add(newbytes);
                    //File.WriteAllBytes(@"C:\Users\Smanwal\Documents\ADB\test.jpg", newbytes.ToArray());
                    match = nmatch;
                }

                if (applicantFiles != null && applicantFiles.Any())
                {
                    var destinationPath = "";

                    if (aWSUseS3)
                    {
                        destinationPath = ApplicantFingerPrintImages + "/" + "Tenant(" + tenantId.ToString() + @")/";
                    }
                    else
                    {
                        destinationPath = Path.Combine(ApplicantFingerPrintImages, "Tenant(" + tenantId.ToString() + @")");
                    }

                    for (var i = 0; i < applicantFiles.Count; i++)
                    {
                        var imageName = Path.GetFileNameWithoutExtension(FileName)
                        + "_" + DateTime.Now.ToString("yyyyMMddhhmm_ssffff")
                        + (i > 0 ? "_" + i : "")
                        + ".jpg";

                        var returnImagePath = "";

                        if (aWSUseS3)
                        {
                            var tempImagePath = Path.Combine(temporaryFileLocation, imageName);
                            File.WriteAllBytes(tempImagePath, applicantFiles[i].ToArray());
                            AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                            returnImagePath = objAmazonS3.SaveDocument(tempImagePath, imageName, destinationPath);
                            try
                            {
                                File.Delete(tempImagePath);
                            }
                            catch { }
                        }
                        else
                        {
                            if (!Directory.Exists(destinationPath))
                            {
                                Directory.CreateDirectory(destinationPath);
                            }
                            returnImagePath = Path.Combine(destinationPath, imageName);
                            File.WriteAllBytes(returnImagePath, applicantFiles[i].ToArray());
                        }

                        if (!string.IsNullOrWhiteSpace(returnImagePath))
                        {
                            applicantImagesPath.Add(returnImagePath);
                        }
                    }
                    //return applicantImagesPath;

                }
            }
            catch { }
            return applicantImagesPath;
        }
    }
}
