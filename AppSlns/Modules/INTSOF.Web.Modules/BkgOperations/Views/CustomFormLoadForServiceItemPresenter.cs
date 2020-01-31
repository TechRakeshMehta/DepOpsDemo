using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using INTSOF.UI.Contract.BkgOperations;
using System.Xml.Linq;
using Entity.ClientEntity;
using INTSOF.ServiceUtil;
using System.Web;
using INTSOF.Contracts;

namespace CoreWeb.BkgOperations.Views
{
    public class CustomFormLoadForServiceItemPresenter : Presenter<ICustomFormLoadForServiceItemView>
    {
        //public void GetListOfCustomFormsForSelectedItem()
        //{
        //    String serviceItemIds = GetCommaSeperatedString();
        //    View.lstSupplementServiceCustomFormList = BackgroundProcessOrderManager.GetListOfCustomFormsForSelectedItem(View.SelectedTenantID, serviceItemIds);
        //}

        public void GetListOfCustomFormsForSelectedServices()
        {
            String supplementalServiceIds = GetCommaSeperatedStringOfSelectedSupplementServices();
            if (!supplementalServiceIds.IsNullOrEmpty())
            {
                View.lstSupplementServiceCustomFormList = BackgroundProcessOrderManager.GetListOfCustomFormsForSelectedServices(View.SelectedTenantID, supplementalServiceIds);
                View.LstDistinctCustomFormId = View.lstSupplementServiceCustomFormList.DistinctBy(col => col.CustomFormID).Select(x => x.CustomFormID).ToList();
            }
            else
            {
                View.lstSupplementServiceCustomFormList = new List<SupplementServiceItemCustomForm>();
                View.LstDistinctCustomFormId = new List<Int32>();
            }
        }

        public void GetListOfAttributesForSelectedItem(Int32 customFormId, Int32 serviceItemId)
        {
            View.lstCustomFormAttributes = BackgroundProcessOrderManager.GetListOfAttributesForSelectedItem(View.SelectedTenantID, customFormId, serviceItemId);
        }

        //private String GetCommaSeperatedString()
        //{
        //    String serviceItems = String.Empty;
        //    if (!View.SelectedServiceItem.IsNullOrEmpty())
        //    {
        //        View.SelectedServiceItem.ForEach(x => serviceItems += Convert.ToString(x) + ",");
        //        //packages = "4";
        //        if (serviceItems.EndsWith(","))
        //            serviceItems = serviceItems.Substring(0, serviceItems.Length - 1);
        //    }
        //    return serviceItems;
        //}

        private String GetCommaSeperatedStringOfSelectedSupplementServices()
        {
            String supplementServices = String.Empty;
            if (!View.SelectedSupplementServices.IsNullOrEmpty())
            {
                View.SelectedSupplementServices.ForEach(x => supplementServices += Convert.ToString(x) + ",");
                //packages = "4";
                if (supplementServices.EndsWith(","))
                    supplementServices = supplementServices.Substring(0, supplementServices.Length - 1);
            }
            return supplementServices;
        }

        #region UAT-586 WB: AMS: When adding supplemental services to an order, need to be able to see what the applicant has already entered (location, alias, etc.)
        public void GetAttributeDataListForPreExistingSupplement(Int32 groupId, Int32 masterOrderId, Int32 serviceItemId, Int32 serviceId)
        {
            View.lstPreExitingSupplementAttributes = BackgroundProcessOrderManager.GetAttributeDataListForPreExistingSupplement(View.SelectedTenantID, groupId, masterOrderId, serviceItemId, serviceId);
        }
        #endregion

        /// <summary>
        /// Gets the Supplemental Service for a particular OrderPackageServiceGroup
        /// </summary>
        public void GetSupplementServices()
        {
            View.lstSupplementServiceList = BackgroundProcessOrderManager.GetSupplementServices(View.MasterOrderID, View.SelectedTenantID);
        }

