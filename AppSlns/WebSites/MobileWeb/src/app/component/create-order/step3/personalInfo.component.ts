import { Component, OnInit } from "@angular/core";
import { PersonalInfo, LookupInfo, LanguageContract, Suffix } from "../../../models/user/user.model";
import { FormBuilder, FormGroup, FormControl } from "@angular/forms";
import { Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { UserInfo } from "../../../models/user/user.model";
import { OrderInfo } from "../../../models/order-flow/order.model";
import { LookupService } from "../../../services/shared-services/lookup.service";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { basename } from "path";
import { conformToMask } from "angular2-text-mask";
import { CommonService } from "../../../services/shared-services/common.service";
import { AppConsts } from "../../../../environments/constants/appConstants";
import { FindValueSubscriber } from "rxjs/operators/find";
import { IMyDpOptions } from 'mydatepicker';

@Component({
  styleUrls: ["./personalInfo.component.css"],
  selector: "address-info",
  templateUrl: "personalInfo.component.html"
})

export class PersonalInfoComponent implements OnInit {

  // textboxSuffixList: Array<Suffix>;
  suffixlist: Array<Suffix>;
  IsSuffixDropDown: boolean = false;
  DuplicateNameMsg: any;
  dobCustMessage: any;
  lstGender: Array<LookupInfo>;
  lstCommLanguages: Array<LanguageContract>;
  selectedGenderId: string = "";
  SelectedGenderDefaultLanID: string = "";
  selectedSuffixID: number = 0;
  selectedCommLanguageId: string;
  isValidDob: boolean = true;
  BirthDate: Date;
  IsEditProfile: boolean;

  mask = [/[0-9]/, /\d/, /\d/, "-", /\d/, /\d/, "-", /\d/, /\d/, /\d/, /\d/];
  firstNameRegex = "^(?=.{1,30}$)(([a-zA-Z])+( ?[a-zA-Z]+)|([a-zA-Z]{1}))$";
  middleNameRegex =
    "^(?=.{1,30}$)((([a-zA-Z])+( ?[a-zA-Z]+))|(-{5})|([a-zA-Z]{1}))$";
  lastNameregex = "^(?=.{1,30}$)(([a-zA-Z])+(-?[a-zA-Z]+)|([a-zA-Z]{1}))$";
  suffixregex = "^[a-zA-Z]*-?[a-zA-Z]*$";
  ssnRegex = "(?!9)(?!\\b(\\d)\\1+-(\\d)\\1+-(\\d)\\1+\\b)(?!000)\\d{3}-(?!00)\\d{2}-(?!0{4})\\d{4}";
  personalForm = this.formBuilder.group({
    firstName: [
      "",
      [Validators.required, Validators.pattern(this.firstNameRegex)]
    ],
    middleName: ["", Validators.pattern(this.middleNameRegex)],
    lastName: [
      "",
      [Validators.required, Validators.pattern(this.lastNameregex)]
    ],
    suffix: ["", [Validators.pattern(this.suffixregex), Validators.maxLength(10)]],
    Gender: ["", Validators.required],
    DOB: [""],
    SSN: ["", Validators.pattern(this.ssnRegex)],
    CommLanguage: [""],
    dropDownSuffix: [""],
  });
  title = "Address";

  constructor(
    private orderFlowService: OrderFlowService,
    private formBuilder: FormBuilder,
    private router: Router,
    private lookupService: LookupService,
    private utilityService: UtilityService,
    private languageTranslationService: LanguageTranslationService,
    private commonService: CommonService,
  ) { }
  IsInvalidName: boolean = false;
  IsDuplicateName: boolean = false;
  userInfo: UserInfo;
  orderInfo: OrderInfo;
  ShowInvalidNameMsg: boolean = false;
  InvalidNameMsg: string = "";
  totalLength: number = 0;
  middleNameLength: number = 0;
  suffixLength: number = 0;
  IsSSNReqd: boolean = false;
  isSpanishDobCalander: boolean = false;
  personal_validation_messages: any;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  ValidationMsgsObservable = this.dataSource.asObservable();

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
    // Set Selected DOB date to DOB Calendar using the patchValue function
    var personalInfo = this.orderFlowService.getOrganizationUserInfoDetails();

    this.personalForm.patchValue({
      DOB: {
        date: this.utilityService.DateStringToCalenderObject(personalInfo.DOB)
      }
    });
  }

  setLocalization() {

    var lstKeys = ["FIRSTNAME", "MIDDLENAME", "LASTNAME", "OF",
      "SUFFIX", "GENDER", "DOB", "SSN", "CREATODR", "STEP", "PERSONALINFO", "PREVIOUS", "NEXT",
      "CNFMCNLORDER", "CNCLORDR", "LSTITMY", "LSTITMN", "CNCL", "PRFRDCOMCTNLANG", "UPDATE","SELECTSUFFIX"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var lstKeysDuplicates = ["PLEASESELECT","SELECTSUFFIX"];
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
        { type: "required", message: validationMsgs["GENDERREQ"] }],

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
    if (this.orderFlowService.IsDobAvailable == false) {
      this.setDate();
    }
    this.setLocalization();
    this.IsEditProfile = this.orderFlowService.IsEditProfile;
    if (!this.IsEditProfile) {
      this.personalForm.controls["DOB"].setValidators(Validators.required);
    }
    this.orderInfo = this.orderFlowService.getOrderInfo();
    this.userInfo = this.orderFlowService.getOrganizationUserInfoDetails();
    this.commonService.isLanguageSpanish.subscribe(isSpanishLang => {
      if (isSpanishLang) {
        this.isSpanishDobCalander = true;
        this.lookupService.getGender(AppConsts.SpanishCode).subscribe(values => {
          if (values.length != undefined) {
            this.lstGender = values;
          }
        });
      }
      else {
        this.isSpanishDobCalander = false;
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
    });
    this.lookupService.isSuffixDropDown().then(value => {
      if (value.length != undefined && value == "True") {
        this.IsSuffixDropDown = true;
        this.orderFlowService.IsSuffixDropDown = true;
      }
      this.lookupService.getSuffixDropDownList().then(s => {
        this.suffixlist = s;
        // this.textboxSuffixList=s;
        this.suffixlist=this.suffixlist.filter(a=>a.IsSystem);
        this.setLocalization();
        this.populateFormControls();
      });


    });

  }

  isError(field: string) {
    return this.utilityService.isError(
      this.personalForm,
      field,
      this.personal_validation_messages[field]
    );
  }
  errorMessage(field: string) {
    return this.utilityService.ErrorMessage(
      this.personalForm,
      field,
      this.personal_validation_messages[field]
    );
  }
  isFieldValid(field: string) {
    return this.utilityService.isFieldValid(this.personalForm, field);
  }

  displayFieldCss(field: string) {
    return this.utilityService.displayFieldCss(this.personalForm, field);
  }
  populateFormControls() {
    //this.setDate();
    var personalInfo = this.userInfo;
    var formControls = this.personalForm.controls;
    formControls["firstName"].setValue(personalInfo.FirstName);
    formControls["middleName"].setValue(personalInfo.MiddleName);
    formControls["lastName"].setValue(personalInfo.LastName);
    if (this.IsSuffixDropDown && personalInfo.SelectedSuffixID>0) {
      if(this.suffixlist.findIndex(x=>x.SuffixID==personalInfo.SelectedSuffixID)>=0)
      {
      this.selectedSuffixID =personalInfo.SelectedSuffixID;
      }
      else
      {
        this.selectedSuffixID=0;
      }
    }

    
    else {
     if(!personalInfo.IsSuffixDropDown)
      formControls["suffix"].setValue(personalInfo.Suffix);
    }
    if (this.orderFlowService.IsDobAvailable == true) {
      formControls["DOB"].setValue(this.orderFlowService.DobAvailableValue);
    }
    //formControls["DOB"].setValue(personalInfo.DOB);
    //this.BirthDate = new Date(personalInfo.DOB);    
    var t = this.orderInfo.IsSSNRequired;
    this.IsSSNReqd = this.orderInfo.IsSSNRequired == "Y" ? true : false;
    //this.IsSSNReqd = this.orderInfo.lstCustomAttribute.find(x=>x.Name == "SSNReqd").AttributeDataValue == 'Y'?true:false;
    if (this.IsSSNReqd) {
      formControls["SSN"].setValidators([Validators.required, Validators.pattern(this.ssnRegex)]);
    }

    // else {
    //   formControls["SSN"].clearValidators();
    // }
    var SSN = conformToMask(
      personalInfo.SSN,
      this.mask,
      { guide: false }
    )
    formControls["SSN"].setValue(SSN.conformedValue);

    this.selectedGenderId = personalInfo.SelectedGenderId.toString();
    this.SelectedGenderDefaultLanID = personalInfo.SelectedGenderDefaultKeyID.toString();

    this.selectedCommLanguageId = personalInfo.SelectedCommLang;
  }
  onSubmit() {
    this.orderFlowService.IsDobAvailable = false;
    var isValidName = this.validateName();
    var isDuplicateName = isValidName && this.validateDuplicateAlias();
    if (
      this.personalForm.valid &&
      isValidName &&
      !isDuplicateName
    ) {
      if(!this.IsEditProfile)
      {
        this.validateDOB();
      }
     
      if(this.isValidDob)
      {
      var personalFormObject = this.personalForm.value;
      this.userInfo.FirstName = personalFormObject.firstName;
      this.userInfo.MiddleName = personalFormObject.middleName;
      this.userInfo.LastName = personalFormObject.lastName;
      this.userInfo.IsSuffixDropDown=this.IsSuffixDropDown;
      if (this.IsSuffixDropDown && this.selectedSuffixID > 0) {
        this.userInfo.Suffix = this.suffixlist.find(x => x.SuffixID == this.selectedSuffixID).Suffix;
        this.userInfo.SelectedSuffixID = this.selectedSuffixID;
      }
      else if(this.IsSuffixDropDown)
      {
        this.userInfo.Suffix="";
        this.userInfo.SelectedSuffixID = this.selectedSuffixID;
      }
      if (!this.IsSuffixDropDown) {
        this.userInfo.Suffix = personalFormObject.suffix;
      }
      if (!this.IsEditProfile) {
        //this.userInfo.DOB = personalFormObject.DOB;      
        this.userInfo.DOB = this.utilityService.CalenderObjectToDateString(personalFormObject.DOB); // this.DobDate.date.year + '-' + this.DobDate.date.month + '-' + this.DobDate.date.day;  
        this.orderFlowService.DobAvailableValue = personalFormObject.DOB;

        this.userInfo.SSN = personalFormObject.SSN;
      }
      var selectedGender = this.lstGender.find(x => x.DefaultLanguageKeyID == parseInt(this.SelectedGenderDefaultLanID))
      if (selectedGender != undefined || selectedGender != null) {
        this.userInfo.SelectedGenderId = selectedGender.ID;
        this.userInfo.SelectedGenderDefaultKeyID = selectedGender.DefaultLanguageKeyID;
        this.userInfo.Gender = selectedGender.Name;
      }
      this.userInfo.SelectedCommLang = this.selectedCommLanguageId;

      this.orderFlowService.setOrganizationUserInfoDetails(this.userInfo);
      this.router.navigate(["/createOrder/aliasInfo"]);
    }
  }
    else {
      this.utilityService.validateAllFormFields(this.personalForm);
    }
  }
  goBack() {
    this.orderFlowService.DecrementStep();
    if (!this.IsEditProfile) {
      this.router.navigate(["createOrder/Orderflowpackages"]);
    }
    else {
      this.router.navigate(["userDashboard"]);
    }
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
    if (
      personalFormObject.firstName != "" ||
      personalFormObject.lastName != "" ||
      personalFormObject.middleName != "" ||
      personalFormObject.suffix != ""
    ) {
      this.ShowInvalidNameMsg = true;
      if (
        personalFormObject.middleName != null &&
        personalFormObject.middleName != ""
      ) {
        this.middleNameLength = personalFormObject.middleName.length;
      }
      if (!this.IsSuffixDropDown && personalFormObject.suffix != null && personalFormObject.suffix != "") {
        this.suffixLength = personalFormObject.suffix.length;
      }
      if (this.IsSuffixDropDown && this.selectedSuffixID > 0) {
        this.suffixLength = this.suffixlist.find(x => x.SuffixID == this.selectedSuffixID).Suffix.length;
      }
      this.totalLength =
        personalFormObject.firstName.length +
        personalFormObject.lastName.length +
        this.middleNameLength +
        this.suffixLength;
      if (this.totalLength < 3) {
        this.IsInvalidName = true;
        //this.proportion = false;
        return false;
      } else {
        this.IsInvalidName = false;
        // this.proportion = true;
        return true;
      }
    }
  }

  validateDuplicateAlias() {
    var lstAlias = this.orderFlowService.getOrganizationUserInfoDetails()
      .PersonAliasList;
    var personalFormObject = this.personalForm.value;

    var res = lstAlias.filter(
      x =>
        x.FirstName.toLowerCase() ===
        personalFormObject.firstName.toLowerCase() &&
        x.LastName.toLowerCase() ===
        personalFormObject.lastName.toLowerCase() &&
        x.MiddleName.toLowerCase() ===
        personalFormObject.middleName.toLowerCase() &&
        x.Suffix.toLowerCase() === personalFormObject.suffix.toLowerCase()
    );

    if (res.length > 0) {
      this.IsDuplicateName = true;
      return true;
    } else {
      this.IsDuplicateName = false;
      return false;
    }
  }
}
