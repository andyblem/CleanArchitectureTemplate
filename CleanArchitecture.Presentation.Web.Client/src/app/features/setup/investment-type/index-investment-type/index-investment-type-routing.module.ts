import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexInvestmentTypeComponent } from './index-investment-type/index-investment-type.component';

const routes: Routes = [
    {
        path: '',
        component: IndexInvestmentTypeComponent
    }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IndexInvestmentTypeRoutingModule { }
