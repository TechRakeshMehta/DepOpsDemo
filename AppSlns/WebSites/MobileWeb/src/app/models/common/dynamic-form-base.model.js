"use strict";
var DynamicFormBase = (function () {
    function DynamicFormBase(options) {
        if (options === void 0) { options = {}; }
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
    return DynamicFormBase;
}());
exports.DynamicFormBase = DynamicFormBase;
//# sourceMappingURL=dynamic-form-base.model.js.map