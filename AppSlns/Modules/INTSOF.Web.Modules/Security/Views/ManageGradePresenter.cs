
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
    public class ManageGradePresenter : Presenter<IManageGradeView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IIntsofSecurityModelController _controller;
        // public ManageGradePresenter([CreateNew] IIntsofSecurityModelController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void DeleteGradeDetails()
        {
            //lkpGradeLevel Grade = SecurityManager.GetGrade(View.ViewContract.Id);
            //View.ViewContract.Description = Grade.Description;
            //Grade.DeleteFlag = true;
            ////View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.GRADE) + SysXUtils.GetMessage(ResourceConst.SPACE) + Grade.Description.Split(new char[] { '_' }).FirstOrDefault() + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.DELETED_SUCCESSFULLY);
            //Grade.Description = Grade.Description + "_" + Guid.NewGuid();
            //if (SecurityManager.DeleteGrade(Grade))
            //{
            //    Entity.ClientEntity.lkpGradeLevel clientExistingGrade = ClientSecurityManager.GetGradeLevel(View.TenantId, Grade.GradeLevelID);
            //    clientExistingGrade.DeleteFlag = Grade.DeleteFlag;
            //    clientExistingGrade.Description = Grade.Description;
            //    ClientSecurityManager.UpdateGradeLevel(View.TenantId);
            //    View.SuccessMessage = String.Format("Grade {0} deleted successfully", View.ViewContract.Description);
            //}

        }

        /// <summary>
        /// Retrieves a list of all Grades with it's details.
        /// </summary>
        public void RetrievingGradeDetails()
        {
            //var organizationDetail = SecurityManager.GetOrganization(View.OrganizationId);
            //View.ParentOrganizationId = Convert.ToInt32(organizationDetail.ParentOrganizationID);
            //View.AllGrades = SecurityManager.GetGradeListByOrganizationId(View.ParentOrganizationId);


        }

        /// <summary>
        /// Retrieves a list of all Grade Groups with it's details.
        /// </summary>
        //public List<lkpGradeLevelGroup> RetrievingGradeGroups()
        //{
        //    View.AllGradeGroups = LookupManager.GetLookUpData<Entity.lkpGradeLevelGroup>();
        //    //View.AllGradeGroups = SecurityManager.GetGradeGroups();
        //    return View.AllGradeGroups;
        //}

        /// <summary>
        /// Performs an insert operation for role.
        /// </summary>
        public void GradeSave()
        {

            //var GradeId = View.ViewContract.Id;

            //if (SecurityManager.IsGradeExists(View.ViewContract.Description, View.ParentOrganizationId, null))
            //{
            //    View.ErrorMessage = View.ViewContract.Description + " " + "Grade already exist.";
            //}
            //else
            //{
            //    View.ErrorMessage = String.Empty;
            //    lkpGradeLevel Grade = new lkpGradeLevel
            //    {
            //        Description = View.ViewContract.Description,
            //        GradeLevelGroupDescription = View.ViewContract.GredeLevelGroupDescription,
            //        OrganizationID = View.ParentOrganizationId,
            //        //GradeLevelGroupID = View.ViewContract.GradeLevelGroupID,
            //        //SEQ = View.ViewContract.SEQ,
            //        DeleteFlag = false
            //    };
            //    SecurityManager.AddGrade(Grade);
            //    if (Grade.GradeLevelID != 0)
            //    {
            //        Entity.ClientEntity.lkpGradeLevel tempGradeLevel = new Entity.ClientEntity.lkpGradeLevel();
            //        tempGradeLevel.GradeLevelID = Grade.GradeLevelID;
            //        tempGradeLevel.Description = Grade.Description;
            //        tempGradeLevel.GradeLevelGroupDescription = Grade.GradeLevelGroupDescription;
            //        tempGradeLevel.OrganizationID = Grade.OrganizationID;
            //        tempGradeLevel.DeleteFlag = Grade.DeleteFlag;
            //        ClientSecurityManager.AddGrade(View.TenantId, tempGradeLevel);
            //        //View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.GRADE) + SysXUtils.GetMessage(ResourceConst.SPACE) + Grade.Description.Split(new char[] { '_' }).FirstOrDefault() + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
            //        View.SuccessMessage = String.Format("Grade {0} saved successfully", View.ViewContract.Description);
            //    }
            //    else
            //    {
            //        View.ErrorMessage = "Some error occured.Please try again.";
            //    }
            //}
        }

        /// <summary>
        ///  Performs an update operation for role details.
        /// </summary>
        public void GradeUpdate()
        {
            //var GradeId = View.ViewContract.Id;
            //lkpGradeLevel ExistingGrade = SecurityManager.GetGrade(GradeId);

            //if (SecurityManager.IsGradeExists(View.ViewContract.Description, View.ParentOrganizationId, ExistingGrade.Description))
            //{
            //    View.ErrorMessage = View.ViewContract.Description + " " + "Grade already exist.";
            //}
            //else
            //{
            //    View.ErrorMessage = String.Empty;

            //    ExistingGrade.Description = View.ViewContract.Description;
            //    ExistingGrade.GradeLevelGroupDescription = View.ViewContract.GredeLevelGroupDescription;
            //    //ExistingGrade.GradeLevelGroupID = View.ViewContract.GradeLevelGroupID;
            //    //ExistingGrade.SEQ = View.ViewContract.SEQ;
            //    ExistingGrade.DeleteFlag = false;
            //    if (SecurityManager.UpdateGrade(ExistingGrade))
            //    {
            //        Entity.ClientEntity.lkpGradeLevel clientExistingGrade = ClientSecurityManager.GetGradeLevel(View.TenantId, GradeId);
            //        clientExistingGrade.Description = ExistingGrade.Description;
            //        clientExistingGrade.GradeLevelGroupDescription = ExistingGrade.GradeLevelGroupDescription;
            //        //ExistingGrade.GradeLevelGroupID = View.ViewContract.GradeLevelGroupID;
            //        //ExistingGrade.SEQ = View.ViewContract.SEQ;

            //        ClientSecurityManager.UpdateGradeLevel(View.TenantId);
            //        // View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.GRADE) + SysXUtils.GetMessage(ResourceConst.SPACE) + ExistingGrade.Description.Split(new char[] { '_' }).FirstOrDefault() + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
            //        View.SuccessMessage = String.Format("Grade {0} updated successfully", View.ViewContract.Description);
            //    }
            //    else
            //    {
            //        View.ErrorMessage = "Some error occured.Please try again.";
            //    }
            //}
        }

        /// <summary>
        /// Check for Grade Linked or not.
        /// </summary>
        /// <param name="gradeLevelId">gradeLevelId</param>
        /// <returns>Boolean</returns>
        public Boolean IsGradeLinked(Int32 gradeLevelId)
        {
           // return SecurityManager.GetAllPrograms(View.ParentOrganizationId).Where(cond => cond.GradeLevelID == gradeLevelId).Count() > AppConsts.NONE;
            return false;
        }
    }
}




