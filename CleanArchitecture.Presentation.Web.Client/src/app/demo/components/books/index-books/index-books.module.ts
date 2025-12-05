import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { IndexBooksRoutingModule } from './index-books-routing.module';
import { IndexBooksComponent } from './index-books/index-books.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EntityMenuComponentModule } from 'src/app/@shared/components/shared/entity-menu-component-module/entity-menu-component.module';
import { PageLoadingComponentModule } from 'src/app/@shared/components/shared/page-loading-component-module/page-loading-component.module';
import { PageProcessingComponentModule } from 'src/app/@shared/components/shared/page-processing-component-module/page-processing-component.module';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DividerComponentModule } from 'src/app/@shared/components/shared/divider-component-module/divider-component.module';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { PanelModule } from 'primeng/panel';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { TooltipModule } from 'primeng/tooltip';
import { InputNumberModule } from 'primeng/inputnumber';


@NgModule({
  declarations: [
    IndexBooksComponent
  ],
  imports: [
      CommonModule,
      IndexBooksRoutingModule,
      FormsModule,
      ReactiveFormsModule,

      EntityMenuComponentModule,
      PageLoadingComponentModule,
      PageProcessingComponentModule,

      ButtonModule,
      DialogModule,
      DividerComponentModule,
      DropdownModule,
      InputTextModule,
      InputNumberModule,
      PanelModule,
      ReactiveFormsModule,
      TableModule,
      ToastModule,
      ToolbarModule,
      TooltipModule
  ]
})
export class IndexBooksModule { }
