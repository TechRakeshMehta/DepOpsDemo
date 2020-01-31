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

namespace INTSOF.Utils
{
    public class AmazonS3Documents
    {
        #region Private Variables

        #region AWS S3

        private String _AWSBucket = string.Empty;
        private String _AWSRegion = string.Empty;
        private AmazonS3Config _S3Config = null;
        private AmazonS3Client _S3Client = null;

        #endregion

        #endregion

        #region Private Class Properties
        private AmazonS3Config S3Configuration
        {
            get
            {
                if (_S3Config == null)
                {
                    _S3Config = new AmazonS3Config();
                    //{
                    //ServiceURL = _AWSRegion,
                    //S3 default protocol is HTTPS so no need to assign
                    //CommunicationProtocol = Amazon.S3.Model.Protocol.HTTPS
                    //};
                    _S3Config.ServiceURL = _AWSRegion;
                    //todo
                    _S3Config.RegionEndpoint = RegionEndpoint.USWest2;
                }
                return _S3Config;
            }
        }

        public AmazonS3Client S3Client
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
        public AmazonS3Documents()
        {
            try { _AWSBucket = ConfigurationManager.AppSettings["AWSBucket"].ToString(); }
            catch { throw new Exception("The AWSBucket was not found in the configuration."); }

            try { _AWSRegion = ConfigurationManager.AppSettings["AWSRegion"].ToString(); }
            catch { throw new Exception("The AWSRegion was not found in the configuration."); }
        }
        #endregion

