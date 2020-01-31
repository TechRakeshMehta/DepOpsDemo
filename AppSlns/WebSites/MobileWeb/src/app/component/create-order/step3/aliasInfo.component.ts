import { Component, OnInit, Input, EventEmitter, Output } from "@angular/core";
import { UserInfo, PersonAliasContract, Suffix } from "../../../models/user/user.model";
import { FormBuilder, FormGroup, FormControl } from "@angular/forms";
import { Validators } from "@angular/forms";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { basename } from "path";
import { LookupService } from "../../../services/shared-services/lookup.service";


@Component({
    selector: "aliasInfo-info",
    templateUrl: "aliasInfo.component.html",
    styleUrls: ['./aliasInfo.component.css'],
})
export class AliasInfoComponent implements OnInit {

    IsEditAddUpdateError: boolean;
    IsSuffixDropDown: boolean = false;
    suffixlist: Array<Suffix>;
    IsEditError: boolean;
    IsDuplicateName: boolean;
    IsInvalidName: boolean;
    OneEditErrorMsg: any;
    EditAddUpdateErrorMsg: any;
    DuplicateNameMsg: any;
    @Input() alias: PersonAliasContract;
    @Output() ShowHideAddMoreButton = new EventEmitter<boolean>();
    @Output() Next = new EventEmitter<boolean>();

    constructor(
        private orderFlowService: OrderFlowService,
        private formBuilder: FormBuilder,
        private utilityService: UtilityService,
        private languageTranslationService: LanguageTranslationService,
        private lookupService: LookupService,
    ) { }
    userInfo: UserInfo;
    AliasReadonly: boolean = true;
    firstNameRegex = "^(?=.{1,30}$)(([a-zA-Z])+( ?[a-zA-Z]+)|([a-zA-Z]{1}))$";
    middleNameRegex = "^(?=.{1,30}$)((([a-zA-Z])+( ?[a-zA-Z]+))|(-{5})|([a-zA-Z]{1}))$";
    lastNameregex = "^(?=.{1,30}$)(([a-zA-Z])+(-?[a-zA-Z]+)|([a-zA-Z]{1}))$";
    suffixregex = "^[a-zA-Z]*-?[a-zA-Z]*$";
    AliasForm = this.formBuilder.group({
        firstName: ["", [Validators.required, Validators.pattern(this.firstNameRegex)]],
        middleName: ["", Validators.pattern(this.middleNameRegex)],
        lastName: ["", [Validators.required, Validators.pattern(this.lastNameregex)]],
        suffix: ["", [Validators.pattern(this.suffixregex), Validators.maxLength(10)]],
        IdForIndex: [""],
        ID:[""]
        // here
    });
    hdnSaveAliasButton: boolean = true;
    hdnEditButton: boolean = false;
    hdnDeleteButton: boolean = false;
    hdnCancelButton: boolean = true;
    InvalidNameMsg: string = "";
    totalLength: number = 0;
    middleNameLength: number = 0;
    suffixLength: number = 0;
    DeleteButtonText: string = "";
    IdForIndex: number = 0;
    alias_validation_messages: any;
    validationMessages: KeyedCollection<string> = new KeyedCollection<string>();

    dataSource = new BehaviorSubject<KeyedCollection<string>>(
        new KeyedCollection<string>()
    );

