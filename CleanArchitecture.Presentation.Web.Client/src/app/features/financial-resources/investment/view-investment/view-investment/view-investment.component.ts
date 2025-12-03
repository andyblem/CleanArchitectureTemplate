import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ViewInvestmentHttpService } from './services/view-investment-http.service';
import { switchMap } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IViewInvestmentDto } from './dtos/i-view-investment-dto';
import { MessageService, SelectItem } from 'primeng/api';

@Component({
  selector: 'app-view-investment',
  templateUrl: './view-investment.component.html',
  styleUrls: ['./view-investment.component.scss'],
  providers: [MessageService]
})
export class ViewInvestmentComponent implements OnInit {
  
  isBusy: boolean;
  isInitializing: boolean;

  investmentId: number;

  viewInvestmentForm: FormGroup;

  icomeTypeOptions: SelectItem[];
  

  constructor(private formBuilder: FormBuilder,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private router: Router,
    private viewInvestmentHttpService: ViewInvestmentHttpService
  ) {
    this.isBusy = false;
    this.isInitializing = false;
    this.investmentId = 0;

    this.icomeTypeOptions = [];

    this.viewInvestmentForm = this.generateViewInvestmentForm();
  }

  ngOnInit(): void {

    this.isInitializing = true;

    Promise.all([
      this.getInvestmentTypeSelectList()
    ])
    .then(result => {

      this.isBusy = true;
      this.isInitializing = false;
      
      this.route.paramMap.pipe(
        switchMap(params => {

          this.investmentId = parseInt(params.get('id')!, 10);
          return this.viewInvestmentHttpService.get(this.investmentId);
        })
      )
      .subscribe({
        next: (successResponse) => {
          this.viewInvestmentForm = this.generateViewInvestmentForm(successResponse as unknown as IViewInvestmentDto);
          this.isBusy = false;
        },
        error: (errorResponse) => {
          this.isBusy = false;
        }
      });
    });
  }

  public generateViewInvestmentForm(): FormGroup;
  public generateViewInvestmentForm(investment: IViewInvestmentDto): FormGroup;
  public generateViewInvestmentForm(...args: any[]): FormGroup {

    if(args.length == 0){

      const viewInvestmentForm = this.formBuilder.group({
        amount: ['', [Validators.required]],
        id: ['', [Validators.required]],
        investmentTypeId: ['', [Validators.required]],
        date: ['', [Validators.required]]
      });

      return viewInvestmentForm;

    } else {

      const argsData = args[0] as unknown as IViewInvestmentDto;
      const viewInvestmentForm = this.formBuilder.group({
        amount: [argsData.amount, [Validators.required]],
        id: [argsData.id, [Validators.required]],
        investmentTypeId: [argsData.investmentTypeId, [Validators.required]],
        date: [new Date(argsData.date), [Validators.required]]
      });

      return viewInvestmentForm;
    }
  }

  public getErrorMessage(controlName: string): string {
      const control = this.viewInvestmentForm.get(controlName);
      if (control?.errors) {
          if (control.errors["required"]) {
              return 'This field is required';
          }
      }

      return '';
  }

  public onClickBack(): void {
    this.router.navigate(['/features/financial-resource/investment']);
  }

  public onClickSave(): void {

    const isValid = this.viewInvestmentForm.valid;
    if(isValid){

      const investment: IViewInvestmentDto = this.viewInvestmentForm.value as IViewInvestmentDto;
            
      this.isBusy = true;
      this.viewInvestmentHttpService.update(this.investmentId, investment)
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

  public getInvestmentTypeSelectList(): void {

      this.viewInvestmentHttpService.getInvestmentTypeSelectList()
      .subscribe({
          next: (successResponse) => {
              this.icomeTypeOptions = successResponse as unknown as SelectItem[];
          },
          error: (errorResponse) => {
              this.icomeTypeOptions = [];
          }
      });
  }
}
