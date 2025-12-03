import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewCostComponent } from './view-cost/view-cost.component';

const routes: Routes = [
  {
    path: '',
    component: ViewCostComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewCostRoutingModule { }
