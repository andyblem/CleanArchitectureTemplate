import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./index-books/index-books.module')
      .then(m => m.IndexBooksModule)
  },
  {
    path: 'view/:id',
    loadChildren: () => import('./view-book/view-book.module')
      .then(m => m.ViewBookModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BooksRoutingModule { }
