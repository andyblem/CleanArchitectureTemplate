import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexVehicleTypeComponent } from './index-vehicle-type/index-vehicle-type.component';

const routes: Routes = [
    {
        path: '',
        component: IndexVehicleTypeComponent
    }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IndexVehicleTypeRoutingModule { }
