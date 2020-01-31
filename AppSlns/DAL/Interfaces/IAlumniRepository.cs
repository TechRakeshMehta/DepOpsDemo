using Entity.SharedDataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace DAL.Interfaces
{
    public interface IAlumniRepository
    {
        //  void DeletingAgencyHierarchyMappings(Int32 agencyHierarchyId, Int32 currentLoggedInUserID);
        Tuple<Int32, Int32> CreateAlumniDefaultSubscription(Int32 currentLoggedInUserID, Int32 organizationUserProfileID, String machineIP);
        Boolean CheckAllSubscriptionsForApplicant(Int32 orgUserId);
    }
}
