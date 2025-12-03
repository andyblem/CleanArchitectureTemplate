import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ViewVehicleTypeHttpService } from './services/view-vehicle-type-http.service';
import { switchMap } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IViewVehicleTypeDto } from './dtos/i-view-vehicle-type-dto';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-view-vehicle-type',
  templateUrl: './view-vehicle-type.component.html',
  styleUrls: ['./view-vehicle-type.component.scss'],
  providers: [MessageService]
})
export class ViewVehicleTypeComponent implements OnInit {
  
  isBusy: boolean;
  isInitializing: boolean;

  vehicleTypeId: number;

  viewVehicleTypeForm: FormGroup;
  

  constructor(private formBuilder: FormBuilder,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private router: Router,
    private viewVehicleTypeHttpService: ViewVehicleTypeHttpService
  ) {
    this.isBusy = false;
    this.isInitializing = false;
    this.vehicleTypeId = 0;

    this.viewVehicleTypeForm = this.generateViewVehicleTypeForm();
  }

  ngOnInit(): void {

    this.isInitializing = true;
    this.route.paramMap.pipe(
      switchMap(params => {

        this.vehicleTypeId = parseInt(params.get('id')!, 10);
        return this.viewVehicleTypeHttpService.get(this.vehicleTypeId);
      })
    )
    .subscribe({
      next: (successResponse) => {
        this.viewVehicleTypeForm = this.generateViewVehicleTypeForm(successResponse as unknown as IViewVehicleTypeDto);
        this.isInitializing = false;
      },
      error: (errorResponse) => {
        this.isInitializing = false;
      }
    });
  }

  public generateViewVehicleTypeForm(): FormGroup;
  public generateViewVehicleTypeForm(vehicleType: IViewVehicleTypeDto): FormGroup;
  public generateViewVehicleTypeForm(...args: any[]): FormGroup {

    if(args.length == 0){

      const viewVehicleTypeForm = this.formBuilder.group({
        id: ['', [Validators.required]],
        name: ['', [Validators.required]]
      });

      return viewVehicleTypeForm;

    } else {

      const argsData = args[0] as unknown as IViewVehicleTypeDto;
      const viewVehicleTypeForm = this.formBuilder.group({
        id: [argsData.id, [Validators.required]],
        name: [argsData.name, [Validators.required]]
      });

      return viewVehicleTypeForm;
    }
  }

  public getErrorMessage(controlName: string): string {
      const control = this.viewVehicleTypeForm.get(controlName);
      if (control?.errors) {
          if (control.errors["required"]) {
              return 'This field is required';
          }
      }

      return '';
  }

  public onClickBack(): void {
    this.router.navigate(['/features/fleet/vehicle-type']);
  }

  public onClickSave(): void {

    const isValid = this.viewVehicleTypeForm.valid;
    if(isValid){

      const vehicleType: IViewVehicleTypeDto = this.viewVehicleTypeForm.value as IViewVehicleTypeDto;
            
      this.isBusy = true;
      this.viewVehicleTypeHttpService.update(this.vehicleTypeId, vehicleType)
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
