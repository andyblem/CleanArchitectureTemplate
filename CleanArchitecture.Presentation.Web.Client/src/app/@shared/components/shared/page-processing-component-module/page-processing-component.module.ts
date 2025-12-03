import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';

import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { BlockUIModule } from 'primeng/blockui';

import { PageProcessingComponent } from './page-processing/page-processing.component';


const Components = [

  PageProcessingComponent,

];
const Angular_Modules = [

  CommonModule,

];
const PrimeNG_Modules = [

  BlockUIModule,
  ProgressSpinnerModule,

];


@NgModule({
  declarations: [
    ...Components,
  ],
  imports: [
    ...Angular_Modules,
    ...PrimeNG_Modules,
  ],
  exports: [
    ...Components,

    ...Angular_Modules,
    ...PrimeNG_Modules,
  ],
})
export class PageProcessingComponentModule { }
