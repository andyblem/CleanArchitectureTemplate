import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReportVehicleRoutingModule } from './report-vehicle-routing.module';
import { ReportVehicleComponent } from './report-vehicle/report-vehicle.component';
import { EntityMenuComponentModule } from 'src/app/@shared/components/shared/entity-menu-component-module/entity-menu-component.module';
import { PageLoadingComponentModule } from 'src/app/@shared/components/shared/page-loading-component-module/page-loading-component.module';
import { PageProcessingComponentModule } from 'src/app/@shared/components/shared/page-processing-component-module/page-processing-component.module';

import { ChartModule } from 'primeng/chart';
import { ToastModule } from 'primeng/toast';
import { TooltipModule } from 'primeng/tooltip';
import { ButtonModule } from 'primeng/button';


@NgModule({
  declarations: [
    ReportVehicleComponent
  ],
  imports: [
    CommonModule,
    ReportVehicleRoutingModule,

    EntityMenuComponentModule,
    PageLoadingComponentModule,
    PageProcessingComponentModule,

    ButtonModule,
    ChartModule,
    ToastModule,
    TooltipModule
  ]
})
export class ReportVehicleModule { }
