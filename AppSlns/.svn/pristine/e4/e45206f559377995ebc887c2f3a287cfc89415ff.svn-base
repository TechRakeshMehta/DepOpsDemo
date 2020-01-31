import { Injectable } from "@angular/core";

import {
  UserInfo,
  PersonalInfo,
  AddressInfo,
  ContactInfo,
  UserContract,
  PersonAliasContract,
  LookupContract
} from "../../models/user/user.model";
import { AuthService } from "../guard/auth-guard.service";
import { DataSharingService } from "../shared-services/data-sharing.service";
import { Router } from "@angular/router";

export class LangData {
  key: string;
  value: string
}

@Injectable()
export class RegisterUserFormService {
  private userInfo: UserInfo = new UserInfo();
  private isPersonalInfoValid: boolean = false;
  private isAliasInfoValid: boolean = false;
  private isAddressInfoValid: boolean = false;
  private isContactInfoValid: boolean = false;
  isPersonalInfoAvailable: boolean = false;
  isAddresslInfoAvailable: boolean = false;
  isContactlInfoAvailable: boolean = false;
  isAliasInfoAvailable: boolean = false;
  hideShowAliasButton: boolean = true;
  public lookupContracts:LookupContract[] = [];
  public selectedAccountToLink : LookupContract;
  private userContract: UserContract = new UserContract();
  SelectedUserPrimaryEmail:string = '';  
  langCollection: Array<LangData>;
  accountLinkError:string = '';
  DobDate: any;
  IsSuffixDropDown :boolean=true;

  constructor(private router: Router,
    private loginDataSharingServive: AuthService, // for auto login
    public dataSharingService: DataSharingService, // for auto login
    ) {}

  getPersonalInfo(): PersonalInfo {
    return this.userInfo.PersonalInfo;
  }

  setPersonalInfo(personalInfo: PersonalInfo) {
    this.isPersonalInfoValid = true;
    this.userInfo.PersonalInfo = personalInfo;
    this.isPersonalInfoAvailable = true;
  }

  getAddressInfo(): AddressInfo {
    return this.userInfo.AddressInfo;
  }

  setAddressInfo(addressInfo: AddressInfo) {
    this.isPersonalInfoValid = true;
    this.userInfo.AddressInfo = addressInfo;
    this.isAddresslInfoAvailable = true;
  }

  getContactInfo(): ContactInfo {
    return this.userInfo.ContactInfo;
  }

  setContactInfo(contactInfo: ContactInfo) {
    this.isPersonalInfoValid = true;
    this.userInfo.ContactInfo = contactInfo;
    this.isContactlInfoAvailable = true;
  }

  setAliasInfo(alias: PersonAliasContract) {
    var index = this.userInfo.PersonAliasList.findIndex(x => x.IdForIndex == alias.IdForIndex);
    if (index > -1) {
      this.userInfo.PersonAliasList[index] = alias;
    } else {
      this.userInfo.PersonAliasList.push(alias);
    }
    this.isAliasInfoValid = true;
    this.isAliasInfoAvailable = true;
  }

  getAliasInfo(): Array<PersonAliasContract> {
    return this.userInfo.PersonAliasList;
  }
  deleteAliasInfo(IdForIndex: number) {
    var index = this.userInfo.PersonAliasList.findIndex(x => x.IdForIndex == IdForIndex);
    if (index !== -1) {
      this.userInfo.PersonAliasList.splice(index, 1);
    }
  }

