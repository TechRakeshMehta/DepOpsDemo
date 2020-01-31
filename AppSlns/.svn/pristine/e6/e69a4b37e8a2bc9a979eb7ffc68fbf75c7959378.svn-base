
/* User defined models */
import { DynamicFormBase } from './dynamic-form-base.model';

export class ProSchedulerDropdown extends DynamicFormBase<string> {
    controlType = 'dropdown';
    options: { id: string, uri: string,name:string }[] = [];

    constructor(options: {} = {}) {
        super(options);
        this.options = options['options'] || [];
    }
}