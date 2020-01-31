import { Component, Input, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { ClientSettingCustomAttributeContract } from '../../../models/custom-forms/custom-attribute';

@Component({
    exportAs: "dynamicForm",
    selector: "dynamic-form",
    template: `
    <form class="dynamic-form" [formGroup]="form">
    <div class="row" >
    <div class="col-md-6 mb-1" *ngFor="let field of fields;">
    <ng-container  dynamicField [field]="field" [group]="form">
    </ng-container>
    </div>
    </div>
    </form>
    `,
    styles: []
})

export class DynamicFormComponent implements OnInit {
    @Input() fields: Array<ClientSettingCustomAttributeContract> = new Array<ClientSettingCustomAttributeContract>();

    form: FormGroup;

    get value() {
        return this.form.value;
    }

    getForm() {
        return this.form;
    }
    constructor(private fb: FormBuilder) { 
        console.log('constructer');
    }

    ngOnInit() {
       this.createControl(this.fields);
    }

    createControl(fields: Array<ClientSettingCustomAttributeContract>) {
        const group = this.fb.group({});
        fields.forEach(field => {
            field.AttributeName = field.SettingOverrideText == '' ? field.SettingName : field.SettingOverrideText;
            if (field.CustomAttributeDatatypeCode === "Button") return;
            const control = this.fb.control(
                field.AttributeDataValue,
                this.bindValidations(field)
            );
            group.addControl(field.AttributeName, control);
        });
        this.form = group;
    }

    bindValidations(validations: ClientSettingCustomAttributeContract) {
        const validList = [];
        if (validations.IsRequired != undefined && validations.IsRequired) {
            validList.push(Validators.required);
        }
        if (validations.ValidateExpression != undefined && validations.ValidateExpression != '') {
            validList.push(Validators.pattern(validations.ValidateExpression.replace('/', '//')));
        }
        if (validList.length > 0)
            return Validators.compose(validList);

        return null;
    }

    validateAllFormFields(formGroup: FormGroup) {
        Object.keys(formGroup.controls).forEach(field => {
            const control = formGroup.get(field);
            control.markAsTouched({ onlySelf: true });
        });
    }
}

