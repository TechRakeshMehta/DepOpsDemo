import { Component, OnInit } from "@angular/core";
import { ModalContentComponent } from "../../confirmation-box/confirmation-modal.component";
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Router } from "@angular/router";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { RegisterUserFormService } from "../../../services/register-user/register-user-form.service";

@Component({
    selector: 'cancel-registration-button',
    templateUrl: 'cancel-registration.component.html'
})

export class CancelRegistrationButtonComponent implements OnInit {

    constructor(private modalService: BsModalService,
        private router: Router,
        private languageTranslationService: LanguageTranslationService,
        private utilityService: UtilityService,
        private registerUserFormService: RegisterUserFormService) {
        this.modalService.onHide.subscribe((result: any) => {
            if (document.getElementById('LSTITMY') != undefined && document.getElementById('LSTITMY') != null) {
                if (result === document.getElementById('LSTITMY').textContent) {
                    this.registerUserFormService.resetFormData();
                    this.router.navigate([""]);
                }
            }
        });
    }

    ngOnInit(): void {
        document.getElementById("CNFMCNLREGISTRATION").style.display = 'none';
        document.getElementById("CNCLREGISTRATION").style.display = 'none';
        document.getElementById("LSTITMY").style.display = 'none';
        document.getElementById("LSTITMN").style.display = 'none';
    }

    cancelClick() {
        const initialState = {
            message: document.getElementById('CNFMCNLREGISTRATION').textContent,
            title: document.getElementById('CNCLREGISTRATION').textContent,
            confirmBtnName: document.getElementById('LSTITMY').textContent,
            closeBtnName: document.getElementById('LSTITMN').textContent,
        };
        this.modalService.show(ModalContentComponent, { initialState });
    }
}