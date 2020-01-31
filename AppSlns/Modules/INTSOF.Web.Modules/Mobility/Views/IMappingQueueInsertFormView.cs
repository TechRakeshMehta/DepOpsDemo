using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.Mobility.Views
{
    public interface IMappingQueueInsertFormView
    {
        Int32 TenantId { get; set; }
        List<Entity.Tenant> lstTenant { get; set; }
        Int32 CurrentLoggedInUserId { get; }
         Boolean IsMappingSkip
        {
            get;
            set;
        }

        Int32 SourceNodeId
        {
            get;
            set;
        }
        Int32 TargetNodeId
        {
            get;
            set;
        }
        List<Entity.ClientEntity.DeptProgramPackage> lstPackage
        {
            get;
            set;
        }
        Int32 SelectedSourcePackageId
        {
            get;
            set;
        }


        Int32 SelectedTargetPackageId
        {
            get;
            set;

        }

        Int32 SelectedSourceNewMappingTenantId
        {
            get;
            set;
        }

        Int32 SelectedTargetNewMappingTenantId
        {
            get;
            set;
        }
        String MappingName { get; set; }
        String SelectedSourceNewMappingPackage { get; set; }
        String SelectedTargetNewMappingPackage { get; set; }
        String ErrorMessage { get; set; }
        Boolean IsComboItemAdded { get; set; }
        String SuccessMessage { get; set; }
        String InfoMessage { get; set; }
    }
}




