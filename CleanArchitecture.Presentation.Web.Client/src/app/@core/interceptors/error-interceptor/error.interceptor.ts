import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HTTP_INTERCEPTORS,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, map, Observable, tap, throwError } from 'rxjs';
import { AuthenticationService } from '../../services/authentication-service/authentication.service';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(private authenticationService: AuthenticationService,
        private router: Router) { }

    intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

        return next.handle(request)
                .pipe(
                    map(res => {

                        // return result
                        return res;
                    }),
                    catchError(error => {

                        console.log(JSON.stringify(error));


                        //if (error.status === 500) {

                        //    // print message
                        //    console.log('backend error');

                        //    // regirect to error page
                        //    this.router.navigate(['./auth/error']);

                        //} else
                        if (error.status === 401) {

                            // print message
                            console.log('token has expired, the user must login again');

                            // regirect to login page
                            this.authenticationService.logout();
                            this.router.navigate(['./auth/login']);

                        } else if (error.status === 403) {

                            // print message
                            console.log('user lacks priviledges to access this resource');

                            // regirect to login page
                            this.router.navigate(['./auth/access']);

                        }

                        let errorResponse: HttpErrorResponse = error.error;
                        console.log(JSON.stringify(errorResponse));

                        return throwError(() => errorResponse);
                    })
               );
    }
}

export const ErrorInterceptorProvider = { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true };
