using DAL.Interfaces;
using Entity.ClientEntity;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Repository
{
    public class PlacementMatchingSetupRepository : ClientBaseRepository, IPlacementMatchingSetupRepository
    {
        #region Variables
        private ADB_LibertyUniversity_ReviewEntities _dbContext;
        #endregion

        #region Default Constructor to initilize DB Context
        ///// <summary>
        ///// Default constructor to initialize the context
        ///// </summary>
        public PlacementMatchingSetupRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }


        #endregion

        #region Methods

        bool IPlacementMatchingSetupRepository.UpdatePlacementDepartment(DepartmentContract departmentContract, Int32 userID)
        {
            try
            {


                Department department = SharedDataDBContext.Departments.Where(dept => dept.DP_ID == departmentContract.DepartmentID).FirstOrDefault();
                if (!department.IsNullOrEmpty())
                {
                    department.DP_Name = departmentContract.Name;
                    department.DP_Description = departmentContract.Description;
                    department.DP_ModifiedOn = DateTime.Now;
                    department.DP_ModifiedByID = userID;
                    if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        bool IPlacementMatchingSetupRepository.UpdatePlacementSpecialty(SpecialtyContract specialtyContract, Int32 userID)
        {
            try
            {
                Specialty specialty = SharedDataDBContext.Specialties.Where(spety => spety.SP_ID == specialtyContract.SpecialtyID).FirstOrDefault();
                if (!specialty.IsNullOrEmpty())
                {
                    specialty.SP_Name = specialtyContract.Name;
                    specialty.SP_Description = specialtyContract.Description;
                    specialty.SP_ModifiedOn = DateTime.Now;
                    specialty.SP_ModifiedByID = userID;
                    if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        bool IPlacementMatchingSetupRepository.UpdatePlacementStudentType(StudentTypeContract studentTypeContract, Int32 userID)
        {
            try
            {

                StudentType studentType = SharedDataDBContext.StudentTypes.Where(stype => stype.ST_ID == studentTypeContract.StudentTypeId).FirstOrDefault();

                if (!studentType.IsNullOrEmpty())
                {
                    studentType.ST_Name = studentTypeContract.Name;
                    studentType.ST_Description = studentTypeContract.Description;
                    studentType.ST_ModifiedByID = userID;
                    studentType.ST_ModifiedOn = DateTime.Now;
                    if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        bool IPlacementMatchingSetupRepository.InsertPlacementDepartment(DepartmentContract departmentContract, Int32 userID)
        {
            try
            {
                Department department = new Department();
                department.DP_Name = departmentContract.Name;
                department.DP_Description = departmentContract.Description;
                department.DP_CreatedByID = userID;
                department.DP_CreatedOn = DateTime.Now;
                SharedDataDBContext.Departments.AddObject(department);
                if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
                else
                    return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        bool IPlacementMatchingSetupRepository.InsertPlacementSpecialty(SpecialtyContract specialtyContract, Int32 userID)
        {
            try
            {
                Specialty specialty = new Specialty();
                specialty.SP_Name = specialtyContract.Name;
                specialty.SP_Description = specialtyContract.Description;
                specialty.SP_CreatedByID = userID;
                specialty.SP_CreatedOn = DateTime.Now;
                SharedDataDBContext.Specialties.AddObject(specialty);
                if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
                else
                    return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        bool IPlacementMatchingSetupRepository.InsertPlacementStudentType(StudentTypeContract studentTypeContract, Int32 userID)
        {
            try
            {
                StudentType studentType = new StudentType();
                studentType.ST_Name = studentTypeContract.Name;
                studentType.ST_Description = studentTypeContract.Description;
                studentType.ST_CreatedByID = userID;
                studentType.ST_CreatedOn = DateTime.Now;
                SharedDataDBContext.StudentTypes.AddObject(studentType);
                if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
                else
                    return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        bool IPlacementMatchingSetupRepository.DeletePlacementDepartment(Int32 departmentID, Int32 userID)
        {
            try
            {
                Department department = SharedDataDBContext.Departments.Where(dept => dept.DP_ID == departmentID).FirstOrDefault();
                if (!department.IsNullOrEmpty())
                {
                    department.DP_IsDeleted = true;
                    department.DP_ModifiedByID = userID;
                    department.DP_ModifiedOn = DateTime.Now;
                    if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        bool IPlacementMatchingSetupRepository.DeletePlacementSpecialty(Int32 specialtyID, Int32 userID)
        {
            try
            {
                Specialty specialty = SharedDataDBContext.Specialties.Where(spety => spety.SP_ID == specialtyID).FirstOrDefault();

                if (!specialty.IsNullOrEmpty())
                {
                    specialty.SP_IsDeleted = true;
                    specialty.SP_ModifiedByID = userID;
                    specialty.SP_ModifiedOn = DateTime.Now;
                    if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        bool IPlacementMatchingSetupRepository.DeletePlacementStudentType(Int32 studentTypeID, Int32 userID)
        {
            try
            {
                StudentType studentType = SharedDataDBContext.StudentTypes.Where(stype => stype.ST_ID == studentTypeID).FirstOrDefault();
                if (!studentType.IsNullOrEmpty())
                {
                    studentType.ST_IsDeleted = true;
                    studentType.ST_ModifiedByID = userID;
                    studentType.ST_ModifiedOn = DateTime.Now;
                    if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        List<DepartmentContract> IPlacementMatchingSetupRepository.GetPlacementDepartments()
        {
            try
            {
                List<DepartmentContract> lstdepartmentContract = new List<DepartmentContract>();
                List<Department> departments = SharedDataDBContext.Departments.Where(dept => !dept.DP_IsDeleted).ToList();
                if (!departments.IsNullOrEmpty() && departments.Count > AppConsts.NONE)
                {
                    foreach (Department department in departments)
                    {
                        DepartmentContract departmentContract = new DepartmentContract();
                        departmentContract.DepartmentID = department.DP_ID;
                        departmentContract.Name = department.DP_Name;
                        departmentContract.Description = department.DP_Description;
                        departmentContract.IsMappingExists = department.AgencyLocationDepartments.Where(cond => !cond.ALD_IsDeleted).Any() ? true : false;
                        lstdepartmentContract.Add(departmentContract);
                    }
                }
                return lstdepartmentContract;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        List<SpecialtyContract> IPlacementMatchingSetupRepository.GetPlacementSpecialties()
        {
            try
            {
                List<SpecialtyContract> lstspecialtyContract = new List<SpecialtyContract>();
                List<Specialty> specialties = SharedDataDBContext.Specialties.Where(spec => !spec.SP_IsDeleted).ToList();
                if (!specialties.IsNullOrEmpty() && specialties.Count > AppConsts.NONE)
                {
                    foreach (Specialty specialty in specialties)
                    {
                        SpecialtyContract specialtyContract = new SpecialtyContract();
                        specialtyContract.SpecialtyID = specialty.SP_ID;
                        specialtyContract.Name = specialty.SP_Name;
                        specialtyContract.Description = specialty.SP_Description;
                        lstspecialtyContract.Add(specialtyContract);
                    }
                }
                return lstspecialtyContract;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        List<StudentTypeContract> IPlacementMatchingSetupRepository.GetPlacementStudentTypes()
        {
            try
            {
                List<StudentTypeContract> lststudentTypeContract = new List<StudentTypeContract>();
                List<StudentType> studentTypes = SharedDataDBContext.StudentTypes.Where(stype => !stype.ST_IsDeleted).ToList();
                if (!studentTypes.IsNullOrEmpty() && studentTypes.Count > AppConsts.NONE)
                {
                    foreach (StudentType studenttype in studentTypes)
                    {
                        StudentTypeContract studentTypeContract = new StudentTypeContract();
                        studentTypeContract.StudentTypeId = studenttype.ST_ID;
                        studentTypeContract.Name = studenttype.ST_Name;
                        studentTypeContract.Description = studenttype.ST_Description;
                        studentTypeContract.IsMappingExists = studenttype.AgencyLocationDepartmentStudentTypes.Where(cond => !cond.LSDT_IsDeleted).Any() ? true : false;
                        lststudentTypeContract.Add(studentTypeContract);
                    }
                }
                return lststudentTypeContract;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        Dictionary<Int32, String> IPlacementMatchingSetupRepository.GetAgencyRootNode(Guid UserId)
        {
            Dictionary<Int32, String> dicAgencyRootNode = new Dictionary<Int32, String>();
            Int32 AgencyRootNodeId = AppConsts.NONE;
            String AgencyRootNode = String.Empty;
            EntityConnection connection = SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAgencyUserRootNode", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", UserId);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    AgencyRootNodeId = dr["AH_ID"] != DBNull.Value ? Convert.ToInt32(dr["AH_ID"]) : AppConsts.NONE;
                    AgencyRootNode = dr["AH_Label"] != DBNull.Value ? Convert.ToString(dr["AH_Label"]) : String.Empty;
                }
                con.Close();
            }
            dicAgencyRootNode.Add(AgencyRootNodeId, AgencyRootNode);
            return dicAgencyRootNode;
        }

        List<AgencyLocationDepartmentContract> IPlacementMatchingSetupRepository.GetAgencyLocations(Int32 agencyRootNodeID)
        {
            List<AgencyLocationDepartmentContract> lstAgencyLocationDepartmentContract = new List<AgencyLocationDepartmentContract>();

            lstAgencyLocationDepartmentContract = SharedDataDBContext.AgencyLocations.Where(cond => !cond.AL_IsDeleted && cond.AL_AgencyHierarchyID == agencyRootNodeID).Select
                (x => new AgencyLocationDepartmentContract
                {
                    AgencyLocationID = x.AL_ID,
                    AgencyHierarchyID = x.AL_AgencyHierarchyID,
                    Location = x.AL_Location,
                    Experience = x.AL_Experience
                }).ToList();

            return lstAgencyLocationDepartmentContract;
        }

        List<AgencyLocationDepartmentContract> IPlacementMatchingSetupRepository.GetAllLocations()
        {
            List<AgencyLocationDepartmentContract> lstAgencyLocationDepartmentContract = new List<AgencyLocationDepartmentContract>();

            lstAgencyLocationDepartmentContract = SharedDataDBContext.AgencyLocations.Where(cond => !cond.AL_IsDeleted).Select
                (x => new AgencyLocationDepartmentContract
                {
                    AgencyLocationID = x.AL_ID,
                    AgencyHierarchyID = x.AL_AgencyHierarchyID,
                    Location = x.AL_Location,
                    Experience = x.AL_Experience
                }).ToList();

            return lstAgencyLocationDepartmentContract;
        }

        Boolean IPlacementMatchingSetupRepository.SaveAgencyLocation(AgencyLocationDepartmentContract agencyLocationContract, Int32 currentLoggedInUserId)
        {
            AgencyLocation agencyLocation = new AgencyLocation();
            // update
            if (!agencyLocationContract.AgencyLocationID.IsNullOrEmpty() && agencyLocationContract.AgencyLocationID > AppConsts.NONE)
            {
                agencyLocation = SharedDataDBContext.AgencyLocations.Where(cond => !cond.AL_IsDeleted && cond.AL_ID == agencyLocationContract.AgencyLocationID).FirstOrDefault();
                agencyLocation.AL_AgencyHierarchyID = agencyLocationContract.AgencyHierarchyID;
                agencyLocation.AL_Location = agencyLocationContract.Location;
                agencyLocation.AL_Experience = agencyLocationContract.Experience;
                agencyLocation.AL_ModifiedBy = currentLoggedInUserId;
                agencyLocation.AL_ModifiedOn = DateTime.Now;
            }
            //Save
            else
            {
                agencyLocation.AL_AgencyHierarchyID = agencyLocationContract.AgencyHierarchyID;
                agencyLocation.AL_Location = agencyLocationContract.Location;
                agencyLocation.AL_Experience = agencyLocationContract.Experience;
                agencyLocation.AL_CreatedBy = currentLoggedInUserId;
                agencyLocation.AL_CreatedOn = DateTime.Now;
                SharedDataDBContext.AgencyLocations.AddObject(agencyLocation);
            }
            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IPlacementMatchingSetupRepository.DeleteAgencyLocation(Int32 agencyLocationID, Int32 currentLoggedInUserId)
        {
            AgencyLocation agencyLocation = SharedDataDBContext.AgencyLocations.Where(cond => !cond.AL_IsDeleted && cond.AL_ID == agencyLocationID).FirstOrDefault();
            if (!agencyLocation.IsNullOrEmpty())
            {
                agencyLocation.AL_IsDeleted = true;
                agencyLocation.AL_ModifiedBy = currentLoggedInUserId;
                agencyLocation.AL_ModifiedOn = DateTime.Now;
            }
            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        List<Department> IPlacementMatchingSetupRepository.GetDepartments()
        {
            return SharedDataDBContext.Departments.Where(cond => !cond.DP_IsDeleted).ToList();
        }

        List<StudentType> IPlacementMatchingSetupRepository.GetStudentTypes()
        {
            return SharedDataDBContext.StudentTypes.Where(cond => !cond.ST_IsDeleted).ToList();
        }

        List<AgencyLocationDepartmentContract> IPlacementMatchingSetupRepository.GetAgencyLocationDepartment(Int32 agencyLocationId)
        {
            List<AgencyLocationDepartmentContract> lstAgencyLocationDepartmentContract = new List<AgencyLocationDepartmentContract>();
            lstAgencyLocationDepartmentContract = SharedDataDBContext.AgencyLocationDepartments.Where(cond => !cond.ALD_IsDeleted
                    && cond.ALD_AgencyLocationID == agencyLocationId).Select(x => new AgencyLocationDepartmentContract
                    {
                        AgencyLocationDepartmentID = x.ALD_ID,
                        AgencyLocationID = x.ALD_AgencyLocationID,
                        DepartmentID = x.ALD_DepartmentID,
                        Department = x.Department.DP_Name,
                        lstStudentTypes = x.AgencyLocationDepartmentStudentTypes.Where(cond => !cond.LSDT_IsDeleted).Select(Sel => Sel.StudentType.ST_Name).ToList(),
                        lstStudentTypeID = x.AgencyLocationDepartmentStudentTypes.Where(con => !con.LSDT_IsDeleted).Select(Sel => Sel.LSDT_StudentTypeID).ToList()
                    }).ToList();

            return lstAgencyLocationDepartmentContract;
        }

        Boolean IPlacementMatchingSetupRepository.SaveAgencyLocationDepartment(AgencyLocationDepartmentContract locationDepartment, Int32 currentLoggedInUserId)
        {
            AgencyLocationDepartment agencyLocationDepartment = new AgencyLocationDepartment();
            List<AgencyLocationDepartmentStudentType> lstAgencyLocationDepartmentStudentType = new List<AgencyLocationDepartmentStudentType>();
            //update
            if (!locationDepartment.AgencyLocationDepartmentID.IsNullOrEmpty() && locationDepartment.AgencyLocationDepartmentID > AppConsts.NONE)
            {
                agencyLocationDepartment = SharedDataDBContext.AgencyLocationDepartments.Where(cond => !cond.ALD_IsDeleted && cond.ALD_ID == locationDepartment.AgencyLocationDepartmentID).FirstOrDefault();
                agencyLocationDepartment.ALD_AgencyLocationID = locationDepartment.AgencyLocationID;
                agencyLocationDepartment.ALD_DepartmentID = locationDepartment.DepartmentID;
                agencyLocationDepartment.ALD_ModifiedBy = currentLoggedInUserId;
                agencyLocationDepartment.ALD_ModifiedOn = DateTime.Now;

                lstAgencyLocationDepartmentStudentType = SharedDataDBContext.AgencyLocationDepartmentStudentTypes.Where(cond => !cond.LSDT_IsDeleted && cond.LSDT_AgencyLocationDepartmentID == locationDepartment.AgencyLocationDepartmentID).ToList();
                foreach (Int32 studentTypeId in locationDepartment.lstStudentTypeID)
                {
                    AgencyLocationDepartmentStudentType addAgencyLocationDepartmentStudentType = new AgencyLocationDepartmentStudentType();
                    if (!lstAgencyLocationDepartmentStudentType.Select(sel => sel.LSDT_StudentTypeID).ToList().Contains(studentTypeId))
                    {
                        //Save new added student types
                        addAgencyLocationDepartmentStudentType.LSDT_AgencyLocationDepartmentID = locationDepartment.AgencyLocationDepartmentID;
                        addAgencyLocationDepartmentStudentType.LSDT_StudentTypeID = studentTypeId;
                        addAgencyLocationDepartmentStudentType.LSDT_CreatedBy = currentLoggedInUserId;
                        addAgencyLocationDepartmentStudentType.LSDT_CreatedOn = DateTime.Now;
                        SharedDataDBContext.AgencyLocationDepartmentStudentTypes.AddObject(addAgencyLocationDepartmentStudentType);
                    }
                }

                foreach (AgencyLocationDepartmentStudentType agencyLocationDepartmentStudentType in lstAgencyLocationDepartmentStudentType)
                {
                    //deleted removed student type
                    if (!locationDepartment.lstStudentTypeID.Contains(agencyLocationDepartmentStudentType.LSDT_StudentTypeID))
                    {
                        agencyLocationDepartmentStudentType.LSDT_IsDeleted = true;
                        agencyLocationDepartmentStudentType.LSDT_ModifiedBy = currentLoggedInUserId;
                        agencyLocationDepartmentStudentType.LSDT_ModifiedOn = DateTime.Now;
                    }
                }
            }
            //save
            else
            {
                agencyLocationDepartment.ALD_AgencyLocationID = locationDepartment.AgencyLocationID;
                agencyLocationDepartment.ALD_DepartmentID = locationDepartment.DepartmentID;
                agencyLocationDepartment.ALD_CreatedBy = currentLoggedInUserId;
                agencyLocationDepartment.ALD_CreatedOn = DateTime.Now;

                foreach (Int32 studentTypeId in locationDepartment.lstStudentTypeID)
                {
                    AgencyLocationDepartmentStudentType addAgencyLocationDepartmentStudentType = new AgencyLocationDepartmentStudentType();
                    addAgencyLocationDepartmentStudentType.LSDT_StudentTypeID = studentTypeId;
                    addAgencyLocationDepartmentStudentType.LSDT_CreatedBy = currentLoggedInUserId;
                    addAgencyLocationDepartmentStudentType.LSDT_CreatedOn = DateTime.Now;
                    SharedDataDBContext.AgencyLocationDepartmentStudentTypes.AddObject(addAgencyLocationDepartmentStudentType);
                }
                SharedDataDBContext.AgencyLocationDepartments.AddObject(agencyLocationDepartment);
            }

            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IPlacementMatchingSetupRepository.DeleteAgencyLocationDepartment(Int32 agencyLocationDepartmentId, Int32 currentLoggedInUserId)
        {
            AgencyLocationDepartment agencyLocationDepartment = SharedDataDBContext.AgencyLocationDepartments.Where(cond => !cond.ALD_IsDeleted && cond.ALD_ID == agencyLocationDepartmentId).FirstOrDefault();
            if (!agencyLocationDepartment.IsNullOrEmpty())
            {
                agencyLocationDepartment.ALD_IsDeleted = true;
                agencyLocationDepartment.ALD_ModifiedBy = currentLoggedInUserId;
                agencyLocationDepartment.ALD_ModifiedOn = DateTime.Now;
            }

            List<AgencyLocationDepartmentStudentType> lstAgencyLocationDepartmentStudentType = SharedDataDBContext.AgencyLocationDepartmentStudentTypes.Where(cond => !cond.LSDT_IsDeleted && cond.LSDT_AgencyLocationDepartmentID == agencyLocationDepartmentId).ToList();

            if (!lstAgencyLocationDepartmentStudentType.IsNullOrEmpty())
            {
                foreach (AgencyLocationDepartmentStudentType agencyLocationDepartmentStudentType in lstAgencyLocationDepartmentStudentType)
                {
                    agencyLocationDepartmentStudentType.LSDT_IsDeleted = true;
                    agencyLocationDepartmentStudentType.LSDT_ModifiedBy = currentLoggedInUserId;
                    agencyLocationDepartmentStudentType.LSDT_ModifiedOn = DateTime.Now;
                }
            }

            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IPlacementMatchingSetupRepository.IsDeptMappedWithLocation(Int32 agencyLocationId)
        {
            return SharedDataDBContext.AgencyLocations.Where(Cond => !Cond.AL_IsDeleted && Cond.AL_ID == agencyLocationId).FirstOrDefault().AgencyLocationDepartments.Where(cond => !cond.ALD_IsDeleted).Any();
        }

        List<lkpInventoryAvailabilityType> IPlacementMatchingSetupRepository.GetInstitutionAvailability()
        {
            return SharedDataDBContext.lkpInventoryAvailabilityTypes.Where(cond => !cond.IAT_IsDeleted).ToList();
        }

        List<PlacementMatchingContract> IPlacementMatchingSetupRepository.GetOpportunitySearch(PlacementSearchContract searchContract)
        {
            var lstOpportunities = new List<PlacementMatchingContract>();
            EntityConnection connection = SharedDataDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyLocationID", searchContract.LocationId),
                    new SqlParameter("@DepartmentID", searchContract.DepartmentId),
                    new SqlParameter("@StartDate", searchContract.StartDate),
                    new SqlParameter("@EndDate", searchContract.EndDate),
                    new SqlParameter("@SpecialtyID", searchContract.SpecialtyId),
                    new SqlParameter("@StudentTypeIds", searchContract.StudentTypeIds),
                    new SqlParameter("@Max", searchContract.Max),
                    new SqlParameter("@DayIds", searchContract.Days),
                    new SqlParameter("@Shift", searchContract.Shift),
                    new SqlParameter("@TenantID", searchContract.TenantId),
                    new SqlParameter("@CustomAtrributesData", searchContract.SharedCustomAttributes)

                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetOpportunitySearch", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lstOpportunities.Add(new PlacementMatchingContract
                            {
                                OpportunityID = Convert.ToInt32(dr["OpportunityID"]),
                                Days = Convert.ToString(dr["Days"]),
                                StudentTypes = Convert.ToString(dr["StudentTypes"]),
                                Location = Convert.ToString(dr["Location"]),
                                Shift = Convert.ToString(dr["Shift"]),
                                StartDate = Convert.ToDateTime(dr["StartDate"]),
                                EndDate = Convert.ToDateTime(dr["EndDate"]),
                                Department = Convert.ToString(dr["Department"]),
                                Specialty = Convert.ToString(dr["Specialty"]),
                                IsPreceptionShip = Convert.ToBoolean(dr["IsPreceptionShip"]),
                                Max = Convert.ToInt32(dr["Max"]),
                                Agency = Convert.ToString(dr["Agency"]),
                                Unit = Convert.ToString(dr["Unit"])
                            });
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstOpportunities;
        }

        PlacementMatchingContract IPlacementMatchingSetupRepository.GetOpportunityDetailByID(Int32 opportunityID)
        {
            PlacementMatchingContract placementMatchingOpportunityContract = new PlacementMatchingContract();
            EntityConnection connection = SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@OpportunityID",opportunityID),
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetOpportunityDetailByID", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            placementMatchingOpportunityContract.OpportunityID = Convert.ToInt32(dr["OpportunityID"]);
                            placementMatchingOpportunityContract.InventoryAvailabilityTypeCode = dr["InventoryAvailabilityTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["InventoryAvailabilityTypeCode"]);
                            //placementMatchingOpportunityContract.AgencyIdList = dr["AgencyIdList"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyIdList"]);
                            placementMatchingOpportunityContract.Agency = dr["Agency"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Agency"]);
                            placementMatchingOpportunityContract.Location = dr["Location"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Location"]);
                            placementMatchingOpportunityContract.AgencyLocationID = dr["AgencyLocationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AgencyLocationID"]);
                            placementMatchingOpportunityContract.DepartmentID = dr["DepartmentID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["DepartmentID"]);
                            placementMatchingOpportunityContract.Department = dr["Department"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Department"]);
                            placementMatchingOpportunityContract.SpecialtyID = dr["SpecialtyID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SpecialtyID"]);
                            placementMatchingOpportunityContract.Specialty = dr["Specialty"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Specialty"]);
                            placementMatchingOpportunityContract.StudentTypes = dr["StudentTypes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StudentTypes"]);
                            placementMatchingOpportunityContract.StudentTypeIds = dr["StudentTypeIds"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StudentTypeIds"]);
                            placementMatchingOpportunityContract.Max = dr["Max"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["Max"]);
                            placementMatchingOpportunityContract.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            placementMatchingOpportunityContract.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                            placementMatchingOpportunityContract.Days = dr["Days"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Days"]);
                            placementMatchingOpportunityContract.DayIds = dr["DayIds"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DayIds"]);
                            placementMatchingOpportunityContract.Shift = dr["Shift"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Shift"]);
                            placementMatchingOpportunityContract.StatusID = dr["StatusID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["StatusID"]);
                            placementMatchingOpportunityContract.Status = dr["Status"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Status"]);
                            placementMatchingOpportunityContract.IsArchived = dr["IsArchived"] == DBNull.Value ? false : (Convert.ToBoolean(dr["IsArchived"]));
                            placementMatchingOpportunityContract.IsPreceptionShip = dr["IsPreceptionShip"] == DBNull.Value ? false : (Convert.ToBoolean(dr["IsPreceptionShip"]));
                            placementMatchingOpportunityContract.ContainsFloatArea = dr["ContainsFloatArea"] == DBNull.Value ? false : (Convert.ToBoolean(dr["ContainsFloatArea"]));
                            placementMatchingOpportunityContract.FloatArea = dr["FloatArea"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FloatArea"]);
                            placementMatchingOpportunityContract.Unit = dr["Unit"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Unit"]);
                            placementMatchingOpportunityContract.AgencyHierarchyID = dr["AgencyHierarchyID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AgencyHierarchyID"]);
                        }
                    }
                }
                return placementMatchingOpportunityContract;
            }
        }

        Boolean IPlacementMatchingSetupRepository.PublishOpportunity(Int32 currentLoggedInUserID, PlacementMatchingContract placementMatchingContract, List<CustomAttribteContract> customAttributeList)
        {
            String publishedStatusCode = InventoryStatus.Published.GetStringValue();
            Int32 publishedStatusId = SharedDataDBContext.lkpInventoryStatus.Where(cond => !cond.INS_IsDeleted && cond.INS_Code == publishedStatusCode).FirstOrDefault().INS_ID;

            String primaryInventoryRecordTypeCode = InventoryRecordType.PrimaryInventory.GetStringValue();
            Int32 primaryInventoryRecordTypeID = SharedDataDBContext.lkpInventoryRecordTypes
                                                        .Where(cond => !cond.IRT_IsDeleted && cond.IRT_Code == primaryInventoryRecordTypeCode).FirstOrDefault().IRT_ID;

            return SaveOpportunityData(currentLoggedInUserID, placementMatchingContract, primaryInventoryRecordTypeID, publishedStatusId, customAttributeList);
        }

        Boolean IPlacementMatchingSetupRepository.SaveOpportunity(Int32 currentLoggedInUserID, PlacementMatchingContract placementMatchingContract, List<CustomAttribteContract> customAttributeList)
        {
            String draftStatusCode = InventoryStatus.Draft.GetStringValue();
            Int32 draftStatusId = SharedDataDBContext.lkpInventoryStatus.Where(cond => !cond.INS_IsDeleted && cond.INS_Code == draftStatusCode).FirstOrDefault().INS_ID;

            String primaryInventoryRecordTypeCode = InventoryRecordType.PrimaryInventory.GetStringValue();
            Int32 primaryInventoryRecordTypeID = SharedDataDBContext.lkpInventoryRecordTypes
                                                        .Where(cond => !cond.IRT_IsDeleted && cond.IRT_Code == primaryInventoryRecordTypeCode).FirstOrDefault().IRT_ID;


            return SaveOpportunityData(currentLoggedInUserID, placementMatchingContract, primaryInventoryRecordTypeID, draftStatusId, customAttributeList);
        }

        Boolean SaveOpportunityData(Int32 currentLoggedInUserID, PlacementMatchingContract placementMatchingContract, Int32 inventoryRecordTypeID, Int32 inventoryStatusId, List<CustomAttribteContract> customAttributeList)
        {
            Int32 clinicalInventoryID = SaveClinicalInventory(currentLoggedInUserID, placementMatchingContract, inventoryStatusId);
            Boolean isStudentTypesSavedSuccessfully = SaveClinicalInventoryStudentType(currentLoggedInUserID, placementMatchingContract, clinicalInventoryID);
            Boolean isShiftDetailsSaveSuccessfully = SaveClinicalInventoryShiftDetails(currentLoggedInUserID, placementMatchingContract, clinicalInventoryID);
            Boolean isVacancySavedSuccessfully = SaveClinicalInventoryVacancy(currentLoggedInUserID, placementMatchingContract, inventoryRecordTypeID, clinicalInventoryID);
            Boolean isCustomAttributesSavedSuccessfully = SaveSharedCustomAttributesValue(currentLoggedInUserID, clinicalInventoryID, customAttributeList, CustomAttributeValueRecordType.ClinicalInventory.GetStringValue());

            if (isStudentTypesSavedSuccessfully && isShiftDetailsSaveSuccessfully && isVacancySavedSuccessfully && clinicalInventoryID > AppConsts.NONE)
                return true;
            return false;
        }

        private Int32 SaveClinicalInventory(Int32 currentLoggedInUserID, PlacementMatchingContract placementMatchingContract, Int32 inventoryStatusId)
        {
            ClinicalInventory clinicalInventory = new ClinicalInventory();
            if (!placementMatchingContract.IsNullOrEmpty())
            {
                Int32 inventoryAvailabilityTypeID = SharedDataDBContext.lkpInventoryAvailabilityTypes.Where(cond => !cond.IAT_IsDeleted && cond.IAT_Code == placementMatchingContract.InventoryAvailabilityTypeCode).FirstOrDefault().IAT_ID;

                if (!placementMatchingContract.OpportunityID.IsNullOrEmpty() && placementMatchingContract.OpportunityID > AppConsts.NONE)
                {
                    clinicalInventory = SharedDataDBContext.ClinicalInventories.Where(cond => !cond.CI_IsDeleted && cond.CI_ID == placementMatchingContract.OpportunityID).FirstOrDefault();
                    AgencyLocation agencyLocation = SharedDataDBContext.AgencyLocations.Where(cond => !cond.AL_IsDeleted && cond.AL_ID == placementMatchingContract.AgencyLocationID).FirstOrDefault();

                    clinicalInventory.CI_AgencyLocationDepartmentID = agencyLocation.AgencyLocationDepartments.Where(cond => !cond.ALD_IsDeleted && cond.ALD_DepartmentID == placementMatchingContract.DepartmentID).FirstOrDefault().ALD_ID;
                    clinicalInventory.CI_InventoryAvailabilityTypeID = inventoryAvailabilityTypeID;
                    clinicalInventory.CI_SpecialityID = placementMatchingContract.SpecialtyID;
                    clinicalInventory.CI_Experience = agencyLocation.AL_Experience;
                    clinicalInventory.CI_StartDate = Convert.ToDateTime(placementMatchingContract.StartDate);
                    clinicalInventory.CI_EndDate = Convert.ToDateTime(placementMatchingContract.EndDate);
                    clinicalInventory.CI_Unit = placementMatchingContract.Unit;
                    clinicalInventory.CI_ContainsFloatArea = placementMatchingContract.ContainsFloatArea;
                    clinicalInventory.CI_FloatArea = placementMatchingContract.FloatArea;
                    clinicalInventory.CI_InventoryStatusID = inventoryStatusId;
                    clinicalInventory.CI_DayString = placementMatchingContract.Days;
                    clinicalInventory.CI_ShiftString = placementMatchingContract.Shift;
                    clinicalInventory.CI_IsArchived = false;
                    clinicalInventory.CI_IsPreceptionship = placementMatchingContract.IsPreceptionShip;
                    clinicalInventory.CI_ModifiedBy = currentLoggedInUserID;
                    clinicalInventory.CI_ModifiedOn = DateTime.Now;
                }
                else
                {
                    AgencyLocation agencyLocation = SharedDataDBContext.AgencyLocations.Where(cond => !cond.AL_IsDeleted && cond.AL_ID == placementMatchingContract.AgencyLocationID).FirstOrDefault();
                    clinicalInventory.CI_AgencyLocationDepartmentID = agencyLocation.AgencyLocationDepartments.Where(cond => !cond.ALD_IsDeleted && cond.ALD_DepartmentID == placementMatchingContract.DepartmentID).FirstOrDefault().ALD_ID;//placementMatchingContract.AGen.CI_ID;
                    clinicalInventory.CI_InventoryAvailabilityTypeID = inventoryAvailabilityTypeID;
                    clinicalInventory.CI_SpecialityID = placementMatchingContract.SpecialtyID;
                    clinicalInventory.CI_Experience = agencyLocation.AL_Experience;
                    clinicalInventory.CI_StartDate = Convert.ToDateTime(placementMatchingContract.StartDate);
                    clinicalInventory.CI_EndDate = Convert.ToDateTime(placementMatchingContract.EndDate);
                    clinicalInventory.CI_Unit = placementMatchingContract.Unit;
                    clinicalInventory.CI_ContainsFloatArea = placementMatchingContract.ContainsFloatArea;
                    clinicalInventory.CI_FloatArea = placementMatchingContract.FloatArea;
                    clinicalInventory.CI_InventoryStatusID = inventoryStatusId;
                    clinicalInventory.CI_DayString = placementMatchingContract.Days;
                    clinicalInventory.CI_ShiftString = placementMatchingContract.Shift;
                    clinicalInventory.CI_IsArchived = false;
                    clinicalInventory.CI_IsPreceptionship = placementMatchingContract.IsPreceptionShip;
                    clinicalInventory.CI_CreatedBy = currentLoggedInUserID;
                    clinicalInventory.CI_CreatedOn = DateTime.Now;
                    SharedDataDBContext.ClinicalInventories.AddObject(clinicalInventory);
                }

            }

            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return clinicalInventory.CI_ID;
            return AppConsts.NONE;
        }

        private Boolean SaveClinicalInventoryStudentType(Int32 currentLoggedInUserID, PlacementMatchingContract placementMatchingContract, Int32 clinicalInventoryID)
        {
            if (!placementMatchingContract.IsNullOrEmpty())
            {
                List<ClinicalInventoryStudentType> lstClinicalInventoryStudentType = new List<ClinicalInventoryStudentType>();
                if (!placementMatchingContract.OpportunityID.IsNullOrEmpty() && placementMatchingContract.OpportunityID > AppConsts.NONE)
                {
                    lstClinicalInventoryStudentType = SharedDataDBContext.ClinicalInventoryStudentTypes.Where(cond => !cond.CIST_IsDeleted && cond.CIST_ClinicalInventoryID == placementMatchingContract.OpportunityID).ToList();
                    List<String> lstStudentTypeId = placementMatchingContract.StudentTypeIds.Split(',').ToList();
                    foreach (String studentTypeId in lstStudentTypeId)
                    {
                        Int32 studentTypeID = Convert.ToInt32(studentTypeId);
                        ClinicalInventoryStudentType addClinicalInventoryStudentType = new ClinicalInventoryStudentType();
                        if (!lstClinicalInventoryStudentType.Select(sel => sel.CIST_StudentTypeID).ToList().Contains(studentTypeID))
                        {
                            //Save new added student types
                            addClinicalInventoryStudentType.CIST_ClinicalInventoryID = placementMatchingContract.OpportunityID;
                            addClinicalInventoryStudentType.CIST_StudentTypeID = studentTypeID;
                            addClinicalInventoryStudentType.CIST_CreatedBy = currentLoggedInUserID;
                            addClinicalInventoryStudentType.CIST_CreatedOn = DateTime.Now;
                            SharedDataDBContext.ClinicalInventoryStudentTypes.AddObject(addClinicalInventoryStudentType);
                        }

                        if (lstClinicalInventoryStudentType.Select(sel => sel.CIST_StudentTypeID).ToList().Contains(studentTypeID))
                        {
                            //update existing
                            addClinicalInventoryStudentType = SharedDataDBContext.ClinicalInventoryStudentTypes.Where(cond => cond.CIST_ClinicalInventoryID == placementMatchingContract.OpportunityID && !cond.CIST_IsDeleted && cond.CIST_StudentTypeID == studentTypeID).FirstOrDefault();
                            addClinicalInventoryStudentType = lstClinicalInventoryStudentType.Where(cond => cond.CIST_StudentTypeID == studentTypeID).FirstOrDefault();

                            addClinicalInventoryStudentType.CIST_ClinicalInventoryID = placementMatchingContract.OpportunityID;
                            addClinicalInventoryStudentType.CIST_StudentTypeID = studentTypeID;
                            addClinicalInventoryStudentType.CIST_ModifiedBy = currentLoggedInUserID;
                            addClinicalInventoryStudentType.CIST_ModifiedOn = DateTime.Now;
                        }
                    }

                    foreach (ClinicalInventoryStudentType clinicalInventoryStudentType in lstClinicalInventoryStudentType)
                    {
                        //deleted removed student type
                        if (!lstStudentTypeId.Contains(clinicalInventoryStudentType.CIST_StudentTypeID.ToString()))
                        {
                            clinicalInventoryStudentType.CIST_IsDeleted = true;
                            clinicalInventoryStudentType.CIST_ModifiedBy = currentLoggedInUserID;
                            clinicalInventoryStudentType.CIST_ModifiedOn = DateTime.Now;
                        }
                    }
                }
                else
                {
                    List<String> lstStudentTypeId = placementMatchingContract.StudentTypeIds.Split(',').ToList();
                    foreach (String studentTypeId in lstStudentTypeId)
                    {
                        ClinicalInventoryStudentType clinicalInventoryStudentType = new ClinicalInventoryStudentType();
                        clinicalInventoryStudentType.CIST_StudentTypeID = Convert.ToInt32(studentTypeId);
                        clinicalInventoryStudentType.CIST_CreatedBy = currentLoggedInUserID;
                        clinicalInventoryStudentType.CIST_CreatedOn = DateTime.Now;
                        clinicalInventoryStudentType.CIST_ClinicalInventoryID = clinicalInventoryID;
                        SharedDataDBContext.ClinicalInventoryStudentTypes.AddObject(clinicalInventoryStudentType);
                    }
                }
            }
            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        private Boolean SaveClinicalInventoryShiftDetails(Int32 currentLoggedInUserID, PlacementMatchingContract placementMatchingContract, Int32 clinicalInventoryID)
        {
            List<ClinicalInventoryShift> lstClinicalInventoryShift = new List<ClinicalInventoryShift>();
            if (!placementMatchingContract.IsNullOrEmpty())
            {
                List<ClinicalInventoryStudentType> lstClinicalInventoryStudentType = new List<ClinicalInventoryStudentType>();
                //Update Shift Detail
                if (!placementMatchingContract.OpportunityID.IsNullOrEmpty() && placementMatchingContract.OpportunityID > AppConsts.NONE)
                {
                    lstClinicalInventoryShift = SharedDataDBContext.ClinicalInventoryShifts.Where(cond => !cond.CIS_IsDeleted && cond.CIS_ClinicalInventoryID == placementMatchingContract.OpportunityID).ToList();
                    List<Int32> lstShiftIds = placementMatchingContract.lstShift.Select(Sel => Sel.ClinicalInventoryShiftID).ToList();

                    foreach (ShiftDetails shiftDetail in placementMatchingContract.lstShift)
                    {
                        //Int32 dayID = Convert.ToInt32(DayId);
                        ClinicalInventoryShift clinicalInventoryShift = new ClinicalInventoryShift();
                        List<ClinicalInventoryShiftDay> lstClinicalInventoryShiftDay = new List<ClinicalInventoryShiftDay>();
                        if (shiftDetail.ClinicalInventoryShiftID > AppConsts.NONE)
                        {
                            //Edit Shift
                            clinicalInventoryShift = SharedDataDBContext.ClinicalInventoryShifts.Where(cond => !cond.CIS_IsDeleted && cond.CIS_ID == shiftDetail.ClinicalInventoryShiftID).FirstOrDefault();
                            clinicalInventoryShift.CIS_ClinicalInventoryID = placementMatchingContract.OpportunityID;
                            clinicalInventoryShift.CIS_ShiftFrom = (TimeSpan)shiftDetail.ShiftFrom;
                            clinicalInventoryShift.CIS_ShiftTo = (TimeSpan)shiftDetail.ShiftTo;
                            clinicalInventoryShift.CIS_NoOfStudents = shiftDetail.NumberOfStudents;
                            clinicalInventoryShift.CIS_Shift = shiftDetail.Shift;
                            clinicalInventoryShift.CIS_ModifiedBy = currentLoggedInUserID;
                            clinicalInventoryShift.CIS_ModifiedOn = DateTime.Now;

                            lstClinicalInventoryShiftDay = clinicalInventoryShift.ClinicalInventoryShiftDays.Where(cond => !cond.CISD_IsDeleted).ToList();
                            List<Int32> lstShiftDayIds = lstClinicalInventoryShiftDay.Select(sel => sel.CISD_DayID).ToList();
                            foreach (Int32 dayId in shiftDetail.lstDaysId)
                            {
                                if (!lstShiftDayIds.Contains(dayId))
                                {
                                    ClinicalInventoryShiftDay newClinicalInventoryShiftDay = new ClinicalInventoryShiftDay();
                                    //Add new day Id.
                                    newClinicalInventoryShiftDay.CISD_DayID = dayId;
                                    newClinicalInventoryShiftDay.CISD_CreatedBy = currentLoggedInUserID;
                                    newClinicalInventoryShiftDay.CISD_CreatedOn = DateTime.Now;
                                    clinicalInventoryShift.ClinicalInventoryShiftDays.Add(newClinicalInventoryShiftDay);
                                    //SharedDataDBContext.ClinicalInventoryShiftDays.AddObject(newClinicalInventoryShiftDay);
                                }
                            }

                            foreach (Int32 dayID in lstShiftDayIds)
                            {
                                if (!shiftDetail.lstDaysId.Contains(dayID))
                                {
                                    //Remove
                                    ClinicalInventoryShiftDay removeClinicalInventoryShiftDay = lstClinicalInventoryShiftDay.Where(cond => cond.CISD_DayID == dayID).FirstOrDefault();
                                    removeClinicalInventoryShiftDay.CISD_IsDeleted = true;
                                    removeClinicalInventoryShiftDay.CISD_ModifiedBy = currentLoggedInUserID;
                                    removeClinicalInventoryShiftDay.CISD_ModifiedOn = DateTime.Now;
                                }
                            }

                        }
                        else
                        {
                            //Save new Shift
                            clinicalInventoryShift.CIS_ClinicalInventoryID = placementMatchingContract.OpportunityID;
                            clinicalInventoryShift.CIS_ShiftFrom = (TimeSpan)shiftDetail.ShiftFrom;
                            clinicalInventoryShift.CIS_ShiftTo = (TimeSpan)shiftDetail.ShiftTo;
                            clinicalInventoryShift.CIS_NoOfStudents = shiftDetail.NumberOfStudents;
                            clinicalInventoryShift.CIS_Shift = shiftDetail.Shift;
                            clinicalInventoryShift.CIS_CreatedBy = currentLoggedInUserID;
                            clinicalInventoryShift.CIS_CreatedOn = DateTime.Now;
                            SharedDataDBContext.ClinicalInventoryShifts.AddObject(clinicalInventoryShift);

                            foreach (Int32 dayId in shiftDetail.lstDaysId)
                            {
                                ClinicalInventoryShiftDay clinicalInventoryShiftDay = new ClinicalInventoryShiftDay();
                                //clinicalInventoryShiftDay.CISD_ClinicalInventoryShiftID = 
                                clinicalInventoryShiftDay.CISD_DayID = dayId;
                                clinicalInventoryShiftDay.CISD_CreatedBy = currentLoggedInUserID;
                                clinicalInventoryShiftDay.CISD_CreatedOn = DateTime.Now;
                                clinicalInventoryShift.ClinicalInventoryShiftDays.Add(clinicalInventoryShiftDay);
                                //SharedDataDBContext.ClinicalInventoryShiftDays.AddObject(clinicalInventoryShiftDay);
                            }

                        }
                    }

                    foreach (ClinicalInventoryShift clinicalInventoryShift in lstClinicalInventoryShift)
                    {
                        //deleted removed Shifts
                        if (!lstShiftIds.Contains(clinicalInventoryShift.CIS_ID))
                        {
                            clinicalInventoryShift.CIS_IsDeleted = true;
                            clinicalInventoryShift.CIS_ModifiedBy = currentLoggedInUserID;
                            clinicalInventoryShift.CIS_ModifiedOn = DateTime.Now;

                            //deleted removed Shift days
                            List<ClinicalInventoryShiftDay> lstClinicalInventoryShiftDay = clinicalInventoryShift.ClinicalInventoryShiftDays.Where(cond => !cond.CISD_IsDeleted).ToList();
                            foreach (ClinicalInventoryShiftDay clinicalInventoryShiftDay in lstClinicalInventoryShiftDay)
                            {
                                clinicalInventoryShiftDay.CISD_IsDeleted = true;
                                clinicalInventoryShiftDay.CISD_ModifiedBy = currentLoggedInUserID;
                                clinicalInventoryShiftDay.CISD_ModifiedOn = DateTime.Now;
                            }
                        }
                    }
                }
                //Insert Shift Details
                else
                {
                    foreach (ShiftDetails shiftDetail in placementMatchingContract.lstShift)
                    {
                        ClinicalInventoryShift clinicalInventoryShift = new ClinicalInventoryShift();
                        clinicalInventoryShift.CIS_ClinicalInventoryID = clinicalInventoryID;
                        clinicalInventoryShift.CIS_ShiftFrom = (TimeSpan)shiftDetail.ShiftFrom;
                        clinicalInventoryShift.CIS_ShiftTo = (TimeSpan)shiftDetail.ShiftTo;
                        clinicalInventoryShift.CIS_NoOfStudents = shiftDetail.NumberOfStudents;
                        clinicalInventoryShift.CIS_Shift = shiftDetail.Shift;
                        clinicalInventoryShift.CIS_CreatedBy = currentLoggedInUserID;
                        clinicalInventoryShift.CIS_CreatedOn = DateTime.Now;
                        //clinicalInventory.ClinicalInventoryShifts.Add(clinicalInventoryShift);
                        SharedDataDBContext.ClinicalInventoryShifts.AddObject(clinicalInventoryShift);

                        foreach (Int32 dayId in shiftDetail.lstDaysId)
                        {
                            ClinicalInventoryShiftDay clinicalInventoryShiftDay = new ClinicalInventoryShiftDay();
                            //clinicalInventoryShiftDay.CISD_ClinicalInventoryShiftID = 
                            clinicalInventoryShiftDay.CISD_DayID = dayId;
                            clinicalInventoryShiftDay.CISD_CreatedBy = currentLoggedInUserID;
                            clinicalInventoryShiftDay.CISD_CreatedOn = DateTime.Now;
                            clinicalInventoryShift.ClinicalInventoryShiftDays.Add(clinicalInventoryShiftDay);
                            //SharedDataDBContext.ClinicalInventoryShiftDays.AddObject(clinicalInventoryShiftDay);
                        }
                    }

                }
            }
            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        private Boolean SaveClinicalInventoryVacancy(Int32 currentLoggedInUserID, PlacementMatchingContract placementMatchingContract, Int32 inventoryRecordTypeID, Int32 clinicalInventoryID)
        {
            if (!placementMatchingContract.IsNullOrEmpty())
            {
                ClinicalInventoryVacancy clinicalInventoryVacancy = new ClinicalInventoryVacancy();
                if (!placementMatchingContract.OpportunityID.IsNullOrEmpty() && placementMatchingContract.OpportunityID > AppConsts.NONE)
                {
                    Int32 total = AppConsts.NONE;
                    foreach (ShiftDetails shiftDetail in placementMatchingContract.lstShift)
                    {
                        total = total + (shiftDetail.NumberOfStudents * shiftDetail.lstDaysId.Count());
                    }

                    clinicalInventoryVacancy = SharedDataDBContext.ClinicalInventoryVacancies.Where(cond => !cond.CIV_IsDeleted && cond.CIV_RecordID == placementMatchingContract.OpportunityID && cond.CIV_RecordTypeID == inventoryRecordTypeID).FirstOrDefault();
                    clinicalInventoryVacancy.CIV_Total = total;
                    clinicalInventoryVacancy.CIV_Open = total;  // need to be calculated.
                    clinicalInventoryVacancy.CIV_ModifiedBy = currentLoggedInUserID;
                    clinicalInventoryVacancy.CIV_ModifiedOn = DateTime.Now;
                }

                else
                {
                    Int32 total = AppConsts.NONE;
                    foreach (ShiftDetails shiftDetail in placementMatchingContract.lstShift)
                    {
                        total = total + (shiftDetail.NumberOfStudents * shiftDetail.lstDaysId.Count());
                    }

                    clinicalInventoryVacancy.CIV_RecordID = clinicalInventoryID;
                    clinicalInventoryVacancy.CIV_RecordTypeID = inventoryRecordTypeID;
                    clinicalInventoryVacancy.CIV_Total = total;
                    clinicalInventoryVacancy.CIV_Open = placementMatchingContract.Max;
                    clinicalInventoryVacancy.CIV_CreatedBy = currentLoggedInUserID;
                    clinicalInventoryVacancy.CIV_CreatedOn = DateTime.Now;
                    SharedDataDBContext.ClinicalInventoryVacancies.AddObject(clinicalInventoryVacancy);
                }
            }
            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        private Boolean SaveSharedCustomAttributesValue(Int32 currentLoggedInUserID, Int32 recordId, List<CustomAttribteContract> customAttributeList, String recordTypeCode)
        {
            if (!customAttributeList.IsNullOrEmpty())
            {
                Int32 recordTypeID = SharedDataDBContext.lkpRecordTypes.Where(cond => !cond.RT_IsDeleted && cond.RT_Code == recordTypeCode).FirstOrDefault().RT_ID;
                foreach (CustomAttribteContract customAttributeContract in customAttributeList)
                {
                    SharedCustomAttributeValue sharedCustomAttributeValue = new SharedCustomAttributeValue();
                    if (customAttributeContract.CustomAttrValueId > AppConsts.NONE)
                    {
                        //update Attribute Value.
                        sharedCustomAttributeValue = SharedDataDBContext.SharedCustomAttributeValues.Where(cond => !cond.SCAV_IsDeleted && cond.SCAV_ID == customAttributeContract.CustomAttrValueId).FirstOrDefault();
                        sharedCustomAttributeValue.SCAV_SharedCustomAttributeMappingID = customAttributeContract.CustomAttrMappingId;
                        sharedCustomAttributeValue.SCAV_AttributeValue = customAttributeContract.CustomAttributeValue;
                        sharedCustomAttributeValue.SCAV_RecordID = recordId;
                        sharedCustomAttributeValue.SCAV_RecordTypeID = recordTypeID;
                        sharedCustomAttributeValue.SCAV_ModifiedBy = currentLoggedInUserID;
                        sharedCustomAttributeValue.SCAV_ModifiedOn = DateTime.Now;
                    }

                    else
                    {
                        //Add Attribute Value.
                        sharedCustomAttributeValue.SCAV_RecordID = recordId;
                        sharedCustomAttributeValue.SCAV_RecordTypeID = recordTypeID;
                        sharedCustomAttributeValue.SCAV_SharedCustomAttributeMappingID = customAttributeContract.CustomAttrMappingId;
                        sharedCustomAttributeValue.SCAV_AttributeValue = customAttributeContract.CustomAttributeValue;
                        sharedCustomAttributeValue.SCAV_CreatedBy = currentLoggedInUserID;
                        sharedCustomAttributeValue.SCAV_CreatedOn = DateTime.Now;

                        SharedDataDBContext.SharedCustomAttributeValues.AddObject(sharedCustomAttributeValue);
                    }
                }

                if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
                return false;
            }
            return false;
        }


        Boolean IPlacementMatchingSetupRepository.DeleteOpportunity(Int32 currentLoggedInUserID, List<Int32> lstSelectedOpportunityId)
        {

            List<ClinicalInventory> lstClinicalInventory = new List<ClinicalInventory>();
            lstClinicalInventory = SharedDataDBContext.ClinicalInventories.Where(cond => !cond.CI_IsDeleted && lstSelectedOpportunityId.Contains(cond.CI_ID)).ToList();
            if (!lstClinicalInventory.IsNullOrEmpty())
            {
                //Get primary inventory type id to delete ClinicalInventoryVacancy mapped with that ClinicalInventory Record of primary inventory type.
                //to do it -- uncomment the below two lines code

                String primaryInventoryRecordTypeCode = InventoryRecordType.PrimaryInventory.GetStringValue();
                Int32 primaryInventoryRecordTypeID = SharedDataDBContext.lkpInventoryRecordTypes.Where(cond => !cond.IRT_IsDeleted && cond.IRT_Code == primaryInventoryRecordTypeCode).FirstOrDefault().IRT_ID;

                foreach (ClinicalInventory clinicalInventory in lstClinicalInventory)
                {

                    clinicalInventory.CI_IsDeleted = true;
                    clinicalInventory.CI_ModifiedBy = currentLoggedInUserID;
                    clinicalInventory.CI_ModifiedOn = DateTime.Now;

                    //Delete clinicalInventoryDays mapped with the ClinicalInventories
                    List<ClinicalInventoryShift> lstClinicalInventoryShift = new List<ClinicalInventoryShift>();

                    lstClinicalInventoryShift = SharedDataDBContext.ClinicalInventoryShifts.Where(cond => !cond.CIS_IsDeleted && clinicalInventory.CI_ID == cond.CIS_ClinicalInventoryID).ToList();

                    foreach (ClinicalInventoryShift clinicalInventoryShift in lstClinicalInventoryShift)
                    {
                        ShiftDetails shift = new ShiftDetails();
                        shift.ClinicalInventoryShiftID = clinicalInventoryShift.CIS_ID;
                        DeleteShiftDetail(currentLoggedInUserID, shift);

                        //clinicalInventoryShift.CIS_IsDeleted = true;
                        //clinicalInventoryShift.CIS_ModifiedBy = currentLoggedInUserID;
                        //clinicalInventoryShift.CIS_ModifiedOn = DateTime.Now;

                        //List<ClinicalInventoryShiftDay> lstClinicalInventoryShiftDay = clinicalInventoryShift.ClinicalInventoryShiftDays.Where(cond => !cond.CISD_IsDeleted).ToList();
                        //foreach (ClinicalInventoryShiftDay clinicalInventoryShiftDay in lstClinicalInventoryShiftDay)
                        //{
                        //    clinicalInventoryShiftDay.CISD_IsDeleted = true;
                        //    clinicalInventoryShiftDay.CISD_ModifiedBy = currentLoggedInUserID;
                        //    clinicalInventoryShiftDay.CISD_ModifiedOn = DateTime.Now;
                        //}
                    }

                    //Delete ClinicalInventoryStudentType mapped with the ClinicalInventories
                    List<ClinicalInventoryStudentType> lstClinicalInventoryStudentType = new List<ClinicalInventoryStudentType>();
                    lstClinicalInventoryStudentType = clinicalInventory.ClinicalInventoryStudentTypes.Where(cond => !cond.CIST_IsDeleted).ToList();
                    foreach (ClinicalInventoryStudentType clinicalInventoryStudentType in lstClinicalInventoryStudentType)
                    {
                        clinicalInventoryStudentType.CIST_IsDeleted = true;
                        clinicalInventoryStudentType.CIST_ModifiedBy = currentLoggedInUserID;
                        clinicalInventoryStudentType.CIST_ModifiedOn = DateTime.Now;
                    }

                    //Delete ClinicalInventoryVacancy mapped with the ClinicalInventories depending upon Record type.
                    //to do it -- uncomment the below code
                    // Check if it is primary inventory record type

                    List<ClinicalInventoryVacancy> lstClinicalInventoryVacancy = new List<ClinicalInventoryVacancy>();
                    lstClinicalInventoryVacancy = SharedDataDBContext.ClinicalInventoryVacancies.Where(cond => !cond.CIV_IsDeleted && cond.CIV_RecordID == clinicalInventory.CI_ID
                                                        && cond.CIV_RecordTypeID == primaryInventoryRecordTypeID).ToList();

                    foreach (ClinicalInventoryVacancy clinicalInventoryVacancy in lstClinicalInventoryVacancy)
                    {
                        clinicalInventoryVacancy.CIV_IsDeleted = true;
                        clinicalInventoryVacancy.CIV_ModifiedBy = currentLoggedInUserID;
                        clinicalInventoryVacancy.CIV_ModifiedOn = DateTime.Now;
                    }


                    //Delete AssociatedTenantInventory mapped with the ClinicalInventories depending upon if inventoryAvailabilityType is Associated Institution.
                    //to Do :-  Uncomment the below code
                    if (clinicalInventory.lkpInventoryAvailabilityType.IAT_Code == InstitutionAvailabilityType.AssociatedInstitution.GetStringValue())
                    {
                        //List<AssociatedTenantInventory> lstAssociatedTenantInventory = new List<AssociatedTenantInventory>();
                        //lstAssociatedTenantInventory = clinicalInventory.AssociatedTenantInventory.Where(cond => !cond.ATI_IsDeleted).ToList();
                        //foreach (AssociatedTenantInventory associatedTenantInventory in lstAssociatedTenantInventory)
                        //{
                        //    associatedTenantInventory.ATI_IsDeleted = true;
                        //    associatedTenantInventory.ATI_ModifiedBy = currentLoggedInUserID;
                        //    associatedTenantInventory.ATI_ModifiedOn= DateTime.Now;
                        //}
                    }
                }

            }

            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IPlacementMatchingSetupRepository.ArchiveOpportunities(Int32 currentLoggedInUserID, List<Int32> lstSelectedOpportunityId)
        {
            List<ClinicalInventory> lstClinicalInventory = new List<ClinicalInventory>();
            lstClinicalInventory = SharedDataDBContext.ClinicalInventories.Where(cond => !cond.CI_IsDeleted && lstSelectedOpportunityId.Contains(cond.CI_ID)).ToList();
            if (!lstClinicalInventory.IsNullOrEmpty())
            {
                foreach (ClinicalInventory clinicalInventory in lstClinicalInventory)
                {
                    clinicalInventory.CI_IsArchived = true;
                    clinicalInventory.CI_ModifiedBy = currentLoggedInUserID;
                    clinicalInventory.CI_ModifiedOn = DateTime.Now;
                }
            }
            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IPlacementMatchingSetupRepository.UnArchiveOpportunities(Int32 currentLoggedInUserID, List<Int32> lstSelectedOpportunityId)
        {
            List<ClinicalInventory> lstClinicalInventory = new List<ClinicalInventory>();
            lstClinicalInventory = SharedDataDBContext.ClinicalInventories.Where(cond => !cond.CI_IsDeleted && lstSelectedOpportunityId.Contains(cond.CI_ID)).ToList();
            if (!lstClinicalInventory.IsNullOrEmpty())
            {
                foreach (ClinicalInventory clinicalInventory in lstClinicalInventory)
                {
                    clinicalInventory.CI_IsArchived = false;
                    clinicalInventory.CI_ModifiedBy = currentLoggedInUserID;
                    clinicalInventory.CI_ModifiedOn = DateTime.Now;
                }
            }
            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        List<RequestDetailContract> IPlacementMatchingSetupRepository.GetRequests(RequestDetailContract searchRequestContract)
        {
            List<RequestDetailContract> lstRequests = new List<RequestDetailContract>();
            //String orderBy = "StartDate";
            //String ordDirection = null;

            //orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            //ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@InventoryAvailabilityTypeCode",searchRequestContract.InventoryAvailabilityTypeCode),
                            new SqlParameter("@AgencyLocationID", searchRequestContract.AgencyLocationID),
                            new SqlParameter("@DepartmentID", searchRequestContract.DepartmentID),
                            new SqlParameter("@StartDate", searchRequestContract.StartDate),
                            new SqlParameter("@EndDate", searchRequestContract.EndDate),
                            new SqlParameter("@SpecialtyID", searchRequestContract.SpecialtyID),
                            new SqlParameter("@StudentTypeIds", searchRequestContract.StudentTypeIds),
                            new SqlParameter("@Max", searchRequestContract.Max),
                            new SqlParameter("@DayIds", searchRequestContract.DayIds),
                            new SqlParameter("@Shift", searchRequestContract.Shift),
                            new SqlParameter("@StatusID", searchRequestContract.StatusID),
                            new SqlParameter("@CustomAtrributesData", searchRequestContract.SharedCustomAttributes)
                            //Agency hierarchy selection 
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetInventoryRequests", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequestDetailContract requestContract = new RequestDetailContract();

                            requestContract.OpportunityID = dr["OpportunityID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OpportunityID"]);
                            requestContract.RequestID = dr["RequestID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequestID"]);
                            requestContract.InstitutionID = dr["InstitutionID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["InstitutionID"]);
                            requestContract.InstitutionName = dr["InstitutionName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["InstitutionName"]);
                            requestContract.AgencyID = dr["AgencyHierarchyID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AgencyHierarchyID"]);
                            requestContract.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            requestContract.AgencyLocationID = dr["AgencyLocationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AgencyLocationID"]);
                            requestContract.Location = dr["Location"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Location"]);
                            requestContract.DepartmentID = dr["DepartmentID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["DepartmentID"]);
                            requestContract.Department = dr["Department"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Department"]);
                            requestContract.SpecialtyID = dr["SpecialtyID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SpecialtyID"]);
                            requestContract.Specialty = dr["Specialty"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Specialty"]);
                            requestContract.StudentTypes = dr["StudentTypes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StudentTypes"]);
                            requestContract.StudentTypeIds = dr["StudentTypeIds"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StudentTypeIds"]);
                            requestContract.Max = dr["Max"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["Max"]);
                            requestContract.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            requestContract.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                            requestContract.Days = dr["Days"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Days"]);
                            //    requestContract.DayIds = dr["DayIds"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DayIds"]);
                            requestContract.Shift = dr["Shift"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Shift"]);
                            requestContract.StatusID = dr["StatusID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["StatusID"]);
                            requestContract.RequestStatus = dr["Status"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Status"]);
                            requestContract.StatusCode = dr["StatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StatusCode"]);

                            lstRequests.Add(requestContract);
                        }
                    }
                }
                return lstRequests;
            }
        }

        List<DepartmentContract> IPlacementMatchingSetupRepository.GetLocationDepartments(Int32 selectedAgencyLocationID)
        {
            List<DepartmentContract> lstDepartmentContract = new List<DepartmentContract>();
            lstDepartmentContract = SharedDataDBContext.AgencyLocationDepartments.Where(cond => !cond.ALD_IsDeleted && cond.ALD_AgencyLocationID == selectedAgencyLocationID).Select(x =>
               new DepartmentContract
               {
                   DepartmentID = x.ALD_DepartmentID,
                   Name = x.Department.DP_Name,
                   Description = x.Department.DP_Description
               }).ToList();
            return lstDepartmentContract;
        }

        List<StudentTypeContract> IPlacementMatchingSetupRepository.GetAgencyDepartmentStudentTypes(Int32 selectedDepartmentID, Int32 selectedAgencyLocationID)
        {
            List<StudentTypeContract> lstStudentTypeContract = new List<StudentTypeContract>();
            AgencyLocationDepartment agencyLocationDepartment = new AgencyLocationDepartment();
            ////1st Approach
            //lstStudentTypeContract = SharedDataDBContext.AgencyLocationDepartments.Where(cond => !cond.ALD_IsDeleted && cond.ALD_DepartmentID == selectedDepartmentID)
            //                            .Select(x => new StudentTypeContract
            //                            {
            //                                StudentTypeId = x.AgencyLocationDepartmentStudentTypes.Where(cond => !cond.LSDT_IsDeleted).Select(sel => sel.LSDT_StudentTypeID).FirstOrDefault(),
            //                                Name = x.AgencyLocationDepartmentStudentTypes.Where(cond => !cond.LSDT_IsDeleted).Select(sel => sel.StudentType.ST_Name).FirstOrDefault(),
            //                                Description = x.AgencyLocationDepartmentStudentTypes.Where(cond => !cond.LSDT_IsDeleted).Select(sel => sel.StudentType.ST_Description).FirstOrDefault(),
            //                            }).ToList();

            ////2nd Approach
            agencyLocationDepartment = SharedDataDBContext.AgencyLocationDepartments.Where(cond => !cond.ALD_IsDeleted && cond.ALD_DepartmentID == selectedDepartmentID && cond.ALD_AgencyLocationID == selectedAgencyLocationID).FirstOrDefault();

            if (!agencyLocationDepartment.IsNullOrEmpty())
            {
                lstStudentTypeContract = agencyLocationDepartment.AgencyLocationDepartmentStudentTypes.Where(cond => !cond.LSDT_IsDeleted).Select(x => new StudentTypeContract
                {
                    StudentTypeId = x.LSDT_StudentTypeID,
                    Name = x.StudentType.ST_Name,
                    Description = x.StudentType.ST_Description
                }).ToList();
            }

            return lstStudentTypeContract;
        }

        #endregion

        List<PlacementMatchingContract> IPlacementMatchingSetupRepository.GetOpportunities(PlacementMatchingContract searchContract)
        {
            List<PlacementMatchingContract> placementMatchingInventoryList = new List<PlacementMatchingContract>();
            //String orderBy = "StartDate";
            //String ordDirection = null;

            //orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            //ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@InventoryAvailabilityTypeCode",searchContract.InventoryAvailabilityTypeCode),
                            new SqlParameter("@AgencyLocationID", searchContract.AgencyLocationID),
                            new SqlParameter("@DepartmentID", searchContract.DepartmentID),
                            new SqlParameter("@StartDate", searchContract.StartDate),
                            new SqlParameter("@EndDate", searchContract.EndDate),
                            new SqlParameter("@SpecialtyID", searchContract.SpecialtyID),
                            new SqlParameter("@StudentTypeIds", searchContract.StudentTypeIds),
                            new SqlParameter("@Max", searchContract.Max),
                            new SqlParameter("@DayIds", searchContract.DayIds),
                            new SqlParameter("@Shift", searchContract.Shift),
                            new SqlParameter("@IsArchived", searchContract.IsArchived),
                            new SqlParameter("@CustomAtrributesData", searchContract.SharedCustomAttributes),
                            //Agency hierarchy selection 
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetClinicalInventorySearch", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            PlacementMatchingContract placementMatchingInventory = new PlacementMatchingContract();

                            placementMatchingInventory.OpportunityID = Convert.ToInt32(dr["OpportunityID"]);

                            //placementMatchingInventory.AgencyIdList = dr["AgencyIdList"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyIdList"]);
                            placementMatchingInventory.Agency = dr["Agency"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Agency"]);

                            placementMatchingInventory.Location = dr["Location"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Location"]);
                            placementMatchingInventory.AgencyLocationID = dr["AgencyLocationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AgencyLocationID"]);

                            placementMatchingInventory.DepartmentID = dr["DepartmentID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["DepartmentID"]);
                            placementMatchingInventory.Department = dr["Department"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Department"]);

                            placementMatchingInventory.SpecialtyID = dr["SpecialtyID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SpecialtyID"]);
                            placementMatchingInventory.Specialty = dr["Specialty"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Specialty"]);

                            placementMatchingInventory.StudentTypes = dr["StudentTypes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StudentTypes"]);
                            placementMatchingInventory.StudentTypeIds = dr["StudentTypeIds"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StudentTypeIds"]);

                            placementMatchingInventory.Max = dr["Max"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["Max"]);

                            placementMatchingInventory.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            placementMatchingInventory.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);

                            placementMatchingInventory.Days = dr["Days"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Days"]);
                            //placementMatchingInventory.DayIds = dr["DayIds"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DayIds"]);

                            placementMatchingInventory.Shift = dr["Shift"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Shift"]);

                            placementMatchingInventory.StatusID = dr["StatusID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["StatusID"]);
                            placementMatchingInventory.Status = dr["Status"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Status"]);
                            placementMatchingInventory.StatusCode = dr["StatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StatusCode"]);

                            placementMatchingInventory.IsArchived = dr["IsArchived"] == DBNull.Value ? false : (Convert.ToBoolean(dr["IsArchived"]));
                            placementMatchingInventory.IsPreceptionShip = dr["IsPreceptionShip"] == DBNull.Value ? false : (Convert.ToBoolean(dr["IsPreceptionShip"]));

                            placementMatchingInventoryList.Add(placementMatchingInventory);
                        }
                    }
                }
                return placementMatchingInventoryList;
            }
        }

        Boolean IPlacementMatchingSetupRepository.SaveRequestDetails(RequestDetailContract requestDetail, Int32 currentUserId, List<CustomAttribteContract> lstCustomAttribute)
        {
            ClinicalInventoryRequest request = new ClinicalInventoryRequest();
            String oldStatus = String.Empty;
            Int32 statusID = SharedDataDBContext.lkpRequestStatus.Where(cond => cond.RS_Code == requestDetail.StatusCode).Select(a => a.RS_ID).FirstOrDefault();
            request = SharedDataDBContext.ClinicalInventoryRequests.Where(cond => cond.CIR_ID == requestDetail.RequestID).FirstOrDefault();
            if (request.IsNullOrEmpty())//insert
            {
                request = new ClinicalInventoryRequest();
                request.CIR_TenantID = requestDetail.SelectedTenantId;
                request.CIR_ClinicalInventoryID = requestDetail.OpportunityID;
                request.CIR_CourseCode = requestDetail.Course;
                request.CIR_NumberOfStudents = requestDetail.NumberOfStudents;
                request.CIR_StartDate = requestDetail.StartDate.Value;
                request.CIR_EndDate = requestDetail.EndDate.Value;
                request.CIR_Note = requestDetail.Notes;
                request.CIR_StatusID = statusID;
                request.CIR_ShiftString = requestDetail.Shift;
                request.CIR_DayString = requestDetail.Days;
                request.CIR_CreatedBy = currentUserId;
                request.CIR_CreatedOn = DateTime.Now;
                request.CIR_StatusChangedByID = currentUserId;
                request.CIR_StatusChangedOn = DateTime.Now;
                request.CIR_IsDeleted = false;

                SharedDataDBContext.ClinicalInventoryRequests.AddObject(request);

                if (requestDetail.ShiftID > AppConsts.NONE)
                {
                    ClinicalInventoryRequestShift requestShift = new ClinicalInventoryRequestShift();
                    requestShift.CIRS_ClinicalInventoryShiftID = requestDetail.ShiftID;
                    requestShift.CIRS_CreatedBy = currentUserId;
                    requestShift.CIRS_CreatedOn = DateTime.Now;
                    requestShift.CIRS_IsDeleted = false;

                    request.ClinicalInventoryRequestShifts.Add(requestShift);
                    //SharedDataDBContext.AddToClinicalInventoryRequestShifts(requestShift);


                    if (!requestDetail.DayIds.IsNullOrEmpty())
                    {
                        foreach (var dayID in requestDetail.DayIds.Split(','))
                        {
                            Int32 dayId = Convert.ToInt32(dayID);
                            ClinicalInventoryRequestDay requestDay = new ClinicalInventoryRequestDay();
                            requestDay.CIRD_ClinicalInventoryShiftDayID = SharedDataDBContext.ClinicalInventoryShiftDays.Where(cond => !cond.CISD_IsDeleted && cond.CISD_ClinicalInventoryShiftID == requestDetail.ShiftID && cond.CISD_DayID == dayId).FirstOrDefault().CISD_ID;
                            requestDay.CIRD_CreatedBy = currentUserId;
                            requestDay.CIRD_CreatedOn = DateTime.Now;
                            requestDay.CIRD_IsDeleted = false;
                            requestShift.ClinicalInventoryRequestDays.Add(requestDay);
                            SharedDataDBContext.ClinicalInventoryRequestDays.AddObject(requestDay);
                        }
                    }
                }

                List<PropertyInfo> requestDetailAuditProperties = typeof(RequestDetailContract).GetProperties().Where(p => p.GetCustomAttributes(typeof(Auditable), true).Any()).ToList();
                if (SharedDataDBContext.Connection.State == ConnectionState.Closed)
                    SharedDataDBContext.Connection.Open();
                using (var t = SharedDataDBContext.Connection.BeginTransaction())
                {
                    try
                    {
                        if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                        {
                            var data = GetProperties(requestDetail);
                            foreach (var a in data.Where(a => requestDetailAuditProperties.Select(b => b.Name).ToList().Contains(a.Key)))
                            {
                                PlacementRequestAuditLog auditLog = new PlacementRequestAuditLog();
                                if (!a.Value.IsNullOrEmpty())
                                {
                                    if (a.Key == "StatusID")
                                    {
                                        auditLog.PRAL_NewValue = request.lkpRequestStatu.RS_Name;
                                        auditLog.PRAL_ColumnName = "Status";
                                    }
                                    else
                                    {
                                        auditLog.PRAL_ColumnName = a.Key;
                                        auditLog.PRAL_NewValue = a.Value;
                                    }
                                    auditLog.PRAL_RequestID = request.CIR_ID;
                                    auditLog.PRAL_CreatedBy = currentUserId;
                                    auditLog.PRAL_CreatedOn = DateTime.Now;
                                    SharedDataDBContext.AddToPlacementRequestAuditLogs(auditLog);
                                }
                            }
                            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                            {
                                if (SaveSharedCustomAttributesValue(currentUserId, request.CIR_ID, lstCustomAttribute, CustomAttributeValueRecordType.ClinicalInventoryRequest.GetStringValue()))
                                {
                                    t.Commit();
                                    return true;
                                }
                                else
                                {
                                    t.Rollback();
                                    return false;
                                }
                            }
                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        t.Rollback();
                        return false;
                    }
                }
            }
            else //Update
            {
                oldStatus = request.lkpRequestStatu.RS_Name;
                request.CIR_TenantID = requestDetail.SelectedTenantId;
                request.CIR_ClinicalInventoryID = requestDetail.OpportunityID;
                request.CIR_CourseCode = requestDetail.Course;
                request.CIR_NumberOfStudents = requestDetail.NumberOfStudents;
                request.CIR_StartDate = requestDetail.StartDate.Value;
                request.CIR_EndDate = requestDetail.EndDate.Value;
                request.CIR_Note = requestDetail.Notes;
                request.CIR_StatusID = statusID;
                request.CIR_ShiftString = requestDetail.Shift;
                request.CIR_DayString = requestDetail.Days;
                request.CIR_ModifiedBy = currentUserId;
                request.CIR_ModifiedOn = DateTime.Now;
                request.CIR_StatusChangedByID = currentUserId;
                request.CIR_StatusChangedOn = DateTime.Now;
                request.CIR_ShiftString = requestDetail.Shift;
                ClinicalInventoryRequestShift oldRequestShift = new ClinicalInventoryRequestShift();
                oldRequestShift = request.ClinicalInventoryRequestShifts.Where(cond => !cond.CIRS_IsDeleted && cond.CIRS_ClinicalInventoryRequestID == requestDetail.RequestID).FirstOrDefault();
                List<ClinicalInventoryRequestDay> lstOldReqDays = new List<ClinicalInventoryRequestDay>();
                lstOldReqDays = oldRequestShift.ClinicalInventoryRequestDays.Where(a => !a.CIRD_IsDeleted && a.CIRD_ClinicalInventoryRequestShiftID == oldRequestShift.CIRS_ID).ToList();
                if (requestDetail.ShiftID > AppConsts.NONE)
                {
                    ClinicalInventoryRequestShift newRequestShift = new ClinicalInventoryRequestShift();
                    if (oldRequestShift.CIRS_ClinicalInventoryShiftID != requestDetail.ShiftID)
                    {
                        newRequestShift.CIRS_ClinicalInventoryShiftID = requestDetail.ShiftID;
                        newRequestShift.CIRS_ClinicalInventoryRequestID = requestDetail.RequestID;
                        newRequestShift.CIRS_CreatedBy = currentUserId;
                        newRequestShift.CIRS_CreatedOn = DateTime.Now;
                        newRequestShift.CIRS_IsDeleted = false;
                        oldRequestShift.CIRS_IsDeleted = true;
                        oldRequestShift.CIRS_ModifiedBy = currentUserId;
                        oldRequestShift.CIRS_ModifiedOn = DateTime.Now;
                        SharedDataDBContext.AddToClinicalInventoryRequestShifts(newRequestShift);
                        foreach (ClinicalInventoryRequestDay day in lstOldReqDays)
                        {
                            day.CIRD_IsDeleted = true;
                            day.CIRD_ModifiedBy = currentUserId;
                            day.CIRD_ModifiedOn = DateTime.Now;
                        }
                        if (!requestDetail.DayIds.IsNullOrEmpty())
                        {
                            foreach (var dayID in requestDetail.DayIds.Split(','))
                            {
                                ClinicalInventoryRequestDay requestDay = new ClinicalInventoryRequestDay();
                                Int32 dayId = Convert.ToInt32(dayID);
                                requestDay.CIRD_ClinicalInventoryShiftDayID = SharedDataDBContext.ClinicalInventoryShiftDays.Where(cond => !cond.CISD_IsDeleted && cond.CISD_ClinicalInventoryShiftID == requestDetail.ShiftID && cond.CISD_DayID == dayId).FirstOrDefault().CISD_ID;
                                requestDay.CIRD_CreatedBy = currentUserId;
                                requestDay.CIRD_CreatedOn = DateTime.Now;
                                requestDay.CIRD_IsDeleted = false;
                                SharedDataDBContext.AddToClinicalInventoryRequestDays(requestDay);
                            }
                        }
                    }

                    else
                    {
                        List<Int32> newReqDayIds = new List<Int32>();
                        List<Int32> oldReqDayIds = new List<Int32>();
                        foreach (var dayID in requestDetail.DayIds.Split(','))
                        {
                            ClinicalInventoryRequestDay requestDay = new ClinicalInventoryRequestDay();
                            Int32 dayId = Convert.ToInt32(dayID);


                            Int32 clinicalInventoryShiftDayId = SharedDataDBContext.ClinicalInventoryShiftDays.Where(cond => !cond.CISD_IsDeleted && cond.CISD_ClinicalInventoryShiftID == requestDetail.ShiftID && cond.CISD_DayID == dayId).FirstOrDefault().CISD_ID;
                            newReqDayIds.Add(clinicalInventoryShiftDayId);
                            if (!lstOldReqDays.Select(a => a.CIRD_ClinicalInventoryShiftDayID).Contains(clinicalInventoryShiftDayId)) //new day ids added
                            {
                                requestDay.CIRD_ClinicalInventoryShiftDayID = clinicalInventoryShiftDayId;
                                requestDay.CIRD_ClinicalInventoryRequestShiftID = oldRequestShift.CIRS_ID;
                                requestDay.CIRD_CreatedBy = currentUserId;
                                requestDay.CIRD_CreatedOn = DateTime.Now;
                                requestDay.CIRD_IsDeleted = false;
                                SharedDataDBContext.AddToClinicalInventoryRequestDays(requestDay);
                            }
                        }
                        oldReqDayIds = lstOldReqDays.Select(a => a.CIRD_ClinicalInventoryShiftDayID).Except(newReqDayIds).ToList();//old days ids to be deleted
                        if (oldReqDayIds.Count > AppConsts.NONE)
                        {
                            foreach (ClinicalInventoryRequestDay day in lstOldReqDays.Where(a => oldReqDayIds.Contains(a.CIRD_ClinicalInventoryShiftDayID)))
                            {
                                day.CIRD_IsDeleted = true;
                                day.CIRD_ModifiedBy = currentUserId;
                                day.CIRD_ModifiedOn = DateTime.Now;
                            }
                        }
                    }
                }
                var myObjectState = SharedDataDBContext.ObjectStateManager.GetObjectStateEntry(request);
                var modifiedProperties = myObjectState.GetModifiedProperties().ToList();
                foreach (var property in modifiedProperties)
                {
                    PlacementRequestAuditLog auditLog = new PlacementRequestAuditLog();
                    if (myObjectState.OriginalValues[property].ToString() != myObjectState.CurrentValues[property].ToString() && (property.ToString() != "CIR_StatusChangedOn" && property.ToString() != "CIR_ModifiedOn" && property.ToString() != "CIR_ModifiedBy"))
                    {
                        if (property.ToString() == "CIR_StatusID")
                        {
                            auditLog.PRAL_ColumnName = "Status";
                            auditLog.PRAL_OldValue = oldStatus;
                            auditLog.PRAL_NewValue = request.lkpRequestStatu.RS_Name;
                        }
                        else
                        {
                            auditLog.PRAL_ColumnName = property.Remove(0, 4).Replace("String", "");
                            auditLog.PRAL_OldValue = myObjectState.OriginalValues[property].ToString();
                            auditLog.PRAL_NewValue = myObjectState.CurrentValues[property].ToString();
                        }
                        auditLog.PRAL_RequestID = requestDetail.RequestID;
                        auditLog.PRAL_CreatedBy = currentUserId;
                        auditLog.PRAL_CreatedOn = DateTime.Now;
                        SharedDataDBContext.AddToPlacementRequestAuditLogs(auditLog);
                    }
                }


                if (SharedDataDBContext.SaveChanges() > AppConsts.NONE && SaveSharedCustomAttributesValue(currentUserId, request.CIR_ID, lstCustomAttribute, CustomAttributeValueRecordType.ClinicalInventoryRequest.GetStringValue()))
                {
                    return true;
                }
                return false;


            }
        }

        List<RequestDetailContract> IPlacementMatchingSetupRepository.GetPlacementRequests(PlacementSearchContract searchContract)
        {
            var lstPlacementRequest = new List<RequestDetailContract>();
            EntityConnection connection = SharedDataDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyLocationID", searchContract.LocationId),
                    new SqlParameter("@DepartmentID", searchContract.DepartmentId),
                    new SqlParameter("@StartDate", searchContract.StartDate),
                    new SqlParameter("@EndDate", searchContract.EndDate),
                    new SqlParameter("@SpecialtyID", searchContract.SpecialtyId),
                    new SqlParameter("@StudentTypeIds", searchContract.StudentTypeIds),
                    new SqlParameter("@Max", searchContract.Max),
                    new SqlParameter("@DayIds", searchContract.Days),
                    new SqlParameter("@Shift", searchContract.Shift),
                    new SqlParameter("@TenantID", searchContract.TenantId),
                    new SqlParameter("@RequestStatusIds", searchContract.StatusIds),
                    new SqlParameter("@StatusCode", searchContract.StatusCode),
                    new SqlParameter("@AgencyHierarchyID", searchContract.AgencyHierarchyID),
                    new SqlParameter("@CustomAtrributesData", searchContract.SharedCustomAttributes)


                };
                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequestSearch", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lstPlacementRequest.Add(new RequestDetailContract
                            {
                                OpportunityID = Convert.ToInt32(dr["OpportunityID"]),
                                Days = Convert.ToString(dr["Days"]),
                                StudentTypes = Convert.ToString(dr["StudentTypes"]),
                                Location = Convert.ToString(dr["Location"]),
                                Shift = Convert.ToString(dr["Shift"]),
                                StartDate = Convert.ToDateTime(dr["StartDate"]),
                                EndDate = Convert.ToDateTime(dr["EndDate"]),
                                Department = Convert.ToString(dr["Department"]),
                                Specialty = Convert.ToString(dr["Specialty"]),
                                StatusID = Convert.ToInt32(dr["RequestedStatusID"]),
                                RequestID = Convert.ToInt32(dr["RequestID"]),
                                StatusCode = Convert.ToString(dr["StatusCode"]),
                                IsRequestPublished = RequestStatusCodes.Draft.GetStringValue() != Convert.ToString(dr["StatusCode"]),
                                RequestStatus = Convert.ToString(dr["RequestStatus"]),
                                AgencyName = Convert.ToString(dr["Agency"]),
                                Max = Convert.ToInt32(dr["Max"]),
                                InstitutionName = Convert.ToString(dr["TenantName"]),
                                Unit = Convert.ToString(dr["Unit"]),
                                ContainsFloatArea = Convert.ToString(dr["ContainsFloatArea"]),
                                Course = Convert.ToString(dr["Course"]),
                                OpportunityStartDate = Convert.ToDateTime(dr["OpportunityStartDate"]),
                                OpportunityEndDate = Convert.ToDateTime(dr["OpportunityEndDate"])


                            });
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstPlacementRequest.OrderBy(a => a.RequestID).ToList();
        }

        RequestDetailContract IPlacementMatchingSetupRepository.GetRequestDetail(Int32 requestID)
        {

            ClinicalInventoryRequest request = new ClinicalInventoryRequest();
            RequestDetailContract requestDetail = new RequestDetailContract();

            if (requestID > AppConsts.NONE)
                request = SharedDataDBContext.ClinicalInventoryRequests.Where(cond => cond.CIR_ID == requestID).FirstOrDefault();


            if (!request.IsNullOrEmpty())
            {
                //ClinicalInventory clinicalInventory =SharedDataDBContext.ClinicalInventories.Where(cond=>!cond.CI_IsDeleted && cond.CI_ID == request.CIR_ClinicalInventoryID).FirstOrDefault();
                ClinicalInventoryRequestShift clinicalInventoryRequestShift = SharedDataDBContext.ClinicalInventoryRequestShifts.Where(cond => !cond.CIRS_IsDeleted && cond.CIRS_ClinicalInventoryRequestID == request.CIR_ID).FirstOrDefault();
                List<ClinicalInventoryRequestDay> lstClinicalReqDays = SharedDataDBContext.ClinicalInventoryRequestDays.Where(cond => !cond.CIRD_IsDeleted && cond.CIRD_ClinicalInventoryRequestShiftID == clinicalInventoryRequestShift.CIRS_ID).ToList();
                List<Int32> lstClinicalInventoryDayIds = lstClinicalReqDays.Select(Sel => Sel.CIRD_ClinicalInventoryShiftDayID).ToList();
                List<Int32> lstDayIds = SharedDataDBContext.ClinicalInventoryShiftDays.Where(cond => !cond.CISD_IsDeleted && lstClinicalInventoryDayIds.Contains(cond.CISD_ID)).Select(Sel => Sel.CISD_DayID).ToList();

                requestDetail.RequestID = request.CIR_ID;
                requestDetail.OpportunityID = request.CIR_ClinicalInventoryID;
                requestDetail.Notes = request.CIR_Note;
                requestDetail.Shift = request.CIR_ShiftString;
                requestDetail.Course = request.CIR_CourseCode;
                requestDetail.StartDate = request.CIR_StartDate;//request.ClinicalInventory.CI_StartDate;
                requestDetail.EndDate = request.CIR_EndDate;//request.ClinicalInventory.CI_EndDate;
                requestDetail.NumberOfStudents = request.CIR_NumberOfStudents;
                requestDetail.Days = request.CIR_DayString;
                requestDetail.ShiftID = request.ClinicalInventoryRequestShifts.Where(a => !a.CIRS_IsDeleted).Select(a => a.CIRS_ClinicalInventoryShiftID).FirstOrDefault();
                requestDetail.lstDays = lstDayIds;
                requestDetail.StatusID = request.CIR_StatusID;
                requestDetail.StatusCode = request.lkpRequestStatu.RS_Code;
                requestDetail.RequestStatus = request.lkpRequestStatu.RS_Name;
                requestDetail.IsRequestPublished = RequestStatusCodes.Draft.GetStringValue() != request.lkpRequestStatu.RS_Code;
                requestDetail.InstitutionID = request.CIR_TenantID;
                requestDetail.InstitutionName = SecurityContext.Tenants.Where(cond => !cond.IsDeleted && cond.TenantID == request.CIR_TenantID && cond.IsActive).FirstOrDefault().TenantName;
            }
            return requestDetail;
        }

        List<ShiftDetails> IPlacementMatchingSetupRepository.GetShiftsForOpportunity(Int32 opportunityId)
        {

            //List<Entity.SharedDataEntity.lkpWeekDay> weekDays = SharedDataDBContext.lkpWeekDays.Where(cond => !cond.WD_IsDeleted).ToList();
            //List<Int32> lstWeekDayId = weekDays.Select(Sel => Sel.WD_ID).ToList();
            List<ShiftDetails> lstShiftDetails = new List<ShiftDetails>();
            lstShiftDetails = SharedDataDBContext.ClinicalInventoryShifts.Where(cond => !cond.CIS_IsDeleted && cond.CIS_ClinicalInventoryID == opportunityId).Select(x =>
                               new ShiftDetails
                               {
                                   ClinicalInventoryID = x.CIS_ClinicalInventoryID,
                                   ClinicalInventoryShiftID = x.CIS_ID,
                                   Shift = x.CIS_Shift,
                                   ShiftFrom = x.CIS_ShiftFrom,
                                   ShiftTo = x.CIS_ShiftTo,
                                   NumberOfStudents = x.CIS_NoOfStudents,
                                   lstDaysId = x.ClinicalInventoryShiftDays.Where(cond => !cond.CISD_IsDeleted).Select(sel => sel.CISD_DayID).ToList(),
                               }).ToList();
            return lstShiftDetails;
        }

        List<ShiftDetails> IPlacementMatchingSetupRepository.GetAllShifts()
        {
            List<ShiftDetails> lstShiftDetails = new List<ShiftDetails>();
            lstShiftDetails = SharedDataDBContext.ClinicalInventoryShifts.Where(cond => !cond.CIS_IsDeleted).Select(x =>
                               new ShiftDetails
                               {
                                   ClinicalInventoryID = x.CIS_ClinicalInventoryID,
                                   ClinicalInventoryShiftID = x.CIS_ID,
                                   Shift = x.CIS_Shift,
                                   NumberOfStudents = x.CIS_NoOfStudents,
                                   lstDaysId = x.ClinicalInventoryShiftDays.Where(cond => !cond.CISD_IsDeleted).Select(sel => sel.CISD_DayID).ToList(),
                               }).ToList();
            return lstShiftDetails;
        }

        Boolean IPlacementMatchingSetupRepository.ChangeRequestStatus(Int32 currentLoggedInUserID, Int32 requestID, string statusCode)
        {
            Int32 statusID = SharedDataDBContext.lkpRequestStatus.Where(a => a.RS_Code == statusCode).Select(a => a.RS_ID).FirstOrDefault();
            if (statusID > AppConsts.NONE)
            {
                ClinicalInventoryRequest request = new ClinicalInventoryRequest();
                PlacementRequestAuditLog auditLog = new PlacementRequestAuditLog();
                auditLog.PRAL_ColumnName = "Status";
                auditLog.PRAL_RequestID = requestID;
                request = SharedDataDBContext.ClinicalInventoryRequests.Where(a => a.CIR_ID == requestID).FirstOrDefault();

                if (!request.IsNullOrEmpty())
                {
                    auditLog.PRAL_OldValue = request.lkpRequestStatu.RS_Name;
                    auditLog.PRAL_CreatedBy = currentLoggedInUserID;
                    auditLog.PRAL_CreatedOn = DateTime.Now;
                    if (statusCode == RequestStatusCodes.Approved.GetStringValue())
                    {
                        request.CIR_ApprovedByID = currentLoggedInUserID;
                        request.CIR_ApprovedOn = DateTime.Now;
                    }
                    request.CIR_StatusID = statusID;
                    request.CIR_ModifiedBy = currentLoggedInUserID;
                    request.CIR_ModifiedOn = DateTime.Now;
                    SharedDataDBContext.AddToPlacementRequestAuditLogs(auditLog);
                    if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                        return true;
                    return false;
                }
            }
            return false;
        }

        Boolean IPlacementMatchingSetupRepository.SaveShiftDetails(Int32 currentLoggedInUserId, ShiftDetails shiftDetail)
        {
            if (!shiftDetail.IsNullOrEmpty())
            {
                ClinicalInventoryShift clinicalInventoryShift = new ClinicalInventoryShift();
                List<ClinicalInventoryShiftDay> lstClinicalInventoryShiftDay = new List<ClinicalInventoryShiftDay>();
                //Update
                if (!shiftDetail.ClinicalInventoryShiftID.IsNullOrEmpty() && shiftDetail.ClinicalInventoryShiftID > AppConsts.NONE)
                {
                    clinicalInventoryShift = SharedDataDBContext.ClinicalInventoryShifts.Where(cond => !cond.CIS_IsDeleted && cond.CIS_ID == shiftDetail.ClinicalInventoryShiftID).FirstOrDefault();
                    clinicalInventoryShift.CIS_ClinicalInventoryID = shiftDetail.ClinicalInventoryID > AppConsts.NONE ? (int?)null : shiftDetail.ClinicalInventoryID;
                    clinicalInventoryShift.CIS_NoOfStudents = shiftDetail.NumberOfStudents;
                    clinicalInventoryShift.CIS_ShiftFrom = (TimeSpan)shiftDetail.ShiftFrom;
                    clinicalInventoryShift.CIS_ShiftTo = (TimeSpan)shiftDetail.ShiftTo;
                    clinicalInventoryShift.CIS_Shift = shiftDetail.Shift;
                    clinicalInventoryShift.CIS_ModifiedBy = currentLoggedInUserId;
                    clinicalInventoryShift.CIS_ModifiedOn = DateTime.Now;
                    lstClinicalInventoryShiftDay = clinicalInventoryShift.ClinicalInventoryShiftDays.Where(cond => !cond.CISD_IsDeleted).ToList();
                    List<Int32> lstShiftDayIds = lstClinicalInventoryShiftDay.Select(sel => sel.CISD_DayID).ToList();

                    foreach (Int32 dayId in shiftDetail.lstDaysId)
                    {
                        if (!lstShiftDayIds.Contains(dayId))
                        {
                            ClinicalInventoryShiftDay newClinicalInventoryShiftDay = new ClinicalInventoryShiftDay();
                            //Add new day Id.
                            newClinicalInventoryShiftDay.CISD_DayID = dayId;
                            newClinicalInventoryShiftDay.CISD_CreatedBy = currentLoggedInUserId;
                            newClinicalInventoryShiftDay.CISD_CreatedOn = DateTime.Now;
                            clinicalInventoryShift.ClinicalInventoryShiftDays.Add(newClinicalInventoryShiftDay);
                            //SharedDataDBContext.ClinicalInventoryShiftDays.AddObject(newClinicalInventoryShiftDay);
                        }
                    }

                    foreach (Int32 dayID in lstShiftDayIds)
                    {
                        if (!shiftDetail.lstDaysId.Contains(dayID))
                        {
                            //Remove
                            ClinicalInventoryShiftDay removeClinicalInventoryShiftDay = lstClinicalInventoryShiftDay.Where(cond => cond.CISD_DayID == dayID).FirstOrDefault();
                            removeClinicalInventoryShiftDay.CISD_IsDeleted = true;
                            removeClinicalInventoryShiftDay.CISD_ModifiedBy = currentLoggedInUserId;
                            removeClinicalInventoryShiftDay.CISD_ModifiedOn = DateTime.Now;
                        }
                    }
                }
                else
                {
                    //Add
                    clinicalInventoryShift.CIS_ClinicalInventoryID = shiftDetail.ClinicalInventoryID > AppConsts.NONE ? shiftDetail.ClinicalInventoryID : (int?)null;
                    clinicalInventoryShift.CIS_NoOfStudents = shiftDetail.NumberOfStudents;
                    clinicalInventoryShift.CIS_ShiftFrom = (TimeSpan)shiftDetail.ShiftFrom;
                    clinicalInventoryShift.CIS_ShiftTo = (TimeSpan)shiftDetail.ShiftTo;
                    clinicalInventoryShift.CIS_Shift = shiftDetail.Shift;
                    clinicalInventoryShift.CIS_CreatedBy = currentLoggedInUserId;
                    clinicalInventoryShift.CIS_CreatedOn = DateTime.Now;

                    SharedDataDBContext.ClinicalInventoryShifts.AddObject(clinicalInventoryShift);

                    foreach (Int32 dayId in shiftDetail.lstDaysId)
                    {
                        ClinicalInventoryShiftDay clinicalInventoryShiftDay = new ClinicalInventoryShiftDay();
                        //clinicalInventoryShiftDay.CISD_ClinicalInventoryShiftID = 
                        clinicalInventoryShiftDay.CISD_DayID = dayId;
                        clinicalInventoryShiftDay.CISD_CreatedBy = currentLoggedInUserId;
                        clinicalInventoryShiftDay.CISD_CreatedOn = DateTime.Now;
                        clinicalInventoryShift.ClinicalInventoryShiftDays.Add(clinicalInventoryShiftDay);
                        //SharedDataDBContext.ClinicalInventoryShiftDays.AddObject(clinicalInventoryShiftDay);
                    }
                }
            }

            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        public Boolean DeleteShiftDetail(Int32 currentLoggedInUser, ShiftDetails shiftDetail)
        {
            if (!shiftDetail.IsNullOrEmpty() && shiftDetail.ClinicalInventoryShiftID.IsNullOrEmpty() && shiftDetail.ClinicalInventoryShiftID > AppConsts.NONE)
            {
                ClinicalInventoryShift clinicalInventoryShift = new ClinicalInventoryShift();
                clinicalInventoryShift = SharedDataDBContext.ClinicalInventoryShifts.Where(cond => !cond.CIS_IsDeleted && cond.CIS_ID == shiftDetail.ClinicalInventoryShiftID).FirstOrDefault();

                clinicalInventoryShift.CIS_IsDeleted = true;
                clinicalInventoryShift.CIS_ModifiedBy = currentLoggedInUser;
                clinicalInventoryShift.CIS_ModifiedOn = DateTime.Now;

                List<ClinicalInventoryShiftDay> lstClinicalInventoryShiftDay = new List<ClinicalInventoryShiftDay>();
                lstClinicalInventoryShiftDay = SharedDataDBContext.ClinicalInventoryShiftDays.Where(cond => !cond.CISD_IsDeleted && cond.CISD_ClinicalInventoryShiftID == shiftDetail.ClinicalInventoryShiftID).ToList();
                foreach (ClinicalInventoryShiftDay clinicalInventoryShiftDay in lstClinicalInventoryShiftDay)
                {
                    clinicalInventoryShiftDay.CISD_IsDeleted = true;
                    clinicalInventoryShiftDay.CISD_ModifiedBy = currentLoggedInUser;
                    clinicalInventoryShiftDay.CISD_ModifiedOn = DateTime.Now;
                }
            }

            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        List<PlacementRequestAuditContract> IPlacementMatchingSetupRepository.GetPlacementRequestAuditLogs(Int32 requestID)
        {
            List<PlacementRequestAuditLog> lstAudits = new List<PlacementRequestAuditLog>();
            List<PlacementRequestAuditContract> lstAuditContract = new List<PlacementRequestAuditContract>();
            lstAudits = SharedDataDBContext.PlacementRequestAuditLogs.Where(a => a.PRAL_RequestID == requestID && !a.PRAL_IsDeleted).OrderByDescending(a => a.PRAL_CreatedOn).ToList();
            foreach (var audit in lstAudits)
            {
                PlacementRequestAuditContract requestAudit = new PlacementRequestAuditContract();
                requestAudit.RequestId = audit.PRAL_RequestID;
                requestAudit.OldValue = audit.PRAL_OldValue;
                requestAudit.NewValue = audit.PRAL_NewValue;
                requestAudit.ColumnName = audit.PRAL_ColumnName;
                requestAudit.CreatedByID = audit.PRAL_CreatedBy;
                requestAudit.CreatedOn = audit.PRAL_CreatedOn;
                lstAuditContract.Add(requestAudit);
            }
            return lstAuditContract;
        }

        private static List<KeyValuePair<string, string>> GetProperties(object item) //where T : class
        {
            var result = new List<KeyValuePair<string, string>>();
            if (item != null)
            {
                var type = item.GetType();
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var pi in properties)
                {
                    var selfValue = type.GetProperty(pi.Name).GetValue(item, null);
                    if (selfValue != null)
                    {
                        result.Add(new KeyValuePair<string, string>(pi.Name, selfValue.ToString()));
                    }
                    else
                    {
                        result.Add(new KeyValuePair<string, string>(pi.Name, null));
                    }
                }
            }
            return result;
        }

        List<RequestStatusContract> IPlacementMatchingSetupRepository.GetRequestStatuses()
        {
            List<RequestStatusContract> lstrequestSearchContract = new List<RequestStatusContract>();
            lstrequestSearchContract = SharedDataDBContext.lkpRequestStatus.Where(cond => !cond.RS_IsDeleted).Select(x =>
               new RequestStatusContract
               {
                   Code = x.RS_Code,
                   Name = x.RS_Name,
                   StatusID = x.RS_ID
               }).ToList();
            return lstrequestSearchContract;
        }

        List<AgencyHierarchyContract> IPlacementMatchingSetupRepository.GetAgencyHierarchyRootNodes(Int32 TenantId)
        {
            List<AgencyHierarchyContract> lstAgencyHierarchyRootNodes = new List<AgencyHierarchyContract>();
            if (TenantId > AppConsts.NONE)
            {
                lstAgencyHierarchyRootNodes = SharedDataDBContext.AgencyHierarchyTenantMappings.Include("AgencyHierarchies").Where(cond => !cond.AHTM_IsDeleted && cond.AHTM_TenantID == TenantId
                                                                && !cond.AgencyHierarchy.AH_IsDeleted && cond.AgencyHierarchy.AH_ParentID == null)
                                                                .Select(x => new AgencyHierarchyContract
                                                                {
                                                                    AgencyID = x.AgencyHierarchy.AH_ID,
                                                                    AgencyName = x.AgencyHierarchy.AH_Label
                                                                }).ToList();

            }
            else
            {
                lstAgencyHierarchyRootNodes = SharedDataDBContext.AgencyHierarchies.Where(cond => !cond.AH_IsDeleted && cond.AH_ParentID == null)
                                                .Select(x => new AgencyHierarchyContract
                                                {
                                                    AgencyID = x.AH_ID,
                                                    AgencyName = x.AH_Label
                                                }).ToList();
            }

            return lstAgencyHierarchyRootNodes;
        }
        List<RequestStatusContract> IPlacementMatchingSetupRepository.GetRequestStatusBarCount(Int32 AgencyID)
        {
            var lstRequestStatusCounts = new List<RequestStatusContract>();
            EntityConnection connection = SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyID",AgencyID ),
                };
                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetInventoryRequestCount", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lstRequestStatusCounts.Add(new RequestStatusContract
                            {
                                Code = Convert.ToString(dr["Code"]),
                                Count = Convert.ToInt32(dr["StatusCount"]),
                                Name = Convert.ToString(dr["Name"]),
                                StatusID = Convert.ToInt32(dr["ID"]),
                            });
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstRequestStatusCounts;
        }

        List<RequestDetailContract> IPlacementMatchingSetupRepository.GetAgencyPlacementDashboardRequests(Int32 agencyHierarchyRootNodeID)
        {
            List<RequestDetailContract> lstRequests = new List<RequestDetailContract>();
            EntityConnection connection = SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            //new SqlParameter("@RequestStatusID",selectedStatusID),
                            new SqlParameter("@AgencyHierarchyRootNodeID", agencyHierarchyRootNodeID),
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyPlacementDashboardRequests", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequestDetailContract requestContract = new RequestDetailContract();

                            requestContract.OpportunityID = dr["OpportunityID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OpportunityID"]);
                            requestContract.RequestID = dr["RequestID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequestID"]);
                            requestContract.InstitutionID = dr["InstitutionID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["InstitutionID"]);
                            requestContract.InstitutionName = dr["InstitutionName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["InstitutionName"]);
                            requestContract.AgencyID = dr["AgencyHierarchyID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AgencyHierarchyID"]);
                            requestContract.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            requestContract.AgencyLocationID = dr["AgencyLocationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AgencyLocationID"]);
                            requestContract.Location = dr["Location"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Location"]);
                            requestContract.DepartmentID = dr["DepartmentID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["DepartmentID"]);
                            requestContract.Department = dr["Department"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Department"]);
                            requestContract.SpecialtyID = dr["SpecialtyID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SpecialtyID"]);
                            requestContract.Specialty = dr["Specialty"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Specialty"]);
                            requestContract.StudentTypes = dr["StudentTypes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StudentTypes"]);
                            requestContract.StudentTypeIds = dr["StudentTypeIds"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StudentTypeIds"]);
                            requestContract.Max = dr["Max"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["Max"]);
                            requestContract.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            requestContract.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                            requestContract.Days = dr["Days"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Days"]);
                            //    requestContract.DayIds = dr["DayIds"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DayIds"]);
                            requestContract.Shift = dr["Shift"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Shift"]);
                            requestContract.StatusID = dr["StatusID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["StatusID"]);
                            requestContract.RequestStatus = dr["Status"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Status"]);
                            requestContract.StatusCode = dr["StatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StatusCode"]);

                            requestContract.LastUpdateByID = dr["LastUpdateByID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["LastUpdateByID"]);
                            requestContract.LastUpdateByName = dr["LastUpdateBy"] == DBNull.Value ? String.Empty : Convert.ToString(dr["LastUpdateBy"]);
                            requestContract.LastUpdateDate = dr["LastUpdatedOn"] == DBNull.Value ? String.Empty : Convert.ToString(dr["LastUpdatedOn"]);
                            requestContract.RequestSubmittedDate = dr["RequestSubmittedDate"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequestSubmittedDate"]);

                            lstRequests.Add(requestContract);
                        }
                    }
                }
                return lstRequests;
            }
        }

        List<InstitutionRequestPieChartContract> IPlacementMatchingSetupRepository.GetIntitutionsRequestsApproved(Int32 agencyHierarchyRootNodeId, DateTime? fromDate, DateTime? toDate)
        {
            List<InstitutionRequestPieChartContract> lstInstitutionsApprovedRequests = new List<InstitutionRequestPieChartContract>();
            EntityConnection connection = SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@AgencyHierarchyRootNodeID",agencyHierarchyRootNodeId),
                            new SqlParameter("@FromDate", fromDate),
                            new SqlParameter("@ToDate", toDate)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetInstitutionPlacementApprovedRequest", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            InstitutionRequestPieChartContract institutionApprovedRequest = new InstitutionRequestPieChartContract();

                            institutionApprovedRequest.TenantID = dr["TenantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["TenantID"]);
                            institutionApprovedRequest.TenantName = dr["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TenantName"]);
                            institutionApprovedRequest.RecordsPercentage = dr["RecordsPercentage"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RecordsPercentage"]);
                            institutionApprovedRequest.Color = dr["Color"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Color"]);

                            lstInstitutionsApprovedRequests.Add(institutionApprovedRequest);
                        }
                    }
                }
                return lstInstitutionsApprovedRequests;
            }
        }

        List<SharedCustomAttributesContract> IPlacementMatchingSetupRepository.GetSharedCustomAttributes()
        {
            List<SharedCustomAttributesContract> lstSharedCustomAttributes = new List<SharedCustomAttributesContract>();
            lstSharedCustomAttributes = SharedDataDBContext.SharedCustomAttributes.Where(cond => !cond.SCA_IsDeleted)
                                        .Select(x => new SharedCustomAttributesContract
                                        {
                                            SharedCustomAttributeID = x.SCA_ID,
                                            AttributeName = x.SCA_AttributeName,
                                            AttributeLabel = x.SCA_AttributeLabel,
                                            AttributeDataTypeID = x.lkpCustomAttributeDataType.CustomAttributeDataTypeID,
                                            AttributeDataTypeCode = x.lkpCustomAttributeDataType.Code,
                                            AttributeDataType = x.lkpCustomAttributeDataType.Name,
                                            AttributeUseTypeID = x.lkpSharedCustomAttributeUseType.SCAUT_ID,
                                            AttributeUseTypeCode = x.lkpSharedCustomAttributeUseType.SCAUT_Code,
                                            AttributeUseType = x.lkpSharedCustomAttributeUseType.SCAUT_Name,
                                            IsActive = x.SCA_IsActive,
                                            IsRequired = x.SCA_IsRequired,
                                            StringLength = x.SCA_StringLength,
                                            RegularExpression = x.SCA_RegularExpression,
                                            RegExpErrorMsg = x.SCA_RegExpErrorMsg,
                                            AgencyHierarchyRootNodeID = x.SharedCustomAttributeMappings.Where(cond => !cond.SCAM_IsDeleted).FirstOrDefault().SCAM_RecordID,
                                            SharedCustomAttributeMappingID = x.SharedCustomAttributeMappings.Where(cond => !cond.SCAM_IsDeleted).FirstOrDefault().SCAM_ID
                                        }).ToList();
            return lstSharedCustomAttributes;
        }

        Boolean IPlacementMatchingSetupRepository.SaveSharedCustomAttribute(Int32 currentLoggedInUserID, SharedCustomAttributesContract sharedCustomAttributes)
        {
            if (!sharedCustomAttributes.IsNullOrEmpty())
            {
                SharedCustomAttribute sharedCustomAttributeToSave = new SharedCustomAttribute();
                SharedCustomAttributeMapping sharedCustomAttributeMapping = new SharedCustomAttributeMapping();
                //update
                if (!sharedCustomAttributes.SharedCustomAttributeID.IsNullOrEmpty() && sharedCustomAttributes.SharedCustomAttributeID > AppConsts.NONE
                            && !sharedCustomAttributes.SharedCustomAttributeMappingID.IsNullOrEmpty() && sharedCustomAttributes.SharedCustomAttributeMappingID > AppConsts.NONE)
                {
                    sharedCustomAttributeToSave = SharedDataDBContext.SharedCustomAttributes.Where(cond => !cond.SCA_IsDeleted && cond.SCA_ID == sharedCustomAttributes.SharedCustomAttributeID).FirstOrDefault();
                    sharedCustomAttributeMapping = SharedDataDBContext.SharedCustomAttributeMappings.Where(cond => !cond.SCAM_IsDeleted
                                                    && cond.SCAM_SharedCustomAttributeID == sharedCustomAttributes.SharedCustomAttributeID && cond.SCAM_ID == sharedCustomAttributes.SharedCustomAttributeMappingID)
                                                    .FirstOrDefault();
                    //update shared custom attribute
                    sharedCustomAttributeToSave.SCA_AttributeName = sharedCustomAttributes.AttributeName;
                    sharedCustomAttributeToSave.SCA_AttributeLabel = sharedCustomAttributes.AttributeLabel;
                    sharedCustomAttributeToSave.SCA_AttributeDataTypeID = sharedCustomAttributes.AttributeDataTypeID;
                    sharedCustomAttributeToSave.SCA_AttributeUseTypeID = sharedCustomAttributes.AttributeUseTypeID;
                    sharedCustomAttributeToSave.SCA_StringLength = sharedCustomAttributes.StringLength;
                    sharedCustomAttributeToSave.SCA_RegularExpression = sharedCustomAttributes.RegularExpression;
                    sharedCustomAttributeToSave.SCA_RegExpErrorMsg = sharedCustomAttributes.RegExpErrorMsg;
                    sharedCustomAttributeToSave.SCA_RelatedCustomAttributeID = sharedCustomAttributes.RelatedCustomAttributeID;
                    sharedCustomAttributeToSave.SCA_IsActive = sharedCustomAttributes.IsActive;
                    sharedCustomAttributeToSave.SCA_IsRequired = sharedCustomAttributes.IsRequired;
                    sharedCustomAttributeToSave.SCA_ModifiedOn = DateTime.Now;
                    sharedCustomAttributeToSave.SAC_ModifiedBy = currentLoggedInUserID;

                    //update shared custom attribute Mapping
                    sharedCustomAttributeMapping.SCAM_RecordID = sharedCustomAttributes.AgencyHierarchyRootNodeID;
                    sharedCustomAttributeMapping.SCAM_SharedCustomAttributeID = sharedCustomAttributes.SharedCustomAttributeID;
                    sharedCustomAttributeMapping.SCAM_IsRequired = sharedCustomAttributes.IsRequired;
                    sharedCustomAttributeMapping.SCAM_ModifiedBy = currentLoggedInUserID;
                    sharedCustomAttributeMapping.SCAM_ModifiedOn = DateTime.Now;
                }
                //Insert
                else
                {
                    //Add Shared custom Attribute
                    sharedCustomAttributeToSave.SCA_AttributeName = sharedCustomAttributes.AttributeName;
                    sharedCustomAttributeToSave.SCA_AttributeLabel = sharedCustomAttributes.AttributeLabel;
                    sharedCustomAttributeToSave.SCA_AttributeDataTypeID = sharedCustomAttributes.AttributeDataTypeID;
                    sharedCustomAttributeToSave.SCA_AttributeUseTypeID = sharedCustomAttributes.AttributeUseTypeID;
                    sharedCustomAttributeToSave.SCA_StringLength = sharedCustomAttributes.StringLength;
                    sharedCustomAttributeToSave.SCA_RegularExpression = sharedCustomAttributes.RegularExpression;
                    sharedCustomAttributeToSave.SCA_RegExpErrorMsg = sharedCustomAttributes.RegExpErrorMsg;
                    sharedCustomAttributeToSave.SCA_RelatedCustomAttributeID = sharedCustomAttributes.RelatedCustomAttributeID;
                    sharedCustomAttributeToSave.SCA_IsActive = sharedCustomAttributes.IsActive;
                    sharedCustomAttributeToSave.SCA_IsRequired = sharedCustomAttributes.IsRequired;
                    sharedCustomAttributeToSave.SCA_CreatedOn = DateTime.Now;
                    sharedCustomAttributeToSave.SCA_CreatedBy = currentLoggedInUserID;

                    SharedDataDBContext.SharedCustomAttributes.AddObject(sharedCustomAttributeToSave);

                    //Add shared Custom Attribute Mapping
                    sharedCustomAttributeMapping.SCAM_IsRequired = sharedCustomAttributes.IsRequired;
                    sharedCustomAttributeMapping.SCAM_RecordID = sharedCustomAttributes.AgencyHierarchyRootNodeID;
                    sharedCustomAttributeMapping.SCAM_CreatedBy = currentLoggedInUserID;
                    sharedCustomAttributeMapping.SCAM_CreatedOn = DateTime.Now;

                    sharedCustomAttributeToSave.SharedCustomAttributeMappings.Add(sharedCustomAttributeMapping);
                }
            }

            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IPlacementMatchingSetupRepository.DeleteSharedCustomAttribute(Int32 currentLoggedInUserId, Int32 selectSharedCustomAttributeId, Int32 selectSharedCustomAttributeMappingID)
        {
            if (!selectSharedCustomAttributeId.IsNullOrEmpty() && selectSharedCustomAttributeId > AppConsts.NONE
                    && !selectSharedCustomAttributeMappingID.IsNullOrEmpty() && selectSharedCustomAttributeMappingID > AppConsts.NONE)
            {
                SharedCustomAttribute sharedCustomAttribute = new SharedCustomAttribute();
                sharedCustomAttribute = SharedDataDBContext.SharedCustomAttributes.Where(cond => !cond.SCA_IsDeleted && cond.SCA_ID == selectSharedCustomAttributeId).FirstOrDefault();

                SharedCustomAttributeMapping sharedCustomAttributeMapping = new SharedCustomAttributeMapping();
                sharedCustomAttributeMapping = SharedDataDBContext.SharedCustomAttributeMappings.Where(cond => !cond.SCAM_IsDeleted && cond.SCAM_ID == selectSharedCustomAttributeMappingID).FirstOrDefault();

                //delete Custom Attribute
                if (!sharedCustomAttribute.IsNullOrEmpty())
                {
                    sharedCustomAttribute.SCA_IsDeleted = true;
                    sharedCustomAttribute.SAC_ModifiedBy = currentLoggedInUserId;
                    sharedCustomAttribute.SCA_ModifiedOn = DateTime.Now;
                }

                //delete Custom Attribute Mapping.
                if (!sharedCustomAttributeMapping.IsNullOrEmpty())
                {
                    sharedCustomAttributeMapping.SCAM_IsDeleted = true;
                    sharedCustomAttributeMapping.SCAM_ModifiedBy = currentLoggedInUserId;
                    sharedCustomAttributeMapping.SCAM_ModifiedOn = DateTime.Now;
                }

                if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
                return false;
            }
            return false;
        }

        List<Entity.SharedDataEntity.lkpCustomAttributeDataType> IPlacementMatchingSetupRepository.GetSharedAttributeDataTypes()
        {
            String userGroupUseTypeCode = CustomAttributeDatatype.User_Group.GetStringValue();
            return SharedDataDBContext.lkpCustomAttributeDataTypes.Where(cond => cond.Code != userGroupUseTypeCode).ToList();
        }

        List<lkpSharedCustomAttributeUseType> IPlacementMatchingSetupRepository.GetSharedAttributeUseTypes()
        {
            return SharedDataDBContext.lkpSharedCustomAttributeUseTypes.Where(cond => !cond.SCAUT_IsDeleted).ToList();
        }

        List<CustomAttribteContract> IPlacementMatchingSetupRepository.GetSharedCustomAttributeList(Int32 agencyRootNodeID, String useTypeCode, Int32? recordId, String recordTypeCode)
        {
            String inventoryAndRequestUseTypeCode = SharedCustomAttributeUseType.ClinicalInventoryAndRequest.GetStringValue();
            List<SharedCustomAttribute> sharedCustomAttributeList = SharedDataDBContext.SharedCustomAttributes.Where(cond => !cond.SCA_IsDeleted && cond.SCA_IsActive
                                                                           && (cond.lkpSharedCustomAttributeUseType.SCAUT_Code == useTypeCode || cond.lkpSharedCustomAttributeUseType.SCAUT_Code == inventoryAndRequestUseTypeCode)).ToList();

            List<CustomAttribteContract> lstCustomAttributeContract = new List<CustomAttribteContract>();
            List<SharedCustomAttributeMapping> existingSharedCustomAttributeMappingList = new List<SharedCustomAttributeMapping>();

            if (!agencyRootNodeID.IsNullOrEmpty() && agencyRootNodeID > AppConsts.NONE)
            {
                List<Int32> lstCustomAttributeIds = sharedCustomAttributeList.Select(sel => sel.SCA_ID).ToList();
                if (!lstCustomAttributeIds.IsNullOrEmpty())
                {
                    existingSharedCustomAttributeMappingList = SharedDataDBContext.SharedCustomAttributeMappings.Include("SharedCustomAttributeValues")
                                                                                             .Where(con => con.SCAM_RecordID == agencyRootNodeID
                                                                                               && lstCustomAttributeIds.Contains(con.SCAM_SharedCustomAttributeID)
                                                                                               && !con.SCAM_IsDeleted).ToList();
                }
            }

            foreach (SharedCustomAttribute sharedCustomAttribute in sharedCustomAttributeList)
            {
                CustomAttribteContract customAttribteContract = new CustomAttribteContract();
                SharedCustomAttributeMapping existingSharedCustomAttributeMapping = new SharedCustomAttributeMapping();
                customAttribteContract.CustomAttributeId = sharedCustomAttribute.SCA_ID;
                customAttribteContract.CustomAttributeName = sharedCustomAttribute.SCA_AttributeName;
                customAttribteContract.CustomAttributeLabel = sharedCustomAttribute.SCA_AttributeLabel;
                customAttribteContract.CustomAttributeDataTypeCode = sharedCustomAttribute.lkpCustomAttributeDataType.Code;
                customAttribteContract.CustomAttributeIsRequired = sharedCustomAttribute.SCA_IsRequired;
                customAttribteContract.MaxLength = sharedCustomAttribute.SCA_StringLength;
                existingSharedCustomAttributeMapping = existingSharedCustomAttributeMappingList.FirstOrDefault(
                    cond => cond.SCAM_SharedCustomAttributeID == sharedCustomAttribute.SCA_ID);

                if (!existingSharedCustomAttributeMapping.IsNullOrEmpty())
                {
                    customAttribteContract.CustomAttrMappingId = existingSharedCustomAttributeMapping.SCAM_ID;
                    if (!existingSharedCustomAttributeMapping.SharedCustomAttributeValues.IsNullOrEmpty() && !recordId.IsNullOrEmpty())
                    {
                        Int32 recordTypeID = SharedDataDBContext.lkpRecordTypes.Where(cond => !cond.RT_IsDeleted && cond.RT_Code == recordTypeCode).FirstOrDefault().RT_ID;
                        customAttribteContract.CustomAttrValueId = existingSharedCustomAttributeMapping.SharedCustomAttributeValues.Where(cond => !cond.SCAV_IsDeleted
                                                                    && cond.SCAV_RecordID == recordId && cond.SCAV_RecordTypeID == recordTypeID).FirstOrDefault().SCAV_ID;
                        customAttribteContract.CustomAttributeValue = existingSharedCustomAttributeMapping.SharedCustomAttributeValues.Where(cond => !cond.SCAV_IsDeleted
                                                                    && cond.SCAV_RecordID == recordId && cond.SCAV_RecordTypeID == recordTypeID).FirstOrDefault().SCAV_AttributeValue;
                    }
                }
                lstCustomAttributeContract.Add(customAttribteContract);
            }

            return lstCustomAttributeContract;
        }

    }

}


