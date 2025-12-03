import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexVehicleMakeComponent } from './index-vehicle-make/index-vehicle-make.component';

const routes: Routes = [
    {
        path: '',
        component: IndexVehicleMakeComponent
    }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IndexVehicleMakeRoutingModule { }
