using System;
using System.Collections.Generic;
using INTSOF.Utils;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
namespace CoreWeb.ComplianceAdministration.Views
{
    // public partial class SetupInstituteHierarchy : System.Web.UI.UserControl, ISetupInstituteHierarchyView
    public partial class SetupInstituteHierarchy : BaseUserControl, ISetupInstituteHierarchyView
    {
        private SetupInstituteHierarchyPresenter _presenter = new SetupInstituteHierarchyPresenter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }

            

            Presenter.OnViewLoaded();
            Response.Redirect("Pages/SetupDepartmentProgram.aspx");
        }


        public SetupInstituteHierarchyPresenter Presenter
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


      


        public override List<ClsFeatureAction> ActionCollection
        {
            get
            {
                List<ClsFeatureAction> actionCollection = new List<ClsFeatureAction>();
                ClsFeatureAction objClsFeatureAction = new ClsFeatureAction();
                objClsFeatureAction.ScreenName = "Institute Hierarchy NodePackage";
                objClsFeatureAction.CustomActionId = "Save";
                objClsFeatureAction.CustomActionLabel = "Add Node";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new ClsFeatureAction();
                objClsFeatureAction.ScreenName = "Institute Hierarchy NodePackage";
                objClsFeatureAction.CustomActionId = "Submit";
                objClsFeatureAction.CustomActionLabel = "Add Package";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new ClsFeatureAction();
                objClsFeatureAction.ScreenName = "Institute Hierarchy NodePackage";
                objClsFeatureAction.CustomActionId = "Clear";
                objClsFeatureAction.CustomActionLabel = "Manage Node Notifications";
                actionCollection.Add(objClsFeatureAction);

                
                return actionCollection;
            }
        }


    }
}

