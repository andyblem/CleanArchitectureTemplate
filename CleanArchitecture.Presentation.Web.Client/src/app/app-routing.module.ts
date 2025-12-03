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
                redirectTo: '',
                pathMatch: 'full'
            },
            {
                path: '',
                component: AppLayoutComponent,
                loadChildren: () => import('./demo/demo.module')
                    .then(m => m.DemoModule)
            },
            {
                path: 'auth',
                loadChildren: () => import('./demo/components/auth/auth.module')
                    .then(m => m.AuthModule)
            },
            /*{
                path: 'features',
                canActivate: [AuthGuard],
                canActivateChild: [AuthGuard],
                component: AppLayoutComponent,
                loadChildren: () => import('./features/features.module')
                    .then(m => m.FeaturesModule)
            },*/
            {
                path: 'notfound',
                component: NotfoundComponent
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
