import { Component, OnInit } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { AttributesForCustomForm } from '../../models/custom-forms/custom-attribute';
@Component({
    selector: "app-select",
    template: `
    <div class="col-md-6 mb-1">
    <label class="required">{{field.AttributeName}}</label>
    <select class="custom-select" formControlName="field.AttributeName">
    <option disabled hidden [value]="selectUndefinedOptionValue">
        <label name="PLEASESELECT"></label>
    </option>
    <option *ngFor="let option of field.options" value={{option.ID}}>
        {{option.Name}}
    </option>
   </select>
`,
    styles: []
})
export class SelectComponent implements OnInit {
    field: AttributesForCustomForm;
    group: FormGroup;
    constructor() { }

    ngOnInit() { }
}