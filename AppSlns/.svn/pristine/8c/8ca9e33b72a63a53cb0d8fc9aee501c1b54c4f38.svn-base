import { Injectable } from '@angular/core';
import { Observable, Observer } from 'rxjs';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/map';

import { User } from './user';

/*User defined service*/
import { HttpService } from "../shared-services/http.service";
import { ContentType } from '../../models/common/content-type.model'
import { environment } from '../../../environments/environment'
const USERS = [
    new User('prashant', 'password', 'ADMIN')
];
let usersObservable = Observable.of(USERS);

@Injectable()
export class AuthService {
    setLoggedIn(IsloggedIn: boolean): any {
        this.isloggedIn=IsloggedIn;
    }
    private redirectUrl: string = '/';
    private loginUrl: string = '/';
    private isloggedIn: boolean = false;
    private loggedInUser: User;
    constructor(public httpService: HttpService,public contentType: ContentType) {

    }
    getAllUsers(): Observable<User[]> {
        return usersObservable;
    }
    // isUserAuthenticated(username: string, password: string): Observable<boolean> {
    //     return this.getAllUsers()
    //         .map(users => {
    //             let user = users.find(user => (user.username === username) && (user.password === password));
    //             if (user) {
    //                 this.isloggedIn = true;
    //                 this.loggedInUser = user;
    //             } else {
    //                 this.isloggedIn = false;
    //             }
    //             return this.isloggedIn;
    //         });
    // }
    postUserLoginInfo(loginData: any): Observable<any> {
        var data = "grant_type=password&username=" + loginData.value.userName + "&password=" + loginData.value.userPassword;  
        return Observable.create((observer: Observer<any>) => {
            this.httpService.post(environment.baseApiUrl+ environment.AuthenticateUrl, data ,this.contentType.urlEncoded)
                .map(this.getRawResult)
                .subscribe((rNArray: any) => {
                    if (rNArray != null && rNArray != undefined) {
                        observer.next(rNArray);
                        observer.complete();
                    }
                }, (err: any) => {
                    console.log("call failed");
                    observer.next(err);
                    observer.complete();
                });
        });
    }

    private getRawResult(apiResponse: any): any[] {
        return apiResponse;
    }
    isUserLoggedIn(): boolean {
        return this.isloggedIn;
    }
    getRedirectUrl(): string {
        return this.redirectUrl;
    }
    setRedirectUrl(url: string): void {
        this.redirectUrl = url;
    }
    getLoginUrl(): string {
        return this.loginUrl;
    }
    getLoggedInUser(): User {
        return this.loggedInUser;
    }
    logoutUser(): void {
        this.isloggedIn = false;
    }
}