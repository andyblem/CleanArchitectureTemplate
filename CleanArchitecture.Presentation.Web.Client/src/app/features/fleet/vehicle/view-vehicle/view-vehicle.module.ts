import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ViewVehicleRoutingModule } from './view-vehicle-routing.module';
import { ViewVehicleComponent } from './view-vehicle/view-vehicle.component';
import { EntityMenuComponentModule } from 'src/app/@shared/components/shared/entity-menu-component-module/entity-menu-component.module';
import { PageLoadingComponentModule } from 'src/app/@shared/components/shared/page-loading-component-module/page-loading-component.module';
import { PageProcessingComponentModule } from 'src/app/@shared/components/shared/page-processing-component-module/page-processing-component.module';

import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { ReactiveFormsModule } from '@angular/forms';
import { TabViewModule } from 'primeng/tabview';
import { ToastModule } from 'primeng/toast';


@NgModule({
  declarations: [
    ViewVehicleComponent
  ],
  imports: [
    CommonModule,
    ViewVehicleRoutingModule,
    ReactiveFormsModule,
    
    EntityMenuComponentModule,
    PageLoadingComponentModule,
    PageProcessingComponentModule,
    
    ButtonModule,
    CalendarModule,
    DropdownModule,
    InputNumberModule,
    InputTextModule,
    TabViewModule,
    ToastModule,
  ]
})
export class ViewVehicleModule { }
