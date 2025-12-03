import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ViewInvestmentTypeHttpService } from './services/view-investment-type-http.service';
import { switchMap } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IViewInvestmentTypeDto } from './dtos/i-view-investment-type-dto';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-view-investment-type',
  templateUrl: './view-investment-type.component.html',
  styleUrls: ['./view-investment-type.component.scss'],
  providers: [MessageService]
})
export class ViewInvestmentTypeComponent implements OnInit {
  
  isBusy: boolean;
  isInitializing: boolean;

  incomeTypeId: number;

  viewInvestmentTypeForm: FormGroup;
  

  constructor(private formBuilder: FormBuilder,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private router: Router,
    private viewInvestmentTypeHttpService: ViewInvestmentTypeHttpService
  ) {
    this.isBusy = false;
    this.isInitializing = false;
    this.incomeTypeId = 0;

    this.viewInvestmentTypeForm = this.generateViewInvestmentTypeForm();
  }

  ngOnInit(): void {

    this.isInitializing = true;
    this.route.paramMap.pipe(
      switchMap(params => {

        this.incomeTypeId = parseInt(params.get('id')!, 10);
        return this.viewInvestmentTypeHttpService.get(this.incomeTypeId);
      })
    )
    .subscribe({
      next: (successResponse) => {
        this.viewInvestmentTypeForm = this.generateViewInvestmentTypeForm(successResponse as unknown as IViewInvestmentTypeDto);
        this.isInitializing = false;
      },
      error: (errorResponse) => {
        this.isInitializing = false;
      }
    });
  }

  public generateViewInvestmentTypeForm(): FormGroup;
  public generateViewInvestmentTypeForm(incomeType: IViewInvestmentTypeDto): FormGroup;
  public generateViewInvestmentTypeForm(...args: any[]): FormGroup {

    if(args.length == 0){

      const viewInvestmentTypeForm = this.formBuilder.group({
        id: ['', [Validators.required]],
        name: ['', [Validators.required]]
      });

      return viewInvestmentTypeForm;

    } else {

      const argsData = args[0] as unknown as IViewInvestmentTypeDto;
      const viewInvestmentTypeForm = this.formBuilder.group({
        id: [argsData.id, [Validators.required]],
        name: [argsData.name, [Validators.required]]
      });

      return viewInvestmentTypeForm;
    }
  }

  public getErrorMessage(controlName: string): string {
      const control = this.viewInvestmentTypeForm.get(controlName);
      if (control?.errors) {
          if (control.errors["required"]) {
              return 'This field is required';
          }
      }

      return '';
  }

  public onClickBack(): void {
    this.router.navigate(['/features/setup/investment-type']);
  }

  public onClickSave(): void {

    const isValid = this.viewInvestmentTypeForm.valid;
    if(isValid){

      const incomeType: IViewInvestmentTypeDto = this.viewInvestmentTypeForm.value as IViewInvestmentTypeDto;
            
      this.isBusy = true;
      this.viewInvestmentTypeHttpService.update(this.incomeTypeId, incomeType)
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
