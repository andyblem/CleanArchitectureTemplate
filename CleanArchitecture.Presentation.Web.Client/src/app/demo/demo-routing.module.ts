import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./components/dashboard/dashboard.module')
            .then(m => m.DashboardModule)
    },
    {
        path: 'books',
        loadChildren: () => import('./components/books/books.module')
            .then(m => m.BooksModule)
    },
    {
        path: 'uikit',
        loadChildren: () => import('./components/uikit/uikit.module')
            .then(m => m.UIkitModule)
    },
    {
        path: 'utilities',
        loadChildren: () => import('./components/utilities/utilities.module')
            .then(m => m.UtilitiesModule)
    },
    {
        path: 'documentation',
        loadChildren: () => import('./components/documentation/documentation.module')
            .then(m => m.DocumentationModule)
    },
    {
        path: 'blocks',
        loadChildren: () => import('./components/primeblocks/primeblocks.module')
            .then(m => m.PrimeBlocksModule)
    },
    {
        path: 'pages',
        loadChildren: () => import('./components/pages/pages.module')
            .then(m => m.PagesModule)
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DemoRoutingModule { }
