#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#region UserDefined

using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.Shell;
using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel;
using INTSOF.UI.Contract.BkgOperations;

#endregion

#endregion

namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgOrderCustomForm : BaseUserControl, IBkgOrderCustomFormView
    {
        #region Variables

        #region Private Variables

        private Int32 _tenantId;
        private BkgOrderCustomFormPresenter _presenter = new BkgOrderCustomFormPresenter();
        private Int32 _customFormId;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        List<AttributesForCustomFormContract> IBkgOrderCustomFormView.lstCustomFormAttributes { get; set; }

        List<BkgOrderDetailCustomFormUserData> IBkgOrderCustomFormView.lstDataForCustomForm { get; set; }

        Int32 IBkgOrderCustomFormView.TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set
            {
                _tenantId = value;
            }
        }

        IBkgOrderCustomFormView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IBkgOrderCustomFormView.CustomFormID
        {
            get
            {
                return _customFormId;
            }
            set
            {
                _customFormId = value;
            }
        }

        Int32 IBkgOrderCustomFormView.MasterOrderID
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID).IsNotNull())
                    return (Int32)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID);
                return 0;
            }
        }

        Int32 IBkgOrderCustomFormView.SelectedTenantID
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID).IsNotNull())
                    return (Int32)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID);
                return 0;
            }
        }

        #endregion

        #region Public Properties

        public BkgOrderCustomFormPresenter Presenter
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

        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.QueryString["args"].IsNull())
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                CurrentViewContext.CustomFormID = Convert.ToInt32(args.GetValue("menuID"));
            }
            createCustomForm();
        }

        private void createCustomForm()
        {
            List<Int32> lstGroupIds = new List<Int32>();
            List<Int32> lstInstanceIds = null;
            Dictionary<Int32, String> customFormData = null;
            List<BkgOrderDetailCustomFormUserData> lstRefinedData = null;
            List<BackgroundOrderData> lstBackGroundOrderData = null;
            Presenter.GetAttributesCustomFormIdOrderId();
            lstGroupIds = CurrentViewContext.lstDataForCustomForm.DistinctBy(x => x.AttributeGroupID).Select(x => x.AttributeGroupID).ToList();
            for (Int32 grpId = 0; grpId < lstGroupIds.Count; grpId++)
            {
                lstBackGroundOrderData = new List<BackgroundOrderData>();
                NewCustomFormHtlm _customForm = Page.LoadControl("~/BkgOperations/UserControl/NewCustomFormHtlm.ascx") as NewCustomFormHtlm;
                lstInstanceIds = CurrentViewContext.lstDataForCustomForm.Where(cond => cond.AttributeGroupID == lstGroupIds[grpId]).DistinctBy(x => x.InstanceID).Select(x => x.InstanceID).ToList();
                for (Int32 instId = 0; instId < lstInstanceIds.Count; instId++)
                {
                    customFormData = new Dictionary<Int32, String>();
                    BackgroundOrderData backgroundOrderData = new BackgroundOrderData();
                    lstRefinedData = CurrentViewContext.lstDataForCustomForm.Where(cond => cond.InstanceID == lstInstanceIds[instId] && cond.AttributeGroupID == lstGroupIds[grpId]).ToList();

                    backgroundOrderData.InstanceId = lstInstanceIds[instId];
                    backgroundOrderData.BkgSvcAttributeGroupId = lstGroupIds[grpId];
                    backgroundOrderData.CustomFormId = CurrentViewContext.CustomFormID;
                    foreach (var element in lstRefinedData)
                    {
                        if (!customFormData.ContainsKey(element.AttributeGroupMappingID))
                            customFormData.Add(element.AttributeGroupMappingID, element.Value);
                    }
                    backgroundOrderData.CustomFormData = customFormData;
                    lstBackGroundOrderData.Add(backgroundOrderData);
                }
                _customForm.lstCustomFormAttributes = CurrentViewContext.lstCustomFormAttributes;
                _customForm.groupId = lstGroupIds[grpId];
                //Total Number Of Instane for a particular group
                _customForm.InstanceId = lstInstanceIds.Count;
                _customForm.CustomFormId = CurrentViewContext.CustomFormID;
                _customForm.tenantId = CurrentViewContext.SelectedTenantID;
                _customForm.lstBackgroundOrderData = lstBackGroundOrderData;
                _customForm.IsReadOnly = true;
                _customForm.IsOrderDetailForEds = Presenter.IsEdsServiceExitForOrder(lstGroupIds[grpId]);
                _customForm.OrderId = CurrentViewContext.MasterOrderID;
                pnlLoader.Controls.Add(_customForm);
            }
        }

    }
}