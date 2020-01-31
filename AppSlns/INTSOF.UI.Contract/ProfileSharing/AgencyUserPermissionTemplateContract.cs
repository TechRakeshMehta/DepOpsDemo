using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Entity.ClientEntity;
using System.Runtime.Serialization;
using INTSOF.Utils;
namespace INTSOF.UI.Contract.ProfileSharing
{
     [Serializable]
    public class AgencyUserPermissionTemplateContract
    {

         public Int32 AGUPT_ID { get; set; }
         public String AGUPT_Name { get; set; }
         public String AGUPT_Description { get; set; }
         public Boolean AGUPT_IsDeleted { get; set; }
         public DateTime AGUPT_CreatedOn {get;set;}
         public Int32 AGUPT_CreatedBy {get;set;}
         public Int32 AGUPT_ModifiedBy {get;set;}
         public DateTime AGUPT_ModifiedOn { get; set; }
         public Int32 TotalCount { get; set; }
         [DataMember]
         public CustomPagingArgsContract GridCustomPagingArguments { get; set; }
         public List<Int32> lstApplicationInvitationMetaDataID { get; set; }
         public Int32? AGUPT_ComplianceSharedInfoTypeID { get; set; }
         public Int32? AGUPT_BkgSharedInfoTypeID { get; set; }
         public Int32? AGUPT_ReqRotationSharedInfoTypeID { get; set; }
         public List<Int32> lstInvitationSharedInfoTypeID { get; set; }

    }
}
