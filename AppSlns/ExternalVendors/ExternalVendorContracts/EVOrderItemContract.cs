using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalVendors.ExternalVendorContracts
{
    [Serializable]
    public class EVOrderContract
    {
        #region OrderProperties

        public Int32 TransmitInd
        {
            get;
            set;
        }

        public String CurrentPersonAddress
        {
            get;
            set;
        }

        
        public Int32? ZipID
        {
            get;
            set;
        }

       
        public String PhoneNumber
        {
            get;
            set;
        }

       
        public String FirstName
        {
            get;
            set;
        }


        public String MiddleName
        {
            get;
            set;
        }


        public String LastName
        {
            get;
            set;
        }


        public DateTime? DateOfBirth
        {
            get;
            set;
        }


        public String AliasFirstName
        {
            get;
            set;
        }


        public String AliasMiddleName
        {
            get;
            set;
        }


        public String AliasLastName
        {
            get;
            set;
        }


        public String SSN
        {
            get;
            set;
        }

        public List<EvOrderItemContract> OrderItems
        {
            get;
            set;
        }

        #endregion
    }

    [Serializable]
    public class EvOrderItemContract
    {
        #region OrderItemProperties

        public String ExternalBackgroundServiceCode
        {
            get;
            set;
        }

        public String ServiceType
        {
            get;
            set;
        }

        #endregion
    }
}
