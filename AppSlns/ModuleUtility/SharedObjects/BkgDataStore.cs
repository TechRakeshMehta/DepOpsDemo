using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Xml;

namespace INTSOF.SharedObjects
{
    public class BkgDataStore
    {
        private Dictionary<String, List<lkpBkgConstantType>> _lstConstantType;
        private Dictionary<String, Dictionary<String, String>> _lstConstantData;

        List<PackageService> lstServices
        {
            get;
            set;
        }

        List<GetAttributeListByPackageId> attributeList
        {
            get;
            set;
        }

        List<lkpBkgConstantType> lstConstant
        {
            get;
            set;
        }

        public Dictionary<String, List<lkpBkgConstantType>> lstConstantType
        {
            get
            {
                if (_lstConstantType == null)
                    _lstConstantType = new Dictionary<String, List<lkpBkgConstantType>>();
                return _lstConstantType;
            }
            set
            {
                _lstConstantType = value;
            }
        }

        public Dictionary<String, Dictionary<String, String>> lstConstantData
        {
            get
            {
                if (_lstConstantData == null)
                    _lstConstantData = new Dictionary<String, Dictionary<String, String>>();
                return _lstConstantData;
            }
            set
            {
                _lstConstantData = value;
            }
        }

        #region UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
        public String NoMiddleNameText
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

        public List<lkpBkgConstantType> getConstantType(Int32 tenantId)
        {
            if (lstConstant == null)
            {
                lstConstant = BackgroungRuleManager.getConstantType(tenantId).ToList();
            }
            return lstConstant;
        }

        public List<lkpBkgConstantType> getConstantTypeByGroup(String group, Int32 tenantId)
        {
            List<lkpBkgConstantType> lstConstantList = new List<lkpBkgConstantType>();
            if (lstConstantType != null && lstConstantType.TryGetValue(group, out lstConstantList))
            {

            }
            else
            {
                lstConstantList = BackgroungRuleManager.getConstantType(tenantId).Where(obj => obj.BCT_Group == group).ToList();
                lstConstantType.Add(group, lstConstantList);
            }
            return lstConstantList;
        }

        public List<PackageService> getServicesByPackageId(Int32 packageId, Int32 tenantId)
        {
            if (lstServices == null)
            {
                lstServices = BackgroungRuleManager.getServiceListInPackage(packageId, tenantId);
            }
            return lstServices.OrderBy(col => col.ServiceName).ToList();
        }

        public List<GetAttributeListByPackageId> getAttributeList(Int32 packageId, Int32 tenantId)
        {
            if (attributeList == null)
            {
                attributeList = BackgroungRuleManager.getAttributeListByPackageId(packageId, tenantId);
            }
            return attributeList.OrderBy(col => col.BkgAttributeName).ToList();
        }

        public Dictionary<String, String> getConstantData(String selectedContantType)
        {
            Dictionary<String, String> constantDataList = new Dictionary<String, String>();
            if (lstConstantData != null && lstConstantData.TryGetValue(selectedContantType, out constantDataList))
            {

            }
            else
            {
                if (selectedContantType == BkgConstantType.COUNTRY.GetStringValue())
                {
                    constantDataList = BackgroungRuleManager.getCountryList();
                }
                else if (selectedContantType == BkgConstantType.STATE.GetStringValue())
                {
                    constantDataList = BackgroungRuleManager.getStateList();
                }
                if (selectedContantType == BkgConstantType.COUNTY.GetStringValue())
                {
                    constantDataList = BackgroungRuleManager.getCountyList();
                }
                if (selectedContantType == BkgConstantType.CITY.GetStringValue())
                {
                    constantDataList = BackgroungRuleManager.getCityList();
                }
                lstConstantData.Add(selectedContantType, constantDataList);
            }
            return constantDataList;
        }

