using DAL.Interfaces;
using Entity.ClientEntity;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class AgencyJobBoardRepository : ClientBaseRepository, IAgencyJobBoardRepository
    {
        #region Variables

        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        #endregion

        #region Default Constructor to initilize DB Context
        ///// <summary>
        ///// Default constructor to initialize the context
        ///// </summary>
        public AgencyJobBoardRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }

        #endregion

        List<AgencyJobContract> IAgencyJobBoardRepository.GetAgencyJobTemplate(Int32 organizationUserID)
        {
            var _lstAgencyJobContract = new List<AgencyJobContract>();
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                     {
                           new SqlParameter("@AgencyUserOrgID", organizationUserID)

                     };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyJobBoardTemplates", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var agencyJob = new AgencyJobContract();
                            agencyJob.AgencyJobID = dr["AJT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AJT_ID"]);
                            agencyJob.AgencyHierarchyID = dr["AJT_AgencyHierarchyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AJT_AgencyHierarchyID"]);
                            agencyJob.TemplateName = dr["AJT_TemplateName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJT_TemplateName"]);
                            agencyJob.JobTitle = dr["AJT_JobTitle"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJT_JobTitle"]);
                            agencyJob.Company = dr["AJT_Company"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJT_Company"]);
                            agencyJob.Instructions = dr["AJT_Instructions"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJT_Instructions"]);
                            agencyJob.TypeID = dr["AJT_TypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AJT_TypeID"]);
                            agencyJob.JobTypeName = dr["AgencyJobType"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyJobType"]);
                            agencyJob.JobDescription = dr["AJT_Description"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJT_Description"]);
                            agencyJob.Location = dr["AJT_Location"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJT_Location"]);
                            agencyJob.HowToApply = dr["AJT_HowToApply"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJT_HowToApply"]);
                            agencyJob.AgencyJobTypeCode = dr["AgencyJobTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyJobTypeCode"]);
                            agencyJob.FieldTypeID = dr["AJT_JobFieldTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AJT_JobFieldTypeID"]);
                            agencyJob.FieldTypeName = dr["JFT_Name"] == DBNull.Value ? String.Empty : Convert.ToString(dr["JFT_Name"]);
                            //agencyJob.LogoPath = dr["AJT_LogoPath"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJT_LogoPath"]);
                            _lstAgencyJobContract.Add(agencyJob);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstAgencyJobContract.OrderBy(ord => ord.TemplateName).ToList();
        }

        #region UAT-3071

        List<DefinedRequirementContract> IAgencyJobBoardRepository.GetJobFieldType()
        {
            List<DefinedRequirementContract> lstAgencyJobContract = new List<DefinedRequirementContract>();
            List<lkpJobFieldType> LstLkpJobFieldType = base.SharedDataDBContext.lkpJobFieldTypes.Where(cond => !cond.JFT_IsDeleted).ToList();
            if (!LstLkpJobFieldType.IsNullOrEmpty())
            {
                foreach (var item in LstLkpJobFieldType)
                {
                    DefinedRequirementContract AgencyJobContract = new DefinedRequirementContract();
                    AgencyJobContract.ID = item.JFT_ID;
                    AgencyJobContract.Description = item.JFT_Name;
                    lstAgencyJobContract.Add(AgencyJobContract);
                }
            }
            return lstAgencyJobContract;
        }
        #endregion
    
        Boolean IAgencyJobBoardRepository.SaveAgencyJobTemplate(AgencyJobContract agencyJob, Int32 currentLoggedInUserId)
        {
            //Int32 agencyJobId = base.SharedDataDBContext.AgencyJobTemplates.Where(cond => cond.AJT_ID == agencyJob.AgencyJobID && !cond.AJT_IsDeleted).FirstOrDefault().AJT_ID;
            Int16 agencyJobTypeId = base.SharedDataDBContext.lkpAgencyJobTypes.Where(cond => cond.AJT_Code == agencyJob.AgencyJobTypeCode && !cond.AJT_IsDeleted).FirstOrDefault().AJT_ID;
            AgencyJobTemplate updateAgencyJobTemplate = base.SharedDataDBContext.AgencyJobTemplates.Where(cond => cond.AJT_ID == agencyJob.AgencyJobID && !cond.AJT_IsDeleted).FirstOrDefault();
            if (!updateAgencyJobTemplate.IsNullOrEmpty())
            {
                updateAgencyJobTemplate.AJT_TemplateName = agencyJob.TemplateName;
                updateAgencyJobTemplate.AJT_Company = agencyJob.Company;
                updateAgencyJobTemplate.AJT_JobTitle = agencyJob.JobTitle;
                updateAgencyJobTemplate.AJT_Description = agencyJob.JobDescription;
                updateAgencyJobTemplate.AJT_Location = agencyJob.Location;
                updateAgencyJobTemplate.AJT_Instructions = agencyJob.Instructions;
                updateAgencyJobTemplate.AJT_HowToApply = agencyJob.HowToApply;
                updateAgencyJobTemplate.AJT_TypeID = agencyJobTypeId;
                updateAgencyJobTemplate.AJT_ModifiedOn = DateTime.Now;
                updateAgencyJobTemplate.AJT_ModifiedBy = currentLoggedInUserId;
                updateAgencyJobTemplate.AJT_AgencyHierarchyID = agencyJob.AgencyHierarchyID;
                updateAgencyJobTemplate.AJT_JobFieldTypeID =Convert.ToInt16(agencyJob.FieldTypeID);

            }

            else
            {
                //ADD Template
                AgencyJobTemplate agencyJobTemplate = new AgencyJobTemplate();
                agencyJobTemplate.AJT_TemplateName = agencyJob.TemplateName;
                agencyJobTemplate.AJT_Company = agencyJob.Company;
                agencyJobTemplate.AJT_JobTitle = agencyJob.JobTitle;
                agencyJobTemplate.AJT_Description = agencyJob.JobDescription;
                agencyJobTemplate.AJT_Location = agencyJob.Location;
                agencyJobTemplate.AJT_Instructions = agencyJob.Instructions;
                agencyJobTemplate.AJT_HowToApply = agencyJob.HowToApply;
                agencyJobTemplate.AJT_TypeID = agencyJobTypeId;
                agencyJobTemplate.AJT_CreatedOn = DateTime.Now;
                agencyJobTemplate.AJT_CreatedBy = currentLoggedInUserId;
                agencyJobTemplate.AJT_AgencyHierarchyID = agencyJob.AgencyHierarchyID;
                agencyJobTemplate.AJT_JobFieldTypeID = Convert.ToInt16(agencyJob.FieldTypeID);
                base.SharedDataDBContext.AgencyJobTemplates.AddObject(agencyJobTemplate);
            }
            if (base.SharedDataDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        Boolean IAgencyJobBoardRepository.DeleteAgencyJobTemplate(AgencyJobContract agencyJob, Int32 currentLoggedInUserId)
        {
            AgencyJobTemplate deleteAgencyJobTemplate = base.SharedDataDBContext.AgencyJobTemplates.Where(cond => cond.AJT_ID == agencyJob.AgencyJobID && !cond.AJT_IsDeleted).FirstOrDefault();
            if (!deleteAgencyJobTemplate.IsNullOrEmpty())
            {
                deleteAgencyJobTemplate.AJT_IsDeleted = true;
                deleteAgencyJobTemplate.AJT_ModifiedBy = currentLoggedInUserId;
                deleteAgencyJobTemplate.AJT_ModifiedOn = DateTime.Now;
            }
            if (base.SharedDataDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        Int32 IAgencyJobBoardRepository.GetAgencyHierarchyId(Int32 organizationUserID)
        {
            Int32 agencyHierarchyId = AppConsts.NONE;
            EntityConnection Connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (var connection = new SqlConnection(Connection.StoreConnection.ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "udf_GetAHRootNodeIDMappedWithAU";

                    command.Parameters.AddWithValue("@AgencyUserOrgID", organizationUserID);

                    SqlParameter returnValue = command.Parameters.Add("@RETURN_VALUE", DbType.Int32);
                    returnValue.Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    command.ExecuteNonQuery();
                    if(!returnValue.Value.IsNullOrEmpty())
                        agencyHierarchyId = Convert.ToInt32(returnValue.Value);
                }
            }
            return agencyHierarchyId;
        }

        List<AgencyJobContract> IAgencyJobBoardRepository.GetAgencyJobPosting(Int32 organizationUserID)
        {
            var _lstAgencyJobContract = new List<AgencyJobContract>();
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                     {
                           new SqlParameter("@AgencyUserOrgID", organizationUserID)

                     };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyJobBoardPosting", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var agencyJob = new AgencyJobContract();
                            agencyJob.AgencyJobID = dr["AJP_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AJP_ID"]);
                            agencyJob.AgencyHierarchyID = dr["AJP_AgencyHierarchyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AJP_AgencyHierarchyID"]);
                            //agencyJob.TemplateName = dr["AJT_TemplateName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJT_TemplateName"]);
                            agencyJob.JobTitle = dr["AJP_JobTitle"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJP_JobTitle"]);
                            agencyJob.Company = dr["AJP_Company"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJP_Company"]);
                            agencyJob.Instructions = dr["AJP_Instructions"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJP_Instructions"]);
                            agencyJob.TypeID = dr["AJP_JobTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AJP_JobTypeID"]);
                            agencyJob.JobTypeName = dr["AgencyJobType"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyJobType"]);
                            agencyJob.JobDescription = dr["AJP_Description"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJP_Description"]);
                            agencyJob.Location = dr["AJP_Location"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJP_Location"]);
                            agencyJob.HowToApply = dr["AJP_HowToApply"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJP_HowToApply"]);
                            agencyJob.AgencyJobTypeCode = dr["AgencyJobTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyJobTypeCode"]);
                            agencyJob.StatusID = dr["AJP_JobStatusID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AJP_JobStatusID"]);
                            agencyJob.Status = dr["StatusName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StatusName"]);
                            agencyJob.StatusCode = dr["StatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StatusCode"]);
                            //agencyJob.LogoPath = dr["AJT_LogoPath"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AJT_LogoPath"]);
                            agencyJob.PublishDate = dr["AJP_PublishDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["AJP_PublishDate"]);
                            agencyJob.FieldTypeID = dr["AJP_JobFieldTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AJP_JobFieldTypeID"]);
                            agencyJob.FieldTypeName = dr["JFT_Name"] == DBNull.Value ? String.Empty : Convert.ToString(dr["JFT_Name"]);
                            _lstAgencyJobContract.Add(agencyJob);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstAgencyJobContract;
        }

        Boolean IAgencyJobBoardRepository.SaveAgencyJobPosting(AgencyJobContract agencyJob, Int32 currentLoggedInUserId)
        {
            Int16 agencyJobTypeId = base.SharedDataDBContext.lkpAgencyJobTypes.Where(cond => cond.AJT_Code == agencyJob.AgencyJobTypeCode && !cond.AJT_IsDeleted).FirstOrDefault().AJT_ID;
            Int16 agencyJobStausId = base.SharedDataDBContext.lkpAgencyJobStatus.Where(cond => cond.AJS_Code == agencyJob.StatusCode && !cond.AJS_IsDeleted).FirstOrDefault().AJS_ID;

            AgencyJobPosting updateAgencyJobPosting = base.SharedDataDBContext.AgencyJobPostings.Where(cond => cond.AJP_ID == agencyJob.AgencyJobID && !cond.AJP_IsDeleted).FirstOrDefault();
            if (!updateAgencyJobPosting.IsNullOrEmpty())
            {
                updateAgencyJobPosting.AJP_Company = agencyJob.Company;
                updateAgencyJobPosting.AJP_JobTitle = agencyJob.JobTitle;
                updateAgencyJobPosting.AJP_Description = agencyJob.JobDescription;
                updateAgencyJobPosting.AJP_Location = agencyJob.Location;
                updateAgencyJobPosting.AJP_Instructions = agencyJob.Instructions;
                updateAgencyJobPosting.AJP_HowToApply = agencyJob.HowToApply;
                updateAgencyJobPosting.AJP_JobTypeID = agencyJobTypeId;
                updateAgencyJobPosting.AJP_JobStatusID = agencyJobStausId;
                updateAgencyJobPosting.AJP_ModifiedOn = DateTime.Now;
                updateAgencyJobPosting.AJP_ModifiedBy = currentLoggedInUserId;
                updateAgencyJobPosting.AJP_AgencyHierarchyID = agencyJob.AgencyHierarchyID;
                updateAgencyJobPosting.AJP_PublishDate = agencyJob.PublishDate;
                updateAgencyJobPosting.AJP_JobFieldTypeID = Convert.ToInt16(agencyJob.FieldTypeID);
            }
            else
            {
                AgencyJobPosting agencyJobPosting = new AgencyJobPosting();
                agencyJobPosting.AJP_Company = agencyJob.Company;
                agencyJobPosting.AJP_JobTitle = agencyJob.JobTitle;
                agencyJobPosting.AJP_Description = agencyJob.JobDescription;
                agencyJobPosting.AJP_Location = agencyJob.Location;
                agencyJobPosting.AJP_Instructions = agencyJob.Instructions;
                agencyJobPosting.AJP_HowToApply = agencyJob.HowToApply;
                agencyJobPosting.AJP_JobTypeID = agencyJobTypeId;
                agencyJobPosting.AJP_JobStatusID = agencyJobStausId;
                agencyJobPosting.AJP_CreatedOn = DateTime.Now;
                agencyJobPosting.AJP_CreatedBy = currentLoggedInUserId;
                agencyJobPosting.AJP_AgencyHierarchyID = agencyJob.AgencyHierarchyID;
                agencyJobPosting.AJP_PublishDate = agencyJob.PublishDate;
                agencyJobPosting.AJP_JobFieldTypeID = Convert.ToInt16(agencyJob.FieldTypeID);
                base.SharedDataDBContext.AgencyJobPostings.AddObject(agencyJobPosting);
            }
            if (base.SharedDataDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        Boolean IAgencyJobBoardRepository.DeleteAgencyJobPosting(AgencyJobContract agencyJobPosting, Int32 currentLoggedInUserId)
        {
            AgencyJobPosting deleteAgencyJobPosting = base.SharedDataDBContext.AgencyJobPostings.Where(cond => cond.AJP_ID == agencyJobPosting.AgencyJobID && !cond.AJP_IsDeleted).FirstOrDefault();
            if (!deleteAgencyJobPosting.IsNullOrEmpty())
            {
                deleteAgencyJobPosting.AJP_IsDeleted = true;
                deleteAgencyJobPosting.AJP_ModifiedBy = currentLoggedInUserId;
                deleteAgencyJobPosting.AJP_ModifiedOn = DateTime.Now;
            }
            if (base.SharedDataDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        AgencyJobContract IAgencyJobBoardRepository.GetTemplateDetailsByID(Int32 selectedTempateID)
        {
            AgencyJobContract templateDetailContract = new AgencyJobContract();
            AgencyJobTemplate seletedTemplateDetails = base.SharedDataDBContext.AgencyJobTemplates.Where(cond => cond.AJT_ID == selectedTempateID && !cond.AJT_IsDeleted).FirstOrDefault();
            String agencyJobTypeCode = base.SharedDataDBContext.lkpAgencyJobTypes.Where(cond => cond.AJT_ID == seletedTemplateDetails.AJT_TypeID && !cond.AJT_IsDeleted).FirstOrDefault().AJT_Code;
            templateDetailContract.Company = seletedTemplateDetails.AJT_Company;
            templateDetailContract.JobDescription = seletedTemplateDetails.AJT_Description;
            templateDetailContract.HowToApply = seletedTemplateDetails.AJT_HowToApply;
            templateDetailContract.Instructions = seletedTemplateDetails.AJT_Instructions;
            templateDetailContract.JobTitle = seletedTemplateDetails.AJT_JobTitle;
            templateDetailContract.TypeID = seletedTemplateDetails.AJT_TypeID;
            templateDetailContract.Location = seletedTemplateDetails.AJT_Location;
            templateDetailContract.FieldTypeID = Convert.ToInt32(seletedTemplateDetails.AJT_JobFieldTypeID);
           // templateDetailContract.FieldTypeName=seletedTemplateDetails.
                
            templateDetailContract.AgencyJobTypeCode = agencyJobTypeCode;
            

            return templateDetailContract;
        }

        AgencyLogoContract IAgencyJobBoardRepository.GetAgencyLogo(Int32 agencyHierarchyID)
        {
            AgencyLogoContract agencyLogoContract = new AgencyLogoContract();

            var agencyLogo = this.SharedDataDBContext.AgencyLogoes
                                        .Where(cond => cond.AL_AgencyHierarchyID == agencyHierarchyID
                                                && !cond.AL_IsDeleted).FirstOrDefault();

            if (!agencyLogo.IsNullOrEmpty())
            {
                agencyLogoContract.AgencyHierarchyID = agencyLogo.AL_AgencyHierarchyID;
                agencyLogoContract.LogoPath = agencyLogo.AL_Path;
                agencyLogoContract.AgencyLogoID = agencyLogo.AL_ID;
            }

            return agencyLogoContract;
        }

        Boolean IAgencyJobBoardRepository.SaveUpdateAgencyLogo(AgencyLogoContract agencyLogoContract, Int32 currentLoggedInUserID)
        {
            var existingAgencyLogo = this.SharedDataDBContext.AgencyLogoes
                                        .Where(cond => cond.AL_ID == agencyLogoContract.AgencyLogoID
                                                && cond.AL_AgencyHierarchyID == agencyLogoContract.AgencyHierarchyID
                                                && !cond.AL_IsDeleted).FirstOrDefault();

            if (!existingAgencyLogo.IsNullOrEmpty())
            {
                existingAgencyLogo.AL_Path = agencyLogoContract.LogoPath;
                existingAgencyLogo.AL_ModifiedBy = currentLoggedInUserID;
                existingAgencyLogo.AL_ModifiedOn = DateTime.Now;
            }
            else
            {
                AgencyLogo agencyLogo = new AgencyLogo();
                agencyLogo.AL_AgencyHierarchyID = agencyLogoContract.AgencyHierarchyID;
                agencyLogo.AL_Path = agencyLogoContract.LogoPath;
                agencyLogo.AL_IsDeleted = false;
                agencyLogo.AL_CreatedBy = currentLoggedInUserID;
                agencyLogo.AL_CreatedOn = DateTime.Now;
                this.SharedDataDBContext.AgencyLogoes.AddObject(agencyLogo);
            }

            if (this.SharedDataDBContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        Boolean IAgencyJobBoardRepository.ClearLogo(Int32 agencyHierarchyID, Int32 currentLoggedInUserID)
        {
            AgencyLogo deleteAgencyLogo = base.SharedDataDBContext.AgencyLogoes.Where(cond => cond.AL_AgencyHierarchyID == agencyHierarchyID && !cond.AL_IsDeleted).FirstOrDefault();
            if (!deleteAgencyLogo.IsNullOrEmpty())
            {
                deleteAgencyLogo.AL_IsDeleted = true;
                deleteAgencyLogo.AL_ModifiedBy = currentLoggedInUserID;
                deleteAgencyLogo.AL_ModifiedOn = DateTime.Now;
            }
            if (base.SharedDataDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        Boolean IAgencyJobBoardRepository.ArchiveJobPosts(List<Int32> agencyJobIds, Int32 currentLoggedInUserID)
        {
            String archivedAgencyJobStatusCode = AgencyJobStatus.Archived.GetStringValue();
            String draftAndArchivedAgencyJobStatusCode = AgencyJobStatus.DraftAndArchived.GetStringValue();
            string draftAgencyJobStatusCode = AgencyJobStatus.Draft.GetStringValue();

            var archiveStatusID = base.SharedDataDBContext.lkpAgencyJobStatus.Where(cond => cond.AJS_Code == archivedAgencyJobStatusCode && !cond.AJS_IsDeleted).FirstOrDefault().AJS_ID;
            var draftAndArchiveStatusID = base.SharedDataDBContext.lkpAgencyJobStatus.Where(cond => cond.AJS_Code == draftAndArchivedAgencyJobStatusCode && !cond.AJS_IsDeleted).FirstOrDefault().AJS_ID;
            var draftAgencyJobStatusID = base.SharedDataDBContext.lkpAgencyJobStatus.Where(cond => cond.AJS_Code == draftAgencyJobStatusCode && !cond.AJS_IsDeleted).FirstOrDefault().AJS_ID;


            var lstAgencyJobPosting = base.SharedDataDBContext.AgencyJobPostings.Where(cond => agencyJobIds.Contains(cond.AJP_ID)
                                                                                        && !cond.AJP_IsDeleted).ToList();

            if (!lstAgencyJobPosting.IsNullOrEmpty())
            {
                foreach (var item in lstAgencyJobPosting)
                {
                    item.AJP_JobStatusID = item.AJP_JobStatusID == draftAgencyJobStatusID ? draftAndArchiveStatusID : archiveStatusID;
                    item.AJP_ModifiedBy = currentLoggedInUserID;
                    item.AJP_ModifiedOn = DateTime.Now;
                }
            }

            if (base.SharedDataDBContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        AgencyJobContract IAgencyJobBoardRepository.GetSelectedJobPostDetails(Int32 CurrentAgencyJobID)
        {
            AgencyJobPosting selectedPostDetails = base.SharedDataDBContext.AgencyJobPostings.Where(cond => cond.AJP_ID == CurrentAgencyJobID && !cond.AJP_IsDeleted).FirstOrDefault();
            AgencyJobContract jobDetail = new AgencyJobContract();

            if (!selectedPostDetails.IsNullOrEmpty())
            {
                AgencyLogo agencyLogo = base.SharedDataDBContext.AgencyLogoes.Where(cond => cond.AL_AgencyHierarchyID == selectedPostDetails.AJP_AgencyHierarchyID && !cond.AL_IsDeleted).FirstOrDefault();

                if (!agencyLogo.IsNullOrEmpty())
                {
                    jobDetail.LogoPath = agencyLogo.AL_Path;
                }

                jobDetail.JobTitle = selectedPostDetails.AJP_JobTitle;
                jobDetail.Company = selectedPostDetails.AJP_Company;
                jobDetail.Location = selectedPostDetails.AJP_Location;
                jobDetail.JobDescription = selectedPostDetails.AJP_Description;
                jobDetail.Instructions = selectedPostDetails.AJP_Instructions;
                jobDetail.HowToApply = selectedPostDetails.AJP_HowToApply;
            }

            return jobDetail;
        }

        List<AgencyJobContract> IAgencyJobBoardRepository.GetViewAgencyJobPosting(JobSearchContract jobSearchContract, CustomPagingArgsContract grdCustomContract)
        {
            List<AgencyJobContract> lstAgencyJobContract = new List<AgencyJobContract>();

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                   {
                           new SqlParameter("@JobTitle", jobSearchContract.JobTitle.IsNullOrEmpty() ? null : jobSearchContract.JobTitle),
                           new SqlParameter("@JobType", jobSearchContract.JobTypeCode.IsNullOrEmpty() ? null : jobSearchContract.JobTypeCode),
                           new SqlParameter ("@Location", jobSearchContract.Location.IsNullOrEmpty() ? null : jobSearchContract.Location),
                           new SqlParameter ("@Company", jobSearchContract.Company.IsNullOrEmpty() ? null : jobSearchContract.Company),
                           new SqlParameter("@OrganizationUserId", jobSearchContract.OrganizationUserId),
                           new SqlParameter("@TenantId", jobSearchContract.TenantId),
                           new SqlParameter("@OrderBy", grdCustomContract.SortExpression.IsNullOrEmpty() ? null : grdCustomContract.SortExpression),
                           new SqlParameter("@OrderDirection", grdCustomContract.SortDirectionDescending ? "DESC" : "ASC"),
                           new SqlParameter("@PageIndex", grdCustomContract.CurrentPageIndex),
                           new SqlParameter("@PageSize", grdCustomContract.PageSize),
                           new SqlParameter("@JobFieldTypeID", jobSearchContract.JobFieldTypeID)
                   };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyJobPostingDetails", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var agencyJobData = new AgencyJobContract();

                            agencyJobData.AgencyJobID = dr["AgencyJobID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyJobID"]);
                            agencyJobData.JobTitle = dr["JobTitle"] == DBNull.Value ? String.Empty : Convert.ToString(dr["JobTitle"]);
                            agencyJobData.JobTypeName = dr["JobTypeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["JobTypeName"]);
                            agencyJobData.Location = dr["Location"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Location"]);
                            agencyJobData.Company = dr["Company"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Company"]);
                            agencyJobData.JobDescription = dr["JobDescription"] == DBNull.Value ? String.Empty : Convert.ToString(dr["JobDescription"]);
                            agencyJobData.TotalCount = dr["TotalCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TotalCount"]);
                            agencyJobData.FieldTypeID = dr["JobFieldTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["JobFieldTypeID"]);
                            agencyJobData.FieldTypeName = dr["JobFieldType"] == DBNull.Value ? String.Empty : Convert.ToString(dr["JobFieldType"]);
                            lstAgencyJobContract.Add(agencyJobData);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return lstAgencyJobContract;
        }

        Boolean IAgencyJobBoardRepository.SaveClientSystemDocument(Int32 agencyHierarchyID, List<RequirementApprovalNotificationDocumentContract> lstRequirementApprovalNotificationDocumentContract)
        {
            string documentTypeCode = lstRequirementApprovalNotificationDocumentContract.IsNullOrEmpty() ? string.Empty : lstRequirementApprovalNotificationDocumentContract.FirstOrDefault().DocumentTypeCode;
            Int32 documentTypeID = base.SharedDataDBContext.lkpDocumentTypes.Where(cond => cond.DMT_Code == documentTypeCode && !cond.DMT_IsDeleted).FirstOrDefault().DMT_ID;
            Int32 createdById = lstRequirementApprovalNotificationDocumentContract.IsNullOrEmpty() ? 0 : lstRequirementApprovalNotificationDocumentContract.FirstOrDefault().CreatedBy;

            foreach (RequirementApprovalNotificationDocumentContract doc in lstRequirementApprovalNotificationDocumentContract)
            {
                Entity.SharedDataEntity.AgencyHierarchySystemDocument agencyHierarchySystemDocument = new AgencyHierarchySystemDocument();
                agencyHierarchySystemDocument.AHSD_AgencyHierarchyID = agencyHierarchyID;
                agencyHierarchySystemDocument.AHSD_AgencyID = doc.AgencyID.IsNullOrEmpty() ? (Int32?)null : doc.AgencyID;
                agencyHierarchySystemDocument.AHSD_IsDeleted = false;
                agencyHierarchySystemDocument.AHSD_CreatedBy = createdById;
                agencyHierarchySystemDocument.AHSD_CreatedOn = DateTime.Now;

                Entity.SharedDataEntity.ClientSystemDocument clientSystemDocument = new Entity.SharedDataEntity.ClientSystemDocument();
                clientSystemDocument.CSD_FileName = doc.FileName;
                clientSystemDocument.CSD_DocumentPath = doc.DocumentPath;
                clientSystemDocument.CSD_Description = doc.Description;
                clientSystemDocument.CSD_DocumentTypeID = documentTypeID;
                clientSystemDocument.CSD_IsDeleted = false;
                clientSystemDocument.CSD_CreatedByID = doc.CreatedBy;
                clientSystemDocument.CSD_CreatedOn = DateTime.Now;

                if (!doc.Size.IsNullOrEmpty())
                    clientSystemDocument.CSD_Size = doc.Size;

                agencyHierarchySystemDocument.ClientSystemDocument = clientSystemDocument;
                base.SharedDataDBContext.AgencyHierarchySystemDocuments.AddObject(agencyHierarchySystemDocument);
            }

            if (base.SharedDataDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        RequirementApprovalNotificationDocumentContract IAgencyJobBoardRepository.GetClientSystemDocumentBasedOnDocumentType(Int32 agencyHierarchyID, String clientSystemDocumentTypeCode)
        {
            Int32 documentTypeID = base.SharedDataDBContext.lkpDocumentTypes.Where(cond => cond.DMT_Code == clientSystemDocumentTypeCode && !cond.DMT_IsDeleted).FirstOrDefault().DMT_ID;
            RequirementApprovalNotificationDocumentContract requirementApprovalNotificationDocumentContract = new RequirementApprovalNotificationDocumentContract();

            var doc = base.SharedDataDBContext.AgencyHierarchySystemDocuments.Include("ClientSystemDocument")
                        .Where(cond => cond.AHSD_AgencyHierarchyID == agencyHierarchyID
                                && cond.ClientSystemDocument.CSD_DocumentTypeID == documentTypeID
                                && !cond.AHSD_AgencyID.HasValue
                                && !cond.AHSD_IsDeleted).FirstOrDefault();

            if (!doc.IsNullOrEmpty() && doc.AHSD_ID > 0)
            {
                requirementApprovalNotificationDocumentContract.FileName = doc.ClientSystemDocument.CSD_FileName;
                requirementApprovalNotificationDocumentContract.Description = doc.ClientSystemDocument.CSD_Description;
                requirementApprovalNotificationDocumentContract.AgencyHierarchySystemDocumentID = doc.AHSD_ID;
                requirementApprovalNotificationDocumentContract.DocumentPath = doc.ClientSystemDocument.CSD_DocumentPath;
                requirementApprovalNotificationDocumentContract.Size = doc.ClientSystemDocument.CSD_Size;
            }
            return requirementApprovalNotificationDocumentContract;
        }

        Boolean IAgencyJobBoardRepository.DeleteClientSystemDocumentBasedOnDocType(Int32 agencyHierarchyID, Int32? agencyID, Int32 currentLoggedInUserID, String requirementApprovalNotificationDocumentTypeCode)
        {
            Int32 requirementApprovalNotificationDocumentTypeID = base.SharedDataDBContext.lkpDocumentTypes.Where(cond => cond.DMT_Code == requirementApprovalNotificationDocumentTypeCode && !cond.DMT_IsDeleted).FirstOrDefault().DMT_ID;

            Boolean isAgencyIDPassed = agencyID.HasValue && agencyID.Value > 0 ? true : false;

            var doc = base.SharedDataDBContext.AgencyHierarchySystemDocuments
                            .Where(cond => cond.AHSD_AgencyHierarchyID == agencyHierarchyID
                                && cond.ClientSystemDocument.CSD_DocumentTypeID == requirementApprovalNotificationDocumentTypeID
                                  && ((isAgencyIDPassed == true && cond.AHSD_AgencyID == agencyID)
                                                || (isAgencyIDPassed == false && !cond.AHSD_AgencyID.HasValue))
                                && !cond.AHSD_IsDeleted).FirstOrDefault();

            if (!doc.IsNullOrEmpty())
            {
                doc.AHSD_IsDeleted = true;
                doc.AHSD_ModifiedBy = currentLoggedInUserID;
                doc.AHSD_ModifiedOn = DateTime.Now;

                var clientSystemDocuments = base.SharedDataDBContext.ClientSystemDocuments
                            .Where(cond => cond.CSD_ID == doc.AHSD_ClientSystemDocumentID && !cond.CSD_IsDeleted).FirstOrDefault();

                if (!clientSystemDocuments.IsNullOrEmpty())
                {
                    clientSystemDocuments.CSD_IsDeleted = true;
                    clientSystemDocuments.CSD_ModifiedByID = currentLoggedInUserID;
                    clientSystemDocuments.CSD_ModifiedOn = DateTime.Now;
                }
            }

            if (base.SharedDataDBContext.SaveChanges() > 0)
                return true;

            return false;
        }

    }
}
