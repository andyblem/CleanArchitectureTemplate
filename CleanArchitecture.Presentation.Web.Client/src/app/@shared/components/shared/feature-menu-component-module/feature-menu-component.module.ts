import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MenubarModule } from 'primeng/menubar';

import { FeatureMenuComponent } from './feature-menu/feature-menu.component';


const Components = [

  FeatureMenuComponent,
];
const Angular_Modules = [

  CommonModule,
];
const PrimeNG_Modules = [

  MenubarModule,
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
export class FeatureMenuComponentModule { }
