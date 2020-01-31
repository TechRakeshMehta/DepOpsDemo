
/* User defined models */
import { DynamicFormBase } from './dynamic-form-base.model';

export class ProSchedulerDatePicker extends DynamicFormBase<string> {

    controlType = 'datepicker';
    type: string;

    constructor(options: {} = {}) {
        super(options);
        this.type = options['type'] || '';
    }
}