export class DynamicFormBase<T>{
    value: T;
    key: string;
    label: string;
    required: boolean;
    order: number;
    controlType: string;
    maxlength: number;
    minlength: number;
    length: number;
    pattern: string;
    validationMessage: string;
    placeholder: string;
    validationData: string;
    error: string;
    readOnly: boolean;

    constructor(options: {
        value?: T,
        key?: string,
        label?: string,
        required?: boolean,
        order?: number,
        controlType?: string,
        maxlength?: number,
        minlength?: number,
        length?: number,
        pattern?: string,
        validationMessage?: string,
        placeholder?: string,
        validationData?: string,
        error?: string,
        readOnly?: boolean
    } = {}) {
        this.value = options.value;
        this.key = options.key || '';
        this.label = options.label || '';
        this.required = !!options.required;
        this.order = options.order === undefined ? 1 : options.order;
        this.controlType = options.controlType || '';
        this.maxlength = options.maxlength || 0;
        this.minlength = options.minlength || 0;
        this.length = options.length || 0;
        this.pattern = options.pattern || '';
        this.validationMessage = options.validationMessage || '';
        this.placeholder = options.placeholder || '';
        this.validationData = options.validationData;
        this.error = options.error || '';
        this.readOnly = options.readOnly;
    }
}