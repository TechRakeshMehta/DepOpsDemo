using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.Templates;

namespace CoreWeb.SystemSetUp.Views
{
    public interface ICreateAccountInvitationView
    {
        Int32 CurrentLoggedInUserId
        {
            get;
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }

        //List<AccountInvitationContract> AccountInvitationList { get; set; }

        List<AccountInvitationContract> AccountInvitationTempList { get; set; }

        String UserFirstName { get; set; }

        String UserLastName { get; set; }

        String UserEmail { get; set; }

        Boolean HasDuplicateNames { get; }

        List<Tenant> LstTenant { get; set; }

        List<Entity.CommunicationTemplatePlaceHolder> TemplatePlaceHolders { get; set; }

        SystemEventTemplatesContract SystemEventTemplate { get; set; }

        Int32 TemplateId { get; set; }

        Int32 SubEventID { get; set; }

        Int32 TenantID { get; set; }
    }
}
