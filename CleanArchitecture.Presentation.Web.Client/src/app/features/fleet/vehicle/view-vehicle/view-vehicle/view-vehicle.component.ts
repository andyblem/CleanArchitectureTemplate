import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ViewVehicleHttpService } from './services/view-vehicle-http.service';
import { switchMap } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IViewVehicleDto } from './dtos/i-view-vehicle-dto';
import { MessageService, SelectItem } from 'primeng/api';
import { DropdownChangeEvent } from 'primeng/dropdown';

@Component({
  selector: 'app-view-vehicle',
  templateUrl: './view-vehicle.component.html',
  styleUrls: ['./view-vehicle.component.scss'],
  providers: [MessageService]
})
export class ViewVehicleComponent implements OnInit {
  
  isBusy: boolean;
  isInitializing: boolean;

  vehicleId: number;

  viewVehicleForm: FormGroup;
    
  vehicleMakeOptions: SelectItem[];
  vehicleModelOptions: SelectItem[];
  vehicleTypeOptions: SelectItem[];
  

  constructor(private formBuilder: FormBuilder,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private router: Router,
    private viewVehicleHttpService: ViewVehicleHttpService
  ) {
    this.isBusy = false;
    this.isInitializing = false;
    this.vehicleId = 0;

    this.vehicleMakeOptions = [];
    this.vehicleModelOptions = [];
    this.vehicleTypeOptions = [];

    this.viewVehicleForm = this.generateViewVehicleForm();
  }

  ngOnInit(): void {

    this.isInitializing = true;

    Promise.all([
      this.getVehicleMakesSelectList()
    ])
    .then(result => {

      this.route.paramMap.pipe(
        switchMap(params => {

          this.vehicleId = parseInt(params.get('id')!, 10);
          return this.viewVehicleHttpService.get(this.vehicleId);
        })
      )
      .subscribe({
        next: (successResponse) => {
          
          const vehicle = successResponse as unknown as IViewVehicleDto;

          this.getVehicleModelsSelectList(vehicle.vehicleMakeId);
          this.viewVehicleForm = this.generateViewVehicleForm(vehicle);

          this.isInitializing = false;
        },
        error: (errorResponse) => {
          this.isInitializing = false;
        }
      });

      this.isInitializing = false;
    });
  }

  public generateViewVehicleForm(): FormGroup;
  public generateViewVehicleForm(vehicle: IViewVehicleDto): FormGroup;
  public generateViewVehicleForm(...args: any[]): FormGroup {

    if(args.length == 0){

      const viewVehicleForm = this.formBuilder.group({
        id: ['', [Validators.required]],
        purchaseAmount: ['', [Validators.required]],
        purchaseDate: ['', [Validators.required]],
        registrationNumber: ['', [Validators.required]],
        vehicleMakeId: ['', [Validators.required]],
        vehicleModelId: ['', [Validators.required]]
      });

      return viewVehicleForm;

    } else {

      const argsData = args[0] as unknown as IViewVehicleDto;
      const viewVehicleForm = this.formBuilder.group({
        id: [argsData.id, [Validators.required]],
        purchaseAmount: [argsData.purchaseAmount, [Validators.required]],
        purchaseDate: [new Date(argsData.purchaseDate), [Validators.required]],
        registrationNumber: [argsData.registrationNumber, [Validators.required]],
        vehicleMakeId: [argsData.vehicleMakeId, [Validators.required]],
        vehicleModelId: [argsData.vehicleModelId, [Validators.required]]
      });

      return viewVehicleForm;
    }
  }

  public getErrorMessage(controlName: string): string {
      const control = this.viewVehicleForm.get(controlName);
      if (control?.errors) {
          if (control.errors["required"]) {
              return 'This field is required';
          }
      }

      return '';
  }

  public getVehicleMakesSelectList(): void {

    this.viewVehicleHttpService.getVehicleMakeSelectList()
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

    this.viewVehicleHttpService.getVehicleModelSelectList(vehicleMakeId)
    .subscribe({
        next: (successResponse) => {
            this.vehicleModelOptions = successResponse as unknown as SelectItem[];
        },
        error: (errorResponse) => {
            this.vehicleModelOptions = [];
        }
    });
}

  public onClickBack(): void {
    this.router.navigate(['/features/fleet/vehicle']);
  }

  public onClickSave(): void {

    const isValid = this.viewVehicleForm.valid;
    if(isValid){

      const vehicle: IViewVehicleDto = this.viewVehicleForm.value as IViewVehicleDto;
            
      this.isBusy = true;
      this.viewVehicleHttpService.update(this.vehicleId, vehicle)
        .subscribe({
          next: (successResponse) => {
            
            this.isBusy = false;

            // show success
            this.messageService.add({
                detail: 'Record updated successfuly',
                summary: 'Success',
                severity: 'success'
            });
          }
        });

    } else {
            
      this.isBusy = false;

      // show error
      this.messageService.add({
          detail: 'Fix validation errors',
          summary: 'Form invalid',
          severity: 'error'
      });
    }
  }

  public onVehicleMakeChange(event: DropdownChangeEvent): void {

      if(event.value == 0){

          this.viewVehicleForm.get('vehicleModelId')?.disable();
          this.vehicleModelOptions = [];
      } else {

          this.viewVehicleForm.get('vehicleModelId')?.enable();
          this.getVehicleModelsSelectList(event.value);
      }
  }
}
