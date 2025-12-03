import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexVehicleComponent } from './index-vehicle/index-vehicle.component';

const routes: Routes = [
    {
        path: '',
        component: IndexVehicleComponent
    }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IndexVehicleRoutingModule { }
