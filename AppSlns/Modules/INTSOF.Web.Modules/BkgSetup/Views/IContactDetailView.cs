using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IContactDetailView
    {

        Int32 TenantId { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 InstitutionContactId { get; set; }
        Int32 HierarchyNodeID { get; set; }
        Int32 SelectedTenantId { get; set; }

        String FirstName { get; set; }
        String Lastname { get; set; }
        String Title { get; set; }
        String PrimaryEmailAddress { get; set; }
        String PrimaryPhone { get; set; }
        String Address1 { get; set; }
        String Address2 { get; set; }
        Int32? ZipCodeId { get; set; }

        //Int32 CreatedById { get; set; }
        //Int32 ModifiedById { get; set; }
        //Int32 CreatedOn { get; set; }
        //Int32 ZipCodeId { get; set; }
    }
}
