import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import Swal from 'sweetalert2'
import { TablePageEvent } from 'primeng/table';
import { IPaginatedResponseDto } from '../../../../../@shared/dtos/i-paginated-response-dto';
import { ICreateCostDto } from './dtos/i-create-cost-dto';
import { IndexCostHttpService } from './services/index-cost-http.service';
import { Router } from '@angular/router';
import { IIndexCostParameterDto } from './dtos/i-index-cost-parameter-dto';

@Component({
  selector: 'app-index-cost',
  templateUrl: './index-cost.component.html',
    styleUrls: ['./index-cost.component.scss'],
    providers: [MessageService]
})
export class IndexCostComponent {

    isAddCostModalVisible: boolean;
    isBusy: boolean;
    isInitializing: boolean;

    addCostForm: FormGroup;

    requestParameter: IIndexCostParameterDto;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
    costData: IPaginatedResponseDto;

    incomeTypeOptions: SelectItem[];
    sortFilterOptions: SelectItem[];
    sortOrderOptions: SelectItem[];
    vehicleOptions: SelectItem[];


    constructor(private formBuilder: FormBuilder,
        private indexCostService: IndexCostHttpService,
        private messageService: MessageService,
        private router: Router) {

        this.isAddCostModalVisible = false;
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
                label: 'Name',
                value: 'name'
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

        this.incomeTypeOptions = [];
        this.vehicleOptions = [];

        this.costData = {} as IPaginatedResponseDto;

        Promise.all([
            this.getIncomeTypeSelectList(),
            this.getVehicleSelectList()
        ])
        .then(result => {
            this.isInitializing = false;
        });

        this.addCostForm = this.generateAddCostForm();
        this.getPaginatedData(this.requestParameter);
    }

    public generateAddCostForm(): FormGroup {
        const addCostForm = this.formBuilder.group({
            amount: ['', [Validators.required]],
            amountPaid: ['', [Validators.required]],
            costTypeId: ['', [Validators.required]],
            vehicleId: ['', [Validators.required]],
            date: ['', [Validators.required]]
        });

        return addCostForm;
    }

    public getErrorMessage(controlName: string): string {
        const control = this.addCostForm.get(controlName);
        if (control?.errors) {
            if (control.errors["required"]) {
                return 'This field is required';
            }
        }

        return '';
    }

    public getIncomeTypeSelectList(): void {

        this.indexCostService.getIncomeTypeSelectList()
        .subscribe({
            next: (successResponse) => {
                this.incomeTypeOptions = successResponse as unknown as SelectItem[];
            },
            error: (errorResponse) => {
                this.incomeTypeOptions = [];
            }
        });
    }

    public getVehicleSelectList(): void {

        this.indexCostService.getVehicleSelectList()
        .subscribe({
            next: (successResponse) => {
                this.vehicleOptions = successResponse as unknown as SelectItem[];
            },
            error: (errorResponse) => {
                this.vehicleOptions = [];
            }
        });
    }

    public getPaginatedData(requestParameter: IIndexCostParameterDto) {
        this.isBusy = true;

        this.indexCostService.getPaginatedList(requestParameter)
            .subscribe({
                next: (successResponse) => {
                    this.isBusy = false;

                    this.costData = successResponse as any as IPaginatedResponseDto;
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

    public showAddCostModal(): void {
        this.addCostForm.reset();
        this.isAddCostModalVisible = true;
    }

    public onClearCost(): void {
        this.requestParameter.vehicleId = 0;
    }

    public onClickDelete(costId: number): void {
        
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
                this.indexCostService.delete(costId)
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

    public onClickRefresh(): void {const today: Date = new Date();
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
        this.getPaginatedData(this.requestParameter);
    }

    public onClickSearch(): void {
        this.getPaginatedData(this.requestParameter);
    }

    public onClickView(costId: number): void {
        this.router.navigate(['/features/financial-resource/cost/view', costId]);
    }

    public onPageChange(event: TablePageEvent): void {
       
        // update request parameter
        this.requestParameter.pageNumber = event.first + 1;
        this.requestParameter.pageSize = event.rows;

        // get data
        this.getPaginatedData(this.requestParameter);
    }

    public onSubmitAddCostForm(): void {

        // check if form is valid
        const isFormValid = this.addCostForm.valid;
        if (isFormValid) {

            this.isBusy = true;
            const cost = this.addCostForm.value as ICreateCostDto;

            this.indexCostService.create(cost)
                .subscribe({
                    next: (successResponse) => {
                        
                        // redirect to edit
                        this.isBusy = false;
                        this.router.navigate(['/features/financial-resource/cost/view', successResponse]);
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
