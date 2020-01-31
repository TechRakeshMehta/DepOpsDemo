import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Rx";
import {
  HttpClient,
  HttpRequest,
  HttpResponse,
  HttpParams,
  HttpHeaders,
  HttpInterceptor,
  HttpEvent,
  HttpHandler
} from "@angular/common/http";

/*User Defined service*/
import { DataSharingService } from "./data-sharing.service";
import { LoaderService } from "./loader.service";
import { ContentType } from "../../models/common/content-type.model";
import { UserContract } from "../../models/user/user.model";
import { CommonService } from "./common.service";
import { tap } from "rxjs/operators";
import { Router } from "@angular/router";

@Injectable()
export class HttpService implements HttpInterceptor {
  public runningServicesCount: number = 0;
  constructor(
    private http: HttpClient,
    private dataSharingService: DataSharingService,
    private loaderService: LoaderService,
    private commonService: CommonService,
    private router: Router
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.commonService.ResetTimer(true);
    return next.handle(req)
    // .
    // do(event => {
    //   if (event instanceof HttpResponse) {
    //     if (event.headers.has("auth_token")) {
    //       var value = event.headers.get("auth_token");
    //       sessionStorage.setItem("auth_token", value);
    //     }
    //     if (event.headers.has("refresh_token")) {
    //       var value = event.headers.get("refresh_token");
    //       sessionStorage.setItem("refresh_token", value);
    //     }

    //   }
    // })
    .catch((error, caught) => {

      if (error.status === 403 || error.status === 401) {
        this.router.navigate([""]);
        //return Observable.throw(error);
      }
      return Observable.throw(error);
    }) as any;


  }
  get(
    url: string,
    properties?: Array<string>,
    orderBy?: string,
    skip?: number,
    take?: number
  ): Observable<any> {
    this.runningServicesCount++;

    if (this.runningServicesCount == 1) {
      this.showLoader();
    }

    //  let queryParams: HttpParams = new HttpParams();
    //  if (properties) {
    //      queryParams.set('properties', properties.join(','));
    //  }
    //  if (orderBy) {
    //      queryParams.set('orderBy', orderBy);
    //  }
    //  if (skip) {
    //      queryParams.set('skip', skip.toString());
    //  }
    //  if (take) {
    //      queryParams.set('take', take.toString());
    //  }
    //  if(this.dataSharingService.loginDataObj != null || this.dataSharingService.loginDataObj != undefined){
    //     access_token = this.dataSharingService.loginDataObj.access_token
    //     refresh_token = this.dataSharingService.loginDataObj.refresh_token
    //     queryParams.set('Authorization', access_token);
    //     queryParams.set('refresh_token', refresh_token);
    // }

    var access_token: string = "";
    var refresh_token: string = "";

    if (sessionStorage.getItem("auth_token") != null) {
      access_token = "Bearer " + sessionStorage.getItem("auth_token");
      refresh_token = sessionStorage.getItem("refresh_token");
    }

    var config = {
      headers: {
        Authorization: access_token,
        refresh_token: refresh_token
      }
    };
    // let options: HttpRequest<any> = new HttpRequest<any>('GET',null, queryParams);

    return this.http
      .get(url, config)
      .map(this.extractData)
      .catch(this.handleError)
      .finally(() => {
        this.runningServicesCount--;

        if (this.runningServicesCount == 0) {
          this.hideLoader();
        }
      });
  }

  post(
    url: string,
    data: any,
    contentType: string,
    description?: any
  ): Observable<any> {
    var access_token: string = "";
    var refresh_token: string = "";
    this.runningServicesCount++;

    if (this.runningServicesCount == 1) {
      this.showLoader();
    }

    if (sessionStorage.getItem("auth_token") != null) {
      access_token = sessionStorage.getItem("auth_token");
      refresh_token = sessionStorage.getItem("refresh_token");
    }
    var config = {
      headers: {
        "Content-Type": contentType,
        Authorization: "Bearer " + access_token,
        DocumentDescription: "",
        refresh_token: refresh_token
      }
    };
    return this.http
      .post(url, data, config)
      .map(this.extractData)
      .catch(this.handleError)
      .finally(() => {
        this.runningServicesCount--;

        if (this.runningServicesCount == 0) {
          this.hideLoader();
        }
      });
  }

  postUpload(
    url: string,
    data: any,
    contentType: string,
    description?: any
  ): Observable<any> {
    var access_token: string = "";
    var refresh_token: string = "";
    this.runningServicesCount++;

    if (this.runningServicesCount == 1) {
      this.showLoader();
    }

    if (sessionStorage.getItem("auth_token") != null) {
      access_token = sessionStorage.getItem("auth_token");
      refresh_token = sessionStorage.getItem("refresh_token");
    }

    var config = {
      headers: {
        Authorization: "Bearer " + access_token,
        DocumentDescription: description,
        refresh_token: refresh_token
      }
    };

    return this.http
      .post(url, data, config)
      .map(this.extractData)
      .catch(this.handleError)
      .finally(() => {
        this.runningServicesCount--;

        if (this.runningServicesCount == 0) {
          this.hideLoader();
        }
      });
  }

  put(url: string, data: any, contentType: string): Observable<any> {
    this.runningServicesCount++;

    if (this.runningServicesCount == 1) {
      this.showLoader();
    }

    return this.http
      .put(url, data, {
        headers: new HttpHeaders().set("Content-Type", contentType)
      })
      .map(this.extractData)
      .catch(this.handleError)
      .finally(() => {
        this.runningServicesCount--;

        if (this.runningServicesCount == 0) {
          this.hideLoader();
        }
      });
  }

  delete(url: string): Observable<any> {
    this.runningServicesCount++;

    if (this.runningServicesCount == 1) {
      this.showLoader();
    }

    return this.http
      .delete(url, {
        headers: new HttpHeaders().set("Content-Type", "text/plain")
      })
      .map(this.extractData)
      .catch(this.handleError)
      .finally(() => {
        this.runningServicesCount--;

        if (this.runningServicesCount == 0) {
          this.hideLoader();
        }
      });
  }
  httpPostExample(url: string, body: UserContract) {

    this.http.post(url,
      {
        body
      })
      .subscribe(
        (val) => {
          console.log("POST call successful value returned in body",
            val);
        },
        response => {
          console.log("POST call in error", response);
        },
        () => {
          console.log("The POST observable is now completed.");
        });
  }
  postSansAuth(
    url: string,
    data: any,
    contentType: string
  ): Observable<any> {
    this.runningServicesCount++;

    if (this.runningServicesCount == 1) {
      this.showLoader();
    }
    var config = {
      headers: {
        "Content-Type": contentType
      }
    };

    return this.http
      .post(url, data, config)
      .map(this.extractData)
      .catch(this.handleError)
      .finally(() => {
        this.runningServicesCount--;

        if (this.runningServicesCount == 0) {
          this.hideLoader();
        }
      });
  }

  private extractData(res: HttpResponse<any>) {
    let body = res;
    return body || {};
  }

  private handleError(error: any) {
    let errMsg = error.message
      ? error.message
      : error.status
        ? `${error.status} - ${error.statusText}`
        : "ERROR";
    console.error(error, errMsg);
    return Observable.throw(error);
  }

  private showLoader(): void {
    this.loaderService.show();
  }

  private hideLoader(): void {
    this.loaderService.hide();
  }
}
