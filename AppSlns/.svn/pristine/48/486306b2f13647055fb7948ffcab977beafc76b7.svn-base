import { Component, OnInit } from '@angular/core';
import { RegisterUserFormService } from '../../../services/register-user/register-user-form.service';
import { LookupContract } from '../../../models/user/user.model';
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { Router } from '@angular/router';

@Component({
  selector: 'app-account-linked',
  templateUrl: './account-linked.component.html',
  styleUrls: ['./account-linked.component.css']
})
export class AccountLinkedComponent implements OnInit {
  lookupContracts:LookupContract[] = [];
  Email:string = '';
  constructor(private router: Router,
    private registerUserFormService: RegisterUserFormService,
    private languageTranslationService: LanguageTranslationService,
    private utilityService: UtilityService
  ) { 
        
  }

  ngOnInit() {
    this.setLocalization();
    this.Email = this.registerUserFormService.SelectedUserPrimaryEmail;
    if (this.Email == '') {
      this.router.navigate([""]);
    }
  }

  onRedirectToLogin(){
    this.router.navigate([""]);
  }

  setLocalization() {
    var lstKeys = ["ACCTHASBNLNKD","THANKUFORREGISTERING","CLICKHERETOLOGIN"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );   
  }

}
