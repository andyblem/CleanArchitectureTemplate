import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexIncomeTypeComponent } from './index-income-type/index-income-type.component';

const routes: Routes = [
    {
        path: '',
        component: IndexIncomeTypeComponent
    }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IndexIncomeTypeRoutingModule { }
