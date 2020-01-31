"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var auth_guard_service_1 = require("./auth-guard.service");
var AuthGuardService = (function () {
    function AuthGuardService(authService, router) {
        this.authService = authService;
        this.router = router;
    }
    AuthGuardService.prototype.canActivate = function (route, state) {
        var url = state.url;
        if (this.authService.isUserLoggedIn()) {
            return true;
        }
        this.authService.setRedirectUrl(url);
        this.router.navigate([this.authService.getLoginUrl()]);
        return false;
    };
    AuthGuardService.prototype.canActivateChild = function (route, state) {
        var loggedInUser = this.authService.getLoggedInUser();
        if (loggedInUser.role === 'ADMIN') {
            return true;
        }
        else {
            //console.log('Unauthorized to open link: ' + state.url);
            return false;
        }
    };
    return AuthGuardService;
}());
AuthGuardService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [auth_guard_service_1.AuthService, router_1.Router])
], AuthGuardService);
exports.AuthGuardService = AuthGuardService;
//# sourceMappingURL=activation-guard.js.map