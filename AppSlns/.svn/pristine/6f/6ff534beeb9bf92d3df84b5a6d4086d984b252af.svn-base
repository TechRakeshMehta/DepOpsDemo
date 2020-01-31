#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:    SysXDocumentUpload.cs
// Purpose:     Provide Common feature to upload document.
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web;
using System.IO;
using System.Web.Configuration;

#endregion

#region Application Specific

using INTSOF.Utils;
using INTSOF.Utils.Consts;
using INTERSOFT.WEB.UI.WebControls;
#endregion

#endregion

namespace CoreWeb
{
    /// <summary>
    /// Summary description for SysXDocumentUpload
    /// </summary>
    public class SysXDocumentUpload
    {
        public SysXDocumentUpload()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Save Document
        /// </summary>
        /// <param name="documentFile"></param>
        /// <returns></returns>
        public String SaveDocument(WclUpload documentFile)
        {
            String fileName = String.Empty;
            if (documentFile.UploadedFiles.Count > Convert.ToInt32(DefaultNumbers.None))
            {
                Int32 lastIndex = documentFile.UploadedFiles[Convert.ToInt32(DefaultNumbers.None)].FileName.LastIndexOf("\\");
                fileName = documentFile.UploadedFiles[Convert.ToInt32(DefaultNumbers.None)].FileName.Substring(lastIndex + Convert.ToInt32(DefaultNumbers.One), documentFile.UploadedFiles[Convert.ToInt32(DefaultNumbers.None)].FileName.Length - (lastIndex + Convert.ToInt32(DefaultNumbers.One)));
                if (fileName.Length <= Convert.ToInt32(DefaultNumbers.Fifty) && !WebConfigurationManager.AppSettings["BatchFileLocation"].IsNull())
                {
                    //String filePath = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["BatchFileLocation"]);
                    String filePath = WebConfigurationManager.AppSettings["BatchFileLocation"];
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    String guid = Guid.NewGuid().ToString();
                    documentFile.UploadedFiles[Convert.ToInt32(DefaultNumbers.None)].SaveAs(filePath + "/" + guid + "__" + fileName);
                    fileName = WebConfigurationManager.AppSettings["BatchFileLocation"] + "/" + guid + "__" + fileName;
                }
                else
                {
                    fileName = AppConsts.INVALID_FILE_NAME_LENGTH;
                }
            }
            return fileName;
        }

        /// <summary>
        /// Save Document
        /// </summary>
        /// <param name="documentFile">File to be uplaoded</param>
        /// <returns>File Name</returns>
        public String SaveDocument(WclUpload documentFile, String filePath)
        {
            String fileName = String.Empty;
            String fileExtension = String.Empty;

            if (documentFile.UploadedFiles.Count > Convert.ToInt32(DefaultNumbers.None))
            {
                Int32 lastIndex = documentFile.UploadedFiles[Convert.ToInt32(DefaultNumbers.None)].FileName.LastIndexOf(@"\");
                fileName = documentFile.UploadedFiles[Convert.ToInt32(DefaultNumbers.None)].FileName.Substring(lastIndex + Convert.ToInt32(DefaultNumbers.One), documentFile.UploadedFiles[Convert.ToInt32(DefaultNumbers.None)].FileName.Length - (lastIndex + Convert.ToInt32(DefaultNumbers.One)));

                fileExtension = documentFile.UploadedFiles[Convert.ToInt32(DefaultNumbers.None)].GetExtension();

                if (fileName.Length <= Convert.ToInt32(DefaultNumbers.Fifty) && !WebConfigurationManager.AppSettings["BatchFileLocation"].IsNull())
                {
                    fileName = Guid.NewGuid().ToString() + fileExtension;
                    documentFile.UploadedFiles[Convert.ToInt32(DefaultNumbers.None)].SaveAs(HttpContext.Current.Server.MapPath(filePath) + @"\" + fileName);
                }
                else
                {
                    fileName = AppConsts.INVALID_FILE_NAME_LENGTH;
                }
            }
            return fileName;
        }
    }
}

