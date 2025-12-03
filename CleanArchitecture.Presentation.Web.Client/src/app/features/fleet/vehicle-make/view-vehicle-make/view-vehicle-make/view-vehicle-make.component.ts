import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ViewVehicleMakeHttpService } from './services/view-vehicle-make-http.service';
import { switchMap } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IViewVehicleMakeDto } from './dtos/i-view-vehicle-make-dto';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-view-vehicle-make',
  templateUrl: './view-vehicle-make.component.html',
  styleUrls: ['./view-vehicle-make.component.scss'],
  providers: [MessageService]
})
export class ViewVehicleMakeComponent implements OnInit {
  
  isBusy: boolean;
  isInitializing: boolean;

  vehicleMakeId: number;

  viewVehicleMakeForm: FormGroup;
  

  constructor(private formBuilder: FormBuilder,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private router: Router,
    private viewVehicleMakeHttpService: ViewVehicleMakeHttpService
  ) {
    this.isBusy = false;
    this.isInitializing = false;
    this.vehicleMakeId = 0;

    this.viewVehicleMakeForm = this.generateViewVehicleMakeForm();
  }

  ngOnInit(): void {

    this.isInitializing = true;
    this.route.paramMap.pipe(
      switchMap(params => {

        this.vehicleMakeId = parseInt(params.get('id')!, 10);
        return this.viewVehicleMakeHttpService.get(this.vehicleMakeId);
      })
    )
    .subscribe({
      next: (successResponse) => {
        this.viewVehicleMakeForm = this.generateViewVehicleMakeForm(successResponse as unknown as IViewVehicleMakeDto);
        this.isInitializing = false;
      },
      error: (errorResponse) => {
        this.isInitializing = false;
      }
    });
  }

  public generateViewVehicleMakeForm(): FormGroup;
  public generateViewVehicleMakeForm(vehicleMake: IViewVehicleMakeDto): FormGroup;
  public generateViewVehicleMakeForm(...args: any[]): FormGroup {

    if(args.length == 0){

      const viewVehicleMakeForm = this.formBuilder.group({
        id: ['', [Validators.required]],
        name: ['', [Validators.required]]
      });

      return viewVehicleMakeForm;

    } else {

      const argsData = args[0] as unknown as IViewVehicleMakeDto;
      const viewVehicleMakeForm = this.formBuilder.group({
        id: [argsData.id, [Validators.required]],
        name: [argsData.name, [Validators.required]]
      });

      return viewVehicleMakeForm;
    }
  }

  public getErrorMessage(controlName: string): string {
      const control = this.viewVehicleMakeForm.get(controlName);
      if (control?.errors) {
          if (control.errors["required"]) {
              return 'This field is required';
          }
      }

      return '';
  }

  public onClickBack(): void {
    this.router.navigate(['/features/fleet/vehicle-make']);
  }

  public onClickSave(): void {

    const isValid = this.viewVehicleMakeForm.valid;
    if(isValid){

      const vehicleMake: IViewVehicleMakeDto = this.viewVehicleMakeForm.value as IViewVehicleMakeDto;
            
      this.isBusy = true;
      this.viewVehicleMakeHttpService.update(this.vehicleMakeId, vehicleMake)
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
