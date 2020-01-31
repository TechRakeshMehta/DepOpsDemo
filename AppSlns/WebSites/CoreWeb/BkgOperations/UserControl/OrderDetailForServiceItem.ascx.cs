using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using System.Web.Configuration;

namespace CoreWeb.BkgOperations.Views
{
    public partial class OrderDetailForServiceItem : BaseUserControl, IOrderDetailForServiceItemView
    {
        #region Variables

        #region Private Variables
        OrderDetailForServiceItemPresenter _presenter = new OrderDetailForServiceItemPresenter();

        SupplementOrderCart supplementOrderCart = null;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties
        #region Private Properties

        #region UAT-2117:"Continue" button behavior
        private String ResultMessage { get; set; }
        private String ResultMessageType { get; set; }
        #endregion

        #region UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
        private String NoMiddleNameText
        {
            get
            {
                String noMiddleNameText = WebConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY];
                if (noMiddleNameText.IsNull())
                {
                    noMiddleNameText = String.Empty;
                }
                return noMiddleNameText;
            }
        }
        #endregion
        #endregion

        #region Public Properties
        public IOrderDetailForServiceItemView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<SupplementServiceItemCustomForm> lstSupplementServiceCustomFormList
        {
            get
            {
                //New Change 21072016
                if (supplementOrderCart.IsNullOrEmpty())
                {
                    supplementOrderCart = (SupplementOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART);
                }
                if (!supplementOrderCart.IsNullOrEmpty() && !supplementOrderCart.lstCustomFormLst.IsNullOrEmpty())
                {
                    return supplementOrderCart.lstCustomFormLst;
                }
                return new List<SupplementServiceItemCustomForm>();
            }
        }

        public List<SupplementOrderData> lstSupplementOrderData
        {
            get
            {
                if (supplementOrderCart.IsNullOrEmpty())
                {
                    supplementOrderCart = (SupplementOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART);
                }
                if (!supplementOrderCart.IsNullOrEmpty() && !supplementOrderCart.lstSupplementOrderData.IsNullOrEmpty())
                {
                    return supplementOrderCart.lstSupplementOrderData;
                }
                return new List<SupplementOrderData>();
            }
        }

