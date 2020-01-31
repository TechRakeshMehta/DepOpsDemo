using Entity;
using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.Shell.Views
{
    public interface IAdditionalAccountVerification
    {
        Int32 TenantID { get; set; }
        Int32 OrganizationUserID { get; set; }
        Guid UserId { get; }
        String UsrVerCode { get; set; }
        Entity.OrganizationUser OrganizationUser { get; set; }


        Boolean AccountVerificationMainSetting { get; set; }

        Boolean AccVerificationProcessResponseReqdSetting { get; set; }

        Boolean AccVerificationProcessDOBSetting { get; set; }

        Boolean AccVerificationProcessSSNSetting { get; set; }

        Boolean AccVerificationProcessLSSNSetting { get; set; }

        Boolean AccVerificationProcessProfCustAttrSetting { get; set; }

        String AccVerificationProcessDOBTextSetting { get; set; }

        String AccVerificationProcessSSNTextSetting { get; set; }

        String AccVerificationProcessLSSNTextSetting { get; set; }

        String AccVerificationProcessProfCustAttrTextSetting { get; set; }

        List<Entity.ClientEntity.lkpSetting> LstSettings { get; set; }

        List<TypeCustomAttributes> LstProfileAttr { get; set; }

        List<TypeCustomAttributes> lstCustomAttrUserData { get; set; }
    }
}
