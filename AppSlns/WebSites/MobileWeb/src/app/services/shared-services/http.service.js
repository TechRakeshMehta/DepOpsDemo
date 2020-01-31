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
var Rx_1 = require("rxjs/Rx");
var http_1 = require("@angular/common/http");
var HttpService = (function () {
    function HttpService(http) {
        this.http = http;
        this.runningServicesCount = 0;
    }
    HttpService.prototype.get = function (url, properties, orderBy, skip, take) {
        var queryParams = new http_1.HttpParams();
        if (properties) {
            queryParams.set('properties', properties.join(','));
        }
        if (orderBy) {
            queryParams.set('orderBy', orderBy);
        }
        if (skip) {
            queryParams.set('skip', skip.toString());
        }
        if (take) {
            queryParams.set('take', take.toString());
        }
        var options = new http_1.HttpRequest('GET', null, queryParams);
        return this.http.get(url, { params: queryParams })
            .map(this.extractData)
            .catch(this.handleError).finally(function () {
        });
    };
    HttpService.prototype.post = function (url, data) {
        return this.http
            .post(url, data, {
            headers: new http_1.HttpHeaders().set('Content-Type', 'application/json'),
        }).map(this.extractData).catch(this.handleError).finally(function () {
        });
    };
    HttpService.prototype.put = function (url, data) {
        return this.http
            .put(url, data, {
            headers: new http_1.HttpHeaders().set('Content-Type', 'application/json'),
        }).map(this.extractData).catch(this.handleError).finally(function () {
        });
    };
    HttpService.prototype.delete = function (url) {
        return this.http.delete(url, {
            headers: new http_1.HttpHeaders().set('Content-Type', 'text/plain'),
        }).map(this.extractData).catch(this.handleError).finally(function () {
        });
    };
    HttpService.prototype.extractData = function (res) {
        var body = res;
        return body || {};
    };
    HttpService.prototype.handleError = function (error) {
        var errMsg = (error.message) ? error.message :
            error.status ? error.status + " - " + error.statusText : 'ERROR';
        console.error(error, errMsg);
        return Rx_1.Observable.throw(errMsg);
    };
    return HttpService;
}());
HttpService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.HttpClient])
], HttpService);
exports.HttpService = HttpService;
//# sourceMappingURL=http.service.js.map