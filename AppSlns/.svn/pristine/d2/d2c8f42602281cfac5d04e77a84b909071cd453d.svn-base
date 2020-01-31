import { Component, OnInit } from "@angular/core";
import { ContactInfo } from "../../../models/user/user.model";
import { FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { LookupService } from "../../../services/shared-services/lookup.service";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { AdbApiService } from "../../../services/shared-services/adb-api-data.service";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { UserInfo } from "../../../models/user/user.model";
import { OrderInfo } from "../../../models/order-flow/order.model";
import { conformToMask } from "angular2-text-mask";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { basename } from "path";
import { CommonService } from "../../../services/shared-services/common.service";
import { AppConsts } from "../../../../environments/constants/appConstants";

@Component({
  selector: "contact-info",
  templateUrl: "contactInfo.component.html"
})
export class ContactInfoComponent implements OnInit {
  EmailAddressNotMatchMsg: any;
  color: string;
  public mask = ['(', /[0-9]/, /\d/, /\d/, ')', '-', /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/]
  emailRegex = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
  passwordRegex = "(?=^.{8,15}$)^(?=.*[A-Z])(?=.*\\d)(?=.*[@#$%^_+~!?\\\\/\\'\\:\\,\\(\\)\{\\}\\[\\]\\-])[a-zA-Z0-9@#$%^_+~!?\\\\/\\'\\:\\,\\(\\)\\{\\}\\[\\]\\-]{8,}$";
  primaryPhoneRegex = "^[(][0-9]{3}[)-]+[0-9]{3}[-]+[0-9]{4}$";
  usernameRegex = "^[.@a-zA-Z0-9_-]{4,50}$";
  contactForm = this.formBuilder.group({
    IsReceiveTextNotification: [""],
    cellularPhone: ["", [Validators.required, Validators.pattern(this.primaryPhoneRegex)]],
    //primaryEmail: ["",Validators.required,Validators.pattern("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")],
    primaryEmail: ["", Validators.compose([
      Validators.required,
      Validators.pattern(this.emailRegex)])],
    secondaryEmail: ["", [Validators.pattern(this.emailRegex)]],
    primaryPhone: ["", [Validators.required, Validators.pattern(this.primaryPhoneRegex)]],
    secondaryPhone: [""],
    confirmSecondaryEmail: ["", Validators.pattern(this.emailRegex)],
    userName: [
      "",
      [Validators.required, Validators.pattern(this.usernameRegex)]
    ],
    confirmPrimaryEmail: [
      "",
      [Validators.required, Validators.pattern(this.emailRegex)]
    ],
    PassRecEmail: [""],
    UpdateAspnetEmail: [false],
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
  hideUserUpdateMsg: boolean = true;
  ErrorMessage: string = "";
  hideUserCreationError: boolean = true;
  userCreationError: string = "";
  contact_validation_messages: any;
  IsEditProfile: boolean;
  UserNameAvailable: any;
  UsernameAlreadyInUseMsg: any;
  LanguageCode: string = "";
  IsEmailReadOnly: boolean = true;
  IsShowUserNameMsg: boolean = false;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();

  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );

  ValidationMsgsObservable = this.dataSource.asObservable();
  setLocalization() {
    //labels
    var lstKeys = ["EMAIL", "SECEMAIL", "CSECEMAIL", "PRPHONE", "SECPHONE",
      "RECIEVETEXTNOTIFICATION", "CELLULARPHNNUM", "PREVIOUS", "NEXT",
      "PERSONALINFO", "CREATODR", "STEP", "CNFMCNLORDER", "CNCLORDR", "CNCL",
      , "CHECK", "CNFRMMAIL", "CHGEPASSWRD", "SNDPWDRECVYEMAILALSOPRMRYADDRS", "PWDRECVRYEMAIL","USERNAME"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var lstKeysDuplicates = ["LSTITMY", "LSTITMN", "CNCL", "UPDATE"];
    this.utilityService.SubscribeLocalListWithDupNames(
      this.languageTranslationService,
      lstKeysDuplicates
    );
    //validaton msgs    
    var lstValidaionKeys = ["EMAILADDRESSREQ", "EMAILNOTVALID", "CNFMEMAILADDRESSREQ", "EMAILNOTVALID",
      "PASSWORDREQ", "INVALIDPASSWORD", "CNFMPASSWORDREQ", "USERNAMEREQ", "PHONEREQ", "PHNFORMAT",
      "EMAILADDRESSNOTMATCH", "USERALREADYEXIST", "USERALREADYEXIST", "DATAUPDATEDSUCCESSFULLY", "USERNAMEAVLBLE", 
      "USERNAMEALREADYINUSE","INVALIDUSERNAME"];
    this.utilityService.PopulateValidationCollectionFromKeys(lstValidaionKeys, this.validationMessages);

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
        {
          type: 'required', message: validationMsgs["CNFMEMAILADDRESSREQ"]
        },
        { type: 'pattern', message: validationMsgs["EMAILNOTVALID"] }
      ],
      password: [
        { type: 'required', message: validationMsgs["PASSWORDREQ"] },
        { type: 'pattern', message: validationMsgs["INVALIDPASSWORD"] }
      ],
      confirmPassword: [
        { type: 'required', message: validationMsgs["CNFMPASSWORDREQ"] },
      ],
      userName: [
        { type: 'required', message: validationMsgs["USERNAMEREQ"] },
        { type: "pattern", message: validationMsgs["INVALIDUSERNAME"] } 
      ],
      primaryPhone: [
        { type: 'required', message: validationMsgs["PHONEREQ"] },
        { type: 'pattern', message: validationMsgs["PHNFORMAT"] }
      ],
      secondaryEmail: [
        { type: 'pattern', message: validationMsgs["EMAILNOTVALID"] }
      ],
      confirmSecondaryEmail: [
        { type: 'pattern', message: validationMsgs["EMAILNOTVALID"] }
      ],
      cellularPhone: [
        { type: 'required', message: validationMsgs["PHONEREQ"] },
        { type: 'pattern', message: validationMsgs["PHNFORMAT"] }
      ]



    }
    this.EmailAddressNotMatchMsg = validationMsgs["EMAILADDRESSNOTMATCH"];
    this.userCreationError = validationMsgs["DATAUPDATEDSUCCESSFULLY"];
    this.UsernameAlreadyInUseMsg = validationMsgs["USERNAMEALREADYINUSE"];
    this.UserNameAvailable = validationMsgs["USERNAMEAVLBLE"];

    if (this.ShowUserNameMsg) {
      this.CheckUser();
    }
  }

  constructor(
    private orderFlowService: OrderFlowService,
    private formBuilder: FormBuilder,
    private router: Router,
    private lookupService: LookupService,
    private adbApiService: AdbApiService,
    private utilityService: UtilityService,
    private languageTranslationService: LanguageTranslationService,
    private commonService: CommonService
  ) { }


  userInfo: UserInfo;
  orderInfo: OrderInfo;
  hdnRcvTextNotification: boolean = false;
  ngOnInit() {
    this.userInfo = this.orderFlowService.getOrganizationUserInfoDetails();
    this.orderInfo = this.orderFlowService.getOrderInfo();
    this.IsEditProfile = this.orderFlowService.IsEditProfile;
    if (this.IsEditProfile) {
      this.IsEmailReadOnly = false;
      this.adbApiService.ChangeUserPasswordEmail = this.userInfo.PswdRecoveryEmail;
    }


    this.setLocalization();
    this.populateFormControls();

    if (this.commonService.isLanguageSpanish) {
      this.commonService.isLanguageSpanish.subscribe(isSpanishLang => {
        this.setLocalizationFields(isSpanishLang);
      });
    }
  }
  setLocalizationFields(isSpanishLanguage: boolean) {
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
    return this.utilityService.isError(this.contactForm, field, this.contact_validation_messages[field]);
  }
  errorMessage(field: string) {
    return this.utilityService.ErrorMessage(this.contactForm, field, this.contact_validation_messages[field]);
  }
  displayFieldCss(field: string) {
    return this.utilityService.displayFieldCss(this.contactForm, field);
  }

  populateFormControls() {
    // populate dropdowns
    var contactInfo = this.userInfo;
    var formControls = this.contactForm.controls;
    formControls["primaryEmail"].setValue(contactInfo.PrimaryEmail);
    formControls["secondaryEmail"].setValue(contactInfo.SecondaryEmail);
    formControls["userName"].setValue(contactInfo.UserName);
    formControls["confirmPrimaryEmail"].setValue(contactInfo.PrimaryEmail);
    formControls["PassRecEmail"].setValue(contactInfo.PswdRecoveryEmail);

    var primaryPhone = conformToMask(
      contactInfo.PrimaryPhone,
      this.mask,
      { guide: false }
    )

    var smsPhone = conformToMask(
      contactInfo.SMSPhoneNumber,
      this.mask,
      { guide: false }
    )

    formControls["primaryPhone"].setValue(primaryPhone.conformedValue);
    formControls["secondaryPhone"].setValue(contactInfo.SecondaryPhone);
    formControls["confirmSecondaryEmail"].setValue(contactInfo.SecondaryEmail);
    formControls["cellularPhone"].setValue(smsPhone.conformedValue);
    if (contactInfo.IsReceiveTextNotification) {
      formControls["IsReceiveTextNotification"].setValue("1");
      formControls["cellularPhone"].setValue(smsPhone.conformedValue);
      formControls["cellularPhone"].setValidators([Validators.required, Validators.pattern(this.primaryPhoneRegex)]);
      this.hdnRcvTextNotification = false;
    }
    else {
      formControls["IsReceiveTextNotification"].setValue("0");
      formControls["cellularPhone"].clearValidators();
      this.hdnRcvTextNotification = true;
    }


  }
  CheckUser() {
    var contactFormObject = this.contactForm.value;
    var userName = contactFormObject.userName;

    this.color = "#a94442";
    if (userName != null && userName != "") {
      this.ShowUserNameMsg = true;
      this.adbApiService.IsUserAlreadyExists(userName).subscribe(s => {
        if (s.Message == "True") {

          //this.UsernameMsg = "This username is already in use. Try another?";
          this.UsernameMsg = this.UsernameAlreadyInUseMsg;
          this.proportion = false;
        }
        else {
          // this.UsernameMsg = "This Username is available.";
          this.UsernameMsg = this.UserNameAvailable;
          this.proportion = true;
        }
      })
    }
  }

  GetColor() {
    if (!this.proportion)
      return "#a94442";
    else
      return "Green";
  }
  onSubmit() {

    var Isvalid = true;

    this.contactInfo = new ContactInfo();
    var contactFormObject = this.contactForm.value;


    if ((contactFormObject.secondaryEmail != contactFormObject.confirmSecondaryEmail)) {
      this.IsSecondaryEmailMatch = false;
      Isvalid = false;
    }
    else {
      this.IsSecondaryEmailMatch = true;
    }

    // this.contactInfo.SecondaryEmail=contactFormObject.confirmSecondaryEmail;
    if (this.contactForm.valid && Isvalid) {
      if (this.userInfo.IsReceiveTextNotification != undefined) {
        //&& contactFormObject.IsReceiveTextNotification != this.userInfo.IsReceiveTextNotification) {
        if (contactFormObject.IsReceiveTextNotification == "1") {
          this.userInfo.IsReceiveTextNotification = true;
          this.userInfo.SMSPhoneNumber = contactFormObject.cellularPhone.replace(/\D+/g, '');
        }
        else {
          this.userInfo.IsReceiveTextNotification = false;
        }
      }
      this.userInfo.PrimaryEmail = contactFormObject.primaryEmail;
      this.userInfo.SecondaryEmail = contactFormObject.secondaryEmail;
      this.userInfo.PrimaryPhone = contactFormObject.primaryPhone.replace(/\D+/g, '');
      this.userInfo.SecondaryPhone = contactFormObject.secondaryPhone.replace(/\D+/g, '');

      this.orderFlowService.setOrganizationUserInfoDetails(this.userInfo);
      this.orderFlowService.IncrementStep();
      this.router.navigate(["createOrder/fingerPrinting"]);
    } else {
      this.utilityService.validateAllFormFields(this.contactForm);
    }
  }

  goBack() {
    this.router.navigate(["createOrder/addressInfo"]);
  }

  HideTxtNotification(hdnRcvTextNotification: boolean) {
    this.hdnRcvTextNotification = hdnRcvTextNotification;
    var formControls = this.contactForm.controls;
    formControls["cellularPhone"].clearValidators();
    formControls["cellularPhone"].setValue("");
    this.utilityService.validateAllFormFields(this.contactForm);
  }
  ShowTxtNotification(hdnRcvTextNotification: boolean) {
    var contactInfo = this.userInfo;
    if (contactInfo.SMSPhoneNumber != "" && contactInfo.SMSPhoneNumber != null && contactInfo.SMSPhoneNumber != undefined) {
      var smsPhone = conformToMask(
        contactInfo.SMSPhoneNumber,
        this.mask,
        { guide: false }
      )
    } else {
      var smsPhone = conformToMask(
        contactInfo.PrimaryPhone,
        this.mask,
        { guide: false }
      )
    }
    this.hdnRcvTextNotification = false;
    var formControls = this.contactForm.controls;
    formControls["cellularPhone"].setValue(smsPhone.conformedValue);
    formControls["cellularPhone"].setValidators([Validators.required, Validators.pattern(this.primaryPhoneRegex)]);

  }
  onUpdate() {
    this.UsernameMsg = "";
    var contactFormObject = this.contactForm.value;
    if(this.userInfo.UserName !=contactFormObject.userName) //if user name changed then check its exist or not
    {
      this.adbApiService.IsUserAlreadyExists(contactFormObject.userName).subscribe(s => {
        
        //if user name exist then show msg 
        if (s.Message == "True") { 
          //this.UsernameMsg = "This username is already in use. Try another?";
          this.UsernameMsg = this.UsernameAlreadyInUseMsg;
          this.proportion = false;
          this.hideUserUpdateMsg =true;
        }
        //else update user profile
        else
        {
           this.update()
        }
      })
    }
    //if not changed then update on same user name
    else
    (
      this.update()
    )
   

  }
  update()
  {
    var Isvalid = true;
    this.contactInfo = new ContactInfo();
    var contactFormObject = this.contactForm.value;
    if (
      contactFormObject.confirmPrimaryEmail != "" &&
      contactFormObject.primaryEmail != contactFormObject.confirmPrimaryEmail
    ) {
      this.IsPrimaryEmailMatch = false;
      Isvalid = false;
    } else {
      this.IsPrimaryEmailMatch = true;
    }

    if ((contactFormObject.secondaryEmail != contactFormObject.confirmSecondaryEmail)) {
      this.IsSecondaryEmailMatch = false;
      Isvalid = false;
    }
    else {
      this.IsSecondaryEmailMatch = true;
    }

    // this.contactInfo.SecondaryEmail=contactFormObject.confirmSecondaryEmail;
    if (this.contactForm.valid && Isvalid) {
      if (this.userInfo.IsReceiveTextNotification != undefined) {
        //&& contactFormObject.IsReceiveTextNotification != this.userInfo.IsReceiveTextNotification) {
        if (contactFormObject.IsReceiveTextNotification == "1") {
          this.userInfo.IsReceiveTextNotification = true;
          this.userInfo.SMSPhoneNumber = contactFormObject.cellularPhone.replace(/\D+/g, '');
        }
        else {
          this.userInfo.IsReceiveTextNotification = false;
        }
      }
      this.userInfo.PrimaryEmail = contactFormObject.primaryEmail;
      this.userInfo.SecondaryEmail = contactFormObject.secondaryEmail;
      this.userInfo.PrimaryPhone = contactFormObject.primaryPhone.replace(/\D+/g, '');
      this.userInfo.SecondaryPhone = contactFormObject.secondaryPhone.replace(/\D+/g, '');
      this.userInfo.UpdateAspnetEmail = contactFormObject.UpdateAspnetEmail;
      if (this.IsEditProfile) {
        this.userInfo.UserName = contactFormObject.userName;
      }
      this.orderFlowService.setOrganizationUserInfoDetails(this.userInfo);
      this.orderFlowService.UpdateUserDetails(this.orderInfo).subscribe(res => {
        if (res.HasError == false) {
          this.hideUserUpdateMsg = false;
          this.UsernameMsg = "";
          if (this.userInfo.UpdateAspnetEmail) {
            this.contactForm.controls["PassRecEmail"].setValue(this.userInfo.PrimaryEmail);
            this.contactForm.controls["UpdateAspnetEmail"].setValue(false);
            this.adbApiService.ChangeUserPasswordEmail = this.userInfo.PrimaryEmail;
            this.userInfo.PswdRecoveryEmail=this.userInfo.PrimaryEmail;
          }
        }

        //this.orderFlowService.IsEditProfile=false;
        //  this.router.navigate(["userDashboard"]);
      })

    } else {
      this.utilityService.validateAllFormFields(this.contactForm);
    }

  }
}
