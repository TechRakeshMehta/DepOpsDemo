using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    public class SupplementServicesInformation
    {
        public Int32 OrderId { get; set; }
        public Int32 ServiceId { get; set; }
        public String ServiceName { get; set; }
        public Int32 PackageID { get; set; }
        public Int32 PackageServiceId { get; set; }
    }

    [Serializable]
    public class SupplementServiceItemInformation
    {
        public Int32 OrderId { get; set; }
        public Int32 ServiceItemId { get; set; }
        public String ServiceItemName { get; set; }
        public Int32 ServiceId { get; set; }
    
    }

    [Serializable]
    public class SupplementServiceItemCustomForm
    {
        public Int32 CustomFormID { get; set; }
        public Int32 ServiceItemID { get; set; }
        public String ServiceItemName { get; set; }
        public String CustomFormName { get; set; }
        public List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }
        public Int32 groupId { get; set; }
        public Int32 ServiceId { get; set; }
        public Int32 instanceId { get; set; }
        public Int32 PackageServiceId { get; set; }
        #region UAT-2062:
        public List<SupplementAdditionalSearchContract> LstSupplementAdditionalSearchDataForName { get; set; }
        public List<SupplementAdditionalSearchContract> LstSupplementAdditionalSearchDataForLocation { get; set; }
        #endregion

    }

    [Serializable]
    public class SessionForSupplementServiceCustomForm 
    {
        public List<SupplementServiceItemCustomForm> lstCustomFormLst { get;set; }

        public List<Int32> lstServiceItemId { get; set; }

        public List<Int32> LstSupplementServiceId { get; set; }

        #region UAT-2118:Show Nationwide Criminal Vendor Service Result if there was a flag on the initial searches
        public List<String> lstFlaggedAndNotParshedResultData { get; set; }
        #endregion
    }


}
