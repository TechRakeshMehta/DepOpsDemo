using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IRowControlView
    {
        /// <summary>
        /// Id of the Master compliance Item (Client database), present in the ApplicantComplianceItem Entity. 
        /// </summary>
        Int32 ItemId { get; set; }

        Int32 TenantId { get; set; }
        Int32 NoOfAttributesPerRow { get; set; }
        IRowControlView CurrentViewContext { get; }
        Int32 ApplicantComplianceItemId { get; set; }
        List<ApplicantComplianceAttributeData> ApplicantAttributeData { get; set; }
        List<ComplianceItemAttribute> ClientItemAttributes { get; set; }

        //Implemented code for UAT-708
        Int32 PackageId
        {
            get;
            set;
        }
        Boolean IsItemSeries
        {
            get;
            set;
        }
        //Implemented code for UAT-708
        Int32 CategoryId
        {
            get;
            set;
        }

        //UAT-4067
         List<String> lstAllowedExtensions { get; set; }
    }
}




