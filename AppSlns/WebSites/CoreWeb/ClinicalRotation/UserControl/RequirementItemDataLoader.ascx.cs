using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.ClinicalRotation.Views;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RequirementItemDataLoader : BaseUserControl, IRequirementItemDataLoader
    {

        #region Variables

        private RequirementItemDataLoaderPresenter _presenter = new RequirementItemDataLoaderPresenter();

        #endregion;

        public RequirementItemDataLoaderPresenter Presenter
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


        List<RequirementVerificationDetailContract> IRequirementItemDataLoader.lstCategoryData
        {
            get;
            set;
        }

        /// <summary>
        /// Reporesents the TenantID of the Applicant
        /// </summary>
        Int32 IRequirementItemDataLoader.TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// prefix for the Item Control
        /// </summary>
        String IRequirementItemDataLoader.ItemControlIdPrefix
        {
            get
            {
                return "ucItemControl_" + CurrentViewContext.CategoryId + "_";
            }
        }

        /// <summary>
        /// Represents the CategoryId
        /// </summary>
        Int32 IRequirementItemDataLoader.CategoryId
        {
            get;
            set;
        }


        /// <summary>
        /// List for 'lkpRequirementItemStatus' entity
        /// </summary>
        List<RequirementItemStatusContract> IRequirementItemDataLoader.lstReqItemStatusTypes
        {
            get;
            set;
        }

        /// <summary>
        /// ApplicantRequirementCategoryDataID
        /// </summary>
        Int32 IRequirementItemDataLoader.ApplReqCatDataId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the screen from which the screen was opened
        /// </summary>
        String IRequirementItemDataLoader.ControlUseType
        {
            get;
            set;
        }

        public IRequirementItemDataLoader CurrentViewContext
        {
            get { return this; }
        }

        string IRequirementItemDataLoader.EntityPermissionName
        {
            get;
            set;
        }

        public List<RequirementObjectPropertiesContract> lstEditableByData
        {
            get;
            set;
        }

        #region UAT-2224: Admin access to upload/associate documents on rotation package items.

        public Int32 SelectedApplicantId_Global
        {
            get
            {
                if (!ViewState["SelectedApplicantIdLoader"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedApplicantIdLoader"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedApplicantIdLoader"].IsNullOrEmpty())
                    ViewState["SelectedApplicantIdLoader"] = value;
            }
        }

        public Int32 CurrentRequirementPackageSubscriptionID_Global
        {
            get
            {
                if (!ViewState["CurrentRequirementPackageSubscriptionIDLoader"].IsNullOrEmpty())
                    return (Int32)(ViewState["CurrentRequirementPackageSubscriptionIDLoader"]);
                else
                    return 0;
            }
            set { ViewState["CurrentRequirementPackageSubscriptionIDLoader"] = value; }
        }

        public List<ApplicantDocumentContract> lstApplicantDocument
        {
            get;
            set;
        }

        public ApplicantRequirementItemDataContract RequirementItemData { get; set; }

        public String CurrentLoggedInUserName_Global
        {
            get
            {
                if (!ViewState["CurrentLoggedInUserNameLoader"].IsNullOrEmpty())
                    return (String)(ViewState["CurrentLoggedInUserNameLoader"]);
                else
                    return String.Empty;
            }
            set
            {
                if (ViewState["CurrentLoggedInUserNameLoader"].IsNullOrEmpty())
                    ViewState["CurrentLoggedInUserNameLoader"] = value;
            }
        }
        public Int32 CurrentTenantId_Global
        {
            get
            {
                if (!ViewState["CurrentTenantIdLoader"].IsNullOrEmpty())
                    return (Int32)(ViewState["CurrentTenantIdLoader"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["CurrentTenantIdLoader"].IsNullOrEmpty())
                    ViewState["CurrentTenantIdLoader"] = value;
            }
        }
        public Boolean IsAdminLoggedIn { get; set; }

        public Boolean IsClientAdminLoggedIn { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentViewContext.ApplReqCatDataId = CurrentViewContext.lstCategoryData.First().ApplReqCatDataId;
            GenerateItemControls();
            Presenter.IsAdminLoggedIn();
            Presenter.IsClientAdmin();
        }

        /// <summary>
        /// Generate the Item level controls
        /// </summary>
        private void GenerateItemControls()
        {
            Presenter.GetReqItemStatusTypes();
            var _distinctItems = CurrentViewContext.lstCategoryData.OrderBy(vdd => vdd.RequirementItemDisplayOrder).Select(vdd => vdd.ItemId).Distinct().ToList();

            //UAT-2224: Admin access to upload/associate documents on rotation package items.
            //Get applicant documents
            Presenter.GetDocuments();
            // this.lstEditableByData = Presenter.GetEditableBiesDataByCategoryId(); //UAT-4165
            foreach (var itemId in _distinctItems)
            {
                System.Web.UI.Control _itemControl = Page.LoadControl("~/ClinicalRotation/UserControl/RequirementVerificationItemControl.ascx");
                (_itemControl as IRequirementVerificationItemControl).lstItemLevelData = CurrentViewContext.lstCategoryData.Where(vdd => vdd.ItemId == itemId).OrderBy(o => o.RequirementItemDisplayOrder).ToList();
                (_itemControl as IRequirementVerificationItemControl).lstReqItemStatusTypes = CurrentViewContext.lstReqItemStatusTypes;
                (_itemControl as IRequirementVerificationItemControl).ItemId = itemId;
                (_itemControl as IRequirementVerificationItemControl).SelectedTenantId = CurrentViewContext.TenantId;
                (_itemControl as RequirementVerificationItemControl).ID = CurrentViewContext.ItemControlIdPrefix + itemId;
                (_itemControl as IRequirementVerificationItemControl).ControlUseType = CurrentViewContext.ControlUseType;

                //UAT-2224: Admin access to upload/associate documents on rotation package items.
                (_itemControl as IRequirementVerificationItemControl).SelectedApplicantId_Global = CurrentViewContext.SelectedApplicantId_Global;
                (_itemControl as IRequirementVerificationItemControl).CurrentRequirementPackageSubscriptionID_Global = CurrentViewContext.CurrentRequirementPackageSubscriptionID_Global;
                (_itemControl as IRequirementVerificationItemControl).lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
                (_itemControl as IRequirementVerificationItemControl).CurrentTenantId_Global = CurrentViewContext.CurrentTenantId_Global;
                //Get mapped documents
                Presenter.GetApplicantRequirementItemData(itemId);
                if (CurrentViewContext.RequirementItemData.IsNotNull() && !CurrentViewContext.RequirementItemData.ApplicantRequirementFieldData.IsNullOrEmpty())
                {
                    var applicantRequirementFieldData = CurrentViewContext.RequirementItemData.ApplicantRequirementFieldData;
                    var applicantFieldData = applicantRequirementFieldData.Where(x => x.FieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue()).FirstOrDefault();

                    if (applicantFieldData.IsNotNull() && !applicantFieldData.LstApplicantFieldDocumentMapping.IsNullOrEmpty())
                    {
                        (_itemControl as IRequirementVerificationItemControl).lstApplicantRequirementDocumentMaps = applicantFieldData.LstApplicantFieldDocumentMapping;
                    }
                }

                //UAT 2371
                (_itemControl as IRequirementVerificationItemControl).EntityPermissionName = CurrentViewContext.EntityPermissionName;

                //UAT 4165
                Boolean isEditableByAdmin = Convert.ToBoolean(CurrentViewContext.lstCategoryData.Where(vdd => vdd.ItemId == itemId).Select(c => c.IsEditableByAdmin).FirstOrDefault());
                Boolean isEditableByClientAdmin = Convert.ToBoolean(CurrentViewContext.lstCategoryData.Where(vdd => vdd.ItemId == itemId).Select(c => c.IsEditableByClientAdmin).FirstOrDefault());
                Boolean isEditableByApplicant = Convert.ToBoolean(CurrentViewContext.lstCategoryData.Where(vdd => vdd.ItemId == itemId).Select(c => c.IsEditableByApplicant).FirstOrDefault());
                Presenter.IsAdminLoggedIn();
                Presenter.IsClientAdmin();
                if (CurrentViewContext.IsAdminLoggedIn)
                {
                    if (isEditableByAdmin)
                    {
                        (_itemControl as IRequirementVerificationItemControl).IsItemEditable = true;
                    }
                    else
                    {
                        (_itemControl as IRequirementVerificationItemControl).IsItemEditable = false;
                    }
                    (_itemControl as IRequirementVerificationItemControl).IsAdminLoggedIn = true;
                }

                if (CurrentViewContext.IsClientAdminLoggedIn)
                {
                    if (isEditableByClientAdmin)
                    {
                        (_itemControl as IRequirementVerificationItemControl).IsItemEditable = true;
                    }
                    else
                    {
                        (_itemControl as IRequirementVerificationItemControl).IsItemEditable = false;
                    }
                    (_itemControl as IRequirementVerificationItemControl).IsClientAdminLoggedIn = true;
                }

                pnlItemContainer.Controls.Add(_itemControl);
            }
        }

        #region Public Methods

        /// <summary>
        /// Get the Category, it's item amd then Field level Data, entered by the admin
        /// </summary>
        public RequirementVerificationCategoryData GetItemData()
        {
            var _reqVerificationData = new RequirementVerificationCategoryData();

            _reqVerificationData.CatId = CurrentViewContext.CategoryId;
            _reqVerificationData.ApplicantCatDataId = CurrentViewContext.ApplReqCatDataId;
            _reqVerificationData.lstItemData = new List<RequirementVerificationItemData>();
            var _distinctItems = CurrentViewContext.lstCategoryData.OrderBy(vdd => vdd.ItemId).Select(vdd => vdd.ItemId).Distinct().ToList();

            foreach (var itemId in _distinctItems)
            {
                var _ctrl = pnlItemContainer.FindServerControlRecursively(CurrentViewContext.ItemControlIdPrefix + itemId);
                if (_ctrl.IsNotNull() && _ctrl is RequirementVerificationItemControl)
                {
                    var _itemControl = _ctrl as RequirementVerificationItemControl;
                    var _itemData = _itemControl.GetItemFieldData();
                    if (_itemData.IsNotNull())
                    {
                        _reqVerificationData.lstItemData.Add(_itemData);
                    }
                }
            }
            return _reqVerificationData;
        }

        #region UAT-3345
        public Int32 saveDocumentMappings()
        {
            foreach (Object control in pnlItemContainer.Controls)
            {
                var itemcontrol = control as RequirementVerificationItemControl;
                if (itemcontrol.IsNotNull())
                {
                    var resultCatDataId = itemcontrol.saveDocumentMappings();
                    if (resultCatDataId > 0)
                    {
                        CurrentViewContext.ApplReqCatDataId = resultCatDataId;
                    }
                }

            }
            return CurrentViewContext.ApplReqCatDataId;

        }
        #endregion
        #endregion


        #region UAT-4260

        public void SetUIValidationMessage(Dictionary<Int32, String> _dicValidationMessages)
        {
            for (int i = 0; i < pnlItemContainer.Controls.Count; i++)
            {
                if (pnlItemContainer.Controls[i] is IRequirementVerificationItemControl)
                {
                    String _validationMessage = "Item could not be validated";
                    IRequirementVerificationItemControl _editModeControl = pnlItemContainer.Controls[i] as IRequirementVerificationItemControl;

                    KeyValuePair<Int32, String> _keyValuePair = _dicValidationMessages.Where(vm => vm.Key == _editModeControl.ItemId).FirstOrDefault();

                    if (!_keyValuePair.IsNullOrEmpty())
                        _validationMessage = _keyValuePair.Value;

                    _editModeControl.SetValidationMessage(_validationMessage);
                }
            }
        }
        #endregion
    }
}