import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService, SortEvent } from 'primeng/api';
import { TableModule, TablePageEvent } from 'primeng/table';
import { IPaginatedResponseDto } from 'src/app/@shared/dtos/i-paginated-response-dto';
import { IRequestParameterDto } from 'src/app/@shared/dtos/i-request-parameter-dto';
import Swal from 'sweetalert2';
import { IndexBookHttpService } from './services/index-book-http.service';
import { ICreateBookDto } from './dtos/i-create-book-dto';
import { EntityMenuComponentModule } from '@/@shared/components/shared/entity-menu-component-module/entity-menu-component.module';
import { PageLoadingComponentModule } from '@/@shared/components/shared/page-loading-component-module/page-loading-component.module';
import { PageProcessingComponentModule } from '@/@shared/components/shared/page-processing-component-module/page-processing-component.module';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DividerComponentModule } from '@/@shared/components/shared/divider-component-module/divider-component.module';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { PanelModule } from 'primeng/panel';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { TooltipModule } from 'primeng/tooltip';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-index-books',
    providers: [MessageService],
    standalone: true,
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,

        EntityMenuComponentModule, 
        PageLoadingComponentModule, 
        PageProcessingComponentModule,

        ButtonModule, 
        DialogModule, 
        DividerComponentModule, 
        InputTextModule, 
        InputNumberModule, 
        PanelModule, 
        TableModule, 
        ToastModule, 
        ToolbarModule, 
        TooltipModule
    ],
    template: `
        @if (isInitializing == true) {
            <app-page-loading></app-page-loading>
        } @else {
            <!--entity-menu-->
            <app-entity-menu>
                <ng-template #brand>
                    Books
                </ng-template>
                <ng-template #items>
                    <p-button label="New Book"
                        icon="pi pi-plus"
                        pTooltip="Add new record"
                        tooltipPosition="bottom"
                        (onClick)="showAddBookModal()" />
                </ng-template>
            </app-entity-menu>

            <div class="pt-3"></div>

            <!--content-->
            <div class="card">
                <!--filter-bar-->
                <div class="flex justify-between pb-4">
                    <div class="flex align-items-center justify-content-center formgroup-inline gap-2">
                        <input pInputText
                        id="search"
                        type="text"
                        placeholder="Search"
                        class="p-inputtext p-component p-element"
                        [(ngModel)]="requestParameter.searchString">
                    </div>
                    <div class="flex align-items-center justify-content-center gap-2">
                        <p-button label="Clear"
                        icon="pi pi-times"
                        pTooltip="Clear filters"
                        tooltipPosition="bottom"
                        [outlined]="true"
                        (onClick)="onClickRefresh()" />
                        <p-button label="Filter"
                        pTooltip="Filter data"
                        tooltipPosition="bottom"
                        icon="pi pi-filter"
                        [outlined]="true"
                        (onClick)="onClickSearch()" />
                    </div>
                </div>

                <app-divider></app-divider>

                <div class="pt-5"></div>

                <!--table-data-->
                <div class="grid">
                    <div class="col">
                        <p-table 
                            [breakpoint]="'960px'"
                            [customSort]="true"
                            [value]="BookData.data"
                            [paginator]="true"
                            [rows]="requestParameter.pageSize"
                            [rowsPerPageOptions]="[10, 20, 50, 100]"
                            [totalRecords]="BookData.totalRecords"
                            (onPage)="onPageChange($event)"
                            (sortFunction)="sortBooks($event)">
                            <ng-template pTemplate="header">
                                <tr>
                                <th style="width: 2rem">#</th>
                                <th pSortableColumn="title">Title <p-sortIcon field="title"></p-sortIcon></th>
                                <th>ISBN</th>
                                <th pSortableColumn="price">Price <p-sortIcon field="price"></p-sortIcon></th>
                                <th style="width:20%">Actions</th>
                                </tr>
                            </ng-template>
                            <ng-template pTemplate="body" let-book let-rowIndex="rowIndex">
                                <tr>
                                    <td>{{ rowIndex + 1 }}</td>
                                    <td>{{ book.title }}</td>
                                    <td>{{ book.isbn }}</td>
                                    <td>{{ book.price | currency:'USD':'symbol':'1.2-2' }}</td>
                                    <td>
                                        <div class="flex gap-1">
                                            <p-button pTooltip="Edit"
                                                tooltipPosition="bottom"
                                                icon="pi pi-pencil"
                                                size="small"
                                                (click)="onClickView(book.id)" />
                                            <p-button pTooltip="Delete"
                                                tooltipPosition="bottom"
                                                icon="pi pi-trash"
                                                severity="danger"
                                                size="small"
                                                (click)="onClickDelete(book.id)" />
                                        </div>
                                    </td>
                                </tr>
                            </ng-template>
                            <ng-template pTemplate="emptymessage">
                                <tr>
                                <td colspan="5">
                                    No records found
                                </td>
                                </tr>
                            </ng-template>
                        </p-table>
                    </div>
                </div>
            </div>
        }

           <!--add vehicle modal-->
        <p-dialog header="New Book"
            [modal]="true"
            [(visible)]="isAddBookModalVisible"
            [style]="{ width: '50rem' }">

            <form [formGroup]="addBookForm">

                <div class="flex flex-col gap-2">
                    <label for="title">Title</label>
                    <input pInputText
                            id="title"
                            placeholder="Title"
                            formControlName="title"
                            type="text"
                            class="w-full"
                            [invalid]="addBookForm.get('title')?.invalid && addBookForm.get('title')?.touched" />
                        @if (addBookForm.get('title')?.invalid && addBookForm.get('title')?.touched) {
                            <small class="text-red-700">
                                {{ getErrorMessage('title') }}
                            </small>
                        }
                </div>

                <div class="flex flex-col gap-2">
                    <label htmlFor="isbn">ISBN</label>
                    <input pInputText
                        id="isbn"
                        placeholder="ISBN"
                        formControlName="isbn"
                        type="text"
                        class="w-full"
                        [invalid]="addBookForm.get('isbn')?.invalid && addBookForm.get('isbn')?.touched" />
                    @if (addBookForm.get('isbn')?.invalid && addBookForm.get('isbn')?.touched) {
                        <small class="text-red-700">
                            {{ getErrorMessage('isbn') }}
                        </small>
                    }
                </div>

                <div class="flex flex-col gap-2">
                    <label htmlFor="summary">Summary</label>
                    <input pInputText
                        id="summary"
                        placeholder="Summary"
                        formControlName="summary"
                        type="text"
                        class="w-full"
                        [invalid]="addBookForm.get('summary')?.invalid && addBookForm.get('summary')?.touched" />
                    @if (addBookForm.get('summary')?.invalid && addBookForm.get('summary')?.touched) {
                        <small class="text-red-700">
                            {{ getErrorMessage('summary') }}
                        </small>
                    }
                </div>

                <div class="flex flex-col gap-2">
                    <label htmlFor="price">Price</label>
                    <p-inputNumber id="integeronly"
                        formControlName="price"
                        mode="currency"
                        currency="USD"
                        [minFractionDigits]="2"
                        [maxFractionDigits]="2"
                        class="w-full"
                        [invalid]="addBookForm.get('price')?.invalid && addBookForm.get('price')?.touched" />
                    @if (addBookForm.get('price')?.invalid && addBookForm.get('price')?.touched) {
                    <small class="text-red-700">
                        {{ getErrorMessage('price') }}
                    </small>
                    }
                </div>
            </form>

            <ng-template pTemplate="footer">
                <div class="flex justify-content-end gap-2">
                <p-button label="Cancel" severity="secondary" (onClick)="isAddBookModalVisible = false" />
                <p-button label="Save" (onClick)="onSubmitAddBookForm()" />
                </div>
            </ng-template>

        </p-dialog>

        <!--page processing-->
        @if (isBusy) {
            <app-page-processing></app-page-processing>
        }

        <!--toast-->
        <p-toast></p-toast>

    `,
})
export class IndexBooksComponent {

