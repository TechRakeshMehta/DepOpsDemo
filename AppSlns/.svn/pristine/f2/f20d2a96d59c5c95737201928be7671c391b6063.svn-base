import { Component, OnInit, OnDestroy } from "@angular/core";
import { ModalContentComponent } from "../../confirmation-box/confirmation-modal.component";
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { Router } from "@angular/router";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { UtilityService } from "../../../services/shared-services/utility.service";

@Component({
    selector: 'cancel-rescheduling-button',
    templateUrl: 'cancel-rescheduling.component.html'
})

export class CancelReschedulingButtonComponent implements OnInit, OnDestroy {
    constructor(private modalService: BsModalService,
        private orderFlowService: OrderFlowService,
        private router: Router,
        private languageTranslationService: LanguageTranslationService,
        private utilityService: UtilityService) {

        this.modalService.onHide.subscribe((result: any) => {
            if (document.getElementById('LSTITMY') != undefined && document.getElementById('LSTITMY') != null) {
                if (result === document.getElementById('LSTITMY').textContent) {
                    this.orderFlowService.setAlreadyPlacedOrderInfoToEmpty();
                    this.router.navigate(["OrderHistory"]);
                }
            }
        });
    }

    ngOnInit(): void {
        // var lstKeys = ["CNFMCNLRESCHEDULING","CNCLRESCHEDULING","LSTITMY","LSTITMN","CNCL"];
        // this.utilityService.SubscribeLocalList(
        //   this.languageTranslationService,
        //   lstKeys
        // );
        document.getElementById("CNFMCNLRESCHEDULING").style.display = 'none';
        document.getElementById("CNCLRESCHEDULING").style.display = 'none';
        document.getElementById("LSTITMY").style.display = 'none';
        document.getElementById("LSTITMN").style.display = 'none';
    }
    ngOnDestroy() {
        //this.modalService.onHide.unsubscribe();
    }
    cancelClick() {
        const initialState = {

            message: document.getElementById('CNFMCNLRESCHEDULING').textContent,
            title: document.getElementById('CNCLRESCHEDULING').textContent,
            confirmBtnName: document.getElementById('LSTITMY').textContent,
            closeBtnName: document.getElementById('LSTITMN').textContent,
        };
        this.modalService.show(ModalContentComponent, { initialState });
    }
}