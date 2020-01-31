using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.Mobility
{
    public class MobilitySearchDataContract
    {

        public List<Int32> StatusIDList { get; set; }
        public String Status
        {
            get;
            set;
        }

        private string _applicantFirstName = String.Empty;
        public String ApplicantFirstName
        {
            get
            {
                return _applicantFirstName;
            }
            set
            {
                if (value.IsNotNull())
                {
                    var countAposNo = value.Split('\'').Length - 1;
                    if (countAposNo > 0)
                    {
                        _applicantFirstName = (value.Replace("'", "''"));
                    }
                }
            }
        }

        public String ApplicantMiddleName
        {
            get;
            set;
        }

        private string _applicantLastName = String.Empty;
        public String ApplicantLastName
        {
            get
            {
                return _applicantLastName;
            }
            set
            {
                if (value.IsNotNull())
                {
                    var countAposNo = value.Split('\'').Length - 1;
                    if (countAposNo > 0)
                    {
                        _applicantLastName = (value.Replace("'", "''"));
                    }
                }
            }
        }

        public Int32? SourceTenantId
        {
            get;
            set;
        }

        public Int32? TargetTenantId
        {
            get;
            set;
        }

        public Int16? StatusID
        {
            get;
            set;
        }

        public String SourcePackage
        {
            get;
            set;
        }

        public String TargetPackage
        {
            get;
            set;
        }
        public List<String> LstStatusCode
        {
            get;
            set;
        }

        public DateTime? TransitionDate
        {
            get;
            set;
        }

        public Int32? SourceNodeId
        {
            get;
            set;
        }

        public String SourceNodeLabel { get; set; }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPagingArguments
        {
            get;
            set;
        }

        public Int32? UserGroupID { get; set; }

        public String SourceNodeIds
        {
            get;
            set;
        }

        //UAT-2258
        public String SourceTenantIds { get; set; }
    }
}
