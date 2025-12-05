import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexBooksComponent } from './index-books/index-books.component';

const routes: Routes = [
  {
    path: '',
    component: IndexBooksComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IndexBooksRoutingModule { }
