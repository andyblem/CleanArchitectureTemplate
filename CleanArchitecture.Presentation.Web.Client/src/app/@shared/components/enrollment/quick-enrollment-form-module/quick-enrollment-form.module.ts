import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';

import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { DropdownModule } from 'primeng/dropdown';
import { EditorModule } from 'primeng/editor';
import { InputTextModule } from 'primeng/inputtext';

import { QuickEnrollmentFormComponent } from './quick-enrollment-form/quick-enrollment-form.component';


const Components = [

  QuickEnrollmentFormComponent,

];
const Angular_Modules = [

  CommonModule,
  FormsModule,
  ReactiveFormsModule,

];
const PrimeNG_Modules = [
    CalendarModule,
    CheckboxModule,
    InputTextModule,
    DropdownModule,
    EditorModule,
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
export class QuickEnrollmentFormModule { }
