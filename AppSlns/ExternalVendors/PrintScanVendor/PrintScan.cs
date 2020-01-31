using Business.RepoManagers;
using Entity;
using Entity.ExternalVendorContracts;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ExternalVendors.PrintScanVendor
{
    public class PrintScan
    {
        #region Private Variables
        private List<Country> lstCountry;
        private List<ExternalVendorFieldOption> lstExternalVendorFieldOption;
        #endregion

        #region Properties
        public List<State> States { get; set; }
        #endregion

        #region Public Methods

        public PrintScan(Int32 vendorID)
        {
            lstCountry = SecurityManager.GetCountries();

            //Get External Attributes field options.
            lstExternalVendorFieldOption = SecurityManager.GetExternalVendorFieldOptionByVendorID(vendorID);
        }

        public string GetJsonForExtendedInformation(List<EvOrderItemAttributeContract> evOrderItemAttributeContract, EvCreateOrderContract evOrderContract, ref  List<ExternalVendors.ClearStarVendor.ClearStar.ParamObject> parameters)
        {
            StringBuilder requestBody = new StringBuilder(string.Empty);

            //Start creating JSON Request
            requestBody.Append("{");

            //Adding extended information
            requestBody.Append("\"personal\":{");

            foreach (EvOrderItemAttributeContract attr in evOrderItemAttributeContract)
            {
                if (!requestBody.ToString().Contains(attr.ExtSvcAttributeLocationField))
                {
                    //Getting attribute value
                    string attrFieldValue = GetAttributeValue(attr, evOrderItemAttributeContract);

                    //Adding into passing parameters collection
                    parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = attr.ExtSvcAttributeLocationField, ParameterValue = attrFieldValue });

                    //Adding Parameter & value into request body
                    requestBody.Append(string.Concat("\"", attr.ExtSvcAttributeLocationField, "\":\"", attrFieldValue, "\","));
                }
            }

            //Adding DOB attr
            string dobAttrValue = evOrderContract.DateOfBirth.HasValue ? evOrderContract.DateOfBirth.Value.ToString("yyyy-MM-dd") : string.Empty;
            parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = "dateOfBirth", ParameterValue = dobAttrValue });
            requestBody.Append(string.Concat("\"dateOfBirth\":\"", dobAttrValue, "\","));

            //Adding Social Security
            string ssnAttrValue = evOrderContract.SSN.Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty);
            parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = "socialSecurity", ParameterValue = ssnAttrValue });
            requestBody.Append(string.Concat("\"socialSecurity\":\"", ssnAttrValue, "\""));

            //Completing extended information
            requestBody.Append("}");


            //Adding personal & Residance Info
            string countryCode = null;
            string stateCode = null;

            //Getting Country Code
            if (!string.IsNullOrEmpty(evOrderContract.Country))
            {
                var country = lstCountry.FirstOrDefault(cond => cond.ShortName.ToLower() == evOrderContract.Country.ToLower());
                if (country.IsNotNull())
                {
                    countryCode = country.PrintScanCode;
                }
            }

            if (!string.IsNullOrEmpty(evOrderContract.State))
            {
                bool isNeedToSearch = true;
                if (string.Compare(evOrderContract.Country.ToLower(), "united states") == 0)
                {
                    if (!string.IsNullOrEmpty(evOrderContract.State) && evOrderContract.State.Length == 2)
                    {
                        stateCode = evOrderContract.State;
                        isNeedToSearch = false;
                    }
                }

                if (isNeedToSearch)
                {
                    var state = States.FirstOrDefault(cond => cond.StateName.ToLower() == evOrderContract.State.ToLower());
                    if (state.IsNotNull())
                    {
                        stateCode = state.StateAbbreviation;
                    }
                }
            }

            //Adding Address1
            string add1 = string.IsNullOrEmpty(evOrderContract.Address1) ? null : evOrderContract.Address1;
            parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = "address1", ParameterValue = add1 });
            requestBody.Append(String.Concat(",\"address1\":\"", add1, "\""));

            //Adding Address2
            string add2 = string.IsNullOrEmpty(evOrderContract.Address2) ? null : evOrderContract.Address1;
            parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = "address2", ParameterValue = add2 });
            requestBody.Append(String.Concat(",\"address2\":\"", add2, "\""));

            //Adding cell & Phone number
            string phone = string.IsNullOrEmpty(evOrderContract.PhoneNumber) ? null : evOrderContract.PhoneNumber.Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty);
            parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = "cell", ParameterValue = phone });
            parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = "phone", ParameterValue = phone });
            requestBody.Append(String.Concat(",\"cell\":\"", phone, "\""));
            requestBody.Append(String.Concat(",\"phone\":\"", phone, "\""));

            //Adding City
            string city = string.IsNullOrEmpty(evOrderContract.City) ? null : evOrderContract.City;
            requestBody.Append(String.Concat(",\"city\":\"", city, "\""));
            parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = "city", ParameterValue = city });

            //Adding country
            requestBody.Append(String.Concat(",\"country\":\"", countryCode, "\""));
            parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = "country", ParameterValue = countryCode });

            //Adding Email
            string email = string.IsNullOrEmpty(evOrderContract.EmailAddress) ? null : evOrderContract.EmailAddress;
            requestBody.Append(String.Concat(",\"email\":\"", email, "\""));
            parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = "email", ParameterValue = email });

            //Adding FName
            string fName = string.IsNullOrEmpty(evOrderContract.FirstName) ? null : evOrderContract.FirstName;
            requestBody.Append(String.Concat(",\"firstName\":\"", fName, "\""));
            parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = "firstName", ParameterValue = fName });

            //Adding LName
            string lName = string.IsNullOrEmpty(evOrderContract.LastName) ? null : evOrderContract.LastName;
            requestBody.Append(String.Concat(",\"lastName\":\"", lName, "\""));
            parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = "lastName", ParameterValue = lName });

            //Adding Middle Name
            string mName = string.IsNullOrEmpty(evOrderContract.MiddleName) ? null : evOrderContract.MiddleName;
            requestBody.Append(String.Concat(",\"middleName\":\"", mName, "\""));
            parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = "middleName", ParameterValue = mName });

            //Adding State
            requestBody.Append(String.Concat(",\"state\":\"", stateCode, "\""));
            parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = "state", ParameterValue = stateCode });

            //Adding ZipCode
            string zipCode = string.IsNullOrEmpty(evOrderContract.ZipCode) ? null : evOrderContract.ZipCode;
            requestBody.Append(String.Concat(",\"zip\":\"", zipCode, "\""));
            parameters.Add(new ExternalVendors.ClearStarVendor.ClearStar.ParamObject { ParameterName = "zip", ParameterValue = zipCode });


            //Completing JSON Request
            requestBody.Append("}");
            return requestBody.ToString();
        }

        public T CreateContractFromResponse<T>(string responseJson)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(responseJson));
            T responseContract = (T)ser.ReadObject(ms);
            return responseContract;
        }

        #endregion

        #region Private Methods
        private string GetAttributeValue(EvOrderItemAttributeContract attr, List<EvOrderItemAttributeContract> evOrderItemAttributeContract)
        {
            string attrFieldValue = attr.FieldValue;

            //If attribute of country datatype
            if (attr.FieldDataType == SvcAttributeDataType.COUNTRY.GetStringValue())
            {
                //If attribute value United States
                if (string.Compare(attrFieldValue.ToLower(), "united states") == 0)
                {
                    if (States.IsNotNull() && States.Count > 0)
                    {
                        //Find State attribute from attribute contract list
                        var stateAttr = evOrderItemAttributeContract.FirstOrDefault(cond => cond.FieldDataType == SvcAttributeDataType.STATE.GetStringValue());
                        if (stateAttr.IsNotNull())
                        {
                            State state = States.FirstOrDefault(cond => cond.StateName.ToLower() == stateAttr.FieldValue.ToLower());
                            if (state.IsNotNull())
                            {
                                //Get State Abbreviation & assign to field value
                                attrFieldValue = state.StateAbbreviation;
                            }
                        }
                    }
                }
                //Get Printscan country code from countryCode
                else
                {
                    if (lstCountry != null && lstCountry.Count > 0)
                    {
                        var country = lstCountry.FirstOrDefault(cond => cond.ShortName.ToLower() == attrFieldValue.ToLower());
                        if (country != null)
                        {
                            attrFieldValue = country.PrintScanCode;
                        }
                    }
                }
            }
            //If attribute of State datatype
            else if (attr.FieldDataType == SvcAttributeDataType.STATE.GetStringValue())
            {
                State state = States.FirstOrDefault(cond => cond.StateName.ToLower() == attrFieldValue.ToLower());
                if (state.IsNotNull())
                {
                    //Get State Abbreviation & assign to field value
                    attrFieldValue = state.StateAbbreviation;
                }
            }
            //If attribute of stand alone datatype
            else if (attr.FieldDataType == SvcAttributeDataType.STAND_ALONE_COUNTRY.GetStringValue())
            {
                if (lstCountry != null && lstCountry.Count > 0)
                {
                    var country = lstCountry.FirstOrDefault(cond => cond.ShortName.ToLower() == attrFieldValue.ToLower());
                    if (country != null)
                    {
                        attrFieldValue = country.PrintScanCode;
                    }
                }
            }


            //Adding extended Info attributes
            if (lstExternalVendorFieldOption != null
                && lstExternalVendorFieldOption.Count > 0
                && lstExternalVendorFieldOption.Any(cond => cond.EVFO_ExternalBkgServiceAttributeID == attr.ExternalBkgSvcAttributeID))
            {
                var attrFieldOption = lstExternalVendorFieldOption
                                        .Where(cond => cond.EVFO_ExternalBkgServiceAttributeID == attr.ExternalBkgSvcAttributeID && cond.EVFO_FieldOption.Contains(attr.FieldValue))
                                        .FirstOrDefault();
                if (attrFieldOption != null)
                {
                    attrFieldValue = attrFieldOption.EVFO_FieldCode;
                }
            }

            return string.IsNullOrEmpty(attrFieldValue) ? string.Empty : attrFieldValue;
        }
        #endregion
    }
}
