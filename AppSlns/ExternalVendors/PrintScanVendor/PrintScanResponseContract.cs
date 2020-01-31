using System;

namespace ExternalVendors.PrintScanVendor
{
    public class PrintScanResponseContract
    {
        public Int32 applicantId { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string cell { get; set; }
        public string reasonForEnrollment { get; set; }
        public string shippingMethod { get; set; }
        public string additionalCopies { get; set; }
        public string paymentType { get; set; }
        public string amountCharged { get; set; }
        public string status { get; set; }
        public string applicantDate { get; set; }
        public string fingerprintDate { get; set; }
        public string expirationDate { get; set; }
        public string category { get; set; }
        public string employeeCode { get; set; }
        public string barCode { get; set; }
        public string tot { get; set; }
        public string ori { get; set; }
        public string tcn { get; set; }
        public string dai { get; set; }
        public string tcr { get; set; }
        public string isFileArchived { get; set; }
        public string isFileDeleted { get; set; }
        public string isProcessed { get; set; }
        public string lastUpdated { get; set; }
        public Personal personal { get; set; }
    }

    public class Personal
    {
        public string dateOfBirth;
        public string placeOfBirth;
        public string citizenship;
        public string socialSecurity;
        public string gender;
        public string race;
        public string eyeColor;
        public string hairColor;
        public string heightFeet;
        public string heightInches;
        public string weight;
    }
}
