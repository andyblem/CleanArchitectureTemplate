import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexCostComponent } from './index-cost/index-cost.component';

const routes: Routes = [
    {
        path: '',
        component: IndexCostComponent
    }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IndexCostRoutingModule { }
