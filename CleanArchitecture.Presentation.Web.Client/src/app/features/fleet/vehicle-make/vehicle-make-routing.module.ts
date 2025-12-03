import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./index-vehicle-make/index-vehicle-make.module')
            .then(m => m.IndexVehicleMakeModule)
    },
    {
      path: 'view/:id',
      loadChildren: () => import('./view-vehicle-make/view-vehicle-make.module')
          .then(m => m.ViewVehicleMakeModule)
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VehicleMakeRoutingModule { }
