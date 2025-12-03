import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'cost-type',
    loadChildren: () => import('./cost-type/cost-type.module')
      .then(m => m.CostTypeModule)
  },
  {
    path: 'income-type',
    loadChildren: () => import('./income-type/income-type.module')
      .then(m => m.IncomeTypeModule)
  },
  {
    path: 'investment-type',
    loadChildren: () => import('./investment-type/investment-type.module')
      .then(m => m.InvestmentTypeModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SetupRoutingModule { }
