import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./index-cost/index-cost.module')
            .then(m => m.IndexCostModule)
    },
    {
      path: 'view/:id',
      loadChildren: () => import('./view-cost/view-cost.module')
          .then(m => m.ViewCostModule)
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CostRoutingModule { }