        public Int32 SelectedTenantID
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID).IsNotNull())
                    return (Int32)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID);
                return 0;
            }
        }

        public Int32 MasterOrderID
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID).IsNotNull())
                    return (Int32)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID);
                return 0;
            }
        }

        public String MasterOrderNumber
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_NUMBER).IsNotNull())
                    return (String)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_NUMBER);
                return String.Empty;
            }
        }

        public List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }
        public OrderDetailForServiceItemPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }

        }

        #region UAT-2117:"Continue" button behavior
        Boolean IOrderDetailForServiceItemView.IsSuccessIndicatorApplicable { get; set; }
        Boolean IOrderDetailForServiceItemView.IsAllExistingSearchesAreClear { get; set; }
        #endregion

        #region UAT-2200:Should send back to queue if everything in the service group is complete and clear and other service group(s) are in progress
        Int32 IOrderDetailForServiceItemView.OrderPackageSvcGroupID
        {
            get;
            set;
        }

        Boolean IOrderDetailForServiceItemView.IsOtherServiceGroupsAreCompleted { get; set; }
        #endregion
        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageTitle("Supplement Order");

            ucApplicantDetails.MasterOrderId = this.MasterOrderID;
            ucApplicantDetails.TenantId = this.SelectedTenantID;
            ucApplicantDetails.MasterOrderNumber = this.MasterOrderNumber;

            if (!IsPostBack)
            {
                createCustomForm();
                ucApplicantData.MasterOrderId = this.MasterOrderID;
                ucApplicantData.TenantId = this.SelectedTenantID;
                ucApplicantDetails.MasterOrderNumber = this.MasterOrderNumber;
                //UAT-2062:
                ucApplicantData.IsShowUnIdentifiedSSNResultMessage = true;
            }
            //UAT-2114: Dont show additional searches if line items will not be created.
            ucApplicantData.ParentScreenName = AppConsts.ORDER_DETAIL_FOR_SERVICE_ITEM_SUPPLEMENT;
        }

        #endregion

        #region Button Events

        protected void CmdBarRestart_Click(object sender, EventArgs e)
        {

            //New Change 21072016
            supplementOrderCart = (SupplementOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART);
            var _opsgId = supplementOrderCart.OrdPkgSvcGroupId;
            var _parentScreen = supplementOrderCart.ParentScreen;
            supplementOrderCart = null;
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART, supplementOrderCart);
            String _viewType = String.Empty;
            Dictionary<String, String> queryString = new Dictionary<String, String>();

            if (_opsgId > 0)
            {
                queryString = new Dictionary<String, String>
                                                                { 
                                                                     { "SelectedTenantId", Convert.ToString(SelectedTenantID)},                                                      
                                                                     { "OrderId", Convert.ToString(MasterOrderID)},
                                                                     { AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, Convert.ToString(_opsgId)},
                                                                     { AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, _parentScreen}
                                                                 };
            }
            else
            {
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(SelectedTenantID) },                                                      
                                                                    { "OrderId", Convert.ToString(MasterOrderID)}
                                                                 };
            }
            Response.Redirect(String.Format("~/BkgOperations/Pages/BkgOrderDetailPage.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString()), true);
        }

        protected void CmdBarSave_Click(object sender, EventArgs e)
        {
            String msgText = String.Empty;
            String msgType = String.Empty;
            try
            {
                supplementOrderCart = (SupplementOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART);
                //UAT-2065:Additional searches should go in one grid for each type (not one for each additional search) with ability to delete
                DeleteReadOnlyInstanceAndGroupFromCart(supplementOrderCart);
                //UAT-2200:Should send back to queue if everything in the service group is complete and clear and other service group(s) are in progress
                CurrentViewContext.OrderPackageSvcGroupID = supplementOrderCart.OrdPkgSvcGroupId;

                List<Entity.ClientEntity.BkgAttributeGroupMapping> lstBkgAttributeGroupMapping = Presenter.GetAllBkgAttributeGroupMapping(this.SelectedTenantID);
                Guid stateAliasGuid = new Guid("CAEAC9FA-FFF4-4F7A-A644-96967F399362");
                Guid countryGuid = new Guid("37B6B708-C691-4568-B604-6F70F24BC839");
                Guid countyGuid = new Guid("C00AEFB5-37DF-44F7-A050-D2C9581909DE");
                //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                Guid aliasNameGuid = new Guid("56258C54-C2BC-4514-94E1-2EF2EFFFDBF5");
                Int32 attributeGroupMappingIdForState = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == stateAliasGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;
                Int32 attributeGroupMappingIdForCountry = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == countryGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;
                Int32 attributeGroupMappingIdForCounty = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == countyGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;
                Int32 attributeGroupMappingIdForAlias = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == aliasNameGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;

                XmlDocument _doc = new XmlDocument();
                XmlElement _rootNode = (XmlElement)_doc.AppendChild(_doc.CreateElement("BkgOrderAddition"));

                List<Int32> _lstPackageServiceIds = lstSupplementServiceCustomFormList
                                                .DistinctBy(svc => svc.PackageServiceId)
                                                .Select(svcId => svcId.PackageServiceId).ToList();

                _rootNode.AppendChild(_doc.CreateElement("MasterOrderID")).InnerText = Convert.ToString(supplementOrderCart.MasterOrderId);

                XmlNode _packageServiceNode = _rootNode.AppendChild(_doc.CreateElement("PackageService"));
                foreach (var _packageServiceId in _lstPackageServiceIds)
                {
                    _packageServiceNode.AppendChild(_doc.CreateElement("PackageSvcID")).InnerText = Convert.ToString(_packageServiceId);
                }

                List<SupplementServiceItemCustomForm> lstCustomForms = new List<SupplementServiceItemCustomForm>();
                lstCustomForms = lstSupplementServiceCustomFormList.DistinctBy(x => x.CustomFormID).Select(col => col).ToList();
                foreach (SupplementServiceItemCustomForm customForm in lstCustomForms)
                {
                    List<SupplementOrderData> _lstSupplementOrderData = supplementOrderCart.lstSupplementOrderData
                                                                        .Where(sod => sod.BkgServiceId == customForm.ServiceId)
                                                                        .ToList();
                    List<SupplementOrderData> _lstBkgAttributeDataGrps = _lstSupplementOrderData
                                                                            .Where(svc => svc.PackageSvcItemId == customForm.ServiceItemID
                                                                                  && svc.BkgServiceId == customForm.ServiceId)
                                                                            .ToList();
                    foreach (var _attrGrp in _lstBkgAttributeDataGrps)
                    {
                        //if (!_attrGrp.FormData.IsNullOrEmpty() && !_attrGrp.FormData.Any(cond => cond.Value == null || cond.Value.Trim() == ""))
                        //{                        
                        XmlNode _attDataGrpNode = _rootNode.AppendChild(_doc.CreateElement("BkgSvcAttributeDataGroup"));
                        _attDataGrpNode.AppendChild(_doc.CreateElement("AttributeGroupID")).InnerText = Convert.ToString(_attrGrp.BkgSvcAttributeGroupId);
                        _attDataGrpNode.AppendChild(_doc.CreateElement("InstanceId")).InnerText = Convert.ToString(_attrGrp.InstanceId);

                        if (_attrGrp.FormData.Any(cond => cond.Key == attributeGroupMappingIdForState))//if not personal alias type mapping id then add mapping id for country
                        {
                            XmlNode expChild_Country = _attDataGrpNode.AppendChild(_doc.CreateElement("BkgSvcAttributeData"));
                            expChild_Country.AppendChild(_doc.CreateElement("BkgAttributeGroupMappingID")).InnerText = Convert.ToString(attributeGroupMappingIdForCountry);
                            expChild_Country.AppendChild(_doc.CreateElement("Value")).InnerText = Convert.ToString("UNITED STATES");
                        }
                        //UAT-2100:
                        Boolean isArmedForceStateExist = IsArmedForcesStateExist(_attrGrp, attributeGroupMappingIdForState, attributeGroupMappingIdForCounty);
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        SetAliasMiddleNameIfNotExist(_attrGrp, attributeGroupMappingIdForAlias);
                        foreach (var _attrData in _attrGrp.FormData)
                        {
                            XmlNode expChild = _attDataGrpNode.AppendChild(_doc.CreateElement("BkgSvcAttributeData"));
                            expChild.AppendChild(_doc.CreateElement("BkgAttributeGroupMappingID")).InnerText = Convert.ToString(_attrData.Key);
                            expChild.AppendChild(_doc.CreateElement("Value")).InnerText = isArmedForceStateExist ? String.Empty : Convert.ToString(_attrData.Value);
                        }
                        //}
                    }
                }

                ////foreach (var _serviceId in _lstServiceIds)
                //foreach (var _packageServiceId in _lstPackageServiceIds)
                //{
                //    XmlNode _packageServiceNode = _rootNode.AppendChild(_doc.CreateElement("PackageService"));
                //    _packageServiceNode.AppendChild(_doc.CreateElement("PackageSvcID")).InnerText = Convert.ToString(_packageServiceId);

                //    //List<SupplementOrderData> _lstSupplementOrderData = supplementOrderCart.lstSupplementOrderData
                //    //                                                    .Where(sod => sod.BkgServiceId == _serviceId)
                //    //                                                    .ToList();
                //    List<SupplementOrderData> _lstSupplementOrderData = supplementOrderCart.lstSupplementOrderData
                //                                    .Where(sod => sod.PackageServiceId == _packageServiceId)
                //                                    .ToList();

                //    //List<Int32> _lstServiceItemIds = supplementOrderCart.lstSupplementOrderData
                //    //                                        .Where(svc => svc.BkgServiceId == _serviceId)
                //    //                                        .DistinctBy(svc => svc.PackageSvcItemId)
                //    //                                        .Select(svcId => svcId.PackageSvcItemId).ToList();
                //    List<Int32> _lstServiceItemIds = supplementOrderCart.lstSupplementOrderData
                //                        .Where(svc => svc.PackageServiceId == _packageServiceId)
                //                        .DistinctBy(svc => svc.PackageSvcItemId)
                //                        .Select(svcId => svcId.PackageSvcItemId).ToList();

                //    //foreach (var _data in _lstSupplementOrderData)
                //    foreach (var _serviceItemId in _lstServiceItemIds)
                //    {
                //        XmlNode _pkgSvcItemNode = _packageServiceNode.AppendChild(_doc.CreateElement("PackageSvcItem"));
                //        //_pkgSvcItemNode.AppendChild(_doc.CreateElement("PackageSvcItemID")).InnerText = Convert.ToString(_serviceItemId);

                //        // Get All the <BkgSvcAttributeDataGroup> Tages for a <PackageSvcItem> in a particular <PackageService> tag
                //        //List<SupplementOrderData> _lstBkgAttributeDataGrps = _lstSupplementOrderData
                //        //                                                        .Where(svc => svc.PackageSvcItemId == _serviceItemId
                //        //                                                              && svc.BkgServiceId == _serviceId)
                //        //                                                        .ToList();

                //        List<SupplementOrderData> _lstBkgAttributeDataGrps = _lstSupplementOrderData
                //                                        .Where(svc => svc.PackageSvcItemId == _serviceItemId
                //                                              && svc.PackageServiceId == _packageServiceId)
                //                                        .ToList();

                //        foreach (var _attrGrp in _lstBkgAttributeDataGrps)
                //        {
                //            XmlNode _attDataGrpNode = _pkgSvcItemNode.AppendChild(_doc.CreateElement("BkgSvcAttributeDataGroup"));
                //            _attDataGrpNode.AppendChild(_doc.CreateElement("AttributeGroupID")).InnerText = Convert.ToString(_attrGrp.BkgSvcAttributeGroupId);
                //            _attDataGrpNode.AppendChild(_doc.CreateElement("InstanceId")).InnerText = Convert.ToString(_attrGrp.InstanceId);

                //            foreach (var _attrData in _attrGrp.FormData)
                //            {
                //                XmlNode expChild = _attDataGrpNode.AppendChild(_doc.CreateElement("BkgSvcAttributeData"));
                //                expChild.AppendChild(_doc.CreateElement("BkgAttributeGroupMappingID")).InnerText = Convert.ToString(_attrData.Key);
                //                expChild.AppendChild(_doc.CreateElement("Value")).InnerText = Convert.ToString(_attrData.Value);
                //            }
                //        }
                //    }
                //}
                if (_lstPackageServiceIds.IsNullOrEmpty())
                {
                    //New Change 21072016
                    if (!supplementOrderCart.LstSupplementServiceId.IsNullOrEmpty())
                    {
                        _lstPackageServiceIds = supplementOrderCart.LstSupplementServiceId;
                    }
                }
                Boolean isAnyLineItemCreated = _presenter.GenerateSupplementOrder(this.SelectedTenantID, base.CurrentUserId, this.MasterOrderID, _doc.OuterXml, supplementOrderCart.OrdPkgSvcGroupId, _lstPackageServiceIds);
                if (isAnyLineItemCreated)
                {
                    base.ShowSuccessMessage("Supplement order has been placed successfully.");
                    fsucCmdBar1.SaveButton.Text = "Back to order details";
                    //UAT-2066:"Continue" button click from the supplement review screen should return the user to the order review queue
                    ResultMessage = "Supplement order has been placed successfully.";
                    ResultMessageType = "success";
                    RedirectToOrderReviewQueue();
                }
                else
                {
                    msgText = "No external services have been created for this order supplement. Please change review status of service group to Review Completed or try to supplement again with valid input.";
                    msgType = "info";
                    //base.ShowInfoMessage(msgText);
                    //UAT-2117:"Continue" button behavior
                    CheckAndUpdateBackgroundOrder(msgText);
                }
                fsucCmdBar1.CancelButton.Enabled = false;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
                msgText = ex.Message;
                msgType = "error";
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
                msgText = ex.Message;
                msgType = "error";
            }
            finally
            {
                createCustomForm();
                //Show messages
                if (msgType == "info")
                    base.ShowInfoMessage(msgText);
                else if (msgType == "error")
                    base.ShowErrorMessage(msgText);
            }

        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public Control CreateSectionForServiceItem(Int32 serviceItemId)
        {
            HtmlGenericControl section = new HtmlGenericControl("div");
            section.Attributes.Add("class", "section");
            //HtmlGenericControl headerTag = new HtmlGenericControl("h1");
            //headerTag.Attributes.Add("class", "mhdr");
            //<span style="float:right;"></span>
            //Label headerLable = new Label();
            //headerLable.ID = "lblHeader_" + serviceItemId;
            //headerLable.Text = lstSupplementServiceCustomFormList.FirstOrDefault(x => x.ServiceItemID == serviceItemId).ServiceItemName;
            //headerTag.Controls.Add(headerLable);
            //section.Controls.Add(headerTag);
            HtmlGenericControl content = new HtmlGenericControl("div");
            content.Attributes.Add("class", "content");

            //Panel in which all the other controls are loaded
            Panel formPanel = new Panel();
            formPanel.ID = "pnl_" + serviceItemId;

            content.Controls.Add(formPanel);
            section.Controls.Add(content);
            pnlLoader.Controls.Add(section);
            return formPanel;
        }


        #endregion

        #region Private Methods

        private void createCustomForm()
        {
            String packages = String.Empty;
            // packages = GetPackageIdString();
            //List<Int32> lstCustomForms = new List<Int32>();
            List<SupplementServiceItemCustomForm> lstCustomForms = new List<SupplementServiceItemCustomForm>();
            List<Int32> lstServiceIds = new List<Int32>();
            List<Int32> lstGroupIds = new List<Int32>();
            List<SupplementOrderData> oldLstSupplementOrderCart = new List<SupplementOrderData>();
            if (!lstSupplementOrderData.IsNullOrEmpty())
            {
                if (!lstSupplementServiceCustomFormList.IsNullOrEmpty())
                {
                    //lstCustomForms = lstSupplementServiceCustomFormList.DistinctBy(x => x.CustomFormID).Select(x => x.CustomFormID).ToList();
                    lstCustomForms = lstSupplementServiceCustomFormList.DistinctBy(x => x.CustomFormID).Select(col => col).ToList();
                    //for (Int32 svcId = 0; svcId < lstServiceIds.Count; svcId++)
                    //{
                    //    Panel panel = new Panel();
                    //    panel = CreateSectionForServiceItem(lstServiceIds[svcId]) as Panel;
                    //    for (Int32 custId = 0; custId < lstCustomForms.Count; custId++)
                    //    {
                    //        Presenter.GetListOfAttributesForSelectedItem(lstCustomForms[custId], lstServiceIds[svcId]);
                    //        oldLstSupplementOrderCart = lstSupplementOrderData.Where(x => x.CustomFormId == lstCustomForms[custId] && x.PackageSvcItemId == lstServiceIds[svcId]).Select(x => x).ToList();
                    //        lstGroupIds = lstCustomFormAttributes.DistinctBy(x => x.AttributeGroupId).Select(x => x.AttributeGroupId).ToList();
                    //        for (Int32 grpId = 0; grpId < lstGroupIds.Count; grpId++)
                    //        {


                    //            CustomFormHtlm _customForm = Page.LoadControl("~/BkgOperations/UserControl/CustomFormHtlm.ascx") as CustomFormHtlm;
                    //            _customForm.lstCustomFormAttributes = lstCustomFormAttributes;
                    //            _customForm.groupId = lstGroupIds[grpId];
                    //            _customForm.ServiceItemID = lstServiceIds[svcId];
                    //            //Total Number Of Instane for a particular group
                    //            _customForm.InstanceId = lstSupplementOrderData.Where(x => x.BkgSvcAttributeGroupId == lstGroupIds[grpId]
                    //                                       && x.CustomFormId == lstCustomForms[custId] && x.PackageSvcItemId == lstServiceIds[svcId]).Count();
                    //            _customForm.CustomFormId = lstCustomForms[custId];
                    //            _customForm.tenantId = SelectedTenantID;
                    //            _customForm.lstSupplementOrderData = oldLstSupplementOrderCart;
                    //            _customForm.IsReadOnly = true;
                    //            _customForm.IsSupplementalOrder = true;
                    //            _customForm.ShowPreExistingSupplementData = false;
                    //            _customForm.ShowEditDetailButton = false;
                    //            panel.Controls.Add(_customForm);
                    //        }
                    //    }
                    //}

                    foreach (SupplementServiceItemCustomForm customForm in lstCustomForms)
                    {

                        Panel panel = new Panel();
                        panel = CreateSectionForServiceItem(customForm.ServiceItemID) as Panel;
                        Presenter.GetListOfAttributesForSelectedItem(customForm.CustomFormID, customForm.ServiceItemID);
                        oldLstSupplementOrderCart = lstSupplementOrderData.Where(x => x.CustomFormId == customForm.CustomFormID && x.PackageSvcItemId == customForm.ServiceItemID).Select(x => x).ToList();
                        lstGroupIds = lstCustomFormAttributes.DistinctBy(x => x.AttributeGroupId).Select(x => x.AttributeGroupId).ToList();
                        if (!oldLstSupplementOrderCart.IsNullOrEmpty())
                        {
                            for (Int32 grpId = 0; grpId < lstGroupIds.Count; grpId++)
                            {
                                CustomFormHtlm _customForm = Page.LoadControl("~/BkgOperations/UserControl/CustomFormHtlm.ascx") as CustomFormHtlm;
                                _customForm.lstCustomFormAttributes = lstCustomFormAttributes;
                                _customForm.groupId = lstGroupIds[grpId];
                                _customForm.ServiceItemID = customForm.ServiceItemID;
                                //Total Number Of Instane for a particular group
                                _customForm.InstanceId = lstSupplementOrderData.Where(x => x.BkgSvcAttributeGroupId == lstGroupIds[grpId]
                                                           && x.CustomFormId == customForm.CustomFormID && x.PackageSvcItemId == customForm.ServiceItemID).Count();
                                _customForm.CustomFormId = customForm.CustomFormID;
                                _customForm.tenantId = SelectedTenantID;
                                _customForm.lstSupplementOrderData = oldLstSupplementOrderCart;
                                _customForm.IsReadOnly = true;
                                _customForm.IsSupplementalOrder = true;
                                _customForm.ShowPreExistingSupplementData = false;
                                _customForm.ShowEditDetailButton = false;
                                panel.Controls.Add(_customForm);
                            }
                        }
                    }
                }
            }
        }

        #region UAT-2066:"Continue" button click from the supplement review screen should return the user to the order review queue
        private void RedirectToOrderReviewQueue()
        {
            //New Change 21072016
            supplementOrderCart = (SupplementOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART);
            var _opsgId = supplementOrderCart.OrdPkgSvcGroupId;
            var _parentScreen = supplementOrderCart.ParentScreen;
            supplementOrderCart = null;
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART, supplementOrderCart);
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    {"Child",ChildControls.BackgroundOrderReviewQueue},
                                                                    { "ShowSuppSuccessMessage","true"},
                                                                    { "MessageType",ResultMessageType},
                                                                    { "Message",ResultMessage},
                                                                 };

            String url = String.Format("~/BkgOperations/Default.aspx?ucid={0}&args={1}", new Guid(), queryString.ToEncryptedQueryString());
            Response.Redirect(url);
        }
        #endregion

        /// <summary>
        /// Delete Read Only Instance and Group From Cart for Supplemental Order
        /// </summary>
        /// <returns></returns>
        private void DeleteReadOnlyInstanceAndGroupFromCart(SupplementOrderCart supplementOrderCart)
        {
            if (!hdnHiddenReadOnlyPanels.Value.Equals(""))
            {
                String[] groupIdAndInstance = hdnHiddenReadOnlyPanels.Value.Split(':');
                if (groupIdAndInstance.Count() > 0)
                {
                    List<SupplementOrderData> supplementOrderDataList = new List<SupplementOrderData>();
                    supplementOrderDataList.AddRange(supplementOrderCart.lstSupplementOrderData);

                    for (Int32 i = 0; i < groupIdAndInstance.Count(); i++)
                    {
                        String[] groupIdRelatedData = groupIdAndInstance[i].Split('_');
                        if (groupIdRelatedData.Length > 1)
                        {
                            foreach (var supplementOrderData in supplementOrderDataList.Where(x => x.BkgSvcAttributeGroupId == Convert.ToInt32(groupIdRelatedData[0]) && x.InstanceId == Convert.ToInt32(groupIdRelatedData[1])))
                            {
                                supplementOrderCart.lstSupplementOrderData.Remove(supplementOrderData);
                            }
                        }
                    }

                    List<Int32> groupIds = supplementOrderCart.lstSupplementOrderData.Select(x => x.BkgSvcAttributeGroupId).Distinct().ToList();

                    foreach (var groupId in groupIds)
                    {
                        var supplementOrderDataGroupList = supplementOrderCart.lstSupplementOrderData.Where(x => x.BkgSvcAttributeGroupId == groupId);
                        Int32 newInstanceId = 1;

                        foreach (var supplementOrderData in supplementOrderDataGroupList)
                        {
                            supplementOrderData.InstanceId = newInstanceId;
                            newInstanceId++;
                        }
                    }

                    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART, supplementOrderCart);
                    hdnHiddenReadOnlyPanels.Value = String.Empty;
                }
            }
        }


        #region UAT-2100:Stop creating line items for Armed Forces Addresses on US Criminal background checks

        private Boolean IsArmedForcesStateExist(SupplementOrderData suppOrderData, Int32 attributeGroupMappingIdForState, Int32 attributeGroupMappingIdForCounty)
        {
            if (!suppOrderData.FormData.IsNullOrEmpty() && suppOrderData.FormData.ContainsKey(attributeGroupMappingIdForState) && suppOrderData.FormData.Any(cnd => cnd.Value.ToLower().Contains(("Armed Forces").ToLower())))
            {
                //suppOrderData.FormData = new Dictionary<int, string>();
                //suppOrderData.FormData[attributeGroupMappingIdForState] = String.Empty;
                //suppOrderData.FormData[attributeGroupMappingIdForCounty] = String.Empty;
                return true;
            }
            return false;
        }
        #endregion

        #region UAT-2117:"Continue" button behavior
        private void RedirectToVendorServicesScreen()
        {
            //New Change 21072016
            supplementOrderCart = (SupplementOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART);
            var _opsgId = supplementOrderCart.OrdPkgSvcGroupId;
            //var _parentScreen = supplementOrderCart.ParentScreen;
            supplementOrderCart = null;
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART, supplementOrderCart);
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(SelectedTenantID) },
                                                                    { "OrderId", Convert.ToString(this.MasterOrderID)},
                                                                    { AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, Convert.ToString(_opsgId)},
                                                                    { AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, AppConsts.BKG_ORDER_REVIEW_QUEUE},
                                                                     { AppConsts.ORDER_NUMBER, this.MasterOrderNumber},
                                                                     { AppConsts.MENU_ID, AppConsts.MINUS_TWO.ToString()},
                                                                     { AppConsts.SOURCE_SCREEN_NAME, AppConsts.ORDER_DETAIL_FOR_SERVICE_ITEM_SUPPLEMENT},
                                                                 };

            String url = String.Format("~/BkgOperations/Pages/BkgOrderDetailPage.aspx?ucid={0}&args={1}", new Guid(), queryString.ToEncryptedQueryString());
            Response.Redirect(url);
        }
        /// <summary>
        /// Method to check and apply success indicator and update the order status.
        /// </summary>
        private void CheckAndUpdateBackgroundOrder(String errorMsg)
        {
            Boolean saveStatus = Presenter.CheckOrderToUpdate(this.MasterOrderID, this.CurrentUserId);
            //UAT-2200:Should send back to queue if everything in the service group is complete and clear and other service group(s) are in progress
            if (CurrentViewContext.IsOtherServiceGroupsAreCompleted)
            {
                if (CurrentViewContext.IsSuccessIndicatorApplicable && CurrentViewContext.IsAllExistingSearchesAreClear)
                {
                    ResultMessage = saveStatus ? "Order has been completed and success flag is applied on order because no external services have been created for this supplement order."
                                               : "Some error has occurred.Please try again.";
                    ResultMessageType = saveStatus ? "info" : "error";
                    RedirectToOrderReviewQueue();
                }
                else if (!CurrentViewContext.IsSuccessIndicatorApplicable || !CurrentViewContext.IsAllExistingSearchesAreClear)
                {
                    RedirectToVendorServicesScreen();
                }
            }
            else
            {
                ResultMessage = errorMsg;
                ResultMessageType = "info";
                RedirectToOrderReviewQueue();
            }

        }
        #endregion

        #region UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
        private void SetAliasMiddleNameIfNotExist(SupplementOrderData suppOrderData, Int32 attributeGroupMappingIdForAlias)
        {
            if (!suppOrderData.FormData.IsNullOrEmpty() && suppOrderData.FormData.ContainsKey(attributeGroupMappingIdForAlias))
            {
                String aliasName = suppOrderData.FormData[attributeGroupMappingIdForAlias];
                String[] splitedAliasNames = aliasName.IsNullOrEmpty() ? null : aliasName.Trim().Split(' ');
                if (!splitedAliasNames.IsNullOrEmpty())
                {
                    String firstName = splitedAliasNames.First().Trim();
                    String middleName = String.Join(" ", splitedAliasNames.Where((cond, index) => index < splitedAliasNames.Length - 1 && index > AppConsts.NONE));
                    String lastName = splitedAliasNames.Length > AppConsts.ONE ? splitedAliasNames.LastOrDefault().Trim() : String.Empty;
                    if (middleName.IsNullOrEmpty())
                    {
                        middleName = middleName.IsNullOrEmpty() ? NoMiddleNameText : middleName;
                        aliasName = firstName + " " + middleName + " " + lastName;
                        suppOrderData.FormData[attributeGroupMappingIdForAlias] = aliasName;
                    }
                }
            }
        }
        #endregion

        #endregion

        #endregion
    }
}