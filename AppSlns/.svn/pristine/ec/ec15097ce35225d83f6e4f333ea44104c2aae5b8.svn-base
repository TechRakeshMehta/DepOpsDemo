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
/* System defined core library */
var core_1 = require("@angular/core");
var rxjs_1 = require("rxjs");
require("rxjs/add/operator/mergeMap");
/*User defined service*/
var http_service_1 = require("./http.service");
var DataSharingService = (function () {
    function DataSharingService(httpService) {
        this.httpService = httpService;
    }
    DataSharingService.prototype.postUserLoginInfo = function (loginData) {
        var _this = this;
        return rxjs_1.Observable.create(function (observer) {
            _this.httpService.post('api/v1/', loginData)
                .map(_this.getRawResult)
                .subscribe(function (rNArray) {
                if (rNArray != null && rNArray != undefined && rNArray.length > 0) {
                    observer.next(rNArray);
                    observer.complete();
                }
            }, function (err) {
                console.log("call failed");
                observer.next(-1);
                observer.complete();
            });
        });
    };
    DataSharingService.prototype.getRawResult = function (apiResponse) {
        return apiResponse;
    };
    return DataSharingService;
}());
DataSharingService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_service_1.HttpService])
], DataSharingService);
exports.DataSharingService = DataSharingService;
//# sourceMappingURL=data-sharing.service.js.map