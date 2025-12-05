import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ViewBookRoutingModule } from './view-book-routing.module';
import { ViewBookComponent } from './view-book/view-book.component';


@NgModule({
  declarations: [
    ViewBookComponent
  ],
  imports: [
    CommonModule,
    ViewBookRoutingModule
  ]
})
export class ViewBookModule { }
