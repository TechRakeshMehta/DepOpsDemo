using Entity.ClientEntity;
using INTSOF.UI.Contract.QueueManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.QueueManagement.Views
{
    public interface IReconcilliationQueueAssignmentControlView
    {
        Int32 CurrentAssignmentConfigurationId
        {
            get;
            set;
        }

        //List<Int32> MappedTenantIds
        //{
        //    get;
        //    set;
        //}

        List<Tenant> lstTenant
        {
            get;
            set;
        }

        String AssignmentDescription
        {
            get;
            set;
        }

        Int32 NumberOfReviews
        {
            get;
            set;
        }

        Decimal Percentage
        {
            get;
            set;
        }
        
        Int32 CurrentLoggedInUserId
        {
            get;
        }
        Int32 SelectedTenantID
        {
            get;
            set;
        }

        List<GetQueueAssigneeList> lstReviewerUsers
        {
            get;
            set;
        }

        List<ManageRandomReviewsContract> lstManageRandomReviews { get; set; }

        String InstitutionHierarchyID { get; set; }
    }
}
