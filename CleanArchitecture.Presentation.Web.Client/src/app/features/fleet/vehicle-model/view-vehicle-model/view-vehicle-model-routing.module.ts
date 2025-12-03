import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewVehicleModelComponent } from './view-vehicle-model/view-vehicle-model.component';

const routes: Routes = [
  {
    path: '',
    component: ViewVehicleModelComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewVehicleModelRoutingModule { }
