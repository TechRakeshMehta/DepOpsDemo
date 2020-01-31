import { Component, OnInit } from "@angular/core";
import { PersonalInfo, LookupInfo, LanguageContract, Suffix } from "../../models/user/user.model";
import { FormBuilder, FormGroup, FormControl } from "@angular/forms";
import { Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { LookupService } from "../../services/shared-services/lookup.service";
import { UtilityService } from "../../services/shared-services/utility.service";
import { RegisterUserFormService } from "../../services/register-user/register-user-form.service";

import { LanguageTranslationService } from "../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
//import { BaseFormComponent } from "../shared/field-error-display1.component";

import { basename } from "path";
import { getLocaleDateTimeFormat, DatePipe } from "@angular/common";
import { CommonService } from "../../services/shared-services/common.service";
import { AppConsts } from "../../../environments/constants/appConstants";

import { IMyDpOptions } from 'mydatepicker';

@Component({
  selector: "address-info",
  templateUrl: "personal.component.html",
  styleUrls: ['./personal.component.css'],
})
export class PersonalComponent implements OnInit {
  selectedSuffixID: number = 0;
  IsSuffixDropDown: boolean;
  suffixlist: Array<Suffix>;
  InvalidDuplicateMsg: string;
  IsDuplicateName: boolean;
  ErrorMsg: string;
  DuplicateNameMsg: any;
  test: string;
  lstGender: Array<LookupInfo>;
  lstCommLanguages: Array<LanguageContract>;
  selectedGenderId: string;
  selectedCommLanguageId: string;
  isValidDob: boolean = true;
  isSpanishDobCalander: boolean = false;
  SelectedDobDate: Date;
  mask = [/[0-9]/, /\d/, /\d/, '-', /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
  firstNameRegex = "^(?=.{1,30}$)(([a-zA-Z])+( ?[a-zA-Z]+)|([a-zA-Z]{1}))$";
  middleNameRegex = "^(?=.{1,30}$)((([a-zA-Z])+( ?[a-zA-Z]+))|(-{5})|([a-zA-Z]{1}))$";
  lastNameregex = "^(?=.{1,30}$)(([a-zA-Z])+(-?[a-zA-Z]+)|([a-zA-Z]{1}))$";
  suffixregex = "^[a-zA-Z]*-?[a-zA-Z]*$";
  ssnRegex = "(?!9)(?!\\b(\\d)\\1+-(\\d)\\1+-(\\d)\\1+\\b)(?!000)\\d{3}-(?!00)\\d{2}-(?!0{4})\\d{4}";
  personalForm = this.formBuilder.group({
    firstName: ["", [Validators.required, Validators.pattern(this.firstNameRegex)]],
    middleName: ["", Validators.pattern(this.middleNameRegex)],
    lastName: ["", [Validators.required, Validators.pattern(this.lastNameregex)]],
    suffix: ["", [Validators.pattern(this.suffixregex), Validators.maxLength(10)]],
    Gender: ["", Validators.required],
    DOB: [null, Validators.required],
    SSN: ["", Validators.pattern(this.ssnRegex)],
    CommLanguage: [""],
    dropDownSuffix: [""]
  });
  title = "Address";
  personalInfo: PersonalInfo;

  InvalidNameMsg: string = "";
  totalLength: number = 0;
  middleNameLength: number = 0;
  suffixLength: number = 0;
  personal_validation_messages: any;
  dobCustMessage: string;
  IsInvalidName: boolean = false;

  constructor(
    private registerUserFormService: RegisterUserFormService,
    private formBuilder: FormBuilder,
    private router: Router,
    private lookupService: LookupService,
    private utilityService: UtilityService,
    private commonService: CommonService,
    private languageTranslationService: LanguageTranslationService,
    private datepipe: DatePipe
  ) { }

  EnglishMyDatePickerOptions: IMyDpOptions = {
    // other options...
    dateFormat: 'mm/dd/yyyy',
    disableUntil: {year: 1900, month:1, day: 1},
    disableSince: {year: 2100, month: 1, day: 1}
  };

  SpanishMyDatePickerOptions: IMyDpOptions = {
    // other options...
    dateFormat: 'mm/dd/yyyy',
    dayLabels: { su: "Do", mo: "Lu", tu: "Ma", we: "Mi", th: "Ju", fr: "Vi", sa: "Sa" },
    monthLabels: { 1: "Ene", 2: "Feb", 3: "Mar", 4: "Abr", 5: "May", 6: "Jun", 7: "Jul", 8: "Ago", 9: "Sep", 10: "Oct", 11: "Nov", 12: "Dic" },
    todayBtnTxt: "Hoy",
    firstDayOfWeek: "mo",
    sunHighlight: true,
    disableUntil: {year: 1900, month:1, day: 1},
    disableSince: {year: 2100, month: 1, day: 1}
  };

  setDate(): void {
    // Set today date to DOB Calendar using the patchValue function
    let date = new Date();
    this.personalForm.patchValue({
      DOB: {
        date: {
          year: date.getFullYear(),
          month: date.getMonth() + 1,
          day: date.getDate()
        }
      }
    });
  }

  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  ValidationMsgsObservable = this.dataSource.asObservable();
  setLocalization() {
    var lstKeys = ["CRTACCOUNT", "FIRSTNAME", "MIDDLENAME", "LASTNAME",
      "GENDER", "SUFFIX", "DOB", "SSN", "PREVIOUS",
      "NEXT", "CNFMCNLREGISTRATION", "CNCLREGISTRATION", "LSTITMY", "LSTITMN", "CNCL", "PRFRDCOMCTNLANG","SELECTSUFFIX"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );

    var lstKeysDuplicates = ["SELECTSUFFIX"];
    this.utilityService.SubscribeLocalListWithDupNames(
      this.languageTranslationService,
      lstKeysDuplicates
    );
    var lstValidationKeys = ["FIRSTNAMEREQ", "FIRSTNAMEVALDT", "LASTNAMEREQ", "LASTNAMEVALDT",
      "MIDDLENAMEVALDT", "GENDERREQ", "DOBREQ", "SSNREQ", "INVALIDSSN",
      "SUFFIXNAMEVALDT", "DOBNOTLESSTHAT10YEAR", "TTLLENFULLNAMEATLEASTTHREECHARS", "DPLNAMECNTADD", "MAXCHARACTERS"];
    this.utilityService.PopulateValidationCollectionFromKeys(lstValidationKeys, this.validationMessages);

    this.dataSource.next(this.validationMessages);
    this.utilityService.SubscribeValidationMessages(
      this.languageTranslationService,
      this.dataSource
    );
    this.ValidationMsgsObservable.subscribe(result => {
      this.initializeValidationMessages(result);
    });
  }
  initializeValidationMessages(validationMsgs: KeyedCollection<string>) {
    this.personal_validation_messages = {
      firstName: [
        { type: "required", message: validationMsgs["FIRSTNAMEREQ"] },
        {
          type: "pattern", message: validationMsgs["FIRSTNAMEVALDT"]
        }
      ],
      lastName: [
        { type: "required", message: validationMsgs["LASTNAMEREQ"] },
        {
          type: "pattern", message: validationMsgs["LASTNAMEVALDT"]

        }
      ],
      middleName: [
        {
          type: "pattern", message: validationMsgs["MIDDLENAMEVALDT"]

        }
      ],
      Gender: [
        { type: "required", message: validationMsgs["GENDERREQ"] }
      ],

      DOB: [{ type: "required", message: validationMsgs["DOBREQ"] }],

      SSN: [
        { type: "required", message: validationMsgs["SSNREQ"] },
        { type: "pattern", message: validationMsgs["INVALIDSSN"] }
      ],
      suffix: [
        {
          type: "pattern", message: validationMsgs["SUFFIXNAMEVALDT"]
        },
        { type: "maxlength", message: validationMsgs["SUFFIXNAMEVALDT"] }
      ]
    };
    this.dobCustMessage = validationMsgs["DOBNOTLESSTHAT10YEAR"];
    this.InvalidNameMsg = validationMsgs["TTLLENFULLNAMEATLEASTTHREECHARS"];
    this.DuplicateNameMsg = validationMsgs["DPLNAMECNTADD"];
  }
  ngOnInit() {
    this.setDate();
    var userInfo = this.registerUserFormService.getUserInfo();
    if (userInfo.IsValidUrl == false) {
      this.router.navigate([""]);
    }
    this.setLocalization();
    this.commonService.SetLoggedIn(false);//Used to set header
  
    this.registerUserFormService.IsSuffixDropDown=false;
    this.commonService.isLanguageSpanish.subscribe(isSpanishLang => {
      if (isSpanishLang) {
        this.isSpanishDobCalander = true;
        var personalInfo = this.registerUserFormService.getPersonalInfo();
        var formControls = this.personalForm.controls;
        if (personalInfo.DOB != "") {
          formControls["DOB"].setValue(personalInfo.DOB);
        }
        else { this.setDate(); }
        this.lookupService.getGender(AppConsts.SpanishCode).subscribe(values => {
          if (values.length != undefined) {
            this.lstGender = values;
          }
        });
      }
      else {
        this.isSpanishDobCalander = false;
        var personalInfo = this.registerUserFormService.getPersonalInfo();
        var formControls = this.personalForm.controls;
        if (personalInfo.DOB != "") {
          formControls["DOB"].setValue(personalInfo.DOB);
        }
        else { this.setDate(); }
        this.lookupService.getGender(AppConsts.EnglishCode).subscribe(values => {
          if (values.length != undefined) {
            this.lstGender = values;
          }
        });
      }
    });

    this.lookupService.getCommLanguages().subscribe(value => {
      if (value.length != undefined) {
        this.lstCommLanguages = value;
        if (this.selectedCommLanguageId == undefined || this.selectedCommLanguageId == "") {
          this.selectedCommLanguageId = this.lstCommLanguages[0].LanguageID;
        }
      }
    })

    this.lookupService.isSuffixDropDown().then(value => {
      if (value.length != undefined && value == "True") {
        this.IsSuffixDropDown = true;
        this.registerUserFormService.IsSuffixDropDown=true;
        this.lookupService.getSuffixDropDownList().then(s => {
          this.suffixlist = s;
          // console.log(this.suffixlist);
          this.suffixlist=this.suffixlist.filter(a=>a.IsSystem);
          this.setLocalization();
        });      
      }
      if (this.registerUserFormService.isPersonalInfoAvailable) {
        this.populateFormControls();
      }
      
    });

    
  }

  isError(field: string) {
    return this.utilityService.isError(this.personalForm, field, this.personal_validation_messages[field]);
  }
  errorMessage(field: string) {
    return this.utilityService.ErrorMessage(this.personalForm, field, this.personal_validation_messages[field]);
  }
  isFieldValid(field: string) {
    return this.utilityService.isFieldValid(this.personalForm, field);
  }

  displayFieldCss(field: string) {
    return this.utilityService.displayFieldCss(this.personalForm, field);
  }
  // displayDOBFieldCss(field: string) {

  //   return this.utilityService.displayFieldCss(this.personalForm, field);
  // } 

  populateFormControls() {

    var personalInfo = this.registerUserFormService.getPersonalInfo();
    var formControls = this.personalForm.controls;
    formControls["firstName"].setValue(personalInfo.FirstName);
    formControls["middleName"].setValue(personalInfo.MiddleName);
    formControls["lastName"].setValue(personalInfo.LastName);
    if (this.IsSuffixDropDown) {
      this.selectedSuffixID = personalInfo.selectedSuffixID;
    }
  
    else {
      formControls["suffix"].setValue(personalInfo.Suffix);
    }
    formControls["DOB"].setValue(personalInfo.DOB);
    formControls["SSN"].setValue(personalInfo.SSN);
    this.selectedGenderId = personalInfo.GenderID;
    this.selectedCommLanguageId = personalInfo.PreferedCommLanguageId;
  }
  onSubmit() {

    var isValidName = this.validateName();
    var isDuplicateName = isValidName && this.validateDuplicateAlias();

    if (this.personalForm.valid && isValidName && !isDuplicateName) {
      this.validateDOB();
      if (this.isValidDob) {
        this.personalInfo = new PersonalInfo();
        var personalFormObject = this.personalForm.value;
        this.personalInfo.FirstName = personalFormObject.firstName;
        this.personalInfo.MiddleName = personalFormObject.middleName;
        this.personalInfo.LastName = personalFormObject.lastName;
        if (this.IsSuffixDropDown && this.selectedSuffixID > 0) {

          this.personalInfo.Suffix = this.suffixlist.find(x => x.SuffixID == this.selectedSuffixID).Suffix;
          this.personalInfo.selectedSuffixID = this.selectedSuffixID;
        }
        if (!this.IsSuffixDropDown) {
          this.personalInfo.Suffix = personalFormObject.suffix;
        }
        // this.SelectedDobDate = personalFormObject.DOB;
        // this.personalInfo.DOB = this.datepipe.transform(this.SelectedDobDate, 'yyyy-MM-dd').toString();     
        this.personalInfo.DOB = personalFormObject.DOB;
        this.personalInfo.SSN = personalFormObject.SSN;
        this.personalInfo.GenderID = this.selectedGenderId;
        this.personalInfo.PreferedCommLanguageId = this.selectedCommLanguageId;
        this.registerUserFormService.setPersonalInfo(this.personalInfo);
        var userInfo = this.registerUserFormService.getUserInfo();
        userInfo.IsPersonalInfoAvail = true;
        if (this.IsSuffixDropDown && this.selectedSuffixID > 0) {

          userInfo.Suffix = this.suffixlist.find(x => x.SuffixID == this.selectedSuffixID).Suffix;
          userInfo.SelectedSuffixID = this.selectedSuffixID;
        }
        else if(this.IsSuffixDropDown)
        {
          this.personalInfo.selectedSuffixID = this.selectedSuffixID;
          this.selectedSuffixID=0;
        }
        if (!this.IsSuffixDropDown) {
          userInfo.Suffix = personalFormObject.suffix;
        }
        this.registerUserFormService.setUserInfo(userInfo);
        this.router.navigate(["/registerUser/alias"]);
      }
    }
    else {
      this.utilityService.validateAllFormFields(this.personalForm);
    }
  }
  goBack() {
    var userInfo = this.registerUserFormService.getUserInfo();
    userInfo.IsValidUrl = false;
    this.registerUserFormService.setUserInfo(userInfo);
    this.router.navigate([""]);
  }

  validateDOB() {
    this.isValidDob = true;
    var personalFormObject = this.personalForm.value;
    var data = personalFormObject.DOB.date.month + '/' + personalFormObject.DOB.date.day + '/' + personalFormObject.DOB.date.year;
    var DOBDate = new Date(data);
    // var DOBDate = new Date(personalFormObject.DOB);
    var DobDate = DOBDate.getDate();
    var DobMonth = DOBDate.getMonth() + 1;
    var DobYear = DOBDate.getFullYear();
    var today = new Date();
    var date = today.getDate();
    var month = today.getMonth() + 1;
    var year = today.getFullYear();
    if (year - DobYear < 10) {
      this.isValidDob = false;
    }
    if (year - DobYear == 10) {
      if (month < DobMonth) {
        this.isValidDob = false;
      }
      if (month == DobMonth) {
        if (date < DobDate) {
          this.isValidDob = false;
        }
      }
    }
  }
  validateName() {
    var personalFormObject = this.personalForm.value;
    this.IsInvalidName = false;
    if (personalFormObject.firstName != "" || personalFormObject.lastName != "" || personalFormObject.middleName != "" || personalFormObject.suffix != "") {

      if (personalFormObject.middleName != null && personalFormObject.middleName != "") {
        this.middleNameLength = personalFormObject.middleName.length;
      }
      if (!this.IsSuffixDropDown && personalFormObject.suffix != null && personalFormObject.suffix != "") {
        this.suffixLength =  personalFormObject.suffix.length;
      }
      if(this.IsSuffixDropDown && this.selectedSuffixID>0)
      {
        this.suffixLength =  this.suffixlist.find(x=>x.SuffixID==this.selectedSuffixID).Suffix.length;
      }
      this.totalLength = (personalFormObject.firstName.length) + (personalFormObject.lastName.length) + this.middleNameLength + this.suffixLength;
      if (this.totalLength < 3) {

        this.IsInvalidName = true;

        //this.proportion = false;
        return false;
      }
      else {
        this.IsInvalidName = false;
        // this.proportion = true;
        return true;
      }

    }
  }

  validateDuplicateAlias() {
    var lstAlias = this.registerUserFormService.getAliasInfo();
    var personalFormObject = this.personalForm.value;
    this.IsDuplicateName = false;
    var res = lstAlias.filter(x =>
      x.FirstName.toLowerCase() === personalFormObject.firstName.toLowerCase()
      && x.LastName.toLowerCase() === personalFormObject.lastName.toLowerCase()
      && x.MiddleName.toLowerCase() === personalFormObject.middleName.toLowerCase()
      && x.Suffix.toLowerCase() === personalFormObject.suffix.toLowerCase()
    );

    if (res.length > 0) {
      this.IsDuplicateName = true;
      return true;
    }
    else {
      this.IsDuplicateName = false;
      return false;
    }
  }

}