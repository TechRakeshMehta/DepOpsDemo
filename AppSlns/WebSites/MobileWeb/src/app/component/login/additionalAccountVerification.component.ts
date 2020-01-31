import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from "@angular/forms";
import { Validators } from "@angular/forms";
import { LookupInfo, VerificationAccountContract, UserContract } from "../../models/user/user.model";
import { Router } from "@angular/router";
import { ClientSettingCustomAttributeContract, AttributesForCustomForm } from '../../models/custom-forms/custom-attribute';
import { UtilityService } from "../../services/shared-services/utility.service";
import { AdbApiService } from '../../services/shared-services/adb-api-data.service';
import { BsModalService } from "ngx-bootstrap/modal";
import { ModalContentComponent } from "../confirmation-box/confirmation-modal.component";
import { DynamicFormComponent } from "../custom-component/dynamic-form/dynamic-form.directive";

@Component({
    selector: 'app-additionVerification',
    templateUrl: './additionalAccountVerification.component.html',
})
export class AdditionAccountVerification implements OnInit {
    @ViewChild(DynamicFormComponent) dynamicForm: DynamicFormComponent;
    ssnRegex: string = "\\d{3}\\-\\d{2}-\\d{4}";
    lastSSNregex: string = "\\d{4}";
    mask = [/[1-9]/, /\d/, /\d/, '-', /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
    LastSSNmask = [/[1-9]/, /\d/, /\d/, /\d/];
    account_validation_messages: any;
    UsrVerCode: string;
    AccountVerficationSettings: Array<any>;
    lstQuestions: Array<LookupInfo> = new Array<LookupInfo>();
    lstProfileCustomAttributes: Array<ClientSettingCustomAttributeContract> = new Array<ClientSettingCustomAttributeContract>();
    lstcustomField: Array<ClientSettingCustomAttributeContract> = new Array<ClientSettingCustomAttributeContract>();
    errMessage: string = '';
    // Settings 
    AccountVerificationMainSetting: boolean;
    AccVerificationProcessResponseReqdSetting: boolean;
    AccVerificationProcessDOBSetting: boolean;
    AccVerificationProcessSSNSetting: boolean;
    AccVerificationProcessLSSNSetting: boolean;
    AccVerificationProcessProfCustAttrSetting: boolean;
    AccVerificationProcessDOBTextSetting: string;
    AccVerificationProcessSSNTextSetting: string;
    AccVerificationProcessLSSNTextSetting: string;
    AccVerificationProcessProfCustAttrTextSetting: string;
    hideDOBAny: boolean = true;
    hideSSNAny: boolean = true;
    hideLastSSNAny: boolean = true;
    dvSectionWithAnyPrmsn: boolean = false;
    dvSectionWithAllPrmsn: boolean = false;
    IsShowLabel: boolean = false;
    userInfo: UserContract;
    SelectedCode: string = "";

    accountVerificationForm = this.formBuilder.group({
        Questions: ["", [Validators.required]],
        DOBAny: [""],
        SSNAny: ["", [Validators.pattern(this.ssnRegex)]],
        LastSSNAny: ["", [Validators.pattern(this.lastSSNregex)]],
        DOBALL: ["", [Validators.required]],
        SSN: ["", [Validators.required, Validators.pattern(this.ssnRegex)]],
        LSSN: ["", [Validators.required, Validators.pattern(this.lastSSNregex)]],
    });
    constructor(private utilityService: UtilityService,
        private formBuilder: FormBuilder,
        private adbApiService: AdbApiService,
        private router: Router,
        private modalService: BsModalService
    ) {

    }

    ngOnInit() {
        this.UsrVerCode = this.adbApiService.UsrVerCode;
        if (this.UsrVerCode != undefined && this.UsrVerCode != '') {
            this.AccountVerficationSettings = new Array<any>();
            this.adbApiService.GetAccountVerificationSettings(this.UsrVerCode).subscribe(data => {
                this.AccountVerficationSettings = data;
                this.AccountVerificationMainSetting = (this.AccountVerficationSettings.find(x => x.Key == "AccountVerificationMainSetting").Value == 'True');
                this.AccVerificationProcessResponseReqdSetting = (this.AccountVerficationSettings.find(x => x.Key == "AccVerificationProcessResponseReqdSetting").Value == 'True');
                this.AccVerificationProcessDOBSetting = (this.AccountVerficationSettings.find(x => x.Key == "AccVerificationProcessDOBSetting").Value == 'True');
                this.AccVerificationProcessSSNSetting = (this.AccountVerficationSettings.find(x => x.Key == "AccVerificationProcessSSNSetting").Value == 'True');
                this.AccVerificationProcessLSSNSetting = (this.AccountVerficationSettings.find(x => x.Key == "AccVerificationProcessLSSNSetting").Value == 'True');
                this.AccVerificationProcessProfCustAttrSetting = (this.AccountVerficationSettings.find(x => x.Key == "AccVerificationProcessProfCustAttrSetting").Value == 'True');
                this.AccVerificationProcessDOBTextSetting = this.AccountVerficationSettings.find(x => x.Key == "AccVerificationProcessDOBTextSetting").Value;
                this.AccVerificationProcessSSNTextSetting = this.AccountVerficationSettings.find(x => x.Key == "AccVerificationProcessSSNTextSetting").Value;
                this.AccVerificationProcessLSSNTextSetting = this.AccountVerficationSettings.find(x => x.Key == "AccVerificationProcessLSSNTextSetting").Value;
                this.AccVerificationProcessProfCustAttrTextSetting = this.AccountVerficationSettings.find(x => x.Key == "AccVerificationProcessProfCustAttrTextSetting").Value;
                if (this.AccVerificationProcessProfCustAttrSetting) {
                    this.adbApiService.GetClientSettingCustomAttribute().subscribe(custattribute => {
                        this.lstProfileCustomAttributes = custattribute;
                        this.lstProfileCustomAttributes.forEach(element => {
                            element.AttributeName = element.SettingName;
                            element.AttributeDataValue = "";
                            element.DisplayLabel = true;
                        });
                        if (this.AccVerificationProcessResponseReqdSetting) {
                            this.lstcustomField = this.lstProfileCustomAttributes.filter(x => x.SettingValue == true);
                        }
                    });
                }

                if (this.AccountVerificationMainSetting) {
                    var formControls = this.accountVerificationForm.controls;

                    if (this.AccVerificationProcessResponseReqdSetting) {
                        //All Permission
                        this.dvSectionWithAllPrmsn = true;
                        this.lstcustomField = this.lstProfileCustomAttributes;
                        formControls["Questions"].clearValidators();
                        formControls["DOBAny"].clearValidators();
                        formControls["SSNAny"].clearValidators();
                        formControls["LastSSNAny"].clearValidators();
                        formControls["LastSSNAny"].disable();
                        formControls["SSNAny"].disable();
                        formControls["DOBAny"].disable();
                        formControls["Questions"].disable();
                        this.BindAllPermissionSection();
                    }
                    else {
                        //Any Permission
                        this.dvSectionWithAnyPrmsn = true;
                        formControls["DOBALL"].clearValidators();
                        formControls["SSN"].clearValidators();
                        formControls["LSSN"].clearValidators();
                        formControls["LSSN"].disable();
                        formControls["SSN"].disable();
                        formControls["DOBALL"].disable();
                        this.adbApiService.GetAccountVerficationQuestions(this.UsrVerCode).subscribe(data => {
                            this.lstQuestions = data;
                        });
                    }
                }
                else {
                    this.router.navigate(["login"]);
                }
            });

            this.account_validation_messages = {
                Questions: [{ type: "required", message: "Question is required." }],
                LastSSNAny: [{ type: "required", message: "Last four SSN is required." },
                { type: "pattern", message: "Last four SSN is required." }
                ],
                DOBAny: [{ type: "required", message: "Date of Birth is required." }],
                SSNAny: [
                    { type: 'required', message: "Social Security Number is required." },
                    { type: 'pattern', message: "Full Social Security Number is required." }
                ],
                LSSN: [{ type: "required", message: "Last four SSN is required." },
                { type: "pattern", message: "Last four SSN is required." }
                ],
                DOBALL: [{ type: "required", message: "Date of Birth is required." }],
                SSN: [
                    { type: 'required', message: "Social Security Number is required." },
                    { type: 'pattern', message: "Full Social Security Number is required." }
                ]

            }
        }
        else {
            this.router.navigate(["login"]);
        }
    }

    isError(field: string) {

        return this.utilityService.isError(this.accountVerificationForm, field, this.account_validation_messages[field]);
    }
    errorMessage(field: string) {

        return this.utilityService.ErrorMessage(this.accountVerificationForm, field, this.account_validation_messages[field]);
    }

    displayFieldCss(field: string) {
        return this.utilityService.displayFieldCss(this.accountVerificationForm, field);
    }

    isFieldValid(field: string) {
        return this.utilityService.isFieldValid(this.accountVerificationForm, field);
    }

    BindAllPermissionSection() {
        var formControls = this.accountVerificationForm.controls;
        if (this.AccVerificationProcessDOBSetting == true) {
            this.AccVerificationProcessDOBTextSetting = this.AccVerificationProcessDOBTextSetting == '' ? 'Date of Birth' : this.AccVerificationProcessDOBTextSetting;
        }
        else {
            formControls["DOBALL"].clearValidators();
            formControls["DOBALL"].disable();
        }
        if (this.AccVerificationProcessSSNSetting == true) {
            this.AccVerificationProcessSSNTextSetting = this.AccVerificationProcessSSNTextSetting == '' ? 'SSN' : this.AccVerificationProcessSSNTextSetting;
        }
        else {
            formControls["SSN"].clearValidators();
            formControls["SSN"].disable();
        }
        if (this.AccVerificationProcessLSSNSetting == true) {
            this.AccVerificationProcessLSSNTextSetting = this.AccVerificationProcessLSSNTextSetting == '' ? 'Last four SSN' : this.AccVerificationProcessLSSNTextSetting;
        }
        else {
            formControls["LSSN"].clearValidators();
            formControls["LSSN"].disable();
        }
    }

    onQuestionSelected(selectedQueCode: string) {
        var formControls = this.accountVerificationForm.controls;
        this.lstcustomField = new Array<ClientSettingCustomAttributeContract>();
        if (selectedQueCode == "AVPSSNPMSN") {
            formControls["SSNAny"].setValidators([Validators.required, Validators.pattern(this.ssnRegex)]);
            formControls["SSNAny"].enable();
            formControls["DOBAny"].clearValidators();
            formControls["LastSSNAny"].clearValidators();
            formControls["LastSSNAny"].disable();
            formControls["DOBAny"].disable();
            this.hideSSNAny = false;
            this.hideDOBAny = true;
            this.hideLastSSNAny = true;
            this.IsShowLabel = true;
            this.SelectedCode = selectedQueCode
        }
        if (selectedQueCode == "AVPDOBPMSN") {
            formControls["DOBAny"].setValidators([Validators.required]);
            formControls["DOBAny"].enable();
            formControls["SSNAny"].clearValidators();
            formControls["SSNAny"].disable();
            formControls["LastSSNAny"].clearValidators();
            formControls["LastSSNAny"].disable();
            this.hideSSNAny = true;
            this.hideDOBAny = false;
            this.hideLastSSNAny = true;
            this.IsShowLabel = true;
            this.SelectedCode = selectedQueCode
        }
        if (selectedQueCode == "AVPLSSNPSN") {
            formControls["LastSSNAny"].setValidators([Validators.required, Validators.pattern(this.lastSSNregex)]);
            formControls["LastSSNAny"].enable();
            formControls["DOBAny"].clearValidators();
            formControls["SSNAny"].clearValidators();
            formControls["SSNAny"].disable();
            formControls["DOBAny"].disable();
            this.hideSSNAny = true;
            this.hideDOBAny = true;
            this.hideLastSSNAny = false;
            this.IsShowLabel = true;
            this.SelectedCode = selectedQueCode
        }

        if (!isNaN(parseInt(selectedQueCode))) {
            formControls["SSNAny"].clearValidators();
            formControls["SSNAny"].disable();
            formControls["LastSSNAny"].clearValidators();
            formControls["LastSSNAny"].disable();
            formControls["DOBAny"].clearValidators();
            formControls["DOBAny"].disable();
            this.hideSSNAny = true;
            this.hideDOBAny = true;
            this.hideLastSSNAny = true;
            this.IsShowLabel = true;
            this.SelectedCode = selectedQueCode
            var selectedCustomAttrID: number;
            selectedCustomAttrID = parseInt(selectedQueCode);
            this.lstcustomField = this.lstProfileCustomAttributes.filter(x => x.CustomAttributeID == selectedCustomAttrID);
            this.lstcustomField.filter(x => x.DisplayLabel = false);
            if (this.dynamicForm != undefined) {
                this.dynamicForm.createControl(this.lstcustomField);
            }
        }
    }

    SubmitClick() {
        if (this.accountVerificationForm.valid && (this.dynamicForm == undefined || this.dynamicForm.getForm().valid)) {
            this.userInfo = new UserContract();
            if (this.AccVerificationProcessResponseReqdSetting) {
                this.userInfo.SSN = this.accountVerificationForm.get("SSN").value;
                this.userInfo.DOB = this.accountVerificationForm.get("DOBALL").value;
                this.userInfo.SSNL4 = this.accountVerificationForm.get("LSSN").value;
                this.SelectedCode = "";
            }
            else {
                this.userInfo.VerificationPermissionCode = this.SelectedCode;
                if (this.SelectedCode == "AVPSSNPMSN") {
                    this.userInfo.SSN = this.accountVerificationForm.get("SSNAny").value;
                }
                if (this.SelectedCode == "AVPDOBPMSN") {
                    this.userInfo.DOB = this.accountVerificationForm.get("DOBAny").value;
                }
                if (this.SelectedCode == "AVPLSSNPSN") {
                    this.userInfo.SSNL4 = this.accountVerificationForm.get("LastSSNAny").value;
                }
            }
            var lstcustomAttributes: Array<AttributesForCustomForm> = new Array<AttributesForCustomForm>();

            this.lstcustomField.forEach(element => {
                var customAttribute: AttributesForCustomForm = new AttributesForCustomForm();
                customAttribute.AttributeId = element.CustomAttributeID;
                customAttribute.AttributeDataValue = element.AttributeDataValue;
                lstcustomAttributes.push(customAttribute);
            });
            var verificationData: VerificationAccountContract = new VerificationAccountContract();
            verificationData.User = this.userInfo;
            verificationData.lstCustomAttributes = lstcustomAttributes;
            this.adbApiService.ValidateandActivateUser(verificationData, this.UsrVerCode).subscribe(values => {

                if (!values.HasError) {
                    var message = "Your account has been activated. Please login to continue.";
                    const initialState = {
                        message: message,
                        title: '',
                        confirmBtnName: '',
                        closeBtnName: 'Continue',
                        hideConfirmBtn: true,
                        hideCloseBtn: false
                    };
                    this.modalService.show(ModalContentComponent, { initialState });
                    // this.router.navigate(["login/:IsUserActivated=1"]);
                }
                else {
                    this.errMessage = values.ErrorMessage;
                }
            });
        }

        else {
            this.utilityService.validateAllFormFields(this.accountVerificationForm);
            this.dynamicForm.validateAllFormFields(this.dynamicForm.getForm());
        }
    }

    CancelClick() {
        this.router.navigate(["/login"]);
    }
}