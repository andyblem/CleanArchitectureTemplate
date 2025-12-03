import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./index-vehicle-model/index-vehicle-model.module')
            .then(m => m.IndexVehicleModelModule)
    },
    {
      path: 'view/:id',
      loadChildren: () => import('./view-vehicle-model/view-vehicle-model.module')
          .then(m => m.ViewVehicleModelModule)
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VehicleModelRoutingModule { }
