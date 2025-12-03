import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./index-vehicle/index-vehicle.module')
            .then(m => m.IndexVehicleModule)
    },
    {
      path: 'report/:id',
      loadChildren: () => import('./report-vehicle/report-vehicle.module')
        .then(m => m.ReportVehicleModule)
    },
    {
      path: 'view/:id',
      loadChildren: () => import('./view-vehicle/view-vehicle.module')
          .then(m => m.ViewVehicleModule)
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VehicleRoutingModule { }
