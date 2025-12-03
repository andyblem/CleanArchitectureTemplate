import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReportVehicleComponent } from './report-vehicle/report-vehicle.component';

const routes: Routes = [
  {
    path: '',
    component: ReportVehicleComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportVehicleRoutingModule { }
