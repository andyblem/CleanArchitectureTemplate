import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService, SortEvent } from 'primeng/api';
import { TablePageEvent } from 'primeng/table';
import { IPaginatedResponseDto } from 'src/app/@shared/dtos/i-paginated-response-dto';
import { IRequestParameterDto } from 'src/app/@shared/dtos/i-request-parameter-dto';
import Swal from 'sweetalert2';
import { IndexBookHttpService } from './services/index-book-http.service';
import { ICreateBookDto } from './dtos/i-create-book-dto';

@Component({
    selector: 'app-index-books',
    templateUrl: './index-books.component.html',
    styleUrls: ['./index-books.component.scss'],
    providers: [MessageService]
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

    public onClickView(BookId: number): void {
        this.router.navigate(['/features/fleet/vehicle-make/view', BookId]);
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
                        this.router.navigate(['/features/books/view', successResponse]);
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
