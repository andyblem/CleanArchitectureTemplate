import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';

import { NotfoundRoutingModule } from './notfound-routing.module';
import { NotfoundComponent } from './notfound.component';

@NgModule({
    imports: [
        CommonModule,
        NotfoundRoutingModule,
        ButtonModule
    ],
    declarations: [NotfoundComponent]
})
export class NotfoundModule { }
