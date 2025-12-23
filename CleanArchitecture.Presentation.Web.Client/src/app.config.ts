import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { APP_INITIALIZER, ApplicationConfig, inject, provideAppInitializer } from '@angular/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideRouter, withEnabledBlockingInitialNavigation, withInMemoryScrolling } from '@angular/router';
import Aura from '@primeuix/themes/aura';
import { providePrimeNG } from 'primeng/config';
import { appRoutes } from './app.routes';
import { initializer } from '@/@core/app-initializer';
import { ApiEndpointService } from '@/@core/services/api-endpoint/api-endpoint.service';
import { EnvironmentService } from '@/@core/services/environment/environment.service';
import { authInterceptor } from '@/@core/interceptors/auth-interceptor/auth-interceptor';
import { errorInterceptor } from '@/@core/interceptors/error-interceptor/error-interceptor';

export const appConfig: ApplicationConfig = {
    providers: [
        
        provideAppInitializer(() => {
            const initializerFn = (initializer)(inject(ApiEndpointService), inject(EnvironmentService));
            return initializerFn();
        }),
        
        provideRouter(appRoutes, withInMemoryScrolling({ anchorScrolling: 'enabled', scrollPositionRestoration: 'enabled' }), withEnabledBlockingInitialNavigation()),
        provideHttpClient(
            withFetch(), 
            withInterceptors([
                authInterceptor,
                errorInterceptor
            ])
        ),
        provideAnimationsAsync(),
        providePrimeNG({ theme: { preset: Aura, options: { darkModeSelector: '.app-dark' } } })
    ]
};
