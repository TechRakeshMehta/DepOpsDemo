import { Component, OnInit } from "@angular/core";
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Router } from "@angular/router";

@Component({
    selector: 'modal-content',
    templateUrl: 'confirmation-modal.component.html' 
  })

  export class ModalContentComponent implements OnInit {
    title: string;
    message:string = 'Are you sure ?';
    confirmBtnName: string = 'Yes';
    closeBtnName: string='No';    
    hideConfirmBtn:boolean=false;
    hideCloseBtn:boolean=false;  
    
    constructor(public bsModalRef: BsModalRef
        ,private modalService: BsModalService,
        private router: Router,) {
            this.modalService.config.backdrop = 'static';
            this.modalService.config.keyboard= false;

            this.modalService.onHide.subscribe((result: any) => {
                if (document.getElementById('Continue') != undefined && document.getElementById('Continue') != null) {
                    if (result === document.getElementById('Continue').textContent) {
                        this.router.navigate(["login"]);
                    }
                }
            });
        }
   
    ngOnInit() {      
        document.getElementById("Continue").style.display = 'none';
    }
    confirm(){
        this.modalService.setDismissReason(this.confirmBtnName);
        this.bsModalRef.hide();
    }
    decline(){
        this.modalService.setDismissReason(this.closeBtnName);
        this.bsModalRef.hide();
    }
  }