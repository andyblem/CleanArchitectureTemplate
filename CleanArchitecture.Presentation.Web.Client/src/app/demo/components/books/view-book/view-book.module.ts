import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ViewBookRoutingModule } from './view-book-routing.module';
import { ViewBookComponent } from './view-book/view-book.component';
import { ReactiveFormsModule } from '@angular/forms';
import { EntityMenuComponentModule } from 'src/app/@shared/components/shared/entity-menu-component-module/entity-menu-component.module';
import { PageLoadingComponentModule } from 'src/app/@shared/components/shared/page-loading-component-module/page-loading-component.module';
import { PageProcessingComponentModule } from 'src/app/@shared/components/shared/page-processing-component-module/page-processing-component.module';
import { ButtonModule } from 'primeng/button';
import { EditorModule } from 'primeng/editor';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';


@NgModule({
  declarations: [
    ViewBookComponent
  ],
  imports: [
    CommonModule,
    ViewBookRoutingModule,
    ReactiveFormsModule,
    
    EntityMenuComponentModule,
    PageLoadingComponentModule,
    PageProcessingComponentModule,
    
    ButtonModule,
    EditorModule,
    InputTextModule,
    ToastModule,
  ]
})
export class ViewBookModule { }
