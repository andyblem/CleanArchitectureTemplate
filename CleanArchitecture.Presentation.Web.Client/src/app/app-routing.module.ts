import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { NotfoundComponent } from './demo/components/notfound/notfound.component';
import { AppLayoutComponent } from "./layout/app.layout.component";
import { AuthGuard } from './@core/guards/auth-guard/auth.guard';

@NgModule({
    imports: [
        RouterModule.forRoot([
            {
                path: '',
                redirectTo: 'features',
                pathMatch: 'full'
            },
            {
                path: 'auth',
                loadChildren: () => import('./demo/components/auth/auth.module')
                    .then(m => m.AuthModule)
            },
            {
                path: 'features',
                component: AppLayoutComponent,
                canActivate: [AuthGuard],
                canActivateChild: [AuthGuard],
                loadChildren: () => import('./demo/demo.module')
                    .then(m => m.DemoModule)
            },
            {
                path: 'notfound',
                loadChildren: () => import('./demo/components/notfound/notfound.module')
                    .then(m => m.NotfoundModule)
            },
            {
                path: '**',
                redirectTo: '/notfound'
            },
        ],
        {
            scrollPositionRestoration: 'enabled',
            anchorScrolling: 'enabled',
            onSameUrlNavigation: 'reload'
        })
    ],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
