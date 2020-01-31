using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IInstitutionNodesView
    {
        /// <summary>
        /// OrganizationUserId of the current logged in User
        /// </summary>
        Int32? CurrentUserId
        {
            get;
        }

        /// <summary>
        /// Selected TenantId
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        IInstitutionNodesView CurrentViewContext
        {
            get;
        }

        Dictionary<Int32, String> dicNodes
        {
            set;
        }
    }
}
