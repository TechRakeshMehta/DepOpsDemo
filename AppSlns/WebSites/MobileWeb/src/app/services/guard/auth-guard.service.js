"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var Observable_1 = require("rxjs/Observable");
require("rxjs/add/observable/of");
require("rxjs/add/operator/map");
var user_1 = require("./user");
var USERS = [
    new user_1.User('prashant', 'password', 'ADMIN')
];
var usersObservable = Observable_1.Observable.of(USERS);
var AuthService = (function () {
    function AuthService() {
        this.redirectUrl = '/';
        this.loginUrl = '/login';
        this.isloggedIn = false;
    }
    AuthService.prototype.getAllUsers = function () {
        return usersObservable;
    };
    AuthService.prototype.isUserAuthenticated = function (username, password) {
        var _this = this;
        return this.getAllUsers()
            .map(function (users) {
            var user = users.find(function (user) { return (user.username === username) && (user.password === password); });
            if (user) {
                _this.isloggedIn = true;
                _this.loggedInUser = user;
            }
            else {
                _this.isloggedIn = false;
            }
            return _this.isloggedIn;
        });
    };
    AuthService.prototype.isUserLoggedIn = function () {
        return true;
    };
    AuthService.prototype.getRedirectUrl = function () {
        return this.redirectUrl;
    };
    AuthService.prototype.setRedirectUrl = function (url) {
        this.redirectUrl = url;
    };
    AuthService.prototype.getLoginUrl = function () {
        return this.loginUrl;
    };
    AuthService.prototype.getLoggedInUser = function () {
        return this.loggedInUser;
    };
    AuthService.prototype.logoutUser = function () {
        this.isloggedIn = false;
    };
    return AuthService;
}());
AuthService = __decorate([
    core_1.Injectable()
], AuthService);
exports.AuthService = AuthService;
//# sourceMappingURL=auth-guard.service.js.map