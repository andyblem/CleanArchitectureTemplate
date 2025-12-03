import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import Swal from 'sweetalert2'
import { TablePageEvent } from 'primeng/table';
import { IPaginatedResponseDto } from '../../../../../@shared/dtos/i-paginated-response-dto';
import { ICreateInvestmentDto } from './dtos/i-create-investment-dto';
import { IndexInvestmentHttpService } from './services/index-investment-http.service';
import { Router } from '@angular/router';
import { IIndexInvestmentParameterDto } from './dtos/i-index-investment-parameter-dto';

@Component({
  selector: 'app-index-investment',
  templateUrl: './index-investment.component.html',
    styleUrls: ['./index-investment.component.scss'],
    providers: [MessageService]
})
export class IndexInvestmentComponent {

    isAddInvestmentModalVisible: boolean;
    isBusy: boolean;
    isInitializing: boolean;

    addInvestmentForm: FormGroup;

    requestParameter: IIndexInvestmentParameterDto;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
    investmentData: IPaginatedResponseDto;

    investmentTypeOptions: SelectItem[];
    sortFilterOptions: SelectItem[];
    sortOrderOptions: SelectItem[];


    constructor(private formBuilder: FormBuilder,
        private indexInvestmentService: IndexInvestmentHttpService,
        private messageService: MessageService,
        private router: Router) {

        this.isAddInvestmentModalVisible = false;
        this.isBusy = false;
        this.isInitializing = true;
        
        const today: Date = new Date();
        this.requestParameter = { 
            pageNumber: 1, 
            pageSize: 10, 
            searchString: '', 
            sortFilter: 'date', 
            sortOrder: 'desc',
            
            investmentTypeId: 0,

            dates: [
                new Date(today.getFullYear(), today.getMonth(), 1),
                today]
        };
        this.sortFilterOptions = [
            {
                label: 'Date',
                value: 'date'
            },
            {
                label: 'Investment',
                value: 'investment'
            }
        ];
        this.sortOrderOptions = [
            {
                label: 'ASC',
                value: 'asc'
            },
            {
                label: 'DESC',
                value: 'desc'
            }
        ];

        this.investmentTypeOptions = [];

        this.investmentData = {} as IPaginatedResponseDto;

        Promise.all([
            this.getInvestmentTypeSelectList()
        ])
        .then(result => {
            this.isInitializing = false;
        });

        this.addInvestmentForm = this.generateAddInvestmentForm();
        this.getPaginatedData(this.requestParameter);
    }


    public generateAddInvestmentForm(): FormGroup {
        const addInvestmentForm = this.formBuilder.group({
            amount: ['', [Validators.required]],
            date: ['', [Validators.required]],
            investmentTypeId: ['', [Validators.required]]
        });

        return addInvestmentForm;
    }

    public getErrorMessage(controlName: string): string {
        const control = this.addInvestmentForm.get(controlName);
        if (control?.errors) {
            if (control.errors["required"]) {
                return 'This field is required';
            }
        }

        return '';
    }

    public getInvestmentTypeSelectList(): void {

        this.indexInvestmentService.getInvestmentTypeSelectList()
        .subscribe({
            next: (successResponse) => {
                this.investmentTypeOptions = successResponse as unknown as SelectItem[];
            },
            error: (errorResponse) => {
                this.investmentTypeOptions = [];
            }
        });
    }

    public getPaginatedData(requestParameter: IIndexInvestmentParameterDto) {
        this.isBusy = true;

        this.indexInvestmentService.getPaginatedList(requestParameter)
            .subscribe({
                next: (successResponse) => {
                    this.isBusy = false;

                    this.investmentData = successResponse as any as IPaginatedResponseDto;
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

    public showAddInvestmentModal(): void {
        this.addInvestmentForm.reset();
        this.isAddInvestmentModalVisible = true;
    }

    public onClearInvestmentType(): void {
        this.requestParameter.investmentTypeId = 0;
    }

    public onClickDelete(investmentId: number): void {
        
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
                this.indexInvestmentService.delete(investmentId)
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
        const today: Date = new Date();
        this.requestParameter = { 
            pageNumber: 1, 
            pageSize: 10, 
            searchString: '', 
            sortFilter: 'date', 
            sortOrder: 'desc',

            investmentTypeId: 0,

            dates: [
                new Date(today.getFullYear(), today.getMonth(), 1),
                today
            ]
        };
        this.getPaginatedData(this.requestParameter);
    }

    public onClickSearch(): void {
        this.getPaginatedData(this.requestParameter);
    }

    public onClickView(investmentId: number): void {
        this.router.navigate(['/features/financial-resource/investment/view', investmentId]);
    }

    public onPageChange(event: TablePageEvent): void {
       
        // update request parameter
        this.requestParameter.pageNumber = event.first + 1;
        this.requestParameter.pageSize = event.rows;

        // get data
        this.getPaginatedData(this.requestParameter);
    }

    public onSubmitAddInvestmentForm(): void {

        // check if form is valid
        const isFormValid = this.addInvestmentForm.valid;
        if (isFormValid) {

            this.isBusy = true;
            const investment = this.addInvestmentForm.value as ICreateInvestmentDto;

            this.indexInvestmentService.create(investment)
                .subscribe({
                    next: (successResponse) => {
                        
                        // redirect to edit
                        this.isBusy = false;
                        this.router.navigate(['/features/financial-resource/investment/view', successResponse]);
                    },
                    error: (errorResponse) => {

                        this.isBusy = false;
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
}
