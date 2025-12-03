import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexIncomeComponent } from './index-income/index-income.component';

const routes: Routes = [
    {
        path: '',
        component: IndexIncomeComponent
    }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IndexIncomeRoutingModule { }
