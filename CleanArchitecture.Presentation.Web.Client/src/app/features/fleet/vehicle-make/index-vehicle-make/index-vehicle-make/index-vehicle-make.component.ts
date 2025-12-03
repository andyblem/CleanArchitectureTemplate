import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import Swal from 'sweetalert2'
import { TablePageEvent } from 'primeng/table';
import { IPaginatedResponseDto } from '../../../../../@shared/dtos/i-paginated-response-dto';
import { IRequestParameterDto } from '../../../../../@shared/dtos/i-request-parameter-dto';
import { ICreateVehicleMakeDto } from './dtos/i-create-vehicle-make-dto';
import { IndexVehicleMakeHttpService } from './services/index-vehicle-make-http.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-index-vehicle-make',
  templateUrl: './index-vehicle-make.component.html',
    styleUrls: ['./index-vehicle-make.component.scss'],
    providers: [MessageService]
})
export class IndexVehicleMakeComponent {

    isAddVehicleMakeModalVisible: boolean;
    isBusy: boolean;
    isInitializing: boolean;

    addVehicleMakeForm: FormGroup;

    requestParameter: IRequestParameterDto;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
    vehicleMakeData: IPaginatedResponseDto;
    sortFilterOptions: SelectItem[];
    sortOrderOptions: SelectItem[];

    constructor(private formBuilder: FormBuilder,
        private indexVehicleMakeService: IndexVehicleMakeHttpService,
        private messageService: MessageService,
        private router: Router) {

        this.isAddVehicleMakeModalVisible = false;
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
        this.vehicleMakeData = {} as IPaginatedResponseDto;

        this.addVehicleMakeForm = this.generateAddVehicleMakeForm();
        this.getPaginatedData(this.requestParameter);
    }

    public generateAddVehicleMakeForm(): FormGroup {
        const addVehicleMakeForm = this.formBuilder.group({
            name: ['', [Validators.required]]
        });

        return addVehicleMakeForm;
    }

    public getErrorMessage(controlName: string): string {
        const control = this.addVehicleMakeForm.get(controlName);
        if (control?.errors) {
            if (control.errors["required"]) {
                return 'This field is required';
            }
        }

        return '';
    }

    public getPaginatedData(requestParameter: IRequestParameterDto) {
        this.isBusy = true;

        this.indexVehicleMakeService.getPaginatedList(requestParameter)
            .subscribe({
                next: (successResponse) => {
                    this.isBusy = false;

                    this.vehicleMakeData = successResponse as any as IPaginatedResponseDto;
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

    public showAddVehicleMakeModal(): void {
        this.addVehicleMakeForm.reset();
        this.isAddVehicleMakeModalVisible = true;
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
                this.indexVehicleMakeService.delete(vehicleMakeId)
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
        this.router.navigate(['/features/fleet/vehicle-make/view', vehicleMakeId]);
    }

    public onPageChange(event: TablePageEvent): void {
       
        // update request parameter
        this.requestParameter.pageNumber = event.first + 1;
        this.requestParameter.pageSize = event.rows;

        // get data
        this.getPaginatedData(this.requestParameter);
    }

    public onSubmitAddVehicleMakeForm(): void {

        // check if form is valid
        const isFormValid = this.addVehicleMakeForm.valid;
        if (isFormValid) {

            this.isBusy = true;
            const vehicleMake = this.addVehicleMakeForm.value as ICreateVehicleMakeDto;

            this.indexVehicleMakeService.create(vehicleMake)
                .subscribe({
                    next: (successResponse) => {
                        
                        // redirect to edit
                        this.isBusy = false;
                        this.router.navigate(['/features/fleet/vehicle-make/view', successResponse]);
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
