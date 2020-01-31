using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using CoreWeb.CommonOperations.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.PackageBundleManagement;
using INTERSOFT.WEB.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using Entity.ClientEntity;
namespace CoreWeb.CommonOperations.Views
{
    public partial class ManagePackageBundle : BaseUserControl, IManageBundle
    {
        #region Private Variables

        private ManageBundlePresenter _presenter = new ManageBundlePresenter();
        private Int32 tenantId = 0;
        private ManagePackageBundleContract _searchContract = null;

        #endregion

        #region Properties

        #region Public Properties

        public ManageBundlePresenter Presenter
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
        public IManageBundle CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }
        string IManageBundle.SuccessMessage
        {
            get;
            set;
        }
        string IManageBundle.ErrorMessage
        {
            get;
            set;
        }
        bool IManageBundle.IsAdminLoggedIn
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        string IManageBundle.HierarchyNode
        {
            get;
            set;
        }

        string IManageBundle.ExplanatoryNotes
        {
            get;
            set;
        }

        bool IManageBundle.IsDeleted
        {
            get;
            set;
        }

        List<Int32> IManageBundle.ScreeningpackageIDs
        {
            get;
            set;
        }

        bool IManageBundle.IsAvailableForOrder
        {
            get;
            set;
        }

        string IManageBundle.BundleName
        {
            get;
            set;
        }

        string IManageBundle.TrackingPackage
        {
            get;
            set;
        }

        string IManageBundle.AdministrativePackage
        {
            get;
            set;
        }

        string IManageBundle.Screeningpackage
        {
            get;
            set;
        }

        string IManageBundle.BundleLabel
        {
            get;
            set;
        }

        string IManageBundle.BundleDescription
        {
            get;
            set;
        }

        Int32 IManageBundle.TrackingPackageID
        {
            get;
            set;
        }

        Int32 IManageBundle.BundleId
        {
            get;
            set;
        }

        Int32 IManageBundle.AdministrativePackageID
        {
            get;
            set;
        }

        Int32 IManageBundle.ScreeningpackageID
        {
            get;
            set;
        }

        Int32 IManageBundle.SelectedTenantIDForAddForm
        {
            get;
            set;
        }

