import { Component, OnInit, ViewChild } from "@angular/core";
import { ContactInfo } from "../../models/user/user.model";
import { FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { LookupService } from "../../services/shared-services/lookup.service";
import { RegisterUserFormService } from "../../services/register-user/register-user-form.service";
import { AdbApiService } from "../../services/shared-services/adb-api-data.service";
import { UtilityService } from "../../services/shared-services/utility.service";
import { AuthService } from "../../services/guard/auth-guard.service"; // for auto login
import { DataSharingService } from "../../services/shared-services/data-sharing.service"; // for auto login
import { LanguageTranslationService } from "../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { AppConsts } from "../../../environments/constants/appConstants";
import { CommonService } from "../../services/shared-services/common.service";
import { LanguageParams } from "../../models/language-translation/language-translation-contract.model";
import { userInfo } from "os";
import { conformToMask } from "angular2-text-mask";
import { RecaptchaComponent } from 'ng-recaptcha';

@Component({
  selector: "contact-info",
  templateUrl: "contact.component.html"
})
export class ContactComponent implements OnInit {

  @ViewChild('captchaRef') captchaRef: RecaptchaComponent;
  EmailAddressNotMatchMsg: any;
  UserNameAvailable: any;
  UsernameAlreadyInUseMsg: any;
  color: string;
  accountLinkError: string = '';
  isFirstSubscribe: boolean = true;
  public mask = ["(", /[1-9]/, /\d/, /\d/, ")", "-", /\d/, /\d/, /\d/, "-", /\d/, /\d/, /\d/, /\d/];
  emailRegex = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
  passwordRegex =
    "(?=^.{8,15}$)^(?=.*[A-Z])(?=.*\\d)(?=.*[@#$%^_+~!?\\\\/\\'\\:\\,\\(\\){\\}\\[\\]\\-])[a-zA-Z0-9@#$%^_+~!?\\\\/\\'\\:\\,\\(\\)\\{\\}\\[\\]\\-]{8,}$";
  primaryPhoneRegex = "^[(][0-9]{3}[)-]+[0-9]{3}[-]+[0-9]{4}$";
  usernameRegex = "^[.@a-zA-Z0-9_-]{4,50}$";
  UserValdiateCusMsg: string = "Invalid username. Must have at least 4 chars (A-Z a-z 0-9 . _ - @).";
  contactForm = this.formBuilder.group({
    recaptchaReactive: [""],
    primaryEmail: [
      "",
      Validators.compose([
        Validators.required,
        Validators.pattern(this.emailRegex)
      ])
    ],
    secondaryEmail: ["", [Validators.pattern(this.emailRegex)]],
    primaryPhone: [
      "",
      [Validators.required, Validators.pattern(this.primaryPhoneRegex)]
    ],
    secondaryPhone: [""],
    userName: [
      "",
      [Validators.required, Validators.pattern(this.usernameRegex)]
    ],
    password: [
      "",
      [Validators.required, Validators.pattern(this.passwordRegex)]
    ],
    confirmPrimaryEmail: [
      "",
      [Validators.required, Validators.pattern(this.emailRegex)]
    ],
    confirmPassword: ["", Validators.required],
    confirmSecondaryEmail: ["", Validators.pattern(this.emailRegex)]
  });
  title = "Contact";
  contactInfo: ContactInfo;
  form: any;
  userCreationResult: string;
  UsernameMsg: string;
  proportion: boolean;
  ShowUserNameMsg: boolean = false;
  IsPrimaryEmailMatch: boolean = true;
  IsSecondaryEmailMatch: boolean = true;
  IsPasswordMatch: boolean = true;
  ErrorMessage: string = "";
  hideUserCreationError: boolean = true;
  userCreationError: string = "";
  contact_validation_messages: any;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  LanguageCode: string = "";
  languageParams: LanguageParams;
  // PassvalidateMsg:string="Your password should be of 8 to 15 characters and contain at least one numeric digit [0-9], one letter [A-Z] [a-z] and one special character [@#$%^_+~!?\':/,(){}[]-]. "

  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );

  ValidationMsgsObservable = this.dataSource.asObservable();
  setLocalization() {
    //labels
    var lstKeys = ["CRTACCOUNT",
      "EMAIL",
      "SECEMAIL",
      "CSECEMAIL",
      "PRPHONE",
      "SECPHONE",
      "USERNAME",
      "CHECK",
      "PASSWORD",
      "CNFMPASSWORD",
      "CNCL",
      "CRTACCNT", "PREVIOUS", "CRTACCANDPROCEED", "PWDDIDNOTMATCH", "CNFMCNLREGISTRATION",
      "CNCLREGISTRATION", "LSTITMY", "LSTITMN", "CNCL", "CNFRMMAIL"
    ];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    //validaton msgs
    var lstValidaionKeys = [
      "EMAILADDRESSREQ",
      "EMAILNOTVALID",
      "CNFMEMAILADDRESSREQ",
      "PASSWORDREQ",
      "INVALIDPASSWORD",
      "CNFMPASSWORDREQ",
      "USERNAMEREQ",
      "PHONEREQ",
      "PHNFORMAT",
      "USERNAMEALREADYINUSE",
      "USERNAMEAVLBLE",
      "EMAILADDRESSNOTMATCH",
      "PSWDCONDITION",
      "INVALIDUSERNAME"
    ];
    this.utilityService.PopulateValidationCollectionFromKeys(
      lstValidaionKeys,
      this.validationMessages
    );

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
    this.contact_validation_messages = {
      primaryEmail: [
        { type: "required", message: validationMsgs["EMAILADDRESSREQ"] },
        { type: "pattern", message: validationMsgs["EMAILNOTVALID"] }
      ],
      confirmPrimaryEmail: [
        { type: "required", message: validationMsgs["CNFMEMAILADDRESSREQ"] },
        { type: "pattern", message: validationMsgs["EMAILNOTVALID"] }
      ],
      password: [
        { type: "required", message: validationMsgs["PASSWORDREQ"] },
        { type: "pattern", message: validationMsgs["PSWDCONDITION"] }
      ],
      confirmPassword: [
        { type: "required", message: validationMsgs["CNFMPASSWORDREQ"] }
      ],
      userName: [{ type: "required", message: validationMsgs["USERNAMEREQ"] },
      { type: "pattern", message: validationMsgs["INVALIDUSERNAME"] }
      ],
      primaryPhone: [
        { type: "required", message: validationMsgs["PHONEREQ"] },
        { type: "pattern", message: validationMsgs["PHNFORMAT"] }
      ],
      secondaryEmail: [
        { type: "pattern", message: validationMsgs["EMAILNOTVALID"] }
      ],
      confirmSecondaryEmail: [
        { type: "pattern", message: validationMsgs["EMAILNOTVALID"] }
      ],
      cellularPhone: [
        { type: "required", message: validationMsgs["PHONEREQ"] },
        { type: "pattern", message: validationMsgs["PHNFORMAT"] }
      ]
    };
    this.UsernameAlreadyInUseMsg = validationMsgs["USERNAMEALREADYINUSE"];
    this.UserNameAvailable = validationMsgs["USERNAMEAVLBLE"];
    this.EmailAddressNotMatchMsg = validationMsgs["EMAILADDRESSNOTMATCH"];
    if (this.ShowUserNameMsg == true) {
      this.CheckUser();
    }
  }

  constructor(
    private registerUserFormService: RegisterUserFormService,
    private formBuilder: FormBuilder,
    private router: Router,
    private lookupService: LookupService,
    private adbApiService: AdbApiService,
    private utilityService: UtilityService,
    private loginDataSharingServive: AuthService, // for auto login
    public dataSharingService: DataSharingService, // for auto login
    private languageTranslationService: LanguageTranslationService,
    private commonService: CommonService
  ) { }

  ngOnInit() {
    var userInfo = this.registerUserFormService.getUserInfo();
    this.accountLinkError = this.registerUserFormService.accountLinkError;
    if (userInfo.IsAddressInfoAvail == false) {
      this.router.navigate([""]);
    }

    this.setLocalization();
    if (this.registerUserFormService.isContactlInfoAvailable) {
      this.populateFormControls();
    }
    if (this.commonService.isLanguageSpanish) {
      this.commonService.isLanguageSpanish.subscribe(isSpanishLang => {
        this.setLocalizationFields(isSpanishLang);
        if (this.isFirstSubscribe) {
          this.isFirstSubscribe = false;
        } else {
          this.accountLinkError = '';
        }
      });
    }
  }
  onCaptcha() {
    if (this.contactForm.valid && this.ValidateForm()) {
    this.captchaRef.execute();
    }
    else
    {
      this.utilityService.validateAllFormFields(this.contactForm);
    }
  }
  setLocalizationFields(isSpanishLanguage: boolean) {
    this.hideUserCreationError = true;
    if (isSpanishLanguage) {
      this.LanguageCode = AppConsts.SpanishCode;
    }
    else {
      this.LanguageCode = AppConsts.EnglishCode;
    }

  }

  isFieldValid(field: string) {
    return this.utilityService.isFieldValid(this.contactForm, field);
  }
  isError(field: string) {
    return this.utilityService.isError(
      this.contactForm,
      field,
      this.contact_validation_messages[field]
    );
  }
  errorMessage(field: string) {
    return this.utilityService.ErrorMessage(
      this.contactForm,
      field,
      this.contact_validation_messages[field]
    );
  }
  displayFieldCss(field: string) {
    return this.utilityService.displayFieldCss(this.contactForm, field);
  }

  populateFormControls() {
    // populate dropdowns
    var contactInfo = this.registerUserFormService.getContactInfo();
    var formControls = this.contactForm.controls;
    formControls["primaryEmail"].setValue(contactInfo.PrimaryEmail);
    formControls["confirmPrimaryEmail"].setValue(contactInfo.PrimaryEmail);
    formControls["secondaryEmail"].setValue(contactInfo.SecondaryEmail);
    formControls["confirmSecondaryEmail"].setValue(contactInfo.SecondaryEmail);
    var primaryPhone = conformToMask(
      contactInfo.PrimaryPhone,
      this.mask,
      { guide: false }
    )
    formControls["primaryPhone"].setValue(primaryPhone.conformedValue);
    var secondaryPhone = conformToMask(
      contactInfo.SecondaryPhone,
      this.mask,
      { guide: false }
    )
    formControls["secondaryPhone"].setValue(secondaryPhone.conformedValue);
    formControls["userName"].setValue(contactInfo.UserName);
  }
  CheckUser() {
    var contactFormObject = this.contactForm.value;
    var userName = contactFormObject.userName;
    this.color = "#a94442";
    if (userName != null && userName != "") {
      this.ShowUserNameMsg = true;
      this.adbApiService.IsUserAlreadyExists(userName).subscribe(s => {
        if (s.Message == "True") {
          this.UsernameMsg = this.UsernameAlreadyInUseMsg;
          this.proportion = false;
        } else {
          this.UsernameMsg = this.UserNameAvailable;
          this.proportion = true;
          this.accountLinkError = '';
        }
      });
    }
  }

  GetColor() {
    if (!this.proportion) return "#a94442";
    else return "Green";
  }


  ValidateForm() {
    var Isvalid = true;
    var contactFormObject = this.contactForm.value;
    if (
      contactFormObject.confirmPassword != "" &&
      contactFormObject.password != contactFormObject.confirmPassword
    ) {
      this.IsPasswordMatch = false;
      Isvalid = false;
    } else {
      this.IsPasswordMatch = true;
    }
    if (
      contactFormObject.confirmPrimaryEmail != "" &&
      contactFormObject.primaryEmail != contactFormObject.confirmPrimaryEmail
    ) {
      this.IsPrimaryEmailMatch = false;
      Isvalid = false;
    } else {
      this.IsPrimaryEmailMatch = true;
    }
    if (
      contactFormObject.secondaryEmail !=
      contactFormObject.confirmSecondaryEmail
    ) {
      this.IsSecondaryEmailMatch = false;
      Isvalid = false;
    } else {
      this.IsSecondaryEmailMatch = true;
    }
    return Isvalid;
  }
  onSubmit() {

    
    this.registerUserFormService.accountLinkError = '';
    this.contactInfo = new ContactInfo();
    var contactFormObject = this.contactForm.value;
    // this.contactInfo.SecondaryEmail=contactFormObject.confirmSecondaryEmail;
    if (this.contactForm.valid && this.ValidateForm()) {
      this.contactInfo.PrimaryEmail = contactFormObject.primaryEmail;
      this.contactInfo.SecondaryEmail = contactFormObject.secondaryEmail;
      this.contactInfo.PrimaryPhone = contactFormObject.primaryPhone.replace(
        /\D+/g,
        ""
      );
      this.contactInfo.SecondaryPhone = contactFormObject.secondaryPhone.replace(
        /\D+/g,
        ""
      );
      this.contactInfo.UserName = contactFormObject.userName;
      this.contactInfo.Password = contactFormObject.password;
      this.contactInfo.SelectedLangCode = this.LanguageCode;
      this.registerUserFormService.setContactInfo(this.contactInfo);
      this.adbApiService
        .registerUser(this.registerUserFormService.getUserContract())
        .subscribe((result: any) => {
          if (result.HasError) {
            if (result.ErrorMessage == 'ShowExistingUsers') {
              this.registerUserFormService.lookupContracts = result.ResponseObject;
              this.router.navigate(["/registerUser/link-account"]);
            } else {
              this.hideUserCreationError = false;
              this.userCreationError = result.ErrorMessage;
        
              this.captchaRef.reset();
            }
          } else {
            this.registerUserFormService.handleAutoLogin(result);
          }
        });
    } else {
      this.utilityService.validateAllFormFields(this.contactForm);
    }
  }

  goBack() {
    var userInfo = this.registerUserFormService.getUserInfo();
    userInfo.IsAddressInfoAvail = false;
    this.registerUserFormService.setUserInfo(userInfo);
    this.registerUserFormService.accountLinkError = '';
    this.router.navigate(["registerUser/address"]);
  }
}