"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var app_routing_1 = require("./app.routing");
var forms_1 = require("@angular/forms"); // <-- #1 import module
var forms_2 = require("@angular/forms");
/*custome component import*/
var app_component_1 = require("./app.component");
var login_component_1 = require("./component/login/login.component");
var welcome_component_1 = require("./component/welcome/welcome.component");
var form_field_component_1 = require("./component/form-field/form-field.component");
var activation_guard_1 = require("./services/guard/activation-guard");
var auth_guard_service_1 = require("./services/guard/auth-guard.service");
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        imports: [platform_browser_1.BrowserModule, app_routing_1.ROUTING, forms_1.ReactiveFormsModule, forms_2.FormsModule],
        declarations: [app_component_1.AppComponent, login_component_1.LoginComponent, form_field_component_1.FormFieldComponent, welcome_component_1.WelcomeComponent],
        bootstrap: [app_component_1.AppComponent],
        providers: [
            auth_guard_service_1.AuthService,
            activation_guard_1.AuthGuardService
        ]
    })
], AppModule);
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map