        Int32 IManageBundle.SelectedTenantID
        {
            get
            {
                if (String.IsNullOrEmpty(ddlTenantName.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlTenantName.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlTenantName.SelectedValue = Convert.ToString(value);
                    hdnTenantId.Value = Convert.ToString(value);
                }
                else
                {
                    ddlTenantName.SelectedIndex = value;
                }
            }
        }

        Int32 IManageBundle.TenantID
        {
            get
            {
                if (tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantId;
            }
            set { tenantId = value; }
        }

        List<Entity.Tenant> IManageBundle.lstTenant
        {
            get
            {
                if (!ViewState["lstTenant"].IsNull())
                {
                    return ViewState["lstTenant"] as List<Entity.Tenant>;
                }

                return new List<Entity.Tenant>();
            }
            set
            {
                ViewState["lstTenant"] = value;
            }
        }

        List<ManagePackageBundleContract> IManageBundle.BundleDataList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ManagePackageBundleContract IManageBundle.SearchContract
        {
            get
            {
                if (_searchContract.IsNull())
                {
                    GetSearchParameters();
                }
                return _searchContract;
            }
            set
            {
                _searchContract = value;
                //SetSearchParameters();
            }
        }

        /*UAT - 3157*/

        Int32 IManageBundle.PreferredSelectedTenantID
        {
            get
            {
                if (!ViewState["PreferredSelectedTenantID"].IsNull())
                {
                    return (Int32)ViewState["PreferredSelectedTenantID"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["PreferredSelectedTenantID"] = value;
            }
        }
        /* END UAT - 3157*/

        #endregion

        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// set the page title on bread crumb
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.Title = "Manage Package Bundle";
            base.SetPageTitle("Manage Package Bundle");
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                if (!this.IsPostBack)
                {
                    grdPackageBundle.Visible = false;
                    Presenter.OnViewInitialized();
                    BindTenant();
                    //Start UAT-3157
                    Presenter.IsAdminLoggedIn();
                    if (CurrentViewContext.IsAdminLoggedIn)
                    {
                        GetPreferredSelectedTenant();
                        //ViewState["ReBindGrid"] = "false";
                    }
                    //End UAT-3157
                }
                Presenter.OnViewLoaded();
                if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                {
                    CurrentViewContext.SelectedTenantID = Convert.ToInt32(ddlTenantName.SelectedValue);
                }
                else if (ddlTenantName.SelectedValue.IsNullOrEmpty())
                {
                    CurrentViewContext.SelectedTenantID = 0;
                }

                hdnTenantId.Value = CurrentViewContext.SelectedTenantID.ToString();
                lblInstituteHierarchyName.Text = hdnHierarchyLabel.Value.HtmlEncode();
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

        #region Grid Events

        protected void grdPackageBundle_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode) && (e.Item is GridEditableItem))
                {
                    hdnDepartmentProgmapNew.Value = String.Empty;
                    hdnInstNodeIdNew.Value = String.Empty;
                    WclComboBox ddlTenantNameNew = ((WclComboBox)e.Item.FindControl("ddlTenantNameNew"));
                    //WclComboBox ddlTrackingPackages = ((WclComboBox)e.Item.FindControl("ddlTrackingPackages"));
                    //WclComboBox ddlAdmPackages = ((WclComboBox)e.Item.FindControl("ddlAdmPackage"));
                    //WclComboBox ddlScreeningpackages = ((WclComboBox)e.Item.FindControl("ddlscreeningpackage"));
                    WclTextBox txtBundleNameNew = (e.Item.FindControl("txtBundleNameNew") as WclTextBox);
                    WclTextBox txtLabel = (e.Item.FindControl("txtLabel") as WclTextBox);
                    RadioButtonList rdAvailableforOrder = (e.Item.FindControl("rdAvailableforOrder") as RadioButtonList);
                    WclEditor rdEditorNotes = (e.Item.FindControl("rdEditorNotes") as WclEditor);
                    WclTextBox txtDescription = (e.Item.FindControl("txtDescription") as WclTextBox);
                    rdAvailableforOrder.SelectedValue = "True";
                    if (ddlTenantNameNew != null)
                    {
                        //_presenter.GetTenants();
                        ddlTenantNameNew.DataSource = CurrentViewContext.lstTenant;
                        ddlTenantNameNew.DataBind();
                        ddlTenantNameNew.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--", AppConsts.ZERO));
                    }
                    if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                    {
                        ddlTenantNameNew.Enabled = false;
                        ddlTenantNameNew.SelectedValue = CurrentViewContext.SelectedTenantID.ToString();
                        CurrentViewContext.SelectedTenantIDForAddForm = CurrentViewContext.SelectedTenantID;
                    }
                    else
                    {
                        ddlTenantNameNew.Enabled = true;
                    }

                    List<PackageBundlePackages> packageBundlePackages = new List<PackageBundlePackages>();

                    ManageInstituteHierarchyPackage ucManageInstituteHierarchyImmunizationPackage = (e.Item.FindControl("ucManageInstituteHierarchyImmunizationPackage") as ManageInstituteHierarchyPackage);
                    if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                    {
                        ucManageInstituteHierarchyImmunizationPackage.TenantID = Convert.ToInt32(CurrentViewContext.SelectedTenantID);
                    }
                    else
                    {
                        ucManageInstituteHierarchyImmunizationPackage.TenantID = 0;
                    }

                    ucManageInstituteHierarchyImmunizationPackage.CompliancePackageTypeCode = CompliancePackageTypes.IMMUNIZATION_COMPLIANCE_PACKAGE.GetStringValue();
                    ucManageInstituteHierarchyImmunizationPackage.IsCompliancePackage = true;
                    ucManageInstituteHierarchyImmunizationPackage.DispalyText = "Select Institution Hierarchy For Immunization Package";

                    ManageInstituteHierarchyPackage ucManageInstituteHierarchyAdministrativePackage = (e.Item.FindControl("ucManageInstituteHierarchyAdministrativePackage") as ManageInstituteHierarchyPackage);
                    if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                    {
                        ucManageInstituteHierarchyAdministrativePackage.TenantID = Convert.ToInt32(CurrentViewContext.SelectedTenantID);
                    }
                    else
                    {
                        ucManageInstituteHierarchyAdministrativePackage.TenantID = 0;
                    }
                    ucManageInstituteHierarchyAdministrativePackage.CompliancePackageTypeCode = CompliancePackageTypes.ADMINISTRATIVE_COMPLIANCE_PACKAGE.GetStringValue();
                    ucManageInstituteHierarchyAdministrativePackage.IsCompliancePackage = true;
                    ucManageInstituteHierarchyAdministrativePackage.DispalyText = "Select Institution Hierarchy For Administrative Package";

                    ManageInstituteHierarchyPackage ucManageInstituteHierarchyScreeningPackage = (e.Item.FindControl("ucManageInstituteHierarchyScreeningPackage") as ManageInstituteHierarchyPackage);
                    if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                    {
                        ucManageInstituteHierarchyScreeningPackage.TenantID = Convert.ToInt32(CurrentViewContext.SelectedTenantID);
                    }
                    else
                    {
                        ucManageInstituteHierarchyScreeningPackage.TenantID = 0;
                    }
                    ucManageInstituteHierarchyScreeningPackage.CompliancePackageTypeCode = String.Empty;
                    ucManageInstituteHierarchyScreeningPackage.IsCompliancePackage = false;
                    ucManageInstituteHierarchyScreeningPackage.DispalyText = "Select Institution Hierarchy For Screening Package";

                    //if (ddlTrackingPackages != null && ddlAdmPackages != null && ddlScreeningpackages != null)
                    //{
                    //    if (!ddlTenantName.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(ddlTenantName.SelectedValue) > AppConsts.NONE)
                    //    {
                    //       
                    //        ////Bind Tracking Compliance Packages
                    //        //BindTrackingCompliancePackages(ddlTrackingPackages, packageBundlePackages);

                    //        ////Bind Administrative Compliance Packages
                    //        //BindAdministrativeCompliancePackages(ddlAdmPackages, packageBundlePackages);

                    //        ////Bind Screening Packages
                    //        //BindScreeningPackages(ddlScreeningpackages, packageBundlePackages);



                    //    }
                    //    else
                    //    {
                    //        ddlTrackingPackages.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--", AppConsts.ZERO));
                    //        ddlAdmPackages.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--", AppConsts.ZERO));
                    //        //ddlScreeningpackages.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--", AppConsts.ZERO));
                    //    }
                    //}

                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        String PackageBundleNodeIds = string.Empty;
                        //// Fill Controls for update -----------------------------------
                        GridEditableItem gridEditableItem = e.Item as GridEditableItem;

                        ManagePackageBundleContract pkgBundleContract = (e.Item.DataItem as ManagePackageBundleContract);

                        CurrentViewContext.BundleId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BundleId"));
                        Entity.ClientEntity.PackageBundle objPBU = Presenter.GetPackageBundlebyId();


                        if (objPBU.IsNotNull())
                        {
                            txtBundleNameNew.Text = objPBU.PBU_Name;
                            txtLabel.Text = objPBU.PBU_Label;
                            rdEditorNotes.Content = objPBU.PBU_ExplanatoryNotes;
                            rdAvailableforOrder.SelectedValue = objPBU.PBU_IsAvailableForOrder.ToString();
                            txtDescription.Text = objPBU.PBU_Description;

                            List<Int32> objPBNM = objPBU.PackageBundleNodeMappings.Where(cond => !cond.PBNM_IsDeleted).Select(col => col.PBNM_DeptProgramMappingID).ToList();
                            if (objPBNM != null)
                            {
                                PackageBundleNodeIds = string.Join(",", objPBNM.ToArray());
                                hdnInstNodeIdNew.Value = PackageBundleNodeIds;
                            }
                            hdnDepartmentProgmapNew.Value = PackageBundleNodeIds;
                            (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = pkgBundleContract.HierarchyNodes.HtmlEncode();
                            hdnInstitutionHierarchyPBLbl.Value = pkgBundleContract.HierarchyNodes;
                            packageBundlePackages = Presenter.GetPackageBundlePackages();
                            List<PackageBundleNodePackage> objPBNP = objPBU.PackageBundleNodePackages.Where(cond => !cond.PBNP_IsDeleted).ToList();

                            String trackingPackageCode = CompliancePackageTypes.IMMUNIZATION_COMPLIANCE_PACKAGE.GetStringValue();
                            PackageBundleNodePackage imuPackage = objPBNP.Where(cond => cond.PBNP_CompliancePackageTypeID != null
                                                                                && cond.lkpCompliancePackageType.CPT_Code == trackingPackageCode).FirstOrDefault();

                            //if (!imuPackage.IsNullOrEmpty() && packageBundlePackages.Any(cond => cond.IsCompliancePackage == true
                            //                                && cond.PackageNodeMappingID == imuPackage.PBNP_DeptProgramPackageID && cond.PackageTypeCode == trackingPackageCode))
                            if (!imuPackage.IsNullOrEmpty())
                            {
                                //ddlTrackingPackages.SelectedValue = imuPackage.PBNP_DeptProgramPackageID.ToString();
                                ucManageInstituteHierarchyImmunizationPackage.PackageNodeMappingID = imuPackage.PBNP_DeptProgramPackageID.ToString();
                                ucManageInstituteHierarchyImmunizationPackage.PackageId = imuPackage.DeptProgramPackage.DPP_CompliancePackageID.ToString();
                                ucManageInstituteHierarchyImmunizationPackage.InstitutionHierarchyNodeID = imuPackage.DeptProgramPackage.DPP_DeptProgramMappingID.ToString();///imuPackage.PBNP_DeptProgramMappingID.ToString();
                                ucManageInstituteHierarchyImmunizationPackage.InstituteHierarchyPackageName = imuPackage.DeptProgramPackage.CompliancePackage.PackageName;
                                ucManageInstituteHierarchyImmunizationPackage.PackageName = imuPackage.DeptProgramPackage.CompliancePackage.PackageName;

                            }

                            String administrativePackageCode = CompliancePackageTypes.ADMINISTRATIVE_COMPLIANCE_PACKAGE.GetStringValue();
                            PackageBundleNodePackage AdmPackage = objPBNP.Where(cond => cond.PBNP_CompliancePackageTypeID != null
                                                                                    && cond.lkpCompliancePackageType.CPT_Code == administrativePackageCode).FirstOrDefault();
                            //if (!AdmPackage.IsNullOrEmpty() && packageBundlePackages.Any(cond => cond.IsCompliancePackage == true
                            //                                    && cond.PackageNodeMappingID == AdmPackage.PBNP_DeptProgramPackageID
                            //                                    && cond.PackageTypeCode == administrativePackageCode))
                            //                                                                                            && cond.lkpCompliancePackageType.CPT_Code == administrativePackageCode).FirstOrDefault();
                            if (!AdmPackage.IsNullOrEmpty())
                            {
                                //lAdmPackages.SelectedValue = AdmPackage.PBNP_DeptProgramPackageID.ToString();
                                ucManageInstituteHierarchyAdministrativePackage.PackageNodeMappingID = AdmPackage.PBNP_DeptProgramPackageID.ToString();
                                ucManageInstituteHierarchyAdministrativePackage.PackageId = AdmPackage.DeptProgramPackage.DPP_CompliancePackageID.ToString();
                                ucManageInstituteHierarchyAdministrativePackage.InstitutionHierarchyNodeID = AdmPackage.DeptProgramPackage.DPP_DeptProgramMappingID.ToString();//AdmPackage.PBNP_DeptProgramMappingID.ToString();
                                ucManageInstituteHierarchyAdministrativePackage.InstituteHierarchyPackageName = AdmPackage.DeptProgramPackage.CompliancePackage.PackageName;
                                ucManageInstituteHierarchyAdministrativePackage.PackageName = AdmPackage.DeptProgramPackage.CompliancePackage.PackageName;

                            }

                            List<PackageBundleNodePackage> screeningPackage = objPBNP.Where(cond => cond.PBNP_CompliancePackageTypeID == null
                                 && cond.PBNP_BkgPackageHierarchyMappingID != null).ToList();

                            foreach (PackageBundleNodePackage item in screeningPackage)
                            {
                                //if (packageBundlePackages.Any(cond => cond.IsCompliancePackage == false && cond.PackageNodeMappingID == item.PBNP_BkgPackageHierarchyMappingID))
                                //{
                                //ddlScreeningpackages.FindItemByValue(item.PBNP_BkgPackageHierarchyMappingID.ToString()).Checked = true;
                                if (ucManageInstituteHierarchyScreeningPackage.PackageNodeMappingID.IsNullOrEmpty())
                                {
                                    ucManageInstituteHierarchyScreeningPackage.PackageNodeMappingID = item.PBNP_BkgPackageHierarchyMappingID.ToString();
                                }
                                else
                                {
                                    ucManageInstituteHierarchyScreeningPackage.PackageNodeMappingID = ucManageInstituteHierarchyScreeningPackage.PackageNodeMappingID + "," + item.PBNP_BkgPackageHierarchyMappingID.ToString();
                                }
                                if (ucManageInstituteHierarchyScreeningPackage.PackageId.IsNullOrEmpty())
                                {
                                    ucManageInstituteHierarchyScreeningPackage.PackageId = item.BkgPackageHierarchyMapping.BPHM_BackgroundPackageID.ToString();
                                }
                                else
                                {
                                    ucManageInstituteHierarchyScreeningPackage.PackageId = ucManageInstituteHierarchyScreeningPackage.PackageId + "," + item.BkgPackageHierarchyMapping.BPHM_BackgroundPackageID;
                                }

                                if (ucManageInstituteHierarchyScreeningPackage.InstitutionHierarchyNodeID.IsNullOrEmpty())
                                {
                                    ucManageInstituteHierarchyScreeningPackage.InstitutionHierarchyNodeID = item.BkgPackageHierarchyMapping.BPHM_InstitutionHierarchyNodeID.ToString();
                                }
                                else
                                {
                                    ucManageInstituteHierarchyScreeningPackage.InstitutionHierarchyNodeID = ucManageInstituteHierarchyScreeningPackage.InstitutionHierarchyNodeID + "," + item.BkgPackageHierarchyMapping.BPHM_InstitutionHierarchyNodeID.ToString();
                                }

                                if (ucManageInstituteHierarchyScreeningPackage.InstituteHierarchyPackageName.IsNullOrEmpty())
                                {
                                    ucManageInstituteHierarchyScreeningPackage.InstituteHierarchyPackageName = item.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Name;
                                }
                                else
                                {
                                    ucManageInstituteHierarchyScreeningPackage.InstituteHierarchyPackageName = ucManageInstituteHierarchyScreeningPackage.InstituteHierarchyPackageName + "," + item.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Name;
                                }
                                //}
                            }
                            ucManageInstituteHierarchyScreeningPackage.PackageName = ucManageInstituteHierarchyScreeningPackage.InstituteHierarchyPackageName;
                        }
                    }

                    if (e.Item is GridEditFormInsertItem)
                    {
                        hdnInstitutionHierarchyPBLbl.Value = "";
                    }
                }
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

        //protected void grdPackageBundle_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        //{
        //    ////if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
        //    ////{
        //    ////    GridEditFormItem editform = (e.Item as GridEditFormItem);
        //    ////    WclComboBox ddlTenantNameNew = editform.FindControl("ddlTenantNameNew") as WclComboBox;

        //    ////    if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
        //    ////    {
        //    ////        CurrentViewContext.SelectedTenantIDForAddForm = CurrentViewContext.SelectedTenantID;
        //    ////    }
        //    ////    if (!ddlTenantNameNew.SelectedValue.IsNullOrEmpty())
        //    ////    {
        //    ////        CurrentViewContext.SelectedTenantIDForAddForm = Convert.ToInt32(ddlTenantNameNew.SelectedValue);
        //    ////    }
        //    ////}

        //    //WclComboBox ddlTenantNameNew = ((WclComboBox)e.Item.FindControl("ddlTenantNameNew"));
        //    //////WclComboBox ddlTrackingPackages = ((WclComboBox)e.Item.FindControl("ddlTrackingPackages"));
        //    //////WclComboBox ddlAdmPackages = ((WclComboBox)e.Item.FindControl("ddlAdmPackage"));
        //    //////WclComboBox ddlScreeningpackages = ((WclComboBox)e.Item.FindControl("ddlscreeningpackage"));

        //    ////try
        //    ////{
        //    //if (ddlTenantNameNew != null)
        //    //{
        //    //    _presenter.GetTenants();
        //    //    ddlTenantNameNew.DataSource = CurrentViewContext.lstTenant;
        //    //    ddlTenantNameNew.DataBind();
        //    //    ddlTenantNameNew.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        //    //}

        //    //    //if (ddlTrackingPackages != null && ddlAdmPackages != null && ddlScreeningpackages != null)
        //    //    //{
        //    //    //    if (!ddlTenantName.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(ddlTenantName.SelectedValue) > 0)
        //    //    //  {
        //    //    //      ds = Presenter.GetPackageBundlePackages(Convert.ToInt32(ddlTenantName.SelectedValue));
        //    //    //    if (ds.Tables[0].Rows.Count > 0)
        //    //    //    {
        //    //    //        ddlTrackingPackages.DataSource = ds.Tables[0];
        //    //    //        ddlTrackingPackages.DataTextField = ds.Tables[0].Columns["TrackingPackage"].ToString();
        //    //    //        ddlTrackingPackages.DataValueField = ds.Tables[0].Columns["DPP_ID"].ToString();
        //    //    //        ddlTrackingPackages.DataBind();
        //    //    //        ddlTrackingPackages.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        //    //    //    }
        //    //    //    if (ds.Tables[1].Rows.Count > 0)
        //    //    //    {
        //    //    //        ddlAdmPackages.DataSource = ds.Tables[1];
        //    //    //        ddlAdmPackages.DataTextField = ds.Tables[1].Columns["AdministrativePackage"].ToString();
        //    //    //        ddlAdmPackages.DataValueField = ds.Tables[1].Columns["DPP_ID"].ToString();
        //    //    //        ddlAdmPackages.DataBind();
        //    //    //        ddlAdmPackages.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        //    //    //    }
        //    //    //    if (ds.Tables[2].Rows.Count > 0)
        //    //    //    {
        //    //    //        ddlScreeningpackages.DataSource = ds.Tables[2];
        //    //    //        ddlScreeningpackages.DataTextField = ds.Tables[2].Columns["Backgroundpackages"].ToString();
        //    //    //        ddlScreeningpackages.DataValueField = ds.Tables[2].Columns["BPHM_ID"].ToString();
        //    //    //        ddlScreeningpackages.DataBind();
        //    //    //        ddlScreeningpackages.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        //    //    //    }
        //    //    //}
        //    //    //}
        //    //}
        //    //catch (SysXException ex)
        //    //{
        //    //    base.LogError(ex);
        //    //    base.ShowErrorMessage(ex.Message);
        //    //}
        //    //catch (System.Exception ex)
        //    //{
        //    //    base.LogError(ex);
        //    //    base.ShowErrorMessage(ex.Message);
        //    //}
        //    //finally
        //    //{
        //    //    ds.Dispose();
        //    //}
        //}

        //protected void grdpackageBundle_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        //{

        //}

        protected void grdPackageBundle_InsertCommand(object sender, GridCommandEventArgs e)
        {
            ManageInstituteHierarchyPackage ucManageInstituteHierarchyImmunizationPackage = (e.Item.FindControl("ucManageInstituteHierarchyImmunizationPackage") as ManageInstituteHierarchyPackage);
            ManageInstituteHierarchyPackage ucManageInstituteHierarchyAdministrativePackage = (e.Item.FindControl("ucManageInstituteHierarchyAdministrativePackage") as ManageInstituteHierarchyPackage);
            ManageInstituteHierarchyPackage ucManageInstituteHierarchyScreeningPackage = (e.Item.FindControl("ucManageInstituteHierarchyScreeningPackage") as ManageInstituteHierarchyPackage);

            CurrentViewContext.BundleName = (e.Item.FindControl("txtBundleNameNew") as WclTextBox).Text.Trim();
            CurrentViewContext.BundleDescription = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
            CurrentViewContext.BundleLabel = (e.Item.FindControl("txtLabel") as WclTextBox).Text.Trim();
            CurrentViewContext.HierarchyNode = hdnDepartmentProgmapNew.Value;
            //WclComboBox ddlTrackingPackages = ((WclComboBox)e.Item.FindControl("ddlTrackingPackages"));
            //WclComboBox ddlAdmPackage = ((WclComboBox)e.Item.FindControl("ddlAdmPackage"));
            //WclComboBox ddlscreeningpackage = (e.Item.FindControl("ddlscreeningpackage") as WclComboBox);
            WclComboBox ddlTenantNameNew = (e.Item.FindControl("ddlTenantNameNew") as WclComboBox);
            if (hdnInstNodeIdNew.Value == AppConsts.ZERO || hdnInstNodeIdNew.Value == String.Empty)
            {
                e.Canceled = true;
                (e.Item.FindControl("lblGridMessage") as Label).ShowMessage("Please select Institution Hierarchy for adding a Package Bundle.", MessageType.Error);
                return;
            }

            //if (ddlTrackingPackages.SelectedValue == AppConsts.ZERO && ddlAdmPackage.SelectedValue == AppConsts.ZERO
            //    && ddlscreeningpackage.CheckedItems.Count() == AppConsts.NONE)
            //{
            //    //base.ShowInfoMessage("Please Select Atleast one Package.");
            //    e.Canceled = true;
            //    (e.Item.FindControl("lblGridMessage") as Label).ShowMessage("Please select atleast one Package for adding a Package Bundle.", MessageType.Error);
            //    return;
            //}

            if (ucManageInstituteHierarchyImmunizationPackage.IsNotNull() && ucManageInstituteHierarchyImmunizationPackage.PackageNodeMappingID.IsNullOrEmpty() && ucManageInstituteHierarchyAdministrativePackage.IsNotNull() && ucManageInstituteHierarchyAdministrativePackage.PackageNodeMappingID.IsNullOrEmpty()
                && ucManageInstituteHierarchyScreeningPackage.IsNotNull() && ucManageInstituteHierarchyScreeningPackage.PackageNodeMappingID.IsNullOrEmpty())
            {
                //base.ShowInfoMessage("Please Select Atleast one Package.");
                (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = "";
                e.Canceled = true;
                (e.Item.FindControl("lblGridMessage") as Label).ShowMessage("Please select atleast one Package for adding a Package Bundle.", MessageType.Error);
                return;
            }
            CurrentViewContext.SelectedTenantID = Convert.ToInt32(ddlTenantNameNew.SelectedValue);
            CurrentViewContext.TenantID = Convert.ToInt32(ddlTenantNameNew.SelectedValue);

            //CurrentViewContext.TrackingPackageID = Convert.ToInt32(ddlTrackingPackages.SelectedValue);
            //CurrentViewContext.AdministrativePackageID = Convert.ToInt32(ddlAdmPackage.SelectedValue);
            //CurrentViewContext.TrackingPackageID = Convert.ToInt32(ucManageInstituteHierarchyImmunizationPackage.PackageNodeMappingID);         
            //CurrentViewContext.AdministrativePackageID = Convert.ToInt32(ucManageInstituteHierarchyAdministrativePackage.PackageNodeMappingID);
            if (!ucManageInstituteHierarchyImmunizationPackage.PackageNodeMappingID.IsNullOrEmpty())
            {
                CurrentViewContext.TrackingPackageID = Convert.ToInt32(ucManageInstituteHierarchyImmunizationPackage.PackageNodeMappingID);
            }
            if (!ucManageInstituteHierarchyAdministrativePackage.PackageNodeMappingID.IsNullOrEmpty())
            {
                CurrentViewContext.AdministrativePackageID = Convert.ToInt32(ucManageInstituteHierarchyAdministrativePackage.PackageNodeMappingID);
            }
            //  List<Int32> lstAbc = new List<int>();
            CurrentViewContext.ScreeningpackageIDs = new List<int>();

            if (ucManageInstituteHierarchyScreeningPackage.PackageNodeMappingID != "")
            {
                List<Int32> ScreeningpackageIds = ucManageInstituteHierarchyScreeningPackage.PackageNodeMappingID.Split(',').Select(t => Int32.Parse(t)).ToList();
                CurrentViewContext.ScreeningpackageIDs = ScreeningpackageIds;
            }

            //foreach (RadComboBoxItem item in ddlscreeningpackage.CheckedItems)
            //{
            //    CurrentViewContext.ScreeningpackageIDs.Add(Convert.ToInt32(item.Value));
            //}

            CurrentViewContext.ExplanatoryNotes = (e.Item.FindControl("rdEditorNotes") as WclEditor).Content;
            RadioButtonList rdbIsAvailableforOrder = (e.Item.FindControl("rdAvailableforOrder") as RadioButtonList);
            CurrentViewContext.IsAvailableForOrder = Convert.ToBoolean(rdbIsAvailableforOrder.SelectedValue);
            // CurrentViewContext.IsAvailableForOrder =Convert.ToBoolean( (e.Item.FindControl("rdAvailableforOrder") as RadioButtonList).SelectedValue);



            //if ((CurrentViewContext.TrackingPackageID != 0 || CurrentViewContext.TrackingPackageID != null) && (CurrentViewContext.AdministrativePackageID != 0 || CurrentViewContext.AdministrativePackageID != null))
            //{
            Presenter.InsertPackageBundle();
            if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
            {
                e.Canceled = true;
                //base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                grdPackageBundle.Rebind();
            }
            else
            {
                hdnDepartmentProgmapNew.Value = string.Empty;
                e.Canceled = false;
                base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
            }
            //}
            //else
            //{
            //    base.ShowInfoMessage("Please Select Atleast one Package");
            //    return;
            //}
        }

        protected void grdPackageBundle_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                    if (!ddlTenantName.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(ddlTenantName.SelectedValue) > 0 && ViewState["ReBindGrid"].IsNullOrEmpty())
                    {
                        //ManagePackageBundleContract objMPBC = new ManagePackageBundleContract();
                        //objMPBC.BundleName = txtBundleSrch.Text.Trim();
                        //objMPBC.HierarchyNode = hdnInstitutionNodeId.Value;
                        //objMPBC.TenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
                        _presenter.GetBundleDetail();
                    }

                    if (CurrentViewContext.BundleDataList.IsNull())
                    {
                        CurrentViewContext.BundleDataList = new List<ManagePackageBundleContract>();
                    }
                    grdPackageBundle.DataSource = CurrentViewContext.BundleDataList;
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

        protected void grdPackageBundle_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.BundleId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BundleId"));
                Presenter.DeletePackageBundle();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;

                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                    grdPackageBundle.Rebind();
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                    grdPackageBundle.Rebind();
                }
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

        protected void grdPackageBundle_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditFormItem edititem = (GridEditFormItem)e.Item;
                CurrentViewContext.BundleId = Convert.ToInt32(edititem.GetDataKeyValue("BundleId"));
                //WclComboBox ddlscreeningpackage = (e.Item.FindControl("ddlscreeningpackage") as WclComboBox);
                //WclComboBox ddlTrackingPackages = ((WclComboBox)e.Item.FindControl("ddlTrackingPackages"));
                //WclComboBox ddlAdmPackage = ((WclComboBox)e.Item.FindControl("ddlAdmPackage"));


                ManageInstituteHierarchyPackage ucManageInstituteHierarchyImmunizationPackage = (e.Item.FindControl("ucManageInstituteHierarchyImmunizationPackage") as ManageInstituteHierarchyPackage);
                ManageInstituteHierarchyPackage ucManageInstituteHierarchyAdministrativePackage = (e.Item.FindControl("ucManageInstituteHierarchyAdministrativePackage") as ManageInstituteHierarchyPackage);
                ManageInstituteHierarchyPackage ucManageInstituteHierarchyScreeningPackage = (e.Item.FindControl("ucManageInstituteHierarchyScreeningPackage") as ManageInstituteHierarchyPackage);


                CurrentViewContext.BundleName = (e.Item.FindControl("txtBundleNameNew") as WclTextBox).Text.Trim();
                CurrentViewContext.BundleLabel = (e.Item.FindControl("txtLabel") as WclTextBox).Text.Trim();
                CurrentViewContext.BundleDescription = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.ExplanatoryNotes = (e.Item.FindControl("rdEditorNotes") as WclEditor).Content;

                //CurrentViewContext.TrackingPackageID = Convert.ToInt32(ddlTrackingPackages.SelectedValue);
                //CurrentViewContext.AdministrativePackageID = Convert.ToInt32(ddlAdmPackage.SelectedValue);
                if (!ucManageInstituteHierarchyImmunizationPackage.PackageNodeMappingID.IsNullOrEmpty())
                {
                    CurrentViewContext.TrackingPackageID = Convert.ToInt32(ucManageInstituteHierarchyImmunizationPackage.PackageNodeMappingID);
                }
                if (!ucManageInstituteHierarchyAdministrativePackage.PackageNodeMappingID.IsNullOrEmpty())
                {
                    CurrentViewContext.AdministrativePackageID = Convert.ToInt32(ucManageInstituteHierarchyAdministrativePackage.PackageNodeMappingID);
                }
                CurrentViewContext.HierarchyNode = hdnDepartmentProgmapNew.Value;

                if (hdnInstNodeIdNew.Value == AppConsts.ZERO || hdnInstNodeIdNew.Value == String.Empty)
                {
                    e.Canceled = true;
                    (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = "";
                    (e.Item.FindControl("lblGridMessage") as Label).ShowMessage("Please select Institution Hierarchy for adding a Package Bundle.", MessageType.Error);
                    return;
                }
                //if (ddlTrackingPackages.SelectedValue == AppConsts.ZERO && ddlAdmPackage.SelectedValue == AppConsts.ZERO
                //                                                        && ddlscreeningpackage.CheckedItems.Count() == AppConsts.NONE)
                //{
                //    e.Canceled = true;
                //    (e.Item.FindControl("lblGridMessage") as Label).ShowMessage("Please select atleast one Package for adding a Package Bundle.", MessageType.Error);
                //    return;
                //}
                if (ucManageInstituteHierarchyImmunizationPackage.IsNotNull() && ucManageInstituteHierarchyImmunizationPackage.PackageNodeMappingID.IsNullOrEmpty() && ucManageInstituteHierarchyAdministrativePackage.IsNotNull() && ucManageInstituteHierarchyAdministrativePackage.PackageNodeMappingID.IsNullOrEmpty()
                     && ucManageInstituteHierarchyScreeningPackage.IsNotNull() && ucManageInstituteHierarchyScreeningPackage.PackageNodeMappingID.IsNullOrEmpty())
                {
                    //base.ShowInfoMessage("Please Select Atleast one Package.");
                    e.Canceled = true;
                    (e.Item.FindControl("lblGridMessage") as Label).ShowMessage("Please select atleast one Package for adding a Package Bundle.", MessageType.Error);
                    return;
                }
                CurrentViewContext.ScreeningpackageIDs = new List<int>();
                //foreach (RadComboBoxItem item in ddlscreeningpackage.CheckedItems)
                //{
                //    CurrentViewContext.ScreeningpackageIDs.Add(Convert.ToInt32(item.Value));
                //}

                if (ucManageInstituteHierarchyScreeningPackage.PackageNodeMappingID != "")
                {
                    List<Int32> ScreeningpackageIds = ucManageInstituteHierarchyScreeningPackage.PackageNodeMappingID.Split(',').Select(t => Int32.Parse(t)).ToList();
                    CurrentViewContext.ScreeningpackageIDs = ScreeningpackageIds;
                }

                RadioButtonList rdbIsAvailableforOrder = (e.Item.FindControl("rdAvailableforOrder") as RadioButtonList);
                CurrentViewContext.IsAvailableForOrder = Convert.ToBoolean(rdbIsAvailableforOrder.SelectedValue);
                Presenter.EditPackageBundle();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    //base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else
                {
                    hdnDepartmentProgmapNew.Value = string.Empty;
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
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

        #region Button Events

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), false);
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

        protected void Search_Click(object sender, EventArgs e)
        {
            grdPackageBundle.Visible = true;
            ViewState["ReBindGrid"] = null;
            grdPackageBundle.Rebind();
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            ResetControls(true);
            ResetGridFilters();
            grdPackageBundle.Rebind(); 
        }

        #endregion

        #region Button Events

        protected void ddlTenantName_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ResetControls(false);
            //ResetGridFilters();
        }

        protected void ddlTenantNameNew_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                GridEditFormInsertItem insertItem = (sender as WclComboBox).NamingContainer as GridEditFormInsertItem;
                String selectedValue = (sender as WclComboBox).SelectedValue;
                CurrentViewContext.SelectedTenantIDForAddForm = selectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(selectedValue);

                //WclComboBox ddlTrackingPackages = insertItem.FindControl("ddlTrackingPackages") as WclComboBox;
                //WclComboBox ddlAdmPackage = insertItem.FindControl("ddlAdmPackage") as WclComboBox;
                //WclComboBox ddlscreeningpackage = insertItem.FindControl("ddlscreeningpackage") as WclComboBox;

                //List<PackageBundlePackages> packageBundlePackagesList = Presenter.GetPackageBundlePackages();

                ////Bind Tracking Compliance Package 
                //BindTrackingCompliancePackages(ddlTrackingPackages, packageBundlePackagesList);

                ////Bind Administrative Compliance Package 
                //BindAdministrativeCompliancePackages(ddlAdmPackage, packageBundlePackagesList);

                ////Bind Screening Compliance Package 
                //BindScreeningPackages(ddlscreeningpackage, packageBundlePackagesList);


                ManageInstituteHierarchyPackage ucManageInstituteHierarchyImmunizationPackage = (insertItem.FindControl("ucManageInstituteHierarchyImmunizationPackage") as ManageInstituteHierarchyPackage);
                ucManageInstituteHierarchyImmunizationPackage.TenantID = Convert.ToInt32(selectedValue);

                //ucManageInstituteHierarchyImmunizationPackage.CompliancePackageTypeCode = CompliancePackageTypes.IMMUNIZATION_COMPLIANCE_PACKAGE.GetStringValue();
                //ucManageInstituteHierarchyImmunizationPackage.IsCompliancePackage = true;

                ManageInstituteHierarchyPackage ucManageInstituteHierarchyAdministrativePackage = (insertItem.FindControl("ucManageInstituteHierarchyAdministrativePackage") as ManageInstituteHierarchyPackage);
                ucManageInstituteHierarchyAdministrativePackage.TenantID = Convert.ToInt32(selectedValue);
                //ucManageInstituteHierarchyAdministrativePackage.CompliancePackageTypeCode = CompliancePackageTypes.ADMINISTRATIVE_COMPLIANCE_PACKAGE.GetStringValue();
                //ucManageInstituteHierarchyAdministrativePackage.IsCompliancePackage = true;
                ManageInstituteHierarchyPackage ucManageInstituteHierarchyScreeningPackage = (insertItem.FindControl("ucManageInstituteHierarchyScreeningPackage") as ManageInstituteHierarchyPackage);
                ucManageInstituteHierarchyScreeningPackage.TenantID = Convert.ToInt32(selectedValue);
                //ucManageInstituteHierarchyScreeningPackage.CompliancePackageTypeCode = String.Empty;
                //ucManageInstituteHierarchyScreeningPackage.IsCompliancePackage = false;

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

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// Get search parameters
        /// </summary>
        private void GetSearchParameters()
        {
            _searchContract = new ManagePackageBundleContract();
            _searchContract.TenantId = CurrentViewContext.SelectedTenantID;
            if (!txtBundleSrch.Text.Trim().IsNullOrEmpty())
            {
                _searchContract.BundleName = txtBundleSrch.Text.Trim();
            }
            if (!hdnInstitutionNodeId.Value.Trim().IsNullOrEmpty())
            {
                _searchContract.HierarchyNode = hdnDepartmntPrgrmMppng.Value.Trim();
            }

        }

        private void BindTenant()
        {
            _presenter.GetTenants();
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataTextField = "TenantName";
            ddlTenantName.DataValueField = "TenantID";
            ddlTenantName.DataBind();
            ddlTenantName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--", AppConsts.ZERO));

            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantID = AppConsts.NONE;
            }
            else
            {
                CurrentViewContext.SelectedTenantID = CurrentViewContext.TenantID;
            }
        }

