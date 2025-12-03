import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewInvestmentComponent } from './view-investment/view-investment.component';

const routes: Routes = [
  {
    path: '',
    component: ViewInvestmentComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewInvestmentRoutingModule { }
