import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import Swal from 'sweetalert2'
import { TablePageEvent } from 'primeng/table';
import { IPaginatedResponseDto } from '../../../../../@shared/dtos/i-paginated-response-dto';
import { IRequestParameterDto } from '../../../../../@shared/dtos/i-request-parameter-dto';
import { ICreateVehicleModelDto } from './dtos/i-create-vehicle-model-dto';
import { IndexVehicleModelHttpService } from './services/index-vehicle-model-http.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-index-vehicle-model',
  templateUrl: './index-vehicle-model.component.html',
    styleUrls: ['./index-vehicle-model.component.scss'],
    providers: [MessageService]
})
export class IndexVehicleModelComponent {

    isAddVehicleModelModalVisible: boolean;
    isBusy: boolean;
    isInitializing: boolean;

    addVehicleModelForm: FormGroup;

    requestParameter: IRequestParameterDto;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
    vehicleModelData: IPaginatedResponseDto;

    sortFilterOptions: SelectItem[];
    sortOrderOptions: SelectItem[];
    vehicleMakeOptions: SelectItem[];
    vehicleTypeOptions: SelectItem[];

    constructor(private formBuilder: FormBuilder,
        private indexVehicleModelService: IndexVehicleModelHttpService,
        private messageService: MessageService,
        private router: Router) {

        this.isAddVehicleModelModalVisible = false;
        this.isBusy = false;
        this.isInitializing = true;

        this.requestParameter = { pageNumber: 1, pageSize: 10, searchString: '', sortFilter: 'name', sortOrder: 'asc' };
        this.sortFilterOptions = [
            {
                label: 'Name',
                value: 'name'
            },
            {
                label: 'Make',
                value: 'make'
            },
            {
                label: 'Type',
                value: 'type'
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
        this.vehicleMakeOptions = [];
        this.vehicleTypeOptions = [];

        this.vehicleModelData = {} as IPaginatedResponseDto;

        this.addVehicleModelForm = this.generateAddVehicleModelForm();

        Promise.all([this.getVehicleMakesSelectList(), this.getVehicleTypesSelectList()])
        .then(result => {
            
            this.isInitializing = false;
            this.getPaginatedData(this.requestParameter);
        });
    }

    public generateAddVehicleModelForm(): FormGroup {
        const addVehicleModelForm = this.formBuilder.group({
            name: ['', [Validators.required]],
            vehicleMakeId: ['', [Validators.required]],
            vehicleTypeId: ['', [Validators.required]]
        });

        return addVehicleModelForm;
    }

    public getErrorMessage(controlName: string): string {
        const control = this.addVehicleModelForm.get(controlName);
        if (control?.errors) {
            if (control.errors["required"]) {
                return 'This field is required';
            }
        }

        return '';
    }

    public getPaginatedData(requestParameter: IRequestParameterDto) {
        this.isBusy = true;

        this.indexVehicleModelService.getPaginatedList(requestParameter)
            .subscribe({
                next: (successResponse) => {
                    this.isBusy = false;

                    this.vehicleModelData = successResponse as any as IPaginatedResponseDto;
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

    public getVehicleMakesSelectList(): void {

        this.indexVehicleModelService.getVehicleMakeSelectList()
        .subscribe({
            next: (successResponse) => {
                this.vehicleMakeOptions = successResponse as unknown as SelectItem[];
            },
            error: (errorResponse) => {
                this.vehicleMakeOptions = [];
            }
        });
    }

    public getVehicleTypesSelectList(): void {

        this.indexVehicleModelService.getVehicleTypeSelectList()
        .subscribe({
            next: (successResponse) => {
                this.vehicleTypeOptions = successResponse as unknown as SelectItem[];
            },
            error: (errorResponse) => {
                this.vehicleTypeOptions = [];
            }
        });
    }

    public showAddVehicleModelModal(): void {
        this.addVehicleModelForm.reset();
        this.isAddVehicleModelModalVisible = true;
    }

    public onClickDelete(vehicleModelId: number): void {
        
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
                this.indexVehicleModelService.delete(vehicleModelId)
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

    public onClickView(vehicleModelId: number): void {
        this.router.navigate(['/features/fleet/vehicle-model/view', vehicleModelId]);
    }

    public onPageChange(event: TablePageEvent): void {
       
        // update request parameter
        this.requestParameter.pageNumber = event.first + 1;
        this.requestParameter.pageSize = event.rows;

        // get data
        this.getPaginatedData(this.requestParameter);
    }

    public onSubmitAddVehicleModelForm(): void {

        // check if form is valid
        const isFormValid = this.addVehicleModelForm.valid;
        if (isFormValid) {

            this.isBusy = true;
            const vehicleModel = this.addVehicleModelForm.value as ICreateVehicleModelDto;

            this.indexVehicleModelService.create(vehicleModel)
                .subscribe({
                    next: (successResponse) => {
                        
                        // redirect to edit
                        this.isBusy = false;
                        this.router.navigate(['/features/fleet/vehicle-model/view', successResponse]);
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
