using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    /// <summary>
    /// Contract to represent the Service forms associated with a service and their Dispatch type mode
    /// either Manual or Electronic, at the Root(Form) level
    /// Service level(Depending on the Mapping type to be 'AllInstitution' or 'InstitutionSpecific' ) and Package Service Mapping level
    /// </summary>
    public class ServiceFormsDispatchTypesContract
    {
        public String ServiceFormName { get; set; }

        /// <summary>
        /// Value of SF_SendAutomatically column in ams.ServiceAttachedForm table
        /// </summary>
        public Boolean IsRootLevelAuto { get; set; }

        /// <summary>
        /// Pk of the ams.BkgServiceAttachedFormMapping table in Security i.e. SFM_ID
        /// </summary>
        public Int32 ServiceAttachedFormMappingId { get; set; }

        /// <summary>
        /// PK of the 'ams.BkgPackageSvcFormOverride' table i.e. BPSO_ID
        /// </summary>
        public Int32 BPSOId { get; set; }

        /// <summary>
        /// lkpMappingTypeCode based on SFM_MapopingTypeID the in ams.BkgServiceAttachedFormMapping table 
        /// </summary>
        public String MappingTypeCode { get; set; }

        /// <summary>
        /// Value of the SFMO_EnforceManual in '[ams].[BkgServiceAttachedFormMappingOverride]' table in Security database 
        /// OR
        /// Value of the SFHMO_EnforceManual in '[ams].[BkgServiceAttachedFormHierarchyMappingOverride]' table in Tenant database, 
        /// when Mapping Type code is 'InstitutionSpecific'
        /// </summary>
        public Boolean? EnforceManual { get; set; }

        /// <summary>
        /// Value of the 'BPSO_IsAutomatic' column in the 'ams.BkgPackageSvcFormOverride' table, when the Mapping type code is 'InstitutionSpecific'
        /// </summary>
        public Boolean? IsPackageLevelAutomatic { get; set; }

        /// <summary>
        /// Value of 'BPSO_HideServiceForm' column in 'BkgPackageSvcFormOverride' table
        /// </summary>
        public Boolean HideServiceForm { get; set; }
    }
}
