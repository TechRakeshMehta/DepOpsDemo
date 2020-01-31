using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using System.Xml.Linq;

namespace CoreWeb.BkgOperations.Views
{
    public class ApplicantDataPresenter : Presenter<IApplicantDataView>
    {
        /// <summary>
        /// Gets Applicant Residential Histories & Personal Alias to display in Supplement Order, added during normal order
        /// </summary>
        /// <returns></returns>
        public Tuple<List<SupplementOrderApplicantResidentialHistoryContract>, List<SupplementOrderApplicantPersonAliasContract>> GetApplicantBkgOrderDeta()
        {
            return StoredProcedureManagers.GetApplicantBkgOrderDeta(View.MasterOrderId, View.TenantId);
        }

        /// <summary>
        /// Gets the SSN and Nationwide Criminal Search Results
        /// </summary>
        /// <returns></returns>
        public SourceServiceDetailForSupplement CheckSourceServicesForSupplement()
        {
            return BackgroundProcessOrderManager.CheckSourceServicesForSupplement(View.TenantId, View.MasterOrderId);
        }

        #region UAT-2062:System to determine and add additional searches in supplement (SSN Trace)
        /// <summary>
        /// Method to convert Additional search data into XML
        /// </summary>
        /// <returns></returns>
        private String GenerateAdditionalSearchXML()
        {
            var tempSSNResultList = View.lstSSNResultForAdditionalSearch.Where(x => x.IsUsedForSearch && x.IsExistInLastSevenYear);
            if (!tempSSNResultList.IsNullOrEmpty())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<SearchDatas>");
                tempSSNResultList.ToList().ForEach(cnd =>
                {
                    sb.Append("<SearchData>");
                    sb.Append("<FirstName>" + cnd.FirstName + "</FirstName>");
                    sb.Append("<LastName>" + cnd.LastName + "</LastName>");
                    sb.Append("<MiddleName>" + cnd.MiddleName + "</MiddleName>");
                    sb.Append("<County>" + cnd.CountyName + "</County>");
                    sb.Append("<StateAbbreviation>" + cnd.StateAbbreviation + "</StateAbbreviation>");
                    sb.Append("<IsUsedForSearch>" + cnd.IsUsedForSearch + "</IsUsedForSearch>");
                    sb.Append("<IsExistInLastSevenYear>" + cnd.IsExistInLastSevenYear + "</IsExistInLastSevenYear>");
                    sb.Append("</SearchData>");
                });
                sb.Append("</SearchDatas>");
                return sb.ToString();
            }
            return null;
        }

        /// <summary>
        /// Get Matched Additional Search Data
        /// </summary>
        public void GetMatchedAdditionalSearchData()
        {
            if (!View.lstSSNResultForAdditionalSearch.IsNullOrEmpty())
            {
                String inputXml = GenerateAdditionalSearchXML();
                if (!inputXml.IsNullOrEmpty())
                {
                    List<SupplementAdditionalSearchContract> lstMatchedAdditionalSearchData = new List<SupplementAdditionalSearchContract>();
                    lstMatchedAdditionalSearchData = BackgroundProcessOrderManager.GetMatchedAdditionalSearchData(View.TenantId, inputXml, View.MasterOrderId);

                    if (!lstMatchedAdditionalSearchData.IsNullOrEmpty())
                    {
                        View.lstMatchedNameForAdditionalSearch = lstMatchedAdditionalSearchData.Where(cnd => cnd.IsNameUsedForSearch)
                                                                                               .DistinctBy(dst => new { dst.FirstName, dst.LastName }).ToList();

                        View.lstMatchedLocationForAdditionalSearch = lstMatchedAdditionalSearchData.Where(cnd => cnd.IsLocationUsedForSearch)
                                                                                               .DistinctBy(dst => new { dst.StateAbbreviation, dst.CountyName }).ToList();

                    }
                }
            }
        }

        #endregion

        #region UAT-2114: Dont show additional searches if line items will not be created.

