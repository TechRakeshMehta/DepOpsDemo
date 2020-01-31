using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.Mobility
{
    [Serializable]
    public class CmplnceItemMappingDetailContract
    {
        public Int32? ComplanceItemMappingDetailID
        {
            get;
            set;
        }

        //public Int32? FromTenantID
        //{
        //    get;
        //    set;
        //}

        //public String FromTenantName
        //{
        //    get;
        //    set;
        //}

        //public Int32? ToTenantID
        //{
        //    get;
        //    set;
        //}

        //public String ToTenantName
        //{
        //    get;
        //    set;
        //}

        public Int32? FromItemID
        {
            get;
            set;
        }

        public String FromItemName
        {
            get;
            set;
        }

        public Int32? ToItemID
        {
            get;
            set;
        }

        public String ToItemName
        {
            get;
            set;
        }

        public Int32? FromAttributeID
        {
            get;
            set;
        }

        public String FromAttributeName
        {
            get;
            set;
        }

        public Int32? ToAttributeID
        {
            get;
            set;
        }

        public String ToAttributeName
        {
            get;
            set;
        }

        public Boolean IsMappingAlreadyExists
        {
            get;
            set;
        }

        public Boolean IsDeleted
        {
            get;
            set;
        }

        public Int32 PackageMappingMasterId
        {
            get;
            set;
        }

        public Boolean isSaveNeeded
        {
            get;
            set;
        }
    }
}
