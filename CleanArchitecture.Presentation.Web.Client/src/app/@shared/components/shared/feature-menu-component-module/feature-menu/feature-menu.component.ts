import { Component, Input, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
    selector: 'app-feature-menu',
    templateUrl: './feature-menu.component.html',
    styleUrls: ['./feature-menu.component.scss'],
    standalone: false
})
export class FeatureMenuComponent implements OnInit {


  @Input() menuItems: MenuItem[] = [];


  constructor() { }


  ngOnInit(): void {
  }

}
