using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using INTSOF.Logger;
using INTSOF.Logger.factory;
using SautinSoft;
using Winnovative.PDFMerge;
using System.Drawing.Imaging;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Reflection;

//using INTSOF.Logger;
//using INTSOF.Logger.factory;
namespace INTSOF.Queues
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PDFConversion" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PDFConversion.svc or PDFConversion.svc.cs at the Solution Explorer and start debugging.
    public class PDFConversion : IPDFConversion
    {
        private ILogger _logger;
        public PDFConversion()
        {
            _logger = SysXLoggerFactory.GetInstance().GetLogger();
            _logger.Info("PDFConversion constructor called.");
        }

        public string DoWork()
        {
            return "In New PDFConversion Service";
        }

        /// <summary>
        /// Invoked to generate pdf.
        /// </summary>
        /// <param name="urlToConvert"></param>
        /// <returns></returns>
        public byte[] GeneratePDF(String urlToConvert)
        {
            try
            {
                _logger.Info("Inside the GeneratePDF Method.");
                Winnovative.PdfConverter pdfConverter = new Winnovative.PdfConverter();
                _logger.Info("Url to Convert as pdf:" + urlToConvert);
                // set the license key - required              
                pdfConverter.LicenseKey = "OrSntaS1pKKktay7pbWmpLukp7usrKys";
                // set the converter options - optional
                pdfConverter.PdfDocumentOptions.PdfPageSize = Winnovative.PdfPageSize.A4;
                pdfConverter.PdfDocumentOptions.PdfCompressionLevel = Winnovative.PdfCompressionLevel.Normal;
                pdfConverter.PdfDocumentOptions.PdfPageOrientation = Winnovative.PdfPageOrientation.Portrait;
                //set margin for the pdf document
                pdfConverter.PdfDocumentOptions.LeftMargin = 8;
                pdfConverter.PdfDocumentOptions.TopMargin = 8;
                pdfConverter.PdfDocumentOptions.RightMargin = 8;
                pdfConverter.PdfDocumentOptions.BottomMargin = 8;
                // optionally wait for asynchronous items
                pdfConverter.ConversionDelay = 2;
                _logger.Info("Converting url to pdf");
                // Performs the conversion and get the pdf document bytes that can 
                // be saved to a file or sent as a browser response
                byte[] pdfBytes = pdfConverter.GetPdfBytesFromUrl(urlToConvert);

                _logger.Info("Converted url to pdf");
                return pdfBytes;
                //return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to generate pdf.", ex);
                return null;
            }
        }


        /// <summary>
        /// Converts a list of applicant documents to pdf
        /// </summary>
        /// <param name="applicantDocuments">List of Applicant Documents to be converted</param>
        /// <param name="tenantID">InstitutionID</param>
        /// <returns>List of Converted documents</returns>
        public List<ApplicantDoc> ConvertApplicantDocumentToPDF(List<ApplicantDoc> applicantDocuments, Int32 tenantID)
        {
            List<ApplicantDoc> convertedApplicantDocuments = new List<ApplicantDoc>();
            String strLogginDocInfo = String.Empty;
            try
            {
                if (applicantDocuments != null && applicantDocuments.Count > 0)
                {
                    strLogginDocInfo = " - Doc Count: " + applicantDocuments.Count() + " - First DocId: " + applicantDocuments[0].ApplicantDocumentID + " - Tenant Id: " + tenantID;
                    _logger.Info("Entered ConvertApplicantDocumentToPDF method." + strLogginDocInfo);

                    applicantDocuments = applicantDocuments.Where(condition => String.IsNullOrEmpty(condition.PdfDocPath)).ToList();
                    UseOffice.eDirection conversionDirection = UseOffice.eDirection.DOC_to_PDF;
                    String destFolder = "Tenant(" + tenantID.ToString() + @")\";

                    foreach (ApplicantDoc appDocument in applicantDocuments)
                    {
                        ApplicantDoc docDetail = new ApplicantDoc();
                        if (!String.IsNullOrEmpty(appDocument.DocumentPath))
                        {
                            byte[] fileBytes = CommonFileManager.RetrieveDocument(appDocument.DocumentPath);
                            //fileBytes = PDFBytesToFlattenPdfForm(fileBytes);
                            //[UAT-862]:- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                            //Added fileBytes length check for checking the 0 size file that not included in merging.
                            if (fileBytes != null && fileBytes.Length > 0)
                            {
                                String fileExtension = Path.GetExtension(appDocument.DocumentPath).ToLower();
                                String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                                String pdfDocPathFileName = destFolder + "UD_" + tenantID.ToString() + "_" + appDocument.ApplicantDocumentID + "_" + date + ".pdf";
                                //Get original file size and MD5Hash
                                Int32? originalDocSize = appDocument.Size;
                                String originalDocMD5Hash = CommonFileManager.GetMd5Hash(fileBytes);
                                bool IsPdfDocumnetFormFlattening = false;

                                try
                                {
                                    switch (fileExtension)
                                    {
                                        case ".pdf":
                                            appDocument.PdfDocPath = pdfDocPathFileName;
                                            _logger.Info("Before the call of convert Form Flattening in PDF Conversion File");
                                            //Release 180:4132
                                            int configFormFlattening = Convert.ToInt32(ConfigurationManager.AppSettings["PDFDocumentFlattening"]);
                                            if (configFormFlattening == 1 || configFormFlattening == 2)
                                            {
                                                docDetail = AddSepearatorToPDF(ConvertFormFlattening(fileBytes), appDocument);
                                                IsPdfDocumnetFormFlattening = true;

                                            }
                                            else
                                            {
                                                docDetail = AddSepearatorToPDF(fileBytes, appDocument);
                                                IsPdfDocumnetFormFlattening = false;
                                            }
                                            docDetail.ApplicantDocumentID = appDocument.ApplicantDocumentID;
                                            docDetail.PdfFileName = appDocument.FileName;
                                            docDetail.ConversionNotes = "Converted Successfully!";
                                            _logger.Info("ConvertApplicantDocumentToPDF: Converted Successfully!");
                                            break;
                                        case ".jpg":
                                        case ".jpe":
                                        case ".jpeg":
                                        case ".png":
                                        case ".tiff":
                                        case ".tif":
                                        case ".gif":
                                        case ".bmp":
                                            _logger.Info("Called ConvertImageToPDF method.");
                                            appDocument.PdfDocPath = pdfDocPathFileName;
                                            docDetail = ConvertImageToPDF(fileBytes, appDocument, destFolder);
                                            docDetail.ApplicantDocumentID = appDocument.ApplicantDocumentID;
                                            break;
                                        case ".doc":
                                        case ".docx":
                                            _logger.Info("Called ConvertDocumentToPDF method.");
                                            appDocument.PdfDocPath = pdfDocPathFileName;
                                            conversionDirection = fileExtension == ".docx" ? SautinSoft.UseOffice.eDirection.DOCX_to_PDF : SautinSoft.UseOffice.eDirection.DOC_to_PDF;
                                            docDetail = ConvertDocumentToPDF(fileBytes, appDocument, conversionDirection);
                                            docDetail.ApplicantDocumentID = appDocument.ApplicantDocumentID;
                                            break;
                                        case ".xls":
                                        case ".xlsx":
                                            _logger.Info("Called ConvertDocumentToPDF method.");
                                            appDocument.PdfDocPath = pdfDocPathFileName;
                                            conversionDirection = fileExtension == ".xlsx" ? SautinSoft.UseOffice.eDirection.XLSX_to_PDF : SautinSoft.UseOffice.eDirection.XLS_to_PDF;
                                            docDetail = ConvertDocumentToPDF(fileBytes, appDocument, conversionDirection);
                                            docDetail.ApplicantDocumentID = appDocument.ApplicantDocumentID;
                                            break;
                                        case ".txt":
                                            _logger.Info("Called ConvertDocumentToPDF method.");
                                            appDocument.PdfDocPath = pdfDocPathFileName;
                                            conversionDirection = SautinSoft.UseOffice.eDirection.TEXT_to_PDF;
                                            docDetail = ConvertDocumentToPDF(fileBytes, appDocument, conversionDirection);
                                            docDetail.ApplicantDocumentID = appDocument.ApplicantDocumentID;
                                            break;
                                        case ".rtf":
                                            _logger.Info("Called ConvertDocumentToPDF method.");
                                            appDocument.PdfDocPath = pdfDocPathFileName;
                                            conversionDirection = SautinSoft.UseOffice.eDirection.RTF_to_PDF;
                                            docDetail = ConvertDocumentToPDF(fileBytes, appDocument, conversionDirection);
                                            docDetail.ApplicantDocumentID = appDocument.ApplicantDocumentID;
                                            break;
                                        default:
                                            break;
                                    }

                                    //Set original file size and MD5Hash
                                    docDetail.IsPdfDocumnetFormFlattening = IsPdfDocumnetFormFlattening;
                                    docDetail.OriginalDocSize = originalDocSize;
                                    docDetail.OriginalDocMD5Hash = originalDocMD5Hash;
                                }
                                catch (Exception ex)
                                {
                                    docDetail.ConversionNotes = ex.Message;
                                }
                            }
                            else
                            {
                                docDetail.ApplicantDocumentID = appDocument.ApplicantDocumentID;
                                docDetail.ConversionNotes = "Cannot get file bytes.";
                                _logger.Error("ConvertApplicantDocumentToPDF: Cannot get file bytes.");
                            }
                        }
                        else
                        {
                            docDetail.ApplicantDocumentID = appDocument.ApplicantDocumentID;
                            docDetail.ConversionNotes = "Applicant document path is null.";
                            _logger.Error("ConvertApplicantDocumentToPDF: Applicant document path is null.");
                        }
                        convertedApplicantDocuments.Add(docDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error while converting ApplicantDocument." + strLogginDocInfo, ex);
            }
            _logger.Info("Exiting from ConvertApplicantDocumentToPDF method." + strLogginDocInfo);
            return convertedApplicantDocuments;
        }

        /// <summary>
        /// Append converted document to pdf.
        /// </summary>
        /// <param name="unifiedDocumentID">UnifiedDocuemntID</param>
        /// <param name="tenantID">InstitutionID</param>
        /// <param name="applicantDocuments">List of Applicant Documents to be merged</param>
        /// <param name="previousUnifiedPdfPath">Previous Unified File Location</param>
        /// <param name="sequenceOrder">Sequence order</param>
        /// <param name="previousPages">Previous Pages</param>
        /// <returns>List of ApplicantDocumentMerging</returns>
        public List<ApplicantDocMerging> AppendConvertedDocumentToPDF(Int32 unifiedDocumentID, Int32 tenantID, List<ApplicantDocToBeMerged> applicantDocuments,
            String previousUnifiedPdfPath, Int32 sequenceOrder, Int32 previousPages,Boolean isDocumentFlatten)
        {
            String strLoggingInfo = String.Empty;
            Int32 totalPageCount = 0;
            Int32 pageCount = 0;
            List<ApplicantDocMerging> mergedApplicantDocuments = new List<ApplicantDocMerging>();
            Boolean isMerged = false;
            String destFolder = "Tenant(" + tenantID.ToString() + @")\";
            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
            String pdfDocPathFileName = destFolder + "UD_" + tenantID.ToString() + "_" + unifiedDocumentID + "_" + date + ".pdf";
            String returnFilePath = String.Empty;
            try
            {
                PdfDocumentOptions pdfDocumentOptions = new PdfDocumentOptions();
                pdfDocumentOptions.PdfCompressionLevel = PDFCompressionLevel.Normal;
                pdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
                pdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait;

                PDFMerge pdfMerge = new PDFMerge(pdfDocumentOptions);
                pdfMerge.LicenseKey = ConfigurationManager.AppSettings["WinnovativePDFMerge"];

                if (applicantDocuments != null && applicantDocuments.Count > 0)
                {
                    strLoggingInfo = " - Count: " + applicantDocuments.Count() + " - First Doc: " + applicantDocuments[0].ApplicantDocumentID;
                    _logger.Info("Enetered AppendConvertedDocumentToPDF method." + strLoggingInfo);
                    sequenceOrder = sequenceOrder + 1;

                    if (!String.IsNullOrEmpty(previousUnifiedPdfPath))
                    {
                        try
                        {
                            byte[] fileBytes = CommonFileManager.RetrieveDocument(previousUnifiedPdfPath);
                            if (fileBytes != null)
                            {
                                if (!isDocumentFlatten)
                                {

                                    int configFormFlattening = Convert.ToInt32(ConfigurationManager.AppSettings["PDFDocumentFlattening"]);
                                    if (configFormFlattening == 2)
                                    {
                                        fileBytes = ConvertFormFlattening(fileBytes);
                                        isDocumentFlatten = true;
                                       
                                    }
                                }
                                MemoryStream stream = new MemoryStream(fileBytes);
                                pdfMerge.AppendPDFStream(stream);
                                totalPageCount = pdfMerge.GetCurrentPageCount();
                                isMerged = true;
                            }
                            else
                            {
                                _logger.Error("AppendConvertedDocumentToPDF: Cannot get file bytes of previous unified document.");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Error("AppendConvertedDocumentToPDF: Error while merging previous unified document." + strLoggingInfo, ex);
                        }
                    }
                    else
                    {
                        int configFormFlatten = Convert.ToInt32(ConfigurationManager.AppSettings["PDFDocumentFlattening"]);
                        if (configFormFlatten == 2)
                        {
                            isDocumentFlatten = true;
                        }
                    }

                    foreach (ApplicantDocToBeMerged appDocument in applicantDocuments)
                    {
                        ApplicantDocMerging appDocumentMerged = new ApplicantDocMerging();
                        try
                        {
                            appDocumentMerged.ADM_ApplicantDocumentID = appDocument.ApplicantDocumentID;
                            appDocumentMerged.ADM_UnifiedPdfDocumentID = unifiedDocumentID;
                            appDocumentMerged.ADM_IsDeleted = false;
                            if (sequenceOrder == 1)
                            {
                                byte[] fileBytes = CommonFileManager.RetrieveDocument(appDocument.PdfDocPath);
                                //[UAT-862]:- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                                //Added fileBytes length check for checking the 0 size file that not included in merging.
                                if (fileBytes != null && fileBytes.Length > 0)
                                {
                                    try
                                    {
                                        MemoryStream stream = new MemoryStream(fileBytes);
                                        pdfMerge.AppendPDFStream(stream);
                                        pageCount = pdfMerge.GetCurrentPageCount();
                                        appDocumentMerged.ADM_MergingNotes = "Merged Successfully!";
                                        appDocumentMerged.ADM_TotalPages = pageCount;
                                        appDocumentMerged.ADM_StartPageNum = 1;
                                        appDocumentMerged.ADM_EndPageNum = pageCount;
                                        totalPageCount = pageCount;
                                        appDocumentMerged.ADM_SequenceOrder = sequenceOrder;
                                        sequenceOrder++;
                                        _logger.Info("AppendConvertedDocumentToPDF: Merged Successfully!");
                                        isMerged = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Error("AppendConvertedDocumentToPDF: Error while merging Applicant Documents." + appDocument.ApplicantDocumentID.ToString() + strLoggingInfo, ex);
                                        if (ex.Message != null)
                                            appDocumentMerged.ADM_MergingNotes = ex.Message;
                                        else
                                            appDocumentMerged.ADM_MergingNotes = "Error While Merging.";
                                        mergedApplicantDocuments.Add(appDocumentMerged);
                                        continue;
                                    }
                                }
                                else
                                {
                                    appDocumentMerged.ADM_MergingNotes = "Cannot get file bytes.";
                                    _logger.Error("AppendConvertedDocumentToPDF: Cannot get file bytes.");
                                }
                            }
                            else
                            {
                                byte[] fileBytes = CommonFileManager.RetrieveDocument(appDocument.PdfDocPath);
                                //[UAT-862]:- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                                //Added fileBytes length check for checking the 0 size file that not included in merging.
                                if (fileBytes != null && fileBytes.Length > 0)
                                {
                                    try
                                    {

                                        MemoryStream stream = new MemoryStream(fileBytes);
                                        pdfMerge.AppendPDFStream(stream);
                                        pageCount = pdfMerge.GetCurrentPageCount();
                                        appDocumentMerged.ADM_MergingNotes = "Merged Successfully!";
                                        appDocumentMerged.ADM_StartPageNum = totalPageCount + 1;
                                        appDocumentMerged.ADM_EndPageNum = pageCount;
                                        appDocumentMerged.ADM_TotalPages = (appDocumentMerged.ADM_EndPageNum - appDocumentMerged.ADM_StartPageNum) + 1;
                                        totalPageCount = pageCount;
                                        appDocumentMerged.ADM_SequenceOrder = sequenceOrder;
                                        sequenceOrder++;
                                        _logger.Info("AppendConvertedDocumentToPDF: Merged Successfully!");
                                        isMerged = true;

                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Error("AppendConvertedDocumentToPDF: Error while merging Applicant Documents." + appDocument.ApplicantDocumentID.ToString() + strLoggingInfo, ex);
                                        if (ex.Message != null)
                                            appDocumentMerged.ADM_MergingNotes = ex.Message;
                                        else
                                            appDocumentMerged.ADM_MergingNotes = "Error While Merging.";
                                        mergedApplicantDocuments.Add(appDocumentMerged);
                                        continue;
                                    }
                                }
                                else
                                {
                                    _logger.Error("AppendConvertedDocumentToPDF: Cannot get file bytes.");
                                    appDocumentMerged.ADM_MergingNotes = "Cannot get file bytes.";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Error("AppendConvertedDocumentToPDF: Error while merging Applicant Documents." + appDocument.ApplicantDocumentID.ToString() + strLoggingInfo, ex);
                            if (ex.Message != null)
                                appDocumentMerged.ADM_MergingNotes = ex.Message;
                            else
                                appDocumentMerged.ADM_MergingNotes = "Error While Merging.";
                            mergedApplicantDocuments.Add(appDocumentMerged);
                            continue;
                        }
                        mergedApplicantDocuments.Add(appDocumentMerged);
                    }
                    if (isMerged)
                    {
                        //Save file at temporary location from bytes
                        String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
                        if (!tempFilePath.EndsWith(@"\"))
                        {
                            tempFilePath += @"\";
                        }
                        tempFilePath += "TempFiles\\";
                        if (!Directory.Exists(tempFilePath))
                            Directory.CreateDirectory(tempFilePath);
                        //Save file at temporary location
                        String mergedTempFilePath = Path.Combine(tempFilePath, Guid.NewGuid().ToString() + ".pdf");
                        pdfMerge.SaveMergedPDFToFile(mergedTempFilePath);
                       

                        returnFilePath = CommonFileManager.SaveDocument(mergedTempFilePath, pdfDocPathFileName);

                        try
                        {
                            if (File.Exists(mergedTempFilePath))
                                File.Delete(mergedTempFilePath);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("AppendConvertedDocumentToPDF: Error while merging Applicant Documents." + strLoggingInfo, ex);
            }
            _logger.Info("Exiting from AppendConvertedDocumentToPDF method." + strLoggingInfo);
            mergedApplicantDocuments[0].UnifiedDocumentPath = returnFilePath;
            mergedApplicantDocuments[0].IsFlattenFile = isDocumentFlatten;
            return mergedApplicantDocuments;
        }

        //private byte[] PDFBytesToFlattenPdfForm(byte[] pdfBytes)
        //{
        //    PdfReader reader = new iTextSharp.text.pdf.PdfReader(pdfBytes);
        //    var memStream = new MemoryStream();
        //    var stamper = new PdfStamper(reader, memStream) { FormFlattening = true };
        //    stamper.Close();
        //    return memStream.ToArray();
        //}


        //UAT:4132
        private byte[] ConvertFormFlattening(byte[] fileBytes)
        {
                byte[] tempFileByte = null;
              _logger.Info("Class Name: " + MethodBase.GetCurrentMethod().DeclaringType.Name + ":-  Function Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :- Call Method");

                using (PdfReader compressionReader = new PdfReader(fileBytes))
                {
                    using (MemoryStream compressionsMS = new MemoryStream())
                    {
                        using (PdfStamper compressionStamper = new PdfStamper(compressionReader, compressionsMS))
                        {
                            compressionStamper.FormFlattening = true;
                            compressionStamper.Close();
                        }
                        tempFileByte = compressionsMS.ToArray();

                    }
                   
                }
            return tempFileByte;


        }

        /// <summary>
        /// Converts image to pdf
        /// </summary>
        /// <param name="filePath">File Path</param>
        /// <param name="fileName">File Name</param>
        /// <param name="pdfDocPathFileName">Coverted PDF File Path</param>
        /// <returns>ApplicantDocument</returns>
        private ApplicantDoc ConvertImageToPDF(byte[] fileBytes, ApplicantDoc appDoc, String destFolder)
        {
            String fileName = appDoc.FileName;
            String filePath = appDoc.DocumentPath;
            Boolean isCompressed = appDoc.IsCompressed;
            String strLoggingInfo = String.Empty;
            ApplicantDoc docDetail = new ApplicantDoc();
            try
            {
                strLoggingInfo = " - Path: " + filePath + " - File: " + fileName;
                _logger.Info("Entered ConvertImageToPDF method." + strLoggingInfo);
                //[UAT-862]:- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                //Added fileBytes length check for checking the 0 size file that not included in merging.
                if (fileBytes != null && fileBytes.Length > 0)
                {
                    if (isCompressed == false)
                    {
                        //Resize and compress file
                        byte[] compressedFileBytes = ResizedImage(fileBytes, filePath, destFolder);
                        if (compressedFileBytes != null)
                        {
                            fileBytes = compressedFileBytes;
                            docDetail.IsCompressed = true;
                            docDetail.Size = compressedFileBytes.Length;
                        }
                    }

                    PdfVision pdfVision = new PdfVision();
                    // Serial Key for activation
                    pdfVision.Serial = ConfigurationManager.AppSettings["PdfVisionSerialKey"];

                    ArrayList arImgBytes = new ArrayList();
                    arImgBytes.Add(fileBytes);

                    Int32 index = fileName.LastIndexOf('.');
                    String pdfFileName = fileName.Substring(0, index) + ".pdf";

                    //set Page Orientation
                    pdfVision.PageStyle.PageOrientation.Auto();

                    //convert arraylist with image streams to pdf stream
                    byte[] pdfBytes = pdfVision.ConvertImageStreamArrayToPDFStream(arImgBytes);
                    try
                    {
                        ApplicantDoc _docDetail = AddSepearatorToPDF(pdfBytes, appDoc);
                        docDetail.PdfDocPath = _docDetail.PdfDocPath;
                        docDetail.TotalPages = _docDetail.TotalPages;
                        docDetail.PdfFileName = pdfFileName;
                        docDetail.ConversionNotes = "Converted Successfully!";
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("ConvertImageToPDF: Error while converting image to pdf." + strLoggingInfo, ex);
                        if (ex.Message != null)
                            docDetail.ConversionNotes = ex.Message;
                        else
                            docDetail.ConversionNotes = "Exception occured while converting image to pdf";
                    }
                }
                else
                {
                    docDetail.ConversionNotes = "Error While converting image to pdf. Cannot get file bytes.";
                    _logger.Error("ConvertImageToPDF: Error While converting image to pdf. Cannot get file bytes.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("ConvertImageToPDF: Error while converting image to pdf." + strLoggingInfo, ex);
                if (ex.Message != null)
                    docDetail.ConversionNotes = ex.Message;
                else
                    docDetail.ConversionNotes = "Exception occured while converting image to pdf";
            }
            _logger.Info("Exiting from ConvertImageToPDF method." + strLoggingInfo);
            return docDetail;
        }

        /// <summary>
        /// Converts Document to pdf
        /// </summary>
        /// <param name="filePath">File Path</param>
        /// <param name="fileName">File Name</param>
        /// <param name="pdfDocPathFileName">Coverted PDF File Path</param>
        /// <returns>ApplicantDocument</returns>
        private ApplicantDoc ConvertDocumentToPDF(byte[] fileBytes, ApplicantDoc appDoc, UseOffice.eDirection conversionDirection)
        {
            String fileName = appDoc.FileName;
            String filePath = appDoc.DocumentPath;
            String strLoggingInfo = String.Empty;
            ApplicantDoc docDetail = new ApplicantDoc();
            UseOffice useOffice = null;
            int ret = 0;
            try
            {
                strLoggingInfo = " - Path: " + filePath + " - File: " + fileName;
                _logger.Info("Entered ConvertDocumentToPDF method." + strLoggingInfo);
                //[UAT-862]:- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                //Added fileBytes length check for checking the 0 size file that not included in merging.
                if (fileBytes != null && fileBytes.Length > 0)
                {
                    Int32 index = fileName.LastIndexOf('.');
                    String pdfFileName = fileName.Substring(0, index) + ".pdf";
                    String ext = fileName.Substring(index, fileName.Length - index);

                    // 1. Launch UseOffice .Net and start converting
                    useOffice = new UseOffice();
                    // Serial Key for activation
                    useOffice.Serial = ConfigurationManager.AppSettings["UseOfficeSerialKey"];

                    useOffice.PageStyle.PageOrientation.Auto();
                    useOffice.PageStyle.PageSize.WidthInch(85f);

                    switch (conversionDirection)
                    {
                        case UseOffice.eDirection.XLSX_to_PDF:
                        case UseOffice.eDirection.XLS_to_PDF:
                            //Prepare UseOffice .Net, loads MS Excel in memory
                            ret = useOffice.InitExcel();
                            break;
                        case UseOffice.eDirection.DOCX_to_PDF:
                        case UseOffice.eDirection.DOC_to_PDF:
                        case UseOffice.eDirection.TEXT_to_PDF:
                        case UseOffice.eDirection.RTF_to_PDF:
                            //Prepare UseOffice .Net, loads MS Word in memory
                            ret = useOffice.InitWord();
                            break;
                        default:
                            break;
                    }

                    //Return values:
                    //0 - Loading successfully
                    //1 - Can't load MS Word/MS Excel library in memory 
                    if (ret == 1)
                    {
                        _logger.Error("ConvertDocumentToPDF: Can't load MS Word/MS Excel library in memory.");
                        docDetail.ConversionNotes = "Can't load MS Word/MS Excel library in memory.";
                    }
                    else
                    {
                        _logger.Info("ConvertDocumentToPDF: UseOffice .Net, loaded MS Word/MS Excel in memory.");

                        //Save file at temporary location from bytes
                        String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
                        if (!tempFilePath.EndsWith(@"\"))
                        {
                            tempFilePath += @"\";
                        }
                        tempFilePath += "TempFiles\\";
                        if (!Directory.Exists(tempFilePath))
                            Directory.CreateDirectory(tempFilePath);
                        //Save file at temporary location
                        String newTempFilePath = Path.Combine(tempFilePath, Guid.NewGuid().ToString());
                        //Writing bytes in destination file using FileStream
                        using (FileStream _FileStream = new FileStream(newTempFilePath,
                                        System.IO.FileMode.Create,
                                        System.IO.FileAccess.Write))
                        {
                            _FileStream.Write(fileBytes, 0, fileBytes.Length);
                        }

                        String convertedTempFile = Path.Combine(tempFilePath, Guid.NewGuid().ToString() + ".pdf");

                        // 2.  Convert the file to a desired result
                        int result = useOffice.ConvertFile(newTempFilePath, convertedTempFile, conversionDirection);

                        switch (result)
                        {
                            // 3. Return output on basis of result
                            case 0:
                                byte[] pdfBytes = GetFileBytes(convertedTempFile);
                                ApplicantDoc _docDetail = AddSepearatorToPDF(pdfBytes, appDoc);
                                docDetail.PdfDocPath = _docDetail.PdfDocPath;
                                docDetail.TotalPages = _docDetail.TotalPages;
                                docDetail.PdfFileName = pdfFileName;
                                docDetail.ConversionNotes = "Converted Successfully!";
                                _logger.Info("ConvertDocumentToPDF: Converted document to pdf sucessfully.");
                                break;
                            case 1:
                                _logger.Error("ConvertDocumentToPDF: Error while converting document to pdf.  Can't open input file. Check that you are using full local path to input file, URL and relative path are not supported.");
                                docDetail.ConversionNotes = "Can't open input file. Check that you are using full local path to input file, URL and relative path are not supported.";
                                break;
                            case 2:
                                _logger.Error("ConvertDocumentToPDF: Error while converting document to pdf. Can't create output file. Please check that you have permissions to write by this path or probably this path already used by another application.");
                                docDetail.ConversionNotes = "Can't create output file. Please check that you have permissions to write by this path or probably this path already used by another application.";
                                break;
                            case 3:
                                _logger.Error("ConvertDocumentToPDF: Error while converting document to pdf. Converting failed, please contact with SautinSoft Support Team.");
                                docDetail.ConversionNotes = "Converting failed, please contact with SautinSoft Support Team.";
                                break;
                            case 4:
                                _logger.Error("ConvertDocumentToPDF: Error while converting document to pdf. MS Office isn't installed. The component requires that any of these versions of MS Office should be installed: 2000, XP, 2003, 2007 or 2010.");
                                docDetail.ConversionNotes = "MS Office isn't installed. The component requires that any of these versions of MS Office should be installed: 2000, XP, 2003, 2007 or 2010.";
                                break;
                            default:
                                _logger.Error("ConvertDocumentToPDF: Error while converting document to pdf. Converting error!");
                                docDetail.ConversionNotes = "Converting error!";
                                break;
                        }

                        try
                        {
                            if (File.Exists(newTempFilePath))
                                File.Delete(newTempFilePath);
                            if (File.Exists(convertedTempFile))
                                File.Delete(convertedTempFile);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                else
                {
                    docDetail.ConversionNotes = "Error While converting document to pdf. Cannot get file bytes.";
                    _logger.Error("ConvertDocumentToPDF: Error While converting document to pdf. Cannot get file bytes.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("ConvertDocumentToPDF: Error while converting document to pdf." + strLoggingInfo, ex);
                if (ex.Message != null)
                    docDetail.ConversionNotes = ex.Message;
                else
                    docDetail.ConversionNotes = "Exception occured while converting document to pdf";
            }
            finally
            {
                try
                {
                    switch (conversionDirection)
                    {
                        case UseOffice.eDirection.XLSX_to_PDF:
                        case UseOffice.eDirection.XLS_to_PDF:
                            //Release MS Excel from memory
                            useOffice.CloseExcel();
                            break;
                        case UseOffice.eDirection.DOCX_to_PDF:
                        case UseOffice.eDirection.DOC_to_PDF:
                        case UseOffice.eDirection.TEXT_to_PDF:
                        case UseOffice.eDirection.RTF_to_PDF:
                            //Release MS Word from memory
                            useOffice.CloseOffice();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception) { }
            }

            _logger.Info("Exiting from ConvertDocumentToPDF method." + strLoggingInfo);
            return docDetail;
        }

        /// <summary>
        /// Add seperator at the end of document
        /// </summary>
        /// <param name="pdfBytes"></param>
        /// <param name="appDoc"></param>
        /// <returns></returns>
        private ApplicantDoc AddSepearatorToPDF(byte[] pdfBytes, ApplicantDoc appDoc)
        {
            ApplicantDoc docDetail = new ApplicantDoc();
            String pdfDocPathFileName = appDoc.PdfDocPath;
            String returnFilePath = String.Empty;
            Boolean isExceptionOccurred = false;
            int numOfPages = 0;
            try
            {
                //Save file at temporary location from bytes
                String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }
                tempFilePath += "TempFiles\\";
                if (!Directory.Exists(tempFilePath))
                    Directory.CreateDirectory(tempFilePath);

                String destinationFile = tempFilePath + Guid.NewGuid().ToString() + ".pdf";

                // Create a reader for a document
                _logger.Info("Reading PDF...");
                iTextSharp.text.pdf.PdfReader.unethicalreading = true;
                try
                {
                    using (iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(pdfBytes))
                    {
                        // Retrieve the total number of pages
                        numOfPages = reader.NumberOfPages;
                        FileStream pdfOutput = new FileStream(destinationFile, FileMode.Create);
                        var stamper = new iTextSharp.text.pdf.PdfStamper(reader, pdfOutput);
                        iTextSharp.text.pdf.PdfContentByte cb;

                        // Adding content
                        int currentPage = 0;
                        while (currentPage < numOfPages)
                        {
                            currentPage++;
                            cb = stamper.GetOverContent(currentPage);

                            // Append the indicator image at the bottom of the last page of current pdf.
                            if (currentPage == numOfPages)
                            {
                                //float rectangleHeight = (float)(28.3464567 / 2);
                                iTextSharp.text.pdf.PdfImportedPage page = stamper.GetImportedPage(reader, currentPage);
                                //draw line at the end of document
                                cb.Rectangle(0, 0, page.Width, 6f);
                                cb.SetColorFill(iTextSharp.text.BaseColor.BLUE);
                                cb.Fill();
                            }
                        }
                        stamper.Close();
                        pdfOutput.Close();
                    }
                }
                catch (Exception)
                {
                    //UAT-1041 if exception occurred we lost the merged pdf file. 
                    isExceptionOccurred = true;
                }
                byte[] _pdfBytes;
                if (isExceptionOccurred)
                {
                    //UAT-1041 Assign the orignal file data and save that to temporary location.
                    _pdfBytes = pdfBytes;
                }
                else
                {
                    // No exception is occurred. Used new generated pdf.
                    _pdfBytes = GetFileBytes(destinationFile);
                }
                returnFilePath = CommonFileManager.SaveDocument(_pdfBytes, pdfDocPathFileName);
                docDetail.PdfDocPath = returnFilePath;
                docDetail.TotalPages = numOfPages;
                _logger.Info("ConvertImageToPDF: Converted Successfully!");

                try
                {
                    if (File.Exists(destinationFile))
                        File.Delete(destinationFile);
                }
                catch (Exception)
                {

                }
            }
            catch (Exception ex)
            {
                _logger.Error("AddSepearatorToPDF: Error while adding visual indicator at the end of document.", ex);
                throw ex;
            }
            return docDetail;
        }

        /// <summary>
        /// Get File Bytes
        /// </summary>
        /// <param name="filePath">File Path</param>
        /// <returns>Byte array of file</returns>
        private byte[] GetFileBytes(String filePath)
        {
            try
            {
                //ReadAllBytes is implemented in an obvious way. It uses the using-statement on a FileStream. Then it loops through the file and puts the bytes into an array. In .NET 4.0, it throws an exception if the file exceeds 2 gigabytes.
                byte[] fileBytes = File.ReadAllBytes(filePath);
                return fileBytes;
            }
            catch (Exception ex)
            {
                _logger.Error("Error while getting file bytes.", ex);
                return null;
            }
        }

        /// <summary>
        /// To compress or resize image file
        /// </summary>
        /// <param name="path"></param>
        private byte[] ResizedImage(byte[] originalFileBytes, String originalFilePath, String destFolder)
        {
            try
            {
                byte[] compressedImageBytes = null;
                String strMaxResolution = ConfigurationManager.AppSettings["ImageMaxResolution"];
                String strMaxHeight = ConfigurationManager.AppSettings["ImageMaxHeight"];
                String strMaxWidth = ConfigurationManager.AppSettings["ImageMaxWidth"];

                //Get OriginalFileName
                Int32 index = 0;
                String originalFileName = String.Empty;
                if (originalFilePath.Contains(@"\"))
                {
                    index = originalFilePath.LastIndexOf(@"\") + 1;
                    originalFileName = originalFilePath.Substring(index, originalFilePath.Length - index);
                }
                else if (originalFilePath.Contains(@"/"))
                {
                    index = originalFilePath.LastIndexOf(@"/") + 1;
                    originalFileName = originalFilePath.Substring(index, originalFilePath.Length - index);
                }
                String fileExtension = originalFilePath.Substring(originalFilePath.LastIndexOf('.')).ToLower();

                //Save file at temporary location from bytes
                String orgTempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
                if (!orgTempFilePath.EndsWith(@"\"))
                {
                    orgTempFilePath += @"\";
                }
                orgTempFilePath += "TempFiles\\";
                if (!Directory.Exists(orgTempFilePath))
                    Directory.CreateDirectory(orgTempFilePath);
                //Save file at temporary location
                String tempFilename = Path.Combine(orgTempFilePath, Guid.NewGuid().ToString() + fileExtension);
                String compressedImageName = Path.Combine(orgTempFilePath, Guid.NewGuid().ToString() + fileExtension);
                //Writing bytes in destination file using FileStream
                using (FileStream _FileStream = new FileStream(tempFilename,
                                System.IO.FileMode.Create,
                                System.IO.FileAccess.Write))
                {
                    _FileStream.Write(originalFileBytes, 0, originalFileBytes.Length);
                }

                Int32 width = 0;
                Int32 height = 0;

                float maxResolution = 0;
                Int32 maxHeight = 0;
                Int32 maxWidth = 0;

                if (strMaxResolution != null)
                {
                    maxResolution = (float)Convert.ToSingle(strMaxResolution);
                }
                if (strMaxHeight != null)
                {
                    maxHeight = Convert.ToInt32(strMaxHeight);
                }
                if (strMaxWidth != null)
                {
                    maxWidth = Convert.ToInt32(strMaxWidth);
                }

                //Do not compress multi-page TIFF file
                int numberOfImages = 0;
                using (Image imageFile = Image.FromFile(tempFilename, true))
                {
                    //Get the dimension properties of the image
                    FrameDimension frameDimensions = new FrameDimension(imageFile.FrameDimensionsList[0]);

                    //Get the number of images in the tiff file
                    numberOfImages = imageFile.GetFrameCount(frameDimensions);
                }
                if (numberOfImages > 1)
                {
                    try
                    {
                        //Delete Temp File
                        if (File.Exists(tempFilename))
                        {
                            File.Delete(tempFilename);
                        }
                    }
                    catch (Exception) { }

                    return null;
                }

                using (Bitmap imgIn = new Bitmap(tempFilename))
                {
                    double y = imgIn.Height;
                    double x = imgIn.Width;

                    if (y > maxHeight)
                        height = maxHeight;
                    else if (x > maxWidth)
                        width = maxWidth;

                    if (imgIn.HorizontalResolution <= maxResolution && imgIn.VerticalResolution <= maxResolution && height == 0 && width == 0)
                        return null;

                    double factor = 1;
                    if (width > 0)
                    {
                        factor = width / x;
                    }
                    else if (height > 0)
                    {
                        factor = height / y;
                    }
                    //Create new compressed/resized file
                    using (System.IO.FileStream outStream = new System.IO.FileStream(compressedImageName, FileMode.Create, FileAccess.Write))
                    {
                        Bitmap imgOut = new Bitmap((int)(x * factor), (int)(y * factor));

                        // Set DPI of image (xDpi, yDpi)
                        imgOut.SetResolution(maxResolution, maxResolution);

                        Graphics g = Graphics.FromImage(imgOut);
                        g.Clear(Color.White);
                        g.DrawImage(imgIn, new Rectangle(0, 0, (int)(factor * x), (int)(factor * y)),
                          new Rectangle(0, 0, (int)x, (int)y), GraphicsUnit.Pixel);

                        imgOut.Save(outStream, GetImageFormat(fileExtension));
                    }
                }

                try
                {
                    //Delete Temp Files
                    if (File.Exists(tempFilename))
                    {
                        File.Delete(tempFilename);
                    }
                    if (File.Exists(compressedImageName))
                    {
                        //Get bytes of compressed file
                        compressedImageBytes = GetFileBytes(compressedImageName);
                        //Delete Original File and save new compressed file.
                        String destFilePath = destFolder + originalFileName;
                        CommonFileManager.DeleteDocument(originalFilePath);
                        CommonFileManager.SaveDocument(compressedImageBytes, destFilePath);
                        File.Delete(compressedImageName);
                    }
                }
                catch (Exception)
                {
                }

                return compressedImageBytes;
            }
            catch (Exception ex)
            {
                _logger.Error("Error while compressing image file." + ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// To get image format
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private ImageFormat GetImageFormat(String fileExtension)
        {
            switch (fileExtension)
            {
                case ".bmp": return ImageFormat.Bmp;
                case ".gif": return ImageFormat.Gif;
                case ".jpg":
                case ".jpe":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".png": return ImageFormat.Png;
                case ".tiff":
                case ".tif": return ImageFormat.Tiff;
                default: break;
            }
            return ImageFormat.Jpeg;
        }

        #region UAT-1544: WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
        /// <summary>
        ///Convert documents to pdf for Print.
        /// </summary>
        /// <param name="tenantID">InstitutionID</param>
        /// <param name="applicantDocuments">List of Applicant Documents to Print</param>
        /// <returns>PDF file path to print</returns>
        public String ConvertDocumentToPDFForPrint(Int32 tenantID, List<ApplicantDocToBeMerged> applicantDocumentsToPrint)
        {
            String strLoggingInfo = String.Empty;
            Boolean isMerged = false;
            String returnFilePath = String.Empty;
            try
            {
                PdfDocumentOptions pdfDocumentOptions = new PdfDocumentOptions();
                pdfDocumentOptions.PdfCompressionLevel = PDFCompressionLevel.Normal;
                pdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
                pdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait;

                PDFMerge pdfMerge = new PDFMerge(pdfDocumentOptions);
                pdfMerge.LicenseKey = ConfigurationManager.AppSettings["WinnovativePDFMerge"];

                if (applicantDocumentsToPrint != null && applicantDocumentsToPrint.Count > 0)
                {
                    strLoggingInfo = " - Count: " + applicantDocumentsToPrint.Count() + " - First Doc: " + applicantDocumentsToPrint[0].ApplicantDocumentID;
                    _logger.Info("Enetered ConvertDocumentToPDFForPrint method." + strLoggingInfo);

                    foreach (ApplicantDocToBeMerged appDocument in applicantDocumentsToPrint)
                    {
                        try
                        {
                            byte[] fileBytes = CommonFileManager.RetrieveDocument(appDocument.PdfDocPath);
                            if (fileBytes != null && fileBytes.Length > 0)
                            {
                                try
                                {
                                    MemoryStream stream = new MemoryStream(fileBytes);
                                    pdfMerge.AppendPDFStream(stream);

                                    _logger.Info("ConvertDocumentToPDFForPrint: Merged Successfully!");
                                    isMerged = true;
                                }
                                catch (Exception ex)
                                {
                                    _logger.Error("ConvertDocumentToPDFForPrint: Error while merging Applicant Documents." + appDocument.ApplicantDocumentID.ToString() + strLoggingInfo, ex);
                                    continue;
                                }
                            }
                            else
                            {
                                _logger.Error("ConvertDocumentToPDFForPrint: Cannot get file bytes.");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Error("ConvertDocumentToPDFForPrint: Error while merging Applicant Documents." + appDocument.ApplicantDocumentID.ToString() + strLoggingInfo, ex);
                            continue;
                        }
                    }
                    if (isMerged)
                    {
                        //Save file at temporary location from bytes
                        String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
                        if (!tempFilePath.EndsWith(@"\"))
                        {
                            tempFilePath += @"\";
                        }
                        tempFilePath += "TempFiles\\";
                        if (!Directory.Exists(tempFilePath))
                            Directory.CreateDirectory(tempFilePath);
                        //Save file at temporary location
                        String mergedTempFilePath = Path.Combine(tempFilePath, Guid.NewGuid().ToString() + ".pdf");

                        pdfMerge.SaveMergedPDFToFile(mergedTempFilePath);
                        returnFilePath = mergedTempFilePath;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("ConvertDocumentToPDFForPrint: Error while merging Applicant Documents." + strLoggingInfo, ex);
            }
            _logger.Info("Exiting from ConvertDocumentToPDFForPrint method." + strLoggingInfo);
            return returnFilePath;
        }
        #endregion

        #region UAT-1564:delete a document from Unified also when deleting it from manage Document page
        /// <summary>
        ///Delete applicant document from unified pdf.
        /// </summary>
        /// <param name="tenantID">InstitutionID</param>
        /// <param name="applicantDocuments">List of Applicant Documents to Print</param>
        /// <returns>PDF file path to print</returns>
        public String DeleteAppDocumentFromUnifiedPDF(Int32 tenantID, Int32 pagesToAdd, Int32 newStartPageNumber, String previousUnifiedPdfPath, Int32 unifiedDocumentID
                                                      , Boolean isNeedToDeleteUnifiedDocument)
        {
            String strLoggingInfo = String.Empty;
            String destFolder = "Tenant(" + tenantID.ToString() + @")\";
            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
            String pdfDocPathFileName = destFolder + "UD_" + tenantID.ToString() + "_" + unifiedDocumentID + "_" + date + ".pdf";
            String unifiedDocumentPath = String.Empty;
            String prevUnifiedDocTempFilePath = String.Empty;
            String destinationTempFilePath = String.Empty;
            try
            {
                PdfDocumentOptions pdfDocumentOptions = new PdfDocumentOptions();
                pdfDocumentOptions.PdfCompressionLevel = PDFCompressionLevel.Normal;
                pdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
                pdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait;

                PDFMerge pdfMerge = new PDFMerge(pdfDocumentOptions);
                pdfMerge.LicenseKey = ConfigurationManager.AppSettings["WinnovativePDFMerge"];

                //If all documents are deleted from unified PDF then delete the unified document.
                if (!isNeedToDeleteUnifiedDocument)
                {
                    String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
                    if (String.IsNullOrEmpty(tempFilePath))
                    {
                        _logger.Error("DeleteAppDocumentFromUnifiedPDF: Temporary File Location is null or empty.");
                        return String.Empty;
                    }
                    else
                    {
                        if (!tempFilePath.EndsWith(@"\"))
                        {
                            tempFilePath += @"\";
                        }
                        tempFilePath += "TempFiles\\";
                        if (!Directory.Exists(tempFilePath))
                            Directory.CreateDirectory(tempFilePath);
                        //Save file at temporary location
                        prevUnifiedDocTempFilePath = Path.Combine(tempFilePath, Guid.NewGuid().ToString() + ".pdf");
                        destinationTempFilePath = Path.Combine(tempFilePath, Guid.NewGuid().ToString() + ".pdf");
                    }
                }

                if (!String.IsNullOrEmpty(previousUnifiedPdfPath))
                {
                    try
                    {
                        //If all documents are deleted from unified PDF then delete the unified document.
                        if (isNeedToDeleteUnifiedDocument)
                        {
                            CommonFileManager.DeleteDocument(previousUnifiedPdfPath);
                            unifiedDocumentPath = "Document Deleted";
                        }
                        else
                        {
                            byte[] fileBytes = CommonFileManager.RetrieveDocument(previousUnifiedPdfPath);
                            if (fileBytes != null)
                            {
                                MemoryStream stream = new MemoryStream(fileBytes);
                                pdfMerge.AppendPDFStream(stream);
                                pdfMerge.SaveMergedPDFToFile(prevUnifiedDocTempFilePath);
                                pdfMerge.AppendPDFFile(prevUnifiedDocTempFilePath, 0, pagesToAdd);
                                if (newStartPageNumber - 1 > 0)
                                {
                                    pdfMerge.AppendPDFFile(prevUnifiedDocTempFilePath, newStartPageNumber - 1);
                                }
                                pdfMerge.SaveMergedPDFToFile(destinationTempFilePath);
                                unifiedDocumentPath = CommonFileManager.SaveDocument(destinationTempFilePath, pdfDocPathFileName);

                                if (unifiedDocumentPath != null && !String.IsNullOrEmpty(unifiedDocumentPath))
                                {
                                    CommonFileManager.DeleteDocument(previousUnifiedPdfPath);
                                }

                                try
                                {
                                    if (File.Exists(prevUnifiedDocTempFilePath))
                                        File.Delete(prevUnifiedDocTempFilePath);
                                }
                                catch (Exception)
                                {

                                }

                            }
                            else
                            {
                                _logger.Error("DeleteAppDocumentFromUnifiedPDF: Cannot get file bytes of previous unified document.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("DeleteAppDocumentFromUnifiedPDF: Error while merging previous unified document." + strLoggingInfo, ex);
                        try
                        {
                            if (File.Exists(prevUnifiedDocTempFilePath))
                                File.Delete(prevUnifiedDocTempFilePath);
                        }
                        catch (Exception)
                        {

                        }

                    }
                }
                return unifiedDocumentPath;
            }
            catch (Exception ex)
            {
                _logger.Error("DeleteAppDocumentFromUnifiedPDF: Error while deleting Applicant Document." + strLoggingInfo, ex);
            }
            _logger.Info("Exiting from DeleteAppDocumentFromUnifiedPDF method." + strLoggingInfo);
            return unifiedDocumentPath;
        }
        #endregion


        #region UAT-2443:Attestation Merge and multiple share behavior changes

        /// <summary>
        /// Method to merge the attestation documents
        /// </summary>
        /// <param name="tenantID">Selected TenantId</param>
        /// <param name="attestationDocPathToMerge">attestation document path to be merged</param>
        /// <param name="previousAttestationPdfPath">Previous saved attestation document path</param>
        /// <param name="currentUserId">current logged in user id</param>
        /// <param name="fileNameToAppend">file name to append in document file name</param>
        /// <returns></returns>
        public String MergeAttestationDocuments(Int32 tenantID, String attestationDocPathToMerge, String previousAttestationPdfPath, Int32 currentUserId, String fileNameToAppend
                                                , String flName, Boolean deletePreviousAttestation)
        {
            String strLoggingInfo = String.Empty;
            String fileName = flName;
            Int32 totalPageCount = 0;
            Int32 pageCount = 0;
            Boolean isMerged = false;
            String destFolder = "Tenant(" + tenantID.ToString() + @")\";
            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();

            fileName = fileName + fileNameToAppend;
            String pdfDocPathFileName = destFolder + fileName + "_" + tenantID.ToString() + "_" + currentUserId + "_" + date + ".pdf";
            String returnFilePath = String.Empty;
            try
            {
                PdfDocumentOptions pdfDocumentOptions = new PdfDocumentOptions();
                pdfDocumentOptions.PdfCompressionLevel = PDFCompressionLevel.Normal;
                pdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
                pdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait;

                PDFMerge pdfMerge = new PDFMerge(pdfDocumentOptions);
                pdfMerge.LicenseKey = ConfigurationManager.AppSettings["WinnovativePDFMerge"];

                if (attestationDocPathToMerge != null)
                {
                    strLoggingInfo = " ";
                    _logger.Info("Enetered MergeAttestationDocuments method." + strLoggingInfo);

                    if (!String.IsNullOrEmpty(previousAttestationPdfPath))
                    {
                        try
                        {
                            byte[] fileBytes = CommonFileManager.RetrieveDocument(previousAttestationPdfPath);
                            if (fileBytes != null)
                            {
                                MemoryStream stream = new MemoryStream(fileBytes);
                                pdfMerge.AppendPDFStream(stream);
                                totalPageCount = pdfMerge.GetCurrentPageCount();
                                isMerged = true;
                            }
                            else
                            {
                                _logger.Error("MergeAttestationDocuments: Cannot get file bytes of previous attestation document.");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Error("MergeAttestationDocuments: Error while merging previous attestation document." + strLoggingInfo, ex);
                        }
                    }


                    try
                    {

                        byte[] fileBytes = CommonFileManager.RetrieveDocument(attestationDocPathToMerge);
                        if (fileBytes != null && fileBytes.Length > 0)
                        {
                            try
                            {
                                MemoryStream stream = new MemoryStream(fileBytes);
                                pdfMerge.AppendPDFStream(stream);
                                pageCount = pdfMerge.GetCurrentPageCount();
                                _logger.Info("MergeAttestationDocuments: Merged Successfully!");
                                isMerged = true;
                            }
                            catch (Exception ex)
                            {
                                _logger.Error("MergeAttestationDocuments: Error while merging attestation Document." + strLoggingInfo, ex);
                            }
                        }
                        else
                        {
                            _logger.Error("MergeAttestationDocuments: Cannot get file bytes.");
                        }
                    }

                    catch (Exception ex)
                    {
                        _logger.Error("MergeAttestationDocuments: Error while getting attestation document to merge." + strLoggingInfo, ex);
                    }

                    if (isMerged)
                    {
                        //Save file at temporary location from bytes
                        String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
                        if (!tempFilePath.EndsWith(@"\"))
                        {
                            tempFilePath += @"\";
                        }
                        tempFilePath += "TempFiles\\";
                        if (!Directory.Exists(tempFilePath))
                            Directory.CreateDirectory(tempFilePath);
                        //Save file at temporary location
                        String mergedTempFilePath = Path.Combine(tempFilePath, Guid.NewGuid().ToString() + ".pdf");

                        pdfMerge.SaveMergedPDFToFile(mergedTempFilePath);

                        //Delete attestaion document that need to merge.
                        if (attestationDocPathToMerge != null && !String.IsNullOrEmpty(attestationDocPathToMerge) && deletePreviousAttestation)
                        {
                            try
                            {
                                CommonFileManager.DeleteDocument(attestationDocPathToMerge);
                            }
                            catch (Exception ex)
                            {
                                _logger.Error("MergeAttestationDocuments: Error while Deleting attestation Document that need to merge." + strLoggingInfo, ex);
                            }
                        }

                        try
                        {
                            returnFilePath = CommonFileManager.SaveDocument(mergedTempFilePath, pdfDocPathFileName);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error("MergeAttestationDocuments: Error while Saving Document." + strLoggingInfo, ex);
                        }
                        try
                        {
                            if (File.Exists(mergedTempFilePath))
                                File.Delete(mergedTempFilePath);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error("MergeAttestationDocuments: Error while Deleting Temp file." + strLoggingInfo, ex);
                        }

                        //Delete previous attestaion document after merging of attestation in one PDF.
                        if (previousAttestationPdfPath != null && !String.IsNullOrEmpty(previousAttestationPdfPath) && deletePreviousAttestation)
                        {
                            try
                            {
                                CommonFileManager.DeleteDocument(previousAttestationPdfPath);
                            }
                            catch (Exception ex)
                            {
                                _logger.Error("MergeAttestationDocuments: Error while Deleting previous attestation Document." + strLoggingInfo, ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("MergeAttestationDocuments: Error while merging attestation Documents." + strLoggingInfo, ex);
            }
            return returnFilePath;
        }
        #endregion

        #region UAT-3238
        public Tuple<String, List<ApplicantDocToBeMerged>> AppendConvertedUnifiedDocumentToPDF(List<ApplicantDocToBeMerged> applicantDocuments)
        {
            String strLoggingInfo = String.Empty;
            Int32 totalPageCount = 0;
            Int32 pageCount = 0;
            //    List<ApplicantDocMerging> mergedApplicantDocuments = new List<ApplicantDocMerging>();
            Boolean isMerged = false;
            String destFolder = "TempFiles" + @"\";
            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
            String pdfDocPathFileName = destFolder + "UD_" + Guid.NewGuid().ToString().Replace('-', '_') + "_" + date + ".pdf";
            String returnFilePath = String.Empty;
            try
            {
                PdfDocumentOptions pdfDocumentOptions = new PdfDocumentOptions();
                pdfDocumentOptions.PdfCompressionLevel = PDFCompressionLevel.Normal;
                pdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
                pdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait;

                PDFMerge pdfMerge = new PDFMerge(pdfDocumentOptions);
                pdfMerge.LicenseKey = ConfigurationManager.AppSettings["WinnovativePDFMerge"];

                if (applicantDocuments != null && applicantDocuments.Count > 0)
                {
                    foreach (ApplicantDocToBeMerged appDocument in applicantDocuments)
                    {
                        try
                        {
                            byte[] fileBytes = CommonFileManager.RetrieveDocument(appDocument.PdfDocPath);
                            if (fileBytes != null && fileBytes.Length > 0)
                            {
                                try
                                {
                                    MemoryStream stream = new MemoryStream(fileBytes);
                                    pdfMerge.AppendPDFStream(stream);
                                    pageCount = pdfMerge.GetCurrentPageCount();
                                    totalPageCount = pageCount;
                                    appDocument.TotalPages = pageCount;
                                    _logger.Info("AppendConvertedDocumentToPDF: Merged Successfully!");
                                    isMerged = true;
                                }
                                catch (Exception ex)
                                {
                                    _logger.Error("AppendConvertedDocumentToPDF: Error while merging Applicant Documents." + appDocument.ApplicantDocumentID.ToString() + strLoggingInfo, ex);
                                    continue;
                                }
                            }
                            else
                            {
                                _logger.Error("AppendConvertedDocumentToPDF: Cannot get file bytes.");
                            }

                        }
                        catch (Exception ex)
                        {
                            _logger.Error("AppendConvertedDocumentToPDF: Error while merging Applicant Documents." + appDocument.ApplicantDocumentID.ToString() + strLoggingInfo, ex);
                            continue;
                        }
                    }
                    if (isMerged)
                    {
                        //Save file at temporary location from bytes
                        String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
                        if (!tempFilePath.EndsWith(@"\"))
                        {
                            tempFilePath += @"\";
                        }
                        tempFilePath += "TempFiles\\";
                        if (!Directory.Exists(tempFilePath))
                            Directory.CreateDirectory(tempFilePath);
                        //Save file at temporary location
                        String mergedTempFilePath = Path.Combine(tempFilePath, Guid.NewGuid().ToString() + ".pdf");

                        pdfMerge.SaveMergedPDFToFile(mergedTempFilePath);

                        //returnFilePath = CommonFileManager.SaveDocument(mergedTempFilePath, pdfDocPathFileName);
                        returnFilePath = mergedTempFilePath;
                        //try
                        //{
                        //    if (File.Exists(mergedTempFilePath))
                        //        File.Delete(mergedTempFilePath);
                        //}
                        //catch (Exception)
                        //{

                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("AppendConvertedDocumentToPDF: Error while merging Applicant Documents." + strLoggingInfo, ex);
            }
            _logger.Info("Exiting from AppendConvertedDocumentToPDF method." + strLoggingInfo);
            return new Tuple<String, List<ApplicantDocToBeMerged>>(returnFilePath, applicantDocuments);
        }

        /// <summary>
        /// To Get Pdf Page number
        /// </summary>
        /// <param name="applicantDocToBeMergedList"></param>
        /// <returns></returns>
        public List<ApplicantDocToBeMerged> GetPdfPageDetails(List<ApplicantDocToBeMerged> applicantDocToBeMergedList)
        {
            foreach (var item in applicantDocToBeMergedList)
            {
                byte[] fileBytes = CommonFileManager.RetrieveDocument(item.PdfDocPath);
                if (fileBytes != null && fileBytes.Length > 0)
                {
                    try
                    {
                        MemoryStream stream = new MemoryStream(fileBytes);
                        using (iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(fileBytes.ToArray()))
                        {
                            item.TotalPages = reader.NumberOfPages;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("AppendConvertedDocumentToPDF: Error while merging Applicant Documents." + item.ApplicantDocumentID.ToString(), ex);
                        continue;
                    }
                }
            }
            return applicantDocToBeMergedList;
        }
        #endregion
    }
}
