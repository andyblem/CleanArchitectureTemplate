import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ViewCostHttpService } from './services/view-cost-http.service';
import { switchMap } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IViewCostDto } from './dtos/i-view-cost-dto';
import { MessageService, SelectItem } from 'primeng/api';

@Component({
  selector: 'app-view-cost',
  templateUrl: './view-cost.component.html',
  styleUrls: ['./view-cost.component.scss'],
  providers: [MessageService]
})
export class ViewCostComponent implements OnInit {
  
  isBusy: boolean;
  isInitializing: boolean;

  costId: number;

  viewCostForm: FormGroup;

  costTypeOptions: SelectItem[];
  vehicleOptions: SelectItem[];
  

  constructor(private formBuilder: FormBuilder,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private router: Router,
    private viewCostHttpService: ViewCostHttpService
  ) {
    this.isBusy = false;
    this.isInitializing = false;
    this.costId = 0;

    this.costTypeOptions = [];
    this.vehicleOptions = [];

    this.viewCostForm = this.generateViewCostForm();
  }

  ngOnInit(): void {

    this.isInitializing = true;
    Promise.all([
      this.getCostTypeSelectList(),
      this.getVehicleSelectList()
    ])
    .then(result => {

      this.isBusy = true;
      this.isInitializing = false;

      this.route.paramMap.pipe(
        switchMap(params => {

          this.costId = parseInt(params.get('id')!, 10);
          return this.viewCostHttpService.get(this.costId);
        })
      )
      .subscribe({
        next: (successResponse) => {
          this.viewCostForm = this.generateViewCostForm(successResponse as unknown as IViewCostDto);
          this.isBusy = false;
        },
        error: (errorResponse) => {
          this.isBusy = false;
        }
      });
    });
  }

  public generateViewCostForm(): FormGroup;
  public generateViewCostForm(vehicleType: IViewCostDto): FormGroup;
  public generateViewCostForm(...args: any[]): FormGroup {

    if(args.length == 0){

      const viewCostForm = this.formBuilder.group({
        id: ['', [Validators.required]],
        amount: ['', [Validators.required]],
        amountPaid: ['', []],
        costTypeId: ['', [Validators.required]],
        vehicleId: ['', [Validators.required]],
        date: ['', [Validators.required]]
      });

      return viewCostForm;

    } else {

      const argsData = args[0] as unknown as IViewCostDto;
      const viewCostForm = this.formBuilder.group({
        id: [argsData.id, [Validators.required]],
        amount: [argsData.amount, [Validators.required]],
        amountPaid: [argsData.amountPaid, []],
        costTypeId: [argsData.costTypeId, [Validators.required]],
        vehicleId: [argsData.vehicleId, [Validators.required]],
        date: [new Date(argsData.date), [Validators.required]]
      });

      return viewCostForm;
    }
  }

  public getErrorMessage(controlName: string): string {
      const control = this.viewCostForm.get(controlName);
      if (control?.errors) {
          if (control.errors["required"]) {
              return 'This field is required';
          }
      }

      return '';
  }  
  
  public getCostTypeSelectList(): void {
  
        this.viewCostHttpService.getCostTypeSelectList()
        .subscribe({
            next: (successResponse) => {
                this.costTypeOptions = successResponse as unknown as SelectItem[];
            },
            error: (errorResponse) => {
                this.costTypeOptions = [];
            }
        });
    }
  
    public getVehicleSelectList(): void {
  
        this.viewCostHttpService.getVehicleSelectList()
        .subscribe({
            next: (successResponse) => {
                this.vehicleOptions = successResponse as unknown as SelectItem[];
            },
            error: (errorResponse) => {
                this.vehicleOptions = [];
            }
        });
    }

  public onClickBack(): void {
    this.router.navigate(['/features/financial-resource/cost']);
  }

  public onClickSave(): void {

    const isValid = this.viewCostForm.valid;
    if(isValid){

      const vehicleType: IViewCostDto = this.viewCostForm.value as IViewCostDto;
            
      this.isBusy = true;
      this.viewCostHttpService.update(this.costId, vehicleType)
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
