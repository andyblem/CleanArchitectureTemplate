import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewCostTypeComponent } from './view-cost-type/view-cost-type.component';

const routes: Routes = [
  {
    path: '',
    component: ViewCostTypeComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewCostTypeRoutingModule { }
