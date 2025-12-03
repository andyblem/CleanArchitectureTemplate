import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EntityMenuComponent } from './entity-menu/entity-menu.component';
import { MenubarModule } from 'primeng/menubar';



@NgModule({
  declarations: [
    EntityMenuComponent,
  ],
  imports: [
      CommonModule,

      MenubarModule,
  ],
  exports: [
      EntityMenuComponent,

      MenubarModule,
  ],
})
export class EntityMenuComponentModule { }
