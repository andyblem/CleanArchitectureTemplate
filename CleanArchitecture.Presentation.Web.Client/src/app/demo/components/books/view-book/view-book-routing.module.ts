import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewBookComponent } from './view-book/view-book.component';

const routes: Routes = [
  {
    path: '',
    component: ViewBookComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewBookRoutingModule { }
