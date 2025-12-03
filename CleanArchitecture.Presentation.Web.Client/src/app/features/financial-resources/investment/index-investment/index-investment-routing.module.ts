import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexInvestmentComponent } from './index-investment/index-investment.component';

const routes: Routes = [
    {
        path: '',
        component: IndexInvestmentComponent
    }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IndexInvestmentRoutingModule { }
