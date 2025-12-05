import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HTTP_INTERCEPTORS
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../../services/authentication-service/authentication.service';
import { EnvironmentService } from '../../services/environment/environment.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(private authenticationService: AuthenticationService,
        private environmentService: EnvironmentService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

      const requestForApis = request.url.startsWith(this.environmentService.apiConfig.apiUrl);
      const isLoggedIn = this.authenticationService.isStillLoggedIn;

      if (isLoggedIn && requestForApis) {

          let token = this.authenticationService.token;

          if (token) {
              request = request.clone({
                  headers: request.headers.set('Authorization', `Bearer ${token}`)
              });
          }
      }

      return next.handle(request);
  }
}

export const AuthInterceptorProvider = { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true };
