using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.BkgSetup.Views
{
    public interface IReviewCriteriaHierarchyMappingView
    {

        #region Properties

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 DefaultTenantId
        {
            get;
            set;
        }

        Int32 DeptProgramMappingID
        {
            get;
            set;
        }


        List<BkgReviewCriteria> ReviewCriteriaList
        {
            get;
            set;
        }

        List<Int32> ReviewCriteriaIDList
        {
            get;
            set;
        }

        List<BkgReviewCriteriaHierarchyMapping> MappedReviewCriteriaList
        {
            get;
            set;
        }

        List<Int32> MappedReviewCriteriaIds
        {
            get;
            set;
        }
        #endregion

    }
}
