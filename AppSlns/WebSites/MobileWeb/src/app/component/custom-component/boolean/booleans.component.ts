import { Component, OnInit } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { ClientSettingCustomAttributeContract } from '../../../models/custom-forms/custom-attribute';
import { UtilityService } from "../../../services/shared-services/utility.service";

@Component({
    selector: "app-boolean",
    template: `    <style>
    .error-msg {
        color: #a94442;
    }

    .fix-error-icon {
        top: 27px;
    }

    .radio-button-label {
        margin-bottom: 0;
    }
     </style>


    <div  [formGroup]="group">
    <label [hidden]="!field.DisplayLabel" class="required">{{field.AttributeName}}</label>
     <div class="form-group">
             <div class="form-check form-check-inline">                                                                          
             <input [formControlName]="field.AttributeName"
                    [name] = "field.AttributeName" 
                    [ngClass]="displayFieldCss(field.AttributeName)"
             [placeholder]="field.AttributeName" [(ngModel)]="field.AttributeDataValue"
             type="radio" class="form-check-input" value="1">
             <label class="radio-button-label" for="field.AttributeName">Yes</label>
             </div>
             <div class="form-check form-check-inline">
             <input [formControlName]="field.AttributeName"
                    [name] = "field.AttributeName" 
                    [ngClass]="displayFieldCss(field.AttributeName)"
             [placeholder]="field.AttributeName" [(ngModel)]="field.AttributeDataValue"
             type="radio" class="form-check-input" value="0">
             <label class="radio-button-label" for="field.AttributeName">No</label>
             </div>
             <app-field-error-display [displayError]="isError(field.AttributeName)" errorMsg="{{errorMessage(field.AttributeName)}}">
             </app-field-error-display>
     </div>
</div>
`,
    styles: []
})

export class BooleanComponent implements OnInit {
    field: ClientSettingCustomAttributeContract;
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