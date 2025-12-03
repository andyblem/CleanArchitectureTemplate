import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewIncomeTypeComponent } from './view-income-type/view-income-type.component';

const routes: Routes = [
  {
    path: '',
    component: ViewIncomeTypeComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewIncomeTypeRoutingModule { }
