import { Component, OnInit } from '@angular/core';
import { RegisterUserFormService } from '../../../services/register-user/register-user-form.service';
import { LookupContract, LinkAccountContract } from '../../../models/user/user.model';
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { AdbApiService } from '../../../services/shared-services/adb-api-data.service';
import { AuthService } from '../../../services/guard/auth-guard.service';
import { DataSharingService } from '../../../services/shared-services/data-sharing.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-password-verification',
  templateUrl: './password-verification.component.html',
  styleUrls: ['./password-verification.component.css']
})
export class PasswordVerificationComponent implements OnInit {
  lookupContracts:LookupContract[] = [];
  userName:string = '';
  password:string = '';
  hideAccountLinkError:boolean=true;
  accountLinkError:string = '';
  hdnSubmitButton: boolean = false;
  incorrectPwdCount: number = 0;

  constructor(private router: Router,
    private registerUserFormService: RegisterUserFormService,
    private languageTranslationService: LanguageTranslationService,
    private utilityService: UtilityService,
    private adbApiService: AdbApiService,
    private loginDataSharingServive: AuthService, // for auto login
    public dataSharingService: DataSharingService, // for auto login
  ) { 
        
  }

  ngOnInit() {
    this.setLocalization();
    this.hdnSubmitButton = false;   
    this.incorrectPwdCount = 0;   
    var accnt = this.registerUserFormService.selectedAccountToLink;
    if(accnt != null){
      this.userName = accnt.Code;
    }else{
      this.userName = '';
    }
    if (this.userName == null 
      ||this.userName == '') {
      this.router.navigate([""]);
    }
  }

  setLocalization() {
    var lstKeys = ["LINKACCOUNT","PSWDVERIFICATION","USERNAME","PASSWORD","CRTNEW","SUBMIT","CNCL","TRYAGAIN"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );    
  }

  onPasswordverification(){
    this.hideAccountLinkError = true;
    this.accountLinkError = '';

    var linkAccountContract = new LinkAccountContract();
    linkAccountContract.User = this.registerUserFormService.getUserContract();
    linkAccountContract.LookupContract = this.registerUserFormService.selectedAccountToLink
    linkAccountContract.VerificationPassword = this.password;
    this.adbApiService
        .LinkAccount(linkAccountContract)
        .subscribe((result: any) => {       
          if (result.HasError) {
            this.hideAccountLinkError = false;
            this.accountLinkError = result.ErrorMessage;    
            this.hdnSubmitButton = true;    
            this.incorrectPwdCount = this.incorrectPwdCount + 1;   
          } else {
            this.registerUserFormService.SelectedUserPrimaryEmail = result.ResponseObject;
            this.router.navigate(["/registerUser/account-linked"]);
          }
        });
  }

  onRegisterNew(){
    this.hideAccountLinkError = true;
    this.accountLinkError = '';

    var linkAccountContract = new LinkAccountContract();
    linkAccountContract.User = this.registerUserFormService.getUserContract();
    linkAccountContract.LookupContract = this.registerUserFormService.selectedAccountToLink;

    this.adbApiService
        .accountToLinkSelected(linkAccountContract,true)
        .subscribe((result: any) => {     
          if (result.HasError) {
            switch(result.ResponseCode)  {
              case"USERNAMEALREADYINUSE":
              case"EMAILALREADYINUSE":
              case"USERNAMEEXIST":
              case"PLSSELVALIDZIP":
              this.registerUserFormService.accountLinkError = result.ErrorMessage;
              this.router.navigate(["/registerUser/contact"]);
              break;
              default:
              this.hideAccountLinkError = false;
              this.accountLinkError = result.ErrorMessage;
              break;
            }            
          } else {
            this.registerUserFormService.handleAutoLogin(result);
          }
        });

  }
}
