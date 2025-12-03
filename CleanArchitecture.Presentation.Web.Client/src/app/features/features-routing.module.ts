import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./dashboard/dashboard.module')
            .then(m => m.DashboardModule)
    },
    {
        path: 'fleet',
        loadChildren: () => import('./fleet/fleet.module')
            .then(m => m.FleetModule)
    },
    {
        path: 'financial-resource',
        loadChildren: () => import('./financial-resources/financial-resources.module')
            .then(m => m.FinancialResourcesModule)
    },
    {
        path: 'setup',
        loadChildren: () => import('./setup/setup.module')
            .then(m => m.SetupModule)
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FeaturesRoutingModule { }
