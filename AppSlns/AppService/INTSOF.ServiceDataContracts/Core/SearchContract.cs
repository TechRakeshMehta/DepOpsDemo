using System;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Core
{
    [Serializable]
    [DataContract]
    public class SearchContract
    {
        [DataMember]
        public Boolean IsBackToSearch { get; set; }
        [DataMember]
        public Boolean DisallowApostropheConversion { get; set; }

        [DataMember]
        public Int32 TenantID { get; set; }

        private String _applicantFirstName = String.Empty;
        [DataMember]
        public String ApplicantFirstName
        {

            get
            {
                return (_applicantFirstName);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var countAposNo = value.Split('\'').Length - 1;
                    if (countAposNo == 1 || countAposNo % 2 != 0)
                    {
                        if (!DisallowApostropheConversion)
                        {
                            //Even
                            _applicantFirstName = (value.Replace("'", "''"));
                        }
                        else
                        {
                            _applicantFirstName = value;
                        }

                    }
                    else if (countAposNo % 2 == 0)
                    {
                        if (!DisallowApostropheConversion)
                        {
                            if (IsBackToSearch == false)
                            {
                                //Even
                                _applicantFirstName = (value.Replace("'", "''"));
                            }
                            else
                            {
                                _applicantFirstName = (value.Replace("''", "'"));
                            }
                        }
                        else
                        {
                            _applicantFirstName = value;
                        }
                    }
                    else
                    {
                        if (IsBackToSearch == true)
                        {
                            //return (_applicantFirstName.Replace("''", "'"));
                            _applicantFirstName = (value.Replace("''", "'"));
                        }
                        else
                        {
                            _applicantFirstName = (value);
                        }
                    }
                }
            }
        }

        private String _applicantMiddleName = String.Empty;
        [DataMember]
        public String ApplicantMiddleName
        {
            get
            {
                return _applicantMiddleName;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _applicantMiddleName = value;
                }
            }
        }

        private String _applicantLastName = String.Empty;
        [DataMember]
        public String ApplicantLastName
        {
            get
            {
                return _applicantLastName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var countAposNo = value.Split('\'').Length - 1;
                    if (countAposNo == 1 || countAposNo % 2 != 0)
                    {
                        if (!DisallowApostropheConversion)
                        {
                            //Even
                            _applicantLastName = (value.Replace("'", "''"));
                        }
                        else
                        {
                            _applicantLastName = value;
                        }

                    }
                    else if (countAposNo % 2 == 0)
                    {
                        if (!DisallowApostropheConversion)
                        {
                            if (IsBackToSearch == false)
                            {
                                //Even
                                _applicantLastName = (value.Replace("'", "''"));
                            }
                            else
                            {
                                _applicantLastName = (value.Replace("''", "'"));
                            }
                        }
                        else
                        {
                            _applicantLastName = value;
                        }
                    }
                    else
                    {
                        if (IsBackToSearch == true)
                        {
                            //return (_applicantFirstName.Replace("''", "'"));
                            _applicantLastName = (value.Replace("''", "'"));
                        }
                        else
                        {
                            _applicantLastName = (value);
                        }
                    }
                }
            }
        }
        [DataMember]
        public DateTime? DateOfBirth { get; set; }
        [DataMember]
        public String EmailAddress { get; set; }
        [DataMember]
        public String SecondaryEmailAddress { get; set; }
        [DataMember]
        public String ApplicantSSN { get; set; }

        [DataMember]
        public String HierarchyNodes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String AgencyIdList { get; set; }

        //UAT-4014
        [DataMember]
        public String SelectedUserTypeCode { get; set; }
    }
}
