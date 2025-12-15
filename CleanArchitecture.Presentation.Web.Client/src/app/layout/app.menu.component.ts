import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LayoutService } from './service/app.layout.service';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html',
    standalone: false
})
export class AppMenuComponent implements OnInit {

    model: any[] = [];

    constructor(public layoutService: LayoutService,
        private router: Router) { }

    ngOnInit() {
        this.model =  this.getMenu();
    }

    private getMenu(): any[] {
        return [
            {
                label: 'Home',
                items: [
                    {
                        label: 'Dashboard',
                        icon: 'pi pi-fw pi-home',
                        routerLink: ['/']
                    }
                ]
            },
            {
                label: 'Demo Features',
                items: [
                    {
                        label: 'Books',
                        icon: 'pi pi-fw pi-book',
                        routerLink: ['/features/books']
                    }
                ]
            },
            {
                label: 'UI Components',
                items: [
                    { label: 'Form Layout', icon: 'pi pi-fw pi-id-card', routerLink: ['/features/uikit/formlayout'] },
                    { label: 'Input', icon: 'pi pi-fw pi-check-square', routerLink: ['/features/uikit/input'] },
                    { label: 'Float Label', icon: 'pi pi-fw pi-bookmark', routerLink: ['/features/uikit/floatlabel'] },
                    { label: 'Invalid State', icon: 'pi pi-fw pi-exclamation-circle', routerLink: ['/features/uikit/invalidstate'] },
                    { label: 'Button', icon: 'pi pi-fw pi-box', routerLink: ['/features/uikit/button'] },
                    { label: 'Table', icon: 'pi pi-fw pi-table', routerLink: ['/features/uikit/table'] },
                    { label: 'List', icon: 'pi pi-fw pi-list', routerLink: ['/features/uikit/list'] },
                    { label: 'Tree', icon: 'pi pi-fw pi-share-alt', routerLink: ['/features/uikit/tree'] },
                    { label: 'Panel', icon: 'pi pi-fw pi-tablet', routerLink: ['/features/uikit/panel'] },
                    { label: 'Overlay', icon: 'pi pi-fw pi-clone', routerLink: ['/features/uikit/overlay'] },
                    { label: 'Media', icon: 'pi pi-fw pi-image', routerLink: ['/features/uikit/media'] },
                    { label: 'Menu', icon: 'pi pi-fw pi-bars', routerLink: ['/features/uikit/menu'], routerLinkActiveOptions: { paths: 'subset', queryParams: 'ignored', matrixParams: 'ignored', fragment: 'ignored' } },
                    { label: 'Message', icon: 'pi pi-fw pi-comment', routerLink: ['/features/uikit/message'] },
                    { label: 'File', icon: 'pi pi-fw pi-file', routerLink: ['/features/uikit/file'] },
                    { label: 'Chart', icon: 'pi pi-fw pi-chart-bar', routerLink: ['/features/uikit/charts'] },
                    { label: 'Misc', icon: 'pi pi-fw pi-circle', routerLink: ['/features/uikit/misc'] }
                ]
            },
            {
                label: 'Prime Blocks',
                items: [
                    { label: 'Free Blocks', icon: 'pi pi-fw pi-eye', routerLink: ['/blocks'], badge: 'NEW' },
                    { label: 'All Blocks', icon: 'pi pi-fw pi-globe', url: ['https://www.primefaces.org/primeblocks-ng'], target: '_blank' },
                ]
            },
            {
                label: 'Utilities',
                items: [
                    { label: 'PrimeIcons', icon: 'pi pi-fw pi-prime', routerLink: ['/utilities/icons'] },
                    { label: 'PrimeFlex', icon: 'pi pi-fw pi-desktop', url: ['https://www.primefaces.org/primeflex/'], target: '_blank' },
                ]
            },
            {
                label: 'Pages',
                icon: 'pi pi-fw pi-briefcase',
                items: [
                    {
                        label: 'Landing',
                        icon: 'pi pi-fw pi-globe',
                        routerLink: ['/features/landing']
                    },
                    {
                        label: 'Auth',
                        icon: 'pi pi-fw pi-user',
                        items: [
                            {
                                label: 'Login',
                                icon: 'pi pi-fw pi-sign-in',
                                routerLink: ['/auth/login']
                            },
                            {
                                label: 'Error',
                                icon: 'pi pi-fw pi-times-circle',
                                routerLink: ['/auth/error']
                            },
                            {
                                label: 'Access Denied',
                                icon: 'pi pi-fw pi-lock',
                                routerLink: ['/auth/access']
                            }
                        ]
                    },
                    {
                        label: 'Crud',
                        icon: 'pi pi-fw pi-pencil',
                        routerLink: ['/features/pages/crud']
                    },
                    {
                        label: 'Timeline',
                        icon: 'pi pi-fw pi-calendar',
                        routerLink: ['/features/pages/timeline']
                    },
                    {
                        label: 'Not Found',
                        icon: 'pi pi-fw pi-exclamation-circle',
                        routerLink: ['/features/notfound']
                    },
                    {
                        label: 'Empty',
                        icon: 'pi pi-fw pi-circle-off',
                        routerLink: ['/features/pages/empty']
                    },
                ]
            },
            {
                label: 'Hierarchy',
                items: [
                    {
                        label: 'Submenu 1', icon: 'pi pi-fw pi-bookmark',
                        items: [
                            {
                                label: 'Submenu 1.1', icon: 'pi pi-fw pi-bookmark',
                                items: [
                                    { label: 'Submenu 1.1.1', icon: 'pi pi-fw pi-bookmark' },
                                    { label: 'Submenu 1.1.2', icon: 'pi pi-fw pi-bookmark' },
                                    { label: 'Submenu 1.1.3', icon: 'pi pi-fw pi-bookmark' },
                                ]
                            },
                            {
                                label: 'Submenu 1.2', icon: 'pi pi-fw pi-bookmark',
                                items: [
                                    { label: 'Submenu 1.2.1', icon: 'pi pi-fw pi-bookmark' }
                                ]
                            },
                        ]
                    },
                    {
                        label: 'Submenu 2', icon: 'pi pi-fw pi-bookmark',
                        items: [
                            {
                                label: 'Submenu 2.1', icon: 'pi pi-fw pi-bookmark',
                                items: [
                                    { label: 'Submenu 2.1.1', icon: 'pi pi-fw pi-bookmark' },
                                    { label: 'Submenu 2.1.2', icon: 'pi pi-fw pi-bookmark' },
                                ]
                            },
                            {
                                label: 'Submenu 2.2', icon: 'pi pi-fw pi-bookmark',
                                items: [
                                    { label: 'Submenu 2.2.1', icon: 'pi pi-fw pi-bookmark' },
                                ]
                            },
                        ]
                    }
                ]
            },
            {
                label: 'Get Started',
                items: [
                    {
                        label: 'Documentation', icon: 'pi pi-fw pi-question', routerLink: ['/documentation']
                    },
                    {
                        label: 'View Source', icon: 'pi pi-fw pi-search', url: ['https://github.com/primefaces/sakai-ng'], target: '_blank'
                    }
                ]
            }
        ];
    }

}
