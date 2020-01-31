using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Amazon;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.IO;
using System.Security.Cryptography;

namespace INTSOF.Queues
{
    public class CommonFileManager
    {
        #region Private Variables

        #region AWS S3

        private static String _AWSBucket = string.Empty;
        private static String _AWSRegion = string.Empty;
        private static String _AWSDirPathInBucket = string.Empty;
        private static AmazonS3Config _S3Config = null;
        private static AmazonS3Client _S3Client = null;

        #endregion

        #region Constants and Fields

        private const string BACKSLASH = @"\";
        private const string FORWARDSLASH = @"/";
        private string _path;

        #endregion

        #endregion

        #region Private Class Properties

        private static AmazonS3Config S3Configuration
        {
            get
            {
                if (_S3Config == null)
                {
                    _S3Config = new AmazonS3Config();
                    _S3Config.ServiceURL = _AWSRegion;
                    //todo
                    _S3Config.RegionEndpoint = RegionEndpoint.USWest2;
                }
                return _S3Config;
            }
        }

        public static AmazonS3Client S3Client
        {
            get
            {
                if (_S3Client == null)
                {
                    String aWSAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"].ToString();
                    String aWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"].ToString();
                    _S3Client = new AmazonS3Client(aWSAccessKey, aWSSecretKey, S3Configuration);
                }
                return _S3Client;
            }
        }

        #endregion

        #region Constructor