        //Created on 12-June-2014
        public String ConvertApplicantDataIntoXML(ApplicantOrderCart applicantOrderCart, Int32 tenantId, Boolean ConsiderCurrentResidenceOnly = false, Boolean isCallForRequiredDocumentaion = false, Boolean callFromOrderReview = false)
        {
            if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() || isCallForRequiredDocumentaion)
            {
                //UAT-2100:
                String armedForcesStateText = "armed forces";
                //Boolean isValidCurrentAddressToProceed = true;
                //if (callFromOrderReview)
                //{
                //    var currentAddress = applicantOrderCart.lstPrevAddresses.FirstOrDefault(x => (x.isCurrent != null && x.isCurrent == true));
                //    if (!currentAddress.IsNullOrEmpty() && currentAddress.StateName.ToLower().Contains(armedForcesStateText))
                //    {
                //        isValidCurrentAddressToProceed = false;
                //    }
                //}
                XmlDocument _doc = new XmlDocument();

                XmlElement _rootNode = (XmlElement)_doc.AppendChild(_doc.CreateElement("PersonalDetails"));

                //UAT-2100:
                List<PreviousAddressContract> lstPreviousAddresses = new List<PreviousAddressContract>();

                lstPreviousAddresses = callFromOrderReview ? applicantOrderCart.lstPrevAddresses.Where(x => !x.StateName.ToLower().Contains(armedForcesStateText)).ToList() : applicantOrderCart.lstPrevAddresses;

                if (!applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.IsNullOrEmpty())
                {
                    #region GENERATE ORGANIZATION USER PROFILE RELATED XML

                      OrganizationUserProfile _orgUserProfile = applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile;
                    XmlNode _profileInformationNode = _rootNode.AppendChild(_doc.CreateElement("ProfileInformation"));
                    _profileInformationNode.AppendChild(_doc.CreateElement("OrganizationUserID")).InnerText = Convert.ToString(_orgUserProfile.OrganizationUserID);
                    _profileInformationNode.AppendChild(_doc.CreateElement("FirstName")).InnerText = _orgUserProfile.FirstName;
                    _profileInformationNode.AppendChild(_doc.CreateElement("LastName")).InnerText = _orgUserProfile.LastName;
                    _profileInformationNode.AppendChild(_doc.CreateElement("DOB")).InnerText = Convert.ToDateTime(_orgUserProfile.DOB).ToShortDateString();
                    _profileInformationNode.AppendChild(_doc.CreateElement("SSN")).InnerText = _orgUserProfile.SSN;
                    _profileInformationNode.AppendChild(_doc.CreateElement("Gender")).InnerText = Convert.ToString(_orgUserProfile.Gender);
                    _profileInformationNode.AppendChild(_doc.CreateElement("PhoneNumber")).InnerText = _orgUserProfile.PhoneNumber;
                    _profileInformationNode.AppendChild(_doc.CreateElement("MiddleName")).InnerText = _orgUserProfile.MiddleName;
                    _profileInformationNode.AppendChild(_doc.CreateElement("PrimaryEmailAddress")).InnerText = _orgUserProfile.PrimaryEmailAddress;
                    _profileInformationNode.AppendChild(_doc.CreateElement("SecondaryEmailAddress")).InnerText = _orgUserProfile.SecondaryEmailAddress;
                    _profileInformationNode.AppendChild(_doc.CreateElement("SecondaryPhone")).InnerText = _orgUserProfile.SecondaryPhone;

                    _rootNode.AppendChild(_profileInformationNode);

                    #endregion
                }
                if (!applicantOrderCart.lstPersonAlias.IsNullOrEmpty())
                {
                    #region GENERATE PERSONAL ALIAS RELATED XML

                    XmlNode _aliasesNode = _rootNode.AppendChild(_doc.CreateElement("Aliases"));
                    foreach (var alias in applicantOrderCart.lstPersonAlias)
                    {
                        String aliasMiddleName = alias.MiddleName.IsNullOrEmpty() ? NoMiddleNameText : alias.MiddleName;
                        XmlNode expChild = _aliasesNode.AppendChild(_doc.CreateElement("Alias"));
                        expChild.AppendChild(_doc.CreateElement("InstanceId")).InnerText = Convert.ToString(alias.AliasSequenceId);
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        expChild.AppendChild(_doc.CreateElement("AliasName")).InnerText = alias.FirstName + " " + aliasMiddleName + " " + alias.LastName;
                        _rootNode.AppendChild(_aliasesNode);
                    }

                    #endregion
                }
                //UAT-2100
                //if (!applicantOrderCart.lstPrevAddresses.IsNullOrEmpty())
                //{
                if (!lstPreviousAddresses.IsNullOrEmpty())
                {
                    #region GENERATE RESIDENTIAL ADDRESS RELATED XML

                    XmlNode _residentialAddressesNode = _rootNode.AppendChild(_doc.CreateElement("ResidentialAddresses"));
                    //UAT-2100
                    //foreach (var _prevAddress in applicantOrderCart.lstPrevAddresses)
                    //{
                    foreach (var _prevAddress in lstPreviousAddresses)
                    {
                        if (ConsiderCurrentResidenceOnly && (_prevAddress.isCurrent.IsNull() || !_prevAddress.isCurrent))
                        {
                            continue;
                        }
                        if (!_prevAddress.isDeleted)
                        {
                            //_prevAddress.UniqueId = Convert.ToString(Guid.NewGuid());
                            XmlNode _residentialAddressNode = _residentialAddressesNode.AppendChild(_doc.CreateElement("ResidentialAddress"));
                            _residentialAddressNode.AppendChild(_doc.CreateElement("InstanceId")).InnerText = Convert.ToString(_prevAddress.ResHistorySeqOrdID);
                            //_residentialAddressNode.AppendChild(_doc.CreateElement("InstanceId")).InnerText = Convert.ToString(rnd.Next(999999));
                            _residentialAddressNode.AppendChild(_doc.CreateElement("Address1")).InnerText = _prevAddress.Address1;
                            _residentialAddressNode.AppendChild(_doc.CreateElement("Address2")).InnerText = _prevAddress.Address2;
                            _residentialAddressNode.AppendChild(_doc.CreateElement("ZipCodeId")).InnerText = Convert.ToString(_prevAddress.ZipCodeID);
                            _residentialAddressNode.AppendChild(_doc.CreateElement("ZipCode")).InnerText = _prevAddress.Zipcode;
                            _residentialAddressNode.AppendChild(_doc.CreateElement("CityName")).InnerText = _prevAddress.CityName;
                            _residentialAddressNode.AppendChild(_doc.CreateElement("CountyName")).InnerText = _prevAddress.CountyName;
                            _residentialAddressNode.AppendChild(_doc.CreateElement("StateName")).InnerText = _prevAddress.StateName;
                            _residentialAddressNode.AppendChild(_doc.CreateElement("CountryName")).InnerText = _prevAddress.Country;
                            _residentialAddressNode.AppendChild(_doc.CreateElement("ResidingFrom")).InnerText = Convert.ToString(_prevAddress.ResidenceStartDate);
                            _residentialAddressNode.AppendChild(_doc.CreateElement("ResidingTo")).InnerText = Convert.ToString(_prevAddress.ResidenceEndDate);
                        }
                    }

                    #endregion
                }
                return _doc.OuterXml;

            }
            return String.Empty;
        }
    }
}
