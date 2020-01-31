using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class AgencyHierarchyNodeAvailabilitySetting : BaseUserControl, IAgencyHierarchyNodeAvailabilitySettingView
    {
        #region Handlers

        public delegate bool ShowMessageHandler(object sender, StatusMessages messageType, String message);
        public event ShowMessageHandler eventShowMessage;

        #endregion


        #region [Variables / Properties]

        #region [Private Variables]

        private AgencyHierarchyNodeAvailabilitySettingPresenter _presenter = new AgencyHierarchyNodeAvailabilitySettingPresenter();

        #endregion

        public IAgencyHierarchyNodeAvailabilitySettingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public AgencyHierarchyNodeAvailabilitySettingPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        Int32 IAgencyHierarchyNodeAvailabilitySettingView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IAgencyHierarchyNodeAvailabilitySettingView.AgencyHierarchyID
        {
            get
            {
                return Convert.ToInt32(ViewState["AgencyHierarchyId"]);
            }
            set
            {
                ViewState["AgencyHierarchyId"] = value;
            }
        }

        Boolean IAgencyHierarchyNodeAvailabilitySettingView.IsRootNode
        {
            get;
            set;
        }

        public Int32 NodeId { get; set; }

        public Int32 SelectedRootNodeID
        {
            get
            {
                if (!ViewState["SelectedRootNodeID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["SelectedRootNodeID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedRootNodeID"] = value;
            }
        }

        AgencyHierarchySettingContract IAgencyHierarchyNodeAvailabilitySettingView.AgencyHierarchyNodeAvailabilitySettingContract
        {
            get;
            set;
        }

        Boolean IAgencyHierarchyNodeAvailabilitySettingView.IsAgencyHierachyNodeAvailabilitySettingExisted
        {
            get;
            set;
        }

        #endregion


        #region [Page Events]

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    CurrentViewContext.AgencyHierarchyID = NodeId;
                    if (CurrentViewContext.AgencyHierarchyID > AppConsts.NONE)
                    {
                        //Presenter.GetAgencyHierachyNodeAvailabilitySettingExisted();
                        //if (CurrentViewContext.IsAgencyHierachyNodeAvailabilitySettingExisted)
                        //{
                        //    //Bind Setting
                        //    BindAgencyHierarchyNodeAvailabilitySetting();
                        //}
                        //else
                        //{
                        //    rbtnNodeAvailablitySettingCheckYes.Checked = true;                            
                        //}

                        BindAgencyHierarchyNodeAvailabilitySetting();
                    }
                    else
                    {
                        dvAgencyHierarchyNodeAvailabilitySetting.Visible = false;
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        #endregion

        #region [Button Events]

        protected void fsucCmdBarNodeAvailablitySetting_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                #region Oldcode
                //var agencyHierarchySettingContract = new AgencyHierarchySettingContract();
                //Int32 settingValue = rbtnNodeAvailablitySettingCheckYes.Checked ? AppConsts.ONE : AppConsts.NONE;
                //agencyHierarchySettingContract.CheckParentSetting = false;
                //agencyHierarchySettingContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;
                //agencyHierarchySettingContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;
                //agencyHierarchySettingContract.SettingTypeCode = AgencyHierarchySettingType.IS_NODE_AVAILABLE_FOR_ROTATION.GetStringValue();
                //agencyHierarchySettingContract.SettingValue = Convert.ToString(settingValue);
                #endregion

                var agencyHierarchySettingContract = new AgencyHierarchySettingContract();
                Int32 settingValue = rbtnNodeAvailablitySettingCheckYes.Checked ? AppConsts.TWO : (rbtnNodeAvailablitySettingCheckNo.Checked ? AppConsts.ONE : AppConsts.NONE);
                agencyHierarchySettingContract.CheckParentSetting = rbtnNodeAvailablitySettingDefault.Checked ? true : false;
                agencyHierarchySettingContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;
                agencyHierarchySettingContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;
                agencyHierarchySettingContract.SettingValue = Convert.ToString(settingValue);
                agencyHierarchySettingContract.SettingTypeCode = AgencyHierarchySettingType.IS_NODE_AVAILABLE_FOR_ROTATION.GetStringValue();

                if (Presenter.SaveUpdateAgencyHierarchyNodeAvailabilitySetting(agencyHierarchySettingContract))

                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Agency Hierarchy Setting(s) Saved Successfully.");
                else
                    eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Error while saving setting(s) to selected node.");
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region [Control Events]

        #endregion


        #region [Methods]

        private void BindAgencyHierarchyNodeAvailabilitySetting()
        {
            #region Oldcode
            //if (!agencysettings.IsNullOrEmpty() && !agencysettings.SettingValue.IsNullOrEmpty())
            //{
            //    if (Convert.ToInt32(agencysettings.SettingValue) == AppConsts.NONE)
            //    {
            //        rbtnNodeAvailablitySettingCheckYes.Checked = false;
            //        rbtnNodeAvailablitySettingCheckNo.Checked = true;
            //    }
            //    else
            //    {
            //        rbtnNodeAvailablitySettingCheckYes.Checked = true;
            //        rbtnNodeAvailablitySettingCheckNo.Checked = false;
            //    }
            //}
            #endregion

            Presenter.GetAgencyHierachyNodeAvailabilitySettingExisted();
            AgencyHierarchySettingContract agencysettings = CurrentViewContext.AgencyHierarchyNodeAvailabilitySettingContract;

            if (NodeId == SelectedRootNodeID) // Or == CurrentViewContext.AgencyHierarchyID
            {
                rbtnNodeAvailablitySettingDefault.Visible = false;
            }

            if (!agencysettings.IsNullOrEmpty() && !agencysettings.SettingValue.IsNullOrEmpty())
            {
                if (Convert.ToInt32(agencysettings.SettingValue) == AppConsts.NONE)
                {
                    rbtnNodeAvailablitySettingDefault.Checked = true; //0
                    rbtnNodeAvailablitySettingCheckNo.Checked = false; //1
                    rbtnNodeAvailablitySettingCheckYes.Checked = false; //2
                }
                else if (Convert.ToInt32(agencysettings.SettingValue) == AppConsts.ONE)
                {
                    rbtnNodeAvailablitySettingDefault.Checked = false;
                    rbtnNodeAvailablitySettingCheckNo.Checked = true;
                    rbtnNodeAvailablitySettingCheckYes.Checked = false;
                }
                else
                {
                    rbtnNodeAvailablitySettingDefault.Checked = false;
                    rbtnNodeAvailablitySettingCheckNo.Checked = false;
                    rbtnNodeAvailablitySettingCheckYes.Checked = true;
                }
            }
            else
            {
                if (NodeId == SelectedRootNodeID) // Or == CurrentViewContext.AgencyHierarchyID
                {
                    rbtnNodeAvailablitySettingDefault.Checked = false; //0
                    rbtnNodeAvailablitySettingCheckNo.Checked = false; //1
                    rbtnNodeAvailablitySettingCheckYes.Checked = true; //2
                }
                else
                {
                    rbtnNodeAvailablitySettingDefault.Checked = true; //0
                    rbtnNodeAvailablitySettingCheckNo.Checked = false; //1
                    rbtnNodeAvailablitySettingCheckYes.Checked = false; //2
                }
            }


        }

        #endregion
    }
}