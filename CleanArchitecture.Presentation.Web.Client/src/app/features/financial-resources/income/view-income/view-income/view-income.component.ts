import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ViewIncomeHttpService } from './services/view-income-http.service';
import { switchMap } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IViewIncomeDto } from './dtos/i-view-income-dto';
import { MessageService, SelectItem } from 'primeng/api';

@Component({
  selector: 'app-view-income',
  templateUrl: './view-income.component.html',
  styleUrls: ['./view-income.component.scss'],
  providers: [MessageService]
})
export class ViewIncomeComponent implements OnInit {
  
  isBusy: boolean;
  isInitializing: boolean;

  vehicleTypeId: number;

  viewIncomeForm: FormGroup;

  icomeTypeOptions: SelectItem[];
  vehicleOptions: SelectItem[];
  

  constructor(private formBuilder: FormBuilder,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private router: Router,
    private viewIncomeHttpService: ViewIncomeHttpService
  ) {
    this.isBusy = false;
    this.isInitializing = false;
    this.vehicleTypeId = 0;

    this.icomeTypeOptions = [];
    this.vehicleOptions = [];

    this.viewIncomeForm = this.generateViewIncomeForm();
  }

  ngOnInit(): void {

    this.isInitializing = true;

    Promise.all([
      this.getIncomeTypeSelectList(),
      this.getVehicleSelectList()
    ])
    .then(result => {

      this.isBusy = true;
      this.isInitializing = false;
      
      this.route.paramMap.pipe(
        switchMap(params => {

          this.vehicleTypeId = parseInt(params.get('id')!, 10);
          return this.viewIncomeHttpService.get(this.vehicleTypeId);
        })
      )
      .subscribe({
        next: (successResponse) => {
          this.viewIncomeForm = this.generateViewIncomeForm(successResponse as unknown as IViewIncomeDto);
          this.isBusy = false;
        },
        error: (errorResponse) => {
          this.isBusy = false;
        }
      });
    });
  }

  public generateViewIncomeForm(): FormGroup;
  public generateViewIncomeForm(vehicleType: IViewIncomeDto): FormGroup;
  public generateViewIncomeForm(...args: any[]): FormGroup {

    if(args.length == 0){

      const viewIncomeForm = this.formBuilder.group({
        amount: ['', [Validators.required]],
        id: ['', [Validators.required]],
        incomeTypeId: ['', [Validators.required]],
        vehicleId: ['', [Validators.required]],
        date: ['', [Validators.required]]
      });

      return viewIncomeForm;

    } else {

      const argsData = args[0] as unknown as IViewIncomeDto;
      const viewIncomeForm = this.formBuilder.group({
        amount: [argsData.amount, [Validators.required]],
        id: [argsData.id, [Validators.required]],
        incomeTypeId: [argsData.incomeTypeId, [Validators.required]],
        vehicleId: [argsData.vehicleId, [Validators.required]],
        date: [new Date(argsData.date), [Validators.required]]
      });

      return viewIncomeForm;
    }
  }

  public getErrorMessage(controlName: string): string {
      const control = this.viewIncomeForm.get(controlName);
      if (control?.errors) {
          if (control.errors["required"]) {
              return 'This field is required';
          }
      }

      return '';
  }

  public onClickBack(): void {
    this.router.navigate(['/features/financial-resource/income']);
  }

  public onClickSave(): void {

    const isValid = this.viewIncomeForm.valid;
    if(isValid){

      const vehicleType: IViewIncomeDto = this.viewIncomeForm.value as IViewIncomeDto;
            
      this.isBusy = true;
      this.viewIncomeHttpService.update(this.vehicleTypeId, vehicleType)
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

  public getIncomeTypeSelectList(): void {

      this.viewIncomeHttpService.getIncomeTypeSelectList()
      .subscribe({
          next: (successResponse) => {
              this.icomeTypeOptions = successResponse as unknown as SelectItem[];
          },
          error: (errorResponse) => {
              this.icomeTypeOptions = [];
          }
      });
  }

  public getVehicleSelectList(): void {

      this.viewIncomeHttpService.getVehicleSelectList()
      .subscribe({
          next: (successResponse) => {
              this.vehicleOptions = successResponse as unknown as SelectItem[];
          },
          error: (errorResponse) => {
              this.vehicleOptions = [];
          }
      });
  }
}
