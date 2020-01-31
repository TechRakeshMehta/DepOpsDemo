using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class PreviousAddressContract
    {
        public Int32 ID { get; set; }
        public DateTime? ResidenceStartDate { get; set; }
        public DateTime? ResidenceEndDate { get; set; }
        public Int32 ZipCodeID { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public int MailingAddressId { get; set; }
        public int MailingExtensionAddressId { get; set; }
        public String PreviousAddress
        {
            get
            {
                return CityName + ", " + StateName + ", " + Country + ", Zipcode -" + Zipcode;
            }
            
        }
        public Boolean isDeleted { get; set; }
        public Boolean isNew { get; set; }
        public Boolean isUpdated { get; set; }
        public String TempId { get; set; }
        public Boolean isCurrent { get; set; }
        public String CityName { get; set; }
        public String StateName { get; set; }
        public String CountyName { get; set; }
        public String Zipcode { get; set; }
        public String Country { get; set; }
        public Int32 CountryId { get; set; }
        //public String UniqueId { get; set; }
        public Int32 ResHistorySeqOrdID { get; set; }
        public String MotherName { get; set; }
        public String IdentificationNumber { get; set; }
        public String LicenseNumber { get; set; }
        public String MailingOptionId { get; set; }
        public String MailingOption { get; set; }
        public String MailingOptionPrice { get; set; }
        public Boolean IsMailingChecked { get; set; }
        public Guid MailingAddressHandleId { get; set; }
        public decimal MailingOptionPriceOnly { get; set; }
    }


    /// <summary>
    /// Contract to store the Address data of a user.
    /// </summary>
    public class AddressContract
    {
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
        public String CityName
        {
            get;
            set;
        }
        public String StateName
        {
            get;
            set;
        }
        public String CountyName
        {
            get;
            set;
        }
        public String Zipcode
        {
            get;
            set;
        }
        public String Country
        {
            get;
            set;
        }
    }

    [Serializable]
    public class SelectedMailingData
    {
        public int? SelectedMailingOptionId { get; set; }
        public decimal? SelectedMailingOptionPrice { get; set; }
    }
}
