import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ViewInvestmentRoutingModule } from './view-investment-routing.module';
import { ViewInvestmentComponent } from './view-investment/view-investment.component';
import { EntityMenuComponentModule } from 'src/app/@shared/components/shared/entity-menu-component-module/entity-menu-component.module';
import { PageLoadingComponentModule } from 'src/app/@shared/components/shared/page-loading-component-module/page-loading-component.module';
import { PageProcessingComponentModule } from 'src/app/@shared/components/shared/page-processing-component-module/page-processing-component.module';
import { ButtonModule } from 'primeng/button';
import { ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { InputNumberModule } from 'primeng/inputnumber';
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';


@NgModule({
  declarations: [
    ViewInvestmentComponent
  ],
  imports: [
    CommonModule,
    ViewInvestmentRoutingModule,
    ReactiveFormsModule,
    
    EntityMenuComponentModule,
    PageLoadingComponentModule,
    PageProcessingComponentModule,
    
    ButtonModule,
    CalendarModule,
    DropdownModule,
    InputNumberModule,
    InputTextModule,
    ToastModule,
  ]
})
export class ViewInvestmentModule { }
