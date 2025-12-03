import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./index-income-type/index-income-type.module')
            .then(m => m.IndexIncomeTypeModule)
    },
    {
      path: 'view/:id',
      loadChildren: () => import('./view-income-type/view-income-type.module')
          .then(m => m.ViewIncomeTypeModule)
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IncomeTypeRoutingModule { }
