using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.BkgSetup.Views;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.UserControl
{
    public partial class OverridePackageServiceFormType : BaseUserControl, IOverridePackageServiceFormDispatchType
    {
        private OverridePackageServiceFormDispatchTypePresenter _presenter = new OverridePackageServiceFormDispatchTypePresenter();

        #region Properties

        #region Public Properties

        public OverridePackageServiceFormDispatchTypePresenter Presenter
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

        /// <summary>
        /// Id of the Tenant Selected by the admin
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        /// <summary>
        /// Id of the Service selected by the admin
        /// </summary>
        public Int32 BkgSvcId
        {
            get
            {
                return Convert.ToInt32(ViewState["BkgSvcId"]);
            }
            set
            {
                ViewState["BkgSvcId"] = value;
            }
        }

        /// <summary>
        /// Id of the Package selected by the admin
        /// </summary>
        public Int32 PackageId
        {
            get
            {
                return Convert.ToInt32(ViewState["PackageId"]);
            }
            set
            {
                ViewState["PackageId"] = value;
            }
        }

        /// <summary>
        /// Id of the Package selected by the admin
        /// </summary>
        public Int32 BPSId
        {
            get
            {
                return Convert.ToInt32(ViewState["BPSId"]);
            }
            set
            {
                ViewState["BPSId"] = value.IsNull() ? AppConsts.NONE : value;
            }
        }

        #region Private Properties

        List<ServiceFormsDispatchTypesContract> _lstSvcFormsDispatchTypes
        {
            get;
            set;
        }

        #endregion
        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.PackageId == AppConsts.NONE)
                    return;
            }
        }

        protected void rptServiceForms_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var _dataItem = ((ServiceFormsDispatchTypesContract)(e.Item.DataItem));

            var rbtnList = (e.Item.FindControl("rbtnList") as RadioButtonList);

            var _stsLiteral = (e.Item.FindControl("hdnInheritedStatus") as HiddenField);
            if (rbtnList.IsNull() || _stsLiteral.IsNull())
                return;

            _stsLiteral.Value = GetInheritedStatus(_dataItem);
            rbtnList.Enabled = _dataItem.IsRootLevelAuto ? true : false;

            if (_dataItem.IsPackageLevelAutomatic.IsNull())
                rbtnList.SelectedValue = ServiceFormDispatchType.DEFAULT.GetStringValue();
            else if (_dataItem.IsPackageLevelAutomatic.IsNotNull() && Convert.ToBoolean(_dataItem.IsPackageLevelAutomatic))
                rbtnList.SelectedValue = ServiceFormDispatchType.AUTOMATIC.GetStringValue();
            else if (_dataItem.IsPackageLevelAutomatic.IsNotNull() && !Convert.ToBoolean(_dataItem.IsPackageLevelAutomatic))
                rbtnList.SelectedValue = ServiceFormDispatchType.MANUAL.GetStringValue();
        }

        #endregion

        #region Methods

        #region Public Methods

        public Boolean ShowServiceForms()
        {
            _lstSvcFormsDispatchTypes = this.Presenter.GetServiceFormDispatchType();
            rptServiceForms.DataSource = _lstSvcFormsDispatchTypes;
            rptServiceForms.DataBind();
            return _lstSvcFormsDispatchTypes.IsNullOrEmpty() ? false : true;
        }

        /// <summary>
        /// Method to get the Data which is to be Added when new Service is added to a service group
        /// </summary>
        /// <returns></returns>
        public String GetAddDataXML()
        {
            var _addData = false;
            StringBuilder _sbXML = new StringBuilder();
            _sbXML.Append("<BPSFOData>");

            foreach (RepeaterItem rptItem in rptServiceForms.Items)
            {
                var _rbtnList = rptItem.FindControl("rbtnList") as RadioButtonList;
                var _hdfSAFMId = rptItem.FindControl("hdfSAFMId") as HiddenField;
                var _chkVisibility = rptItem.FindControl("chkVisibility") as CheckBox;

                if (_hdfSAFMId.IsNotNull() && _hdfSAFMId.IsNotNull() && _chkVisibility.IsNotNull() && IsAddRequired(_rbtnList, _chkVisibility, AppConsts.NONE))
                {
                    _sbXML.Append("<BPSFO>");
                    _sbXML.Append("<SAFMId>" + _hdfSAFMId.Value + "</SAFMId>");

                    var _selectedType = _rbtnList.SelectedValue;
                    if (_selectedType == ServiceFormDispatchType.DEFAULT.GetStringValue())
                        _sbXML.Append("<IsAuto></IsAuto>");
                    else if (_selectedType == ServiceFormDispatchType.AUTOMATIC.GetStringValue())
                        _sbXML.Append("<IsAuto>1</IsAuto>");
                    else
                        _sbXML.Append("<IsAuto>0</IsAuto>");

                    _sbXML.Append("<HideSvcForm> " + (_chkVisibility.Checked == true ? "1" : "0") + "</HideSvcForm>");
                    _sbXML.Append("</BPSFO>");
                    _addData = true;
                }
            }
            _sbXML.Append("</BPSFOData>");
            if (_addData)
                return Convert.ToString(_sbXML);

            return String.Empty;
        }

        /// <summary>
        /// Get the data to be added or updated, when a Service is updated in a Service Group 
        /// </summary>
        /// <returns></returns>
        public List<BkgPackageSvcFormOverride> GetUpdateData()
        {
            var currentDateTime = DateTime.Now;
            List<BkgPackageSvcFormOverride> _lst = new List<BkgPackageSvcFormOverride>();
            foreach (RepeaterItem rptItem in rptServiceForms.Items)
            {
                var _rbtnList = rptItem.FindControl("rbtnList") as RadioButtonList;
                var _hdfSAFMId = rptItem.FindControl("hdfSAFMId") as HiddenField;
                var _chkVisibility = rptItem.FindControl("chkVisibility") as CheckBox;
                var _hdfBPSOId = rptItem.FindControl("hdfBPSOId") as HiddenField;

                if (_hdfBPSOId.IsNotNull() && _hdfSAFMId.IsNotNull() && _hdfSAFMId.IsNotNull() && _chkVisibility.IsNotNull() && IsAddRequired(_rbtnList, _chkVisibility, Convert.ToInt32(_hdfBPSOId.Value)))
                {
                    var _selectedType = _rbtnList.SelectedValue;

                    Boolean? _isPkgLevelAutomatic = null;

                    if (_selectedType == ServiceFormDispatchType.AUTOMATIC.GetStringValue())
                        _isPkgLevelAutomatic = true;
                    else if (_selectedType == ServiceFormDispatchType.MANUAL.GetStringValue())
                        _isPkgLevelAutomatic = false;
                    if (_hdfBPSOId.Value.IsNull() || _hdfBPSOId.Value == "0")
                    {
                        _lst.Add(new BkgPackageSvcFormOverride
                        {
                            BPSO_ServiceAttachedFormMapping = Convert.ToInt32(_hdfSAFMId.Value),
                            BPSO_IsAutomatic = _isPkgLevelAutomatic,
                            BPSO_HideServiceForm = _chkVisibility.Checked,
                            BPSO_IsDeleted = false,
                            BPSO_CreatedBy = base.CurrentUserId,
                            BPSO_CreatedOn = currentDateTime,
                        });
                    }
                    else
                    {
                        _lst.Add(new BkgPackageSvcFormOverride
                        {
                            BPSO_ID = Convert.ToInt32(_hdfBPSOId.Value),
                            BPSO_HideServiceForm = _chkVisibility.Checked,
                            BPSO_IsAutomatic = _isPkgLevelAutomatic,
                            BPSO_ModifiedByID = base.CurrentUserId,
                            BPSO_ModifiedOn = currentDateTime
                        });
                    }
                }
            }
            return _lst;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Check if there is any need to add a Pacakge Level Service form dispatch mode, 
        /// depending on the admin selection of the settings
        /// </summary>
        /// <param name="rbtnList"></param>
        /// <param name="chkHideForm"></param>
        /// <returns></returns>
        private Boolean IsAddRequired(RadioButtonList rbtnList, CheckBox chkHideForm, Int32 bpsoId)
        {
            if ((rbtnList.SelectedValue != "AAAA" || chkHideForm.Checked) || (bpsoId > AppConsts.NONE))
                return true;
            return false;
        }

        /// <summary>
        /// Get the inherited status to be displayed for the Overriding of the Dispatch mode
        /// </summary>
        /// <param name="svcFormDispatchTypes"></param>
        /// <returns></returns>
        private String GetInheritedStatus(ServiceFormsDispatchTypesContract svcFormDispatchTypes)
        {
            if (!svcFormDispatchTypes.IsRootLevelAuto)
                return "Manual";


            if (svcFormDispatchTypes.IsRootLevelAuto && (
               (svcFormDispatchTypes.EnforceManual.IsNullOrEmpty()
               ||
               !Convert.ToBoolean(svcFormDispatchTypes.EnforceManual))))
            {
                return "Automatic";
            }
            else
                return "Manual";
        }

        #endregion

        #endregion
    }
}