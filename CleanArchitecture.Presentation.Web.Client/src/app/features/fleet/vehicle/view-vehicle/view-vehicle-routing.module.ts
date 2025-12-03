import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewVehicleComponent } from './view-vehicle/view-vehicle.component';

const routes: Routes = [
  {
    path: '',
    component: ViewVehicleComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewVehicleRoutingModule { }
