import { Component, OnInit, OnDestroy } from "@angular/core";
import { ModalContentComponent } from "../../confirmation-box/confirmation-modal.component";
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { Router } from "@angular/router";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { UtilityService } from "../../../services/shared-services/utility.service";

@Component({
    selector: 'cancel-order-button',
    templateUrl: 'cancel-order.component.html'
})

export class CancelOrderButtonComponent implements OnInit, OnDestroy {
    constructor(private modalService: BsModalService,
        private orderFlowService: OrderFlowService,
        private router: Router,
        private languageTranslationService: LanguageTranslationService,
        private utilityService: UtilityService) {

        this.modalService.onHide.subscribe((result: any) => {
            if (document.getElementById('LSTITMY') != undefined && document.getElementById('LSTITMY') != null) {
                if (result === document.getElementById('LSTITMY').textContent) {
                    this.orderFlowService.setOrderInfoToEmpty();
                    this.router.navigate(["userDashboard"]);
                }
            }
        });
    }

    ngOnInit(): void {
        // var lstKeys = ["CNFMCNLORDER","CNCLORDR","LSTITMY","LSTITMN","CNCL"];
        // this.utilityService.SubscribeLocalList(
        //   this.languageTranslationService,
        //   lstKeys
        // );
        document.getElementById("CNFMCNLORDER").style.display = 'none';
        document.getElementById("CNCLORDR").style.display = 'none';
        document.getElementById("LSTITMY").style.display = 'none';
        document.getElementById("LSTITMN").style.display = 'none';
    }
    ngOnDestroy() {
        //this.modalService.onHide.unsubscribe();
    }
    cancelClick() {
        const initialState = {

            message: document.getElementById('CNFMCNLORDER').textContent,
            title: document.getElementById('CNCLORDR').textContent,
            confirmBtnName: document.getElementById('LSTITMY').textContent,
            closeBtnName: document.getElementById('LSTITMN').textContent,
        };
        this.modalService.show(ModalContentComponent, { initialState });
    }
}