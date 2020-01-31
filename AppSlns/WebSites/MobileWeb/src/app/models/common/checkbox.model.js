"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/* User defined models */
var dynamic_form_base_model_1 = require("./dynamic-form-base.model");
var ProSchedulerCheckbox = (function (_super) {
    __extends(ProSchedulerCheckbox, _super);
    function ProSchedulerCheckbox(options) {
        if (options === void 0) { options = {}; }
        var _this = _super.call(this, options) || this;
        _this.controlType = 'checkbox';
        _this.type = options['type'] || '';
        return _this;
    }
    return ProSchedulerCheckbox;
}(dynamic_form_base_model_1.DynamicFormBase));
exports.ProSchedulerCheckbox = ProSchedulerCheckbox;
//# sourceMappingURL=checkbox.model.js.map