    ValidationMsgsObservable = this.dataSource.asObservable();
    ngOnInit() {
        this.setLocalization();
        this.IsSuffixDropDown = this.orderFlowService.IsSuffixDropDown;       
        if(this.IsSuffixDropDown){
            const suffixControl =  this.AliasForm.get('suffix');
            suffixControl.clearValidators();
            this.lookupService.getSuffixDropDownList().then(s => {
                this.suffixlist = s;
                // this.textboxSuffixList=s;
                this.suffixlist=this.suffixlist.filter(a=>a.IsSystem);
                //this.setLocalization();
                this.populateFormControls();
              });
        }
        else{
            //this.setLocalization();
            this.populateFormControls();
        }
        //  this.setDeleteButtonText();
        //this.look
    }
    private populateFormControls() {
        this.userInfo = this.orderFlowService.getOrganizationUserInfoDetails();
        this.alias.IsSaveButtonHidden = this.hdnSaveAliasButton = this.alias.IsSaveButtonHidden == undefined ? true : this.alias.IsSaveButtonHidden;
        this.alias.IsEditbtnHidden = this.hdnEditButton = this.alias.IsEditbtnHidden == undefined ? false : this.alias.IsEditbtnHidden;
        this.alias.IsDeletebtnHidden = this.hdnDeleteButton = this.alias.IsDeletebtnHidden == undefined ? false : this.alias.IsDeletebtnHidden;
        this.alias.IsAliasReadonly = this.AliasReadonly = this.alias.IsAliasReadonly == undefined ? true : this.alias.IsAliasReadonly;
        this.alias.IsCancelbtnHidden = this.hdnCancelButton = this.alias.IsCancelbtnHidden == undefined ? true : this.alias.IsCancelbtnHidden;
        if(!this.alias.IdForIndex){
            this.alias.IdForIndex = this.alias.ID;
        }
        this.IdForIndex = this.alias.IdForIndex;
        var formControls = this.AliasForm.controls;
        formControls["firstName"].setValue(this.alias.FirstName);
        formControls["middleName"].setValue(this.alias.MiddleName);
        formControls["lastName"].setValue(this.alias.LastName);        
        formControls["IdForIndex"].setValue(this.alias.IdForIndex);
        formControls["ID"].setValue(this.alias.ID);

        // if (this.IsSuffixDropDown) {
        //     if(this.suffixlist.findIndex(x=>x.SuffixID==personalInfo.SelectedSuffixID)>=0)
        //     {
        //     this.selectedSuffixID =personalInfo.SelectedSuffixID;
        //     }
        //     else
        //     {
        //       this.selectedSuffixID=0;
        //     }
        //   }
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

    setLocalization() {
        //labels
        var lstKeys = ["ALIASFIRSTNAME", "ALIASMIDDLENAME", "ALIASLASTNAME",
            "SUFFIX", "SAVE", "EDIT", "DLT", "CNCL","CNFMCNLORDER","CNCLORDR","LSTITMY","LSTITMN","SELECTSUFFIX"];
        this.utilityService.SubscribeLocalListWithDupNames(
            this.languageTranslationService,
            lstKeys
        );
        //validaton msgs    
        var lstValidaionKeys = ["ALIASFIRSTNAMEREQ", "FIRSTNAMEVALDT", "ALIASLASTNAMEREQ", "LASTNAMEVALDT",
            "MIDDLENAMEVALDT", "SUFFIXNAMEVALDT", "TTLLENFULLNAMEATLEASTTHREECHARS", "DPLNAMECNTADD", "ONLYONEALIASADDITNVALDATN", 
            "ONLYONEALIASCANBEUPDATED","MAXCHARACTERS"];
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
        this.alias_validation_messages = {
            firstName: [
                { type: "required", message: validationMsgs["ALIASFIRSTNAMEREQ"] },
                {
                    type: "pattern", message: validationMsgs["FIRSTNAMEVALDT"]
                }
            ],
            lastName: [
                { type: "required", message: validationMsgs["ALIASLASTNAMEREQ"] },
                {
                    type: "pattern", message: validationMsgs["LASTNAMEVALDT"]

                }
            ],
            middleName: [
                {
                    type: "pattern", message: validationMsgs["MIDDLENAMEVALDT"]

                }
            ],
            suffix: [
                {
                    type: "pattern", message: validationMsgs["SUFFIXNAMEVALDT"]
                },
                { type: "maxlength", message: validationMsgs["SUFFIXNAMEVALDT"]}
            ]

        }
        this.InvalidNameMsg = validationMsgs["TTLLENFULLNAMEATLEASTTHREECHARS"];
        this.DuplicateNameMsg = validationMsgs["DPLNAMECNTADD"];
        this.EditAddUpdateErrorMsg = validationMsgs["ONLYONEALIASADDITNVALDATN"];
        this.OneEditErrorMsg = validationMsgs["ONLYONEALIASCANBEUPDATED"];


    }
    title = "Alias Info";
    form: any;


    isError(field: string) {
        return this.utilityService.isError(this.AliasForm, field, this.alias_validation_messages[field]);
    }
    errorMessage(field: string) {
        return this.utilityService.ErrorMessage(this.AliasForm, field, this.alias_validation_messages[field]);
    }

    onEdit() {
        this.IsEditError = false;
        if (this.userInfo.PersonAliasList.filter(x => x.IsAddMoreButtonClicked)[0] != undefined) {
            this.IsEditError = true;
            this.IsEditAddUpdateError = true;
        }
        else if (this.userInfo.PersonAliasList.filter(x => !x.IsAliasReadonly && !x.IsAddMoreButtonClicked)[0] != undefined) {
            this.IsEditError = true;
            this.IsEditAddUpdateError = false;
        }
        else {
            this.IsEditError = false;
            this.InvalidNameMsg = "";
            this.hdnSaveAliasButton = false;
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
        this.IsInvalidName = false;
        this.AliasReadonly = true;
        var index = this.userInfo.PersonAliasList.findIndex(x => x.IdForIndex == Id);
        var isAddMoreClicked = this.userInfo.PersonAliasList.find(x => x.IdForIndex == Id).IsAddMoreButtonClicked;
        if ((isAddMoreClicked != undefined && isAddMoreClicked == true) || (this.hdnDeleteButton == false && this.hdnCancelButton == true)) {
            if (index !== -1) {
                this.userInfo.PersonAliasList.splice(index, 1);
            }
        }
        else {
            this.userInfo.PersonAliasList.find(x => x.IdForIndex == Id).IsEditbtnHidden = false;
            this.userInfo.PersonAliasList.find(x => x.IdForIndex == Id).IsSaveButtonHidden = true;
            this.userInfo.PersonAliasList.find(x => x.IdForIndex == Id).IsCancelbtnHidden = true;
            this.userInfo.PersonAliasList.find(x => x.IdForIndex == Id).IsDeletebtnHidden = false;
            this.userInfo.PersonAliasList.find(x => x.IdForIndex == Id).IsAliasReadonly = true;
            this.AliasReadonly = true;
            this.hdnDeleteButton = false;
            this.hdnCancelButton = true;
            this.hdnEditButton = false;
            this.hdnSaveAliasButton = true;

            ///// For resetting last value of alias in case of Cancel click
            // var alaisFormObject = this.AliasForm.value;
            // alaisFormObject.firstName = this.alias.FirstName;
            // alaisFormObject.middleName = this.alias.MiddleName;
            // alaisFormObject.lastName = this.alias.LastName;
            // alaisFormObject.suffix = this.alias.Suffix;
            // alaisFormObject.IdForIndex =this.alias.IdForIndex;

            var formControls = this.AliasForm.controls;
            formControls["firstName"].setValue(this.alias.FirstName);
            formControls["middleName"].setValue(this.alias.MiddleName);
            formControls["lastName"].setValue(this.alias.LastName);
            formControls["suffix"].setValue(this.alias.Suffix);
            formControls["IdForIndex"].setValue(this.alias.IdForIndex);
        }
        this.orderFlowService.setOrganizationUserInfoDetails(this.userInfo);
        this.ShowHideAddMoreButton.emit(false);
        if (this.userInfo.PersonAliasList.filter(x => x.IsAddMoreButtonClicked)[0] != undefined)
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
                    var alaisFormObject = this.AliasForm.value;
                    this.alias.FirstName = alaisFormObject.firstName;
                    this.alias.MiddleName = alaisFormObject.middleName;
                    this.alias.LastName = alaisFormObject.lastName;
                    this.alias.Suffix = alaisFormObject.suffix;
                    this.alias.IdForIndex = alaisFormObject.IdForIndex;
                    this.alias.IsAliasReadonly = true;
                    this.AliasReadonly = true;
                    this.alias.IsDeletebtnHidden = false;
                    this.alias.IsCancelbtnHidden = true;
                    this.alias.IsEditbtnHidden = false;
                    this.alias.IsSaveButtonHidden = true;
                    this.alias.IsAddMoreButtonClicked = false;
                    this.hdnDeleteButton = false;
                    this.hdnCancelButton = true;
                    this.hdnEditButton = false;
                    this.hdnSaveAliasButton = true;
                    this.orderFlowService.setOrganizationUserInfoDetails(this.userInfo);
                    this.Next.emit(false);
                    this.ShowHideAddMoreButton.emit(false);
                    // this.setDeleteButtonText();
                }
            }
        }
        else {
            this.utilityService.validateAllFormFields(this.AliasForm);
            this.Next.emit(true);
        }
    }

    validateName() {
        debugger;
        this.IsInvalidName = false;
        var aliasFormObject = this.AliasForm.value;
        if (aliasFormObject.firstName != undefined && aliasFormObject.lastName != undefined)
            if (aliasFormObject.firstName != "" || aliasFormObject.lastName != "" || aliasFormObject.middleName != "" || aliasFormObject.suffix != "") {
                if (aliasFormObject.middleName != null && aliasFormObject.middleName != "" && aliasFormObject.middleName != undefined) {
                    this.middleNameLength = aliasFormObject.middleName.length;
                }else{
                    this.middleNameLength = 0;
                }
                if (aliasFormObject.suffix != null && aliasFormObject.suffix != "" && aliasFormObject.suffix != undefined) {
                    this.suffixLength = aliasFormObject.suffix.length;
                }
                else{
                    this.suffixLength = 0;
                }
                this.totalLength = (aliasFormObject.firstName.length) + (aliasFormObject.lastName.length) + this.middleNameLength + 
                this.suffixLength;
                if (this.totalLength < 3) {

                    this.IsInvalidName = true;
                    this.setLocalization();
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
        var userData = this.orderFlowService.getOrganizationUserInfoDetails();
        var lstAlias = userData.PersonAliasList;
        var personalInfo = userData;
        var alaisFormObject = this.AliasForm.value;
        var res = lstAlias.filter(x =>
            x.FirstName.toLowerCase() === alaisFormObject.firstName.toLowerCase()
            && x.LastName.toLowerCase() === alaisFormObject.lastName.toLowerCase()
            && this.IsNull(x.MiddleName).toLowerCase() === alaisFormObject.middleName.toLowerCase()
            && this.IsNull(x.Suffix).toLowerCase() === this.IsNull(alaisFormObject.suffix).toLowerCase() 
            && x.IdForIndex != alaisFormObject.IdForIndex
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
    //     if (this.hdnSaveAliasButton) {
    //         this.DeleteButtonText = "Delete";
    //     }
    //     else {
    //         this.DeleteButtonText = "Cancel";
    //     }
    // }

    IsNull(message: string) {
        if (message == null || message == undefined)
            return '';
        else
            return message;
    }

}
