import { initializer } from './app-initializer';
import { ModuleWithProviders, NgModule, Optional, SkipSelf, inject, provideAppInitializer } from '@angular/core';
import { CommonModule } from '@angular/common';
import { throwIfAlreadyLoaded } from './module-import-guard';
import { AuthGuard } from './guards/auth-guard/auth.guard';
import { ApiEndpointService } from './services/api-endpoint/api-endpoint.service';
import { EnvironmentService } from './services/environment/environment.service';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
  ]
})
export class CoreModule {

    constructor(
        @Optional() @SkipSelf() parentModule: CoreModule) {

        throwIfAlreadyLoaded(parentModule, 'CoreModule');
    }

    static forRoot(): ModuleWithProviders<CoreModule> {
        return {
            ngModule: CoreModule,
            providers: [
                {
                    provide: AuthGuard,
                    useValue: AuthGuard
                },
                provideAppInitializer(() => {
        const initializerFn = (initializer)(inject(ApiEndpointService), inject(EnvironmentService));
        return initializerFn();
      }),
            ]
        };
    }
}