        //private static void BindTrackingCompliancePackages(WclComboBox ddlTrackingPackages, List<PackageBundlePackages> packageBundlePackages)
        //{
        //    String trackingPackageCode = CompliancePackageTypes.IMMUNIZATION_COMPLIANCE_PACKAGE.GetStringValue();

        //    List<PackageBundlePackages> trackingPackages = packageBundlePackages.Where(cond => cond.IsCompliancePackage == true
        //                                                                              && cond.PackageTypeCode == trackingPackageCode).ToList();
        //    ddlTrackingPackages.DataSource = trackingPackages;
        //    ddlTrackingPackages.DataTextField = "PackageName";
        //    ddlTrackingPackages.DataValueField = "PackageNodeMappingID";
        //    ddlTrackingPackages.DataBind();
        //    ddlTrackingPackages.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--", AppConsts.ZERO));
        //}

        //private static void BindAdministrativeCompliancePackages(WclComboBox ddlAdmPackages, List<PackageBundlePackages> packageBundlePackages)
        //{
        //    String administrativePackageCode = CompliancePackageTypes.ADMINISTRATIVE_COMPLIANCE_PACKAGE.GetStringValue();
        //    List<PackageBundlePackages> adminitrativePackages = packageBundlePackages.Where(cond => cond.IsCompliancePackage == true
        //                                                                          && cond.PackageTypeCode == administrativePackageCode).ToList();
        //    ddlAdmPackages.DataSource = adminitrativePackages;
        //    ddlAdmPackages.DataTextField = "PackageName";
        //    ddlAdmPackages.DataValueField = "PackageNodeMappingID";
        //    ddlAdmPackages.DataBind();
        //    ddlAdmPackages.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--", AppConsts.ZERO));
        //}

