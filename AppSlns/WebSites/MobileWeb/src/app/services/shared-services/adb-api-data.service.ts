import { Injectable } from "@angular/core";
import { Observable, Observer } from "rxjs";
import "rxjs/add/observable/of";
import "rxjs/add/operator/map";

/*User defined service*/
import { HttpService } from "../shared-services/http.service";
import { ContentType } from "../../models/common/content-type.model";
import { environment } from "../../../environments/environment";
import { PackageDetail } from "../../models/applicant/package-contract.model";
import { UserInfo, UserContract, LinkAccountContract, VerificationAccountContract } from "../../models/user/user.model";
import { ClientSettingCustomAttributeContract } from '../../models/custom-forms/custom-attribute';

@Injectable()
export class AdbApiService {
  ChangeUserPasswordEmail: any;
  setChangeUserPasswordEmail(email: any): any {
    this.ChangeUserPasswordEmail = email;
  }
  constructor(
    public httpService: HttpService,
    public contentType: ContentType
  ) { }
  public PackageDetail: PackageDetail;
  public UsrVerCode: string;
  public forgotPasswordType: number;
  public forgotPasswordTypeMsg: string;

  getUserInfo(): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(environment.baseApiUrl + environment.GetAuthorizedData)
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }
  getTenantName(): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(environment.baseApiUrl + environment.GetTenantName)
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }
  getCbiUrlInfo(): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(environment.baseApiUrl + environment.GetCbiUrlInfo)
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }
  getApplicantPackages(): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(environment.baseApiUrl + environment.GetApplicantPackageList)
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  getApplicantPackageCategoryDetails(pkgId: number): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.GetApplicantPackageCategoryDetails +
          "/" +
          pkgId
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }
  postUploadDocument(fileData: any, description: any): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .postUpload(
          environment.baseApiUrl + environment.ApplicantUploadDocument,
          fileData,
          this.contentType.multipartRequest,
          description
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }
  private getRawResult(apiResponse: any): any[] {
    return apiResponse;
  }





  registerUser(userInfo: UserContract): Observable<any> {
    //var userInfoJson=JSON.stringify(userInfo);
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .postSansAuth(
          environment.baseApiUrl + environment.RegisterUser,
          userInfo,
          'application/json'
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  accountToLinkSelected(linkAccountContract: LinkAccountContract, doNormalRegistration: boolean): Observable<any> {
    //var userInfoJson=JSON.stringify(userInfo);
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .postSansAuth(
          environment.baseApiUrl + environment.AccountToLinkSelected + '/' + doNormalRegistration,
          linkAccountContract,
          'application/json'
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  LinkAccount(linkAccountContract: LinkAccountContract): Observable<any> {
    //var userInfoJson=JSON.stringify(userInfo);
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .postSansAuth(
          environment.baseApiUrl + environment.LinkAccount,
          linkAccountContract,
          'application/json'
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  registerUser1(userInfo: UserContract) {
    //var userInfoString=JSON.stringify(userInfo);
    this.httpService.httpPostExample(environment.baseApiUrl + environment.RegisterUser, userInfo);
  }

  IsUserAlreadyExists(Username: string): Observable<any> {
    //var userInfoJson=JSON.stringify(userInfo);
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl + environment.UserExists + "/" + Username,
      )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }


  GenerateVerificationCode(email: string, selectedType: number): Observable<any> {
    //var userInfoJson=JSON.stringify(userInfo);
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl + "/" + environment.GenerateVerificationCode + "/" + btoa(email) + "/" + selectedType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  UpdatePassword(email: string, password: String): Observable<any> {
    //var userInfoJson=JSON.stringify(userInfo);
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl + "/" + environment.UpdatePassword + "/" + btoa(email) + "/" + password
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }
  ValidateVerificationCode(email: string, code: string, selectedType: number): Observable<any> {
    //var userInfoJson=JSON.stringify(userInfo);
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl + "/" + environment.ValidateVerificationCode + "/" + btoa(email) + "/" + code + "/" + selectedType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }



  ValidateUserViaEmailAndRedirect(verificationCode: string): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl + environment.ValidateUserViaEmailAndRedirect + "/" + verificationCode
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  GetAccountVerficationQuestions(verificationCode: string): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl + environment.GetAccountVerficationQuestions + "/" + verificationCode
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  GetClientSettingCustomAttribute(): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(environment.baseApiUrl + environment.GetClientSettingCustomAttribute)
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  GetAccountVerificationSettings(verificationCode: string): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl + environment.GetAccountVerificationSettings + "/" + verificationCode
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  ValidateandActivateUser(verificationAccountData:VerificationAccountContract, verificationCode: string): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .post(
          environment.baseApiUrl + environment.ValidateandActivateUser + '/' + verificationCode,
          verificationAccountData,
          this.contentType.jsonType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  ChangePassword(email: string, password: String,oldpassword:string): Observable<any> {
    //var userInfoJson=JSON.stringify(userInfo);
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl + "/" + environment.ChangePassword + "/" + btoa(email) + "/" + password + "/" + oldpassword
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  IsPasswordExpired(): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(environment.baseApiUrl + environment.IsPasswordExpired)
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }
}