        #region UAT-2249
        public List<Entity.ClientEntity.BkgAttributeGroupMapping> GetAllBkgAttributeGroupMapping(Int32 tenantId)
        {
            return BackgroundProcessOrderManager.GetAllBkgAttributeGroupMapping(tenantId);
        }
        /// <summary>
        /// Generate Supplment order line items for a Service Group
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="orderId"></param>
        /// <param name="inputXML"></param>
        public Boolean GenerateSupplementOrder(Int32 tenantId, Int32 currentUserId, Int32 orderId, String inputXML, Int32 ordPkgSvcGroupId, List<Int32> lstPackageServiceIds)
        {
            SupplementOrderContract _supplementOrdContract = new SupplementOrderContract();
            _supplementOrdContract.CreatedById = currentUserId;
            _supplementOrdContract.OrderId = orderId;
            String _pricingXML = StoredProcedureManagers.GetSupplementOrderPricingData(inputXML, tenantId);

            List<SupplementOrderServices> _lstOutput = ParseOutputXML(_pricingXML);

            bool checkIfLinetItemsExists = false;
            _lstOutput.ForEach(oli =>
            {
                if (!oli.lstOrderLineItems.IsNullOrEmpty())
                {
                    checkIfLinetItemsExists = true;
                }
            });


            if (checkIfLinetItemsExists) // if any line item is created
            {
                SupplementOrderContract _supplementOrderContract = new SupplementOrderContract
                {
                    OrderId = orderId,
                    CreatedById = currentUserId,
                    lstSupplementOrderData = _lstOutput,
                    OrderPkgSvcGroupId = ordPkgSvcGroupId
                };
                BackgroundProcessOrderManager.GenerateSupplementOrder(_supplementOrderContract, tenantId, lstPackageServiceIds);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Parse the output received from the Supplement order pricing stored procedure
        /// </summary>
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
        /// <summary>
        /// Method to check and apply success indicator and update the order status.
        /// </summary>
        /// <param name="masterOrderID">MatserOrderID</param>
        /// <param name="currentLoggedInUserID">Current Logged In user</param>
        /// <returns>Boolean</returns>
        public Boolean CheckOrderToUpdate(Int32 masterOrderID, Int32 currentLoggedInUserID)
        {
            Dictionary<String, String> resultDic = new Dictionary<String, String>();
            Int32 institutionColorFlagID = AppConsts.NONE;
            Int32 orderStatusTypeID_Completed = AppConsts.NONE;
            String orderStatusTypeCode_Completed = OrderStatusType.COMPLETED.GetStringValue();
            Boolean saveStatus = false;

            resultDic = BackgroundProcessOrderManager.CheckBackgroundOrderToUpdate(View.SelectedTenantID, masterOrderID, View.OrderPackageSvcGroupID);

            if (!resultDic.IsNullOrEmpty() && resultDic.ContainsKey(AppConsts.IS_SUCCESS_INDICATOR_APPLICABLE) && resultDic.ContainsKey(AppConsts.IS_ALL_EXISTING_SEARCHES_ARE_CLEAR))
            {
                View.IsSuccessIndicatorApplicable = Convert.ToBoolean(resultDic[AppConsts.IS_SUCCESS_INDICATOR_APPLICABLE]);
                View.IsAllExistingSearchesAreClear = Convert.ToBoolean(resultDic[AppConsts.IS_ALL_EXISTING_SEARCHES_ARE_CLEAR]);
                institutionColorFlagID = Convert.ToInt32(resultDic[AppConsts.INSTITUTION_COLOR_FLAG_ID]);
                View.IsOtherServiceGroupsAreCompleted = Convert.ToBoolean(resultDic[AppConsts.IS_OTHER_SERVICE_GROUPS_ARE_COMPLETED]);
            }
            orderStatusTypeID_Completed = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatusType>(View.SelectedTenantID).FirstOrDefault(x => x.Code == orderStatusTypeCode_Completed).OrderStatusTypeID;
            if (View.IsOtherServiceGroupsAreCompleted)
            {
                if (View.IsSuccessIndicatorApplicable && View.IsAllExistingSearchesAreClear)
                {
                    if (BackgroundProcessOrderManager.UpdateOrderStatus(View.SelectedTenantID, institutionColorFlagID, masterOrderID, orderStatusTypeID_Completed, currentLoggedInUserID
                                                 , AppConsts.NONE, null))
                    {
                        saveStatus = true;
                        CopyDataParallelTask(masterOrderID, currentLoggedInUserID);
                    }
                }
            }
            return saveStatus;
        }
        public void CopyDataParallelTask(Int32 masterOrderID, Int32 currentLoggedInUserID)
        {

            BkgOrder bkgOrder = BackgroundProcessOrderManager.GetBkgOrderByOrderID(View.SelectedTenantID, masterOrderID);

            Dictionary<String, Object> dataDict = new Dictionary<String, Object>();
            dataDict.Add("packageSubscriptionID", -1);
            dataDict.Add("tenantId", View.SelectedTenantID);
            dataDict.Add("CurrentLoggedInUserId", currentLoggedInUserID);
            dataDict.Add("BkgOrderID", bkgOrder.BOR_ID);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
            ParallelTaskContext.PerformParallelTask(CopyData, dataDict, LoggerService, ExceptiomService);
        }
        private void CopyData(Dictionary<String, Object> data)
        {
            Int32 packageSubscriptionID = Convert.ToInt32(data.GetValue("packageSubscriptionID"));
            Int32 tenantId = Convert.ToInt32(data.GetValue("tenantId"));
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("CurrentLoggedInUserId"));
            Int32 bkgOrderID = Convert.ToInt32(data.GetValue("BkgOrderID"));
            ComplianceDataManager.CopyData(packageSubscriptionID, tenantId, currentLoggedInUserId, bkgOrderID);
        }
        #endregion
    }
}
