using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using INTSOF.Utils;
using System.Xml.Serialization;
using System.IO;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    [Serializable]
    public class SearchItemDataContract
    {
        public Int32 ClientID
        {
            get;
            set;
        }

        public String PackageID  //UAT-4136: Converted From Int32 to String.
        {
            get;
            set;
        }

        public Int32 CategoryID
        {
            get;
            set;
        }
        public String CategoryIDs
        {
            get;
            set;
        }

        public String PackageIDs
        {
            get;
            set;
        }
        public String ItemStatusID
        {
            get;
            set;
        }

        public List<Int32> StatusID
        {
            get;
            set;
        }

        public List<StatusIDClass> StatusIDForSearch
        {
            get;
            set;
        }

        public List<Int32> CategoryIDList
        {
            get;
            set;
        }

        public List<Int32> OverAllIDList
        {
            get;
            set;
        }

        public String CategoryStatusID
        {
            get;
            set;
        }
        public String OverAllStatusID
        {
            get;
            set;
        }

        //UAT:4155
        public String UserName { get; set; }

        //UAT:-4153:- Add "Account Activated" (can be Yes/No) info in Support Portal and Manage Preceptor results grids.	
        public String IsAccountActivated { get; set; }

        //UAT-4020
     
        public String SelectedUserTypeCode { get; set; }

        public Boolean IsBackToSearch { get; set; }

        public Boolean DisallowApostropheConversion { get; set; }

        private string _applicantFirstName = String.Empty;
        public String ApplicantFirstName
        {
            //get;
            //set;
            get
            {
                return (_applicantFirstName);
                //var countAposNo = _applicantFirstName.Split('\'').Length - 1;
                //if (countAposNo == 1 || countAposNo % 2 != 0)
                //{
                //    //Even
                //    return (_applicantFirstName.Replace("'", "''"));
                //}
                //else
                //{
                //    //return (_applicantFirstName.Replace("''", "'"));
                //    return (_applicantFirstName);
                //}

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

                //_applicantFirstName = value;
                //_applicantFirstName = String.Empty;
                //if (!String.IsNullOrEmpty(value))
                //{
                //    var countApos = value.Split('\'').Length - 1;
                //    if (countApos == 0 || countApos == 1)
                //    {
                //        _applicantFirstName = value;
                //    }
                //    else
                //    {
                //        _applicantFirstName = value.Replace("''", "'");
                //    }
                //}
            }
        }

        public string _applicantMiddleName = String.Empty;
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

        public string _applicantLastName = String.Empty;
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


        #region UAT-2759
        public Boolean? IsUserGroupAssigned
        {
            get;
            set;
        }
        #endregion

        #region UAT-950
        public String AdminFirstName
        {
            get;
            set;
        }

        public String AdminLastName
        {
            get;
            set;
        }
        public Int32 ItemID
        {
            get;
            set;
        }

        #endregion

        public Int32 ProgramID
        {
            get;
            set;
        }

        public Int32? NodeId
        {
            get;
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPagingArguments
        {
            get;
            set;
        }

        public Int32? DPM_Id
        {
            get;
            set;
        }

        public DateTime? DateOfBirth
        {
            get;
            set;
        }

        public String EmailAddress
        {
            get;
            set;
        }

        public String SecondaryEmailAddress
        {
            get;
            set;
        }

        public String ItemLabel
        {
            get;
            set;
        }

        public Int32? AssignedToUserID
        {
            get;
            set;
        }

        public List<String> FilterColumns
        {
            get;
            set;
        }

        public List<String> FilterOperators
        {
            get;
            set;
        }

        public List<String> FilterTypes
        {
            get;
            set;
        }

        public ArrayList FilterValues
        {
            get;
            set;
        }

        public List<String> LstStatusCode
        {
            get;
            set;
        }

        public Int32 DepartmentID
        {
            get;
            set;
        }

        public List<Int32> LstProgramID
        {
            get;
            set;
        }
        public List<Int32> LstPackageID
        {
            get;
            set;
        }

        public Int32? OrderID
        {
            get;
            set;
        }

        public String OrderNumber
        {
            get;
            set;
        }

        public Boolean? ShowOnlyRushOrder
        {
            get;
            set;
        }

        public Int32? OrganizationUserId
        {
            get;
            set;
        }

        public String ApplicantSSN
        {
            get;
            set;
        }

        public String CustomFields
        {
            get;
            set;
        }

        public String NodeLabel
        {
            get;
            set;
        }

        public List<String> LstPaymentType
        {
            get;
            set;
        }

        public List<DateTime> LstOrderCreatedDate
        {
            get;
            set;
        }

        public List<DateTime> LstOrderPaidDate
        {
            get;
            set;
        }

        public Int32 DeptProgramMappingID
        {
            get;
            set;
        }

        public Int32? FilterUserGroupID
        {
            get;
            set;
        }

        public Int32? MatchUserGroupID
        {
            get;
            set;
        }

        public List<Int32> MatchedSelectedUserGroupIDs
        {
            get;
            set;
        }

        public String ArchieveStateId
        {
            get;
            set;
        }

        public String ArchieveStateIDForItemSearch
        {
            get;
            set;
        }

        public List<String> LstArchiveState
        {
            get;
            set;
        }

        public DateTime? FromDate
        {
            get;
            set;
        }

        public DateTime? ToDate
        {
            get;
            set;
        }

        public Int32? LoggedInUserId
        {
            get;
            set;
        }

        public Int32? LoggedInUserTenantId
        {
            get;
            set;
        }

        public List<Int32> SelectedTenants
        {
            get;
            set;
        }

        // Changes done for "Use SP for Compliance Item Data Search"                
        public String XMLStatusCodes
        {
            get;
            set;
        }

        // START UAT-539 Rajeev Jha 11 Aug 2014
        // Added this attribute to hold list of Node IDs which will include 
        // selected node as well as all children nodes in hierarchy
        public List<Int32> LstNodeId
        {
            get;
            set;
        }

        public Boolean IsRestricted
        {
            get;
            set;
        }
        
        

        public String CreateXml()
        {
            var serializer = new XmlSerializer(typeof(SearchItemDataContract));
            var sb = new StringBuilder();

            SearchItemDataContract xmlData = this;
            //if (this.FilterColumns.IsNotNull())
            //{
            //    xmlData.FilterData = this.FilterColumns.Select((col, index) => new FilteringData { FilterColumn = col, FilterOperator = this.FilterOperators[index], FilterValue = FormatSQLValue(this.FilterValues[index], this.FilterTypes[index]) }).ToList();
            //}

            //xmlData.SortExpression = this.SortExpression;
            //xmlData.SecondarySortExpression = this.SecondarySortExpression;
            //xmlData.SortDirectionDescending = this.SortDirectionDescending;
            //xmlData.PageSize = this.PageSize;
            //xmlData.PageIndex = this.CurrentPageIndex;
            //xmlData.DefaultSortExpression = this.DefaultSortExpression;

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, xmlData);
            }


            return sb.ToString();
        }

        // END UAT-539

        #region UAT-846:
        public List<Int32> SelectedUserGroupIDList
        {
            get;
            set;
        }

        public List<Int32> SelectedItemIDList
        {
            get;
            set;
        }

        public String SelectedUserGroupIDs
        {
            get;
            set;
        }
        public String SelectedItemIDs
        {
            get;
            set;
        }

        /// <summary>
        /// List of Selected item Names/Labels
        /// </summary>
        public List<String> SelectedItemList
        {
            get;
            set;
        }
        /// <summary>
        /// XML of Selected item Names/Labels
        /// </summary>
        public String SelectedItemNames
        {
            get;
            set;
        }

        /// <summary>
        /// It will store the DPM ids on which current loggedIn user has permission
        /// </summary>
        public List<Int32> LstUserNodePermissions
        {
            get;
            set;
        }

        #endregion

        #region UAT-1088 - Add order date ranges to the user group mapping screen

        public DateTime? OrderCreatedFrom
        {
            get;
            set;
        }
        public DateTime? OrderCreatedTo
        {
            get;
            set;
        }

        #endregion

        #region UAT-1110 - ProfileSharing

        public Int32 AgencyID
        {
            get;
            set;
        }

        public DateTime? OrderPaidFrom
        {
            get;
            set;
        }
        public DateTime? OrderPaidTo
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// CSV DPMID's of the multiple selected nodes - UAT 1055
        /// </summary>
        public String SelectedDPMIds
        {
            get;
            set;
        }

        /// <summary>
        /// CSV NodeID's of the multiple selected nodes - UAT 1055
        /// </summary>
        public String NodeIds
        {
            get;
            set;
        }

        /// <summary>
        /// Codes of the Order types i.e. Compliance, Background or Both
        /// </summary>
        public List<String> lstOrderPackageTypes
        {
            get;
            set;
        }

        /// <summary>
        /// CSV UserGroupID's of the multiple selected UserGroup - UAT 1685
        /// </summary>
        public String UserGroupIds
        {
            get;
            set;
        }

        /// <summary>
        /// UAT-1560 : We should be able to add documents that need to be signed to the order process
        /// Using for additional document search screen
        /// </summary>
        public String DocumentName
        {
            get;
            set;
        }

        public Boolean IsADBAdmin { get; set; }

        #region UAT-963
        public String PackageName { get; set; }
        public String CategoryName { get; set; }
        public String ItemName { get; set; }
        public String SelectedTenantIDs { get; set; }

        #endregion

        #region UAT-2054
        public Int32 AgencyIDToFilterRotation { get; set; }
        public String ComplioID { get; set; }
        public String RotationName { get; set; }
        public String Department { get; set; }
        public String Program { get; set; }
        public String Course { get; set; }
        public String Term { get; set; }
        public String UnitFloorLoc { get; set; }
        public float? RecommendedHours { get; set; }
        public float? Students { get; set; }
        public String Shift { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public String DaysIdList { get; set; }
        public String ContactIdList { get; set; }
        public String TypeSpecialty { get; set; }
        public String RotationCustomAttributes { get; set; }
        #endregion

        public Boolean IsClientAdminLoggedIn { get; set; } //UAT 2834
        public String SelectedExpiryStateCode
        {
            get;
            set;
        }
        #region UAT - 4107
        public String RoleNames { get; set; } // comma seperated role names for applicantDataAuditHistory screen
        #endregion

        public Boolean ShowActiveOrdersOnly { get; set; } //UAT-4273
       
    }



    public class StatusIDClass
    {
        public Int32 statusID { get; set; }
    }

}
