import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import Swal from 'sweetalert2'
import { TablePageEvent } from 'primeng/table';
import { IPaginatedResponseDto } from '../../../../../@shared/dtos/i-paginated-response-dto';
import { IRequestParameterDto } from '../../../../../@shared/dtos/i-request-parameter-dto';
import { ICreateIncomeTypeDto } from './dtos/i-create-income-type-dto';
import { IndexIncomeTypeHttpService } from './services/index-income-type-http.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-index-income-type',
  templateUrl: './index-income-type.component.html',
    styleUrls: ['./index-income-type.component.scss'],
    providers: [MessageService]
})
export class IndexIncomeTypeComponent {

    isAddIncomeTypeModalVisible: boolean;
    isBusy: boolean;
    isInitializing: boolean;

    addIncomeTypeForm: FormGroup;

    requestParameter: IRequestParameterDto;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
    incomeTypeData: IPaginatedResponseDto;

    sortFilterOptions: SelectItem[];
    sortOrderOptions: SelectItem[];

    constructor(private formBuilder: FormBuilder,
        private indexIncomeTypeService: IndexIncomeTypeHttpService,
        private messageService: MessageService,
        private router: Router) {

        this.isAddIncomeTypeModalVisible = false;
        this.isBusy = false;
        this.isInitializing = false;

        this.requestParameter = { pageNumber: 1, pageSize: 10, searchString: '', sortFilter: 'name', sortOrder: 'asc' };
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

        this.incomeTypeData = {} as IPaginatedResponseDto;

        this.addIncomeTypeForm = this.generateAddIncomeTypeForm();
        this.getPaginatedData(this.requestParameter);
    }

    public generateAddIncomeTypeForm(): FormGroup {
        const addIncomeTypeForm = this.formBuilder.group({
            name: ['', [Validators.required]]
        });

        return addIncomeTypeForm;
    }

    public getErrorMessage(controlName: string): string {
        const control = this.addIncomeTypeForm.get(controlName);
        if (control?.errors) {
            if (control.errors["required"]) {
                return 'This field is required';
            }
        }

        return '';
    }

    public getPaginatedData(requestParameter: IRequestParameterDto) {
        this.isBusy = true;

        this.indexIncomeTypeService.getPaginatedList(requestParameter)
            .subscribe({
                next: (successResponse) => {
                    this.isBusy = false;

                    this.incomeTypeData = successResponse as any as IPaginatedResponseDto;
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

    public showAddIncomeTypeModal(): void {
        this.addIncomeTypeForm.reset();
        this.isAddIncomeTypeModalVisible = true;
    }

    public onClickDelete(vehicleMakeId: number): void {
        
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
                this.indexIncomeTypeService.delete(vehicleMakeId)
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

    public onClickView(vehicleMakeId: number): void {
        this.router.navigate(['/features/setup/income-type/view', vehicleMakeId]);
    }

    public onPageChange(event: TablePageEvent): void {
       
        // update request parameter
        this.requestParameter.pageNumber = event.first + 1;
        this.requestParameter.pageSize = event.rows;

        // get data
        this.getPaginatedData(this.requestParameter);
    }

    public onSubmitAddIncomeTypeForm(): void {

        // check if form is valid
        const isFormValid = this.addIncomeTypeForm.valid;
        if (isFormValid) {

            this.isBusy = true;
            const vehicleMake = this.addIncomeTypeForm.value as ICreateIncomeTypeDto;

            this.indexIncomeTypeService.create(vehicleMake)
                .subscribe({
                    next: (successResponse) => {
                        
                        // redirect to edit
                        this.isBusy = false;
                        this.router.navigate(['/features/setup/income-type/view', successResponse]);
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
