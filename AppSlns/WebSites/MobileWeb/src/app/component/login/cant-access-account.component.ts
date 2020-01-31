import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UtilityService } from '../../services/shared-services/utility.service';
import { DatePipe } from '@angular/common';
import { LanguageTranslationService } from '../../services/language-translations/language-translations.service';
import { KeyedCollection } from '../../models/common/dictionary.model';
import { BehaviorSubject } from 'rxjs';
import { AdbApiService } from '../../services/shared-services/adb-api-data.service';
import { CommonService } from '../../services/shared-services/common.service';
import { AppConsts } from '../../../environments/constants/appConstants';
import { OrderFlowService } from "../../services/order-flow/order-flow.service";

@Component({
  selector: 'app-cant-access-account',
  templateUrl: './cant-access-account.component.html',
  styleUrls: ['./cant-access-account.component.css'],
})
export class CantAccessAccountComponent implements OnInit {
  enterCorrectVerificationCode: any;
  verificationCodeRequired: any;
  LanguageCode: string;
  errorMsg: any;
  isEmailDisabled: boolean;
  isSelectedTypeDisabled: boolean;
  RegisteredEmailAddress: any;
  EnterCorrectVerificationCode: any;
  VerificationCodeEmailSntInfo: any;
  IsgenCodeDisabled: boolean = false;
  IsSubmitDisabled: boolean = true;
  isVerificateCodeDisabled: boolean = true;
  ForgotPassword_validation_messages: any;
  ShowErrorMsg: boolean = false;
  ShowInvalidVerificationCodeMsg: boolean = false;

  emailRegex = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
  forgotPasswordForm = this.formBuilder.group({
    email: [
      "",
      Validators.compose([
        Validators.required,
        Validators.pattern(this.emailRegex)
      ],
      )],
    verificationCode: [""],
    selectedType: [""]
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
      "GENRTCODE",
      "SUBMIT",
      "CNCL",
      "RECOVER",
      "USERNAME",
      "PASSWORD",
      "RECOVERACCOUNT"
    ];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var lstValidationKeys = [
      "EMAILADDRESSREQ",
      "NTCORRECTFORMATEMAIL",
      "CODEREQ",
      "VERIFICATIONCODEEMAILSNTINFOMSG",
      "ENTRCRCTVERIFICATIONCODE",
      "REGISTEREDEMAILREQUIRED",
      "ENTRCRCTVERIFICATIONCODE",
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
    this.ForgotPassword_validation_messages = {
      email: [
        { type: "required", message: validationMsgs["EMAILADDRESSREQ"] },
        { type: "pattern", message: validationMsgs["NTCORRECTFORMATEMAIL"] }
      ],

    };
    this.VerificationCodeEmailSntInfo = validationMsgs["VERIFICATIONCODEEMAILSNTINFOMSG"];
    this.EnterCorrectVerificationCode = validationMsgs["ENTRCRCTVERIFICATIONCODE"];
    this.RegisteredEmailAddress = validationMsgs["REGISTEREDEMAILREQUIRED"];
    this.verificationCodeRequired = validationMsgs["CODEREQ"];
    this.enterCorrectVerificationCode = validationMsgs["ENTRCRCTVERIFICATIONCODE"];

  }
  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private utilityService: UtilityService,
    private datepipe: DatePipe,
    private languageTranslationService: LanguageTranslationService,
    private adbApiService: AdbApiService,
    private commonService: CommonService,
    private orderFlowService: OrderFlowService,
  ) { }

  ngOnInit() {
    this.orderFlowService.IsEditProfile=false;
    this.setLocalization();
    var formControls = this.forgotPasswordForm.controls;
    formControls["selectedType"].setValue("2");

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
    return this.utilityService.isFieldValid(this.forgotPasswordForm, field);
  }
  isError(field: string) {
    return this.utilityService.isError(
      this.forgotPasswordForm,
      field,
      this.ForgotPassword_validation_messages[field]
    );
  }
  errorMessage(field: string) {
    return this.utilityService.ErrorMessage(
      this.forgotPasswordForm,
      field,
      this.ForgotPassword_validation_messages[field]
    );
  }
  displayFieldCss(field: string) {
    return this.utilityService.displayFieldCss(this.forgotPasswordForm, field);
  }

  generateCode() {

    if (this.forgotPasswordForm.controls['email'].status == "VALID") {
      this.ShowErrorMsg = true;
      var forgotPasswordFormObject = this.forgotPasswordForm.value;
      var selectedType = +forgotPasswordFormObject.selectedType;
      var email = forgotPasswordFormObject.email;
      this.adbApiService.GenerateVerificationCode(email, selectedType).subscribe(s => {

        if (s.HasError == true) {
          switch (s.ErrorMessage) {
            case "REGISTEREDEMAILREQUIRED": {
              this.errorMsg = this.RegisteredEmailAddress;
              break;
            }

            default: break;
          }  // 

        }
        else {
          this.isSelectedTypeDisabled = true;
          this.isEmailDisabled = true;
          this.IsgenCodeDisabled = true;
          this.IsSubmitDisabled = false;
          this.isVerificateCodeDisabled = false;
          switch (s.ErrorMessage) {
            case "VERIFICATIONCODEEMAILSNTINFOMSG": {
              this.errorMsg = this.VerificationCodeEmailSntInfo;
              break;
            }
            default: break;
          }  // 
        }
      });
    }

    else {
      this.utilityService.validateAllFormFields(this.forgotPasswordForm);
    }
  }

  onSubmit() {
    this.ShowInvalidVerificationCodeMsg = true;
    var forgotPasswordFormObject = this.forgotPasswordForm.value;
    var verificationCode = forgotPasswordFormObject.verificationCode;
    var selectedType = +forgotPasswordFormObject.selectedType;
    var email = forgotPasswordFormObject.email;
    if (verificationCode.length > 0) {
      if (verificationCode.indexOf(".") + 1 < 1)
{
        this.adbApiService.ValidateVerificationCode(email, verificationCode, selectedType).subscribe(s => {

          if (s.HasError == true)
            this.errorMsg = this.enterCorrectVerificationCode;
          else if (selectedType == 1) {
            this.adbApiService.forgotPasswordType = 1;
            this.router.navigate([""]);
          }
          else {
            this.adbApiService.setChangeUserPasswordEmail(email);
            this.router.navigate(["/changePassword"]);
          }
        });
      }
      else
      this.errorMsg = this.enterCorrectVerificationCode;
    }
    else {
      this.errorMsg = this.verificationCodeRequired;
    }
  }
}
