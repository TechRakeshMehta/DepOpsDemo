using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ContractManagement
{
    /// <summary>
    /// Contract class for ContractContact entity
    /// </summary>
    [Serializable]
    public class ContactContract
    {
        public Int32 TempContactId { get; set; }

        public Int32 ContactId { get; set; }

        /// <summary>
        /// MappingId of the Contract and ContractContact i.e. CCM_ID of 'ContractContactMapping' table
        /// </summary>
        public Int32 ContractContactMappingId { get; set; }

        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
        public String Title { get; set; }

        /// <summary>
        /// 'True' if the Contact is deleted temporariy i.e. the final save is not done.
        /// </summary>
        public Boolean IsTempDeleted { get; set; }
        //UAT-2447
        public Boolean IsInternationalPhone { get; set; }
    }
}
