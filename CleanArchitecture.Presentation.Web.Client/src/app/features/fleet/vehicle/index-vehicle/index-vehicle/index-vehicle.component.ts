import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import Swal from 'sweetalert2'
import { TablePageEvent } from 'primeng/table';
import { IPaginatedResponseDto } from '../../../../../@shared/dtos/i-paginated-response-dto';
import { IRequestParameterDto } from '../../../../../@shared/dtos/i-request-parameter-dto';
import { ICreateVehicleDto } from './dtos/i-create-vehicle-dto';
import { IndexVehicleHttpService } from './services/index-vehicle-http.service';
import { Router } from '@angular/router';
import { DropdownChangeEvent } from 'primeng/dropdown';

@Component({
  selector: 'app-index-vehicle',
  templateUrl: './index-vehicle.component.html',
    styleUrls: ['./index-vehicle.component.scss'],
    providers: [MessageService]
})
export class IndexVehicleComponent {

    isAddVehicleModalVisible: boolean;
    isBusy: boolean;
    isInitializing: boolean;

    addVehicleForm: FormGroup;

    requestParameter: IRequestParameterDto;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
    vehicleData: IPaginatedResponseDto;

    sortFilterOptions: SelectItem[];
    sortOrderOptions: SelectItem[];
    vehicleMakeOptions: SelectItem[];
    vehicleModelOptions: SelectItem[];


    constructor(private formBuilder: FormBuilder,
        private indexVehicleService: IndexVehicleHttpService,
        private messageService: MessageService,
        private router: Router) {

        this.isAddVehicleModalVisible = false;
        this.isBusy = false;
        this.isInitializing = true;

        this.requestParameter = { pageNumber: 1, pageSize: 10, searchString: '', sortFilter: 'registrationNumber', sortOrder: 'asc' };
        this.sortFilterOptions = [
            {
                label: 'Registration Number',
                value: 'registrationNumber'
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
        this.vehicleModelOptions = [];

        this.vehicleData = {} as IPaginatedResponseDto;

        this.addVehicleForm = this.generateAddVehicleForm();

        Promise.all([
            this.getVehicleMakesSelectList()
        ])
        .then(result => {
            
            this.isInitializing = false;
            this.getPaginatedData(this.requestParameter);
        });
    }

    public generateAddVehicleForm(): FormGroup {
        const addVehicleForm = this.formBuilder.group({
            registrationNumber: ['', [Validators.required]],
            vehicleMakeId: ['', [Validators.required]],
            vehicleModelId: [{value: '', disabled: true }, [Validators.required]],
        });

        return addVehicleForm;
    }

    public getErrorMessage(controlName: string): string {
        const control = this.addVehicleForm.get(controlName);
        if (control?.errors) {
            if (control.errors["required"]) {
                return 'This field is required';
            }
        }

        return '';
    }

    public getVehicleMakesSelectList(): void {

        this.indexVehicleService.getVehicleMakeSelectList()
        .subscribe({
            next: (successResponse) => {
                this.vehicleMakeOptions = successResponse as unknown as SelectItem[];
            },
            error: (errorResponse) => {
                this.vehicleMakeOptions = [];
            }
        });
    }

    public getVehicleModelsSelectList(vehicleMakeId: number): void {

        this.indexVehicleService.getVehicleModelSelectList(vehicleMakeId)
        .subscribe({
            next: (successResponse) => {
                this.vehicleModelOptions = successResponse as unknown as SelectItem[];
            },
            error: (errorResponse) => {
                this.vehicleModelOptions = [];
            }
        });
    }

    public getPaginatedData(requestParameter: IRequestParameterDto) {
        this.isBusy = true;

        this.indexVehicleService.getPaginatedList(requestParameter)
            .subscribe({
                next: (successResponse) => {
                    this.isBusy = false;

                    this.vehicleData = successResponse as any as IPaginatedResponseDto;
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

    public showAddVehicleModal(): void {
        this.addVehicleForm.reset();
        this.isAddVehicleModalVisible = true;
    }

    public onClickDelete(vehicleId: number): void {
        
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
                this.indexVehicleService.delete(vehicleId)
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

    public onClickReport(vehicleId: number): void {
        this.router.navigate(['/features/fleet/vehicle/report', vehicleId]);
    }

    public onClickSearch(): void {
        this.getPaginatedData(this.requestParameter);
    }

    public onClickView(vehicleId: number): void {
        this.router.navigate(['/features/fleet/vehicle/view', vehicleId]);
    }

    public onPageChange(event: TablePageEvent): void {
       
        // update request parameter
        this.requestParameter.pageNumber = event.first + 1;
        this.requestParameter.pageSize = event.rows;

        // get data
        this.getPaginatedData(this.requestParameter);
    }

    public onVehicleMakeChange(event: DropdownChangeEvent): void {

        if(event.value == 0){

            this.addVehicleForm.get('vehicleModelId')?.disable();
            this.vehicleModelOptions = [];
        } else {

            this.addVehicleForm.get('vehicleModelId')?.enable();
            this.getVehicleModelsSelectList(event.value);
        }
    }

    public onSubmitAddVehicleForm(): void {

        // check if form is valid
        const isFormValid = this.addVehicleForm.valid;
        if (isFormValid) {

            this.isBusy = true;
            const vehicle = this.addVehicleForm.value as ICreateVehicleDto;

            this.indexVehicleService.create(vehicle)
                .subscribe({
                    next: (successResponse) => {
                        
                        // redirect to edit
                        this.isBusy = false;
                        this.router.navigate(['/features/fleet/vehicle/view', successResponse]);
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
