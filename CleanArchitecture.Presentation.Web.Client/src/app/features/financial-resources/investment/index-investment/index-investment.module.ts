import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { IndexInvestmentRoutingModule } from './index-investment-routing.module';
import { IndexInvestmentComponent } from './index-investment/index-investment.component';
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
import { CalendarModule } from 'primeng/calendar';
import { InputNumberModule } from 'primeng/inputnumber';


@NgModule({
  declarations: [
    IndexInvestmentComponent
  ],
  imports: [
      CommonModule,
      IndexInvestmentRoutingModule,
      FormsModule,
      ReactiveFormsModule,

      EntityMenuComponentModule,
      PageLoadingComponentModule,
      PageProcessingComponentModule,

      ButtonModule,
      CalendarModule,
      DialogModule,
      DividerComponentModule,
      DropdownModule,
      InputNumberModule,
      InputTextModule,
      PanelModule,
      ReactiveFormsModule,
      TableModule,
      ToastModule,
      ToolbarModule,
      TooltipModule
  ]
})
export class IndexInvestmentModule { }
