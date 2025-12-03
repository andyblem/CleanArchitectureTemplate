import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./index-vehicle-type/index-vehicle-type.module')
            .then(m => m.IndexVehicleTypeModule)
    },
    {
      path: 'view/:id',
      loadChildren: () => import('./view-vehicle-type/view-vehicle-type.module')
          .then(m => m.ViewVehicleTypeModule)
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VehicleTypeRoutingModule { }
