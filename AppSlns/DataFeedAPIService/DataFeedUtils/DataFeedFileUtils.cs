using INTSOF.Utils;
using Ionic.Zip;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DataFeedAPIService.DataFeedUtils
{
    public static class DataFeedFileUtils
    {
        public static String GetFileExtension(String outputCode)
        {
            String fileExtension = String.Empty;

            switch (outputCode.ToUpper())
            {
                case DataFeedUtilityConstants.LKP_OUTPUT_CSV:
                    fileExtension = ".csv";
                    break;
                case DataFeedUtilityConstants.LKP_OUTPUT_XML:
                    fileExtension = ".xml";
                    break;
                case DataFeedUtilityConstants.LKP_OUTPUT_TEXT:
                    fileExtension = ".txt";
                    break;
                case TextFileEnum.TEXT_MD5:
                    fileExtension = ".MD5sig";
                    break;
                case TextFileEnum.TEXT_DATA:
                    fileExtension = ".data";
                    break;
                case TextFileEnum.TEXT_ZIP:
                    fileExtension = ".zip";
                    break;
                case TextFileEnum.TEXT_MANIFEST:
                    fileExtension = ".manifest";
                    break;

                default: fileExtension = ".csv";
                    break;
            }
            return fileExtension;
        }

        public static FileNamePath CreateZippedTextDataFile(String fileName, String tempFilePath, String localPath)
        
        {
            FileNamePath zFile = new FileNamePath();
            //MD5 File
            String zippedDataFeedFile = String.Empty;
            String md5Hash = String.Empty;
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(localPath))
                {
                    md5Hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
                }
            }
            String fullMD5FileName = fileName + GetFileExtension(TextFileEnum.TEXT_MD5.ToString());
            String mdf5FileLocalPath = Path.Combine(tempFilePath, fullMD5FileName);
            File.WriteAllText(mdf5FileLocalPath, md5Hash);

            String fullZipFileName = String.Empty;
            String zipFileLocalPath = String.Empty;
            String zipFileExt = GetFileExtension(TextFileEnum.TEXT_ZIP.ToString());

            //Zip File
            using (ZipFile zip = new ZipFile())
            {
                //zip.AddFile(mdf5FileLocalPath);
                // add the report into a different directory in the archive
                //zip.AddFile(localPath, "");

                fullZipFileName = fileName + zipFileExt;
                zipFileLocalPath = Path.Combine(tempFilePath, fullZipFileName);
                zip.AddDirectory(tempFilePath, "");
                zip.Save(zipFileLocalPath);
                zippedDataFeedFile = zipFileLocalPath;
            }

            try
            {
                File.Delete(mdf5FileLocalPath);
                File.Delete(localPath);
            }
            catch (Exception)
            {
            }

            zFile.FileName = fileName;
            zFile.FullFileName = fullZipFileName;
            zFile.FullFilePath = zipFileLocalPath;
            zFile.FileExtension = zipFileExt;
            return zFile;
        }

        public static FileNamePath CreateManifestFile(String fullDataFeedFileName, String fileName, String remotePath, String tempFilePath)
        {
            //String manifestFilePath = String.Empty;
            //String manifestFileName = String.Empty;
            //ConfigFile=configGetData.ini
            //ArchiveFile=C:/Users/JobRunner/Documents/CodeRoot/AMSGenReportAndPush/internal/2014-12-30-05-21-04-Liberty.zip
            //ManifestFile=C:/Users/JobRunner/Documents/CodeRoot/AMSGenReportAndPush/internal/2014-12-30-05-21-04-Liberty.manifest
            //ArchiveTargetFile=2014-12-30-05-21-04-Liberty.zip
            //ManifestTargetFile=2014-12-30-05-21-04-Liberty.manifest
            //RemoteDirectories=Documents/Incoming_Files
            //Send=C:/Users/JobRunner/Documents/CodeRoot/AMSGenReportAndPush/internal/2014-12-30-05-20-34-Liberty-GetDefaultInstitutionExtract.data
            //Send=C:/Users/JobRunner/Documents/CodeRoot/AMSGenReportAndPush/internal/2014-12-30-05-20-34-Liberty-GetDefaultInstitutionExtract.MD5sig
            //Send=C:/Users/JobRunner/Documents/CodeRoot/AMSGenReportAndPush/internal/2014-12-30-05-20-34-Liberty-GetDefaultInstitutionExtract.out.pk
            //DONE

            FileNamePath manifestFile = new FileNamePath();

            String manifestFileExt = GetFileExtension(TextFileEnum.TEXT_MANIFEST.ToString());
            String manifestFileName = fileName + manifestFileExt;
            String manifestFilePath = Path.Combine(tempFilePath, manifestFileName);

            StringBuilder mSB = new StringBuilder();

            mSB.Append("ConfigFile=configGetData.ini" + Environment.NewLine);
            mSB.Append("ArchiveFile=" + Path.Combine(tempFilePath, fullDataFeedFileName) + Environment.NewLine);
            mSB.Append("ManifestFile=" + manifestFilePath + Environment.NewLine);
            mSB.Append("ArchiveTargetFile=" + fileName + GetFileExtension(TextFileEnum.TEXT_ZIP.ToString()) + Environment.NewLine);
            mSB.Append("ManifestTargetFile=" + manifestFileName + Environment.NewLine);
            mSB.Append("RemoteDirectories=" + remotePath + Environment.NewLine);
            mSB.Append("Send=" + Path.Combine(tempFilePath, fullDataFeedFileName) + Environment.NewLine);

            String fullMD5FileName = fileName + GetFileExtension(TextFileEnum.TEXT_MD5.ToString());
            String mdf5FileLocalPath = Path.Combine(tempFilePath, fullMD5FileName);

            mSB.Append("Send=" + mdf5FileLocalPath + Environment.NewLine);
            //mSB.Append("Send=" + Environment.NewLine);
            mSB.Append("DONE" + Environment.NewLine);

            File.WriteAllText(manifestFilePath, mSB.ToString());

            manifestFile.FileName = fileName;
            manifestFile.FullFileName = manifestFileName;
            manifestFile.FullFilePath = manifestFilePath;
            manifestFile.FileExtension = manifestFileExt;
            return manifestFile;
        }
    }
}
