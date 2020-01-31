using System;
using System.Linq;
using INTSOF.Utils;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Collections.Generic;
using CoreWeb.ComplianceOperations.Views;


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class CategoryDataEntry : System.Web.UI.Page, ICategoryDataEntryView
    {
        private CategoryDataEntryPresenter _presenter=new CategoryDataEntryPresenter();
        private ClientComplianceItemsContract _currentViewContract;

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public ClientComplianceItemsContract ClientComplianceItemsContract
        {
            get
            {
                if (_currentViewContract == null)
                    _currentViewContract = new ClientComplianceItemsContract();
                return _currentViewContract;
            }
        }

        public Int32 TenantId
        {
            get;
            set;
        }

        public List<Entity.ClientEntities.ClientComplianceItem> ClientComplianceItems
        {
            get;
            set;
        }

        public ICategoryDataEntryView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 PackageSubscriptionId
        {
            get;
            set;
        }

        public Entity.ClientEntities.ApplicantComplianceCategoryData ApplicantCategoryData
        {
            get;
            set;
        }

        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Fixed Values - To be changed when screens are linked

            CurrentViewContext.TenantId = 35;
            helpText.InnerText = "This is test help text";
            CurrentViewContext.PackageSubscriptionId = 5;
            ClientComplianceItemsContract.ClientComplianceCategoryId = 75;

            #endregion

            if (!this.IsPostBack)
            {
                LoadInitialData();
            }
            Presenter.OnViewLoaded();
            GenerateForm(false);
        }

        private void LoadInitialData()
        {
            Presenter.OnViewInitialized();

            if (CurrentViewContext.ApplicantCategoryData.IsNotNull())
                hdf.Value = Convert.ToString(CurrentViewContext.ApplicantCategoryData.ApplicantComplianceCategoryDataID);
        }

        public void GenerateForm(Boolean isSaved)
        {
            if (isSaved)
            {
                LoadInitialData();
                //pnl.Controls.Clear();
            }
            CurrentViewContext.ClientComplianceItems = CurrentViewContext.ClientComplianceItems.OrderBy(clientItems => clientItems.DisplayOrder).ToList();

            foreach (var clientItem in CurrentViewContext.ClientComplianceItems)
            {
                System.Web.UI.Control ctrl = Page.LoadControl("~\\ComplianceOperations\\UserControl\\ItemDetails.ascx");
                
                //ItemDetails ucItemDetails = ctrl as ItemDetails;
                //ucItemDetails.ItemName = clientItem.Name;
                //ucItemDetails.ItemId = clientItem.ClientComplianceItemID;
                //ucItemDetails.ID = "ucItemDetails_" + Convert.ToString(clientItem.ClientComplianceItemID);
                //ucItemDetails.ClientItemAttributes = clientItem.ClientComplianceItemAttributes.ToList();
                //ucItemDetails.ComplianceCategoryId = ClientComplianceItemsContract.ClientComplianceCategoryId;
                //ucItemDetails.PackageSubscriptionId = CurrentViewContext.PackageSubscriptionId;
                //ucItemDetails.TenantId = CurrentViewContext.TenantId;
                //if (CurrentViewContext.ApplicantCategoryData.IsNotNull() && CurrentViewContext.ApplicantCategoryData.ApplicantComplianceItemDatas.Count() > 0)
                //    ucItemDetails.ApplicantItemData = CurrentViewContext.ApplicantCategoryData.ApplicantComplianceItemDatas.Where(itemData => itemData.ComplianceItemID == clientItem.ClientComplianceItemID).FirstOrDefault();

                //if (!String.IsNullOrEmpty(hdf.Value))
                //    ucItemDetails.ApplicantComplianceCategoryIdGenerated = Convert.ToInt32(hdf.Value);

                //pnl.Controls.Add(ucItemDetails);
            }
        }

        public EventHandler a;

        
        public CategoryDataEntryPresenter Presenter
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

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //
    }
}