        /// <summary>
        /// To save document
        /// </summary>
        /// <param name="srcFilePath"></param>
        /// <param name="destFileName"></param>
        /// <param name="destFolder"></param>
        /// <returns></returns>
        public String SaveDocument(String srcFilePath, String destFileName, String destFolder = "")
        {
            try
            {
                TransferUtility fileTransferUtility = new TransferUtility(S3Client);
                String dirInRootBucket = ConfigurationManager.AppSettings["AWSDirPathInBucket"].ToString();
                String newDestFolder = String.Empty;
                String newFilePath = String.Empty;
                if (destFolder.IsNullOrEmpty())
                {
                    if (dirInRootBucket.IsNullOrEmpty())
                    {
                        newFilePath = destFileName;
                    }
                    else
                    {
                        newFilePath = dirInRootBucket + @"/" + destFileName;
                    }
                }
                else
                {
                    if (dirInRootBucket.IsNullOrEmpty())
                    {
                        newFilePath = destFolder + destFileName;
                    }
                    else
                    {
                        newFilePath = dirInRootBucket + @"/" + destFolder + @"/" + destFileName;
                    }
                }
                //extract file path
                newFilePath = ExtractFilePath(newFilePath);
                //Upload the file to S3
                //newFilePath is used as key for S3
                fileTransferUtility.Upload(srcFilePath, _AWSBucket, newFilePath);
                //Check for proper creation
                S3FileInfo s3ficheck = new S3FileInfo(S3Client, _AWSBucket, newFilePath);
                //UAT-862:- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                // Added length check for verifing the size of uploaded file is not 0.
                if (s3ficheck.Exists && s3ficheck.Length > AppConsts.NONE)
                {
                    return newFilePath;
                }
                else
                {
                    return "";
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
        /// <param name="filePathKey">It contains path of S3 file</param>
        /// <returns></returns>
        public byte[] RetrieveDocument(String filePathKey)
        {
            try
            {
                if (filePathKey.IsNullOrEmpty())
                {
                    return null;
                }
                byte[] returnVal = null;
                String dirInRootBucket = ConfigurationManager.AppSettings["AWSDirPathInBucket"].ToString();
                if (!dirInRootBucket.IsNullOrEmpty())
                {
                    if (!filePathKey.Contains(dirInRootBucket))
                    {
                        filePathKey = dirInRootBucket + @"/" + filePathKey;
                    }
                }
                filePathKey = ExtractFilePath(filePathKey);
                //filePathKey is used as key for S3
                S3FileInfo s3ficheck = new S3FileInfo(S3Client, _AWSBucket, filePathKey);
                if (!s3ficheck.Exists)
                    return null;

                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = _AWSBucket,
                    Key = filePathKey
                };

                using (GetObjectResponse response = S3Client.GetObject(request))
                {
                    returnVal = ReadFully(response.ResponseStream);
                }

                return returnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To retrieve and copy document to destination location
        /// </summary>
        /// <param name="filePathKey">It contains path of S3 file</param>
        /// <param name="destFilePath"></param>
        /// <returns></returns>
        public Boolean RetrieveAndCopyDocument(String filePathKey, String destFilePath)
        {
            try
            {
                String destFilePathKey = filePathKey;
                if (filePathKey.IsNullOrEmpty())
                {
                    return false;
                }
                String dirInRootBucket = ConfigurationManager.AppSettings["AWSDirPathInBucket"].ToString();
                if (!dirInRootBucket.IsNullOrEmpty())
                {
                    if (!filePathKey.Contains(dirInRootBucket))
                    {
                        filePathKey = dirInRootBucket + @"/" + filePathKey;
                    }
                }

                //if (!dirInRootBucket.IsNullOrEmpty())
                //{
                //    filePathKey = dirInRootBucket + @"/" + filePathKey;
                //}
                filePathKey = ExtractFilePath(filePathKey);
                //filePathKey is used as key for S3
                S3FileInfo s3ficheck = new S3FileInfo(S3Client, _AWSBucket, filePathKey);
                if (!s3ficheck.Exists)
                    return false;

                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = _AWSBucket,
                    Key = filePathKey
                };
                using (GetObjectResponse response = S3Client.GetObject(request))
                {
                    String destTempFolder = Path.Combine(destFilePath, destFilePathKey);
                    if (!File.Exists(destTempFolder))
                    {
                        response.WriteResponseStreamToFile(destTempFolder);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To delete document
        /// </summary>
        /// <param name="filePathKey">It contains path of S3 file</param>
        /// <returns></returns>
        public Boolean DeleteDocument(String filePathKey)
        {
            try
            {
                if (filePathKey.IsNullOrEmpty())
                {
                    return false;
                }
                String dirInRootBucket = ConfigurationManager.AppSettings["AWSDirPathInBucket"].ToString();
                if (!dirInRootBucket.IsNullOrEmpty())
                {
                    if (!filePathKey.Contains(dirInRootBucket))
                    {
                        filePathKey = dirInRootBucket + @"/" + filePathKey;
                    }
                }
                //if (!dirInRootBucket.IsNullOrEmpty())
                //{
                //    filePathKey = dirInRootBucket + @"/" + filePathKey;
                //}
                filePathKey = ExtractFilePath(filePathKey);
                //delete file from file system
                DeleteObjectResponse dor = S3Client.DeleteObject(new DeleteObjectRequest()
                {
                    BucketName = _AWSBucket,
                    Key = filePathKey
                });
                S3FileInfo s3ficheck = new S3FileInfo(S3Client, _AWSBucket, filePathKey);
                if (!s3ficheck.Exists)
                    return true;
                else
                    return false;
                //if (dor.HttpStatusCode == System.Net.HttpStatusCode.OK)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To move document from src S3 to dest S3
        /// </summary>
        /// <param name="srcFilePathKey">It contains path of src S3 file</param>
        /// <param name="destFilePathKey">It contains path of dest S3 file</param>
        /// <returns></returns>
        public String MoveDocument(String srcFilePathKey, String destFilePathKey)
        {
            try
            {
                if (srcFilePathKey.IsNullOrEmpty() || destFilePathKey.IsNullOrEmpty())
                {
                    return "";
                }
                String dirInRootBucket = ConfigurationManager.AppSettings["AWSDirPathInBucket"].ToString();
                if (!dirInRootBucket.IsNullOrEmpty())
                {
                    if (!srcFilePathKey.Contains(dirInRootBucket))
                    {
                        srcFilePathKey = dirInRootBucket + @"/" + srcFilePathKey;
                    }
                    if (!destFilePathKey.Contains(dirInRootBucket))
                    {
                        destFilePathKey = dirInRootBucket + @"/" + destFilePathKey;
                    }
                }
                //if (!dirInRootBucket.IsNullOrEmpty())
                //{
                //    srcFilePathKey = dirInRootBucket + @"/" + srcFilePathKey;
                //    destFilePathKey = dirInRootBucket + @"/" + destFilePathKey;
                //}
                srcFilePathKey = ExtractFilePath(srcFilePathKey);
                destFilePathKey = ExtractFilePath(destFilePathKey);
                //copy file from file system
                CopyObjectResponse cor = S3Client.CopyObject(new CopyObjectRequest()
                {
                    SourceBucket = _AWSBucket,
                    SourceKey = srcFilePathKey,
                    DestinationBucket = _AWSBucket,
                    DestinationKey = destFilePathKey
                });

                //delete original file from file system
                DeleteObjectResponse dor = S3Client.DeleteObject(new DeleteObjectRequest()
                {
                    BucketName = _AWSBucket,
                    Key = srcFilePathKey
                });

                if (cor.HttpStatusCode == System.Net.HttpStatusCode.OK || dor.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return destFilePathKey;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To extract file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private String ExtractFilePath(String filePath)
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
    }
}
