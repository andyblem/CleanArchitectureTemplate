import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewVehicleMakeComponent } from './view-vehicle-make/view-vehicle-make.component';

const routes: Routes = [
  {
    path: '',
    component: ViewVehicleMakeComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewVehicleMakeRoutingModule { }
