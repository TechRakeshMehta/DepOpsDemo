using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.Templates;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Threading;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;

namespace Business.RepoManagers
{
    public static class StoredProcedureManagers
    {
        #region Stored Procedures - Verification Details Screen

        #region Public Methods

        /// <summary>
        /// Get Document Mappings for all the item types i.e. Exception and Normal Items
        /// </summary>
        /// <param name="applicantComplianceAttributeIdsXML"></param>
        /// <param name="applicantComplianceItemIdsXML"></param>
        /// <returns></returns>
        public static List<ApplicantDocumentMappingData> GetDocumentMappings(String applicantComplianceAttributeIdsXML, String applicantComplianceItemIdsXML, Int32 tenantId)
        {
            try
            {
                DataSet _ds = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetDocumentMappings(applicantComplianceAttributeIdsXML, applicantComplianceItemIdsXML);
                DataTable _dt = _ds.Tables[0];
                List<ApplicantDocumentMappingData> _lstData = new List<ApplicantDocumentMappingData>();

                if (_ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        _lstData.Add(new ApplicantDocumentMappingData
                        {
                            ExceptionDocumentMappingId = String.IsNullOrEmpty(Convert.ToString(_dt.Rows[i]["ExceptionDocumentMappingId"]))
                                                         ? AppConsts.NONE
                                                         : Convert.ToInt32(_dt.Rows[i]["ExceptionDocumentMappingId"]),

                            ApplicantComplianceAttributeId = String.IsNullOrEmpty(Convert.ToString(_dt.Rows[i]["ApplicantComplianceAttributeId"]))
                                                         ? AppConsts.NONE
                                                         : Convert.ToInt32(_dt.Rows[i]["ApplicantComplianceAttributeId"]),

                            ApplicantComplianceDocumentMapId = String.IsNullOrEmpty(Convert.ToString(_dt.Rows[i]["ApplicantComplianceDocumentMapId"]))
                                                                              ? AppConsts.NONE
                                                                              : Convert.ToInt32(_dt.Rows[i]["ApplicantComplianceDocumentMapId"]),

                            ApplicantComplianceItemId = Convert.ToInt32(_dt.Rows[i]["ApplicantComplianceItemId"]),
                            ApplicantDocumentId = Convert.ToInt32(_dt.Rows[i]["ApplicantDocumentId"]),
                            IsExceptionDocument = Convert.ToBoolean(_dt.Rows[i]["IsExceptionDocument"]),

                        });
                    }
                }

                return _lstData;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get Updated Document Mappings for a particular Item
        /// </summary>
        /// <param name="applicantComplianceAttributeIdsXML"></param>
        /// <param name="applicantComplianceItemIdsXML"></param>
        /// <returns></returns>
        public static List<ApplicantComplianceDocumentMap> GetUpdatedDocumentItemMappings(Int32 applicantComplianceItemId, Boolean isExceptionType, Int32 tenantId)
        {
            try
            {
                List<ApplicantComplianceDocumentMap> _lst = new List<ApplicantComplianceDocumentMap>();
                DataSet _ds = GetUpdatedDocumentMappings(applicantComplianceItemId, isExceptionType, tenantId);
                DataTable dt = _ds.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _lst.Add(new ApplicantComplianceDocumentMap
                    {
                        ApplicantDocumentID = Convert.ToInt32(dt.Rows[i]["ApplicantDocumentId"]),
                        ApplicantComplianceDocumentMapID = Convert.ToInt32(dt.Rows[i]["ApplicantComplianceDocumentMapId"]),
                        ApplicantComplianceAttributeID = Convert.ToInt32(dt.Rows[i]["ApplicantComplianceAttributeId"])
                    });
                }
                return _lst;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get Updated Document Mappings for a particular Exception
        /// </summary>
        /// <param name="applicantComplianceAttributeIdsXML"></param>
        /// <param name="applicantComplianceItemIdsXML"></param>
        /// <returns></returns>
        public static List<ExceptionDocumentMapping> GetUpdatedDocumentExceptionMappings(Int32 applicantComplianceItemId, Boolean isExceptionType, Int32 tenantId)
        {
            try
            {
                List<ExceptionDocumentMapping> _lst = new List<ExceptionDocumentMapping>();
                DataSet _ds = GetUpdatedDocumentMappings(applicantComplianceItemId, isExceptionType, tenantId);
                DataTable dt = _ds.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _lst.Add(new ExceptionDocumentMapping
                    {
                        ApplicantDocumentID = Convert.ToInt32(dt.Rows[i]["ApplicantDocumentId"]),
                        ApplicantComplianceItemID = applicantComplianceItemId,
                        ExceptionDocumentMappingID = Convert.ToInt32(dt.Rows[i]["ExceptionDocumentMappingId"])
                    });
                }

                return _lst;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<NodeNotificationSpecificTemplates> GetNodeTemplatesByQuery(String query, Int32 tenantId)
        {
            try
            {
                DataTable _dt = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetNodeTemplatesByQuery(query);

                if (_dt.Rows.Count > 0)
                {
                    IEnumerable<DataRow> rows = _dt.AsEnumerable();
                    return rows.Select(x => new NodeNotificationSpecificTemplates
                    {
                        Id = Convert.ToInt32(x["Id"]),
                        Name = Convert.ToString(x["Name"])
                    }).ToList();
                }
                else
                    return new List<NodeNotificationSpecificTemplates>();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Stored Procedures - Applicant Order Flow

        public static String GetPricingDataInputXML(String inputXML, Int32 tenantId, List<BackgroundOrderData> _lstBkgOrderData,
           List<BackgroundPackagesContract> lstPackages, out Boolean isXMLGenerated)
        {
            return GeneratePricingDataInputXML(inputXML, tenantId, _lstBkgOrderData,
           lstPackages, null, out isXMLGenerated);
        }

        public static String GetPricingDataInputXML(String inputXML, Int32 tenantId, List<BackgroundOrderData> _lstBkgOrderData,
           List<BackgroundPackagesContract> lstPackages, PreviousAddressContract MailingOption, out Boolean isXMLGenerated)
        {
            return GeneratePricingDataInputXML(inputXML, tenantId, _lstBkgOrderData,
           lstPackages, MailingOption, out isXMLGenerated);
        }


        /// Method used to convert the 
        /// 1. OrganizationUserProfile 
        /// 2. List of Aliases 
        /// 3. List of Residential History 
        /// 4. List of BkgPackages 
        /// related data into XML's and then call the Pricing procedure
        /// <param name="inputXML"></param>
        /// <param name="tenantId"></param>
        /// <param name="_lstBkgOrderData"></param>
        /// <param name="lstPackages"></param>
        /// <param name="isXMLGenerated"></param>
        /// <returns></returns>
        private static String GeneratePricingDataInputXML(String inputXML, Int32 tenantId, List<BackgroundOrderData> _lstBkgOrderData,
       List<BackgroundPackagesContract> lstPackages, PreviousAddressContract MailingOption, out Boolean isXMLGenerated)
        {
            try
            {
                isXMLGenerated = false;
                var _repoInstance = BALUtils.GetStoredProceduresRepoInstance(tenantId);
                DataTable _dtPersonalInformation = _repoInstance.ConvertXMLToAttribute(inputXML, "ams.usp_ConvertPersonalInformationToAttribute");
                DataTable _dtPersonalAlias = _repoInstance.ConvertXMLToAttribute(inputXML, "ams.usp_ConvertPersonalAliasToAttribute");
                DataTable _dtResidentialHistory = _repoInstance.ConvertXMLToAttribute(inputXML, "ams.usp_ConvertResidentialHistoryToAttribute");

                XmlDocument _xmlDoc = new XmlDocument();
                XmlElement _rootElement = (XmlElement)_xmlDoc.AppendChild(_xmlDoc.CreateElement("BkgOrderData"));

                XmlElement _bkgPackagesElement = (XmlElement)_rootElement.AppendChild(_xmlDoc.CreateElement("Packages"));
                foreach (var pkg in lstPackages)
                {
                    XmlElement _packageNode = (XmlElement)_bkgPackagesElement.AppendChild(_xmlDoc.CreateElement("package"));
                    _packageNode.AppendChild(_xmlDoc.CreateElement("PackageId")).InnerText = Convert.ToString(pkg.BPAId);
                    _packageNode.AppendChild(_xmlDoc.CreateElement("BkgPkgHierarchyMappingId")).InnerText = Convert.ToString(pkg.BPHMId);
                    //if (pkg.CopiesCount > 1) // To accomodate the note for copies even in case of 1 copy count
                        if (pkg.CopiesCount > 0)
                        {
                            _packageNode.AppendChild(_xmlDoc.CreateElement("CopiesCount")).InnerText = Convert.ToString(pkg.CopiesCount);
                        }
                    if (pkg.ServiceCode != BkgServiceType.SIMPLE.GetStringValue() && !MailingOption.IsNullOrEmpty() && Convert.ToInt32(MailingOption.MailingOptionId) > 0)
                    {
                        _packageNode.AppendChild(_xmlDoc.CreateElement("MailingOptionId")).InnerText = MailingOption.MailingOptionId;
                    }
                    if (pkg.ServiceCode == BkgServiceType.FingerPrint_Card.GetStringValue())
                    {
                        _packageNode.AppendChild(_xmlDoc.CreateElement("ServiceCode")).InnerText = Convert.ToString(pkg.ServiceCode);
                        _packageNode.AppendChild(_xmlDoc.CreateElement("FCAdditionalPrice")).InnerText = Convert.ToString(pkg.FCAdditionalPrice);
                    }
                    if (pkg.ServiceCode == BkgServiceType.Passport_Photo.GetStringValue())
                    {
                        _packageNode.AppendChild(_xmlDoc.CreateElement("ServiceCode")).InnerText = Convert.ToString(pkg.ServiceCode);
                        _packageNode.AppendChild(_xmlDoc.CreateElement("FCAdditionalPrice")).InnerText = Convert.ToString(pkg.FCAdditionalPrice);
                        _packageNode.AppendChild(_xmlDoc.CreateElement("PPCopiesCount")).InnerText = Convert.ToString(pkg.PPCopiesCount);
                        _packageNode.AppendChild(_xmlDoc.CreateElement("PPAdditionalPrice")).InnerText = Convert.ToString(pkg.PPAdditionalPrice);
                    }
                }

                XmlElement _grpListElement = (XmlElement)_rootElement.AppendChild(_xmlDoc.CreateElement("BkgSvcAttributeGroupList"));
                IEnumerable<DataRow> _attRecords = null;

                if (_dtPersonalInformation.Rows.Count > 0)
                {
                    _attRecords = _dtPersonalInformation.AsEnumerable();
                    GenerateAttributeXMLBasicData(_xmlDoc, _grpListElement, _attRecords);
                    isXMLGenerated = true;
                }
                if (_dtPersonalAlias.Rows.Count > 0)
                {
                    _attRecords = _dtPersonalAlias.AsEnumerable();
                    GenerateAttributeXMLBasicData(_xmlDoc, _grpListElement, _attRecords);
                    isXMLGenerated = true;
                }
                if (_dtResidentialHistory.Rows.Count > 0)
                {
                    _attRecords = _dtResidentialHistory.AsEnumerable();
                    GenerateAttributeXMLBasicData(_xmlDoc, _grpListElement, _attRecords);
                    isXMLGenerated = true;
                }
                if (!_lstBkgOrderData.IsNullOrEmpty())
                {
                    GenerateAttributeXMLBkgOrderData(_xmlDoc, _grpListElement, _lstBkgOrderData);
                    isXMLGenerated = true;
                }
                return _xmlDoc.OuterXml;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Execute the Pricing stored procedure
        /// </summary>
        /// <param name="xml"></param>
        public static String GetPricingData(String inputXML, Int32 tenantId)
        {
            try
            {
                String _outputXML = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetPricingData(inputXML);
                return _outputXML;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String GetLastPackageOrderInputXML(List<BackgroundPackagesContract> lstPackages, PreviousAddressContract MailingOption)
        {
            try
            {
                XmlDocument _xmlDoc = new XmlDocument();
                XmlElement _rootElement = (XmlElement)_xmlDoc.AppendChild(_xmlDoc.CreateElement("BkgOrderData"));

                XmlElement _bkgPackagesElement = (XmlElement)_rootElement.AppendChild(_xmlDoc.CreateElement("Packages"));
                foreach (var pkg in lstPackages)
                {
                    XmlElement _packageNode = (XmlElement)_bkgPackagesElement.AppendChild(_xmlDoc.CreateElement("package"));
                    _packageNode.AppendChild(_xmlDoc.CreateElement("PackageId")).InnerText = Convert.ToString(pkg.BPAId);
                    _packageNode.AppendChild(_xmlDoc.CreateElement("BkgPkgHierarchyMappingId")).InnerText = Convert.ToString(pkg.BPHMId);
                    if (pkg.CopiesCount > 0)
                    {
                        _packageNode.AppendChild(_xmlDoc.CreateElement("CopiesCount")).InnerText = Convert.ToString(pkg.CopiesCount);
                    }
                    if (pkg.ServiceCode != BkgServiceType.SIMPLE.GetStringValue() && !MailingOption.IsNullOrEmpty() && Convert.ToInt32(MailingOption.MailingOptionId) > 0)
                    {
                        _packageNode.AppendChild(_xmlDoc.CreateElement("MailingOptionId")).InnerText = MailingOption.MailingOptionId;
                    }
                }
                return _xmlDoc.OuterXml;
            }


            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        /// <summary>
        /// To get the Order Line Items
        /// </summary>
        /// <param name="inputXM"></param>
        /// <returns></returns>

        public static List<OrderLineItem> GetOrderLineItems(Int32 tenantId, String inputXML)
        {
            try
            {
                var _dsOrderLineItems = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetOrderLineItems(inputXML);
                List<OrderLineItem> _orderLineItems = new List<OrderLineItem>();
                if (_dsOrderLineItems.Tables.Count > 0)
                {
                    DataTable _table = _dsOrderLineItems.Tables[0];
                    for (int i = 0; i < _table.Rows.Count; i++)
                    {
                        OrderLineItem _OrderLineItem = new OrderLineItem();
                        _OrderLineItem.OrderName = Convert.ToString(_table.Rows[i]["Order"]);
                        if (_table.Rows[i]["Quantity"] != System.DBNull.Value)
                        {
                            _OrderLineItem.Quantity = Convert.ToInt32(_table.Rows[i]["Quantity"]);
                        }
                        if (_table.Rows[i]["Price"] != System.DBNull.Value)
                        {
                            _OrderLineItem.Price = Convert.ToDecimal(_table.Rows[i]["Price"]);
                        }
                        if (_table.Rows[i]["Amount"] != System.DBNull.Value)
                        {
                            _OrderLineItem.Amount = Convert.ToDecimal(_table.Rows[i]["Amount"]);
                        }
                        if (_table.Rows[i]["PPQuantity"] != System.DBNull.Value)
                        {
                            _OrderLineItem.PPQuantity = Convert.ToInt32(_table.Rows[i]["PPQuantity"]);
                        }
                        if (_table.Rows[i]["FCAdditionalPrice"] != System.DBNull.Value)
                        {
                            _OrderLineItem.FCAdditionalPrice = Convert.ToDecimal(_table.Rows[i]["FCAdditionalPrice"]);
                        }
                        if (_table.Rows[i]["PPAdditionalPrice"] != System.DBNull.Value)
                        {
                            _OrderLineItem.PPAdditionalPrice = Convert.ToDecimal(_table.Rows[i]["PPAdditionalPrice"]);
                        }
                        if (_table.Rows[i]["ServiceCode"] != System.DBNull.Value)
                        {
                            _OrderLineItem.ServiceCode = Convert.ToString(_table.Rows[i]["ServiceCode"]);
                        }
                        _orderLineItems.Add(_OrderLineItem);
                    }
                }
                return _orderLineItems;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<OrderLineItem> GetSavedOrderLineItems(Int32 tenantId, Int32 OrderId)
        {
            try
            {
                var _dsOrderLineItems = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetSavedOrderLineItems(OrderId);
                List<OrderLineItem> _orderLineItems = new List<OrderLineItem>();
                if (_dsOrderLineItems.Tables.Count > 0)
                {
                    DataTable _table = _dsOrderLineItems.Tables[0];
                    for (int i = 0; i < _table.Rows.Count; i++)
                    {
                        OrderLineItem _OrderLineItem = new OrderLineItem();
                        _OrderLineItem.OrderName = Convert.ToString(_table.Rows[i]["Order"]);
                        if (_table.Rows[i]["Quantity"] != System.DBNull.Value)
                        {
                            _OrderLineItem.Quantity = Convert.ToInt32(_table.Rows[i]["Quantity"]);
                        }
                        if (_table.Rows[i]["Price"] != System.DBNull.Value)
                        {
                            _OrderLineItem.Price = Convert.ToDecimal(_table.Rows[i]["Price"]);
                        }
                        if (_table.Rows[i]["Amount"] != System.DBNull.Value)
                        {
                            _OrderLineItem.Amount = Convert.ToDecimal(_table.Rows[i]["Amount"]);
                        }
                        _orderLineItems.Add(_OrderLineItem);
                    }
                }
                return _orderLineItems;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }





        /// <summary>
        /// Updates the External Service and Vendor Id for the linte items, after order is successfully placed.
        /// </summary>
        /// <param name="masterOrderId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean UpdateExtServiceVendorforLineItems(Int32 masterOrderId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetStoredProceduresRepoInstance(tenantId).UpdateExtServiceVendorforLineItems(masterOrderId, tenantId);
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //12-June-2014
        public static Dictionary<Int32, String> GetPricingDataDictionary(Int32 tenantId, String inputXML)
        {
            try
            {
                Int32 AttributeGroupMappingID = 0;
                String AliasNames = String.Empty;
                List<String> lstAliasName = new List<String>();
                Dictionary<Int32, String> _attrGrpMappingDictionary = new Dictionary<Int32, String>();
                var _repoInstance = BALUtils.GetStoredProceduresRepoInstance(tenantId);
                DataTable _dtPersonalInformation = _repoInstance.ConvertXMLToAttribute(inputXML, "ams.usp_ConvertPersonalInformationToAttribute");
                DataTable _dtResidentialHistory = _repoInstance.ConvertXMLToAttribute(inputXML, "ams.usp_ConvertResidentialHistoryToAttribute");
                DataTable _dtPersonalAlias = _repoInstance.ConvertXMLToAttribute(inputXML, "ams.usp_ConvertPersonalAliasToAttribute");
                foreach (DataRow _row in _dtPersonalInformation.Rows)
                {
                    _attrGrpMappingDictionary.Add(Convert.ToInt32(_row["AttributeGroupMappingID"]), Convert.ToString(_row["AttributeValue"]));
                }

                foreach (DataRow _row in _dtResidentialHistory.Rows)
                {
                    _attrGrpMappingDictionary.Add(Convert.ToInt32(_row["AttributeGroupMappingID"]), Convert.ToString(_row["AttributeValue"]));
                }
                foreach (DataRow _row in _dtPersonalAlias.Rows)
                {
                    AttributeGroupMappingID = Convert.ToInt32(_row["AttributeGroupMappingID"]);
                    lstAliasName.Add(Convert.ToString(_row["AttributeValue"]));
                }
                if (AttributeGroupMappingID > 0 && lstAliasName.IsNotNull() && lstAliasName.Count > 0)
                {
                    AliasNames = String.Join("," + " ", lstAliasName);
                    _attrGrpMappingDictionary.Add(AttributeGroupMappingID, AliasNames);
                }
                return _attrGrpMappingDictionary;

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get the Payment Options for the Packages, at Package Level 
        /// along with the Node Level(Last selected Node in the hierarchy)
        /// </summary>
        /// <param name="dppId"></param>
        /// <param name="bphmIds"></param>
        /// <param name="dpmId"></param>
        /// <returns></returns>
        public static List<PkgList> GetPaymentOptions(int dppId, String bphmIds, Int32 dpmId, Int32 tenantId)
        {
            return GetPaymentOptions(dppId.ToString(), bphmIds, dpmId, tenantId);
        }
        public static List<PkgList> GetPaymentOptions(string dppIds, String bphmIds, Int32 dpmId, Int32 tenantId)
        {
            try
            {
                var _dsPaymentOptions = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetPaymentOptions(dppIds, bphmIds, dpmId);
                var _uniqueKeyColumnName = "UniqueKey";
                List<PkgList> _lstPkgs = new List<PkgList>();
                DataTable _dtPkgs = _dsPaymentOptions.Tables[0];

                DataTable _dtNodePaymentOptions = _dsPaymentOptions.Tables[1];

                DataView dv = new DataView(_dtPkgs);
                //DataTable _dtPkgIds = dv.ToTable(true, "PkgNodeMappingId");
                DataTable _dtPkgIds = dv.ToTable(true, _uniqueKeyColumnName);

                for (int i = 0; i < _dtPkgIds.Rows.Count; i++)
                {
                    //var _pkgNodeMappingId = Convert.ToInt32(_dtPkgIds.Rows[i]["PkgNodeMappingId"]);
                    var _pkgNodeMappingId = AppConsts.NONE;

                    var _uniqueKey = Convert.ToString(_dtPkgIds.Rows[i][_uniqueKeyColumnName]);

                    var crntPackageData = _dtPkgs.AsEnumerable()
                        //.Where(psId => psId.Field<Int32>("PkgNodeMappingId") == _pkgNodeMappingId)
                        .Where(psId => psId.Field<Guid>(_uniqueKeyColumnName) == Guid.Parse(_uniqueKey))
                      .Select(pkgData => new
                      {
                          pkgName = pkgData.Field<String>("PkgName"),
                          pkgId = pkgData.Field<Int32>("PkgId"),
                          _isPkgLevel = pkgData.Field<Boolean>("IsPkgLevel"),
                          isBkgPkg = pkgData.Field<Boolean>("IsBkgPkg"),
                          _pkgNodeMappingId = pkgData.Field<Int32>("PkgNodeMappingId"),
                          isApprovalReqd = pkgData.Field<Boolean>("IsApprovalReqd")
                      }).First();

                    var _isPkgLevel = crntPackageData._isPkgLevel;

                    PkgList _pkg = new PkgList
                    {
                        PkgId = crntPackageData.pkgId,
                        PkgName = crntPackageData.pkgName,
                        IsPkgLevel = _isPkgLevel,
                        IsBkgPkg = crntPackageData.isBkgPkg,
                        PkgNodeMappingId = _pkgNodeMappingId,
                        IsApprovalRequired = crntPackageData.isApprovalReqd
                    };

                    if (_pkg.lstPaymentOptions.IsNullOrEmpty())
                        _pkg.lstPaymentOptions = new List<PkgPaymentOptions>();

                    if (_isPkgLevel)
                    {
                        //var _lstPOs = _dtPkgs.AsEnumerable().Where(x => x.Field<Int32>("PkgNodeMappingId") == _pkgNodeMappingId)
                        var _lstPOs = _dtPkgs.AsEnumerable().Where(x => x.Field<Guid>(_uniqueKeyColumnName) == Guid.Parse(_uniqueKey))
                                .Select(po => new
                                {
                                    paymentOptnName = po.Field<String>("PaymentOptionName"),
                                    paymentOptnId = po.Field<Int32>("PaymentOptionId"),
                                    paymentOptnCode = po.Field<String>("PaymentOptionCode"),
                                    isApprovalRequired = po.Field<Boolean>("IsApprovalReqd") //Added in UAT-3958
                                }).ToList();

                        foreach (var po in _lstPOs)
                        {
                            _pkg.lstPaymentOptions.Add(new PkgPaymentOptions
                            {
                                PaymentOptionName = po.paymentOptnName,
                                PaymentOptionId = po.paymentOptnId,
                                PaymentOptionCode = po.paymentOptnCode,
                                IsApprovalRequired = po.isApprovalRequired //Added in UAT-3958
                            });
                        }
                    }
                    else
                    {
                        for (int j = 0; j < _dtNodePaymentOptions.Rows.Count; j++)
                        {
                            _pkg.lstPaymentOptions.Add(new PkgPaymentOptions
                            {
                                PaymentOptionId = Convert.ToInt32(_dtNodePaymentOptions.Rows[j]["POId"]),
                                PaymentOptionName = Convert.ToString(_dtNodePaymentOptions.Rows[j]["POName"]),
                                PaymentOptionCode = Convert.ToString(_dtNodePaymentOptions.Rows[j]["POCode"]),
                                IsApprovalRequired = Convert.ToBoolean(_dtNodePaymentOptions.Rows[j]["IsApprovalReqd"]) //Added in UAT-3958
                            });
                        }
                        //UAT 4437
                        if (_pkg.IsNotNull() && _pkg.lstPaymentOptions.Any(x => x.PaymentOptionCode == PaymentOptions.Credit_Card.GetStringValue() && x.IsApprovalRequired == true))
                        {
                            _pkg.IsApprovalRequired = true;
                        }
                    }

                    _lstPkgs.Add(_pkg);
                }
                return _lstPkgs;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }




        #endregion

        #region Private Methods

        /// <summary>
        /// Get Updated Document Mappings for a particular Item, whether Normal or Exception type
        /// </summary>
        /// <param name="applicantComplianceAttributeIdsXML"></param>
        /// <param name="applicantComplianceItemIdsXML"></param>
        /// <returns></returns>
        private static DataSet GetUpdatedDocumentMappings(Int32 applicantComplianceItemId, Boolean isExceptionType, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetStoredProceduresRepoInstance(tenantId).GetUpdatedDocumentMappings(applicantComplianceItemId, isExceptionType);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Generates the Attribute based XML for 
        /// 1. Peronal Information
        /// 2. List of Aliases
        /// 3. List of Residential History addresses
        /// </summary>
        /// <param name="_xmlDoc"></param>
        /// <param name="_rootElement"></param>
        /// <param name="attributeRecords"></param> 
        private static void GenerateAttributeXMLBasicData(XmlDocument _xmlDoc, XmlElement _rootElement, IEnumerable<DataRow> attributeRecords)
        {
            Int32 _attributeGroupId = attributeRecords.Select(col => Convert.ToInt32(col["AttributeGroupID"])).FirstOrDefault();
            attributeRecords.Select(col => col["InstanceID"].ToString()).Distinct().ForEach(instance =>
            {
                XmlNode exp = _rootElement.AppendChild(_xmlDoc.CreateElement("BkgSvcAttributeDataGroup"));
                exp.AppendChild(_xmlDoc.CreateElement("AttributeGroupID")).InnerText = _attributeGroupId.ToString();
                exp.AppendChild(_xmlDoc.CreateElement("InstanceId")).InnerText = instance;
                attributeRecords.Where(cond => cond["InstanceID"].ToString() == instance).ForEach(attribute =>
                {
                    XmlNode expChild = exp.AppendChild(_xmlDoc.CreateElement("BkgSvcAttributeData"));
                    expChild.AppendChild(_xmlDoc.CreateElement("BkgAttributeGroupMappingID")).InnerText = attribute["AttributeGroupMappingID"].ToString();
                    expChild.AppendChild(_xmlDoc.CreateElement("Value")).InnerText = attribute["AttributeValue"].ToString();
                });
            });
        }

        /// <summary>
        /// Generates the Attribute based XML for BkgOrderData 
        /// </summary>
        /// <param name="_xmlDoc"></param>
        /// <param name="_rootElement"></param>
        /// <param name="_lstBkgOrderData"></param> 
        private static void GenerateAttributeXMLBkgOrderData(XmlDocument _xmlDoc, XmlElement _rootElement, List<BackgroundOrderData> _lstBkgOrderData)
        {
            foreach (var _orderData in _lstBkgOrderData)
            {
                Int32 _attributeGroupId = _orderData.BkgSvcAttributeGroupId;

                XmlNode exp = _rootElement.AppendChild(_xmlDoc.CreateElement("BkgSvcAttributeDataGroup"));
                exp.AppendChild(_xmlDoc.CreateElement("AttributeGroupID")).InnerText = _attributeGroupId.ToString();
                exp.AppendChild(_xmlDoc.CreateElement("InstanceId")).InnerText = Convert.ToString(_orderData.InstanceId);

                foreach (var _attrData in _orderData.CustomFormData)
                {
                    XmlNode expChild = exp.AppendChild(_xmlDoc.CreateElement("BkgSvcAttributeData"));
                    expChild.AppendChild(_xmlDoc.CreateElement("BkgAttributeGroupMappingID")).InnerText = Convert.ToString(_attrData.Key);
                    expChild.AppendChild(_xmlDoc.CreateElement("Value")).InnerText = Convert.ToString(_attrData.Value);
                }
            }
        }


        /// <summary>
        /// Get Updated Document Mappings for a particular Item, whether Normal or Exception type
        /// </summary>
        /// <param name="applicantComplianceAttributeIdsXML"></param>
        /// <param name="applicantComplianceItemIdsXML"></param>
        /// <returns></returns>
        private static DataSet GetBackroundOrderSearchDetail(CustomPagingArgsContract gridCustomPaging, BkgOrderSearchContract bkgOrderSearchContract, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetStoredProceduresRepoInstance(tenantId).GetBackroundOrderSearchDetail(gridCustomPaging, bkgOrderSearchContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        #endregion

        #region Public Methods

        #region UAT-1683

        public static String ArchieveBkgOrderIds(List<Int32> selectedOrderIds, Int32 tenantID, Int32 currentuserID)
        {
            short archieveStateID = LookupManager.GetLookUpData<lkpArchiveState>(tenantID).FirstOrDefault(cond => cond.AS_Code == ArchiveState.Archived.GetStringValue()).AS_ID;

            List<Int32> ArchivedandGraduatedStateIDs = LookupManager.GetLookUpData<lkpArchiveState>(tenantID).Where(cond => cond.AS_Code == ArchiveState.Archived.GetStringValue()
             || cond.AS_Code == ArchiveState.Archived_and_Graduated.GetStringValue() || cond.AS_Code == ArchiveState.Graduated.GetStringValue()).Select(x => Convert.ToInt32(x.AS_ID)).ToList();
            Int16 archiveChangeTypeID = ComplianceDataManager.GetComplianceSubsArchiveChangeTypeIdByCode(tenantID, ComplianceSubscriptionArchiveChangeType.SET_TO_ARCHIVE.GetStringValue());
            List<BkgOrderArchiveHistory> lstsubscriptionArchiveHistoryData = new List<BkgOrderArchiveHistory>();
            List<BkgOrder> lstBkgOrderToBeArchieved = BALUtils.GetStoredProceduresRepoInstance(tenantID).GetBkgOrderToBeArchived(selectedOrderIds, ArchivedandGraduatedStateIDs);
            List<Int32> lstOrderID = lstBkgOrderToBeArchieved.Select(x => x.BOR_ID).ToList();

            #region Create Xml for subscriptionChangeDetail due to archive state change.
            foreach (Int32 id in lstOrderID)
            {
                BkgOrder currentBkgOrder = new BkgOrder();
                currentBkgOrder = lstBkgOrderToBeArchieved.Where(x => id == x.BOR_ID).FirstOrDefault();

                String subscriptionChangeDetailXML = "";
                subscriptionChangeDetailXML = "<SubscriptionChangeDetails>";
                subscriptionChangeDetailXML += "<OldArchiveStateID>" + (currentBkgOrder.BOR_ArchiveStateID.IsNull() ? null : currentBkgOrder.BOR_ArchiveStateID.ToString()) + "</OldArchiveStateID>";
                subscriptionChangeDetailXML += "<NewArchiveStateID>" + archieveStateID.ToString() + "</NewArchiveStateID>";
                subscriptionChangeDetailXML += "<OldArchiveDate>" + (currentBkgOrder.BOR_LastArchivedDate.IsNull() ? null : currentBkgOrder.BOR_LastArchivedDate.ToString()) + "</OldArchiveDate>";
                subscriptionChangeDetailXML += "<NewArchiveDate>" + DateTime.Now.ToString() + "</NewArchiveDate>";
                subscriptionChangeDetailXML += "</SubscriptionChangeDetails>";

                BkgOrderArchiveHistory subscriptionArchiveHistoryData = new BkgOrderArchiveHistory();
                subscriptionArchiveHistoryData = GetCompliancePkgSubscriptionArchHistoryObject(archiveChangeTypeID, id, subscriptionChangeDetailXML, currentuserID);
                lstsubscriptionArchiveHistoryData.Add(subscriptionArchiveHistoryData);
            }
            if (lstBkgOrderToBeArchieved.IsNotNull() && lstBkgOrderToBeArchieved.Count > 0)
            {
                return BALUtils.GetStoredProceduresRepoInstance(tenantID).ArchieveBkgOrder(archieveStateID, currentuserID, lstBkgOrderToBeArchieved, lstsubscriptionArchiveHistoryData);
            }
            else
            {
                return "The selected user(s) does not have any active Background Order(s).";
            }
        }

        #region UAT-4085
        public static String UnArchieveBkgOrderIds(List<Int32> selectedOrderIds, Int32 tenantID, Int32 currentuserID)
        {
            short archieveStateID = LookupManager.GetLookUpData<lkpArchiveState>(tenantID).FirstOrDefault(cond => cond.AS_Code == ArchiveState.Active.GetStringValue()).AS_ID;

            List<Int32> UnArchivedStateIDs = LookupManager.GetLookUpData<lkpArchiveState>(tenantID).Where(cond => cond.AS_Code == ArchiveState.Active.GetStringValue()
             ).Select(x => Convert.ToInt32(x.AS_ID)).ToList();
            Int16 archiveChangeTypeID = ComplianceDataManager.GetComplianceSubsArchiveChangeTypeIdByCode(tenantID, ComplianceSubscriptionArchiveChangeType.UN_ARCHIVE_BY_ADMIN.GetStringValue());
            List<BkgOrderArchiveHistory> lstsubscriptionArchiveHistoryData = new List<BkgOrderArchiveHistory>();
            List<BkgOrder> lstBkgOrderToBeUnArchived = BALUtils.GetStoredProceduresRepoInstance(tenantID).GetBkgOrderToBeUnArchived(selectedOrderIds, UnArchivedStateIDs);
            List<Int32> lstOrderID = lstBkgOrderToBeUnArchived.Select(x => x.BOR_ID).ToList();

            //Create Xml for subscriptionChangeDetail due to archive state change.
            foreach (Int32 id in lstOrderID)
            {
                BkgOrder currentBkgOrder = new BkgOrder();
                currentBkgOrder = lstBkgOrderToBeUnArchived.Where(x => id == x.BOR_ID).FirstOrDefault();

                String subscriptionChangeDetailXML = "";
                subscriptionChangeDetailXML = "<SubscriptionChangeDetails>";
                subscriptionChangeDetailXML += "<OldArchiveStateID>" + (currentBkgOrder.BOR_ArchiveStateID.IsNull() ? null : currentBkgOrder.BOR_ArchiveStateID.ToString()) + "</OldArchiveStateID>";
                subscriptionChangeDetailXML += "<NewArchiveStateID>" + archieveStateID.ToString() + "</NewArchiveStateID>";
                subscriptionChangeDetailXML += "<OldArchiveDate>" + (currentBkgOrder.BOR_LastArchivedDate.IsNull() ? null : currentBkgOrder.BOR_LastArchivedDate.ToString()) + "</OldArchiveDate>";
                subscriptionChangeDetailXML += "<NewArchiveDate>" + DateTime.Now.ToString() + "</NewArchiveDate>";
                subscriptionChangeDetailXML += "</SubscriptionChangeDetails>";

                BkgOrderArchiveHistory subscriptionArchiveHistoryData = new BkgOrderArchiveHistory();
                subscriptionArchiveHistoryData = GetCompliancePkgSubscriptionArchHistoryObject(archiveChangeTypeID, id, subscriptionChangeDetailXML, currentuserID);
                lstsubscriptionArchiveHistoryData.Add(subscriptionArchiveHistoryData);
            }
            if (lstBkgOrderToBeUnArchived.IsNotNull() && lstBkgOrderToBeUnArchived.Count > 0)
            {
                return BALUtils.GetStoredProceduresRepoInstance(tenantID).UnArchieveBkgOrder(archieveStateID, currentuserID, lstBkgOrderToBeUnArchived, lstsubscriptionArchiveHistoryData);
            }
            else
            {
                return "The selected user(s) does not have any archived Background Order(s).";
            }
        }
        #endregion

        /// <summary>
        /// Method to return the object with set values of ComplinacePackageSubscriptionArchiveHistory.
        /// </summary>
        /// <param name="archiveChangeTypeID">ChangeTypeID</param>
        /// <param name="packageSubscriptionID">packageSubscriptionID</param>
        /// <param name="subscriptionChangeDetailXML">subscriptionChangeDetailXML</param>
        /// <param name="currentLoggedInUserId">currentLoggedInUserId</param>
        /// <returns></returns>
        public static BkgOrderArchiveHistory GetCompliancePkgSubscriptionArchHistoryObject(Int16 archiveChangeTypeID, Int32 BkgOrderID, String subscriptionChangeDetailXML, Int32 currentLoggedInUserId)
        {
            try
            {
                BkgOrderArchiveHistory subscriptionArchiveHistoryData = new BkgOrderArchiveHistory();
                subscriptionArchiveHistoryData.BOAH_ChangeTypeID = archiveChangeTypeID;
                subscriptionArchiveHistoryData.BOAH_IsActive = true;
                subscriptionArchiveHistoryData.BOAH_IsDeleted = false;
                subscriptionArchiveHistoryData.BOAH_SubscriptionChangeDetail = subscriptionChangeDetailXML;
                subscriptionArchiveHistoryData.BOAH_BkgOrderID = BkgOrderID;
                subscriptionArchiveHistoryData.BOAH_CreatedBy = currentLoggedInUserId;
                subscriptionArchiveHistoryData.BOAH_CreatedOn = DateTime.Now;
                return subscriptionArchiveHistoryData;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #endregion

        #region Stored Procedures - Applicant Order Processing


        public static BackroundOrderSearchContract GetBackroundOrderSearch(CustomPagingArgsContract gridCustomPaging, BkgOrderSearchContract bkgOrderSearchContract, Int32 tenantId)
        {
            try
            {
                BackroundOrderSearchContract backroundOrderSearchContract = new BackroundOrderSearchContract();

                DataSet ds = new DataSet();
                ds = GetBackroundOrderSearchDetail(gridCustomPaging, bkgOrderSearchContract, tenantId);

                //UAT-1456 related changes.
                if (ds.Tables[0].Rows.Count > 0)
                {
                    {
                        gridCustomPaging.CurrentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                        gridCustomPaging.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["VirtualCount"]));
                    }
                }
                DataTable dtOrder = ds.Tables[1];
                DataTable dtServiceGroup = ds.Tables[2];
                // DataTable dtServices = ds.Tables[2];

                for (int i = 0; i < dtOrder.Rows.Count; i++)
                {
                    backroundOrderSearchContract.BackroundOrder.Add(new BackroundOrderContract
                    {
                        OrganizationUserId = dtOrder.Rows[i]["OrganizationUserId"] != DBNull.Value ? Convert.ToInt32(dtOrder.Rows[i]["OrganizationUserId"]) : 0,
                        OrderID = dtOrder.Rows[i]["OrderID"] != DBNull.Value ? Convert.ToInt32(dtOrder.Rows[i]["OrderID"]) : 0,
                        HierarchyNodeID = dtOrder.Rows[i]["HierarchyNodeID"] != DBNull.Value ? Convert.ToInt32(dtOrder.Rows[i]["HierarchyNodeID"]) : 0,
                        HierarchyLabel = dtOrder.Rows[i]["HierarchyLabel"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["HierarchyLabel"]) : null,
                        DPM_IsEmploymentType = dtOrder.Rows[i]["DPM_IsEmployment"] != DBNull.Value ? Convert.ToBoolean(dtOrder.Rows[i]["DPM_IsEmployment"]) : false, //UAT-3429
                        OrderPrice = dtOrder.Rows[i]["OrderPrice"] != DBNull.Value ? Convert.ToDecimal(dtOrder.Rows[i]["OrderPrice"]) : 0,
                        ApplicantFirstName = dtOrder.Rows[i]["ApplicantFirstName"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["ApplicantFirstName"]) : null,
                        ApplicantLastName = dtOrder.Rows[i]["ApplicantLastName"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["ApplicantLastName"]) : null,
                        // SSN = dtOrder.Rows[i]["SSN"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["SSN"]).Length > 6 ? Convert.ToString(dtOrder.Rows[i]["SSN"]).Insert(5, "-").Insert(3, "-") : Convert.ToString(dtOrder.Rows[i]["SSN"]) : null,
                        SSN = dtOrder.Rows[i]["SSN"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["SSN"]) : null,
                        OrderStatus = dtOrder.Rows[i]["OrderStatus"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["OrderStatus"]) : null,
                        PaymentStatus = dtOrder.Rows[i]["PaymentStatus"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["PaymentStatus"]) : null,
                        OrderFlag = dtOrder.Rows[i]["OrderFlag"] != DBNull.Value ? Convert.ToBoolean(dtOrder.Rows[i]["OrderFlag"]) : false,
                        ClearStarStatus = dtOrder.Rows[i]["ClearStarStatus"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["ClearStarStatus"]) : null,
                        OrderClientStatusID = dtOrder.Rows[i]["OrderClientStatusID"] != DBNull.Value ? Convert.ToInt32(dtOrder.Rows[i]["OrderClientStatusID"]) : 0,
                        OrderClientStatusTypeName = dtOrder.Rows[i]["OrderClientStatusTypeName"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["OrderClientStatusTypeName"]) : null,
                        OrderCreatedDate = dtOrder.Rows[i]["OrderCreatedDate"] != DBNull.Value ? Convert.ToDateTime(dtOrder.Rows[i]["OrderCreatedDate"]) : (DateTime?)null,
                        OrderCompletedDate = dtOrder.Rows[i]["OrderCompletedDate"] != DBNull.Value ? Convert.ToDateTime(dtOrder.Rows[i]["OrderCompletedDate"]) : (DateTime?)null,
                        DOB = dtOrder.Rows[i]["DOB"] != DBNull.Value ? Convert.ToDateTime(dtOrder.Rows[i]["DOB"]) : (DateTime?)null,
                        ArchiveStatus = dtOrder.Rows[i]["ArchiveStatus"] != DBNull.Value ? Convert.ToBoolean(dtOrder.Rows[i]["ArchiveStatus"]) : (Boolean?)null,
                        InstitutionStatusColorID = dtOrder.Rows[i]["InstitutionStatusColorID"] != DBNull.Value ? Convert.ToInt32(dtOrder.Rows[i]["InstitutionStatusColorID"]) : 0,
                        BkgOrderStatus = dtOrder.Rows[i]["BkgOrderStatus"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["BkgOrderStatus"]) : null,
                        IsOrderItemsComplete = dtOrder.Rows[i]["IsOrderItemsComplete"] != DBNull.Value ? Convert.ToBoolean(dtOrder.Rows[i]["IsOrderItemsComplete"]) : false,
                        CustomAttributes = dtOrder.Rows[i]["CustomAttributes"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["CustomAttributes"]) : null,
                        TotalCount = Convert.ToInt32(dtOrder.Rows[i]["TotalCount"]),
                        ManualServiceForms = dtOrder.Rows[i]["ManualServiceForms"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["ManualServiceForms"]) : null,
                        UserGroupNames = dtOrder.Rows[i]["UserGroupName"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["UserGroupName"]) : null,
                        OrderNote = dtOrder.Rows[i]["OrderNote"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["OrderNote"]) : null,
                        OrderNumber = dtOrder.Rows[i]["OrderNumber"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["OrderNumber"]) : String.Empty,
                        BkgPackageNames = dtOrder.Rows[i]["BkgPackageNames"] != DBNull.Value ? Convert.ToString(dtOrder.Rows[i]["BkgPackageNames"]) : String.Empty
                    });
                }

                for (int i = 0; i < dtServiceGroup.Rows.Count; i++)
                {
                    backroundOrderSearchContract.BackroundServiceGroup.Add(new BackroundServiceGroupContract
                    {
                        OrderID = Convert.ToInt32(dtServiceGroup.Rows[i]["OrderID"]),
                        OrderPaymentStatus = Convert.ToString(dtServiceGroup.Rows[i]["OrderPaymentStatus"]),
                        OrderStatusType = Convert.ToString(dtServiceGroup.Rows[i]["OrderStatusType"]),
                        ServiceGroupId = Convert.ToInt32(dtServiceGroup.Rows[i]["ServiceGroupId"]),
                        ServicreGroupName = Convert.ToString(dtServiceGroup.Rows[i]["ServicreGroupName"]),
                        BkgOrderPackageSvcGroupID = Convert.ToInt32(dtServiceGroup.Rows[i]["BkgOrderPackageSvcGroupID"]),
                        IsServiceGroupFlagged = Convert.ToBoolean(dtServiceGroup.Rows[i]["IsServiceGroupFlagged"]),
                        IsServiceGroupComplete = Convert.ToBoolean(dtServiceGroup.Rows[i]["IsServiceGroupComplete"]),
                        IsOperationSupportAutoCompleteServiceType = Convert.ToBoolean(dtServiceGroup.Rows[i]["IsOperationSupportAutoCompleteServiceType"]),
                        ServiceCount = Convert.ToInt32(dtServiceGroup.Rows[i]["ServiceCount"]),
                        IsServiceGroupStatusComplete = Convert.ToBoolean(dtServiceGroup.Rows[i]["IsServiceGroupStatusComplete"]),
                        IsAllServiceNonReportable = Convert.ToBoolean(dtServiceGroup.Rows[i]["IsAllServiceNonReportable"]),
                        BkgPackageSvcGroupId = Convert.ToInt32(dtServiceGroup.Rows[i]["BkgPackageSvcGroupId"]),
                    });
                }

                //Fixed UAT 624- Background Order Queue  For Client Admins only display Service Groups, not the services within the group.

                //for (int i = 0; i < dtServices.Rows.Count; i++)
                //{
                //    backroundOrderSearchContract.BackroundServices.Add(new BackroundServicesContract
                //    {

                //        BkgOrderPackageSvcGroupID = Convert.ToInt32(dtServices.Rows[i]["BkgOrderPackageSvcGroupID"]),
                //        ServiceID = Convert.ToInt32(dtServices.Rows[i]["ServiceID"]),
                //        ServicreName = Convert.ToString(dtServices.Rows[i]["ServicreName"]),
                //        ServiceFormStatusTypeID = dtServices.Rows[i]["ServiceFormStatusTypeID"] != DBNull.Value ? Convert.ToInt32(dtServices.Rows[i]["ServiceFormStatusTypeID"]) : 0,
                //        ServiceFlagged = dtServices.Rows[i]["ServiceFlagged"] != DBNull.Value ? Convert.ToBoolean(dtServices.Rows[i]["ServiceFlagged"]) : false,
                //        OrderItemResultStatusID = Convert.ToInt32(dtServices.Rows[i]["OrderItemResultStatusID"]),
                //        LastStatusChangeDate = dtServices.Rows[i]["LastStatusChangeDate"] != DBNull.Value ? Convert.ToString(dtServices.Rows[i]["LastStatusChangeDate"]) : "NA",
                //        ServiceFormStatus = dtServices.Rows[i]["ServiceFormStatus"] != DBNull.Value ? Convert.ToString(dtServices.Rows[i]["ServiceFormStatus"]) : "NA",
                //        ServiceTypeCode = dtServices.Rows[i]["ServiceTypeCode"] != DBNull.Value ? Convert.ToString(dtServices.Rows[i]["ServiceTypeCode"]) : String.Empty
                //    });
                //}
                return backroundOrderSearchContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region Stored Procedures - Supplement Order

        /// <summary>
        /// Get the output XML from the Supplement order stored procedure
        /// </summary>
        /// <param name="inputXML"></param>
        /// <returns></returns>
        public static String GetSupplementOrderPricingData(String inputXML, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetStoredProceduresRepoInstance(tenantId).GetSupplementOrderPricingData(inputXML);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Gets Applicant details, related to the Current Background Order
        /// </summary>
        public static SupplementOrderApplicantDataContract GetApplicantData(Int32 masterOrderId, Int32 tenantId)
        {
            try
            {
                DataTable _dtApplicantDetails = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetApplicantData(masterOrderId);

                // Currently it is configured to get only single record
                if (_dtApplicantDetails.Rows.Count == 1)
                    return new SupplementOrderApplicantDataContract
                    {
                        ApplicantName = Convert.ToString(_dtApplicantDetails.Rows[0]["ApplicantName"]),
                        BkgOrderId = Convert.ToInt32(_dtApplicantDetails.Rows[0]["BkgOrderId"]),
                        BkgOrderStatus = Convert.ToString(_dtApplicantDetails.Rows[0]["BkgOrderStatus"]),
                        InstitutionName = Convert.ToString(_dtApplicantDetails.Rows[0]["InstituteName"])
                    };
                else
                    return new SupplementOrderApplicantDataContract();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        /// <summary>
        /// Gets Applicant Residential Histories & Personal Alias to display in Supplement Order, added during normal order
        /// </summary>
        public static Tuple<List<SupplementOrderApplicantResidentialHistoryContract>, List<SupplementOrderApplicantPersonAliasContract>> GetApplicantBkgOrderDeta(Int32 masterOrderId, Int32 tenantId)
        {
            try
            {
                DataSet _dsApplicantData = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetApplicantBkgOrderDeta(masterOrderId);
                List<SupplementOrderApplicantResidentialHistoryContract> _lstResidentialHistory = new List<SupplementOrderApplicantResidentialHistoryContract>();
                List<SupplementOrderApplicantPersonAliasContract> _lstPersonAlias = new List<SupplementOrderApplicantPersonAliasContract>();

                if (_dsApplicantData.Tables.Count > 0)
                {
                    if (_dsApplicantData.Tables[0].Rows.Count > 0)
                    {
                        DataTable _dt = _dsApplicantData.Tables[0];

                        for (int i = 0; i < _dt.Rows.Count; i++)
                        {
                            _lstResidentialHistory.Add(new SupplementOrderApplicantResidentialHistoryContract
                            {
                                StateName = Convert.ToString(_dt.Rows[i]["StateName"]),
                                CountyName = Convert.ToString(_dt.Rows[i]["CountyName"]),
                                IsCountySearch = Convert.ToBoolean(_dt.Rows[i]["IsCountySearch"]),
                                IsStateSearch = Convert.ToBoolean(_dt.Rows[i]["IsStateSearch"])
                            });
                        }
                    }

                    if (_dsApplicantData.Tables[1].Rows.Count > 0)
                    {
                        DataTable _dt = _dsApplicantData.Tables[1];

                        for (int i = 0; i < _dt.Rows.Count; i++)
                        {
                            _lstPersonAlias.Add(new SupplementOrderApplicantPersonAliasContract
                            {
                                FirstName = Convert.ToString(_dt.Rows[i]["FirstName"]),
                                LastName = Convert.ToString(_dt.Rows[i]["LastName"]),
                                //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                                MiddleName = Convert.ToString(_dt.Rows[i]["MiddleName"]),
                                IsUsed = Convert.ToBoolean(_dt.Rows[i]["IsUsed"])
                            });
                        }
                    }
                }
                var _dataTuple = Tuple.Create(_lstResidentialHistory, _lstPersonAlias);
                return _dataTuple;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        #endregion
        #endregion

        #region Stored Procedures - Manual Service Forms


        public static List<ManualServiceFormContract> GetManualServiceFormSearch(CustomPagingArgsContract gridCustomPaging, ManualServiceFormsSearchContract manualServiceFormsSearchContract, Int32 tenantId)
        {
            try
            {
                List<ManualServiceFormContract> lstManualServiceFormsContract = new List<ManualServiceFormContract>();

                DataSet ds = new DataSet();
                ds = GetManualServiceFormsSearchDetail(gridCustomPaging, manualServiceFormsSearchContract, tenantId);
                DataTable dtManualServiceForms = ds.Tables[0];

                for (int i = 0; i < dtManualServiceForms.Rows.Count; i++)
                {
                    lstManualServiceFormsContract.Add(new ManualServiceFormContract
                    {
                        OrganizationUserId = dtManualServiceForms.Rows[i]["OrganizationUserID"] != DBNull.Value ? Convert.ToInt32(dtManualServiceForms.Rows[i]["OrganizationUserID"]) : 0,
                        OrderID = dtManualServiceForms.Rows[i]["OrderID"] != DBNull.Value ? Convert.ToInt32(dtManualServiceForms.Rows[i]["OrderID"]) : 0,
                        HierarchyLabel = dtManualServiceForms.Rows[i]["HierarchyLabel"] != DBNull.Value ? Convert.ToString(dtManualServiceForms.Rows[i]["HierarchyLabel"]) : null,
                        ApplicantFirstName = dtManualServiceForms.Rows[i]["ApplicantFirstName"] != DBNull.Value ? Convert.ToString(dtManualServiceForms.Rows[i]["ApplicantFirstName"]) : null,
                        ApplicantLastName = dtManualServiceForms.Rows[i]["ApplicantLastName"] != DBNull.Value ? Convert.ToString(dtManualServiceForms.Rows[i]["ApplicantLastName"]) : null,
                        TotalCount = Convert.ToInt32(dtManualServiceForms.Rows[i]["TotalCount"]),
                        //ServiceID = Convert.ToInt32(dtManualServiceForms.Rows[i]["ServiceID"]),
                        ServiceName = dtManualServiceForms.Rows[i]["ServiceName"] != DBNull.Value ? Convert.ToString(dtManualServiceForms.Rows[i]["ServiceName"]) : null,
                        //ServiceFormStatus = dtManualServiceForms.Rows[i]["ServiceFormStatus"] != DBNull.Value ? Convert.ToString(dtManualServiceForms.Rows[i]["ServiceFormStatus"]) : "NA",
                        ApplicantAddress = dtManualServiceForms.Rows[i]["ApplicantAddress"] != DBNull.Value ? Convert.ToString(dtManualServiceForms.Rows[i]["ApplicantAddress"]) : null,
                        SFName = dtManualServiceForms.Rows[i]["SFName"] != DBNull.Value ? Convert.ToString(dtManualServiceForms.Rows[i]["SFName"]) : null,
                        SFStatus = dtManualServiceForms.Rows[i]["SFStatus"] != DBNull.Value ? Convert.ToString(dtManualServiceForms.Rows[i]["SFStatus"]) : null,
                        ServiceFormId = dtManualServiceForms.Rows[i]["ServiceFormId"] != DBNull.Value ? Convert.ToInt32(dtManualServiceForms.Rows[i]["ServiceFormId"]) : 0,
                        SFStatusId = dtManualServiceForms.Rows[i]["SFStatusId"] != DBNull.Value ? Convert.ToInt32(dtManualServiceForms.Rows[i]["SFStatusId"]) : 0,
                        OrderServiceFormId = dtManualServiceForms.Rows[i]["OrderServiceFormId"] != DBNull.Value ? Convert.ToInt32(dtManualServiceForms.Rows[i]["OrderServiceFormId"]) : 0,
                        NotificationId = dtManualServiceForms.Rows[i]["NotificationId"] != DBNull.Value ? Convert.ToInt32(dtManualServiceForms.Rows[i]["NotificationId"]) : 0,
                        HierarchyNodeID = dtManualServiceForms.Rows[i]["HierarchyNodeID"] != DBNull.Value ? Convert.ToInt32(dtManualServiceForms.Rows[i]["HierarchyNodeID"]) : 0,
                        ApplicantEmailAddress = dtManualServiceForms.Rows[i]["ApplicantEmailAddress"] != DBNull.Value ? Convert.ToString(dtManualServiceForms.Rows[i]["ApplicantEmailAddress"]) : null,
                        OrderNumber = dtManualServiceForms.Rows[i]["OrderNumber"] != DBNull.Value ? Convert.ToString(dtManualServiceForms.Rows[i]["OrderNumber"]) : null,
                        //UAT-2165
                        PackageName = dtManualServiceForms.Rows[i]["PackageName"] != DBNull.Value ? Convert.ToString(dtManualServiceForms.Rows[i]["PackageName"]) : null,
                        //UAT-2671
                        ServiceGroupName = dtManualServiceForms.Rows[i]["ServiceGroupName"] != DBNull.Value ? Convert.ToString(dtManualServiceForms.Rows[i]["ServiceGroupName"]) : null
                    });
                }

                return lstManualServiceFormsContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get Updated Document Mappings for a particular Item, whether Normal or Exception type
        /// </summary>
        /// <param name="applicantComplianceAttributeIdsXML"></param>
        /// <param name="applicantComplianceItemIdsXML"></param>
        /// <returns></returns>
        private static DataSet GetManualServiceFormsSearchDetail(CustomPagingArgsContract gridCustomPaging, ManualServiceFormsSearchContract manualServiceFormsSearchContract, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetStoredProceduresRepoInstance(tenantId).GetManualServiceFormsSearchDetail(gridCustomPaging, manualServiceFormsSearchContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        public static List<ApplicantInstitutionHierarchyMapping> GetApplicantInstitutionHierarchyMapping(Int32 tenantId, String organizationUserIDs)
        {

            try
            {
                return BALUtils.GetStoredProceduresRepoInstance(tenantId).GetApplicantInstitutionHierarchyMapping(organizationUserIDs);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region Evaluate Post Submit Rules For Multi
        /// <summary>
        /// Method is used Execute Post Submit Rules For Multi
        /// </summary>
        /// <param name="ruleXml">ruleXml</param>
        /// <param name="userID">userID</param>
        /// <param name="tenantId">tenantId</param>
        public static void EvaluatePostSubmitRulesForMulti(String ruleXml, Int32 userID, Int32 tenantId)
        {
            try
            {
                BALUtils.GetStoredProceduresRepoInstance(tenantId).EvaluatePostSubmitRulesForMulti(ruleXml, userID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        public static Dictionary<String, List<BackgroundOrderDailyReport>> GetPermissionsSubscriptionSettings(String subEventCode, String serviceType, Int32 tenantId)
        {
            try
            {
                DataTable _dt = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetPermissionsSubscriptionSettings(subEventCode, serviceType, tenantId);

                if (_dt.Rows.Count == 0)
                    return new Dictionary<String, List<BackgroundOrderDailyReport>>();

                var _orgUserIdColumnName = "OrgUserId";
                var _dpmIdColumnName = "DPMId";
                var _captureDateTimeColumnName = "CaptureDateTime";
                var _emailAddressColumnName = "EmailAddress";
                var _userNameColumnName = "UserName";
                var _serviceExecutionHistoryIdColumnName = "ServiceExecutionHistoryId";
                var _fromDateColumnName = "FromDate";

                var _capturedDateTime = Convert.ToDateTime(_dt.Rows[0][_captureDateTimeColumnName]);
                var _fromDate = _dt.Rows[0][_fromDateColumnName] != DBNull.Value ? Convert.ToDateTime(_dt.Rows[0][_fromDateColumnName]) : (DateTime?)null;
                var _serviceExecutionHistoryId = Convert.ToInt32(_dt.Rows[0][_serviceExecutionHistoryIdColumnName]);

                Dictionary<String, List<BackgroundOrderDailyReport>> _dic = new Dictionary<String, List<BackgroundOrderDailyReport>>();

                DataView dv = new DataView(_dt);
                DataTable _dtOrgIds = dv.ToTable(true, _orgUserIdColumnName);

                for (int rowNo = 0; rowNo < _dtOrgIds.Rows.Count; rowNo++)
                {


                    var _orgUserId = Convert.ToInt32(_dtOrgIds.Rows[rowNo][_orgUserIdColumnName]);
                    var _lstNodeIds = _dt.AsEnumerable()
                        .Where(oid => oid.Field<Int32>(_orgUserIdColumnName) == _orgUserId)
                        .Select(nid => nid.Field<Int32?>(_dpmIdColumnName))
                        .ToList();

                    var _emailAddress = _dt.AsEnumerable()
                      .Where(oid => oid.Field<Int32>(_orgUserIdColumnName) == _orgUserId)
                      .Select(nid => nid.Field<String>(_emailAddressColumnName))
                      .FirstOrDefault();

                    var _userName = _dt.AsEnumerable()
                     .Where(oid => oid.Field<Int32>(_orgUserIdColumnName) == _orgUserId)
                     .Select(nid => nid.Field<String>(_userNameColumnName))
                     .FirstOrDefault();


                    StringBuilder _sbNodeIds = new StringBuilder();

                    foreach (var nodeId in _lstNodeIds)
                    {
                        if (!nodeId.IsNullOrEmpty())
                        {
                            _sbNodeIds.Append(nodeId + ",");
                        }
                    }

                    var _generatedKey = _lstNodeIds.IsNullOrEmpty() ? String.Empty : Convert.ToString(_sbNodeIds);

                    if (!String.IsNullOrEmpty(_generatedKey))
                    {
                        _generatedKey = _generatedKey.Substring(0, _generatedKey.LastIndexOf(','));

                        List<BackgroundOrderDailyReport> _lstIds = new List<BackgroundOrderDailyReport>();

                        if (_dic.ContainsKey(_generatedKey))
                        {
                            _lstIds = _dic[_generatedKey];
                            _lstIds.Add(GenerateInstance(serviceType, _capturedDateTime, rowNo, _orgUserId, _emailAddress, _userName, _fromDate, _serviceExecutionHistoryId));
                        }
                        else
                        {
                            _lstIds = new List<BackgroundOrderDailyReport>();
                            _lstIds.Add(GenerateInstance(serviceType, _capturedDateTime, rowNo, _orgUserId, _emailAddress, _userName, _fromDate, _serviceExecutionHistoryId));
                            _dic.Add(_generatedKey, _lstIds);
                        }
                    }
                }

                return _dic;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static BackgroundOrderDailyReport GenerateInstance(String serviceType, DateTime _captureDateTime, Int32 rowNo, Int32 _orgUserId, String emailAddress, String userName, DateTime? fromDate, Int32 serviceExecutionHistoryId)
        {
            return new BackgroundOrderDailyReport
            {
                OrganizationUserId = _orgUserId,
                CaptureDateTime = _captureDateTime,
                ServiceTypeCode = serviceType,
                UserName = userName,
                EmailAddress = emailAddress,
                FromDate = fromDate,
                ServiceExecutionHistoryId = serviceExecutionHistoryId
            };
        }

        #region Sales Force

        /// <summary>
        /// Gets the JSON based string of the data to be uploaded to the SalesForce
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="chunkSize"></param>
        /// <param name="hasAnyData">Used to identify if there is any data to upload</param>
        /// <param name="tpcduIds">ID's used to update the Upload status after Task execution</param>
        /// <returns></returns>
        public static String GetComplianceDataToUpload(Int32 tenantId, Int32 chunkSize, out Boolean hasAnyData, out String tpcduIds)
        {
            try
            {
                #region Constants

                hasAnyData = false;
                tpcduIds = String.Empty;
                var _dataToUpload = String.Empty;
                var _adbPrefix = "adb";
                var _formatPrefix = "ADB_";
                var _formatSuffix = "__c";
                var _pkgSubcIdColumnName = "PackageSubscriptionId";
                var _userIdColumnName = "OrganizationUserID";
                var _firstNameColumnName = "FirstName";
                var _lastNameColumnName = "LastName";
                var _emailIdColumnName = "EmailID";
                var _tpcduId = "TPCDUId";
                var _catLabelColumnName = "CatLabel";
                var _catStsColumnName = "CatSts";
                var _catExpDateColumnName = "CatExpiryDate";
                var _attrLabelColumnName = "CALabel";
                var _attrValueColumnName = "CAValue";
                var _compliantDate = "CompliantDate";
                StringBuilder _sbString = new StringBuilder();
                StringBuilder sbTPCDUIds = new StringBuilder();
                var _escapedDblQuote = "\"";
                #endregion

                DataSet _dsData = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetComplianceDataToUpload(chunkSize);
                if (_dsData.Tables.Count == AppConsts.NONE || (_dsData.Tables.Count > AppConsts.NONE && _dsData.Tables[AppConsts.NONE].Rows.Count == AppConsts.NONE))
                    return _dataToUpload;

                DataTable _dt = _dsData.Tables[0];
                DataTable _dtCustomAttributes = _dsData.Tables[1];

                DataView dv = new DataView(_dt);
                DataTable _dtPkgSubIds = dv.ToTable(true, _pkgSubcIdColumnName);

                _sbString.Append("{ " + _escapedDblQuote + "records" + _escapedDblQuote + ":[");
                var _subsCount = _dtPkgSubIds.Rows.Count;

                for (Int32 _psId = 0; _psId < _subsCount; _psId++)
                {
                    hasAnyData = true;

                    var _currentSubscriptionId = Convert.ToInt32(_dtPkgSubIds.Rows[_psId][_pkgSubcIdColumnName]);

                    var currentUserData = _dt.AsEnumerable()
                          .Where(psId => psId.Field<Int32>(_pkgSubcIdColumnName) == _currentSubscriptionId)
                          .Select(uData => new
                          {
                              userId = uData.Field<Int32>(_userIdColumnName),
                              emailId = uData.Field<String>(_emailIdColumnName),
                              fName = uData.Field<String>(_firstNameColumnName),
                              lName = uData.Field<String>(_lastNameColumnName),
                              tpcduId = uData.Field<Int32>(_tpcduId),

                          }).First();

                    _sbString.Append("{");

                    #region Custom Attribute JSON

                    var customAttrLst = _dtCustomAttributes.AsEnumerable()
                        .Where(psId => psId.Field<Int32>(_pkgSubcIdColumnName) == _currentSubscriptionId)
                        .Select(cAttrData => new
                        {
                            attrLbl = cAttrData.Field<String>(_attrLabelColumnName),
                            attrValue = cAttrData.Field<String>(_attrValueColumnName)
                        })
                        .ToList();

                    var _attrCount = customAttrLst.Count();

                    for (int i = 0; i < _attrCount; i++)
                    {
                        //Changes regarding UAt-1081
                        if (customAttrLst[i].attrLbl.ToLower().Contains("studentid"))
                        {
                            _sbString.Append(_escapedDblQuote + "studentId" + _escapedDblQuote +
                                             ":" + _escapedDblQuote + customAttrLst[i].attrValue + _escapedDblQuote);
                        }
                        else
                        {
                            string label = customAttrLst[i].attrLbl;
                            TextInfo info = Thread.CurrentThread.CurrentCulture.TextInfo;
                            label = info.ToTitleCase(label);
                            string[] parts = label.Split(new char[] { },
                                StringSplitOptions.RemoveEmptyEntries);
                            string result = String.Join(String.Empty, parts);
                            result = result.Substring(0, 1).ToLower() + result.Substring(1);
                            _sbString.Append(_escapedDblQuote + label + _escapedDblQuote +
                                                ":" + _escapedDblQuote + customAttrLst[i].attrValue + _escapedDblQuote);
                        }

                        _sbString.Append(",");
                    }

                    #endregion

                    _sbString.Append(_escapedDblQuote + _adbPrefix + "Id" + _escapedDblQuote +
                               ":" + _escapedDblQuote + currentUserData.userId + _escapedDblQuote + ",");

                    _sbString.Append(_escapedDblQuote + "studentName" + _escapedDblQuote +
                               ":" + _escapedDblQuote + currentUserData.fName + " " + currentUserData.lName + _escapedDblQuote + ",");

                    _sbString.Append(_escapedDblQuote + "studentEmail" + _escapedDblQuote +
                               ":" + _escapedDblQuote + currentUserData.emailId + _escapedDblQuote + ",");

                    sbTPCDUIds.Append(currentUserData.tpcduId + ",");

                    #region Category Data JSON

                    var catLst = _dt.AsEnumerable()
                          .Where(psId => psId.Field<Int32>(_pkgSubcIdColumnName) == _currentSubscriptionId)
                          .Select(catData => new
                          {
                              catLbl = catData.Field<String>(_catLabelColumnName),
                              catSts = catData.Field<String>(_catStsColumnName),
                              catExpDate = catData.Field<DateTime?>(_catExpDateColumnName),
                              compliantDate = catData.Field<DateTime?>(_compliantDate),
                          })
                          .ToList();

                    var _catCount = catLst.Count();
                    _sbString.Append(_escapedDblQuote + "categories" + _escapedDblQuote + ":[");
                    for (int i = 0; i < _catCount; i++)
                    {
                        var _sts = catLst[i].catSts == "1" ? "true" : "false";

                        _sbString.Append("{");
                        _sbString.Append(_escapedDblQuote + "name" + _escapedDblQuote +
                                   ":" + _escapedDblQuote + _formatPrefix + catLst[i].catLbl + _formatSuffix + _escapedDblQuote + ",");

                        _sbString.Append(_escapedDblQuote + "compliant" + _escapedDblQuote +
                                   ":" + _escapedDblQuote + _sts + _escapedDblQuote + ",");

                        _sbString.Append(_escapedDblQuote + "dateName" + _escapedDblQuote +
                                   ":" + _escapedDblQuote + _formatPrefix + catLst[i].catLbl + "ComplianceDate" + _formatSuffix + _escapedDblQuote + ",");

                        if (catLst[i].compliantDate.IsNullOrEmpty())
                            _sbString.Append(_escapedDblQuote + "complianceDate" + _escapedDblQuote + ":" + _escapedDblQuote + String.Empty + _escapedDblQuote + ",");
                        else
                            _sbString.Append(_escapedDblQuote + "complianceDate" + _escapedDblQuote +
                                    ":" + _escapedDblQuote + Convert.ToDateTime(catLst[i].compliantDate).ToShortDateString() + _escapedDblQuote + ",");

                        _sbString.Append(_escapedDblQuote + "expirationDateName" + _escapedDblQuote +
                                   ":" + _escapedDblQuote + _formatPrefix + catLst[i].catLbl + "ExpirationDate" + _formatSuffix + _escapedDblQuote + ",");

                        if (catLst[i].catExpDate.IsNullOrEmpty())
                            _sbString.Append(_escapedDblQuote + "expirationDate" + _escapedDblQuote + ":" + _escapedDblQuote + String.Empty + _escapedDblQuote);
                        else
                            _sbString.Append(_escapedDblQuote + "expirationDate" + _escapedDblQuote +
                                    ":" + _escapedDblQuote + Convert.ToDateTime(catLst[i].catExpDate).ToShortDateString() + _escapedDblQuote);


                        _sbString.Append("}");
                        if (i < _catCount - 1)
                            _sbString.Append(",");
                    }
                    _sbString.Append("]");

                    #endregion

                    _sbString.Append("}");
                    if (_psId < _subsCount - 1)
                        _sbString.Append(",");
                }
                _sbString.Append("]}");

                _dataToUpload = Convert.ToString(_sbString);
                tpcduIds = Convert.ToString(sbTPCDUIds);

                return _dataToUpload;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Save the history for the execution of the service to uplaod the data to Sales Force
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public static void SaveComplianceUploadServiceHistory(String request, String response, Int32 tenantId)
        {
            try
            {
                BALUtils.GetStoredProceduresRepoInstance(tenantId).SaveComplianceUploadServiceHistory(request, response);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Update the status of the Records that have been either uploaded or Error occured in their upload,
        /// depending on the code passed
        /// </summary>
        /// <param name="tpcduIds">CSV of the TPCDU_ID's</param>
        /// <param name="statusCode"></param>
        public static void UpdateThirdPartyComplianceDataUploadStatus(String tpcduIds, String statusCode, Int32 backgroundProcessUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetStoredProceduresRepoInstance(tenantId).UpdateThirdPartyComplianceDataUploadStatus(tpcduIds, statusCode, backgroundProcessUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Update the status of the Records that have been either uploaded or Error occured in their upload,
        /// depending on the code passed
        /// </summary>
        /// <param name="tpcduIds">CSV of the TPCDU_ID's</param>
        /// <param name="statusCode"></param>
        public static List<ThirdPartyDataUploadResponseTypeContract> GetThirdPartyDataUploadResponseRegex(Int32 clientDataUploadID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetStoredProceduresRepoInstance(tenantId).GetThirdPartyDataUploadResponseRegex(clientDataUploadID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        /// <summary>
        /// Gets the Package level Payment Options, based on the package type i.e. Background or Compliance
        /// </summary>
        /// <param name="dppId"></param>
        /// <param name="packageTypeCode"></param>
        /// <param name="offlineSettlementCode"></param>
        /// <returns></returns>
        public static List<Tuple<Int32, String, Boolean>> GetPackagePaymentOptions(Int32 pkgNodeMappingId, String packageTypeCode, Int32 tenantId)
        {
            try
            {
                List<Tuple<Int32, String, Boolean>> _lst = new List<Tuple<int, string, bool>>();
                DataTable _dt = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetPackagePaymentOptions(pkgNodeMappingId, packageTypeCode, PaymentOptions.OfflineSettlement.GetStringValue());

                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    _lst.Add(Tuple.Create(
                        Convert.ToInt32(_dt.Rows[i]["PaymentOptionId"]),
                        Convert.ToString(_dt.Rows[i]["PaymentOptionName"]),
                        Convert.ToBoolean(_dt.Rows[i]["IsSelected"])
                        ));
                }
                return _lst;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Updates the Display order of the Nodes of the Hierarchy Tree
        /// </summary>
        /// <param name="lstDPMIds"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="currentUserId"></param>
        public static Boolean UpdateNodeDisplayOrder(List<GetChildNodesWithPermission> lstDPMIds, Int32? destinationIndex, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetStoredProceduresRepoInstance(tenantId).UpdateNodeDisplayOrder(lstDPMIds, destinationIndex, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get the Institution Nodes of the Selected Tenant, for the current user, based on the Permissions
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Dictionary<Int32, String> GetInstitutionNodes(Int32? userId, Int32 tenantId)
        {
            try
            {
                Dictionary<Int32, String> _dicNodes = new Dictionary<Int32, String>();
                DataTable _dt = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetInstitutionNodes(userId);

                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    _dicNodes.Add(Convert.ToInt32(_dt.Rows[i]["DPM_ID"]), Convert.ToString(_dt.Rows[i]["DPM_Label"]));
                }
                return _dicNodes;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-916
        public static DataSet GetPaymentOptions(Int32 tenantId, Int32 dppId, String bphmIds, Int32 dpmId)
        {
            try
            {
                return BALUtils.GetStoredProceduresRepoInstance(tenantId).GetPaymentOptions(dppId, bphmIds, dpmId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Admin Data Entry

        /// <summary>
        /// Gets the All the Package subscription related meta-data and applicant data 
        /// for the selected package subscription in Admin Data entry details screen
        /// </summary>
        /// <param name="pkgSubscriptionId"></param>
        /// <returns></returns>
        public static List<AdminDataEntryUIContract> GetAdminDataEntrySubscription(Int32 pkgSubscriptionId, Int32 documentId, Int32 tenantId)
        {
            try
            {
                var _ds = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetAdminDataEntrySubscription(pkgSubscriptionId, documentId);
                var lstDataEntryUIContract = new List<AdminDataEntryUIContract>();
                if (_ds.Tables.Count > 0)
                {
                    var _dt = _ds.Tables[0];

                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        var _dataEntryUIContract = new AdminDataEntryUIContract();

                        _dataEntryUIContract.PkgName = Convert.ToString(_dt.Rows[i]["PkgName"]);
                        _dataEntryUIContract.CatName = Convert.ToString(_dt.Rows[i]["CatName"]);
                        _dataEntryUIContract.ItemName = Convert.ToString(_dt.Rows[i]["ItemName"]);
                        _dataEntryUIContract.AttrName = Convert.ToString(_dt.Rows[i]["AttName"]);
                        _dataEntryUIContract.AttrDataType = Convert.ToString(_dt.Rows[i]["AttDataType"]);

                        _dataEntryUIContract.PkgId = Convert.ToInt32(_dt.Rows[i]["PkgId"]);
                        _dataEntryUIContract.CatId = Convert.ToInt32(_dt.Rows[i]["CatId"]);
                        _dataEntryUIContract.ItemId = Convert.ToInt32(_dt.Rows[i]["ItemId"]);
                        _dataEntryUIContract.AttrId = Convert.ToInt32(_dt.Rows[i]["AttributeId"]);

                        _dataEntryUIContract.AttrOptionText = Convert.ToString(_dt.Rows[i]["OptionText"]);
                        _dataEntryUIContract.AttrOptionValue = Convert.ToString(_dt.Rows[i]["OptionValue"]);

                        _dataEntryUIContract.CatDataId = Convert.ToInt32(_dt.Rows[i]["CatDataId"]);
                        _dataEntryUIContract.ItemDataId = Convert.ToInt32(_dt.Rows[i]["ItemDataId"]);
                        _dataEntryUIContract.OldItemStatus = Convert.ToString(_dt.Rows[i]["ItemStatus"]);
                        _dataEntryUIContract.OldItemStatusCode = Convert.ToString(_dt.Rows[i]["ItemStatusCode"]);

                        //UAT-3805
                        _dataEntryUIContract.OldCategoryStatusCode = Convert.ToString(_dt.Rows[i]["CategoryStatusCode"]);


                        _dataEntryUIContract.AttrDataId = Convert.ToInt32(_dt.Rows[i]["AttrDataId"]);
                        _dataEntryUIContract.AttrValue = Convert.ToString(_dt.Rows[i]["AttrValue"]);

                        _dataEntryUIContract.AttrGroupId = Convert.ToInt32(_dt.Rows[i]["AttrGroupId"]);
                        _dataEntryUIContract.AttrGroupName = Convert.ToString(_dt.Rows[i]["AttrGrpName"]);
                        _dataEntryUIContract.IsGrouped = Convert.ToBoolean(_dt.Rows[i]["IsGrouped"]);
                        _dataEntryUIContract.IsCurrentDocAssociated = Convert.ToBoolean(_dt.Rows[i]["IsDocAssociated"]);

                        #region UAT-1540
                        _dataEntryUIContract.CatExplanatoryNotes = Convert.ToString(_dt.Rows[i]["CatExplanatoryNotes"]);
                        #endregion

                        #region UAT-1608: Admin data entry screen[Shot Series Changes]
                        _dataEntryUIContract.IsReadOnly = Convert.ToBoolean(_dt.Rows[i]["IsReadOnly"]);
                        _dataEntryUIContract.IsItemSeries = Convert.ToBoolean(_dt.Rows[i]["IsItemSeries"]);
                        _dataEntryUIContract.ItemSeriesID = Convert.ToInt32(_dt.Rows[i]["ItemSeriesID"]);

                        #endregion

                        //UAT-1642:WB: Admin Data Entry to only function for ADB Review and Joint Review
                        _dataEntryUIContract.IsReviewerTypeAdmin = Convert.ToBoolean(_dt.Rows[i]["IsReviewerTypeAdmin"]);
                        _dataEntryUIContract.ifDataEntryAllowed = Convert.ToBoolean(_dt.Rows[i]["DataEntryAllowed"]);

                        _dataEntryUIContract.ifCatExceptionMapped = Convert.ToBoolean(_dt.Rows[i]["CatExceptionMapped"]);
                        _dataEntryUIContract.ifCatComplianceRule = Convert.ToBoolean(_dt.Rows[i]["ComplianceRule"]);
                        _dataEntryUIContract.ifItemExpiryRule = Convert.ToBoolean(_dt.Rows[i]["ExpiryRule"]);
                        _dataEntryUIContract.ComplianceAttributeTypeCode = _dt.Rows[i]["ComplianceAttributeTypeCode"].ToString();
                        lstDataEntryUIContract.Add(_dataEntryUIContract);
                    }
                }
                return lstDataEntryUIContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Gets the All the Package subscription related meta-data and applicant data 
        /// for the selected package subscription in Admin Data entry details screen
        /// </summary>
        /// <param name="pkgSubscriptionId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<ApplicantComplianceCategoryData> GetPackageDetailsBySubscriptionId(Int32 pkgSubscriptionId, Int32 tenantId)
        {
            try
            {
                var _ds = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetPackageDetailsBySubscriptionId(pkgSubscriptionId);
                var _lstCategoryData = new List<ApplicantComplianceCategoryData>();

                if (_ds.Tables.Count > 0)
                {
                    var _catIdColumnName = "CatId";
                    var _dt = _ds.Tables[0];

                    DataView _dvDistinctCat = new DataView(_dt);
                    DataTable _dtCategories = _dvDistinctCat.ToTable(true, _catIdColumnName);

                    for (int i = 0; i < _dtCategories.Rows.Count; i++)
                    {
                        var _catData = new ApplicantComplianceCategoryData();
                        _catData.ComplianceCategoryID = Convert.ToInt32(_dtCategories.Rows[i][_catIdColumnName]);

                        _catData.ApplicantComplianceItemDatas = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ApplicantComplianceItemData>();

                        var _lstItems = _dt.AsEnumerable().ToList().Where
                            (
                              cat => Convert.ToInt32(cat[_catIdColumnName]) == Convert.ToInt32(_dtCategories.Rows[i][_catIdColumnName])
                            )
                            .Select(itm => new
                            {
                                CategoryName = Convert.ToString(itm["CatName"]),
                                CatDataId = Convert.ToInt32(itm["CatDataId"]),
                                PkgSubId = Convert.ToInt32(itm["PkgSubId"]),
                                ItemName = Convert.ToString(itm["ItemName"]),
                                ItemId = Convert.ToInt32(itm["ItemId"]),
                                ItemDataId = Convert.ToInt32(itm["ItemDataId"]),
                                PkgName = Convert.ToString(itm["PkgName"]),
                                ApplicantName = Convert.ToString(itm["ApplicantName"]),
                                ApplicantId = Convert.ToInt32(itm["ApplicantId"]),
                                HierarchyNodeId = Convert.ToInt32(itm["HierarchyNodeId"]),
                                RushOrderStsCode = Convert.ToString(itm["RushOrderStatusCode"]),
                                RushOrderStsText = Convert.ToString(itm["RushOrderStatus"]),
                                SubmissionDate = itm["SubmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(itm["SubmissionDate"]),
                                ReconciliationReviewCount = itm["ReconciliationReviewCount"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(itm["ReconciliationReviewCount"]),
                                StatusId = itm["StatusId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(itm["StatusId"])
                            }).ToList();

                        foreach (var item in _lstItems)
                        {
                            _catData.ApplicantComplianceCategoryID = item.CatDataId;
                            _catData.PackageSubscriptionID = item.PkgSubId;
                            _catData.ApplicantComplianceItemDatas.Add(new ApplicantComplianceItemData
                            {
                                ApplicantComplianceCategoryID = item.CatDataId,
                                ComplianceCategoryName = item.CategoryName,
                                ComplianceItemName = item.ItemName,
                                ComplianceItemID = item.ItemId,
                                ApplicantComplianceItemID = item.ItemDataId,
                                CompliancePackageName = item.PkgName,
                                ApplicantId = item.ApplicantId,
                                ApplicantName = item.ApplicantName,
                                HierarchyNodeId = item.HierarchyNodeId,
                                RushOrderStatusCode = item.RushOrderStsCode,
                                RushOrderStatusText = item.RushOrderStsText,
                                SubmissionDate = item.SubmissionDate,
                                StatusID = item.StatusId.IsNotNull() ? item.StatusId.Value : AppConsts.NONE,
                                ReconciliationReviewCount = item.ReconciliationReviewCount
                            });
                        }
                        _lstCategoryData.Add(_catData);
                    }
                }
                return _lstCategoryData;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-1067:Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on.
        /// <summary>
        ///  Method to get Compliance and background packages of selected node or its parent nodes.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="nodeId"></param>
        /// <param name="organizationUserId"></param>
        /// <param name="orderPackageTypeCode"></param>
        /// <returns></returns>
        public static List<AvailablePackageContarct> GetAvailableCompAndBkgPackages(Int32 tenantId, Int32 nodeId, String orderPackageTypeCode, Boolean isLoadParentPackages = false)
        {
            try
            {
                DataTable availablePackagesData = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetAvailableCompAndBkgPackages(nodeId, orderPackageTypeCode, isLoadParentPackages);
                return AssignValuesToPackagesDataModel(availablePackagesData);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static List<AvailablePackageContarct> AssignValuesToPackagesDataModel(DataTable availablePackagesData)
        {
            IEnumerable<DataRow> rows = availablePackagesData.AsEnumerable();
            return rows.Select(x => new AvailablePackageContarct
            {
                PackageId = Convert.ToInt32(x["PackageId"]),
                DPP_ID = x["DPP_ID"].GetType().Name == "DBNull" ? null : (Int32?)(x["DPP_ID"]),
                PackageName = Convert.ToString(x["PackageName"]),
                BPHM_ID = x["BPHM_ID"].GetType().Name == "DBNull" ? null : (Int32?)(x["BPHM_ID"]),
                DPM_ID = Convert.ToInt32(x["DPM_ID"]),
                IsCompliancePackage = Convert.ToBoolean(x["IsCompliancePackage"])
            }).ToList();
        }
        #endregion

        #region Profile sharing

        /// <summary>
        /// Get the compliance and background packages that can be shared by the applicant
        /// </summary>
        /// <param name="orgUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<ProfileSharingPackages> GetSharingPackages(Int32 orgUserId, Int32 tenantId)
        {
            try
            {
                var _ds = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetSharingPackages(orgUserId);
                var _lstPackages = new List<ProfileSharingPackages>();

                var _pkgIdColumn = "PackageId";
                var _pkgNameColumn = "PackageName";
                var _pkgComplinaceStatus = "PkgComplinaceStatus";

                if (_ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    var _pkgSubIdColumn = "PkgSubscriptionId";

                    var _catNameColumn = "CategoryName";
                    var _catIdColumn = "CategoryId";
                    var _dtCompliancePkgs = _ds.Tables[0];

                    DataView _dvCompliancePkg = new DataView(_dtCompliancePkgs);
                    //DataTable _dt = _dvCompliancePkg.ToTable(true, _pkgIdColumn);
                    DataTable _dt = _dvCompliancePkg.ToTable(true, _pkgSubIdColumn);

                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        var _sharingPackage = new ProfileSharingPackages();
                        _sharingPackage.CompliancePkgCategories = new List<ComplianceCategory>();
                        //var _currentPkgId = Convert.ToInt32(_dt.Rows[i][_pkgIdColumn]);
                        var _currentPkgSubId = Convert.ToInt32(_dt.Rows[i][_pkgSubIdColumn]);

                        var _lstPkgs = _dtCompliancePkgs.AsEnumerable().ToList().Where
                        (
                          pkg => Convert.ToInt32(pkg[_pkgSubIdColumn]) == _currentPkgSubId
                        )
                        .Select(itm => new
                        {
                            PkgId = Convert.ToInt32(itm[_pkgIdColumn]),
                            PkgSubId = _currentPkgSubId,
                            PkgName = Convert.ToString(itm[_pkgNameColumn]),
                            CatName = Convert.ToString(itm[_catNameColumn]),
                            CatId = Convert.ToInt32(itm[_catIdColumn]),
                            //UAT-2232
                            pkgComStatus = Convert.ToString(itm[_pkgComplinaceStatus])
                        }).ToList();

                        foreach (var pkg in _lstPkgs)
                        {
                            _sharingPackage.PackageId = pkg.PkgId;
                            _sharingPackage.PackageSubscriptionId = pkg.PkgSubId;
                            _sharingPackage.PackageName = pkg.PkgName;
                            _sharingPackage.IsCompliancePkg = true;
                            //UAT-2232
                            _sharingPackage.PkgComplianceStatus = pkg.pkgComStatus;

                            if (pkg.CatId > AppConsts.NONE && !_sharingPackage.CompliancePkgCategories.Any(cpc => cpc.ComplianceCategoryID == pkg.CatId))
                            {
                                _sharingPackage.CompliancePkgCategories.Add(new ComplianceCategory
                                {
                                    CategoryName = pkg.CatName,
                                    ComplianceCategoryID = pkg.CatId
                                });
                            }
                        }
                        _lstPackages.Add(_sharingPackage);
                    }
                }

                if (_ds.Tables.Count > 0 && _ds.Tables[1].Rows.Count > 0)
                {
                    Int32? nullValue = null;
                    var _svcGrpNameColumn = "SvcGroupName";
                    var _svcGrpIdColumn = "SvcGroupId";
                    var _bkgOrderPkgIdColumn = "BkgOrderPkgId";

                    var _dtBkgPkgs = _ds.Tables[1];

                    DataView _dvBkgPkg = new DataView(_dtBkgPkgs);
                    DataTable _dt = _dvBkgPkg.ToTable(true, _bkgOrderPkgIdColumn);

                    var bkgPkgs = _dtBkgPkgs.AsEnumerable().ToList();

                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        var _sharingPackage = new ProfileSharingPackages();
                        _sharingPackage.BkgSvcGroups = new List<BkgSvcGroup>();
                        var _currentBkgOrderPackageID = Convert.ToInt32(_dt.Rows[i][_bkgOrderPkgIdColumn]);

                        var _lstPkgs = bkgPkgs.Where(
                                  bkgOrderPackage => Convert.ToInt32(bkgOrderPackage[_bkgOrderPkgIdColumn]) == _currentBkgOrderPackageID
                              )
                              .Select(itm => new
                              {
                                  PkgName = Convert.ToString(itm[_pkgNameColumn]),
                                  SvcGrpName = Convert.ToString(itm[_svcGrpNameColumn]),
                                  //SvcGrpId = Convert.ToInt32(itm[_svcGrpIdColumn]),
                                  SvcGrpId = itm[_svcGrpIdColumn].GetType().Name == "DBNull" ? nullValue : Convert.ToInt32(itm[_svcGrpIdColumn]),
                                  BkgOrderPkgId = Convert.ToInt32(itm[_bkgOrderPkgIdColumn]),
                                  PackageId = Convert.ToInt32(itm[_pkgIdColumn]),
                                  SvcGrpCompletionDate = itm["SvcGroupCompletionDate"].GetType().Name == "DBNull" ? (DateTime?)null : Convert.ToDateTime(itm["SvcGroupCompletionDate"]),
                              }).ToList();

                        //var _lstPkgs = _dtBkgPkgs.AsEnumerable().ToList().Where
                        //(
                        //  pkg => Convert.ToInt32(pkg[_pkgIdColumn]) == _currentPkgId
                        //)
                        //.Select(itm => new
                        //{
                        //    PkgName = Convert.ToString(itm[_pkgNameColumn]),
                        //    SvcGrpName = Convert.ToString(itm[_svcGrpNameColumn]),
                        //    SvcGrpId = Convert.ToInt32(itm[_svcGrpIdColumn]),
                        //    BkgOrderPkgId = Convert.ToInt32(itm[_bkgOrderPkgIdColumn])
                        //}).ToList

                        foreach (var pkg in _lstPkgs)
                        {
                            _sharingPackage.PackageId = pkg.PackageId;
                            _sharingPackage.PackageName = pkg.PkgName;
                            _sharingPackage.IsCompliancePkg = false;
                            _sharingPackage.BkgOrderPkgId = pkg.BkgOrderPkgId;
                            // if (pkg.SvcGrpId > AppConsts.NONE)
                            if (pkg.SvcGrpId.IsNotNull())
                                _sharingPackage.BkgSvcGroups.Add(new BkgSvcGroup
                                {
                                    BSG_Name = pkg.SvcGrpName,
                                    //BSG_ID = pkg.SvcGrpId
                                    BSG_ID = pkg.SvcGrpId.Value,
                                    SvcGrpCompletionDate = pkg.SvcGrpCompletionDate
                                });
                        }
                        _lstPackages.Add(_sharingPackage);
                    }
                }
                return _lstPackages;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get the Requirement packages that can be shared by the admins, for Rotation sharing
        /// </summary>
        /// <param name="orgUserId"></param>
        /// <param name="rotationId"></param>
        /// <param name="reqPkgIds">CSV of the Requirement Packages selected for Sharing</param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<ProfileSharingRequirementPackage> GetSharingRequirementPackages(Int32 orgUserId, Int32 rotationId, Int32 tenantId)
        {
            try
            {
                var _ds = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetSharingRequirementPackages(orgUserId, rotationId);
                var _lstPackages = new List<ProfileSharingRequirementPackage>();

                var _pkgIdColumn = "RequirementPackageId";
                var _pkgNameColumn = "PackageName";

                if (_ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    var _pkgSubIdColumn = "PkgSubscriptionId";
                    var _pkgTypeCodeColumn = "PackageTypeCode";
                    var _catNameColumn = "CategoryName";
                    var _catIdColumn = "CategoryId";
                    var _dtCompliancePkgs = _ds.Tables[0];

                    DataView _dvCompliancePkg = new DataView(_dtCompliancePkgs);
                    DataTable _dt = _dvCompliancePkg.ToTable(true, _pkgSubIdColumn);

                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        var _sharingPackage = new ProfileSharingRequirementPackage();
                        _sharingPackage.RequirementPkgCategories = new List<RequirementCategory>();

                        var _currentPkgSubId = Convert.ToInt32(_dt.Rows[i][_pkgSubIdColumn]);

                        var _lstPkgs = _dtCompliancePkgs.AsEnumerable().ToList().Where
                        (
                          pkg => Convert.ToInt32(pkg[_pkgSubIdColumn]) == _currentPkgSubId
                        )
                        .Select(itm => new
                        {
                            PkgId = Convert.ToInt32(itm[_pkgIdColumn]),
                            PkgSubId = _currentPkgSubId,
                            PkgName = Convert.ToString(itm[_pkgNameColumn]),
                            CatName = Convert.ToString(itm[_catNameColumn]),
                            CatId = Convert.ToInt32(itm[_catIdColumn]),
                            PkgTypeCode = Convert.ToString(itm[_pkgTypeCodeColumn]),
                        }).ToList();

                        foreach (var pkg in _lstPkgs)
                        {
                            _sharingPackage.RequirementPackageId = pkg.PkgId;
                            _sharingPackage.PackageSubscriptionId = pkg.PkgSubId;
                            _sharingPackage.RequirementPackageName = pkg.PkgName;
                            _sharingPackage.PackageTypeCode = pkg.PkgTypeCode;

                            if (pkg.CatId > AppConsts.NONE && !_sharingPackage.RequirementPkgCategories.Any(rpc => rpc.RC_ID == pkg.CatId))
                            {
                                _sharingPackage.RequirementPkgCategories.Add(new RequirementCategory
                                {
                                    RC_CategoryName = pkg.CatName,
                                    RC_ID = pkg.CatId
                                });
                            }
                        }
                        _lstPackages.Add(_sharingPackage);
                    }
                }
                return _lstPackages;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }



        /// <summary>
        /// Get the Compliance & Background Packages (and their related data) for the
        /// selected applicants, out of which admin can select which category/service group etc can be shared - UAT 1324
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        public static Tuple<List<ProfileSharingPackages>, List<ProfileSharingRequirementPackage>> GetSharingPackageData(String organizationUserIds, Int32 tenantId)
        {
            try
            {

                var _ds = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetSharingPackageData(organizationUserIds);
                var _lstPackages = new List<ProfileSharingPackages>();

                var _pkgIdColumn = "PackageId";
                var _pkgNameColumn = "PackageName";

                if (_ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    var _catNameColumn = "CategoryName";
                    var _catIdColumn = "CategoryId";
                    var _dtCompliancePkgs = _ds.Tables[0];
                    DataView _dvCompliancePkg = new DataView(_dtCompliancePkgs);
                    DataTable _dt = _dvCompliancePkg.ToTable(true, _pkgIdColumn);

                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        #region Set Compliance Packages

                        var _sharingPackage = new ProfileSharingPackages();
                        _sharingPackage.CompliancePkgCategories = new List<ComplianceCategory>();

                        var _currentPkgId = Convert.ToInt32(_dt.Rows[i][_pkgIdColumn]);

                        var _lstPkgs = _dtCompliancePkgs.AsEnumerable().ToList().Where
                        (
                          pkg => Convert.ToInt32(pkg[_pkgIdColumn]) == _currentPkgId
                        )
                        .Select(itm => new
                        {
                            PkgId = Convert.ToInt32(itm[_pkgIdColumn]),
                            PkgName = Convert.ToString(itm[_pkgNameColumn]),
                            CatName = Convert.ToString(itm[_catNameColumn]),
                            CatId = Convert.ToInt32(itm[_catIdColumn])
                        }).ToList();

                        foreach (var pkg in _lstPkgs)
                        {
                            _sharingPackage.PackageId = pkg.PkgId;
                            _sharingPackage.PackageName = pkg.PkgName;
                            _sharingPackage.IsCompliancePkg = true;

                            if (pkg.CatId > AppConsts.NONE && !_sharingPackage.CompliancePkgCategories.Any(cpc => cpc.ComplianceCategoryID == pkg.CatId))
                            {
                                _sharingPackage.CompliancePkgCategories.Add(new ComplianceCategory
                                {
                                    CategoryName = pkg.CatName,
                                    ComplianceCategoryID = pkg.CatId
                                });
                            }
                        }

                        #endregion

                        _lstPackages.Add(_sharingPackage);
                    }
                }

                if (_ds.Tables.Count > 0 && _ds.Tables[1].Rows.Count > 0)
                {
                    Int32? nullValue = null;
                    var _svcGrpNameColumn = "SvcGroupName";
                    var _svcGrpIdColumn = "SvcGroupId";

                    var _dtBkgPkgs = _ds.Tables[1];

                    DataView _dvBkgPkg = new DataView(_dtBkgPkgs);
                    DataTable _dt = _dvBkgPkg.ToTable(true, _pkgIdColumn);

                    var bkgPkgs = _dtBkgPkgs.AsEnumerable().ToList();

                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        #region Set Background Packages

                        var _sharingPackage = new ProfileSharingPackages();
                        _sharingPackage.BkgSvcGroups = new List<BkgSvcGroup>();
                        var _currentBkgPackageID = Convert.ToInt32(_dt.Rows[i][_pkgIdColumn]);

                        var _lstPkgs = bkgPkgs.Where(
                                  bkgOrderPackage => Convert.ToInt32(bkgOrderPackage[_pkgIdColumn]) == _currentBkgPackageID
                              )
                              .Select(itm => new
                              {
                                  PkgName = Convert.ToString(itm[_pkgNameColumn]),
                                  SvcGrpName = Convert.ToString(itm[_svcGrpNameColumn]),
                                  SvcGrpId = itm[_svcGrpIdColumn].GetType().Name == "DBNull" ? nullValue : Convert.ToInt32(itm[_svcGrpIdColumn]),
                                  PackageId = Convert.ToInt32(itm[_pkgIdColumn])
                              }).ToList();


                        foreach (var pkg in _lstPkgs)
                        {
                            _sharingPackage.PackageId = pkg.PackageId;
                            _sharingPackage.PackageName = pkg.PkgName;
                            _sharingPackage.IsCompliancePkg = false;
                            if (pkg.SvcGrpId.IsNotNull())
                                _sharingPackage.BkgSvcGroups.Add(new BkgSvcGroup
                                {
                                    BSG_Name = pkg.SvcGrpName,
                                    BSG_ID = pkg.SvcGrpId.Value
                                });
                        }

                        #endregion

                        _lstPackages.Add(_sharingPackage);
                    }
                }

                List<ProfileSharingRequirementPackage> _lstRequirementPkgs = new List<ProfileSharingRequirementPackage>();
                //if (_ds.Tables.Count > 0 && _ds.Tables[2].Rows.Count > 0)
                //{
                //    var _pkgTypeCodeColumn = "PackageTypeCode";
                //    var _catNameColumn = "CategoryName";
                //    var _catIdColumn = "CategoryId";
                //    var _dtReqPkgs = _ds.Tables[2];

                //    DataView _dvCompliancePkg = new DataView(_dtReqPkgs);
                //    DataTable _dt = _dvCompliancePkg.ToTable(true, _pkgIdColumn);

                //    for (int i = 0; i < _dt.Rows.Count; i++)
                //    {
                //        #region Set Requirement Packages

                //        var _sharingPackage = new ProfileSharingRequirementPackage();
                //        _sharingPackage.RequirementPkgCategories = new List<RequirementCategory>();

                //        var _currentPkgId = Convert.ToInt32(_dt.Rows[i][_pkgIdColumn]);

                //        var _lstPkgs = _dtReqPkgs.AsEnumerable().ToList().Where
                //        (
                //          pkg => Convert.ToInt32(pkg[_pkgIdColumn]) == _currentPkgId
                //        )
                //        .Select(itm => new
                //        {
                //            PkgId = Convert.ToInt32(itm[_pkgIdColumn]),
                //            PkgName = Convert.ToString(itm[_pkgNameColumn]),
                //            CatName = Convert.ToString(itm[_catNameColumn]),
                //            CatId = Convert.ToInt32(itm[_catIdColumn]),
                //            PkgTypeCode = Convert.ToString(itm[_pkgTypeCodeColumn]),
                //        }).ToList();

                //        foreach (var pkg in _lstPkgs)
                //        {
                //            _sharingPackage.RequirementPackageId = pkg.PkgId;
                //            _sharingPackage.RequirementPackageName = pkg.PkgName;
                //            _sharingPackage.PackageTypeCode = pkg.PkgTypeCode;

                //            if (pkg.CatId > AppConsts.NONE && !_sharingPackage.RequirementPkgCategories.Any(rpc => rpc.RC_ID == pkg.CatId))
                //            {
                //                _sharingPackage.RequirementPkgCategories.Add(new RequirementCategory
                //                {
                //                    RC_CategoryName = pkg.CatName,
                //                    RC_ID = pkg.CatId
                //                });
                //            }
                //        }

                //        #endregion

                //        _lstRequirementPkgs.Add(_sharingPackage);
                //    }
                //}
                return new Tuple<List<ProfileSharingPackages>, List<ProfileSharingRequirementPackage>>(_lstPackages, _lstRequirementPkgs);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        /// <summary>
        /// Stored procedure to get the address of a User, by Address HandleId
        /// </summary>
        /// <param name="addressHandleId"></param>
        /// <returns></returns>
        public static AddressContract GetAddressByAddressHandleId(Guid addressHandleId, Int32 tenantId)
        {

            try
            {
                DataTable _dtAddresses = BALUtils.GetStoredProceduresRepoInstance(tenantId).GetAddressByAddressHandleId(addressHandleId);
                var _address = new AddressContract();
                if (_dtAddresses.Rows.Count > AppConsts.NONE)
                {
                    _address.Country = Convert.ToString(_dtAddresses.Rows[0]["CountryName"]);
                    _address.StateName = Convert.ToString(_dtAddresses.Rows[0]["FullStateName"]);
                    _address.CityName = Convert.ToString(_dtAddresses.Rows[0]["CityName"]);
                    _address.CountyName = Convert.ToString(_dtAddresses.Rows[0]["CountyName"]);
                    _address.Zipcode = Convert.ToString(_dtAddresses.Rows[0]["ZipCode"]);
                    _address.Address1 = Convert.ToString(_dtAddresses.Rows[0]["Address1"]);
                    _address.Address2 = Convert.ToString(_dtAddresses.Rows[0]["Address2"]);
                }
                return _address;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion
        #endregion

        #region Generic 'Execute' Stored Procedure

        /// <summary>
        ///  Geeneric method to execute any Stored Procedure 
        /// </summary>
        /// <param name="dicParameters"></param>
        /// <param name="procedureName"></param>
        public static void ExecuteProcedure(Dictionary<String, Object> dicParameters, String procedureName, Int32 tenantId)
        {
            try
            {
                BALUtils.GetStoredProceduresRepoInstance(tenantId).ExecuteProcedure(dicParameters, procedureName);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-1843
        public static List<Int32> GetBkgOrderIdByOrgUsers(List<Int32> orgUserIds, String ArchiveCode, Int32 tenantId)
        {
            try
            {
                Int32 ArchiveId = LookupManager.GetLookUpData<lkpArchiveState>(tenantId).FirstOrDefault(cond => cond.AS_Code == ArchiveCode).AS_ID;
                return BALUtils.GetStoredProceduresRepoInstance(tenantId).GetBkgOrderIdByOrgUsers(orgUserIds, ArchiveId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-3077

        public static Tuple<Int32, Int32, Int32> ApprovedPaymentItem(Int32 orderId, Int32 tenantId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetStoredProceduresRepoInstance(tenantId).ApprovedPaymentItem(orderId, currentLoggedInUserId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion
    }

}



