using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IPlacementMatchingSetupRepository
    {
        bool UpdatePlacementDepartment(DepartmentContract departmentContract, Int32 currentLoggedInUserId);
        bool UpdatePlacementStudentType(StudentTypeContract studentTypeContract, Int32 currentLoggedInUserId);
        bool UpdatePlacementSpecialty(SpecialtyContract specialtyContract, Int32 currentLoggedInUserId);

        bool InsertPlacementDepartment(DepartmentContract departmentContract, Int32 currentLoggedInUserId);
        bool InsertPlacementStudentType(StudentTypeContract studentTypeContract, Int32 currentLoggedInUserId);
        bool InsertPlacementSpecialty(SpecialtyContract specialtyContract, Int32 currentLoggedInUserId);

        bool DeletePlacementDepartment(Int32 departmentID, Int32 currentLoggedInUserId);
        bool DeletePlacementStudentType(Int32 studentTypeID, Int32 currentLoggedInUserId);
        bool DeletePlacementSpecialty(Int32 specialtyID, Int32 currentLoggedInUserId);

        List<DepartmentContract> GetPlacementDepartments();
        List<StudentTypeContract> GetPlacementStudentTypes();
        List<SpecialtyContract> GetPlacementSpecialties();
        Dictionary<Int32, String> GetAgencyRootNode(Guid UserId);
        List<AgencyLocationDepartmentContract> GetAgencyLocations(Int32 agencyRootNodeID);
        Boolean SaveAgencyLocation(AgencyLocationDepartmentContract agencyLocation, Int32 currentLoggedInUserId);
        Boolean DeleteAgencyLocation(Int32 agencyLocationID, Int32 currentLoggedInUserId);
        List<Department> GetDepartments();
        List<StudentType> GetStudentTypes();
        List<AgencyLocationDepartmentContract> GetAgencyLocationDepartment(Int32 agencyLocationId);
        Boolean SaveAgencyLocationDepartment(AgencyLocationDepartmentContract locationDepartment, Int32 currentLoggedInUserId);
        Boolean DeleteAgencyLocationDepartment(Int32 agencyLocationDepartmentId, Int32 currentLoggedInUserId);
        Boolean IsDeptMappedWithLocation(Int32 agencyLocationId);
        List<lkpInventoryAvailabilityType> GetInstitutionAvailability();
        List<PlacementMatchingContract> GetOpportunities(PlacementMatchingContract searchContract);
        List<AgencyLocationDepartmentContract> GetAllLocations();
        PlacementMatchingContract GetOpportunityDetailByID(Int32 opportunityID);
        Boolean PublishOpportunity(Int32 currentLoggedInUserID, PlacementMatchingContract placementMatchingContract, List<CustomAttribteContract> customAttributeList);
        Boolean SaveOpportunity(Int32 currentLoggedInUserID, PlacementMatchingContract placementMatchingContract, List<CustomAttribteContract> customAttributeList);
        Boolean DeleteOpportunity(Int32 currentLoggedInUserID, List<Int32> lstSelectedOpportunityId);
        Boolean ArchiveOpportunities(Int32 currentLoggedInUserID, List<Int32> lstSelectedOpportunityId);
        Boolean UnArchiveOpportunities(Int32 currentLoggedInUserID, List<Int32> lstSelectedOpportunityId);
        List<RequestDetailContract> GetRequests(RequestDetailContract searchRequestContract);
        List<DepartmentContract> GetLocationDepartments(Int32 selectedAgencyLocationID);
        List<StudentTypeContract> GetAgencyDepartmentStudentTypes(Int32 selectedDepartmentID, Int32 selectedAgencyLocationID);

        List<PlacementMatchingContract> GetOpportunitySearch(PlacementSearchContract searchContract);
        List<RequestDetailContract> GetPlacementRequests(PlacementSearchContract searchContract);
        List<ShiftDetails> GetShiftsForOpportunity(Int32 opportunityId);
        Boolean SaveRequestDetails(RequestDetailContract requestDetail, Int32 currentLoggedInUserId, List<CustomAttribteContract> lstCustomAttribute);
        RequestDetailContract GetRequestDetail(Int32 requestID);
        List<ShiftDetails> GetAllShifts();
        Boolean ChangeRequestStatus(Int32 currentLoggedInUser, Int32 requestID, string statusCode);
        Boolean SaveShiftDetails(Int32 currentLoggedInUser, ShiftDetails shiftDetail);
        Boolean DeleteShiftDetail(Int32 currentLoggedInUser, ShiftDetails shiftDetail);
        List<PlacementRequestAuditContract> GetPlacementRequestAuditLogs(Int32 requestID);
        List<AgencyHierarchyContract> GetAgencyHierarchyRootNodes(Int32 TenantId);
        List<RequestStatusContract> GetRequestStatuses();
        List<RequestStatusContract> GetRequestStatusBarCount(Int32 AgencyID);
        List<RequestDetailContract> GetAgencyPlacementDashboardRequests(Int32 agencyHierarchyRootNodeID);
        List<InstitutionRequestPieChartContract> GetIntitutionsRequestsApproved(Int32 agencyHierarchyRootNodeId, DateTime? fromDate, DateTime? toDate);
        List<SharedCustomAttributesContract> GetSharedCustomAttributes();
        Boolean SaveSharedCustomAttribute(Int32 currentLoggedInUserID, SharedCustomAttributesContract sharedCustomAttributes);
        Boolean DeleteSharedCustomAttribute(Int32 currentLoggedInUserId, Int32 selectSharedCustomAttributeId, Int32 selectSharedCustomAttributeMappingID);
        List<lkpCustomAttributeDataType> GetSharedAttributeDataTypes();
        List<lkpSharedCustomAttributeUseType> GetSharedAttributeUseTypes();
        List<CustomAttribteContract> GetSharedCustomAttributeList(Int32 agencyRootNodeID, String useTypeCode, Int32? recordId, String recordTypeCode);
    }
}
