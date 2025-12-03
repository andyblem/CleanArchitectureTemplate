import { inject } from "@angular/core";
import { CanActivateFn, Router, CanActivateChildFn, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { AuthenticationService } from "../../services/authentication-service/authentication.service";

export const AuthGuard: CanActivateFn = (route, state) => {

    // check authentication status
    var authenticationService = inject(AuthenticationService);
    var isAuthenticated = authenticationService.isAuthenticated;

    if (isAuthenticated == true) {

        // return result
        return true;

    } else {

        // logout user
        authenticationService.logout();

        // trigger navigation to login
        var router = inject(Router);
        router.navigate(['/auth/login'], { queryParams: { returnUrl: state.url } });

        // return result
        return false;
    }
};

export const canActivateChild: CanActivateChildFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => AuthGuard(route, state);
