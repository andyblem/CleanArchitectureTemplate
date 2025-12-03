import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewVehicleTypeComponent } from './view-vehicle-type/view-vehicle-type.component';

const routes: Routes = [
  {
    path: '',
    component: ViewVehicleTypeComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewVehicleTypeRoutingModule { }
