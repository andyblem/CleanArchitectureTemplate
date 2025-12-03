import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: 'vehicle',
        loadChildren: () => import('./vehicle/vehicle.module')
            .then(m => m.VehicleModule)
    },
    {
        path: 'vehicle-make',
        loadChildren: () => import('./vehicle-make/vehicle-make.module')
            .then(m => m.VehicleMakeModule)
    },
    {
        path: 'vehicle-model',
        loadChildren: () => import('./vehicle-model/vehicle-model.module')
            .then(m => m.VehicleModelModule)
    },
    {
        path: 'vehicle-type',
        loadChildren: () => import('./vehicle-type/vehicle-type.module')
            .then(m => m.VehicleTypeModule)
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FleetRoutingModule { }
