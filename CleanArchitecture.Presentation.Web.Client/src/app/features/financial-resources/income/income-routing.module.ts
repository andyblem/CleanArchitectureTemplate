import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./index-income/index-income.module')
            .then(m => m.IndexIncomeModule)
    },
    {
      path: 'view/:id',
      loadChildren: () => import('./view-income/view-income.module')
          .then(m => m.ViewIncomeModule)
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IncomeRoutingModule { }
