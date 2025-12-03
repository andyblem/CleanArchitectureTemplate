import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SyncAccountsComponent } from './sync-accounts/sync-accounts.component';
import { DialogModule } from 'primeng/dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';


@NgModule({
  declarations: [
    SyncAccountsComponent
  ],
  imports: [
      CommonModule,
      FormsModule,
      ReactiveFormsModule,

      ButtonModule,
      DialogModule,
      InputTextModule,
      PasswordModule
    ],
    exports: [
        SyncAccountsComponent
    ]
})
export class SyncAccountsModule { }