    isAddBookModalVisible: boolean;
    isBusy: boolean;
    isInitializing: boolean;

     addBookForm: FormGroup;

    requestParameter: IRequestParameterDto;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
    BookData: IPaginatedResponseDto;

    constructor(private formBuilder: FormBuilder,
        private indexBookservice: IndexBookHttpService,
        private messageService: MessageService,
        private router: Router) {

        this.isAddBookModalVisible = false;
        this.isBusy = false;
        this.isInitializing = false;

        this.requestParameter = { pageNumber: 1, pageSize: 10, searchString: '', sortFilter: 'name', sortOrder: 'asc' };
        this.BookData = {} as IPaginatedResponseDto;

        this.addBookForm = this.generateAddBookForm();
        this.getPaginatedData(this.requestParameter);
    }

    public generateAddBookForm(): FormGroup {
        const addBookForm = this.formBuilder.group({
          
            isbn: ['', [Validators.required]],
            summary: ['', [Validators.required]],
            title: ['', [Validators.required]],

            price: ['', [Validators.required, Validators.min(0.01)]],
        });

        return addBookForm;
    }

    public getErrorMessage(controlName: string): string {
        const control = this.addBookForm.get(controlName);
        if (control?.errors) {
            if (control.errors["required"]) {
                return 'This field is required';
            }
        }

        return '';
    }

