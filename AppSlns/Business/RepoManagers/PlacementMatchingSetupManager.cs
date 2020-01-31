using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.RepoManagers
{
    public class PlacementMatchingSetupManager
    {
        #region Methods
        /// <summary>
        /// Update Placement Department
        /// </summary>
        /// <param name="departmentContract"></param>

        public static bool UpdatePlacementDepartment(DepartmentContract departmentContract, Int32 userID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).UpdatePlacementDepartment(departmentContract, userID);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Update Placement Student Type
        /// </summary>
        /// <param name="studentTypeContract"></param>
        /// <returns></returns>
        public static bool UpdatePlacementStudentType(StudentTypeContract studentTypeContract, Int32 userID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).UpdatePlacementStudentType(studentTypeContract, userID);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// UpdatePlacement Specialty
        /// </summary>
        /// <param name="specialtyContract"></param>
        /// <returns></returns>
        public static bool UpdatePlacementSpecialty(SpecialtyContract specialtyContract, Int32 userID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).UpdatePlacementSpecialty(specialtyContract, userID);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Get list Of Placement Departments
        /// </summary>
        /// <returns></returns>
        public static List<DepartmentContract> GetPlacementDepartments()
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetPlacementDepartments();

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Get list of Placement StudentTypes
        /// </summary>
        /// <returns></returns>
        public static List<StudentTypeContract> GetPlacementStudentTypes()
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetPlacementStudentTypes();

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Get list of Placement Specialties
        /// </summary>
        /// <returns></returns>
        public static List<SpecialtyContract> GetPlacementSpecialties()
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetPlacementSpecialties();

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Insert Placement Department
        /// </summary>
        /// <param name="departmentContract"></param>
        /// <returns></returns>
        public static bool InsertPlacementDepartment(DepartmentContract departmentContract, Int32 userID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).InsertPlacementDepartment(departmentContract, userID);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Insert Placement StudentType
        /// </summary>
        /// <param name="studentTypeContract"></param>
        /// <returns></returns>
        public static bool InsertPlacementStudentType(StudentTypeContract studentTypeContract, Int32 userID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).InsertPlacementStudentType(studentTypeContract, userID);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Insert Placement Specialty
        /// </summary>
        /// <param name="specialtyContract"></param>
        /// <returns></returns>
        public static bool InsertPlacementSpecialty(SpecialtyContract specialtyContract, Int32 userID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).InsertPlacementSpecialty(specialtyContract, userID);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Delete Placement Department
        /// </summary>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        public static bool DeletePlacementDepartment(Int32 departmentID, Int32 userID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).DeletePlacementDepartment(departmentID, userID);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Delete Placement StudentType
        /// </summary>
        /// <param name="studentTypeÍD"></param>
        /// <returns></returns>
        public static bool DeletePlacementStudentType(Int32 studentTypeÍD, Int32 userID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).DeletePlacementStudentType(studentTypeÍD, userID);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Delete Placement Specialty
        /// </summary>
        /// <param name="specialtyID"></param>
        /// <returns></returns>
        public static bool DeletePlacementSpecialty(Int32 specialtyID, Int32 userID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).DeletePlacementSpecialty(specialtyID, userID);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Dictionary<Int32, String> GetAgencyRootNode(Guid UserId)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetAgencyRootNode(UserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<AgencyLocationDepartmentContract> GetAgencyLocations(Int32 agencyRootNodeID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetAgencyLocations(agencyRootNodeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<AgencyLocationDepartmentContract> GetAllLocations()
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetAllLocations();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveAgencyLocation(AgencyLocationDepartmentContract agencyLocation, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).SaveAgencyLocation(agencyLocation, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteAgencyLocation(Int32 agencyLocationID, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).DeleteAgencyLocation(agencyLocationID, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Department> GetDepartments()
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetDepartments();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<StudentType> GetStudentTypes()
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetStudentTypes();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<AgencyLocationDepartmentContract> GetAgencyLocationDepartment(Int32 agencyLocationId)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetAgencyLocationDepartment(agencyLocationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveAgencyLocationDepartment(AgencyLocationDepartmentContract locationDepartment, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).SaveAgencyLocationDepartment(locationDepartment, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteAgencyLocationDepartment(Int32 agencyLocationDepartmentId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).DeleteAgencyLocationDepartment(agencyLocationDepartmentId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsDeptMappedWithLocation(Int32 agencyLocationId)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).IsDeptMappedWithLocation(agencyLocationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<lkpInventoryAvailabilityType> GetInstitutionAvailability()
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetInstitutionAvailability();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<PlacementMatchingContract> GetOpportunities(PlacementMatchingContract searchContract)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetOpportunities(searchContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static PlacementMatchingContract GetOpportunityDetailByID(Int32 opportunityID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetOpportunityDetailByID(opportunityID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean PublishOpportunity(Int32 currentLoggedInUserID, PlacementMatchingContract placementMatchingContract, List<CustomAttribteContract> customAttributeList)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).PublishOpportunity(currentLoggedInUserID, placementMatchingContract, customAttributeList);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveOpportunity(Int32 currentLoggedInUserID, PlacementMatchingContract placementMatchingContract, List<CustomAttribteContract> customAttributeList)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).SaveOpportunity(currentLoggedInUserID, placementMatchingContract, customAttributeList);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteOpportunity(Int32 currentLoggedInUserID, List<Int32> lstSelectedOpportunityId)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).DeleteOpportunity(currentLoggedInUserID, lstSelectedOpportunityId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean ArchiveOpportunities(Int32 currentLoggedInUserID, List<Int32> lstSelectedOpportunityId)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).ArchiveOpportunities(currentLoggedInUserID, lstSelectedOpportunityId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UnArchiveOpportunities(Int32 currentLoggedInUserID, List<Int32> lstSelectedOpportunityId)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).UnArchiveOpportunities(currentLoggedInUserID, lstSelectedOpportunityId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequestDetailContract> GetRequests(RequestDetailContract searchRequestContract)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetRequests(searchRequestContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static List<DepartmentContract> GetLocationDepartments(Int32 selectedAgencyLocationID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetLocationDepartments(selectedAgencyLocationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<StudentTypeContract> GetAgencyDepartmentStudentTypes(Int32 selectedDepartmentID, Int32 selectedAgencyLocationID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetAgencyDepartmentStudentTypes(selectedDepartmentID, selectedAgencyLocationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion


        public static List<PlacementMatchingContract> GetOpportunitySearch(PlacementSearchContract searchContract)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetOpportunitySearch(searchContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequestDetailContract> GetPlacementRequests(PlacementSearchContract searchContract)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetPlacementRequests(searchContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveRequest(RequestDetailContract requestDetail, Int32 currentLoggedInUserId, List<CustomAttribteContract> lstCustomAttribute)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).SaveRequestDetails(requestDetail, currentLoggedInUserId, lstCustomAttribute);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }

            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static RequestDetailContract GetRequestDetail(Int32 requestID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetRequestDetail(requestID);


            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<ShiftDetails> GetShiftsForOpportunity(Int32 opportunityId)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetShiftsForOpportunity(opportunityId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<ShiftDetails> GetAllShifts()
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetAllShifts();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean ChangeRequestStatus(Int32 currentLoggedInUser, Int32 requestID, string statusCode)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).ChangeRequestStatus(currentLoggedInUser, requestID, statusCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        //public static Boolean SaveShiftDetails(Int32 currentLoggedInUser, ShiftDetails shiftDetail)
        //{
        //    try
        //    {
        //        return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).SaveShiftDetails(currentLoggedInUser, shiftDetail);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        //public static Boolean DeleteShiftDetail(Int32 currentLoggedInUser, ShiftDetails shiftDetail)
        //{
        //    try
        //    {
        //        return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).DeleteShiftDetail(currentLoggedInUser, shiftDetail);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        public static List<PlacementRequestAuditContract> GetPlacementRequestAuditLogs(Int32 requestID)
        {
            try
            {

                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetPlacementRequestAuditLogs(requestID);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        public static List<RequestStatusContract> GetRequestStatuses()
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetRequestStatuses();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequestDetailContract> GetAgencyPlacementDashboardRequests(Int32 agencyHierarchyRootNodeID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetAgencyPlacementDashboardRequests(agencyHierarchyRootNodeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<AgencyHierarchyContract> GetAgencyHierarchyRootNodes(Int32 TenantID)
        {
            try
            {

                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetAgencyHierarchyRootNodes(TenantID);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<RequestStatusContract> GetRequestStatusBarCount(Int32 AgencyID)
        {
            try
            {

                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetRequestStatusBarCount(AgencyID);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<InstitutionRequestPieChartContract> GetIntitutionsRequestsApproved(Int32 agencyHierarchyRootNodeId, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetIntitutionsRequestsApproved(agencyHierarchyRootNodeId, fromDate, toDate);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<SharedCustomAttributesContract> GetSharedCustomAttributes()
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetSharedCustomAttributes();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveSharedCustomAttribute(Int32 currentLoggedInUserID, SharedCustomAttributesContract sharedCustomAttributes)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).SaveSharedCustomAttribute(currentLoggedInUserID, sharedCustomAttributes);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteSharedCustomAttribute(Int32 currentLoggedInUserId, Int32 selectSharedCustomAttributeId, Int32 selectSharedCustomAttributeMappingID)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).DeleteSharedCustomAttribute(currentLoggedInUserId, selectSharedCustomAttributeId, selectSharedCustomAttributeMappingID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<lkpCustomAttributeDataType> GetSharedAttributeDataTypes()
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetSharedAttributeDataTypes();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<lkpSharedCustomAttributeUseType> GetSharedAttributeUseTypes()
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetSharedAttributeUseTypes();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<CustomAttribteContract> GetSharedCustomAttributeList(Int32 agencyRootNodeID, String useTypeCode, Int32? recordId, String recordTypeCode)
        {
            try
            {
                return BALUtils.GetPlacementMatchingSetupRepoInstance(AppConsts.NONE).GetSharedCustomAttributeList(agencyRootNodeID, useTypeCode, recordId, recordTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
    }
}

