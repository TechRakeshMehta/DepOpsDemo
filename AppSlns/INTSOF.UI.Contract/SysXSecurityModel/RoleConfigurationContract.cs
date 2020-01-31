using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SysXSecurityModel
{
    public class RoleConfigurationContract
    {
        public Int32 RPTS_ID
        {
            get;
            set;
        }
        public String RPTS_RoleID
        {
            get;
            set;
        }

        public Boolean RPTS_IsAllowPreferredTenant
        {
            get;
            set;
        }
        public Boolean RPTS_IsAllowDataEntry
        {
            get;
            set;
        }
        public Boolean RPTS_IsAllowComplianceVerfication
        {
            get;
            set;
        }
        public Boolean RPTS_IsAllowRotationVerfication
        {
            get;
            set;
        }
        public Boolean RPTS_IsAllowLocationEnroller
        {
            get;
            set;
        }

        public String RoleName
        {
            get;
            set;
        }

        public String RoleDescription
        {
            get;
            set;
        }

        public String _IsAllowPreferredTenantExp
        { get; set; }
        public String _IsAllowDataEntryExp
        { get; set; }
        public String _IsAllowComplianceVerficationExp
        { get; set; }
        public String _IsAllowRotationVerficationExp
        { get; set; }
        public String _IsAllowLocationEnrollerExp
        { get; set; }
    }
}
