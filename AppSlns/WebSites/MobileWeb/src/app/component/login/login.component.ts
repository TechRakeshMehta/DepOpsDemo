import { Component, OnInit, ViewChildren, AfterViewInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { AuthService } from "../../services/guard/auth-guard.service";
import { DataSharingService } from "../../services/shared-services/data-sharing.service";
import { Subscription } from "rxjs";
import { AppConstant } from "../../models/common/AppConst";
import { AdbApiService } from "../../services/shared-services/adb-api-data.service";
import { CommonService } from "../../services/shared-services/common.service";
import { LanguageTranslationComponent } from "../common/languageTranslation.component";
import { LanguageTranslation } from "../../models/language-translation/language-translation-contract.model";
import { LanguageTranslationService } from "../../services/language-translations/language-translations.service";
import { Key } from "selenium-webdriver";
import { RegisterUserFormService } from "../../services/register-user/register-user-form.service";
import { UtilityService } from "../../services/shared-services/utility.service";
import {
  UserInfo
} from "../../models/user/user.model";
import { KeyedCollection } from "../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { AppConsts } from "../../../environments/constants/appConstants";
import { LanguageParams } from "../../models/language-translation/language-translation-contract.model";
import { BsModalService } from "ngx-bootstrap/modal";
import { ModalContentComponent } from "../confirmation-box/confirmation-modal.component";
import { OrderFlowService } from "../../services/order-flow/order-flow.service";

declare var window: any;
@Component({
  selector: "login-app",
  templateUrl: "./login.html",
  //templateUrl: "./login.html?v=${new Date().getTime()}",
})
export class LoginComponent implements OnInit {

  successMsgPasswordChangedSuccessfully: any;
  successMsgUserNameChangedSuccessfully: any;
  ForgotUserErrorMsg: string;
  hideForgotUserErrorMsg: boolean = true;


  private userInfo: UserInfo = new UserInfo();
  private userAuthenticationData: any;
  public complioLogo: string;
  public errorMsg: string = "";
  private tenantName: string;
  private isCbiUrl: string;
  hideErrorMsg: boolean = true;
  login_validation_messages: any;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  public errorMsgIncorrectPassword: string;
  public errorMsgAccountLock: string;
  public errorMsgAccountInActive: string;
  hideErrorMsgIncorrectPassword: boolean = true;
  hideErrorMsgLocked: boolean = true;
  hideErrorMsgInActive: boolean = true;
  UsrVerCode: string;

  constructor(
    private commonService: CommonService,
    private loginDataSharingServive: AuthService,
    public appConstant: AppConstant,
    public dataSharingService: DataSharingService,
    public router: Router,
    private adbApiService: AdbApiService,
    private languageTranslationService: LanguageTranslationService,
    private registerUserFormService: RegisterUserFormService,
    private utilityService: UtilityService,
    private activeRoute: ActivatedRoute,
    private modalService: BsModalService,
    private orderFlowService: OrderFlowService,
  ) {
    this.complioLogo = this.appConstant.complioLogo;
    this.adbApiService.getTenantName().subscribe(userData => {
      this.tenantName = userData.TenantName;
    });

    this.adbApiService.getCbiUrlInfo().subscribe(userData => {
      this.isCbiUrl = userData.Message;
      this.commonService.SetLoggedIn(false);
      var lstKeys = ["USERNAME", "PASSWORD", "LOGIN", "REGISTER", "RETURNTODESKTOP", "INCORRECTUSRPWD", "YOURACCTLOCKEDMSG", "USERINACTIVE", "CANTACCESSACCOUNT"];
      this.utilityService.SubscribeLocalList(this.languageTranslationService, lstKeys);
    });
  }

  ngOnInit(): void {
    this.setLocalization();

    if (this.adbApiService.forgotPasswordType == 1) {
      this.hideForgotUserErrorMsg = false;
      this.ForgotUserErrorMsg = this.successMsgUserNameChangedSuccessfully;
    }
    else if (this.adbApiService.forgotPasswordType == 2) {
      this.hideForgotUserErrorMsg = false;
      this.ForgotUserErrorMsg = this.adbApiService.forgotPasswordTypeMsg;
    }
    this.adbApiService.forgotPasswordType = 0;
    this.activeRoute.queryParams
      .filter(params => params.UsrVerCode)
      .subscribe(params => {
        this.UsrVerCode = params.UsrVerCode;
        if (this.UsrVerCode != undefined) {
          this.adbApiService.UsrVerCode = this.UsrVerCode;
          this.adbApiService.ValidateUserViaEmailAndRedirect(this.UsrVerCode).subscribe(message => {
            if (message.length > 0) {
              if (message == 'ShowAdditionalAccountVerificationPage') {
                this.router.navigate(["AdditionAccountVerification"]);
              }
              else {
                const initialState = {
                  message: message,
                  title: '',
                  confirmBtnName: '',
                  closeBtnName: 'Continue',
                  hideConfirmBtn: true,
                  hideCloseBtn: false
                };
                this.modalService.show(ModalContentComponent, { initialState });
              }
            }

          }
          );
        }
      });
  }



  ValidationMsgsObservable = this.dataSource.asObservable();
  setLocalization() {
    //validaton msgs
    var lstValidaionKeys = [
      "INCORRECTUSRPWD", "YOURACCTLOCKEDMSG", "USERINACTIVE", "USRNMESNDEMLADDSUC", "PSWRDCHGSUCC"
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
    this.errorMsgIncorrectPassword = validationMsgs["INCORRECTUSRPWD"];
    this.errorMsgAccountLock = validationMsgs["YOURACCTLOCKEDMSG"];
    this.errorMsgAccountInActive = validationMsgs["USERINACTIVE"];
    this.successMsgUserNameChangedSuccessfully = validationMsgs["USRNMESNDEMLADDSUC"];
    this.successMsgPasswordChangedSuccessfully = validationMsgs["PSWRDCHGSUCC"];
  }

  submitLoginInfo(loginInfo: any) {
    this.hideForgotUserErrorMsg = true;
    this.loginDataSharingServive
      .postUserLoginInfo(loginInfo)
      .subscribe(holdsObjects => {
        this.dataSharingService.loginDataObj = holdsObjects;
        if (holdsObjects != null || holdsObjects != undefined) {
          sessionStorage.setItem("auth_token", holdsObjects.access_token);
          sessionStorage.setItem("refresh_token", holdsObjects.refresh_token);
        }
        if (holdsObjects.status == 400) {
          this.errorMsg = holdsObjects.error.error_description;
          if (this.errorMsg.includes("Your account has been locked.")) {
            this.hideErrorMsgLocked = false;
            this.hideErrorMsgIncorrectPassword = true;
            this.hideErrorMsgInActive = true;
          }
          else if (this.errorMsg.includes("Your account is not active.")) {
            this.hideErrorMsgInActive = false;
            this.hideErrorMsgLocked = true;
            this.hideErrorMsgIncorrectPassword = true;
          }
          else if (this.errorMsg.includes("The user name or password is incorrect")) {
            this.hideErrorMsgIncorrectPassword = false;
            this.hideErrorMsgInActive = true;
            this.hideErrorMsgLocked = true;
          }
          else {
            this.hideErrorMsgIncorrectPassword = false;
            this.hideErrorMsgInActive = true;
            this.hideErrorMsgLocked = true;
          }

        } else if (holdsObjects.status != undefined) {
          // this.errorMsg = holdsObjects.statusText;
          this.hideErrorMsg = false;
        } else {
          this.adbApiService.IsPasswordExpired().subscribe(message => {
            if (message.Message == "True") {
              this.orderFlowService.IsEditProfile=true;
              this.orderFlowService.IsPasswordExpiry=true;
              this.orderFlowService.getOrganizationUser().then(()=>{               
                this.router.navigate(["/changePassword"]);               
              });                  
            }
            else {
              this.loginDataSharingServive.setLoggedIn(true);
              if (this.isCbiUrl == "True") {
                this.router.navigate(["/userDashboard"]);
              } else {
                this.router.navigate(["/welcome"]);
              }
            }
          }
          );
        }
      });
  }
  registerUser() {
    var tempUrl = window.location.origin;
    this.registerUserFormService.resetFormData();
    this.userInfo = this.registerUserFormService.getUserInfo();
    this.userInfo.IsValidUrl = true;
    this.registerUserFormService.setUserInfo(this.userInfo);
    //window.location.href = tempUrl  + "/registerUser";
    this.router.navigate(["/registerUser"]);
  }

  returnToDesktop() {
    var tempUrl = window.location.origin;
    tempUrl = tempUrl.replace("m.", "");
    tempUrl = tempUrl.replace("m-", "");
    tempUrl = tempUrl.replace("/MobileWebApi", "");
    tempUrl = tempUrl.replace("/#/", "");
    tempUrl = tempUrl.replace(".com/", ".com");
    window.location.href = tempUrl + "?isReturnToDesktopSite=1";
  }
}
