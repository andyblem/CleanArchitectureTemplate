import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ViewIncomeTypeHttpService } from './services/view-income-type-http.service';
import { switchMap } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IViewIncomeTypeDto } from './dtos/i-view-income-type-dto';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-view-income-type',
  templateUrl: './view-income-type.component.html',
  styleUrls: ['./view-income-type.component.scss'],
  providers: [MessageService]
})
export class ViewIncomeTypeComponent implements OnInit {
  
  isBusy: boolean;
  isInitializing: boolean;

  incomeTypeId: number;

  viewIncomeTypeForm: FormGroup;
  

  constructor(private formBuilder: FormBuilder,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private router: Router,
    private viewIncomeTypeHttpService: ViewIncomeTypeHttpService
  ) {
    this.isBusy = false;
    this.isInitializing = false;
    this.incomeTypeId = 0;

    this.viewIncomeTypeForm = this.generateViewIncomeTypeForm();
  }

  ngOnInit(): void {

    this.isInitializing = true;
    this.route.paramMap.pipe(
      switchMap(params => {

        this.incomeTypeId = parseInt(params.get('id')!, 10);
        return this.viewIncomeTypeHttpService.get(this.incomeTypeId);
      })
    )
    .subscribe({
      next: (successResponse) => {
        this.viewIncomeTypeForm = this.generateViewIncomeTypeForm(successResponse as unknown as IViewIncomeTypeDto);
        this.isInitializing = false;
      },
      error: (errorResponse) => {
        this.isInitializing = false;
      }
    });
  }

  public generateViewIncomeTypeForm(): FormGroup;
  public generateViewIncomeTypeForm(incomeType: IViewIncomeTypeDto): FormGroup;
  public generateViewIncomeTypeForm(...args: any[]): FormGroup {

    if(args.length == 0){

      const viewIncomeTypeForm = this.formBuilder.group({
        id: ['', [Validators.required]],
        name: ['', [Validators.required]]
      });

      return viewIncomeTypeForm;

    } else {

      const argsData = args[0] as unknown as IViewIncomeTypeDto;
      const viewIncomeTypeForm = this.formBuilder.group({
        id: [argsData.id, [Validators.required]],
        name: [argsData.name, [Validators.required]]
      });

      return viewIncomeTypeForm;
    }
  }

  public getErrorMessage(controlName: string): string {
      const control = this.viewIncomeTypeForm.get(controlName);
      if (control?.errors) {
          if (control.errors["required"]) {
              return 'This field is required';
          }
      }

      return '';
  }

  public onClickBack(): void {
    this.router.navigate(['/features/setup/income-type']);
  }

  public onClickSave(): void {

    const isValid = this.viewIncomeTypeForm.valid;
    if(isValid){

      const incomeType: IViewIncomeTypeDto = this.viewIncomeTypeForm.value as IViewIncomeTypeDto;
            
      this.isBusy = true;
      this.viewIncomeTypeHttpService.update(this.incomeTypeId, incomeType)
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
