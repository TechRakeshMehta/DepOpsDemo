using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using INTSOF.IMAGE.MANAGER;
using Affirma.ThreeSharp;
using Affirma.ThreeSharp.Query;
using Affirma.ThreeSharp.Model;

namespace AMS.DocumentWeb
{
    /// <summary>
    /// Summary description for ImageFileHandler
    /// </summary>
    public class ImageFileHandler : IHttpHandler
    {

        private HttpContext _context;
        private HttpContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        private const String _fileManagerModeDB = "DB";
        private const String _fileManagerModeS3 = "S3";
        private String aWSAccessKey;
        private String aWSSecretKey;
        private String bucketName;
        private String _fileManagerMode;

        private DBDataServer dataServer;
        private DBDataServer DataServer
        {
            get
            {
                if (dataServer == null)
                {
                    dataServer = new DBDataServer(ConfigurationManager.ConnectionStrings["ConnectionName"].ConnectionString);
                }
                return dataServer;
            }
        }

        public void ProcessRequest(HttpContext context)
        {

            if (context.Request.QueryString["path"] == null)
            {
                return;
            }

            Context = context;

            //Find the fileManagerMode [DB/S3
            this._fileManagerMode = ConfigurationManager.AppSettings["FileManagerMode"];
            if (String.IsNullOrEmpty(_fileManagerMode))
            {
                return;
            }
            if (_fileManagerMode == _fileManagerModeDB)
            {
                String path = Context.Server.UrlDecode(Context.Request.QueryString["path"]);
                var item = DataServer.GetItem(path);
                if (item == null) return;
                WriteFile((byte[])item["Content"], item["Name"].ToString(), item["MimeType"].ToString(), context.Response);

            }
            else if (_fileManagerMode == _fileManagerModeS3)
            {
                String pathToFile = HttpUtility.UrlDecode(Context.Request.QueryString["path"]);
                aWSAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
                aWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
                bucketName = ConfigurationManager.AppSettings["AWSBucket"];
                this.WriteFile(SpecialUrlCharsHelperr.UrlEncode(pathToFile));
            }

        }

        private void WriteFile(byte[] content, string fileName, string contentType, HttpResponse response)
        {
            response.Buffer = false;
            response.Clear();
            response.ContentType = contentType;
            string extension = System.IO.Path.GetExtension(fileName).ToLower();
            if (extension != ".htm" && extension != ".html" && extension != ".xml" && extension != ".jpg" && extension != ".gif" && extension != ".png")
            {
                response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            }
            response.BinaryWrite(content);
            response.Flush();
            response.End();
        }


        private void WriteFile(string pathToFile)
        {
            using (ObjectGetRequest getRequest = new ObjectGetRequest(bucketName, pathToFile))
            {
                using (ObjectGetResponse getResponse = S3Service.ObjectGet(getRequest))
                {
                    Context.Response.BinaryWrite(getResponse.StreamResponseToBytes());
                    Context.Response.Flush();
                    Context.Response.End();
                }
            }
        }

        private IThreeSharp S3Service
        {
            get
            {
                ThreeSharpConfig config = new ThreeSharpConfig();
                config.AwsAccessKeyID = aWSAccessKey;
                config.AwsSecretAccessKey = aWSSecretKey;
                IThreeSharp service = new ThreeSharpQuery(config);
                return service;
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}