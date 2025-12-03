import { Component, ElementRef, ViewChild } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { LayoutService } from "./service/app.layout.service";
import { AuthenticationService } from '../@core/services/authentication-service/authentication.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-topbar',
    templateUrl: './app.topbar.component.html'
})
export class AppTopBarComponent {

    items!: MenuItem[];

    @ViewChild('menubutton') menuButton!: ElementRef;

    @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

    @ViewChild('topbarmenu') menu!: ElementRef;

    constructor(
        private authenticationService: AuthenticationService,
        public layoutService: LayoutService,
        private router: Router) { 
    }

    public onClickSignOutButton(): void {
        
        // logout user and redirect to login
        this.authenticationService.logout();
        this.router.navigate(['/auth/login']);
    }
}
