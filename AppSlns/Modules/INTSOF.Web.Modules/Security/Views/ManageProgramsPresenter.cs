#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageProgramsPresenter.cs
// Purpose:   
//

#endregion

#region Namespace

#region System Defined

using System;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Business.RepoManagers;
using System.Collections.Generic;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    public class ManageProgramsPresenter : Presenter<IManageProgramsView>
    {

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// This method is invoked by the view every time it loads.
        /// </summary>
        public override void OnViewLoaded()
        {
            //if (!View.IsAdmin)
            //{
            //    if (!View.LoginUserProductId.IsNull())
            //    {
            //        View.ViewContract.LoginUserProductName = SecurityManager.GetTenantProduct((Int32)View.LoginUserProductId).Name;
            //    }
            //}

            //View.TenantsRole = SecurityManager.GetTenant(View.ViewContract.TenantId);
        }


        /// <summary>
        /// Performs a delete operation for role details.
        /// </summary>
        public void DeleteProgramDetails()
        {
            //AdminProgramStudy Program = SecurityManager.GetProgram(View.ViewContract.ProgramId);
           // DeptProgramMapping depProgramMapping = SecurityManager.GetOrganizationProgram(View.DepProgramMappingId);
            //depProgramMapping.DPM_IsDeleted = true;
            //depProgramMapping.DPM_ModifiedByID = View.CurrentUserId;
            //depProgramMapping.DPM_ModifiedOn = DateTime.Now;
            //depProgramMapping.AdminProgramStudy.DeleteFlag = true;
            //depProgramMapping.AdminProgramStudy.ModifyUserID = View.CurrentUserId;
            //depProgramMapping.AdminProgramStudy.ModifyDate = DateTime.Now;
            //depProgramMapping.AdminProgramStudy.ProgramStudy = depProgramMapping.AdminProgramStudy.ProgramStudy + "_" + Guid.NewGuid();
           // View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.ROLE) + SysXUtils.GetMessage(ResourceConst.SPACE) + Program.ProgramStudy.Split(new char[] { '_' }).FirstOrDefault() + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.DELETED_SUCCESSFULLY);
            //View.SuccessMessage = String.Format("Program {0} deleted successfully", Program.ProgramStudy);
            //Program.ProgramStudy = Program.ProgramStudy + "_" + Guid.NewGuid();
            if (SecurityManager.UpdateProgramObject())
            {
                Entity.ClientEntity.DeptProgramMapping clientDepProgramMapping = ClientSecurityManager.GetDepProgramMapping(View.TenantId, View.DepProgramMappingId);
                clientDepProgramMapping.DPM_IsDeleted = true;
                clientDepProgramMapping.DPM_ModifiedByID = View.CurrentUserId;
                clientDepProgramMapping.DPM_ModifiedOn = DateTime.Now;
                //clientDepProgramMapping.AdminProgramStudy.DeleteFlag = true;
                //clientDepProgramMapping.AdminProgramStudy.ModifyUserID = View.CurrentUserId;
                //clientDepProgramMapping.AdminProgramStudy.ModifyDate = DateTime.Now;
                //clientDepProgramMapping.AdminProgramStudy.ProgramStudy = depProgramMapping.AdminProgramStudy.ProgramStudy;
                List<Entity.ClientEntity.DeptProgramPaymentOption> tempPaymentOptionList = ClientSecurityManager.GetMappedDepProgramPaymentOption(View.TenantId, View.DepProgramMappingId).ToList();
                tempPaymentOptionList.ForEach(cond=>
                {
                    cond.DPPO_IsDeleted = true;
                    cond.DPPO_ModifiedByID = View.CurrentUserId;
                    cond.DPPO_ModifiedOn = DateTime.Now;
                });
                ClientSecurityManager.DeleteProgram(View.TenantId);
                View.SuccessMessage = "Program deleted successfully";
            }
            else
            {
                View.ErrorMessage = "Some error occured.Please try again";
            }

        }

        /// <summary>
        /// Retrieves a list of all Grades with it's details.
        /// </summary>
        //public void RetrievingGradeDetails()
        //{
        //   View.AllGrades = (List<lkpGradeLevel>)SecurityManager.GetGrades().ToList();
        //   //var Grades= Grades.Select(s => new { ID= s.ID, Name= s.Description }).ToList();

        //}
        public IQueryable<lkpGradeLevel> RetrievingGradeDetails()
        {
            var organizationDetail = SecurityManager.GetOrganization(View.ViewContract.OrganizationID);
            var Grades = SecurityManager.GetGradeListByOrganizationId(Convert.ToInt32(organizationDetail.ParentOrganizationID));
            //View.AllGrades = View.AllGrades;
            return Grades;
        }

        /// <summary>
        /// method to set payment options
        /// </summary>
        public void GetAllPaymentOption()
        {
            View.AllPaymentOption = ClientSecurityManager.GetAllPaymentOption(View.TenantId);
        }

        /// <summary>
        /// Get MappedPaymentOptionIds
        /// </summary>
        /// <param name="depProgramMappingId">depProgramMappingId</param>
        /// <returns>List</returns>
        public List<Int32> MappedPaymentOptionIds(Int32 depProgramMappingId)
        {
            List<Int32> listIds = ClientSecurityManager.GetMappedDepProgramPaymentOption(View.TenantId, depProgramMappingId).Select(x => x.DPPO_PaymentOptionID).ToList();
            return listIds;
        }

        /// <summary>
        /// Retrieves a list of all roles with it's details.
        /// </summary>
        public void RetrievingProgramDetails(Int32 OrganizationID)
        {


            //if (View.ViewContract.TenantId > AppConsts.NONE)
            //{
            //    var tenantProductId = SecurityManager.GetTenantProductId(View.ViewContract.TenantId) ?? default(Int32);
            //    View.ViewContract.TenantProductId = tenantProductId;

            //    View.RoleDetails = SecurityManager.GetRoleDetailsByProductId(tenantProductId).ToList();
            //    View = SecurityManager.GetRoleDetailsByProductId(tenantProductId).ToList();
            //}
            //else
            //{
            //    View.RoleDetails = SecurityManager.GetRoleDetail(View.IsAdmin, View.CurrentUserId).ToList();
            //}

            //View.OrganizationPrograms = SecurityManager.GetOrganizationProgramList(OrganizationID);
        }

        /// <summary>
        /// Performs an insert operation for role.
        /// </summary>
        public void ProgramSave()
        {

            //var ProgramId = View.ViewContract.ProgramId;
            //var organizationDetail = SecurityManager.GetOrganization(View.ViewContract.OrganizationID);
            //if (SecurityManager.IsProgramExists(View.ViewContract.ProgramStudy, null))
            //{
            //    View.ErrorMessage = View.ViewContract.ProgramStudy + " " + "Program already exists.";
            //}
            //else
            //{
            //    View.ErrorMessage = String.Empty;
               
            //    //DeptProgramMapping depProgramMapping = new DeptProgramMapping();
            //    AdminProgramStudy ProgramStudy = new AdminProgramStudy
            //    {
            //        ProgramStudy = View.ViewContract.ProgramStudy,
            //        OrganizationID = Convert.ToInt32(organizationDetail.ParentOrganizationID),
            //       // RenewalTerm = View.ViewContract.RenewalTerm,
            //        ManagementFee = View.ViewContract.ManagementFee,
            //        Description = View.ViewContract.Description,
            //        CreateUserID = View.CurrentUserId,
                    
            //        DurationMonth=View.ViewContract.DurationMonth,
            //        CreateDate = DateTime.Now,
            //        //IsActive = true,
            //        DeleteFlag = false
            //        //IsUserGroupLevel = View.ViewContract.IsUserGroupLevel
            //    };
            //    //depProgramMapping.DPM_OrganizationID = View.ViewContract.OrganizationID;
            //    //depProgramMapping.DPM_CreatedByID = View.CurrentUserId;
            //    //depProgramMapping.DPM_CreatedOn = DateTime.Now;
            //    //depProgramMapping.DPM_IsDeleted = false;
            //    if (View.ViewContract.GradeLevelID != 0)
            //        ProgramStudy.GradeLevelID = View.ViewContract.GradeLevelID;
            //    //depProgramMapping.AdminProgramStudy = ProgramStudy;
            //    //SecurityManager.AddProgram(depProgramMapping);
            //    //if (depProgramMapping.AdminProgramStudy.AdminProgramStudyID != 0 && depProgramMapping.DPM_ID != 0)
            //    //{
            //    //    Entity.ClientEntity.DeptProgramMapping clientDepProgramMapping = new Entity.ClientEntity.DeptProgramMapping();
            //    //    clientDepProgramMapping.DPM_OrganizationID = depProgramMapping.DPM_OrganizationID;
            //    //    clientDepProgramMapping.DPM_ID = depProgramMapping.DPM_ID;
            //    //    clientDepProgramMapping.DPM_CreatedOn = depProgramMapping.DPM_CreatedOn;
            //    //    clientDepProgramMapping.DPM_IsDeleted = depProgramMapping.DPM_IsDeleted;
            //    //    clientDepProgramMapping.DPM_CreatedByID = depProgramMapping.DPM_CreatedByID;
            //    //    Entity.ClientEntity.AdminProgramStudy clientProgramStudy = new Entity.ClientEntity.AdminProgramStudy
            //    //    {
            //    //        ProgramStudy = View.ViewContract.ProgramStudy,
            //    //        AdminProgramStudyID = depProgramMapping.AdminProgramStudy.AdminProgramStudyID,
            //    //        OrganizationID = depProgramMapping.AdminProgramStudy.OrganizationID,
            //    //        //RenewalTerm = depProgramMapping.AdminProgramStudy.RenewalTerm,
            //    //        ManagementFee = depProgramMapping.AdminProgramStudy.ManagementFee,
            //    //        DurationMonth = depProgramMapping.AdminProgramStudy.DurationMonth,
            //    //        Description = depProgramMapping.AdminProgramStudy.Description,
            //    //        GradeLevelID = depProgramMapping.AdminProgramStudy.GradeLevelID,
            //    //        CreateUserID = View.CurrentUserId,
            //    //        CreateDate = depProgramMapping.AdminProgramStudy.CreateDate,
            //    //        //IsActive = true,
            //    //        DeleteFlag = false
            //    //    };
            //    //    clientDepProgramMapping.AdminProgramStudy = clientProgramStudy;

            //    //    ClientSecurityManager.AddProgram(View.TenantId, clientDepProgramMapping, GetPaymentOptionList(depProgramMapping.DPM_ID));
            //        View.SuccessMessage = "Program added successfully.";
            //    //}
            //    //else
            //    //{
            //    //    View.ErrorMessage = "Some error occured.Please try again";
            //    //}
                
            //   // View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.ROLE) + SysXUtils.GetMessage(ResourceConst.SPACE) + ProgramStudy.ProgramStudy.Split(new char[] { '_' }).FirstOrDefault() + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
            //    //View.SuccessMessage = String.Format("Program {0} saved successfully", View.ViewContract.ProgramStudy);
            //}
        }

        /// <summary>
        ///  Performs an update operation for role details.
        /// </summary>
        public void ProgramUpdate()
        {
            //var ProgramId = View.ViewContract.ProgramId;
            //AdminProgramStudy ExistingProgram = SecurityManager.GetProgram(ProgramId);
            ////DeptProgramMapping existingProgramMapping = SecurityManager.GetOrganizationProgram(ProgramId);
            //if (SecurityManager.IsProgramExists(View.ViewContract.ProgramStudy, ExistingProgram.ProgramStudy))
            //{
            //    View.ErrorMessage = View.ViewContract.ProgramStudy + " " + "Program already exists.";
            //}
            //else
            //{
            //    View.ErrorMessage = String.Empty;


            //    //AdminProgramStudy ProgramStudy = new AdminProgramStudy
            //    //{
            //    ExistingProgram.AdminProgramStudyID = View.ViewContract.ProgramId;
            //    ExistingProgram.ProgramStudy = View.ViewContract.ProgramStudy;
            //    //ExistingProgram.RenewalTerm = View.ViewContract.RenewalTerm;
            //    ExistingProgram.ManagementFee = View.ViewContract.ManagementFee;
            //    //ExistingProgram.OrganizationID = View.ViewContract.OrganizationID;
            //    ExistingProgram.Description = View.ViewContract.Description;
            //    ExistingProgram.DurationMonth = View.ViewContract.DurationMonth;
            //    if (View.ViewContract.GradeLevelID != 0)
            //        ExistingProgram.GradeLevelID = View.ViewContract.GradeLevelID;
            //    else
            //        ExistingProgram.GradeLevelID = null;
            //    ExistingProgram.ModifyUserID = View.CurrentUserId;
            //    ExistingProgram.ModifyDate = DateTime.Now;
            //    //IsActive = true,
            //    ExistingProgram.DeleteFlag = false;
            //    //IsUserGroupLevel = View.ViewContract.IsUserGroupLevel
            //    //};

            //    if (SecurityManager.UpdateProgram(ExistingProgram))
            //    {
            //        Entity.ClientEntity.AdminProgramStudy clientProgram = ClientSecurityManager.GetDepartmentProgram(View.TenantId, ExistingProgram.AdminProgramStudyID);
            //        clientProgram.AdminProgramStudyID = ExistingProgram.AdminProgramStudyID;
            //        clientProgram.ProgramStudy = ExistingProgram.ProgramStudy;
            //        //clientProgram.RenewalTerm = ExistingProgram.RenewalTerm;
            //        clientProgram.ManagementFee = ExistingProgram.ManagementFee;
            //        //clientProgram.OrganizationID = ExistingProgram.OrganizationID;
            //        clientProgram.Description = ExistingProgram.Description;
            //        clientProgram.DurationMonth = ExistingProgram.DurationMonth;
            //        clientProgram.GradeLevelID = ExistingProgram.GradeLevelID;
            //        clientProgram.ModifyUserID = ExistingProgram.ModifyUserID;
            //        clientProgram.ModifyDate = DateTime.Now;
            //        //IsActive = true,
            //        clientProgram.DeleteFlag = false;
            //        ClientSecurityManager.UpdateProgram(View.TenantId, View.DepProgramMappingId, GetPaymentOptionList(View.DepProgramMappingId),View.CurrentUserId);
            //        //View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.ROLE) + SysXUtils.GetMessage(ResourceConst.SPACE) + ExistingProgram.ProgramStudy.Split(new char[] { '_' }).FirstOrDefault() + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
            //        //View.SuccessMessage = String.Format("Program {0} updated successfully", View.ViewContract.ProgramStudy);
            //        View.SuccessMessage = "Program updated successfully.";
            //    }
            //    else
            //    {
            //        View.ErrorMessage = "Some error occured.Please try again";
            //    }

            //}
        }

        /// <summary>
        /// Method to check Program mapping
        /// </summary>
        /// <param name="depProgramMappingId">depProgramMappingId</param>
        /// <returns>Boolean</returns>
        public Boolean IsProgramMapped(Int32 depProgramMappingId)
        {
            //return ClientSecurityManager.IsProgramMapped(View.TenantId, depProgramMappingId);
            return false;
        }
        #endregion

        public Int32 GetTenantId()
        {
            var organizationDetail = SecurityManager.GetOrganization(View.ViewContract.OrganizationID);
            return Convert.ToInt32(organizationDetail.TenantID);
        }

        #region Private Methods
        /// <summary>
        /// Get list of selected paymentoptions
        /// </summary>
        /// <param name="depProgramMappingId">depProgramMappingId</param>
        /// <returns></returns>
        private List<Entity.ClientEntity.DeptProgramPaymentOption> GetPaymentOptionList(Int32 depProgramMappingId)
        {
    
            List<Entity.ClientEntity.DeptProgramPaymentOption> tempPaymentOptionList = new List<Entity.ClientEntity.DeptProgramPaymentOption>();
            for (int i = 0; i < View.SelectedPaymentOptionIds.Count; i++)
            {
                Entity.ClientEntity.DeptProgramPaymentOption tempPaymentOption = new Entity.ClientEntity.DeptProgramPaymentOption();
                tempPaymentOption.DPPO_DeptProgramMappingID = depProgramMappingId;
                tempPaymentOption.DPPO_PaymentOptionID = View.SelectedPaymentOptionIds[i];
                tempPaymentOption.DPPO_IsDeleted = false;
                tempPaymentOption.DPPO_CreatedOn = DateTime.Now;
                tempPaymentOption.DPPO_CreatedByID = View.CurrentUserId;
                tempPaymentOptionList.Add(tempPaymentOption);
            }
            return tempPaymentOptionList;
 
        }
        #endregion

        #endregion
    }
}




