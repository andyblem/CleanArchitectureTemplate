import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexVehicleModelComponent } from './index-vehicle-model/index-vehicle-model.component';

const routes: Routes = [
    {
        path: '',
        component: IndexVehicleModelComponent
    }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IndexVehicleModelRoutingModule { }
