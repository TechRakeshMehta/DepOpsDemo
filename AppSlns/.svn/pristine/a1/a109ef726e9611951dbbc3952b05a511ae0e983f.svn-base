import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { RecaptchaComponent } from 'ng-recaptcha';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { OrderFlowService } from '../services/order-flow/order-flow.service';


@Component({
  selector: 'app-recap',
  templateUrl: './recap.component.html',
  styleUrls: ['./recap.component.css']
})
export class RecapComponent implements OnInit {
  Locations: any;
  lstImages: any;
  reactiveForm: FormGroup;


  @ViewChild('captchaRef') captchaRef:RecaptchaComponent;
  modalRef: BsModalRef;

  constructor(private modalService: BsModalService,private orderFlowService:OrderFlowService) { }

  ngOnInit() {
    this.reactiveForm = new FormGroup({
      recaptchaReactive: new FormControl(null, Validators.required)
    });
    // this.orderFlowService.getLocationImages(113).subscribe(s=>
    //   {
    
    // this.Locations=s;
    // console.log(this.Locations);
    //   });
    // grecaptcha.execute();
  }
  resolved(captchaResponse: string) {
   alert("dfgdfg");
}
submit(captchaResponse: string)
{

}

onCapcha()
{
  this.captchaRef.execute();
}

openModal(template: TemplateRef<any>) {

  this.orderFlowService.getLocationImages(113).subscribe(s=>
  {

    this.Locations=s;
console.log(this.Locations);
this.modalRef = this.modalService.show(template);
  });
  
}


}
