import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexCostTypeComponent } from './index-cost-type/index-cost-type.component';

const routes: Routes = [
    {
        path: '',
        component: IndexCostTypeComponent
    }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IndexCostTypeRoutingModule { }
