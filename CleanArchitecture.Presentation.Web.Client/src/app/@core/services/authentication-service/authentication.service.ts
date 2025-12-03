import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject } from 'rxjs';
import { AuthenticationConstants } from './constants/authentication-constants/authentication-constants';
import { IAuthenticateResponseDto } from './interfaces/i-authenticate-response-dto';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

    // a refernce to jwt instance
    private jwt: JwtHelperService = new JwtHelperService();

    // a reference to whether a user is logged in or not
    private _loginStatus: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.checkLoginStatus());
    private _userId: BehaviorSubject<string> = new BehaviorSubject<string>(AuthenticationConstants.userId());
    private _userName: BehaviorSubject<string> = new BehaviorSubject<string>(AuthenticationConstants.userName());
    private _userRole: BehaviorSubject<string> = new BehaviorSubject<string>(AuthenticationConstants.userRole());


    constructor() { }


    get isAuthenticated(): boolean {

        // if local storage says I'm still logged in, then
        // I'm logged out if token has expired
        // else I'm not authenticated
        if (this.isStillLoggedIn) {
            return this.isTokenStillValid;
        } else {
            return false;
        }
    }

    get isLoggedIn() {
        return this._loginStatus.asObservable();
    }

    get isStillLoggedIn() {

        // get the login status from storage
        const loginStatus = localStorage.getItem(AuthenticationConstants.loginStatus());

        // return true if login status is equal to 1 else false
        return loginStatus === '1';
    }

    get isTokenStillValid() {
        // get the token from disk
        // return the result
        const token = localStorage.getItem(AuthenticationConstants.jwt());

        // return result
        return this.jwt.isTokenExpired(token) === false;
    }

    get getCurrUserId() {
        return this._userId.asObservable();
    }

    get currentUserName() {
        return this._userName.asObservable();
    }

    get currentUserRole() {
        return this._userRole.asObservable();
    }

    get userId() {
        return localStorage.getItem(AuthenticationConstants.userId());
    }

    get userName() {
        return localStorage.getItem(AuthenticationConstants.userName());
    }

    get token() {
        return localStorage.getItem(AuthenticationConstants.jwt());
    }


    checkLoginStatus(): boolean {
        return false;
    }

    //public hasClaim(claim: string): boolean {

    //    // get the token
    //    const token = this.token;
    //    const decodedToken = this.jwt.decodeToken(token);

    //    // return true if we have this claim
    //    return decodedToken.Claims.find(c => c === claim) !== undefined;
    //}

    public authenticate(authenticationData: IAuthenticateResponseDto): void {
        
        // store user details and jwt token in local storage to keep user logged in between
        // page refreshes
        this._loginStatus.next(true);
        localStorage.setItem(AuthenticationConstants.loginStatus(), '1');
        localStorage.setItem(AuthenticationConstants.jwt(), authenticationData.accessToken);
        localStorage.setItem(AuthenticationConstants.userId(), authenticationData.userId);
        localStorage.setItem(AuthenticationConstants.userName(), authenticationData.userName);
        localStorage.setItem(AuthenticationConstants.expiration(), authenticationData.expiresIn);
        localStorage.setItem(AuthenticationConstants.userRole(), authenticationData.userRole);
                    
    }

    public setUserName(userName: string): void {
        localStorage.setItem(AuthenticationConstants.userName(), userName);
    }

    public logout(): void {

        // clear all the login details
        this._loginStatus.next(false);
        localStorage.removeItem(AuthenticationConstants.loginStatus());
        localStorage.removeItem(AuthenticationConstants.jwt());
        localStorage.removeItem(AuthenticationConstants.userId());
        localStorage.removeItem(AuthenticationConstants.userName());
        localStorage.removeItem(AuthenticationConstants.expiration());
        localStorage.removeItem(AuthenticationConstants.userRole());
    }
}
