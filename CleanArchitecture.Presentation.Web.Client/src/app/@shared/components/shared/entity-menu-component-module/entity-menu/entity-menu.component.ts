import { Component, ContentChild, OnInit, TemplateRef } from '@angular/core';

@Component({
    selector: 'app-entity-menu',
    templateUrl: './entity-menu.component.html',
    styleUrls: ['./entity-menu.component.scss'],
    standalone: false
})
export class EntityMenuComponent implements OnInit {
    
    @ContentChild('brand', { static: true }) brandTemplate!: TemplateRef<any>;
    @ContentChild('items', { static: true }) itemsTemplate!: TemplateRef<any>;
    @ContentChild('breadcrumb', { static: true }) breadcrumbTemplate!: TemplateRef<any>;

    constructor() { }

    ngOnInit(): void {
    }

}
