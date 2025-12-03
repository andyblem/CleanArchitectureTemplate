import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import Swal from 'sweetalert2'
import { TablePageEvent } from 'primeng/table';
import { IPaginatedResponseDto } from '../../../../../@shared/dtos/i-paginated-response-dto';
import { IRequestParameterDto } from '../../../../../@shared/dtos/i-request-parameter-dto';
import { ICreateIncomeDto } from './dtos/i-create-income-dto';
import { IndexIncomeHttpService } from './services/index-income-http.service';
import { Router } from '@angular/router';
import { IIndexIncomeParameterDto } from './dtos/i-index-income-parameter-dto';

@Component({
  selector: 'app-index-income',
  templateUrl: './index-income.component.html',
    styleUrls: ['./index-income.component.scss'],
    providers: [MessageService]
})
export class IndexIncomeComponent {

    isAddIncomeModalVisible: boolean;
    isBusy: boolean;
    isInitializing: boolean;

    addIncomeForm: FormGroup;

    requestParameter: IIndexIncomeParameterDto;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
    incomeData: IPaginatedResponseDto;

    icomeTypeOptions: SelectItem[];
    sortFilterOptions: SelectItem[];
    sortOrderOptions: SelectItem[];
    vehicleOptions: SelectItem[];


    constructor(private formBuilder: FormBuilder,
        private indexIncomeService: IndexIncomeHttpService,
        private messageService: MessageService,
        private router: Router) {

        this.isAddIncomeModalVisible = false;
        this.isBusy = false;
        this.isInitializing = true;
        
        const today: Date = new Date();
        this.requestParameter = { 
            pageNumber: 1, 
            pageSize: 10, 
            searchString: '', 
            sortFilter: 'date', 
            sortOrder: 'desc',
            
            vehicleId: 0,

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
                label: 'Vehicle',
                value: 'vehicle'
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

        this.icomeTypeOptions = [];
        this.vehicleOptions = [];

        this.incomeData = {} as IPaginatedResponseDto;

        Promise.all([
            this.getIncomeTypeSelectList(),
            this.getVehicleSelectList()
        ])
        .then(result => {
            this.isInitializing = false;
        });

        this.addIncomeForm = this.generateAddIncomeForm();
        this.getPaginatedData(this.requestParameter);
    }


    public generateAddIncomeForm(): FormGroup {
        const addIncomeForm = this.formBuilder.group({
            amount: ['', [Validators.required]],
            date: ['', [Validators.required]],
            incomeTypeId: ['', [Validators.required]],
            vehicleId: ['', [Validators.required]]
        });

        return addIncomeForm;
    }

    public getErrorMessage(controlName: string): string {
        const control = this.addIncomeForm.get(controlName);
        if (control?.errors) {
            if (control.errors["required"]) {
                return 'This field is required';
            }
        }

        return '';
    }

    public getIncomeTypeSelectList(): void {

        this.indexIncomeService.getIncomeTypeSelectList()
        .subscribe({
            next: (successResponse) => {
                this.icomeTypeOptions = successResponse as unknown as SelectItem[];
            },
            error: (errorResponse) => {
                this.icomeTypeOptions = [];
            }
        });
    }

    public getVehicleSelectList(): void {

        this.indexIncomeService.getVehicleSelectList()
        .subscribe({
            next: (successResponse) => {
                this.vehicleOptions = successResponse as unknown as SelectItem[];
            },
            error: (errorResponse) => {
                this.vehicleOptions = [];
            }
        });
    }

    public getPaginatedData(requestParameter: IIndexIncomeParameterDto) {
        this.isBusy = true;

        this.indexIncomeService.getPaginatedList(requestParameter)
            .subscribe({
                next: (successResponse) => {
                    this.isBusy = false;

                    this.incomeData = successResponse as any as IPaginatedResponseDto;
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

    public showAddIncomeModal(): void {
        this.addIncomeForm.reset();
        this.isAddIncomeModalVisible = true;
    }

    public onClearVehicle(): void {
        this.requestParameter.vehicleId = 0;
    }

    public onClickDelete(incomeId: number): void {
        
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
                this.indexIncomeService.delete(incomeId)
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

            vehicleId: 0,

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

    public onClickView(incomeId: number): void {
        this.router.navigate(['/features/financial-resource/income/view', incomeId]);
    }

    public onPageChange(event: TablePageEvent): void {
       
        // update request parameter
        this.requestParameter.pageNumber = event.first + 1;
        this.requestParameter.pageSize = event.rows;

        // get data
        this.getPaginatedData(this.requestParameter);
    }

    public onSubmitAddIncomeForm(): void {

        // check if form is valid
        const isFormValid = this.addIncomeForm.valid;
        if (isFormValid) {

            this.isBusy = true;
            const income = this.addIncomeForm.value as ICreateIncomeDto;

            this.indexIncomeService.create(income)
                .subscribe({
                    next: (successResponse) => {
                        
                        // redirect to edit
                        this.isBusy = false;
                        this.router.navigate(['/features/financial-resource/income/view', successResponse]);
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
