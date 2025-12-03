import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from "primeng/button";
import { AccountSettingsComponent } from './account-settings.component';
import { TooltipModule } from 'primeng/tooltip';
import { SyncAccountsModule } from './sync-accounts-module/sync-accounts.module';



@NgModule({
  declarations: [
    AccountSettingsComponent
  ],
  imports: [
      CommonModule,

      SyncAccountsModule,

      ButtonModule,
      TooltipModule
    ],
    exports: [
        AccountSettingsComponent
    ]
})
export class AccountSettingsModule { }
