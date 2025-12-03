import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import Swal from 'sweetalert2'
import { TablePageEvent } from 'primeng/table';
import { IPaginatedResponseDto } from '../../../../../@shared/dtos/i-paginated-response-dto';
import { IRequestParameterDto } from '../../../../../@shared/dtos/i-request-parameter-dto';
import { ICreateVehicleTypeDto } from './dtos/i-create-vehicle-type-dto';
import { IndexVehicleTypeHttpService } from './services/index-vehicle-type-http.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-index-vehicle-type',
  templateUrl: './index-vehicle-type.component.html',
    styleUrls: ['./index-vehicle-type.component.scss'],
    providers: [MessageService]
})
export class IndexVehicleTypeComponent {

    isAddVehicleTypeModalVisible: boolean;
    isBusy: boolean;
    isInitializing: boolean;

    addVehicleTypeForm: FormGroup;

    requestParameter: IRequestParameterDto;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
    vehicleTypeData: IPaginatedResponseDto;
    sortFilterOptions: SelectItem[];
    sortOrderOptions: SelectItem[];

    constructor(private formBuilder: FormBuilder,
        private indexVehicleTypeService: IndexVehicleTypeHttpService,
        private messageService: MessageService,
        private router: Router) {

        this.isAddVehicleTypeModalVisible = false;
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
        this.vehicleTypeData = {} as IPaginatedResponseDto;

        this.addVehicleTypeForm = this.generateAddVehicleTypeForm();
        this.getPaginatedData(this.requestParameter);
    }

    public generateAddVehicleTypeForm(): FormGroup {
        const addVehicleTypeForm = this.formBuilder.group({
            name: ['', [Validators.required]]
        });

        return addVehicleTypeForm;
    }

    public getErrorMessage(controlName: string): string {
        const control = this.addVehicleTypeForm.get(controlName);
        if (control?.errors) {
            if (control.errors["required"]) {
                return 'This field is required';
            }
        }

        return '';
    }

    public getPaginatedData(requestParameter: IRequestParameterDto) {
        this.isBusy = true;

        this.indexVehicleTypeService.getPaginatedList(requestParameter)
            .subscribe({
                next: (successResponse) => {
                    this.isBusy = false;

                    this.vehicleTypeData = successResponse as any as IPaginatedResponseDto;
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

    public showAddVehicleTypeModal(): void {
        this.addVehicleTypeForm.reset();
        this.isAddVehicleTypeModalVisible = true;
    }

    public onClickDelete(vehicleTypeId: number): void {
        
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
                this.indexVehicleTypeService.delete(vehicleTypeId)
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

    public onClickView(vehicleTypeId: number): void {
        this.router.navigate(['/features/fleet/vehicle-type/view', vehicleTypeId]);
    }

    public onPageChange(event: TablePageEvent): void {
       
        // update request parameter
        this.requestParameter.pageNumber = event.first + 1;
        this.requestParameter.pageSize = event.rows;

        // get data
        this.getPaginatedData(this.requestParameter);
    }

    public onSubmitAddVehicleTypeForm(): void {

        // check if form is valid
        const isFormValid = this.addVehicleTypeForm.valid;
        if (isFormValid) {

            this.isBusy = true;
            const vehicleType = this.addVehicleTypeForm.value as ICreateVehicleTypeDto;

            this.indexVehicleTypeService.create(vehicleType)
                .subscribe({
                    next: (successResponse) => {
                        
                        // redirect to edit
                        this.isBusy = false;
                        this.router.navigate(['/features/fleet/vehicle-type/view', successResponse]);
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
