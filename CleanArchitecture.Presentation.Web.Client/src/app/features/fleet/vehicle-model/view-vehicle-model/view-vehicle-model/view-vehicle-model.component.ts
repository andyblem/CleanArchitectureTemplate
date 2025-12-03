import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ViewVehicleModelHttpService } from './services/view-vehicle-model-http.service';
import { switchMap } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IViewVehicleModelDto } from './dtos/i-view-vehicle-model-dto';
import { MessageService, SelectItem } from 'primeng/api';

@Component({
  selector: 'app-view-vehicle-model',
  templateUrl: './view-vehicle-model.component.html',
  styleUrls: ['./view-vehicle-model.component.scss'],
  providers: [MessageService]
})
export class ViewVehicleModelComponent implements OnInit {
  
  isBusy: boolean;
  isInitializing: boolean;

  vehicleModelId: number;

  viewVehicleModelForm: FormGroup;
  
  vehicleMakeOptions: SelectItem[];
  vehicleTypeOptions: SelectItem[];
  

  constructor(private formBuilder: FormBuilder,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private router: Router,
    private viewVehicleModelHttpService: ViewVehicleModelHttpService
  ) {
    this.isBusy = false;
    this.isInitializing = false;
    this.vehicleModelId = 0;

    this.vehicleMakeOptions = [];
    this.vehicleTypeOptions = [];

    this.viewVehicleModelForm = this.generateViewVehicleModelForm();
  }

  ngOnInit(): void {

    this.isInitializing = true;
    Promise.all([this.getVehicleMakesSelectList(), this.getVehicleTypesSelectList()])
      .then(result => {
          
          this.isInitializing = false;
          this.route.paramMap.pipe(
            switchMap(params => {
      
              this.vehicleModelId = parseInt(params.get('id')!, 10);
              return this.viewVehicleModelHttpService.get(this.vehicleModelId);
            })
          )
          .subscribe({
            next: (successResponse) => {
              this.viewVehicleModelForm = this.generateViewVehicleModelForm(successResponse as unknown as IViewVehicleModelDto);
              this.isInitializing = false;
            },
            error: (errorResponse) => {
              this.isInitializing = false;
            }
          });
      });
  }

  public generateViewVehicleModelForm(): FormGroup;
  public generateViewVehicleModelForm(vehicleModel: IViewVehicleModelDto): FormGroup;
  public generateViewVehicleModelForm(...args: any[]): FormGroup {

    if(args.length == 0){

      const viewVehicleModelForm = this.formBuilder.group({
        id: ['', [Validators.required]],
        name: ['', [Validators.required]],
        vehicleMakeId: ['', [Validators.required]],
        vehicleTypeId: ['', [Validators.required]],
      });

      return viewVehicleModelForm;

    } else {

      const argsData = args[0] as unknown as IViewVehicleModelDto;
      const viewVehicleModelForm = this.formBuilder.group({
        id: [argsData.id, [Validators.required]],
        name: [argsData.name, [Validators.required]],
        vehicleMakeId: [argsData.vehicleMakeId, [Validators.required]],
        vehicleTypeId: [argsData.vehicleTypeId, [Validators.required]],
      });

      return viewVehicleModelForm;
    }
  }

  public getErrorMessage(controlName: string): string {
      const control = this.viewVehicleModelForm.get(controlName);
      if (control?.errors) {
          if (control.errors["required"]) {
              return 'This field is required';
          }
      }

      return '';
  }   
   
  public getVehicleMakesSelectList(): void {

        this.viewVehicleModelHttpService.getVehicleMakeSelectList()
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

        this.viewVehicleModelHttpService.getVehicleTypeSelectList()
        .subscribe({
            next: (successResponse) => {
                this.vehicleTypeOptions = successResponse as unknown as SelectItem[];
            },
            error: (errorResponse) => {
                this.vehicleTypeOptions = [];
            }
        });
    }

  public onClickBack(): void {
    this.router.navigate(['/features/fleet/vehicle-model']);
  }

  public onClickSave(): void {

    const isValid = this.viewVehicleModelForm.valid;
    if(isValid){

      const vehicleModel: IViewVehicleModelDto = this.viewVehicleModelForm.value as IViewVehicleModelDto;
            
      this.isBusy = true;
      this.viewVehicleModelHttpService.update(this.vehicleModelId, vehicleModel)
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
}
