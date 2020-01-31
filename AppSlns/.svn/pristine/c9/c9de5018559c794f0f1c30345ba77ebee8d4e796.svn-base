import { AttributesForCustomForm } from "../custom-forms/custom-attribute";

export class PersonalInfo {
  FirstName: string = "";
  MiddleName: string = "";
  LastName: string = "";
  Suffix: string = "";
  GenderID: string = "";
  DOB: string = "";
  SSN: string = "";
  PreferedCommLanguageId: string = "";
  selectedSuffixID:number;
}

export class LookupInfo {
  ID: number;
  Name: string;
  Code:string;
  LanguageID:number;
  DefaultLanguageKeyID:number;
}

export class AddressInfo {
  Address: string = "";
  CountryId: string = "";
  CountryName: string = "";
  StateId: string = "";
  StateName: string = "";
  ZipCode: string = "";
  City: string = "";
}

export class ContactInfo {
  PrimaryEmail: string = "";
  SecondaryEmail: string = "";
  PrimaryPhone: string = "";
  SecondaryPhone: string = "";
  UserName: string = "";
  Password: string = "";
  SelectedLangCode: string;
  PswdRecoveryEmail:string;
}
export class PersonAliasContract {
  IdForIndex: number;
  ID: number;
  FirstName: string = "";
  LastName: string = "";
  MiddleName: string = "";
  AliasSequenceId: string;
  Suffix: string = "";
  SuffixID: number;
  AttributeMiddleNameReadonly: boolean = false;
  IsAliasReadonly: boolean = true;
  IsEditbtnHidden: boolean = false;
  IsDeletebtnHidden: boolean = false;
  IsCancelbtnHidden: boolean = false;
  IsSaveButtonHidden: boolean = true;
  IsAddMoreButtonClicked: boolean = false;
}

export class UserContract {
  SelectedGenderId: number;
  SelectedGenderDefaultKeyID:number;
  ID: string;
  PrimaryEmail: string;
  SecondaryEmail: string;
  PswdRecoveryEmail:string;
  SSN: string;
  SSNL4:string;
  Username: string;
  UserName: string;
  OrganizationUserID: string;
  UserID: string;
  InstitutionName: string;
  ApplicantFirstName: string;
  ApplicantLastName: string;
  DOB: string;
  TenantID: string;
  PrimaryPhoneNumber: string;
  Password: string;
  FirstName: string;
  MiddleName: string;
  LastName: string;
  FilePath: string;
  OriginalFileName: string;
  PrimaryPhone: string;
  SecondaryPhone: string;
  SelectedCommLang: string;
  ErrorMessage: string;
  IsAutoActive: boolean;
  MasterZipcodeID: number;
  CountryId: number;
  Address: string;
  ZipId: number;
  StateName: string;
  CityName: string;
  PostalCode: string;
  IsLocationServiceTenant: boolean;
  NoMiddleNameText: string;
  IsMaskingOfPrimaryPhoneNumber: boolean;
  IsMaskingOfSecondaryPhoneNumber: boolean;
  SelectedSuffixID: number;
  CountryName: string = "";
  Gender: string = "";
  Suffix: string = "";
  PersonAliasList: Array<PersonAliasContract>;
  SMSPhoneNumber: string = "";
  IsReceiveTextNotification: boolean;
  SelectedLanguageCode: string;
  VerificationPermissionCode:string;
  UpdateAspnetEmail:boolean;
}


export class UserInfo {
  PersonalInfo: PersonalInfo;
  AddressInfo: AddressInfo;
  ContactInfo: ContactInfo;
  PersonAliasList: Array<PersonAliasContract>;
  SelectedGenderId: number;
  SelectedGenderDefaultKeyID:number;
  ID: string;
  PrimaryEmail: string;
  SecondaryEmail: string;
  PswdRecoveryEmail:string;
  SSN: string;
  Username: string;
  UserName: string;
  OrganizationUserID: number;
  UserID: string;
  InstitutionName: string;
  ApplicantFirstName: string;
  ApplicantLastName: string;
  DOB: string;
  TenantID: string;
  PrimaryPhoneNumber: string;
  Password: string;
  FirstName: string;
  MiddleName: string;
  LastName: string;
  FilePath: string;
  OriginalFileName: string;
  PrimaryPhone: string;
  SecondaryPhone: string;
  SelectedCommLang: string;
  ErrorMessage: string;
  IsAutoActive: boolean;
  MasterZipcodeID: number;
  CountryId: number;
  Address: string;
  ZipId: number;
  StateName: string;
  CityName: string;
  PostalCode: string = "";
  IsLocationServiceTenant: boolean;
  NoMiddleNameText: string;
  IsMaskingOfPrimaryPhoneNumber: boolean;
  IsMaskingOfSecondaryPhoneNumber: boolean;
  SelectedSuffixID: number;
  CountryName: string = "";
  Gender: string = "";
  Suffix: string = "";
  StateId: string = "";
  SMSPhoneNumber: string = "";
  IsReceiveTextNotification: boolean;
  IsValidUrl: boolean = false;
  IsPersonalInfoAvail: boolean = false;
  IsAliasInfoAvail: boolean = false;
  IsAddressInfoAvail: boolean = false;
  SelectedLanguageCode: string;
  UpdateAspnetEmail:boolean;
  IsSuffixDropDown:boolean;

  constructor() {
    this.PersonalInfo = new PersonalInfo();
    this.AddressInfo = new AddressInfo();
    this.ContactInfo = new ContactInfo();
    this.PersonAliasList = new Array<PersonAliasContract>();
  }
}

export class LanguageContract {
  LanguageID : string;
  LanguageName : string;
  LanguageCode: string;
  LanguageCulture : string;
}

export class LookupContract{
  ID:number;
  Code:string;
  Name:string;
  UserID:string;
  IsFirstLogin:string;
}

export class LinkAccountContract{
  User:UserContract;
  LookupContract:LookupContract;
  VerificationPassword:string;  
}

export class VerificationAccountContract{
  User:UserContract;
  lstCustomAttributes:Array<AttributesForCustomForm> = new Array<AttributesForCustomForm>();
}
export class Suffix{
  Suffix: string;
  SuffixID:number; 
  SuffixCode:string;
  IsSystem:boolean;
}
