
/* System defined core library */
import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

/* User defined models */
import { DynamicFormBase } from '../../models/common/dynamic-form-base.model';

@Injectable()
export class DynamicControlService {
    constructor() { }

    toFormGroup(inputsData: DynamicFormBase<any>[]) {
        let group: any = {};

        inputsData.forEach(input => {

            // Validation Required :
            group[input.key] = input.required ? new FormControl(input.value || '', Validators.required)
                : new FormControl(input.value || '');

        });
        return new FormGroup(group);
    }
}