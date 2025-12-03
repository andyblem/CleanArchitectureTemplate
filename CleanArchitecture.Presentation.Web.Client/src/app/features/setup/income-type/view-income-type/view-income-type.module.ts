import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ViewIncomeTypeRoutingModule } from './view-income-type-routing.module';
import { ViewIncomeTypeComponent } from './view-income-type/view-income-type.component';
import { EntityMenuComponentModule } from 'src/app/@shared/components/shared/entity-menu-component-module/entity-menu-component.module';
import { PageLoadingComponentModule } from 'src/app/@shared/components/shared/page-loading-component-module/page-loading-component.module';
import { PageProcessingComponentModule } from 'src/app/@shared/components/shared/page-processing-component-module/page-processing-component.module';
import { ButtonModule } from 'primeng/button';
import { ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';


@NgModule({
  declarations: [
    ViewIncomeTypeComponent
  ],
  imports: [
    CommonModule,
    ViewIncomeTypeRoutingModule,
    ReactiveFormsModule,
    
    EntityMenuComponentModule,
    PageLoadingComponentModule,
    PageProcessingComponentModule,
    
    ButtonModule,
    InputTextModule,
    ToastModule,
  ]
})
export class ViewIncomeTypeModule { }
