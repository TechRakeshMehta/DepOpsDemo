using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Text;
using INTSOF.UI.Contract.BkgSetup;
using CoreWeb.Shell;
using System.Xml;
using System.Web.Configuration;

namespace CoreWeb.BkgOperations.Views
{
    public partial class ApplicantData : BaseUserControl, IApplicantDataView
    {
        #region Private Variables
        private ApplicantDataPresenter _presenter = new ApplicantDataPresenter();
        //UAT-2149:Scenarios not to show in red text on SSN Trace results
        /// <summary>
        /// This variable is used to identify that first name and last name is valid for SSN result. 
        /// </summary>
        private Boolean _isValidNamesForSSN = false;

        //UAT-2240: If SSN Trace result has no date, should be Grayed out not Red
        private Boolean _isDateExistInSSNResult = true;
        #endregion

        #region Properties

        #region Public Properties
        public ApplicantDataPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public Int32 MasterOrderId
        {
            get;
            set;
        }

        public Int32 TenantId
        {
            get;
            set;
        }
        public String ParentScreenName
        {
            get;
            set;
        }

        public Int32 AttributeGroupMappingIdForState
        {
            get
            {
                return ViewState["AttributeGroupMappingIdForState"].IsNotNull() ? Convert.ToInt32((ViewState["AttributeGroupMappingIdForState"])) : 54;
            }
            set
            {
                ViewState["AttributeGroupMappingIdForState"] = value;
            }
        }
        public Int32 AttributeGroupMappingIdForCounty
        {
            get
            {
                return ViewState["AttributeGroupMappingIdForCounty"].IsNotNull() ? Convert.ToInt32((ViewState["AttributeGroupMappingIdForCounty"])) : 55;
            }
            set
            {
                ViewState["AttributeGroupMappingIdForCounty"] = value;
            }
        }

        public SupplementOrderCart supplementOrderCartTemp { get; set; }
        #region UAT-2062:
        public IApplicantDataView CurrentViewContext
        {
            get { return this; }
        }
        List<SupplementAdditionalSearchContract> IApplicantDataView.lstSSNResultForAdditionalSearch
        {
            get;
            set;
        }

        public List<SupplementAdditionalSearchContract> lstMatchedNameForAdditionalSearch
        {
            get
            {
                return ViewState["lstMatchedNameForAdditionalSearch"].IsNotNull() ? (List<SupplementAdditionalSearchContract>)(ViewState["lstMatchedNameForAdditionalSearch"]) : new List<SupplementAdditionalSearchContract>();
            }
            set
            {
                ViewState["lstMatchedNameForAdditionalSearch"] = value;
            }
        }

        public List<SupplementAdditionalSearchContract> lstMatchedLocationForAdditionalSearch
        {
            get
            {
                return ViewState["lstMatchedLocationForAdditionalSearch"].IsNotNull() ? (List<SupplementAdditionalSearchContract>)(ViewState["lstMatchedLocationForAdditionalSearch"]) : new List<SupplementAdditionalSearchContract>();
            }
            set
            {
                ViewState["lstMatchedLocationForAdditionalSearch"] = value;
            }
        }

        public Boolean IsShowUnIdentifiedSSNResultMessage
        {
            get
            {
                return ViewState["IsShowUnIdentifiedSSNResultMessage"].IsNotNull() ? Convert.ToBoolean((ViewState["IsShowUnIdentifiedSSNResultMessage"])) : false;
            }
            set
            {
                ViewState["IsShowUnIdentifiedSSNResultMessage"] = value;
            }
        }

        #endregion

        public List<SupplementServiceItemCustomForm> lstSupplementServiceCustomFormList
        {
            //New Change 21072016
            get;
            set;
        }
        #endregion

        #region Private Properties
        private String UnIdentifiedSSNResultMessage
        {
            get
            {
                return ViewState["UnIdentifiedSSNResultMessage"].IsNotNull() ? Convert.ToString((ViewState["UnIdentifiedSSNResultMessage"])) : String.Empty;
            }
            set
            {
                ViewState["UnIdentifiedSSNResultMessage"] = value;
            }
        }

        #region UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
        private String NoMiddleNameText
        {
            get
            {
                String noMiddleNameText = WebConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY];
                if (noMiddleNameText.IsNull())
                {
                    noMiddleNameText = String.Empty;
                }
                return noMiddleNameText;
            }
        }
        #endregion
        #endregion
        #endregion

