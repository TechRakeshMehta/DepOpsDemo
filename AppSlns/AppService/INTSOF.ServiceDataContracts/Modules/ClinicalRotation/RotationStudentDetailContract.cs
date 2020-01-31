using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{

    [Serializable]
    [DataContract]
    public class RotationStudentDetailContract
    {
        public string _applicantFirstName = String.Empty;
        public string _applicantLastName = String.Empty;

        [DataMember]
        public Int32 ClinicalRotationID { get; set; }

        [DataMember]
        public Int32 ProfileSharingInvGrpID { get; set; }

        [DataMember]
        public Int32 SelectedTenantID { get; set; }

        [DataMember]
        public Int32 LoggedInUserID { get; set; }

        [DataMember]
        public String ApplicantFirstName
        {
            get
            {
                return _applicantFirstName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var countAposNo = value.Split('\'').Length - 1;
                    if (countAposNo == 1 || countAposNo % 2 != 0)
                    {
                        if (!DisAllowApostrophyConversion)
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
                        if (!DisAllowApostrophyConversion)
                        {
                            if (IsBackFromSearch == false)
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
                        if (IsBackFromSearch == true)
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
                        if (!DisAllowApostrophyConversion)
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
                        if (!DisAllowApostrophyConversion)
                        {
                            if (IsBackFromSearch == false)
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
                        if (IsBackFromSearch == true)
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
        public String ApplicantEmail
        {
            get;
            set;
        }

        [DataMember]
        public DateTime? DateOfBirth
        {
            get;
            set;
        }

        [DataMember]
        public String ApplicantSSN
        {
            get;
            set;
        }

        [DataMember]
        public CustomPagingArgsContract GridCustomPagingArguments { get; set; }

        [DataMember]
        public Boolean IsBackFromSearch { get; set; }

        public bool DisAllowApostrophyConversion { get; set; }

        [DataMember]
        public Boolean IsInstructor { get; set; }

        [DataMember]
        public Boolean IsAgencyUser { get; set; }

        [DataMember]
        public Int32 AgencyID { get; set; }
    }
}
