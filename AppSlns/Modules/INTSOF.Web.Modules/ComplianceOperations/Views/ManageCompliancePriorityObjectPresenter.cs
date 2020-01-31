using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageCompliancePriorityObjectPresenter : Presenter<IManageCompliancePriorityObjectView>
    {
        //Method to get Compliance Priority Objects
        public void GetCompliancePriorityObjects()
        {
            View.lstCompPriorityObject = new List<CompliancePriorityObjectContract>();
            View.lstCompPriorityObject = SecurityManager.GetCompliancePriorityObjects();
        }

        //Method to save/update compliance priority object.
        public Boolean SaveComplPriorityObject(CompliancePriorityObjectContract compPriorityObject)
        {
            return SecurityManager.SaveComplPriorityObject(View.CurrentLoggedInUserID, compPriorityObject);
        }

        //Method to delete compliance priority object
        public Boolean DeleteComplPriorityObject(Int32 compPriorityObjectID)
        {
            return SecurityManager.DeleteComplPriorityObject(View.CurrentLoggedInUserID, compPriorityObjectID);
        }
    }
}