        #region Events

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BindResidentialHistoryAliasData();
                //UAT-2114: Dont show additional searches if line items will not be created.
                if (CurrentViewContext.ParentScreenName == AppConsts.ORDER_DETAIL_FOR_SERVICE_ITEM_SUPPLEMENT)
                    BindOtherServiceResults();
                if (IsShowUnIdentifiedSSNResultMessage && !UnIdentifiedSSNResultMessage.IsNullOrEmpty())
                {
                    ShowInfoMessage(UnIdentifiedSSNResultMessage);
                }
            }
        }
        #endregion

        #region Grid Events

        #region UAT-2061:Gray out any results where the end date is further than 7 years from the current date (SSN Trace)
        protected void grdSSNResults_ItemDataBound(object sender, GridItemEventArgs e)
        {

            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    if (e.Item is GridDataItem)
                    {
                        KeyValuePair<string, string> dataItem = (KeyValuePair<string, string>)e.Item.DataItem;
                        //Gray out result where the end date is further than 7 years from the current date (SSN Trace)
                        if (!CurrentViewContext.lstSSNResultForAdditionalSearch.IsNullOrEmpty())
                        {
                            if (!dataItem.IsNullOrEmpty() && CurrentViewContext.lstSSNResultForAdditionalSearch.Any(cnd => cnd.UniqueRowId == dataItem.Value && cnd.DisplayRowInGray))
                            {
                                e.Item.BackColor = System.Drawing.Color.Gray;
                                e.Item.ForeColor = System.Drawing.Color.White;
                            }
                            //Highlited row in red:If a record doesnt match the normal format for additional search UAT-2062.
                            if (IsShowUnIdentifiedSSNResultMessage && !dataItem.IsNullOrEmpty() && CurrentViewContext.lstSSNResultForAdditionalSearch
                                                                                                                     .Any(an => an.IsExistInLastSevenYear && !an.DisplayRowInGray
                                                                                                                      && !an.IsUsedForSearch && an.UniqueRowId == dataItem.Value))
                            {
                                e.Item.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Binds the applicant Residential History and Alias data used for Line item generation.
        /// </summary>
        private void BindResidentialHistoryAliasData()
        {
            var _applicantData = Presenter.GetApplicantBkgOrderDeta();
            if (_applicantData.Item2.Count() > AppConsts.NONE)
            {
                grdAliasNames.DataSource = _applicantData.Item2;
                grdAliasNames.DataBind();

                #region UAT-1571:WB: increase the count of Alias and Location inputs to 25. I still don’t want this to be open ended.
                grdAliasNames.PageSize = _applicantData.Item2.Count();
                grdAliasNames.MasterTableView.PageSize = _applicantData.Item2.Count();
                #endregion

                pnlAlias.Visible = true;
            }
            if (_applicantData.Item1.Count() > AppConsts.NONE)
            {
                grdResidentialHistory.DataSource = _applicantData.Item1;
                grdResidentialHistory.DataBind();

                #region UAT-1571:WB: increase the count of Alias and Location inputs to 25. I still don’t want this to be open ended.
                grdResidentialHistory.PageSize = _applicantData.Item1.Count();
                grdResidentialHistory.MasterTableView.PageSize = _applicantData.Item1.Count();
                #endregion

                pnlResidentialHistory.Visible = true;
            }
        }

        //UAT - 1342 : Update to handle Supplement of NW Criminal
        /*
        private void BindOtherServiceResults()
        {
            SourceServiceDetailForSupplement sourceService = Presenter.CheckSourceServicesForSupplement();
            if (sourceService.IfSSNServiceExist)
            {
                String resultText = sourceService.SSNServiceResult;
                pnlSSN.Visible = true;

                if (String.IsNullOrEmpty(resultText))
                {
                    resultText = "Empty result. Perhaps not sent yet to Clearstar...";
                }
                int myLastCharPosition = resultText.IndexOf("This product is a locater index");
                if (myLastCharPosition > 0)
                {
                    resultText = resultText.Substring(0, myLastCharPosition);
                }
                string[] mySplitSSNResults = Regex.Split(resultText, @"_+");
                List<KeyValuePair<string, string>> ssnDS = new List<KeyValuePair<string, string>>();
                int counter = 1;
                foreach (string s in mySplitSSNResults)
                {
                    Regex.Replace(s, @"^\s+", "");
                    Regex.Replace(s, @"\s+$", "");
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(s, counter.ToString());
                    ssnDS.Add(kvp); counter++;
                }
                grdSSNResults.DataSource = ssnDS;
                grdSSNResults.ClientSettings.Scrolling.AllowScroll = true;
                grdSSNResults.ClientSettings.Scrolling.ScrollHeight = 100;
                grdSSNResults.DataBind();
                grdSSNResults.Visible = true;
                //ssnDataSource = ssnDS;
            }
            if (sourceService.IfNationalCriminalServiceExist)
            {
                pnlNationwideCriminalSrch.Visible = true;
                String resultText = sourceService.NationalCriminalServiceResult;
                if (String.IsNullOrEmpty(resultText))
                {
                    resultText = "Empty result. Perhaps not sent yet to Clearstar...";
                }
                int myNoDataFoundPosition = resultText.IndexOf("No offenses found for this name");
                if (myNoDataFoundPosition > 0)
                {
                    resultText = "No offenses found for this name.";
                }
                else
                {
                    int myFirstCharPosition = resultText.IndexOf("*************************************************************************************************************");
                    int myLastCharPosition = resultText.IndexOf("Sources SearchedAL - Alabama Sex Offender");
                    if (myLastCharPosition > 0)
                    {
                        resultText = resultText.Substring(myFirstCharPosition, myLastCharPosition - myFirstCharPosition);
                        myFirstCharPosition = resultText.IndexOf(" ");
                        myLastCharPosition = resultText.LastIndexOf("*************************************************************************************************************");
                        resultText = resultText.Substring(myFirstCharPosition, myLastCharPosition - myFirstCharPosition);
                        resultText = resultText.Trim();
                    }
                }
                resultText = resultText.Replace("*************************************************************************************************************", "_");
                string[] mySplitNationwideResults = Regex.Split(resultText, @"_+");
                List<KeyValuePair<string, string>> nationwideDS = new List<KeyValuePair<string, string>>();
                int counter = 1;
                foreach (string s in mySplitNationwideResults)
                {
                    Regex.Replace(s, @"^\s+", "");
                    Regex.Replace(s, @"\s+$", "");
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(s, counter.ToString());
                    nationwideDS.Add(kvp); counter++;
                }
                grdNationwideCriminalSrchResults.DataSource = nationwideDS;
                grdNationwideCriminalSrchResults.ClientSettings.Scrolling.AllowScroll = true;
                grdNationwideCriminalSrchResults.ClientSettings.Scrolling.ScrollHeight = 100;
                grdNationwideCriminalSrchResults.DataBind();
                grdNationwideCriminalSrchResults.Visible = true;
            }
        }
*/

        private static void ProcessSSNResultText(List<KeyValuePair<string, string>> ssnDS, String ssnResultText)
        {
            String resultText = ssnResultText;
            int myLastCharPosition = resultText.IndexOf("This product is a locater index");
            if (myLastCharPosition > 0)
            {
                resultText = resultText.Substring(0, myLastCharPosition);
            }
            string[] mySplitSSNResults = Regex.Split(resultText, @"_+");
            foreach (string s in mySplitSSNResults)
            {
                Regex.Replace(s, @"^\s+", "");
                Regex.Replace(s, @"\s+$", "");
                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(s, Guid.NewGuid().ToString());
                ssnDS.Add(kvp);
            }
        }

        private static String ProcessNWCriminalResultText(List<KeyValuePair<string, string>> nationwideDS, String splittedResultText)
        {
            String flaggedAndNotParshedResult = String.Empty;
            try
            {
                //UAT-2118:Show Nationwide Criminal Vendor Service Result if there was a flag on the initial searches
                String[] splittedText = Regex.Split(splittedResultText, @"##FlaggedIndicator##");
                Boolean flaggedIndicator = splittedText[0].IsNullOrEmpty() ? false : Convert.ToBoolean(splittedText[0]);
                splittedResultText = splittedText[1];
                ////UAT-2118:Show Nationwide Criminal Vendor Service Result if there was a flag on the initial searches
                if (!flaggedIndicator)
                {
                    String resultText = splittedResultText;

                    if (String.IsNullOrEmpty(resultText))
                    {
                        resultText = "Empty result. Perhaps not sent yet to Clearstar...";
                    }
                    int myNoDataFoundPosition = resultText.IndexOf("No offenses found for this name");
                    int myNoRecordFoundPosition = resultText.IndexOf("No Record Found");
                    if (myNoDataFoundPosition > 0 || myNoRecordFoundPosition > 0)
                    {
                        resultText = "No offenses found for this name.";
                    }
                    else
                    {
                        int myFirstCharPosition = resultText.IndexOf("*************************************************************************************************************");
                        int myLastCharPosition = resultText.IndexOf("Sources SearchedAL - Alabama Sex Offender");
                        if (myLastCharPosition > 0)
                        {
                            resultText = resultText.Substring(myFirstCharPosition, myLastCharPosition - myFirstCharPosition);
                            myFirstCharPosition = resultText.IndexOf(" ");
                            myLastCharPosition = resultText.LastIndexOf("*************************************************************************************************************");
                            resultText = resultText.Substring(myFirstCharPosition, myLastCharPosition - myFirstCharPosition);
                            resultText = resultText.Trim();
                        }
                    }
                    resultText = resultText.Replace("*************************************************************************************************************", "_");

                    string[] mySplitNationwideResults = Regex.Split(resultText, @"_+");

                    foreach (string s in mySplitNationwideResults)
                    {
                        Regex.Replace(s, @"^\s+", "");
                        Regex.Replace(s, @"\s+$", "");
                        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(s, Guid.NewGuid().ToString());
                        nationwideDS.Add(kvp);
                    }
                }
                else
                {
                    //UAT-2118:Show Nationwide Criminal Vendor Service Result if there was a flag on the initial searches
                    //KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(splittedResultText, Guid.NewGuid().ToString());
                    flaggedAndNotParshedResult = splittedResultText.Replace("\n", "<p></p>");
                    return flaggedAndNotParshedResult;
                }
            }
            catch (Exception ex)
            {
                //String exceptionMessage = "Complio could not parse result text. Please see the Order Result PDF for details.";
                //KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(exceptionMessage, Guid.NewGuid().ToString());
                //nationwideDS.Add(kvp);
                //UAT-2118:Show Nationwide Criminal Vendor Service Result if there was a flag on the initial searches
                //KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(splittedResultText, Guid.NewGuid().ToString());
                flaggedAndNotParshedResult = splittedResultText.Replace("\n", "<p></p>");
                return flaggedAndNotParshedResult;
            }
            return flaggedAndNotParshedResult;
        }

        #region UAT-2062:System to determine and add additional searches in supplement (SSN Trace)

        /// <summary>
        /// Method to get Additional Search Data from SSN Result
        /// </summary>
        /// <param name="SSNResults"></param>
        private void GetAdditionalSearchDataFromSSNResult(List<KeyValuePair<string, string>> SSNResults)
        {
            CurrentViewContext.lstSSNResultForAdditionalSearch = new List<SupplementAdditionalSearchContract>();
            //UAT-2149:Scenarios not to show in red text on SSN Trace results
            Boolean isMilitaryAddressExist = false;
            if (!SSNResults.IsNullOrEmpty())
            {
                foreach (var item in SSNResults)
                {
                    String firstName = String.Empty;
                    String lastName = String.Empty;
                    String middleName = String.Empty;
                    String county = String.Empty;
                    String toDate = String.Empty;
                    String state = String.Empty;
                    String resultRow = item.Key;
                    String guidValue = item.Value;
                    String lastSevenYrStringDate = DateTime.Now.AddYears(-7).ToString("MM/yyyy");
                    DateTime lastSevenYrDate = Convert.ToDateTime(lastSevenYrStringDate);
                    DateTime? toDateTime = null;

                    //check for 'Subject' row if exist : (ex.'5 Subjects Found.SSN is valid.Issued in California  (Issued In Year 1989-1990)')
                    if (!resultRow.IsNullOrEmpty() && !resultRow.Contains("Subjects"))
                    {
                        try
                        {
                            String[] splitRsultRowBySSN = Regex.Split(resultRow, @"SSN");
                            if (!splitRsultRowBySSN.IsNullOrEmpty() && splitRsultRowBySSN.Length > AppConsts.ONE && IsResultContainsRequiredParameters(resultRow))
                            {
                                //To Get FirstName And LastName
                                String applicantName = splitRsultRowBySSN[0].Replace("\n", "");
                                String[] splitedNames = applicantName.IsNullOrEmpty() ? null : applicantName.Trim().Split(' ');
                                if (!splitedNames.IsNullOrEmpty() && splitedNames.Length > AppConsts.NONE)
                                {
                                    firstName = splitedNames.First().Trim();
                                    //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                                    middleName = String.Join(" ", splitedNames.Where((cond, index) => index < splitedNames.Length - 1 && index > AppConsts.NONE));
                                    lastName = splitedNames.Length > AppConsts.ONE ? splitedNames.LastOrDefault().Trim() : String.Empty;
                                    middleName = middleName.IsNullOrEmpty() ? NoMiddleNameText : middleName;
                                }
                                //To Get County: Get all text after 5-digit number and upto the text 'county'.
                                var countyMatches = Regex.Matches(splitRsultRowBySSN[1], @"(([^\d{5}]*)(county*\s))", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
                                if (!countyMatches.IsNullOrEmpty() && countyMatches.Count > AppConsts.NONE && !countyMatches[0].IsNullOrEmpty())
                                {
                                    String[] splittedCounty = Regex.Split(Convert.ToString(countyMatches[0]).Trim(), @" ");
                                    county = String.Join(" ", splittedCounty.Where((cond, index) => index < splittedCounty.Length - 1));
                                    county = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(county.Trim().ToLower());
                                }

                                //To Get State: Get all text after ',' and upto the 5-digit number.
                                var stateMatches = Regex.Matches(splitRsultRowBySSN[1], @"([^\,]*)(\s\d{5}\s)", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
                                if (!stateMatches.IsNullOrEmpty() && stateMatches.Count > AppConsts.NONE && !stateMatches[0].IsNullOrEmpty())
                                {
                                    string stateMatch = Convert.ToString(stateMatches[0]);
                                    if (Regex.IsMatch(Convert.ToString(stateMatches[0]), @"DOB", RegexOptions.IgnoreCase)
                                        && Regex.IsMatch(Convert.ToString(stateMatches[0]), @"Address", RegexOptions.IgnoreCase)
                                        && stateMatches.Count > AppConsts.ONE
                                        )
                                    {
                                        stateMatch = Convert.ToString(stateMatches[1]);
                                    }
                                    String[] splittedState = Regex.Split(stateMatch.Trim(), @" ");
                                    splittedState = splittedState.Where(x => x != null && x.Trim() != "").ToArray();
                                    if (splittedState.Length == AppConsts.TWO && Regex.IsMatch(splittedState[1], @"(\d{5})", RegexOptions.IgnoreCase)
                                         && splittedState[0].Length == AppConsts.TWO)
                                    {
                                        state = splittedState[0].Trim();
                                    }
                                }

                                //To Get End Date: Get text after text 'to'
                                var toDatematches = Regex.Matches(splitRsultRowBySSN[1], @"(?<=to\s)(?<toDate>\b\S+\b)", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
                                if (!toDatematches.IsNullOrEmpty() && toDatematches.Count > AppConsts.NONE && !toDatematches[0].IsNullOrEmpty())
                                {
                                    toDate = Convert.ToString(toDatematches[0].Groups["toDate"]);
                                    if (!toDate.IsNullOrEmpty())
                                    {
                                        toDateTime = Convert.ToDateTime(toDate);
                                    }
                                }
                            }
                            //UAT-2149:Scenarios not to show in red text on SSN Trace results
                            isMilitaryAddressExist = IsMilitaryAddressExist(resultRow);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                        finally
                        {
                            SupplementAdditionalSearchContract additionalSearchData = new SupplementAdditionalSearchContract();
                            additionalSearchData.StateAbbreviation = state;
                            additionalSearchData.CountyName = county;
                            additionalSearchData.FirstName = firstName;
                            additionalSearchData.LastName = lastName;
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            //additionalSearchData.MiddleName = middleName;
                            //UAT-2309:Remove middle names only from supplement searches. We should only send the first and last names to clearstar with “——-“ 
                            //in the middle name field always. only one version of each first/last combo should be displayed and sent for supplement search.
                            additionalSearchData.MiddleName = String.Empty;
                            additionalSearchData.UniqueRowId = guidValue;
                            additionalSearchData.IsUsedForSearch = isMilitaryAddressExist ? false : IsSSNResultValidForAdditionalSearch(firstName, lastName, state, county, toDateTime);
                            additionalSearchData.IsExistInLastSevenYear = true;
                            //UAT-2149:Scenarios not to show in red text on SSN Trace results
                            additionalSearchData.DisplayRowInGray = !_isDateExistInSSNResult ? true : isMilitaryAddressExist ? true : !_isValidNamesForSSN;

                            //Business logic: end date within the last 7 years
                            if ((!toDateTime.IsNullOrEmpty() && toDateTime < lastSevenYrDate))
                            {
                                additionalSearchData.IsExistInLastSevenYear = false;
                                //UAT-2149:Scenarios not to show in red text on SSN Trace results
                                additionalSearchData.DisplayRowInGray = true;
                            }

                            CurrentViewContext.lstSSNResultForAdditionalSearch.Add(additionalSearchData);
                        }
                    }
                }
                if (!CurrentViewContext.lstSSNResultForAdditionalSearch.IsNullOrEmpty() && CurrentViewContext.lstSSNResultForAdditionalSearch.Any(an => an.IsExistInLastSevenYear && !an.IsUsedForSearch && !an.DisplayRowInGray))
                {
                    UnIdentifiedSSNResultMessage = "System could not parse highlighted SSN trace results based on format specified. Please process these results manually.";
                }
            }
        }

        private Boolean IsSSNResultValidForAdditionalSearch(String firstName, String lastName, String state, String county, DateTime? toDate)
        {
            Boolean isValidForSearch = false;
            _isValidNamesForSSN = true;
            if (!firstName.IsNullOrEmpty() && !lastName.IsNullOrEmpty() && !state.IsNullOrEmpty() && !county.IsNullOrEmpty() && !toDate.IsNullOrEmpty())
            {
                //Bussiness logic: there must be at least 3 characters in both the first name and last name in order to add the search [UAT-2149]
                isValidForSearch = IsValidFirstAndLastNameLength(firstName, lastName, AppConsts.THREE);
                _isValidNamesForSSN = isValidForSearch;
            }
            return isValidForSearch;
        }

        /// <summary>
        /// Method to Check following parameters exist in result or not
        /// Parameters: SSN:,5 digit number,Comma,"to","county"
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Boolean</returns>
        private Boolean IsResultContainsRequiredParameters(String result)
        {
            Boolean isValid = true;
            _isDateExistInSSNResult = true;
            if (!Regex.IsMatch(result, @"(SSN:)", RegexOptions.IgnoreCase))
                isValid = false;
            if (!Regex.IsMatch(result, @"(\s\d{5}\s)", RegexOptions.IgnoreCase))
                isValid = false;
            if (!result.Contains(","))
                isValid = false;
            if (!Regex.IsMatch(result, @"(\sto\s)", RegexOptions.IgnoreCase))
            {
                _isDateExistInSSNResult = false;
                isValid = false;
            }
            if (!Regex.IsMatch(result, @"(county)", RegexOptions.IgnoreCase))
                isValid = false;
            return isValid;

        }
        #endregion

        #region UAT-2114: Dont show additional searches if line items will not be created.

        /// <summary>
        /// Is Armed Forces State Exist
        /// </summary>
        /// <param name="suppOrderData"></param>
        /// <param name="attributeGroupMappingIdForState"></param>
        /// <param name="attributeGroupMappingIdForCounty"></param>
        /// <returns></returns>
        private Boolean IsArmedForcesStateExist(SupplementOrderData suppOrderData, Int32 attributeGroupMappingIdForState, Int32 attributeGroupMappingIdForCounty)
        {
            if (!suppOrderData.FormData.IsNullOrEmpty() && suppOrderData.FormData.ContainsKey(attributeGroupMappingIdForState) && suppOrderData.FormData.Any(cnd => cnd.Value.ToLower().Contains(("Armed Forces").ToLower())))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region UAT-2118:Show Nationwide Criminal Vendor Service Result if there was a flag on the initial searches
        /// <summary>
        /// Method to set flagged and not parshed nationwide result in session.
        /// </summary>
        /// <param name="flaggedResultlist">lst of result</param>
        private void SetFlaggedAndNotParshedNationwideResultInSession(List<String> flaggedResultlist)
        {
            SessionForSupplementServiceCustomForm supplementServiceCustomFormTemp = new SessionForSupplementServiceCustomForm();
            supplementServiceCustomFormTemp = (SessionForSupplementServiceCustomForm)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_NATIONWIDE_SERVICE_RESULTS);
            if (supplementServiceCustomFormTemp.IsNullOrEmpty())
            {
                supplementServiceCustomFormTemp = new SessionForSupplementServiceCustomForm();
            }
            supplementServiceCustomFormTemp.lstFlaggedAndNotParshedResultData = flaggedResultlist.Where(x => x != null && x != String.Empty).ToList();
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_NATIONWIDE_SERVICE_RESULTS, supplementServiceCustomFormTemp);
        }
        #endregion

        #region UAT-2149:Scenarios not to show in red text on SSN Trace results
        /// <summary>
        /// Method to identify that the SSN result contains Military Address(text:- APO, AE, DPO, or FPO)
        /// </summary>
        /// <param name="SSNResultRowData">SSN result</param>
        /// <returns>Boolean</returns>
        private Boolean IsMilitaryAddressExist(String SSNResultRowData)
        {
            String address = string.Empty;
            Boolean isMilitaryAddressExist = false;
            if (!SSNResultRowData.IsNullOrEmpty())
            {
                String[] splitAddress = Regex.Split(SSNResultRowData, @"Address", RegexOptions.IgnoreCase);
                if (!splitAddress.IsNullOrEmpty())
                {
                    address = splitAddress.Where(x => x != null && x != String.Empty).LastOrDefault();
                    address = address.Replace(',', ' ');
                    if (!address.IsNullOrEmpty()
                        && (Regex.IsMatch(address, @"(?<=\s)(APO+\s)", RegexOptions.IgnoreCase)
                            || Regex.IsMatch(address, @"(?<=\s)(AE+\s)", RegexOptions.IgnoreCase)
                            || Regex.IsMatch(address, @"(?<=\s)(DPO+\s)", RegexOptions.IgnoreCase)
                            || Regex.IsMatch(address, @"(?<=\s)(FPO+\s)", RegexOptions.IgnoreCase)
                           )
                       )
                    {
                        isMilitaryAddressExist = true;
                    }
                }
            }
            return isMilitaryAddressExist;
        }

        /// <summary>
        /// Method to validate the first name and last name according to the following business logics
        /// 1) Bussiness logic: there must be at least 2 characters in both the first name and last name for non-military addresses in order to add the search [UAT-2062]
        /// 2) Bussiness logic: there must be at least 3 characters in both the first name and last name for military addresses in order to add the search [UAT-2149]
        /// </summary>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        /// <param name="charLength">Char length ie. 2 or 3 according to above defined business logics</param>
        /// <returns>Boolean</returns>
        private Boolean IsValidFirstAndLastNameLength(String firstName, String lastName, Int32 charLength)
        {
            Boolean isLengthValid = false;
            if (firstName.Length > charLength - 1 && lastName.Length > charLength - 1)
            {
                isLengthValid = true;
            }
            return isLengthValid;
        }
        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Bind the SSN Trace and Nationwide Criminal Search result data.
        /// </summary>
        public void BindOtherServiceResults(Boolean isFilteredAditionalSearchData = false)
        {
            SourceServiceDetailForSupplement sourceService = Presenter.CheckSourceServicesForSupplement();
            if (sourceService.IfSSNServiceExist)
            {
                String completeSSNResultText = sourceService.SSNServiceResult;
                pnlSSN.Visible = true;
                List<KeyValuePair<string, string>> ssnDS = new List<KeyValuePair<string, string>>();

                if (String.IsNullOrEmpty(completeSSNResultText))
                {
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>("Empty result. Perhaps not sent yet to Clearstar...", Guid.NewGuid().ToString());
                    ssnDS.Add(kvp);
                }
                else
                {
                    string[] splittedCompleteSSNResultText = Regex.Split(completeSSNResultText, @"##NewLine##");
                    foreach (String ssnResultText in splittedCompleteSSNResultText)
                    {
                        ProcessSSNResultText(ssnDS, ssnResultText);
                    }
                    //Method to Get Additional Search data from  SSN Result XML [UAT-2062]
                    GetAdditionalSearchDataFromSSNResult(ssnDS);

                    //UAT-2114: Dont show additional searches if line items will not be created.
                    Presenter.GetMatchedAdditionalSearchData();
                    //Get Supplement Order Pricing Data and Get Matched Additional Search Data
                    //GetSupplementOrderPricingData(isFilteredAditionalSearchData);

                }
                grdSSNResults.DataSource = ssnDS;
                grdSSNResults.DataBind();
                grdSSNResults.Visible = true;
            }
            if (sourceService.IfNationalCriminalServiceExist)
            {
                pnlNationwideCriminalSrch.Visible = true;
                String completeResultText = sourceService.NationalCriminalServiceResult;
                List<KeyValuePair<string, string>> nationwideDS = new List<KeyValuePair<string, string>>();
                if (completeResultText.IsNullOrEmpty())
                {
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>("Empty result. Perhaps not sent yet to Clearstar...", Guid.NewGuid().ToString());
                    nationwideDS.Add(kvp);
                }
                else
                {
                    #region UAT-2118:Show Nationwide Criminal Vendor Service Result if there was a flag on the initial searches
                    List<String> flaggedResultlist = new List<String>();
                    string[] splittedCompleteResultText = Regex.Split(completeResultText, @"##NewLine##");
                    foreach (String splittedResultText in splittedCompleteResultText)
                    {
                        flaggedResultlist.Add(ProcessNWCriminalResultText(nationwideDS, splittedResultText));
                    }
                    //Set Flagged and not parshed result data in session
                    SetFlaggedAndNotParshedNationwideResultInSession(flaggedResultlist);
                    #endregion
                }

                grdNationwideCriminalSrchResults.DataSource = nationwideDS;
                grdNationwideCriminalSrchResults.DataBind();
                grdNationwideCriminalSrchResults.Visible = true;
            }
        }

        #region UAT-2114: Dont show additional searches if line items will not be created.
        /// <summary>
        /// Get Supplement Order Pricing Data
        /// </summary>
        public void GetFilteredLocationSearches()
        {
            List<Entity.ClientEntity.BkgAttributeGroupMapping> lstBkgAttributeGroupMapping = Presenter.GetAllBkgAttributeGroupMapping();
            Guid stateAliasGuid = new Guid("CAEAC9FA-FFF4-4F7A-A644-96967F399362");
            Guid countryGuid = new Guid("37B6B708-C691-4568-B604-6F70F24BC839");
            Guid countyGuid = new Guid("C00AEFB5-37DF-44F7-A050-D2C9581909DE");
            Int32 attributeGroupMappingIdForState = AttributeGroupMappingIdForState = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == stateAliasGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;
            Int32 attributeGroupMappingIdForCounty = AttributeGroupMappingIdForCounty = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == countyGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;
            Int32 attributeGroupMappingIdForCountry = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == countryGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;

            List<SupplementOrderServices> _lstSupplementOrderPricing = new List<SupplementOrderServices>();
            //New Change 21072016
            SupplementOrderCart supplementOrderCart = supplementOrderCartTemp;
            if (supplementOrderCart.IsNotNull() && CurrentViewContext.ParentScreenName == AppConsts.CUSTOM_FORM_FOR_SERVICE_ITEM_SUPPLEMENT)
            {
                XmlDocument _doc = new XmlDocument();
                XmlElement _rootNode = (XmlElement)_doc.AppendChild(_doc.CreateElement("BkgOrderAddition"));

                List<Int32> _lstPackageServiceIds = lstSupplementServiceCustomFormList
                                                .DistinctBy(svc => svc.PackageServiceId)
                                                .Select(svcId => svcId.PackageServiceId).ToList();

                _rootNode.AppendChild(_doc.CreateElement("MasterOrderID")).InnerText = Convert.ToString(CurrentViewContext.MasterOrderId);

                XmlNode _packageServiceNode = _rootNode.AppendChild(_doc.CreateElement("PackageService"));
                foreach (var _packageServiceId in _lstPackageServiceIds)
                {
                    _packageServiceNode.AppendChild(_doc.CreateElement("PackageSvcID")).InnerText = Convert.ToString(_packageServiceId);
                }

                List<SupplementServiceItemCustomForm> lstCustomForms = new List<SupplementServiceItemCustomForm>();
                lstCustomForms = lstSupplementServiceCustomFormList.DistinctBy(x => x.CustomFormID).Select(col => col).ToList();
                foreach (SupplementServiceItemCustomForm customForm in lstCustomForms)
                {
                    List<SupplementOrderData> _lstSupplementOrderData = supplementOrderCart.lstSupplementOrderData
                                                                        .Where(sod => sod.BkgServiceId == customForm.ServiceId)
                                                                        .ToList();
                    List<SupplementOrderData> _lstBkgAttributeDataGrps = _lstSupplementOrderData
                                                                            .Where(svc => svc.PackageSvcItemId == customForm.ServiceItemID
                                                                                  && svc.BkgServiceId == customForm.ServiceId)
                                                                            .ToList();
                    foreach (var _attrGrp in _lstBkgAttributeDataGrps)
                    {
                        XmlNode _attDataGrpNode = _rootNode.AppendChild(_doc.CreateElement("BkgSvcAttributeDataGroup"));
                        _attDataGrpNode.AppendChild(_doc.CreateElement("AttributeGroupID")).InnerText = Convert.ToString(_attrGrp.BkgSvcAttributeGroupId);
                        _attDataGrpNode.AppendChild(_doc.CreateElement("InstanceId")).InnerText = Convert.ToString(_attrGrp.InstanceId);

                        if (_attrGrp.FormData.Any(cond => cond.Key == attributeGroupMappingIdForState))//if not personal alias type mapping id then add mapping id for country
                        {
                            XmlNode expChild_Country = _attDataGrpNode.AppendChild(_doc.CreateElement("BkgSvcAttributeData"));
                            expChild_Country.AppendChild(_doc.CreateElement("BkgAttributeGroupMappingID")).InnerText = Convert.ToString(attributeGroupMappingIdForCountry);
                            expChild_Country.AppendChild(_doc.CreateElement("Value")).InnerText = Convert.ToString("UNITED STATES");
                        }
                        //UAT-2100:
                        Boolean isArmedForceStateExist = IsArmedForcesStateExist(_attrGrp, attributeGroupMappingIdForState, attributeGroupMappingIdForCounty);
                        foreach (var _attrData in _attrGrp.FormData)
                        {
                            XmlNode expChild = _attDataGrpNode.AppendChild(_doc.CreateElement("BkgSvcAttributeData"));
                            expChild.AppendChild(_doc.CreateElement("BkgAttributeGroupMappingID")).InnerText = Convert.ToString(_attrData.Key);
                            expChild.AppendChild(_doc.CreateElement("Value")).InnerText = isArmedForceStateExist ? String.Empty : Convert.ToString(_attrData.Value);
                        }
                    }
                }
                if (_lstPackageServiceIds.IsNullOrEmpty())
                {
                    //New Change 21072016
                    if (!supplementOrderCart.LstSupplementServiceId.IsNullOrEmpty())
                    {
                        _lstPackageServiceIds = supplementOrderCart.LstSupplementServiceId;
                    }
                }

                _lstSupplementOrderPricing = Presenter.GetSupplementOrderPricingData(base.CurrentUserId, _doc.OuterXml, supplementOrderCart.OrdPkgSvcGroupId, _lstPackageServiceIds);
            }

            Presenter.FilterLocationSearchData(_lstSupplementOrderPricing, attributeGroupMappingIdForState, attributeGroupMappingIdForCounty);
        }
        #endregion

        #endregion

        #endregion
    }
}