  getUserInfo(): UserInfo {
    this.populateUserInfo();
    return this.userInfo;
  }
  getUserContract(): UserContract {
    
    this.populateUserInfo();    
    this.userContract.SelectedGenderId = this.userInfo.SelectedGenderId;
    this.userContract.PrimaryEmail = this.userInfo.PrimaryEmail;
    this.userContract.SecondaryEmail = this.userInfo.SecondaryEmail;
    
    //this.userContract.SSN = this.userInfo.SSN;
    this.userContract.UserName = this.userInfo.UserName;
    this.userContract.OrganizationUserID = this.userInfo.OrganizationUserID != undefined ? this.userInfo.OrganizationUserID.toString() : "";
    this.userContract.UserID = this.userInfo.UserID;
    this.userContract.InstitutionName = this.userInfo.InstitutionName;
    this.userContract.ApplicantFirstName = this.userInfo.ApplicantFirstName;
    this.userContract.ApplicantLastName = this.userInfo.ApplicantLastName;
    this.DobDate = this.userInfo.DOB;
    this.userContract.DOB = this.DobDate.date.year + '-' + this.DobDate.date.month + '-' + this.DobDate.date.day;  
    
    this.userContract.PrimaryPhoneNumber = this.userInfo.PrimaryPhoneNumber;
    this.userContract.Password = this.userInfo.Password;
    this.userContract.FirstName = this.userInfo.FirstName;
    this.userContract.FirstName = this.userInfo.FirstName;
    this.userContract.MiddleName = this.userInfo.MiddleName;
    this.userContract.LastName = this.userInfo.LastName;
    this.userContract.FilePath = this.userInfo.FilePath;
    this.userContract.OriginalFileName = this.userInfo.OriginalFileName;
    this.userContract.PrimaryPhone = this.userInfo.PrimaryPhone;
    this.userContract.SecondaryPhone = this.userInfo.SecondaryPhone;
    this.userContract.SelectedCommLang = this.userInfo.SelectedCommLang;
    this.userContract.ErrorMessage = this.userInfo.ErrorMessage;
    this.userContract.IsAutoActive = this.userInfo.IsAutoActive;
    this.userContract.MasterZipcodeID = this.userInfo.MasterZipcodeID;
    this.userContract.CountryId = this.userInfo.CountryId;
    this.userContract.CountryName = this.userInfo.CountryName;
    this.userContract.Address = this.userInfo.Address;
    this.userContract.ZipId = this.userInfo.ZipId;
    if(this.userInfo.PersonalInfo!=undefined && this.userInfo.PersonalInfo!=null){
      this.userContract.Suffix = this.userInfo.PersonalInfo.Suffix;
      this.userContract.SSN = this.userInfo.PersonalInfo.SSN;
      this.userContract.SelectedCommLang = this.userInfo.PersonalInfo.PreferedCommLanguageId;
    }
    if (this.userInfo.StateName == "SITI@4863") {
      this.userContract.StateName = "";
    }
    else {
      this.userContract.StateName = this.userInfo.StateName;
    }
    this.userContract.CityName = this.userInfo.CityName;
    this.userContract.PostalCode = this.userInfo.PostalCode;
    this.userContract.IsLocationServiceTenant = this.userInfo.IsLocationServiceTenant;
    this.userContract.NoMiddleNameText = this.userInfo.NoMiddleNameText;
    this.userContract.IsMaskingOfPrimaryPhoneNumber = this.userInfo.IsMaskingOfPrimaryPhoneNumber;
    this.userContract.IsMaskingOfSecondaryPhoneNumber = this.userInfo.IsMaskingOfSecondaryPhoneNumber;
    this.userContract.SelectedSuffixID = this.userInfo.SelectedSuffixID;
    this.userContract.PersonAliasList = this.userInfo.PersonAliasList;
    this.userContract.SelectedLanguageCode = this.userInfo.SelectedLanguageCode;
    return this.userContract;
  }
  populateUserInfo() {
    //personal info
    this.userInfo.ApplicantFirstName = this.userInfo.PersonalInfo.FirstName;
    this.userInfo.ApplicantLastName = this.userInfo.PersonalInfo.LastName;
    this.userInfo.FirstName = this.userInfo.PersonalInfo.FirstName;
    this.userInfo.MiddleName = this.userInfo.PersonalInfo.MiddleName;
    this.userInfo.LastName = this.userInfo.PersonalInfo.LastName;
    this.userInfo.DOB = this.userInfo.PersonalInfo.DOB;
    this.userInfo.SelectedGenderId = parseInt(
      this.userInfo.PersonalInfo.GenderID
    );

    this.userInfo.Address = this.userInfo.AddressInfo.Address;
    this.userInfo.CityName = this.userInfo.AddressInfo.City;
    this.userInfo.CountryId = parseInt(this.userInfo.AddressInfo.CountryId);
    this.userInfo.CountryName = this.userInfo.AddressInfo.CountryName;
    this.userInfo.StateName = this.userInfo.AddressInfo.StateName;
    this.userInfo.PostalCode = this.userInfo.AddressInfo.ZipCode;
    // contact
    this.userInfo.PrimaryEmail = this.userInfo.ContactInfo.PrimaryEmail;
    this.userInfo.SecondaryEmail = this.userInfo.ContactInfo.SecondaryEmail;
    this.userInfo.PrimaryPhone = this.userInfo.ContactInfo.PrimaryPhone;
    this.userInfo.SecondaryPhone = this.userInfo.ContactInfo.SecondaryPhone;
    this.userInfo.UserName = this.userInfo.ContactInfo.UserName;
    this.userInfo.Password = this.userInfo.ContactInfo.Password;
    this.userInfo.SelectedLanguageCode=this.userInfo.ContactInfo.SelectedLangCode;
  }

  setAliasAddButton(value: boolean) {
    this.hideShowAliasButton = value;
  }
  isAliasAddButtonVisible() {
    return this.hideShowAliasButton;
  }

  isFormValid() {
    return (
      this.isPersonalInfoValid &&
      this.isAddressInfoValid &&
      this.isContactInfoValid
    );
  }
  resetFormData() {
    this.userInfo = new UserInfo();
    this.userContract = new UserContract();
    this.isPersonalInfoValid = false;
    this.isAliasInfoValid = false;
    this.isAddressInfoValid = false;
    this.isContactInfoValid = false;
    this.isPersonalInfoAvailable = false;
    this.isAddresslInfoAvailable = false;
    this.isContactlInfoAvailable = false;
    this.isAliasInfoAvailable = false;
    this.hideShowAliasButton = true;
    this.accountLinkError='';
  }

  setUserInfo(userInfo: UserInfo){    
    this.userInfo = userInfo;
  }

  handleAutoLogin(result:any){
    var responsedata = {
      token_type: "",
      access_token: "",
      expires_in: "",
      refresh_token: ""
    };
    if (
      result.ResponseObject != null ||
      result.ResponseObject != undefined
    ) {
      var response = result.ResponseObject;
      responsedata.access_token = response.access_token;
      responsedata.expires_in = response.expires_in;
      responsedata.refresh_token = response.refresh_token;
      responsedata.token_type = response.token_type;
      this.dataSharingService.loginDataObj = responsedata;      
      if (response != null || response != undefined) {
        sessionStorage.setItem("auth_token", response.access_token);
        sessionStorage.setItem("refresh_token", response.refresh_token);
      }
      this.loginDataSharingServive.setLoggedIn(true);
      if (response.IsLocationServiceTenant) {
        this.router.navigate(["/userDashboard"]);
      } else {
        this.router.navigate(["/welcome"]);
      }
    } else {
      // autologin
      //redirect to login screen for now
      this.router.navigate([""]);
    }
  }
}
