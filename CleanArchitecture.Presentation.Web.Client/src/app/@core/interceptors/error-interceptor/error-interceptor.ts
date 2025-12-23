import { AuthenticationService } from '@/@core/services/authentication-service/authentication.service';
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, map, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {

  const authenticationService = inject(AuthenticationService);
  const router = inject(Router);

  return next(req).pipe(
            map(res => {

                // return result
                return res;
            }),
            catchError(error => {

                if (error.status === 500) {

                    // print message
                    console.log('backend error');

                    // regirect to error page
                    router.navigate(['./auth/error']);

                } else
                if (error.status === 401) {

                    // print message
                    console.log('token has expired, the user must login again');

                    // regirect to login page
                    authenticationService.logout();
                    router.navigate(['./auth/login']);

                } else if (error.status === 403) {

                    // print message
                    console.log('user lacks priviledges to access this resource');

                    // regirect to login page
                    router.navigate(['./auth/access']);

                }

                let errorResponse: HttpErrorResponse = error.error;
                console.log(JSON.stringify(errorResponse));

                return throwError(() => errorResponse);
            })
        );
};
