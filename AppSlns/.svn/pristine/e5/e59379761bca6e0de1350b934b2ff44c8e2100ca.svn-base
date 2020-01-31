import { Component, OnInit } from '@angular/core';
import { CommonService } from '../../services/shared-services/common.service';
import { AppConsts } from '../../../environments/constants/appConstants';
import { UtilityService } from '../../services/shared-services/utility.service';
import { Validators, FormBuilder } from '@angular/forms';
import { KeyedCollection } from '../../models/common/dictionary.model';
import { BehaviorSubject } from 'rxjs';
import { LanguageTranslationService } from '../../services/language-translations/language-translations.service';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { AdbApiService } from '../../services/shared-services/adb-api-data.service';
import { OrderFlowService } from "../../services/order-flow/order-flow.service";
import { UserInfo } from '../../models/user/user.model';
import { OrderInfo } from "../../models/order-flow/order.model";

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {

  samePasswordError: any;
  oldPasswordError: any;
  OLDPASWRDCNTSAMETONEWPASWRD: any
  errorMsg: any;

  IsPasswordMatch: boolean = true;
  ChangePassword_validation_messages: {
    password: { type: string; message: any; }[];
    confirmPassword: { type: string; message: any; }[];
    oldpassword: { type: string; message: any; }[];
  };

  LanguageCode: string;
  IsEditProfile: boolean;
  IsPasswordExpiry: boolean = false;;
  hidePasswordExpiryMsg: boolean = true;
  passwordExpiryMsg: any;
  userInfo: UserInfo;



  passwordRegex =
    "(?=^.{8,15}$)^(?=.*[A-Z])(?=.*\\d)(?=.*[@#$%^_+~!?\\\\/\\'\\:\\,\\(\\){\\}\\[\\]\\-])[a-zA-Z0-9@#$%^_+~!?\\\\/\\'\\:\\,\\(\\)\\{\\}\\[\\]\\-]{8,}$";
  changePasswordForm = this.formBuilder.group({
    password: [
      "",
      [Validators.required, Validators.pattern(this.passwordRegex)]
    ],
    confirmPassword: ["", Validators.required],
    oldpassword: [""]
  });

  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  ValidationMsgsObservable = this.dataSource.asObservable();
  setLocalization() {
    var lstKeys = [
      "RECOVERUSERPSWD",
      "PLSENTEREMAIL",
      "REQUIREDMARK",
      "VERIFICATIONCODE",
      "EMAIL",
      "NWPASWD",
      "CNFMPASSWORD",
      "CNCL",
      "SUBMIT",
      "PWDDIDNOTMATCH",
      "RECOVER",
      "USERNAME",
      "PSWDCONDITION",
      "CHGEPASSWRD",
      "OLDPASWRD"
    ];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var lstValidationKeys = [
      "PASSWORDREQ",
      "INVALIDPASSWORD",
      "CNFMPASSWORDREQ",
      "PSWDCONDITION",
      "PASWRDSHOULDNTSAMETOOLDTENPASWRD",
      "OLDPASWRDCNTSAMETONEWPASWRD",
      "OLDPASWRDNOTMATCH",
      "OLDPASWRDREQ",
      "INVALIDOLDPASWRDENTEREMAILRECIEVEDPASWRD",
      "OLDPASWRDCNTSAMETONEWPASWRD","PWDEXPIRED","PSWRDCHGSUCC"

    ];
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
    this.ChangePassword_validation_messages = {
      password: [
        { type: "required", message: validationMsgs["PASSWORDREQ"] },
        { type: "pattern", message: validationMsgs["PSWDCONDITION"] }
      ],
      oldpassword: [
        { type: "required", message: validationMsgs["OLDPASWRDREQ"] },
      ],
      confirmPassword: [
        { type: "required", message: validationMsgs["CNFMPASSWORDREQ"] }
      ],


    };
    this.samePasswordError = validationMsgs["PASWRDSHOULDNTSAMETOOLDTENPASWRD"];
    this.oldPasswordError = validationMsgs["OLDPASWRDNOTMATCH"];
    this.OLDPASWRDCNTSAMETONEWPASWRD = validationMsgs["OLDPASWRDCNTSAMETONEWPASWRD"];
    this.passwordExpiryMsg = validationMsgs["PWDEXPIRED"];
    this.adbApiService.forgotPasswordTypeMsg = validationMsgs["PSWRDCHGSUCC"];
  }

  constructor(
    private commonService: CommonService,
    private utilityService: UtilityService,
    private languageTranslationService: LanguageTranslationService,
    private formBuilder: FormBuilder,
    private router: Router,
    private datepipe: DatePipe,
    private adbApiService: AdbApiService,
    private orderFlowService: OrderFlowService,
  ) { }
  orderInfo: OrderInfo;  
  ngOnInit() {    
    this.hidePasswordExpiryMsg = true;       
    this.setLocalization();
    this.IsEditProfile = this.orderFlowService.IsEditProfile;
    this.IsPasswordExpiry = this.orderFlowService.IsPasswordExpiry;
    if (this.IsEditProfile) {
      this.changePasswordForm.get("oldpassword").setValidators([Validators.required]);
    }

    if (this.IsPasswordExpiry) {  
      this.hidePasswordExpiryMsg = false;
      this.orderInfo = this.orderFlowService.getOrderInfo();
      this.adbApiService.ChangeUserPasswordEmail = this.orderInfo.userInfo.PrimaryEmail;      
    }



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
    return this.utilityService.isFieldValid(this.changePasswordForm, field);
  }
  isError(field: string) {
    return this.utilityService.isError(
      this.changePasswordForm,
      field,
      this.ChangePassword_validation_messages[field]
    );
  }
  errorMessage(field: string) {
    return this.utilityService.ErrorMessage(
      this.changePasswordForm,
      field,
      this.ChangePassword_validation_messages[field]
    );
  }
  displayFieldCss(field: string) {
    return this.utilityService.displayFieldCss(this.changePasswordForm, field);
  }

  OnSubmit() {
    this.hidePasswordExpiryMsg = true;    
    if (this.changePasswordForm.valid) {
      var changePasswordFormObject = this.changePasswordForm.value;
      if (!this.IsEditProfile) {
        if (changePasswordFormObject.confirmPassword != "" &&
          changePasswordFormObject.password != changePasswordFormObject.confirmPassword
        ) {
          this.IsPasswordMatch = false;
        }
        else {
          this.IsPasswordMatch = true;
          this.adbApiService.UpdatePassword(this.adbApiService.ChangeUserPasswordEmail.toString(),
            changePasswordFormObject.password).subscribe(s => {
              if (s.HasError == false) {
                this.adbApiService.forgotPasswordType = 2;                
                this.router.navigate([""]);
              }
              else {
                if (s.ErrorMessage == "OLDPASWRDCNTSAMETONEWPASWRD") {
                  this.errorMsg = this.samePasswordError;
                }

                if (s.ErrorMessage == "PASWRDSHOULDNTSAMETOOLDTENPASWRD")
                  this.errorMsg = this.samePasswordError;
              }
            });
        }

      }
      else {
        if (changePasswordFormObject.confirmPassword != "" &&
          changePasswordFormObject.password != changePasswordFormObject.confirmPassword
        ) {
          this.IsPasswordMatch = false;
          this.errorMsg = "";
        }
        else if (changePasswordFormObject.password == changePasswordFormObject.oldpassword
        ) {
          this.errorMsg = this.OLDPASWRDCNTSAMETONEWPASWRD;
          this.IsPasswordMatch = true;
        }
        else {
          this.IsPasswordMatch = true;
          this.adbApiService.ChangePassword(this.adbApiService.ChangeUserPasswordEmail.toString(),
            changePasswordFormObject.password, changePasswordFormObject.oldpassword).subscribe(s => {
              if (s.HasError == false) {
                this.adbApiService.forgotPasswordType = 2;            
                this.orderFlowService.IsPasswordExpiry = false;
                this.router.navigate([""]);
              }
              else {
                if (s.ErrorMessage == "OLDPASWRDNOTMATCH") {
                  this.errorMsg = this.oldPasswordError;
                }

                if (s.ErrorMessage == "PASWRDSHOULDNTSAMETOOLDTENPASWRD") {
                  this.errorMsg = this.samePasswordError;
                }

              }
            });
        }
      }

    }
    else
      this.utilityService.validateAllFormFields(this.changePasswordForm);

  }

  onCancelClick() {
    if (this.IsEditProfile) {
      this.orderFlowService.IsPasswordExpiry = false;
      if (this.IsPasswordExpiry) { 
        sessionStorage.clear();
        this.orderFlowService.setOrderInfoToEmpty();
        this.router.navigate(['']); } 
      else{ 
        this.router.navigate(["createOrder/contactInfo"]); }      
    }
    else {
      this.router.navigate(["/"]);
    }

  }

}
