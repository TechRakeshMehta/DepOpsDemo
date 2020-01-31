import { Component, OnInit, Input, EventEmitter, Output } from "@angular/core";
import { PersonAliasContract, Suffix } from "../../models/user/user.model";
import { FormBuilder, FormGroup, FormControl } from "@angular/forms";
import { Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { RegisterUserFormService } from "../../services/register-user/register-user-form.service";
import { UtilityService } from "../../services/shared-services/utility.service";
import { LanguageTranslationService } from "../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { LookupService } from "../../services/shared-services/lookup.service";

@Component({
    selector: "alias-info",
    templateUrl: "alias.component.html",
    styleUrls: ['./alias.component.css'],
})
export class AliasComponent implements OnInit {

    EditAddUpdateErrorMsg: any;
    IsEditAddUpdateError: boolean = false;
    IsEditError: boolean;
    IsSuffixDropDown: boolean = false;
    selectedSuffixTxt: string = "0";
    suffixlist: Array<Suffix>;
    OneEditErrorMsg: string;
    DuplicateNameMsg: any;
    IsDuplicateName: boolean;
    IsInvalidName: boolean;
    @Input() alias: PersonAliasContract;
    @Output() ShowHideAddMoreButton = new EventEmitter<boolean>();
    @Output() Next = new EventEmitter<boolean>();
    alias_validation_messages: any;
    constructor(
        private registerUserFormService: RegisterUserFormService,
        private formBuilder: FormBuilder,
        private router: Router,
        private utilityService: UtilityService,
        private languageTranslationService: LanguageTranslationService,
        private lookupService: LookupService,
    ) { }

    validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
    dataSource = new BehaviorSubject<KeyedCollection<string>>(
        new KeyedCollection<string>()
    );
    ValidationMsgsObservable = this.dataSource.asObservable();
    setLocalization() {

        var lstKeys = ["FIRSTNAME", "MIDDLENAME", "LASTNAME", "SUFFIX", "SAVE", "EDIT", "DLT", "CNCL", "SELECTSUFFIX"];

        this.utilityService.SubscribeLocalListWithDupNames(
            this.languageTranslationService,
            lstKeys
        );

        var lstValidationKeys = [
            "ALIASFIRSTNAMEREQ", "ALIASLASTNAMEREQ", "FIRSTNAMEVALDT",
            "ALIASFIRSTMIDNAMEREGEX", "ALIASLASTNAMEREGEX", "SUFFIXNAMEVALDT",
            "CNFMCNLREGISTRATION", "CNCLREGISTRATION", "LSTITMY", "LSTITMN", "CNCL",
            "TTLLENFULLNAMEATLEASTTHREECHARS", "DPLNAMECNTADD", "ONLYONEALIASADDITNVALDATN",
            "ONLYONEALIASCANBEUPDATED", "MAXCHARACTERS"
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

    firstNameRegex = "^(?=.{1,30}$)(([a-zA-Z])+( ?[a-zA-Z]+)|([a-zA-Z]{1}))$";
    middleNameRegex = "^(?=.{1,30}$)((([a-zA-Z])+( ?[a-zA-Z]+))|(-{5})|([a-zA-Z]{1}))$";
    lastNameregex = "^(?=.{1,30}$)(([a-zA-Z])+(-?[a-zA-Z]+)|([a-zA-Z]{1}))$";
    suffixregex = "^[a-zA-Z]*-?[a-zA-Z]*$";
    AliasForm = this.formBuilder.group({
        firstName: ["", [Validators.required, Validators.pattern(this.firstNameRegex)]],
        middleName: ["", Validators.pattern(this.middleNameRegex)],
        lastName: ["", [Validators.required, Validators.pattern(this.lastNameregex)]],
        suffix: [""],
        IdForIndex: [""]
    });

    lstAlias: Array<PersonAliasContract> = [];
    hdnSaveButton: boolean = false;
    hdnEditButton: boolean = true;
    hdnDeleteButton: boolean = false;
    hdnCancelButton: boolean = true;
    InvalidNameMsg: string = "";
    totalLength: number = 0;
    middleNameLength: number = 0;
    suffixLength: number = 0;
    IdForIndex: number = 0;
    AliasReadonly: boolean = false;
    DeleteButtonText: string = "";

    ngOnInit() {
        this.setLocalization();
        this.lstAlias = this.registerUserFormService.getAliasInfo();
        if (!this.alias.IsSaveButtonHidden) {
            this.Next.emit(true);
        }
        this.IsSuffixDropDown = this.registerUserFormService.IsSuffixDropDown;

        if(this.IsSuffixDropDown){
            const suffixControl =  this.AliasForm.get('suffix');
            suffixControl.clearValidators();
            this.lookupService.getSuffixDropDownList().then(s => {
                this.suffixlist = s;
                this.suffixlist=this.suffixlist.filter(a=>a.IsSystem);
                this.setLocalization();
                this.populateFormControls();
              });
        }
        else{
            this.populateFormControls();
        }
       
    }

    private populateFormControls() {
        this.hdnSaveButton = this.alias.IsSaveButtonHidden;
        this.hdnEditButton = this.alias.IsEditbtnHidden;
        this.hdnDeleteButton = this.alias.IsDeletebtnHidden;
        this.hdnCancelButton = this.alias.IsCancelbtnHidden;
        this.AliasReadonly = this.alias.IsAliasReadonly;
        var formControls = this.AliasForm.controls;

        formControls["IdForIndex"].setValue(this.alias.IdForIndex);
        this.IdForIndex = this.alias.IdForIndex;

        formControls["firstName"].setValue(this.alias.FirstName);
        formControls["middleName"].setValue(this.alias.MiddleName);
        formControls["lastName"].setValue(this.alias.LastName);

        if(!this.IsSuffixDropDown){
            formControls["suffix"].setValue(this.alias.Suffix);
        }else{
            if(this.suffixlist.findIndex(x=>x.Suffix == this.alias.Suffix) >= 0){
                formControls["suffix"].setValue(this.alias.Suffix);
            }
            else{
                formControls["suffix"].setValue('');
            }            
        }

      
        
    }
    initializeValidationMessages(validationMsgs: KeyedCollection<string>) {
        this.alias_validation_messages = {
            'firstName': [
                { type: 'required', message: validationMsgs["ALIASFIRSTNAMEREQ"] },
                { type: 'pattern', message: validationMsgs["FIRSTNAMEVALDT"] }
            ],
            'lastName': [
                { type: 'required', message: validationMsgs["ALIASLASTNAMEREQ"] },
                { type: 'pattern', message: validationMsgs["ALIASLASTNAMEREGEX"] }
            ],
            'middleName': [
                { type: 'pattern', message: validationMsgs["ALIASFIRSTMIDNAMEREGEX"] }
            ],
            'suffix': [
                { type: 'pattern', message: validationMsgs["SUFFIXNAMEVALDT"] },
                { type: "maxlength", message: validationMsgs["SUFFIXNAMEVALDT"] }
            ]

        }
        this.InvalidNameMsg = validationMsgs["TTLLENFULLNAMEATLEASTTHREECHARS"];
        this.DuplicateNameMsg = validationMsgs["DPLNAMECNTADD"];
        this.EditAddUpdateErrorMsg = validationMsgs["ONLYONEALIASADDITNVALDATN"];
        this.OneEditErrorMsg = validationMsgs["ONLYONEALIASCANBEUPDATED"];
    }
    title = "Alias Info";
    aliasInfo: PersonAliasContract;
    form: any;

    isError(field: string) {
        return this.utilityService.isError(this.AliasForm, field, this.alias_validation_messages[field]);
    }
    errorMessage(field: string) {
        return this.utilityService.ErrorMessage(this.AliasForm, field, this.alias_validation_messages[field]);
    }

    onEdit() {
        this.IsEditError = false;
        if (this.lstAlias.filter(x => x.IsAddMoreButtonClicked)[0] != undefined) {
            this.IsEditError = true;
            this.IsEditAddUpdateError = true;
        }
        else if (this.lstAlias.filter(x => !x.IsAliasReadonly && !x.IsAddMoreButtonClicked)[0] != undefined) {
            this.IsEditError = true;
            this.IsEditAddUpdateError = false;
        }
        else {
            this.IsEditError = false;
            this.OneEditErrorMsg = "";
            this.hdnSaveButton = false;
            this.hdnEditButton = true;
            this.hdnDeleteButton = true;
            this.hdnCancelButton = false;
            this.alias.IsAliasReadonly = this.AliasReadonly = false;
            this.Next.emit(true);
            this.ShowHideAddMoreButton.emit(true);
            //this.setDeleteButtonText();
        }
    }
    onDelete(Id: number) {
        this.IsInvalidName=false;
        if ((this.hdnDeleteButton == false && this.hdnCancelButton == true) || this.alias.IsAddMoreButtonClicked) {
            if (Id !== -1) {
                this.registerUserFormService.deleteAliasInfo(Id);
            }
        }
        else {
            var formControls = this.AliasForm.controls;
            formControls["firstName"].setValue(this.alias.FirstName);
            formControls["middleName"].setValue(this.alias.MiddleName);
            formControls["lastName"].setValue(this.alias.LastName);
            formControls["suffix"].setValue(this.alias.Suffix);
            formControls["IdForIndex"].setValue(this.alias.IdForIndex);
            this.alias.IsAliasReadonly = this.AliasReadonly = true;
            this.hdnDeleteButton = false;
            this.hdnCancelButton = true;
            this.hdnEditButton = false;
            this.hdnSaveButton = true;
            //this.IdForIndex = this.alias.IdForIndex;
        }
        this.ShowHideAddMoreButton.emit(false);
        if (this.lstAlias.filter(x => x.IsAddMoreButtonClicked)[0] != undefined)
            this.Next.emit(true);
        else
            this.Next.emit(false);
    }

    isFieldValid(field: string) {
        return this.utilityService.isFieldValid(this.AliasForm, field);
    }

    displayFieldCss(field: string) {
        return this.utilityService.displayFieldCss(this.AliasForm, field);
    }
    onSubmit() {
        if (this.AliasForm.valid) {
            var isValidName = this.validateName();
            if (isValidName) {
                var isNotAliasDuplicate = this.validateDuplicateAlias();
                if (isNotAliasDuplicate) {
                    this.aliasInfo = new PersonAliasContract();
                    var alaisFormObject = this.AliasForm.value;
                    this.aliasInfo.FirstName = alaisFormObject.firstName;
                    this.aliasInfo.MiddleName = alaisFormObject.middleName;
                    this.aliasInfo.LastName = alaisFormObject.lastName;
                    this.aliasInfo.Suffix = alaisFormObject.suffix;
                    this.aliasInfo.IdForIndex = alaisFormObject.IdForIndex;
                    this.aliasInfo.IsAliasReadonly = true;
                    this.aliasInfo.IsDeletebtnHidden = false;
                    this.aliasInfo.IsCancelbtnHidden = true;
                    this.aliasInfo.IsEditbtnHidden = false;
                    this.aliasInfo.IsSaveButtonHidden = true;
                    this.aliasInfo.IsAddMoreButtonClicked = false;
                    this.registerUserFormService.setAliasInfo(this.aliasInfo);
                    this.Next.emit(false);
                    this.ShowHideAddMoreButton.emit(false);
                }
            }
        }
        else {
            this.utilityService.validateAllFormFields(this.AliasForm);
            this.Next.emit(true);
        }

    }
    validateName() {
        var aliasFormObject = this.AliasForm.value;
        this.IsInvalidName = false;
        if (aliasFormObject.firstName != "" || aliasFormObject.lastName != "" || aliasFormObject.middleName != "" || aliasFormObject.suffix != "") {
            if (aliasFormObject.middleName != null && aliasFormObject.middleName != "") {
                this.middleNameLength = aliasFormObject.middleName.length;
            }
            if (aliasFormObject.suffix != null && aliasFormObject.suffix != "") {
                this.suffixLength = aliasFormObject.suffix.length;
            }
            this.totalLength = (aliasFormObject.firstName.length) + (aliasFormObject.lastName.length) + this.middleNameLength 
            + this.suffixLength;
            if (this.totalLength < 3) {

                this.IsInvalidName = true;
                return false;
            }
            else {
                this.IsInvalidName = false;
                return true;
            }
        }
    }
    validateDuplicateAlias() {
        this.IsDuplicateName = false;
        var lstAlias = this.registerUserFormService.getAliasInfo();
        var personalInfo = this.registerUserFormService.getPersonalInfo();
        var alaisFormObject = this.AliasForm.value;
        var res = lstAlias.filter(x =>
            x.FirstName.toLowerCase() === alaisFormObject.firstName.toLowerCase()
            && x.LastName.toLowerCase() === alaisFormObject.lastName.toLowerCase()
            && this.IsNull(x.MiddleName).toLowerCase() === alaisFormObject.middleName.toLowerCase()
            && this.IsNull(x.Suffix).toLowerCase() === this.IsNull(alaisFormObject.suffix).toLowerCase() && x.IdForIndex != alaisFormObject.IdForIndex
        );
        if (personalInfo.FirstName.toLowerCase() === alaisFormObject.firstName.toLowerCase()
            && personalInfo.LastName.toLowerCase() === alaisFormObject.lastName.toLowerCase()
            && this.IsNull(personalInfo.MiddleName).toLowerCase() === alaisFormObject.middleName.toLowerCase()
            && this.IsNull(personalInfo.Suffix).toLowerCase() === alaisFormObject.suffix.toLowerCase()
        ) {
            this.IsDuplicateName = true;
            return false;
        }
        if (res.length > 0) {
            this.IsDuplicateName = true;
            return false;
        }
        else {
            this.IsDuplicateName = false;
            return true;
        }
    }
    // setDeleteButtonText() {
    //     if (this.hdnSaveButton) {
    //         //this.DeleteButtonText = "Delete";
    //         document.getElementById("BTNDLT").style.display="block"
    //         document.getElementById("BTNCNL").style.display="none"
    //     }
    //     else {
    //         //this.DeleteButtonText = "Cancel";
    //         document.getElementById("BTNDLT").style.display="none"
    //         document.getElementById("BTNCNL").style.display="block"
    //     }
    // }

    IsNull(message: string) {
        if (message == null || message == undefined)
            return '';
        else
            return message;
    }
}