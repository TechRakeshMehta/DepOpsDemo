import { Component, OnInit } from '@angular/core';
import { RegisterUserFormService } from '../../../services/register-user/register-user-form.service';
import { LookupContract, LinkAccountContract } from '../../../models/user/user.model';
import { AdbApiService } from '../../../services/shared-services/adb-api-data.service';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/guard/auth-guard.service';
import { LanguageTranslationService } from '../../../services/language-translations/language-translations.service';
import { DataSharingService } from '../../../services/shared-services/data-sharing.service';
import { UtilityService } from "../../../services/shared-services/utility.service";

@Component({
  selector: 'app-link-account',
  templateUrl: './link-account.component.html',
  styleUrls: ['./link-account.component.css']
})
export class LinkAccountComponent implements OnInit {
  lookupContracts:LookupContract[] = [];
  selectedAccountCode:string = '';
  hideAccountLinkError:boolean=true;
  accountLinkError:string = '';

  constructor(private router: Router,
    private registerUserFormService: RegisterUserFormService,
    private adbApiService: AdbApiService,
    private loginDataSharingServive: AuthService, // for auto login
    public dataSharingService: DataSharingService, // for auto login
    private languageTranslationService: LanguageTranslationService,
    private utilityService: UtilityService) { 
    // var t = new LookupContract();
    // t.ID=1;
    // t.Code='asdas';
    // t.Name='asdas',
    // t.UserID='asdas'
    // var t1 = new LookupContract();
    // t1.ID=2;
    // t1.Code='asdas2';
    // t1.Name='asdas2',
    // t1.UserID='asdas2'
    // var t2 = new LookupContract();
    // t2.ID=2;
    // t2.Code='asdas3';
    // t2.Name='asdas3',
    // t2.UserID='asdas3'
    // this.lookupContracts.push(t);
    // this.lookupContracts.push(t1);
    // this.lookupContracts.push(t2);
    
  }

  onSubmit(){
    var linkAccountContract = new LinkAccountContract();
    linkAccountContract.User = this.registerUserFormService.getUserContract();
    linkAccountContract.LookupContract = this.lookupContracts.find(l=>l.UserID == this.selectedAccountCode);
    this.registerUserFormService.selectedAccountToLink = linkAccountContract.LookupContract;
    this.adbApiService
        .accountToLinkSelected(linkAccountContract,false)
        .subscribe((result: any) => {          
          if (result.HasError) {
            if(result.ResponseCode == 'VERIFYPASSWORD'){              
              this.router.navigate(["/registerUser/password-verification"]);
            }
            else if(result.ResponseCode == 'ACCWITHUSERNAMEEXISTS'){
              this.registerUserFormService.accountLinkError = result.ErrorMessage;
              this.router.navigate(["/registerUser/contact"]);
            }
            else if(result.ResponseCode == 'USERNAMEALREADYINUSE'){
              this.registerUserFormService.accountLinkError = result.ErrorMessage;
              this.router.navigate(["/registerUser/contact"]);
            }
            else if(result.ResponseCode == 'EMAILALREADYINUSE'){
              this.registerUserFormService.accountLinkError = result.ErrorMessage;
              this.router.navigate(["/registerUser/contact"]);
            }
            else{
              this.hideAccountLinkError = false;
              this.accountLinkError = result.ErrorMessage;              
            }            
          } else {
            this.registerUserFormService.handleAutoLogin(result);
          }
        });

  }

  ngOnInit() {
    this.setLocalization();
    this.lookupContracts =this.registerUserFormService.lookupContracts;
    if (this.lookupContracts == null 
      || this.lookupContracts.length <= 0) {
      this.router.navigate([""]);
    }
  }

  setLocalization() {
    var lstKeys = ["LINKACCOUNT","ALREADYEXISTINPROFILEMSG","NEXT","CNCL"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );    
  }

}