    public getPaginatedData(requestParameter: IRequestParameterDto) {
        this.isBusy = true;

        this.indexBookservice.getPaginatedList(requestParameter)
            .subscribe({
                next: (successResponse) => {
                    this.isBusy = false;

                    this.BookData = successResponse as any as IPaginatedResponseDto;
                },
                error: (errorResponse) => {
                    this.isBusy = false;

                    // show error
                    this.messageService.add({
                        detail: 'Error whilst retrieving data',
                        summary: 'Error',
                        severity: 'error'
                    });
                }
            });
    }

    public showAddBookModal(): void {
        this.addBookForm.reset();
        this.isAddBookModalVisible = true;
    }

    public onClickDelete(bookId: number): void {
        
        // show delete notification
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {

            if (result.isConfirmed) {
                this.isBusy = true;
                this.indexBookservice.delete(bookId)
                    .subscribe({
                        next: (successResponse) => {
                            
                            this.isBusy = false;

                            // show message
                            this.messageService.add({
                                severity: 'info',
                                summary: 'Deleted!',
                                detail: 'Record has been deleted.',
                                key: 'tl',
                                life: 3000
                            });

                            // reload data
                            this.getPaginatedData(this.requestParameter);
                        },
                        error: (errorResponse) => {

                            this.isBusy = false;

                            // show message
                            this.messageService.add({
                                severity: 'error',
                                summary: 'Error',
                                detail: 'Error whilst deleting record',
                                key: 'tl',
                                life: 3000
                            });
                        }
                    });
            }
        });
    }

    public onClickRefresh(): void {
        this.requestParameter = { pageNumber: 1, pageSize: 10, searchString: '', sortFilter: 'name', sortOrder: 'asc' };
        this.getPaginatedData(this.requestParameter);
    }

    public onClickSearch(): void {
        this.getPaginatedData(this.requestParameter);
    }

    public onClickView(bookId: number): void {
        this.router.navigate(['/uikit/books/view', bookId]);
    }

    public onPageChange(event: TablePageEvent): void {
       
        // update request parameter
        this.requestParameter.pageNumber = event.first + 1;
        this.requestParameter.pageSize = event.rows;

        // get data
        this.getPaginatedData(this.requestParameter);
    }

    public onSubmitAddBookForm(): void {

        // check if form is valid
        const isFormValid = this.addBookForm.valid;
        if (isFormValid) {

            this.isBusy = true;
            const Book = this.addBookForm.value as ICreateBookDto;

            this.indexBookservice.create(Book)
                .subscribe({
                    next: (successResponse) => {
                        
                        // redirect to edit
                        this.isBusy = false;
                        this.router.navigate(['/uikit/books/view', (successResponse as any).data]);
                    },
                    error: (errorResponse) => {

                        this.isBusy = false;

                        // show error
                        this.messageService.add({
                            detail: errorResponse.message || 'Error whilst creating book',
                            summary: 'Form invalid',
                            severity: 'error'
                        });
                    }
                });

        } else {

            // set all fields to touched to show validation errors
            this.addBookForm.markAllAsTouched();
            this.addBookForm.updateValueAndValidity();
            
            // show error
            this.messageService.add({
                detail: 'Fix validation errors',
                summary: 'Form invalid',
                severity: 'error'
            });

        }
    }
    
    public sortBooks(event: SortEvent): void {

        if(event.field != null){

            const sortOrder: string = event.order == 1 ? 'asc' : 'desc';

            this.requestParameter.sortFilter = event.field;
            this.requestParameter.sortOrder = sortOrder;
        }
    }
}
