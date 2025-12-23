import { Routes } from "@angular/router";
import { IndexBooksComponent } from "./index-books/index-books.component";
import { ViewBookComponent } from "./view-book/view-book.component";

export default [
    {path:'', data:{breadcrumb:'Books'}, component: IndexBooksComponent},
    {path:'view/:id', data:{breadcrumb:'View Book'}, component: ViewBookComponent}
] as Routes;