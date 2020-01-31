using System;
using System.Collections.Generic;
using System.Text;
using Entity;
using INTSOF.UI.Contract.Messaging;

namespace CoreWeb.Messaging.Views
{
    public interface ITransferRulesMaintenanceFormView
    {
        List<Tenant> Institutions
        {
            get;
            set;
        }

        List<OrganizationLocation> InstitutionLocations
        {
            get;
            set;
        }

        //Commented by Sachin Singh for flexible hierarchy.
        //List<AdminProgramStudy> InstitutionPrograms
        //{
        //    set;
        //    get;
        //}

        List<lkpMessageFolder> FolderList
        {
            set;
        }
        Int32? InstitutionId
        {
            set;
            get;
        }
        Int32? LocationId
        {
            set;
            get;
        }
        Int32? ProgramId
        {
            set;
            get;
        }
        Int32 CurrentUserID
        {
            get;
        }
        Int32 UserGroupID
        {
            get;
        }

        MessagingRulesContract ViewContract
        {
            get;
            set;
        }
        Dictionary<Int32, String> MessageFromUsers
        {
            get;
            set;
        }
        Int32 TenantID
        {
            get;
            set;
        }
    }
}




