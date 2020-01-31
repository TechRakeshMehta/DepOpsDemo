using ClientServiceLibrary.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web;
namespace ClientServiceLibrary
{
    public class TyphonAuthentication
    {

        public ThirdPartyDataUploadResponse UploadData(object[] requestParameters)
        {
            String AuthRequestUrl = requestParameters[1].ToString();
            String DataToUpload = requestParameters[4].ToString();
            Boolean IsLocalhost = Convert.ToBoolean(requestParameters[5]);

            String provider = String.Empty;
            String statusdate = String.Empty;
            String studentemail = String.Empty;
            String programcode = String.Empty;
            String categorycode = String.Empty;
            String status = String.Empty;
            String expdate = String.Empty;
            String TPCDUId = String.Empty;
            String responseMessage = String.Empty;
            String requestXml = String.Empty;
            String ComplianceCategoryID = String.Empty;
            String PackageSubscriptionId = String.Empty;
            String ReturnParmDataXML = String.Empty;
            String ParentTPCDUIdID = String.Empty;

            List<ThirdPartyDataUploadBatchResponse> listResponse = new List<ThirdPartyDataUploadBatchResponse>();
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(DataToUpload);

                XmlElement xelRoot = xmlDoc.DocumentElement;
                XmlNodeList xnlNodes = xelRoot.SelectNodes("/root/batchRequests");

                foreach (XmlNode xndNode in xnlNodes)
                {
                    try
                    {
                        responseMessage = String.Empty;
                        response = new HttpResponseMessage();
                        String updateResponse = String.Empty;
                        String url = String.Empty;
                        String formattedUrl = String.Empty;

                        provider = xndNode["provider"].InnerText;
                        statusdate = !String.IsNullOrEmpty(xndNode["statusdate"].InnerText) ? WebUtility.UrlEncode(xndNode["statusdate"].InnerText) : String.Empty;
                        studentemail = !String.IsNullOrEmpty(xndNode["studentemail"].InnerText) ? WebUtility.UrlEncode(xndNode["studentemail"].InnerText) : String.Empty;
                        programcode = xndNode["programcode"].InnerText;
                        categorycode = xndNode["categorycode"].InnerText;
                        status = !String.IsNullOrEmpty(xndNode["status"].InnerText) ? System.Uri.EscapeDataString(xndNode["status"].InnerText) : String.Empty;
                        expdate = !String.IsNullOrEmpty(xndNode["expdate"].InnerText) ? WebUtility.UrlEncode(xndNode["expdate"].InnerText) : String.Empty;
                        TPCDUId = xndNode["TPCDUId"].InnerText;
                        ComplianceCategoryID = xndNode["ComplianceCategoryID"].InnerText;
                        PackageSubscriptionId = xndNode["PackageSubscriptionId"].InnerText;

                        ReturnParmDataXML = "<Data><TPCDUId>" + TPCDUId + "</TPCDUId><ComplianceCategoryID>" + ComplianceCategoryID + "</ComplianceCategoryID><PackageSubscriptionId>" + PackageSubscriptionId + "</PackageSubscriptionId></Data>";

                        requestXml = "<root><batchRequests><provider> " + provider + "</provider><statusdate>" + xndNode["statusdate"].InnerText + "</statusdate><studentemail>" + xndNode["studentemail"].InnerText + "</studentemail><programcode> " + programcode + " </programcode><categorycode> " + categorycode + " </categorycode><status> " + xndNode["status"].InnerText + " </status><expdate>" + xndNode["expdate"].InnerText + "</expdate></batchRequests></root>";

                        if (!String.IsNullOrEmpty(expdate))
                        {
                            url = AuthRequestUrl + "?provider={0}&status_date={1}&student_email={2}&program_code={3}&category_code={4}&status={5}&expdate={6}";
                            formattedUrl = String.Format(url, provider, statusdate, studentemail, programcode, categorycode, status, expdate);
                        }
                        else
                        {
                            url = AuthRequestUrl + "?provider={0}&status_date={1}&student_email={2}&program_code={3}&category_code={4}&status={5}";
                            formattedUrl = String.Format(url, provider, statusdate, studentemail, programcode, categorycode, status);
                        }

                        if (IsLocalhost)
                        {
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        }

                        HttpWebRequest updateWebRequest = (HttpWebRequest)WebRequest.Create(formattedUrl);

                        using (WebResponse updateWebResponse = updateWebRequest.GetResponse())
                        {
                            var _streamReader = new StreamReader(updateWebResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                            updateResponse = _streamReader.ReadToEnd();
                            _streamReader.Close();
                            response.StatusCode = HttpStatusCode.OK;
                            response.Content = new StringContent(updateResponse, Encoding.UTF8, "application/xml");
                            listResponse.Add(new ThirdPartyDataUploadBatchResponse() { TPDUId = TPCDUId, Response = response, DataXml = ReturnParmDataXML });
                        }
                    }

                    catch (Exception ex)
                    {
                        responseMessage = String.Empty;
                        response = new HttpResponseMessage();
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/xml");
                        listResponse.Add(new ThirdPartyDataUploadBatchResponse() { TPDUId = TPCDUId, Response = response, DataXml = ReturnParmDataXML });
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage = String.Empty;
                response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/xml");
                listResponse.Add(new ThirdPartyDataUploadBatchResponse() { Response = response, DataXml = requestXml });
            }

            ThirdPartyDataUploadResponse thirdPartyDataUploadBatchResponse = GenerateOutputXml(listResponse);
            return thirdPartyDataUploadBatchResponse;
        }


        private ThirdPartyDataUploadResponse GenerateOutputXml(List<ThirdPartyDataUploadBatchResponse> listResponse)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlElement root = doc.CreateElement("ExternalResponse");
            String PackageSubscriptionId = String.Empty;
            String ComplianceCategoryID = String.Empty;
            String TPCDUId = String.Empty;
            List<Tuple<String, String>> list = new List<Tuple<String, String>>();
            ThirdPartyDataUploadResponse returnResponse = new ThirdPartyDataUploadResponse();
            returnResponse.ThirdPartyBatchResponse = listResponse;
            Boolean IgnoreUploadedEntityChildNode = false;
            if (listResponse != null && listResponse.Count > 0)
            {
                foreach (ThirdPartyDataUploadBatchResponse item in listResponse.ToList())
                {
                    if (!String.IsNullOrEmpty(item.DataXml))
                    {
                        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                        xmlDoc.LoadXml(item.DataXml);

                        PackageSubscriptionId = xmlDoc.DocumentElement.SelectSingleNode("/Data/PackageSubscriptionId").InnerText;
                        ComplianceCategoryID = xmlDoc.DocumentElement.SelectSingleNode("/Data/ComplianceCategoryID").InnerText;
                        TPCDUId = xmlDoc.DocumentElement.SelectSingleNode("/Data/TPCDUId").InnerText;

                        System.Xml.XmlElement uploadedEntity = doc.CreateElement("UploadedEntity");
                        uploadedEntity.SetAttribute("PackageSubscriptionID", PackageSubscriptionId);

                        Boolean IsGroupingRequired = listResponse.Where(x => x.TPDUId == item.TPDUId).ToList().Count > 1 ? true : false;

                        if (IsGroupingRequired)
                        {
                            List<ThirdPartyDataUploadBatchResponse> lstResponsedata = listResponse.Where(x => x.TPDUId == item.TPDUId).ToList();
                            if (doc != null && doc.DocumentElement != null)
                            {
                                XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/ExternalResponse/UploadedEntity/DetailEnity");
                                if (nodeList != null && nodeList.Count > 0)
                                {
                                    foreach (XmlNode nodeData in nodeList)
                                    {
                                        if (nodeData.Attributes["TPCDUId"].Value == item.TPDUId)
                                        {
                                            list.Add(new Tuple<String, String>(nodeData.Attributes["ComplianceCategoryID"].Value, item.TPDUId));
                                        }
                                    }
                                }
                            }

                            foreach (ThirdPartyDataUploadBatchResponse item1 in lstResponsedata)
                            {
                                System.Xml.XmlDocument xmlDocData = new System.Xml.XmlDocument();
                                xmlDocData.LoadXml(item1.DataXml);
                                ComplianceCategoryID = xmlDocData.DocumentElement.SelectSingleNode("/Data/ComplianceCategoryID").InnerText;
                                TPCDUId = xmlDocData.DocumentElement.SelectSingleNode("/Data/TPCDUId").InnerText;

                                if (list.Count == 0 || (list.Count > 0 && !list.Any(x => x.Item1 == ComplianceCategoryID && x.Item2 == TPCDUId)))
                                {
                                    System.Xml.XmlElement detailEnity = doc.CreateElement("DetailEnity");
                                    detailEnity.SetAttribute("ComplianceCategoryID", ComplianceCategoryID);
                                    detailEnity.SetAttribute("TPCDUId", TPCDUId);
                                    System.Xml.XmlElement response = doc.CreateElement("Response");
                                    response.InnerText = item1.Response.Content.ReadAsStringAsync().Result;
                                    detailEnity.AppendChild(response);
                                    uploadedEntity.AppendChild(detailEnity);
                                    IgnoreUploadedEntityChildNode = false;
                                }
                                else
                                {
                                    IgnoreUploadedEntityChildNode = true;
                                }
                            }

                            if (!IgnoreUploadedEntityChildNode)
                            {
                                root.AppendChild(uploadedEntity);
                                doc.AppendChild(root);
                            }
                        }
                        else
                        {
                            System.Xml.XmlElement detailEnity = doc.CreateElement("DetailEnity");
                            detailEnity.SetAttribute("ComplianceCategoryID", ComplianceCategoryID);
                            detailEnity.SetAttribute("TPCDUId", TPCDUId);
                            System.Xml.XmlElement Response = doc.CreateElement("Response");
                            Response.InnerText = item.Response.Content.ReadAsStringAsync().Result;
                            detailEnity.AppendChild(Response);
                            uploadedEntity.AppendChild(detailEnity);
                            root.AppendChild(uploadedEntity);
                            doc.AppendChild(root);
                        }
                    }
                    else
                    {
                        System.Xml.XmlElement uploadedEntity = doc.CreateElement("UploadedEntity");
                        System.Xml.XmlElement detailEnity = doc.CreateElement("DetailEnity");
                        System.Xml.XmlElement response = doc.CreateElement("Response");
                        response.InnerText = item.Response.Content.ReadAsStringAsync().Result;
                        detailEnity.AppendChild(response);
                        uploadedEntity.AppendChild(detailEnity);
                        root.AppendChild(uploadedEntity);
                        doc.AppendChild(root);
                    }
                }
                returnResponse.ReposnseXML = doc.InnerXml;
            }
            return returnResponse;
        }
    }
}
