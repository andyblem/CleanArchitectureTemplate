import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';

import { SkeletonModule } from 'primeng/skeleton';

import { PageLoadingComponent } from './page-loading/page-loading.component';


const Components = [

  PageLoadingComponent,

];
const Angular_Modules = [

  CommonModule,

];
const PrimeNG_Modules = [

  SkeletonModule,

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
export class PageLoadingComponentModule { }
