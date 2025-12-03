import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SignUpRoutingModule } from './sign-up-routing.module';
import { SignUpComponent } from './sign-up.component';

import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PasswordModule } from 'primeng/password';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { PageProcessingComponentModule } from '../../../@shared/components/shared/page-processing-component-module/page-processing-component.module';
import { TooltipModule } from 'primeng/tooltip';


@NgModule({
  declarations: [
      SignUpComponent
  ],
  imports: [
      CommonModule,
      FormsModule,
      ReactiveFormsModule,

      PageProcessingComponentModule,
      SignUpRoutingModule,

      ButtonModule,
      CheckboxModule,
      InputTextModule,
      PasswordModule,
      ToastModule,
      TooltipModule
  ]
})
export class SignUpModule { }
