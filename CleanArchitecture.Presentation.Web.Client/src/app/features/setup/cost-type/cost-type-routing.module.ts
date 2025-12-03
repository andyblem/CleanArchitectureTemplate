import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./index-cost-type/index-cost-type.module')
            .then(m => m.IndexCostTypeModule)
    },
    {
      path: 'view/:id',
      loadChildren: () => import('./view-cost-type/view-cost-type.module')
          .then(m => m.ViewCostTypeModule)
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CostTypeRoutingModule { }
