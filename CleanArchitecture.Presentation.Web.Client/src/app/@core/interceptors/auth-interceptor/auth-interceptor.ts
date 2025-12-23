import { AuthenticationService } from '@/@core/services/authentication-service/authentication.service';
import { EnvironmentService } from '@/@core/services/environment/environment.service';
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  
  const authenticationService = inject(AuthenticationService);
  const environmentService = inject(EnvironmentService);


  const requestForApis = req.url.startsWith(environmentService.apiConfig.apiUrl);
  const isLoggedIn = authenticationService.isStillLoggedIn;

  if (isLoggedIn && requestForApis) {

      let token = authenticationService.token;

      if (token) {
          req = req.clone({
              headers: req.headers.set('Authorization', `Bearer ${token}`)
          });
      }
  }

  return next(req);
};
