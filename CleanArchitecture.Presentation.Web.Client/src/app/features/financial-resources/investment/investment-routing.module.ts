import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./index-investment/index-investment.module')
            .then(m => m.IndexInvestmentModule)
    },
    {
      path: 'view/:id',
      loadChildren: () => import('./view-investment/view-investment.module')
          .then(m => m.ViewInvestmentModule)
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvestmentRoutingModule { }
