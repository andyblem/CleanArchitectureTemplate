import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PasswordModule } from 'primeng/password';
import { InputTextModule } from 'primeng/inputtext';
import { PageProcessingComponentModule } from '../../../@shared/components/shared/page-processing-component-module/page-processing-component.module';
import { ToastModule } from 'primeng/toast';
import { TooltipModule } from 'primeng/tooltip';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,

        PageProcessingComponentModule,
        LoginRoutingModule,

        ButtonModule,
        CheckboxModule,
        InputTextModule,
        PasswordModule,
        ToastModule,
        TooltipModule
    ],
    declarations: [LoginComponent]
})
export class LoginModule { }
