using System;
using System.Collections.Generic;

namespace Entity.ExternalVendorContracts
{
    [Serializable]
    public class EvCreateOrderContract
    {
        #region OrderProperties

        public Int32 BkgOrderID
        {
            get;
            set;
        }

        public String Address1
        {
            get;
            set;
        }

        public String Address2
        {
            get;
            set;
        }


        public String ZipCode
        {
            get;
            set;
        }

        public String City
        {
            get;
            set;
        }

        public String County
        {
            get;
            set;
        }

        public String State
        {
            get;
            set;
        }

        public String Country
        {
            get;
            set;
        }

        public String PhoneNumber
        {
            get;
            set;
        }

        public String EmailAddress
        {
            get;
            set;
        }

        public String Gender
        {
            get;
            set;
        }

        public String InstitutionName
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

        public Int32 VendorID
        {
            get;
            set;
        }

        public String AccountNumber
        {
            get;            //return account number i.e. "AMER_02772";
            set;
        }

        public String AccountName
        {
            get;
            set;
        }

        public String LoginName
        {
            get;
            set;
        }

        public String Password
        {
            get;
            set;
        }

        public Int32 BusinessOwnerID
        {
            get;
            set;
        }

        public Boolean UseADBTestAccountForAMS
        {
            get;
            set;
        }

        public String ADBTestAccountForAMS
        {
            get;
            set;
        }

        public Int32 OrganizationUserProfileID
        {
            get;
            set;
        }

        public Int32 OrderID
        {
            get;
            set;
        }
        public String ProfileSuffix
        {
            get;
            set;
        }

        public List<EvCreateOrderSvcGroupContract> PackageSvcGroups
        {
            get;
            set;
        }

        //implemented changes related to production issue for aliases at the time of supplementOrder[19/07/2016]
        public List<EvOrderProfileAliases> OrderProfileAliases { get; set; }

        #region UAT-2169:Send Middle Name and Email address to clearstar in Complio
        public String NoMiddleNameDefaultText { get; set; }
        #endregion

        #region UAT-2254:Complio: Use CreateProfileForCountry API to create profile instead of CreateProfile which is being used in all three system to create profile.
        public Int32 ClearStarCountryID { get; set; }
        #endregion

        #region //UAT-2893:Update country code sent to clearstar for international criminals
        public List<EvCountryLookup> lstCountries { get; set; }
        #endregion

        #endregion

        public String ClearStarCCFUserName
        {
            get;
            set;
        }

        public String ClearStarCCFPassword
        {
            get;
            set;
        }
    }

    [Serializable]
    public class EvCreateOrderSvcGroupContract
    {
        #region OrderSvcGroupProperties

        public Int32 BkgOrderPkgSvcGroupID
        {
            get;
            set;
        }

        public String BkgOrderVendorProfileID
        {
            get;
            set;
        }

        public Boolean TransmitInd
        {
            get;
            set;
        }

        public VendorResponse VendorResponse
        {
            get;
            set;
        }

        public Boolean IsTransmitted
        {
            get;
            set;
        }


        public List<EvOrderItemContract> OrderItems
        {
            get;
            set;
        }

        public Boolean IsSupplement
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

        public String Alias
        {
            get;
            set;
        }

        public Int32 BkgSvcTypeID
        {
            get;
            set;
        }

        public String BkgSvcTypeCode
        {
            get;
            set;
        }

        public VendorResponse VendorResponse
        {
            get;
            set;
        }

        public Int32 BkgOrderPackageSvcLineItemID
        {
            get;
            set;
        }

        public String ExternalVendorOrderID
        {
            get;
            set;
        }

        public Int32 BackgroundServiceID
        {
            get;
            set;
        }

        public Int32 ExternalServiceID
        {
            get;
            set;
        }

        public List<EvOrderItemAttributeContract> EvOrderItemAttributeContracts
        {
            get;
            set;
        }

        //public String BkgOrderLineItemVendorProfileID
        //{
        //    get;
        //    set;
        //}
        #endregion
    }

    [Serializable]
    public class VendorResponse
    {
        public String ResponseCode
        {
            get;
            set;
        }

        public String ResponseType
        {
            get;
            set;

        }

        public String ResponseMessage
        {
            get;
            set;
        }

        public Boolean IsVendorError
        {
            get;
            set;
        }

        public Boolean IsSpecialError
        {
            get;
            set;
        }
    }

    [Serializable]
    public class EvOrderItemAttributeContract
    {
        public Int32 AttributeGroupID
        {
            get;
            set;
        }

        public Int32 FieldID
        {
            get;
            set;
        }

        public String FieldValue
        {
            get;
            set;
        }

        public String FieldName
        {
            get;
            set;
        }

        public String DefaultValue
        {
            get;
            set;
        }

        public String FieldDelimiter
        {
            get;
            set;
        }

        public String FieldFormat
        {
            get;
            set;
        }

        public Int32 FieldSequence
        {
            get;
            set;
        }

        public String FieldDataType
        {
            get;
            set;
        }

        public String FieldLabel
        {
            get;
            set;
        }

        public int ExternalBkgSvcAttributeID
        {
            get;
            set;
        }

        public String ExtSvcAttributeLocationField
        {
            get;
            set;
        }
    }

    #region implemented changes related to production issue for aliases at the time of supplementOrder[19/07/2016]
    [Serializable]
    public class EvOrderProfileAliases
    {
        public String FirstName {get;set;}
        public String LastName {get;set;}
        public String MiddleName { get; set; }
        public String FullName {get;set;}
    }
#endregion
    #region //UAT-2893:Update country code sent to clearstar for international criminals
    [Serializable]
    public class EvCountryLookup
    {
        public Int32 CountryID { get; set; }
        public Int32 ClearStarCountryID { get; set; }
        public String FullName { get; set; }
        public String Alpha2Code { get; set; }
        public String ISO3Code { get; set; }
        public String PrintScanCode { get; set; }
        public String CompleteName { get; set; }
        public String ShortName { get; set; }
    }
    #endregion
}
