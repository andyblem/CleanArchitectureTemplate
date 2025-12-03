import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ViewVehicleMakeRoutingModule } from './view-vehicle-make-routing.module';
import { ViewVehicleMakeComponent } from './view-vehicle-make/view-vehicle-make.component';
import { EntityMenuComponentModule } from 'src/app/@shared/components/shared/entity-menu-component-module/entity-menu-component.module';
import { PageLoadingComponentModule } from 'src/app/@shared/components/shared/page-loading-component-module/page-loading-component.module';
import { PageProcessingComponentModule } from 'src/app/@shared/components/shared/page-processing-component-module/page-processing-component.module';
import { ButtonModule } from 'primeng/button';
import { ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';


@NgModule({
  declarations: [
    ViewVehicleMakeComponent
  ],
  imports: [
    CommonModule,
    ViewVehicleMakeRoutingModule,
    ReactiveFormsModule,
    
    EntityMenuComponentModule,
    PageLoadingComponentModule,
    PageProcessingComponentModule,
    
    ButtonModule,
    InputTextModule,
    ToastModule,
  ]
})
export class ViewVehicleMakeModule { }
