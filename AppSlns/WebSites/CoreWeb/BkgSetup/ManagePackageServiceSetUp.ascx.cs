using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManagePackageServiceSetUp : BaseUserControl, IManagePackageServiceSetUpView
    {
        #region Variables

        #region Private variables
        private ManagePackageServiceSetUpPresenter _presenter = new ManagePackageServiceSetUpPresenter();
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                ApplyActionLevelPermission(ActionCollection, "Manage Package Service SetUp");
            }
            Presenter.OnViewLoaded();
            Response.Redirect("Pages/ManagePackageServiceHierarchy.aspx");
        }

        #endregion

        #region Properties

        #region Public Properties
        public ManagePackageServiceSetUpPresenter Presenter
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

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        #endregion

        #region Public Methods

        #endregion

        #endregion

        #region Apply Permissions

        public override List<ClsFeatureAction> ActionCollection
        {
            get
            {
                List<ClsFeatureAction> actionCollection = new List<ClsFeatureAction>();
                actionCollection.Add(new ClsFeatureAction
                {
                    CustomActionId = "Show Institution",
                    CustomActionLabel = "Show Institution",
                    ScreenName = "Manage Package Service SetUp"
                });
                actionCollection.Add(new ClsFeatureAction
                {
                    CustomActionId = "Delete Package",
                    CustomActionLabel = "Delete Package",
                    ScreenName = "Manage Package Service SetUp"
                });
                actionCollection.Add(new ClsFeatureAction
                {
                    CustomActionId = "Add Package",
                    CustomActionLabel = "Add Package",
                    ScreenName = "Manage Package Service SetUp"
                });
                actionCollection.Add(new ClsFeatureAction
                {
                    CustomActionId = "Edit Package",
                    CustomActionLabel = "Edit Package",
                    ScreenName = "Manage Package Service SetUp"
                });
                actionCollection.Add(new ClsFeatureAction
                {
                    CustomActionId = "Add Service Group",
                    CustomActionLabel = "Add Service Group",
                    ScreenName = "Manage Package Service SetUp"
                });
                actionCollection.Add(new ClsFeatureAction
                {
                    CustomActionId = "Delete Service Group",
                    CustomActionLabel = "Delete Service Group",
                    ScreenName = "Manage Package Service SetUp"
                });
                actionCollection.Add(new ClsFeatureAction
                {
                    CustomActionId = "Add Service",
                    CustomActionLabel = "Add Service",
                    ScreenName = "Manage Package Service SetUp"
                });
                actionCollection.Add(new ClsFeatureAction
                {
                    CustomActionId = "Edit Service",
                    CustomActionLabel = "Edit Service",
                    ScreenName = "Manage Package Service SetUp"
                });
                actionCollection.Add(new ClsFeatureAction
                {
                    CustomActionId = "Delete Service",
                    CustomActionLabel = "Delete Service",
                    ScreenName = "Manage Package Service SetUp"
                });
                actionCollection.Add(new ClsFeatureAction
                {
                    CustomActionId = "Add Attribute",
                    CustomActionLabel = "Add Attribute",
                    ScreenName = "Manage Package Service SetUp"
                });
                actionCollection.Add(new ClsFeatureAction
                {
                    CustomActionId = "Delete Attribute",
                    CustomActionLabel = "Delete Attribute",
                    ScreenName = "Manage Package Service SetUp"
                });
                actionCollection.Add(new ClsFeatureAction
                {
                    CustomActionId = "Edit Attribute",
                    CustomActionLabel = "Edit Attribute",
                    ScreenName = "Manage Package Service SetUp"
                });
                actionCollection.Add(new ClsFeatureAction
                {
                    CustomActionId = "EditInstructionText",
                    CustomActionLabel = "Edit Instruction Text",
                    ScreenName = "Manage Package Service SetUp"
                });
                return actionCollection;
            }
        }
        #endregion

    }
}