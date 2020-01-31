using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using System.Threading;
using System.Configuration;
using System.IO;

public partial class Messaging_Pages_AttachmentDownload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String fileName = Request.QueryString[AppConsts.FILE_NAME_QUERY_STRING];
        String originalFileName = Request.QueryString[AppConsts.ORIGINAL_FILE_NAME_QUERY_STRING];

        System.IO.Stream stream = null;
        try
        {
            Boolean aWSUseS3 = false;
            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
            {
                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
            }
            if (aWSUseS3 == false)
            {
                stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                long bytesToRead = stream.Length;

                Response.ContentType = GetContentType(originalFileName.Substring(originalFileName.LastIndexOf(".")));
                Response.AddHeader("Content-Disposition", "attachment; filename=" + originalFileName.Replace(" ", "_"));

                while (bytesToRead > 0)
                {
                    if (Response.IsClientConnected)
                    {
                        byte[] buffer = new Byte[10000];
                        int length = stream.Read(buffer, 0, 10000);
                        Response.OutputStream.Write(buffer, 0, length);
                        Response.Flush();

                        bytesToRead = bytesToRead - length;
                    }
                    else
                    {
                        bytesToRead = -1;
                    }
                }
            }
            else
            {
                InitializeS3Documents(fileName, originalFileName);
            }
        }
        //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
        //catch (ThreadAbortException thex)
        //{
        //    //You can ignore this 
        //}
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        finally
        {
            if (stream != null)
            {
                
                stream.Close();
                Response.End();
            }
        }
    }

    private void InitializeS3Documents(String documentPath, String fileName)
    {
        Response.Clear();
        Response.Buffer = true;
        try
        {
            AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
            byte[] documentContent = objAmazonS3Documents.RetrieveDocument(documentPath);
            if (!documentContent.IsNullOrEmpty())
            {
                String extension = Path.GetExtension(fileName);
                Response.ClearHeaders();
                Response.Clear();
                Response.AddHeader("Content-Length", documentContent.Length.ToString());
                Response.ContentType = GetContentType(extension);
                //Add Content-Disposition HTTP header
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName.Replace(" ", "_"));
                if (Response.IsClientConnected)
                {
                    // Send the data to the browser
                    Response.BinaryWrite(documentContent);
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }

        finally
        {
            Response.Flush();
            Response.End();
        }
    }

    /// <summary>
    /// Get content type based on the file type.
    /// </summary>
    /// <param name="fileExtension">Extension of the file to download.</param>
    /// <returns>Content type for the file.</returns>
    private String GetContentType(String fileExtension)
    {
        switch (fileExtension)
        {
            case ".txt":
                return "text/plain";
            case ".doc":
            case ".docx":
                return "application/ms-word";
            case ".tiff":
            case ".tif":
                return "image/tiff";
            case ".zip":
                return "application/zip";
            case ".xls":
            case ".xlsx":
            case ".csv":
                return "application/vnd.ms-excel";
            case ".gif":
                return "image/gif";
            case ".jpg":
            case "jpeg":
                return "image/jpeg";
            case ".bmp":
                return "image/bmp";
            case ".pdf":
                return "application/pdf";
            case ".ppt":
                return "application/mspowerpoint";
            default:
                return "application/octet-stream";
        }
    }
}