        /// <summary>
        /// Get Matched Additional Filtered Search Data
        /// </summary>
        /// <param name="_lstSupplementOrderPricing"></param>
        /// <param name="attributeGroupMappingIdForState"></param>
        /// <param name="attributeGroupMappingIdForCounty"></param>
        public void FilterLocationSearchData(List<SupplementOrderServices> _lstSupplementOrderPricing, Int32 attributeGroupMappingIdForState, Int32 attributeGroupMappingIdForCounty)
        {
            if (!View.lstSSNResultForAdditionalSearch.IsNullOrEmpty())
            {
                String inputXml = GenerateAdditionalSearchXML();
                if (!inputXml.IsNullOrEmpty())
                {
                    List<SupplementAdditionalSearchContract> lstMatchedAdditionalSearchData = new List<SupplementAdditionalSearchContract>();
                    lstMatchedAdditionalSearchData = BackgroundProcessOrderManager.GetMatchedAdditionalSearchData(View.TenantId, inputXml, View.MasterOrderId);

                    if (!lstMatchedAdditionalSearchData.IsNullOrEmpty())
                    {
                        View.lstMatchedNameForAdditionalSearch = lstMatchedAdditionalSearchData.Where(cnd => cnd.IsNameUsedForSearch)
                                                                                               .DistinctBy(dst => new { dst.FirstName, dst.LastName }).ToList();

                        var lstMatchedLocationForAdditionalSearch = lstMatchedAdditionalSearchData.Where(cnd => cnd.IsLocationUsedForSearch)
                                                                                               .DistinctBy(dst => new { dst.StateAbbreviation, dst.CountyName }).ToList();

                        List<SupplementAdditionalSearchContract> lstMatchedLocationForAdditionalSearchFinal = new List<SupplementAdditionalSearchContract>();
                        lstMatchedLocationForAdditionalSearchFinal.AddRange(lstMatchedLocationForAdditionalSearch);
                        //Dont show additional searches if line items will not be created.
                        List<AttributesForCustomFormContract> lstAttributeValueData = new List<AttributesForCustomFormContract>();
                        Int32 instanceId = 1;
                        foreach (var supplementOrderPricing in _lstSupplementOrderPricing)
                        {
                            foreach (var orderLineItem in supplementOrderPricing.lstOrderLineItems)
                            {
                                foreach (var bkgSvcAttributeDataGroup in orderLineItem.lstBkgSvcAttributeDataGroup)
                                {
                                    foreach (var attributeData in bkgSvcAttributeDataGroup.lstAttributeData.Where(x => x.AttributeGroupMappingID == attributeGroupMappingIdForState || x.AttributeGroupMappingID == attributeGroupMappingIdForCounty))
                                    {
                                        AttributesForCustomFormContract attributeValueData = new AttributesForCustomFormContract();
                                        attributeValueData.AtrributeGroupMappingId = attributeData.AttributeGroupMappingID;
                                        attributeValueData.AttributeDataValue = attributeData.AttributeValue.ToLower();
                                        attributeValueData.InstanceID = instanceId;
                                        lstAttributeValueData.Add(attributeValueData);
                                    }
                                    instanceId++;
                                }
                            }
                        }
                        var groupedAttributeValueStateList = lstAttributeValueData
                                                             .GroupBy(x => x.InstanceID)
                                                             .Where(grp => grp.Count() == 1)
                                                             .Select(grp => grp.ToList())
                                                             .ToList();
                        var groupedAttributeValueCountyList = lstAttributeValueData
                                                              .GroupBy(x => x.InstanceID)
                                                              .Where(grp => grp.Count() > 1)
                                                              .Select(grp => grp.ToList())
                                                              .ToList();

                        List<AttributesForCustomFormContract> stateList = new List<AttributesForCustomFormContract>();
                        List<AttributesForCustomFormContract> countyList = new List<AttributesForCustomFormContract>();

                        foreach (var groupedState in groupedAttributeValueStateList)
                        {
                            foreach (var state in groupedState)
                            {
                                stateList.Add(state);
                            }
                        }
                        foreach (var groupedCounty in groupedAttributeValueCountyList)
                        {
                            foreach (var county in groupedCounty)
                            {
                                countyList.Add(county);
                            }
                        }

                        //UAT-2248
                        var countySearchStates = countyList.Where(y => y.AtrributeGroupMappingId == attributeGroupMappingIdForState).Select(slct => slct.AttributeDataValue).ToList();
                        var stateSearchList = stateList.Where(x => !countySearchStates.Contains(x.AttributeDataValue)).ToList();

                        var lstState = lstMatchedLocationForAdditionalSearch.Select(x => x.StateName.ToLower()).Distinct().ToList();

                        foreach (var state in lstState)
                        {
                            //UAT-2248:System created additional searches should only add lines for what will be created searches
                            if (stateSearchList.Select(slct => slct.AttributeDataValue).Contains(state))
                            {
                                Int32 index = AppConsts.NONE;
                                foreach (var matchedLocationForAdditionalSearch in lstMatchedLocationForAdditionalSearch.Where(x => x.StateName.ToLower() == state).ToList())
                                {
                                    if (index != AppConsts.NONE)
                                        lstMatchedLocationForAdditionalSearchFinal.Remove(matchedLocationForAdditionalSearch);
                                    index = index + 1;
                                }
                            }
                            else
                            {
                                Boolean isStateExist = false;
                                foreach (var attributeValueData in lstAttributeValueData.Where(x => x.AttributeDataValue == state))
                                {
                                    isStateExist = true;

                                    if (!stateList.Contains(attributeValueData) && countyList.Contains(attributeValueData))
                                    {
                                        List<Int32> instanceIDs = countyList.Where(x => x.AttributeDataValue == state).Select(x => x.InstanceID).ToList();
                                        List<String> lstCounty = new List<String>();

                                        foreach (var instanceID in instanceIDs)
                                        {
                                            var countyName = countyList.FirstOrDefault(x => x.InstanceID == instanceID && x.AtrributeGroupMappingId == attributeGroupMappingIdForCounty).AttributeDataValue;
                                            lstCounty.Add(countyName);
                                        }
                                        //if counties do not match then remove them from additional search for a state
                                        //means Dont show additional searches if line items will not be created for a state and its counties.
                                        foreach (var matchedLocationForAdditionalSearch in lstMatchedLocationForAdditionalSearch.Where(x => x.StateName.ToLower() == state && !lstCounty.Contains(x.CountyName.ToLower())).ToList())
                                        {
                                            //matchedLocationForAdditionalSearch.IsLocationUsedForSearch = false;
                                            lstMatchedLocationForAdditionalSearchFinal.Remove(matchedLocationForAdditionalSearch);
                                        }
                                    }
                                    break;
                                }
                                //If any state does not exist then remove from additional search i.e. line items will not be created for that state 
                                if (!isStateExist)
                                {
                                    foreach (var matchedLocationForAdditionalSearch in lstMatchedLocationForAdditionalSearch.Where(x => x.StateName.ToLower() == state).ToList())
                                    {
                                        //matchedLocationForAdditionalSearch.IsLocationUsedForSearch = false;
                                        lstMatchedLocationForAdditionalSearchFinal.Remove(matchedLocationForAdditionalSearch);
                                    }
                                }
                            }

                            View.lstMatchedLocationForAdditionalSearch = lstMatchedLocationForAdditionalSearchFinal;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Get All Bkg Attribute Group Mapping
        /// </summary>
        /// <returns></returns>
        public List<Entity.ClientEntity.BkgAttributeGroupMapping> GetAllBkgAttributeGroupMapping()
        {
            return BackgroundProcessOrderManager.GetAllBkgAttributeGroupMapping(View.TenantId);
        }

        /// <summary>
        /// Get Supplement Order Pricing Data
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="inputXML"></param>
        /// <param name="ordPkgSvcGroupId"></param>
        /// <param name="lstPackageServiceIds"></param>
        /// <returns></returns>
        public List<SupplementOrderServices> GetSupplementOrderPricingData(Int32 currentUserId, String inputXML, Int32 ordPkgSvcGroupId, List<Int32> lstPackageServiceIds)
        {
            SupplementOrderContract _supplementOrdContract = new SupplementOrderContract();
            _supplementOrdContract.CreatedById = currentUserId;
            _supplementOrdContract.OrderId = View.MasterOrderId;
            String _pricingXML = StoredProcedureManagers.GetSupplementOrderPricingData(inputXML, View.TenantId);
            return ParseOutputXML(_pricingXML);
        }

        /// <summary>
        /// Parse the output received from the Supplement order pricing stored procedure
        /// </summary>
        /// <param name="outputXML"></param>
        /// <returns></returns>
        private List<SupplementOrderServices> ParseOutputXML(String outputXML)
        {
            XDocument _docToParse = XDocument.Parse(outputXML);

            // GET <Service> TAG'S INSIDE <Services> TAG
            var _services = _docToParse.Root.Descendants("Services")
                               .Descendants("Service")
                               .Select(element => element)
                               .ToList();

            List<SupplementOrderServices> _lstData = new List<SupplementOrderServices>();
            foreach (var _service in _services)
            {

                Int32 _serviceId = Convert.ToInt32(_service.Element("ServiceID").Value);
                SupplementOrderServices _supplementService = new SupplementOrderServices();
                //_supplementService.ServiceId = _serviceId;

                #region ADD DATA OF <OrderLineItem> TAG'S INSIDE <OrderLineItems> TAG

                var _orderLineItems = _service.Descendants("OrderLineItems").Descendants("OrderLineItem")
                                         .Select(element => element)
                                         .ToList();

                _supplementService.lstOrderLineItems = new List<SupplementOrderOrderLineItem_PricingData>();

                foreach (var _ordLineItem in _orderLineItems)
                {
                    SupplementOrderOrderLineItem_PricingData _orderLineItem = new SupplementOrderOrderLineItem_PricingData();
                    _orderLineItem.PackageOrderItemPriceId = String.IsNullOrEmpty(_ordLineItem.Element("PackageOrderItemPriceID").Value)
                                                            ? AppConsts.NONE
                                                            : Convert.ToInt32(_ordLineItem.Element("PackageOrderItemPriceID").Value);

                    _orderLineItem.PackageSvcGrpID = String.IsNullOrEmpty(_ordLineItem.Element("PackageSvcGrpID").Value)
                                                           ? AppConsts.NONE
                                                           : Convert.ToInt32(_ordLineItem.Element("PackageSvcGrpID").Value);

                    _orderLineItem.PackageServiceItemId = Convert.ToInt32(_ordLineItem.Element("PackageServiceItemID").Value);
                    _orderLineItem.Price = String.IsNullOrEmpty(_ordLineItem.Element("Price").Value)
                                           ? AppConsts.NONE
                                           : Convert.ToDecimal(_ordLineItem.Element("Price").Value);

                    _orderLineItem.TotalPrice = String.IsNullOrEmpty(_ordLineItem.Element("TotalPrice").Value)
                                                ? AppConsts.NONE
                                                : Convert.ToDecimal(_ordLineItem.Element("TotalPrice").Value);

                    _orderLineItem.PriceDescription = _ordLineItem.Element("PriceDescription").Value;
                    _orderLineItem.LineItemDescription = _ordLineItem.Element("Description").Value;

                    #region ADD DATA OF <Fee> TAG'S INSIDE  <Fees> TAG

                    var _fees = _ordLineItem.Descendants("Fees").Descendants("Fee")
                                                 .Select(element => element)
                                                 .ToList();

                    _orderLineItem.lstFees = new List<SupplementOrderFee_PricingData>();
                    foreach (var _fee in _fees)
                    {
                        _orderLineItem.lstFees.Add(new SupplementOrderFee_PricingData
                        {
                            Amount = String.IsNullOrEmpty(_fee.Element("Amount").Value) ? AppConsts.NONE : Convert.ToDecimal(_fee.Element("Amount").Value),
                            Description = _fee.Element("Description").Value,
                            PackageOrderItemFeeId = String.IsNullOrEmpty(_fee.Element("PackageOrderItemFeeID").Value)
                                                    ? AppConsts.NONE
                                                    : Convert.ToInt32(_fee.Element("PackageOrderItemFeeID").Value)

                        });
                    }

                    #endregion

                    #region ADD DATA OF <BkgSvcAttributeDataGroup> TAG

                    var _bkgAttrDataGrps = _ordLineItem.Descendants("BkgSvcAttributeDataGroup")
                                                                   .Select(element => element)
                                                                   .ToList();

                    _orderLineItem.lstBkgSvcAttributeDataGroup = new List<SupplementOrderBkgSvcAttributeDataGroup_PricingData>();
                    foreach (var _bkgAttrDataGrp in _bkgAttrDataGrps)
                    {
                        String _instanceId = _bkgAttrDataGrp.Element("InstanceID").Value;

                        SupplementOrderBkgSvcAttributeDataGroup_PricingData _bkgSvcAttrDataGrpPricingData = new SupplementOrderBkgSvcAttributeDataGroup_PricingData
                        {
                            AttributeGroupId = Convert.ToInt32(_bkgAttrDataGrp.Element("AttributeGroupID").Value),
                            InstanceId = _instanceId
                        };

                        //if (String.IsNullOrEmpty(_instanceId))
                        var _attributeData = _bkgAttrDataGrp.Descendants("BkgSvcAttributes").Descendants("BkgSvcAttributeData")
                                                      .Select(element => element)
                                                      .ToList();

                        _bkgSvcAttrDataGrpPricingData.lstAttributeData = new List<SupplementOrderAttributeData_PricingData>();
                        foreach (var _attrData in _attributeData)
                        {
                            #region ADD DATA OF BkgSvcAttributeData TAG

                            String _attributeGrpMappingId = _attrData.Element("AttributeGroupMapingID").Value;

                            if (!String.IsNullOrEmpty(_attributeGrpMappingId))
                            {
                                _bkgSvcAttrDataGrpPricingData.lstAttributeData.Add(new SupplementOrderAttributeData_PricingData
                                {
                                    AttributeGroupMappingID = Convert.ToInt32(_attributeGrpMappingId),
                                    AttributeValue = _attrData.Element("Value").Value
                                });
                            }

                            #endregion
                        }

                        _orderLineItem.lstBkgSvcAttributeDataGroup.Add(_bkgSvcAttrDataGrpPricingData);
                    }
                    #endregion

                    _supplementService.lstOrderLineItems.Add(_orderLineItem);
                }
                #endregion

                _lstData.Add(_supplementService);
            }
            return _lstData;
        }

        #endregion
    }
}
