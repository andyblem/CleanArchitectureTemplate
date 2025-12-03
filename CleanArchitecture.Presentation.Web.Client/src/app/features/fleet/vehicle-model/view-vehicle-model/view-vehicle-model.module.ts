import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ViewVehicleModelRoutingModule } from './view-vehicle-model-routing.module';
import { ViewVehicleModelComponent } from './view-vehicle-model/view-vehicle-model.component';
import { EntityMenuComponentModule } from 'src/app/@shared/components/shared/entity-menu-component-module/entity-menu-component.module';
import { PageLoadingComponentModule } from 'src/app/@shared/components/shared/page-loading-component-module/page-loading-component.module';
import { PageProcessingComponentModule } from 'src/app/@shared/components/shared/page-processing-component-module/page-processing-component.module';

import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { DropdownModule } from 'primeng/dropdown';
import { TabViewModule } from 'primeng/tabview';
import { DividerComponentModule } from 'src/app/@shared/components/shared/divider-component-module/divider-component.module';


@NgModule({
  declarations: [
    ViewVehicleModelComponent
  ],
  imports: [
    CommonModule,
    ViewVehicleModelRoutingModule,
    ReactiveFormsModule,
    
    DividerComponentModule,
    EntityMenuComponentModule,
    PageLoadingComponentModule,
    PageProcessingComponentModule,
    
    ButtonModule,
    DividerModule,
    DropdownModule,
    InputTextModule,
    TabViewModule,
    ToastModule,
  ]
})
export class ViewVehicleModelModule { }
