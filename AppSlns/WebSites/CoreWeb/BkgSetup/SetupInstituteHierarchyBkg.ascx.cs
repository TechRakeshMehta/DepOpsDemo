using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;

namespace CoreWeb.BkgSetup.Views
{
    public partial class SetupInstituteHierarchyBkg : BaseUserControl
	{	
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
                ApplyActionLevelPermission(ActionCollection, "SetupInstituteHierarchyBkg");
			}

            Response.Redirect("Pages/SetupInstitutionHierarchyBkg.aspx");   
		}

        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                Entity.ClientEntity.ClsFeatureAction objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();                

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                //objClsFeatureAction.SystemControl = fsucCmdBarNodePackage;
                objClsFeatureAction.CustomActionId = "AddPackage";
                objClsFeatureAction.CustomActionLabel = "AddPackage";
                objClsFeatureAction.ScreenName = "Institute Hierarchy NodePackageBkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                //objClsFeatureAction.SystemControl = fsucCmdBarNodePackage;
                objClsFeatureAction.CustomActionId = "AddNode";
                objClsFeatureAction.CustomActionLabel = "AddNode";
                objClsFeatureAction.ScreenName = "Institute Hierarchy NodePackageBkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "AddRegulatoryEntity";
                objClsFeatureAction.CustomActionLabel = "Add Regulatory Entity";
                objClsFeatureAction.ScreenName = "Institute Hierarchy NodePackageBkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "AddExtVendorAcct";
                objClsFeatureAction.CustomActionLabel = "AddExtVendorAcct";
                objClsFeatureAction.ScreenName = "Institute Hierarchy NodePackageBkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                //objClsFeatureAction.SystemControl = fsucCmdBarNodePackage;
                objClsFeatureAction.CustomActionId = "ManageContacts";
                objClsFeatureAction.CustomActionLabel = "ManageContacts";
                objClsFeatureAction.ScreenName = "Institute Hierarchy NodePackageBkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                //objClsFeatureAction.SystemControl = grdNode;
                objClsFeatureAction.CustomActionId = "Save";
                objClsFeatureAction.CustomActionLabel = "Save Payment";
                objClsFeatureAction.ScreenName = "Institute Hierarchy NodePackageBkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                //objClsFeatureAction.SystemControl = grdNode;
                objClsFeatureAction.CustomActionId = "DeleteNode";
                objClsFeatureAction.CustomActionLabel = "DeleteNode";
                objClsFeatureAction.ScreenName = "Institute Hierarchy NodePackageBkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                //objClsFeatureAction.SystemControl = grdPackage;
                objClsFeatureAction.CustomActionId = "DeletePackage";
                objClsFeatureAction.CustomActionLabel = "DeletePackage";
                objClsFeatureAction.ScreenName = "Institute Hierarchy NodePackageBkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "DeleteRegulatoryEntity";
                objClsFeatureAction.CustomActionLabel = "Delete Regulatory Entity";
                objClsFeatureAction.ScreenName = "Institute Hierarchy NodePackageBkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "DeleteExtVendorAcct";
                objClsFeatureAction.CustomActionLabel = "DeleteExtVendorAcct";
                objClsFeatureAction.ScreenName = "Institute Hierarchy NodePackageBkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "ReOrder";
                objClsFeatureAction.CustomActionLabel = "ReOrder Package Sequence";
                objClsFeatureAction.ScreenName = "Institute Hierarchy NodePackage";
                actionCollection.Add(objClsFeatureAction);


                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "SaveFeeItem";
                objClsFeatureAction.CustomActionLabel = "AddFeeItem";
                objClsFeatureAction.ScreenName = "Manage Fee Items";
                actionCollection.Add(objClsFeatureAction);


                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "DeleteFeeItem";
                objClsFeatureAction.CustomActionLabel = "DeleteFeeItem";
                objClsFeatureAction.ScreenName = "Manage Fee Items";
                actionCollection.Add(objClsFeatureAction);

                // SaveCustomForm  Manage Custom Forms  DeleteCustomForm

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "SaveCustomForm";
                objClsFeatureAction.CustomActionLabel = "SaveCustomForm";
                objClsFeatureAction.ScreenName = "Manage Custom Forms";
                actionCollection.Add(objClsFeatureAction);


                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "DeleteCustomForm";
                objClsFeatureAction.CustomActionLabel = "DeleteCustomForm";
                objClsFeatureAction.ScreenName = "Manage Custom Forms";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "EditFeeItem";
                objClsFeatureAction.CustomActionLabel = "EditFeeItem";
                objClsFeatureAction.ScreenName = "Manage Fee Items";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "SaveFeeRecord";
                objClsFeatureAction.CustomActionLabel = "SaveFeeRecord";
                objClsFeatureAction.ScreenName = "Manage Fee Record";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "DeleteFeeRecord";
                objClsFeatureAction.CustomActionLabel = "DeleteFeeRecord";
                objClsFeatureAction.ScreenName = "Manage Fee Record";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "AddNewRuleSet";
                objClsFeatureAction.CustomActionLabel = "Add New Rule Set";
                objClsFeatureAction.ScreenName = "Rule set List Bkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "DeleteRuleSet";
                objClsFeatureAction.CustomActionLabel = "Delete Rule Set";
                objClsFeatureAction.ScreenName = "Rule set List Bkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "EditRuleSet";
                objClsFeatureAction.CustomActionLabel = "Edit Rule Set";
                objClsFeatureAction.ScreenName = "Rule set Info Bkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "AddNewRuleList";
                objClsFeatureAction.CustomActionLabel = "Add New Rule List";
                objClsFeatureAction.ScreenName = "Rule List Bkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "DeleteRule";
                objClsFeatureAction.CustomActionLabel = "Delete Rule";
                objClsFeatureAction.ScreenName = "Rule List Bkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "AddRule";
                objClsFeatureAction.CustomActionLabel = "Add Rule";
                objClsFeatureAction.ScreenName = "Rule List Bkg";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "AddNewAttributeValue";
                objClsFeatureAction.CustomActionLabel = "Add New Attribute Value";
                objClsFeatureAction.ScreenName = "Manage Srvc Item Entity Record";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "EditAttributeValue";
                objClsFeatureAction.CustomActionLabel = "Edit Attribute Value";
                objClsFeatureAction.ScreenName = "Manage Srvc Item Entity Record";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "DeleteAttributeValue";
                objClsFeatureAction.CustomActionLabel = "Delete Attribute Value";
                objClsFeatureAction.ScreenName = "Manage Srvc Item Entity Record";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "EditRule";
                objClsFeatureAction.CustomActionLabel = "Edit Rule";
                objClsFeatureAction.ScreenName = "Rule List Bkg";
                actionCollection.Add(objClsFeatureAction);

                return actionCollection;
               
                
               
            }
        }
        public override List<String> ChildScreenPathCollection
        {
            get
            {
                List<String> childScreenPathCollection = new List<String>();
                childScreenPathCollection.Add(@"~/ComplianceAdministration/UserControl/NodeNotificationSettings.ascx");
                childScreenPathCollection.Add(@"~/BkgSetup/UserControl/EditServiceItemDetail.ascx");
                childScreenPathCollection.Add(@"~/BkgSetup/UserControl/ManageServiceItem.ascx");
                childScreenPathCollection.Add(@"~/BkgSetup/UserControl/ManageBkgPackageDetails.ascx");
                return childScreenPathCollection;
            }
        }
		// TODO: Forward events to the presenter and show state to the user.
		// For examples of this, see the View-Presenter (with Application Controller) QuickStart:
		//	
        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            //ApplyPermisions();

        }
	}
}

