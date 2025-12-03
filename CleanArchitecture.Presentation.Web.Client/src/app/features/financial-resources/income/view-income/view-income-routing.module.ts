import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewIncomeComponent } from './view-income/view-income.component';

const routes: Routes = [
  {
    path: '',
    component: ViewIncomeComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewIncomeRoutingModule { }
