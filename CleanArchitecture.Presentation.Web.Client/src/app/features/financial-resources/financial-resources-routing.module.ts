import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'cost',
    loadChildren: () => import('./cost/cost.module')
      .then(m => m.CostModule)
  },
  {
    path: 'income',
    loadChildren: () => import('./income/income.module')
      .then(m => m.IncomeModule)
  },
  {
    path: 'investment',
    loadChildren: () => import('./investment/investment.module')
      .then(m => m.InvestmentModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinancialResourcesRoutingModule { }
