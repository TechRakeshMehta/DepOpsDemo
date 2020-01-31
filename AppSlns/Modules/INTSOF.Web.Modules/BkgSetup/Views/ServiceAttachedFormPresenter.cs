using System;
using System.Collections.Generic;
using System.Linq;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class ServiceAttachedFormPresenter : Presenter<IServiceAttachedFormView>
    {
        #region Methods

        #region Public Methods

        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetTenants()
        {
            //View.ListTenants = ComplianceDataManager.getClientTenant();
        }

        /// <summary>
        /// Gets all the service groups for the given tanent ID.
        /// </summary>
        public void GetServiceAttachedForm()
        {
            View.ListServiceAttachedForm = BackgroundSetupManager.GetServoceAttachedFormList(View.TenantId);
        }

        public void GetParentServiceAttachedForm()
        {
            List<Entity.ServiceAttachedForm> tempList = BackgroundSetupManager.GetParentServiceattachedForm(View.TenantId).OrderBy(ordBy => ordBy.SF_Name).ToList();
            if (!(tempList.Count > 0))
            {
                tempList = new List<Entity.ServiceAttachedForm>();
            }
            tempList.Insert(0, new Entity.ServiceAttachedForm { SF_Name = "--SELECT--", SF_ID = 0 });
            View.LstParentServiceAttachedForm = tempList;
        }

        public void BindTemplatePlaceHolders()
        {
            List<String> lstValues = Enum.GetNames(typeof(CommunicationTemplatePlaceHoldersProperty)).Cast<String>().ToList();
            View.TemplatePlaceHolders = CommunicationManager.GetTemplatePlaceHolders(lstValues);
        }

        ///// <summary>
        ///// Saves the service groups in the MasterDB followed by ClientDB(in case of client).
        ///// </summary>
        public void SaveServiceAttachedForm()
        {
            Entity.ServiceAttachedForm newServiceAttachedForm = new Entity.ServiceAttachedForm();
            newServiceAttachedForm.SF_Name = View.FormName;
            newServiceAttachedForm.SF_IsDeleted = false;
            newServiceAttachedForm.SF_CreatedBy = View.CurrentLoggedInUserId;
            newServiceAttachedForm.SF_CreatedOn = DateTime.Now;

            /*Service Form Type : Form Version-0 : New Form-1 */
            if (View.ServiceFormType == AppConsts.NONE)
            {
                newServiceAttachedForm.SF_ParentServiceFormID = View.ParentFormId;
                newServiceAttachedForm.SystemDocument = View.SystemDocumentToSaveUpdate;
                newServiceAttachedForm.SF_SendAutomatically = true;
            }

            if (View.ServiceFormDispatchType.IsNotNull() && View.ServiceFormDispatchType == Convert.ToString(DispatchType.Automatic))
            {
                newServiceAttachedForm.SF_SendAutomatically = true;
                newServiceAttachedForm.SystemDocument = View.SystemDocumentToSaveUpdate;
            }
            else if (View.ServiceFormDispatchType.IsNotNull() && View.ServiceFormDispatchType == Convert.ToString(DispatchType.Manual))
            {
                newServiceAttachedForm.SF_SendAutomatically = false;
            }

            BackgroundSetupManager.SaveServiceAttachedForm(View.TenantId, newServiceAttachedForm);

            View.SuccessMessage = "Service Form added successfully.";

            //If Service Form Type is Form Version, then insert data into CommunicationTemplate Entity data and CommunicationTemplateID should be same of ParentServiceFormID
            if (View.ServiceFormType == AppConsts.NONE)
            {
                InsertCommunicationTemplateEntityDataFromParentSvcForm(newServiceAttachedForm.SF_ID, View.ParentFormId);
                View.SuccessMessage = "Service Form Version added successfully";
            }
            else if (View.ServiceFormType == AppConsts.ONE && View.ServiceFormDispatchType == Convert.ToString(DispatchType.Automatic))
            {
                try
                {
                    List<Entity.lkpCommunicationSubEvent> commSubEventsList = new List<Entity.lkpCommunicationSubEvent>();

                    Entity.lkpCommunicationSubEvent commSubEvents = AddCommunicationSubEvents(false, newServiceAttachedForm.SF_ID);
                    AddCommunicationTemplatePlaceHolderSubEvents(commSubEvents);
                    AddCommunicationTemplate(commSubEvents, newServiceAttachedForm.SF_ID, false);

                    Entity.lkpCommunicationSubEvent reminderCommSubEvents = AddCommunicationSubEvents(true, newServiceAttachedForm.SF_ID);
                    AddCommunicationTemplatePlaceHolderSubEvents(reminderCommSubEvents);
                    AddCommunicationTemplate(reminderCommSubEvents, newServiceAttachedForm.SF_ID, true);

                    commSubEventsList.Add(commSubEvents);
                    commSubEventsList.Add(reminderCommSubEvents);

                    CommunicationManager.SaveServiceAttachedFormCommunicationTemplate(commSubEventsList);
                    View.SuccessMessage = "Service Form and its related Communication Sub-Events and Communication Templates added successfully.";
                }
                catch (Exception ex)
                {
                    View.ErrorMessage = "Service Form added successfully. But Some error occurred while adding Communication Sub-Events and Communication Templates." +
                                        " Please Edit/Update Service Form to add Communication Sub-Events and Communication Templates OR contact System Administrator.";
                }
            }
        }

        /// <summary>
        /// Eidt/Update Service Attached Form
        /// </summary>
        public void UpdateServiceAttachedForm()
        {
            Entity.ServiceAttachedForm updateServiceForm = BackgroundSetupManager.GetServiceAttachedFormByID(View.TenantId, View.SF_ID);
            if (updateServiceForm.IsNotNull())
            {
                //updateServiceForm.SF_ID = View.SF_ID;
                updateServiceForm.SF_Name = View.FormName;
                updateServiceForm.SF_IsDeleted = false;
                updateServiceForm.SF_ModifiedBy = View.CurrentLoggedInUserId;
                updateServiceForm.SF_ModifiedOn = DateTime.Now;

                /*Service Form Type : Form Version-0 : New Form-1 */

                //If Service Form Type is 'Form Version' then user can edit Parent Form and if new service form document is uploaded 
                //then System will uplaod document and make new entry in SystemDocument table.
                if (View.ServiceFormType == AppConsts.NONE)
                {
                    updateServiceForm.SF_ParentServiceFormID = View.ParentFormId;
                    if (View.SystemDocumentToSaveUpdate.IsNotNull() && !(View.SystemDocumentToSaveUpdate.FileName.IsNullOrEmpty()))
                    {
                        updateServiceForm.SystemDocument = View.SystemDocumentToSaveUpdate;
                    }
                }

                //If Service Form Type is 'New Form' and Dispatch Type is Automatic then user can upload new service form document,
                //System will uplaod document and make new entry in SystemDocument table.
                if (View.ServiceFormType == AppConsts.ONE && View.ServiceFormDispatchType.IsNotNull()
                    && View.ServiceFormDispatchType == Convert.ToString(DispatchType.Automatic))
                {
                    if (View.SystemDocumentToSaveUpdate.IsNotNull() && !(View.SystemDocumentToSaveUpdate.FileName.IsNullOrEmpty()))
                    {
                        updateServiceForm.SystemDocument = View.SystemDocumentToSaveUpdate;
                    }
                }

                //Update service form data in security database.
                BackgroundSetupManager.UpdateServiceAttachedForm(View.TenantId);
                View.SuccessMessage = "Service Form updated successfully.";


                //Update Service Form related Communication data in Messaging database. 
                if (View.ServiceFormType == AppConsts.NONE)
                {
                    UpdateCommunicationTemplateEntityData(updateServiceForm.SF_ID, View.ParentFormId);
                    View.SuccessMessage = "Service Form Version updated successfully";
                }
                else if (View.ServiceFormType == AppConsts.ONE && View.ServiceFormDispatchType == Convert.ToString(DispatchType.Automatic))
                {
                    try
                    {
                        Entity.CommunicationTemplate svcFormCommunicationTemplateToUpdate = CommunicationManager.GetCommunicationTemplateData(View.SvcFormCommunicationTemplateID);
                        if (svcFormCommunicationTemplateToUpdate.IsNotNull())
                        {
                            svcFormCommunicationTemplateToUpdate.Name = View.TemplateName;
                            svcFormCommunicationTemplateToUpdate.Subject = View.TemplateSubject;
                            svcFormCommunicationTemplateToUpdate.Content = View.TemplateContent;
                            svcFormCommunicationTemplateToUpdate.LanguageId = View.SelectedLanguageId;
                        }

                        Entity.CommunicationTemplate reminderCommunicationTemplateToUpdate = CommunicationManager.GetCommunicationTemplateData(View.ReminderCommunicationTemplateID);
                        if (reminderCommunicationTemplateToUpdate.IsNotNull())
                        {
                            reminderCommunicationTemplateToUpdate.Name = View.ReminderTemplateName;
                            reminderCommunicationTemplateToUpdate.Subject = View.ReminderTemplateSubject;
                            reminderCommunicationTemplateToUpdate.Content = View.ReminderTemplateContent;
                            reminderCommunicationTemplateToUpdate.LanguageId = View.SelectedLanguageId;
                        }

                        CommunicationManager.UpdateServiceAttachedFormCommunicationTemplate();
                        View.SuccessMessage = "Service Form and Communication Templates updated successfully.";
                    }
                    catch (Exception ex)
                    {
                        View.ErrorMessage = "Service Form updated successfully. But Some error occurred while updating Communication Templates." +
                                            " Please try again OR contact System Administrator.";
                    }
                }
            }
        }

        public void GetBkgServiceAttachedFormMappingByServiceFormID()
        {
            var bkgServiceAttachedFormMapping = BackgroundSetupManager.GetBkgServiceAttachedFormMappingByServiceFormID(View.TenantId, View.SF_ID);

            if (bkgServiceAttachedFormMapping.IsNotNull() && bkgServiceAttachedFormMapping.Count() > AppConsts.NONE)
            {
                View.IsBkgServiceAttachedFormMappingExists = true;
            }
        }

        public void GetServiceFormCommunicationTemplateData()
        {
            View.ServiceFormCommunicationTemplateData = CommunicationManager.GetCommunicationTemplateEntityData(View.SF_ID).ToList();
        }

        public Boolean CheckIfServiceAttachedFormNameAlreadyExist()
        {
            return BackgroundSetupManager.CheckIfServiceAttachedFormNameAlreadyExist(View.TenantId, View.FormName, View.SF_ID, View.IsUpdate);
        }

        ///// <summary>
        ///// Deletes the service groups.
        ///// </summary>
        public void DeleteServiceAttachedForm()
        {
            Entity.ServiceAttachedForm serviceAttachedFormToUpdate = BackgroundSetupManager.GetServiceAttachedFormByID(View.TenantId, View.SF_ID);
            if (serviceAttachedFormToUpdate.IsNotNull())
            {
                Boolean IsServiceAttachedFormVersionsDeleted = false;
                try
                {
                    //UAT-2480
                    Boolean VersionServiceFormToDelete = true;
                    if (!serviceAttachedFormToUpdate.SF_ParentServiceFormID.IsNullOrEmpty())
                    {
                        VersionServiceFormToDelete = false;
                    }
                    else
                    {
                        //Boolean IsServiceAttachedFormVersionsDeleted = BackgroundSetupManager.IsServiceAttachedFormVersionsDeleted(View.TenantId, serviceAttachedFormToUpdate.SF_ID, serviceAttachedFormToUpdate.SF_ParentServiceFormID.IsNull() ? 0 : serviceAttachedFormToUpdate.SF_ParentServiceFormID.Value);
                        IsServiceAttachedFormVersionsDeleted = BackgroundSetupManager.IsServiceAttachedFormVersionsDeleted(View.TenantId, serviceAttachedFormToUpdate.SF_ID);
                    }


                    if (!serviceAttachedFormToUpdate.SF_ParentServiceFormID.IsNullOrEmpty() || (serviceAttachedFormToUpdate.SF_ParentServiceFormID.IsNullOrEmpty() && IsServiceAttachedFormVersionsDeleted))
                    {
                        CommunicationManager.DeleteServiceAttachedFormCommunicationData(View.SF_ID, View.CurrentLoggedInUserId, VersionServiceFormToDelete);
                        serviceAttachedFormToUpdate.SF_IsDeleted = true;
                        serviceAttachedFormToUpdate.SF_ModifiedBy = View.CurrentLoggedInUserId;
                        serviceAttachedFormToUpdate.SF_ModifiedOn = DateTime.Now;
                        BackgroundSetupManager.UpdateServiceAttachedForm(View.TenantId);
                        View.SuccessMessage = "Servcie Form Deleted successfully.";
                    }
                    else
                    {
                        View.IsActiveVersionsPresent = true;
                        //  " You cannot delete this service form because it has active child version(s).";
                        //return;
                    }

                }
                catch (Exception ex)
                {
                    View.ErrorMessage = "An error occurred while deleting Service Form. Please try again or Contact System Adminstrator.";
                    return;
                }


            }
            else
            {
                View.ErrorMessage = "An error occurred while deleting Service Form. Please try again or Contact System Adminstrator.";
            }

        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }

        #endregion

        #region Private Methods

        private void AddCommunicationTemplate(Entity.lkpCommunicationSubEvent commSubEvents, Int32 newServiceAttachedFormID, Boolean isReminderTemplate)
        {
            Entity.CommunicationTemplate ct = new Entity.CommunicationTemplate();
            ct.Name = isReminderTemplate ? View.ReminderTemplateName : View.TemplateName;
            ct.Subject = isReminderTemplate ? View.ReminderTemplateSubject : View.TemplateSubject;
            ct.Content = isReminderTemplate ? View.ReminderTemplateContent : View.TemplateContent;
            ct.IsDeleted = false;
            ct.CreatedOn = DateTime.Now;
            ct.CreatedByID = View.CurrentLoggedInUserId;
            ct.LanguageId = View.SelectedLanguageId;

            Int32 communicationEntityType_BkgSvcFormID = 0;
            Int32 communicationEntityType_SvcFormNotificationReminderID = 0;

            List<Entity.lkpCommunicationEntityType> communicationEntityType = LookupManager.GetMessagingLookUpData<Entity.lkpCommunicationEntityType>();
            if (communicationEntityType.IsNotNull() && communicationEntityType.Any(cond => cond.CET_Code == CommunicationEntityType.BACKGROUND_SERVICE_FORM.GetStringValue()))
            {
                communicationEntityType_BkgSvcFormID = communicationEntityType.FirstOrDefault(cond => cond.CET_Code == CommunicationEntityType.BACKGROUND_SERVICE_FORM.GetStringValue())
                                                            .CET_ID;
            }

            if (communicationEntityType.IsNotNull() && communicationEntityType.Any(cond => cond.CET_Code == CommunicationEntityType.SERVICE_FORM_NOTIFICATION_REMINDER.GetStringValue()))
            {
                communicationEntityType_SvcFormNotificationReminderID = communicationEntityType.FirstOrDefault(cond => cond.CET_Code == CommunicationEntityType.SERVICE_FORM_NOTIFICATION_REMINDER.GetStringValue())
                                                            .CET_ID;
            }

            Entity.CommunicationTemplateEntity cte = new Entity.CommunicationTemplateEntity();
            cte.CTE_CommunicationEntityTypeID = isReminderTemplate ? communicationEntityType_SvcFormNotificationReminderID : communicationEntityType_BkgSvcFormID;
            cte.CTE_EntityID = newServiceAttachedFormID;
            cte.CTE_IsDeleted = false;
            cte.CTE_CreatedBy = View.CurrentLoggedInUserId;
            cte.CTE_CreatedOn = DateTime.Now;

            ct.CommunicationTemplateEntities.Add(cte);
            commSubEvents.CommunicationTemplates.Add(ct);
        }

        private Entity.lkpCommunicationSubEvent AddCommunicationSubEvents(Boolean isReminderTemplate, Int32 newServiceAttachedFormID)
        {
            Entity.lkpCommunicationSubEvent commSubEvents = new Entity.lkpCommunicationSubEvent();

            Int32 communicationTypeNotificationID = 0;
            Int32 communicationEventSubscriptionID = 0;
            Int32 subEventCategoryTypeAMSSystemID = 0;

            List<Entity.lkpCommunicationEvent> communicationEvent = LookupManager.GetMessagingLookUpData<Entity.lkpCommunicationEvent>();
            if (communicationEvent.IsNotNull() && communicationEvent.Any(cond => cond.Code == lkpCommunicationEventContext.SUBSCRIPTIONS.GetStringValue()))
            {
                communicationEventSubscriptionID = communicationEvent.FirstOrDefault(cond => cond.Code == lkpCommunicationEventContext.SUBSCRIPTIONS.GetStringValue())
                                                            .CommunicationEventID;
            }

            List<Entity.lkpCommunicationType> communicationType = LookupManager.GetMessagingLookUpData<Entity.lkpCommunicationType>();
            if (communicationType.IsNotNull() && communicationType.Any(cond => cond.Code == lkpCommunicationTypeContext.NOTIFICATION.GetStringValue()))
            {
                communicationTypeNotificationID = communicationType.FirstOrDefault(cond => cond.Code == lkpCommunicationTypeContext.NOTIFICATION.GetStringValue())
                                                            .CommunicationTypeID;
            }

            List<Entity.lkpSubEventCategoryType> subEventCategoryType = LookupManager.GetMessagingLookUpData<Entity.lkpSubEventCategoryType>();
            if (subEventCategoryType.IsNotNull() && subEventCategoryType.Any(cond => cond.SCT_Code == SubEventCategoryType.AMS_SYSTEM.GetStringValue()))
            {
                subEventCategoryTypeAMSSystemID = subEventCategoryType.FirstOrDefault(cond => cond.SCT_Code == SubEventCategoryType.AMS_SYSTEM.GetStringValue())
                                                            .SCT_ID;
            }

            commSubEvents.CommunicationEventID = communicationEventSubscriptionID;
            commSubEvents.CommunicationTypeID = communicationTypeNotificationID;
            commSubEvents.CategoryTypeID = Convert.ToInt16(subEventCategoryTypeAMSSystemID);
            commSubEvents.Name = isReminderTemplate ? ("Notification For " + View.FormName + " Dispatched") : ("Notification For" + View.FormName);
            commSubEvents.Code = System.Guid.NewGuid().ToString().Substring(0, 8);
            commSubEvents.IsDeleted = false;
            commSubEvents.CreatedByID = View.CurrentLoggedInUserId;
            commSubEvents.CreatedOn = DateTime.Now;
            commSubEvents.IsHierarchySpecific = false;//UAT-3348

            return commSubEvents;
        }

        private void AddCommunicationTemplatePlaceHolderSubEvents(Entity.lkpCommunicationSubEvent commSubEvents)
        {
            View.TemplatePlaceHolders.ForEach(tph =>
            {
                Entity.CommunicationTemplatePlaceHolderSubEvent ctphse = new Entity.CommunicationTemplatePlaceHolderSubEvent();
                ctphse.CommunicationTemplatePlaceHolderID = tph.CommunicationTemplatePlaceHolderID;
                commSubEvents.CommunicationTemplatePlaceHolderSubEvents.Add(ctphse);
            });
        }

        private void InsertCommunicationTemplateEntityDataFromParentSvcForm(Int32 serviceFormID, Int32 parentServcieFormID)
        {
            List<Entity.CommunicationTemplateEntity> communicationTemplateEntitiesToBeAdded = new List<Entity.CommunicationTemplateEntity>();

            //Add new Communication Template Entities by getting communication Template ID from Parent Service Form.
            IEnumerable<Entity.CommunicationTemplateEntity> communicationTemplateEntities = CommunicationManager.GetCommunicationTemplateEntityData(parentServcieFormID);

            var communicationTemplateEntity_bkgSvcForm = communicationTemplateEntities.Where(cond => cond.lkpCommunicationEntityType.CET_Code
                                                                                 == CommunicationEntityType.BACKGROUND_SERVICE_FORM.GetStringValue())
                                                                            .FirstOrDefault();
            if (communicationTemplateEntity_bkgSvcForm.IsNotNull())
            {
                Entity.CommunicationTemplateEntity newCte = new Entity.CommunicationTemplateEntity();
                newCte.CTE_CommunicationEntityTypeID = communicationTemplateEntity_bkgSvcForm.CTE_CommunicationEntityTypeID;
                newCte.CTE_EntityID = serviceFormID;
                newCte.CTE_TemplateID = communicationTemplateEntity_bkgSvcForm.CTE_TemplateID;
                newCte.CTE_IsDeleted = false;
                newCte.CTE_CreatedBy = View.CurrentLoggedInUserId;
                newCte.CTE_CreatedOn = DateTime.Now;
                communicationTemplateEntitiesToBeAdded.Add(newCte);
            }

            var communicationTemplateEntity_BkgSvcFormReminder = communicationTemplateEntities.Where(cond => cond.lkpCommunicationEntityType.CET_Code
                                                                                 == CommunicationEntityType.SERVICE_FORM_NOTIFICATION_REMINDER.GetStringValue())
                                                                           .FirstOrDefault();
            if (communicationTemplateEntity_BkgSvcFormReminder.IsNotNull())
            {
                Entity.CommunicationTemplateEntity newCte = new Entity.CommunicationTemplateEntity();
                newCte.CTE_CommunicationEntityTypeID = communicationTemplateEntity_BkgSvcFormReminder.CTE_CommunicationEntityTypeID;
                newCte.CTE_EntityID = serviceFormID;
                newCte.CTE_TemplateID = communicationTemplateEntity_BkgSvcFormReminder.CTE_TemplateID;
                newCte.CTE_IsDeleted = false;
                newCte.CTE_CreatedBy = View.CurrentLoggedInUserId;
                newCte.CTE_CreatedOn = DateTime.Now;
                communicationTemplateEntitiesToBeAdded.Add(newCte);
            }
            CommunicationManager.InsertCommunicationTemplatesEntities(communicationTemplateEntitiesToBeAdded);
        }

        private void UpdateCommunicationTemplateEntityData(Int32 serviceFormID, Int32 parentServcieFormID)
        {
            IEnumerable<Entity.CommunicationTemplateEntity> svcFormCommunicationTemplateEntities = CommunicationManager.GetCommunicationTemplateEntityData(serviceFormID);
            IEnumerable<Entity.CommunicationTemplateEntity> parentSvcFormCommunicationTemplateEntities = CommunicationManager.GetCommunicationTemplateEntityData(parentServcieFormID);

            if (svcFormCommunicationTemplateEntities.IsNotNull() && svcFormCommunicationTemplateEntities.Count() > 0)
            {
                svcFormCommunicationTemplateEntities.ForEach(cteToBeUpdated =>
                {
                    var cte = parentSvcFormCommunicationTemplateEntities.Where(cond => cond.CTE_CommunicationEntityTypeID == cteToBeUpdated.CTE_CommunicationEntityTypeID
                                                                        && cond.CTE_IsDeleted == false).FirstOrDefault();
                    cteToBeUpdated.CTE_TemplateID = cte.CTE_TemplateID;
                    cteToBeUpdated.CTE_ModifiedBy = View.CurrentLoggedInUserId;
                    cteToBeUpdated.CTE_ModifiedOn = DateTime.Now;
                });
            }
            CommunicationManager.UpdateServiceAttachedFormCommunicationTemplate();
        }

        #endregion

        #endregion

        public void GetLanguages()
        {
            View.Languages = CommunicationManager.GetLanguages();
        }
    }

}