        public CommonFileManager()
        {

        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// To save document
        /// </summary>
        /// <param name="srcFilePath"></param>
        /// <param name="destFilePath"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static String SaveDocument(String srcFilePath, String destFilePath)
        {
            try
            {
                Boolean aWSUseS3 = false;
                String destFileLocation = String.Empty;
                String finalDestFileLocation = String.Empty;
                if (String.IsNullOrEmpty(srcFilePath) && String.IsNullOrEmpty(destFilePath))
                {
                    return null;
                }
                if (ConfigurationManager.AppSettings["AWSUseS3"] != null)
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }
                if (ConfigurationManager.AppSettings["ApplicantFileLocation"] != null)
                {
                    destFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"];
                }

                //Check whether use AWS S3, 
                //false if file drive will be used
                if (aWSUseS3 == false)
                {
                    if (!destFileLocation.EndsWith("\\"))
                    {
                        destFileLocation += "\\";
                    }

                    Int32 indx = destFilePath.LastIndexOf(@"\");
                    finalDestFileLocation = destFileLocation + destFilePath.Substring(0, indx);
                    if (!Directory.Exists(finalDestFileLocation))
                    {
                        Directory.CreateDirectory(finalDestFileLocation);
                    }

                    //Create destination file location
                    destFileLocation = destFileLocation + destFilePath;

                    //Move file to destination location    
                    File.Copy(srcFilePath, destFileLocation);
                    try
                    {
                        if (!String.IsNullOrEmpty(srcFilePath))
                            File.Delete(srcFilePath);
                    }
                    catch (Exception) { }

                    //Check for proper creation
                    if (File.Exists(destFileLocation))
                    {
                        return destFileLocation;
                    }
                    else
                    {
                        return null;
                    }
                }
                else //true if Amazon S3 will be used
                {
                    String newFilePath = String.Empty;
                    try { _AWSBucket = ConfigurationManager.AppSettings["AWSBucket"].ToString(); }
                    catch { throw new Exception("The AWSBucket was not found in the configuration."); }

                    try { _AWSRegion = ConfigurationManager.AppSettings["AWSRegion"].ToString(); }
                    catch { throw new Exception("The AWSRegion was not found in the configuration."); }

                    try { _AWSDirPathInBucket = ConfigurationManager.AppSettings["AWSDirPathInBucket"].ToString(); }
                    catch { throw new Exception("The AWSDirPathInBucket was not found in the configuration."); }

                    TransferUtility fileTransferUtility = new TransferUtility(S3Client);

                    if (!destFileLocation.EndsWith(@"/"))
                    {
                        destFileLocation += @"/";
                    }
                    destFileLocation = destFileLocation + destFilePath;

                    destFileLocation = destFileLocation.Replace(@"\", @"/");
                    //extract file path
                    destFileLocation = ExtractFilePath(destFileLocation);
                    if (String.IsNullOrEmpty(_AWSDirPathInBucket))
                    {
                        newFilePath = destFileLocation;
                    }
                    else
                    {
                        if (!_AWSDirPathInBucket.EndsWith(@"/"))
                        {
                            _AWSDirPathInBucket += @"/";
                        }
                        newFilePath = _AWSDirPathInBucket + destFileLocation;
                    }

                    //extract file path
                    newFilePath = ExtractFilePath(newFilePath);

                    //Upload the file to S3
                    //newFilePath is used as key for S3
                    fileTransferUtility.Upload(srcFilePath, _AWSBucket, newFilePath);

                    //try
                    //{
                    //    if (!String.IsNullOrEmpty(srcFilePath))
                    //        File.Delete(srcFilePath);
                    //}
                    //catch (Exception) { }

                    //Check for proper creation
                    S3FileInfo s3ficheck = new S3FileInfo(S3Client, _AWSBucket, newFilePath);
                    if (s3ficheck.Exists)
                    {
                        return newFilePath;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To save document from bytes
        /// </summary>
        /// <param name="srcFileBytes"></param>
        /// <param name="destFilePath"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static String SaveDocument(Byte[] srcFileBytes, String destFilePath)
        {
            try
            {
                Boolean aWSUseS3 = false;
                String destFileLocation = String.Empty;
                String finalDestFileLocation = String.Empty;
                if (srcFileBytes != null && String.IsNullOrEmpty(destFilePath))
                {
                    return null;
                }
                if (ConfigurationManager.AppSettings["AWSUseS3"] != null)
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }
                if (ConfigurationManager.AppSettings["ApplicantFileLocation"] != null)
                {
                    destFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"];
                }

                //Check whether use AWS S3, 
                //false if file drive will be used
                if (aWSUseS3 == false)
                {
                    if (!destFileLocation.EndsWith("\\"))
                    {
                        destFileLocation += "\\";
                    }

                    Int32 indx = destFilePath.LastIndexOf(@"\");
                    finalDestFileLocation = destFileLocation + destFilePath.Substring(0, indx);
                    if (!Directory.Exists(finalDestFileLocation))
                    {
                        Directory.CreateDirectory(finalDestFileLocation);
                    }

                    //Create destination file location
                    destFileLocation = destFileLocation + destFilePath;

                    //Writing bytes in destination file using FileStream
                    using (FileStream _FileStream = new FileStream(destFileLocation,
                                    System.IO.FileMode.Create,
                                    System.IO.FileAccess.Write))
                    {
                        _FileStream.Write(srcFileBytes, 0, srcFileBytes.Length);
                    }

                    //Check for proper creation
                    if (File.Exists(destFileLocation))
                    {
                        return destFileLocation;
                    }
                    else
                    {
                        return null;
                    }
                }
                else //true if Amazon S3 will be used
                {
                    String newFilePath = String.Empty;
                    try { _AWSBucket = ConfigurationManager.AppSettings["AWSBucket"].ToString(); }
                    catch { throw new Exception("The AWSBucket was not found in the configuration."); }

                    try { _AWSRegion = ConfigurationManager.AppSettings["AWSRegion"].ToString(); }
                    catch { throw new Exception("The AWSRegion was not found in the configuration."); }

                    try { _AWSDirPathInBucket = ConfigurationManager.AppSettings["AWSDirPathInBucket"].ToString(); }
                    catch { throw new Exception("The AWSDirPathInBucket was not found in the configuration."); }

                    TransferUtility fileTransferUtility = new TransferUtility(S3Client);
                    if (!destFileLocation.EndsWith(@"/"))
                    {
                        destFileLocation += @"/";
                    }
                    destFileLocation = destFileLocation + destFilePath;

                    destFileLocation = destFileLocation.Replace(@"\", @"/");
                    //extract file path
                    destFileLocation = ExtractFilePath(destFileLocation);
                    if (String.IsNullOrEmpty(_AWSDirPathInBucket))
                    {
                        newFilePath = destFileLocation;
                    }
                    else
                    {
                        if (!_AWSDirPathInBucket.EndsWith(@"/"))
                        {
                            _AWSDirPathInBucket += @"/";
                        }
                        newFilePath = _AWSDirPathInBucket + destFileLocation;
                    }

                    //extract file path
                    newFilePath = ExtractFilePath(newFilePath);

                    //byte[] buffer = new byte[srcFileBytes.Length];
                    MemoryStream stream = new MemoryStream();
                    stream.Write(srcFileBytes, 0, srcFileBytes.Length);

                    //Upload the file to S3
                    //newFilePath is used as key for S3
                    fileTransferUtility.Upload(stream, _AWSBucket, newFilePath);
                    stream.Close();

                    //Check for proper creation
                    S3FileInfo s3ficheck = new S3FileInfo(S3Client, _AWSBucket, newFilePath);
                    if (s3ficheck.Exists)
                    {
                        return newFilePath;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To retrieve or get document
        /// </summary>
        /// <param name="filePathKey"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static byte[] RetrieveDocument(String filePath)
        {
            try
            {
                if (String.IsNullOrEmpty(filePath))
                {
                    return null;
                }
                byte[] returnBytes = null;
                Boolean aWSUseS3 = false;
                String destFileLocation = String.Empty;
                if (ConfigurationManager.AppSettings["AWSUseS3"] != null)
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }

                //Check whether use AWS S3, 
                //false if file drive will be used
                if (aWSUseS3 == false)
                {
                    if (File.Exists(filePath))
                    {
                        //ReadAllBytes is implemented in an obvious way. It uses the using-statement on a FileStream. 
                        //Then it loops through the file and puts the bytes into an array. In .NET 4.0, it throws an exception if the file exceeds 2 gigabytes.
                        returnBytes = File.ReadAllBytes(filePath);
                        return returnBytes;
                    }
                    else
                    {
                        return null;
                    }
                }
                else //true if Amazon S3 will be used
                {
                    try { _AWSBucket = ConfigurationManager.AppSettings["AWSBucket"].ToString(); }
                    catch { throw new Exception("The AWSBucket was not found in the configuration."); }

                    try { _AWSDirPathInBucket = ConfigurationManager.AppSettings["AWSDirPathInBucket"].ToString(); }
                    catch { throw new Exception("The AWSDirPathInBucket was not found in the configuration."); }

                    if (!String.IsNullOrEmpty(_AWSDirPathInBucket))
                    {
                        if (!filePath.Contains(_AWSDirPathInBucket))
                        {
                            filePath = _AWSDirPathInBucket + @"/" + filePath;
                        }
                    }

                    filePath = filePath.Replace(@"\", @"/");
                    //extract file path
                    filePath = ExtractFilePath(filePath);

                    //filePathKey is used as key for S3
                    S3FileInfo s3ficheck = new S3FileInfo(S3Client, _AWSBucket, filePath);
                    if (!s3ficheck.Exists)
                        return null;

                    GetObjectRequest request = new GetObjectRequest
                    {
                        BucketName = _AWSBucket,
                        Key = filePath
                    };

                    using (GetObjectResponse response = S3Client.GetObject(request))
                    {
                        returnBytes = ReadFully(response.ResponseStream);
                    }

                    return returnBytes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To delete document
        /// </summary>
        /// <param name="filePathKey"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static Boolean DeleteDocument(String filePath)
        {
            try
            {
                Boolean aWSUseS3 = false;
                String destFileLocation = String.Empty;
                if (String.IsNullOrEmpty(filePath))
                {
                    return false;
                }
                if (ConfigurationManager.AppSettings["AWSUseS3"] != null)
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }
                if (ConfigurationManager.AppSettings["ApplicantFileLocation"] != null)
                {
                    destFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"];
                }
                //Check whether use AWS S3, 
                //false if file drive will be used
                if (aWSUseS3 == false)
                {
                    if (!String.IsNullOrEmpty(filePath) && File.Exists(filePath))
                    {
                        try
                        {
                            File.Delete(filePath);
                            return true;
                        }
                        catch (Exception) { }
                    }
                }
                else //true if Amazon S3 will be used
                {
                    try { _AWSBucket = ConfigurationManager.AppSettings["AWSBucket"].ToString(); }
                    catch { throw new Exception("The AWSBucket was not found in the configuration."); }

                    filePath = filePath.Replace(@"\", @"/");

                    //extract file path
                    filePath = ExtractFilePath(filePath);

                    //delete file from file system
                    DeleteObjectResponse dor = S3Client.DeleteObject(new DeleteObjectRequest()
                    {
                        BucketName = _AWSBucket,
                        Key = filePath
                    });
                    if (dor.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To rename or move document from src S3 to dest S3
        /// </summary>
        /// <param name="srcFilePathKey">It contains path of src S3 file</param>
        /// <param name="destFilePathKey">It contains path of dest S3 file</param>
        /// <returns></returns>
        public Boolean RenameDocument(String srcFilePath, String destFilePath, String fileType)
        {
            try
            {
                Boolean aWSUseS3 = false;
                String destFileLocation = String.Empty;
                String finalDestFileLocation = String.Empty;
                if (String.IsNullOrEmpty(srcFilePath) &&  String.IsNullOrEmpty(destFilePath))
                {
                    return false;
                }
                if (ConfigurationManager.AppSettings["AWSUseS3"] != null)
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }
                if (ConfigurationManager.AppSettings["ApplicantFileLocation"] != null)
                {
                    destFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"];
                }

                //Check whether use AWS S3, 
                //false if file drive will be used
                if (aWSUseS3 == false)
                {
                    if (!destFileLocation.EndsWith("\\"))
                    {
                        destFileLocation += "\\";
                    }

                    Int32 indx = destFilePath.LastIndexOf(@"\");
                    finalDestFileLocation = destFileLocation + destFilePath.Substring(0, indx);
                    if (!Directory.Exists(finalDestFileLocation))
                    {
                        Directory.CreateDirectory(finalDestFileLocation);
                    }

                    //Create destination file location
                    destFileLocation = destFileLocation + destFilePath;

                    //Move file to destination location
                    if (!String.IsNullOrEmpty(srcFilePath) && File.Exists(srcFilePath))
                    {
                        //Copy and delete source file
                        File.Copy(srcFilePath, destFileLocation);
                        try
                        {
                            File.Delete(srcFilePath);
                            return true;
                        }
                        catch (Exception) { }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else //true if Amazon S3 will be used
                {
                    String newFilePath = String.Empty;
                    try { _AWSBucket = ConfigurationManager.AppSettings["AWSBucket"].ToString(); }
                    catch { throw new Exception("The AWSBucket was not found in the configuration."); }

                    try { _AWSRegion = ConfigurationManager.AppSettings["AWSRegion"].ToString(); }
                    catch { throw new Exception("The AWSRegion was not found in the configuration."); }

                    try { _AWSDirPathInBucket = ConfigurationManager.AppSettings["AWSDirPathInBucket"].ToString(); }
                    catch { throw new Exception("The AWSDirPathInBucket was not found in the configuration."); }

                    TransferUtility fileTransferUtility = new TransferUtility(S3Client);
                    if (!destFileLocation.EndsWith(@"/"))
                    {
                        destFileLocation += @"/";
                    }
                    destFileLocation = destFileLocation + destFilePath;

                    destFileLocation = destFileLocation.Replace(@"\", @"/");
                    //extract file path
                    destFileLocation = ExtractFilePath(destFileLocation);
                    if (String.IsNullOrEmpty(_AWSDirPathInBucket))
                    {
                        newFilePath = destFileLocation;
                    }
                    else
                    {
                        if (!_AWSDirPathInBucket.EndsWith(@"/"))
                        {
                            _AWSDirPathInBucket += @"/";
                        }
                        newFilePath = _AWSDirPathInBucket + destFileLocation;
                    }

                    srcFilePath = srcFilePath.Replace(@"\", @"/");
                    //extract file path
                    srcFilePath = ExtractFilePath(srcFilePath);

                    //copy file from file system
                    CopyObjectResponse cor = S3Client.CopyObject(new CopyObjectRequest()
                    {
                        SourceBucket = _AWSBucket,
                        SourceKey = srcFilePath,
                        DestinationBucket = _AWSBucket,
                        DestinationKey = newFilePath
                    });

                    //delete original file from file system
                    DeleteObjectResponse dor = S3Client.DeleteObject(new DeleteObjectRequest()
                    {
                        BucketName = _AWSBucket,
                        Key = srcFilePath
                    });

                    if (cor.HttpStatusCode == System.Net.HttpStatusCode.OK || dor.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get document Md5 Hash
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetMd5Hash(byte[] data)
        {
            byte[] tmpHash;

            //Compute hash based on source data.
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(data);

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < tmpHash.Length; i++)
            {
                sBuilder.Append(tmpHash[i].ToString("X2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To extract file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static String ExtractFilePath(String filePath)
        {
            while (filePath.IndexOf(@"//") > -1)
                filePath = filePath.Replace(@"//", @"/");

            if (filePath.IndexOf('/') == 0)
                filePath = filePath.Substring(1);

            return filePath;
            //return filePath.ToLower();
        }

        /// <summary>
        /// To read file and return byte array
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="initialLength"></param>
        /// <returns></returns>
        private static byte[] ReadFully(Stream stream, Int32 initialLength = -1)
        {
            // If we've been passed an unhelpful initial length, just use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        #endregion

        #endregion
    }
}