        //private static void BindScreeningPackages(WclComboBox ddlScreeningpackages, List<PackageBundlePackages> packageBundlePackages)
        //{
        //    List<PackageBundlePackages> screeningPackages = packageBundlePackages.Where(cond => cond.IsCompliancePackage == false).ToList();
        //    ddlScreeningpackages.DataSource = screeningPackages;
        //    ddlScreeningpackages.DataTextField = "PackageName";
        //    ddlScreeningpackages.DataValueField = "PackageNodeMappingID";
        //    ddlScreeningpackages.DataBind();
        //    //ddlScreeningpackages.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--", AppConsts.ZERO));
        //}

        /// <summary>
        /// Method to Reset Grid 
        /// </summary>
        private void ResetGridFilters()
        {
            grdPackageBundle.MasterTableView.SortExpressions.Clear();
            grdPackageBundle.CurrentPageIndex = 0;
            grdPackageBundle.MasterTableView.CurrentPageIndex = 0;
            grdPackageBundle.MasterTableView.IsItemInserted = false;
            grdPackageBundle.MasterTableView.ClearEditItems();

        }

        private void ResetControls(Boolean rebindGrid)
        {
            txtBundleSrch.Text = string.Empty;
            hdnDepartmentProgmapNew.Value = String.Empty;
            hdnDepartmntPrgrmMppng.Value = String.Empty;
            hdnHierarchyLabel.Value = String.Empty;
            hdnInstitutionNodeId.Value = String.Empty;
            hdnInstNodeIdNew.Value = String.Empty;
            lblInstituteHierarchyName.Text = String.Empty;
            //ResetGridFilters(); //UAT-3874
            if (rebindGrid)
            {
                hdnTenantIdNew.Value = String.Empty;
                hdnTenantId.Value = String.Empty;
                CurrentViewContext.SelectedTenantID = AppConsts.NONE;
                ViewState["ReBindGrid"] = null;
            }
            else
            {
                ViewState["ReBindGrid"] = "false";
            }
            /*Start UAT-3157*/
            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                GetPreferredSelectedTenant();
                //ViewState["ReBindGrid"] = "false";
            }
            /*END UAT-3157*/
            //grdPackageBundle.Rebind();
        }

        #region UAT-3157:- Sticky institution

        private void GetPreferredSelectedTenant()
        {
            if (CurrentViewContext.SelectedTenantID.IsNullOrEmpty() || CurrentViewContext.SelectedTenantID == AppConsts.NONE)
            {
                // Presenter.GetPreferredSelectedTenant();
                CurrentViewContext.PreferredSelectedTenantID = (Session["PreferredSelectedTenant"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(Session["PreferredSelectedTenant"]);
                if (CurrentViewContext.PreferredSelectedTenantID > AppConsts.NONE)
                {
                    ddlTenantName.SelectedValue = Convert.ToString(CurrentViewContext.PreferredSelectedTenantID);
                    CurrentViewContext.SelectedTenantID = Convert.ToInt32(ddlTenantName.SelectedValue);
                }
            }
        }
        #endregion

        #endregion

        #endregion
    }
}