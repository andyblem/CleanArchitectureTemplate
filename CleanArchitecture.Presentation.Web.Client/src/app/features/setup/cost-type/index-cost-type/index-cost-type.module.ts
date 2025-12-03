import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { IndexCostTypeRoutingModule } from './index-cost-type-routing.module';
import { IndexCostTypeComponent } from './index-cost-type/index-cost-type.component';
import { EntityMenuComponentModule } from '../../../../@shared/components/shared/entity-menu-component-module/entity-menu-component.module';
import { PageLoadingComponentModule } from '../../../../@shared/components/shared/page-loading-component-module/page-loading-component.module';
import { PageProcessingComponentModule } from '../../../../@shared/components/shared/page-processing-component-module/page-processing-component.module';

import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { PanelModule } from 'primeng/panel';
import { TableModule } from 'primeng/table';
import { ToolbarModule } from 'primeng/toolbar';
import { TooltipModule } from 'primeng/tooltip';
import { ToastModule } from 'primeng/toast';
import { DividerComponentModule } from '../../../../@shared/components/shared/divider-component-module/divider-component.module';


@NgModule({
  declarations: [
    IndexCostTypeComponent
  ],
  imports: [
      CommonModule,
      IndexCostTypeRoutingModule,
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
      PanelModule,
      ReactiveFormsModule,
      TableModule,
      ToastModule,
      ToolbarModule,
      TooltipModule
  ]
})
export class IndexCostTypeModule { }
