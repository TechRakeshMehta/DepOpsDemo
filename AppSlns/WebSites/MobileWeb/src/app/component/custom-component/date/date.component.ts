import { Component, OnInit } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { AttributesForCustomForm } from '../../../models/custom-forms/custom-attribute';
import { UtilityService } from "../../../services/shared-services/utility.service";

@Component({
    selector: "app-date",
    template: `    <style>
    .error-msg {
        color: #a94442;
    }

    .fix-error-icon {
        top: 27px;
    }
     </style>
     <div class="col-md-6 mb-1" [formGroup]="group">
     <label [hidden]="!field.DisplayLabel" class="required font-weight-bold">{{field.AttributeName}}</label>
     <input [formControlName]="field.AttributeName" [ngClass]="displayFieldCss(field.AttributeName)"
         [placeholder]="field.AttributeName" 
         [(ngModel)]="field.AttributeDataValue"
         type="date" class="form-control mb-1">
     <div *ngIf="field.IsRequired">
         <app-field-error-display [displayError]="isError(field.AttributeName)" errorMsg="{{errorMessage(field.AttributeName)}}">
         </app-field-error-display>
     </div>
 </div>`,
    styles: []
})

export class DateComponent implements OnInit {
    field: AttributesForCustomForm;
    group: FormGroup;

    constructor(private utilityService: UtilityService) { }
    ngOnInit() {

    }

    displayFieldCss(field: string) {
        return this.utilityService.displayFieldCss(this.group, field);
    }

    isError(fieldName: any): any {
        const control = this.group.get(fieldName);

        if (this.field.IsRequired != undefined && this.field.IsRequired) {
            var hasError = control.hasError('required');
            if (hasError != undefined && hasError && control.touched) {
                return true;
            }
        }
        if (this.field.ValidateExpression != undefined && this.field.ValidateExpression != '') {
            var hasError = control.hasError('pattern');
            if (hasError != undefined && hasError && control.touched) {
                return true;
            }
        }
        return false;
    }

    errorMessage(fieldName: any): any {

        const control = this.group.get(fieldName);
        if (this.field.IsRequired != undefined && this.field.IsRequired) {
            var hasError = control.hasError('required');
            if (hasError != undefined && hasError && control.touched) {
                return this.field.AttributeName + " is required.";
            }
        }
        if (this.field.ValidateExpression != undefined && this.field.ValidateExpression != '') {
            var hasError = control.hasError('pattern');
            if (hasError != undefined && hasError && control.touched) {
                return this.field.ValidationMessage;
            }
        }
        return "";
    }
}