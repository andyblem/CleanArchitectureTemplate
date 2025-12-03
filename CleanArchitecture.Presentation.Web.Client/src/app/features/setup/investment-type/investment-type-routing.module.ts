import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./index-investment-type/index-investment-type.module')
            .then(m => m.IndexInvestmentTypeModule)
    },
    {
      path: 'view/:id',
      loadChildren: () => import('./view-investment-type/view-investment-type.module')
          .then(m => m.ViewInvestmentTypeModule)
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvestmentTypeRoutingModule { }
