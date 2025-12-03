import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewInvestmentTypeComponent } from './view-investment-type/view-investment-type.component';

const routes: Routes = [
  {
    path: '',
    component: ViewInvestmentTypeComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewInvestmentTypeRoutingModule { }
