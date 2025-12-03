import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ViewCostTypeHttpService } from './services/view-cost-type-http.service';
import { switchMap } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IViewCostTypeDto } from './dtos/i-view-cost-type-dto';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-view-cost-type',
  templateUrl: './view-cost-type.component.html',
  styleUrls: ['./view-cost-type.component.scss'],
  providers: [MessageService]
})
export class ViewCostTypeComponent implements OnInit {
  
  isBusy: boolean;
  isInitializing: boolean;

  incomeTypeId: number;

  viewCostTypeForm: FormGroup;
  

  constructor(private formBuilder: FormBuilder,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private router: Router,
    private viewCostTypeHttpService: ViewCostTypeHttpService
  ) {
    this.isBusy = false;
    this.isInitializing = false;
    this.incomeTypeId = 0;

    this.viewCostTypeForm = this.generateViewCostTypeForm();
  }

  ngOnInit(): void {

    this.isInitializing = true;
    this.route.paramMap.pipe(
      switchMap(params => {

        this.incomeTypeId = parseInt(params.get('id')!, 10);
        return this.viewCostTypeHttpService.get(this.incomeTypeId);
      })
    )
    .subscribe({
      next: (successResponse) => {
        this.viewCostTypeForm = this.generateViewCostTypeForm(successResponse as unknown as IViewCostTypeDto);
        this.isInitializing = false;
      },
      error: (errorResponse) => {
        this.isInitializing = false;
      }
    });
  }

  public generateViewCostTypeForm(): FormGroup;
  public generateViewCostTypeForm(incomeType: IViewCostTypeDto): FormGroup;
  public generateViewCostTypeForm(...args: any[]): FormGroup {

    if(args.length == 0){

      const viewCostTypeForm = this.formBuilder.group({
        id: ['', [Validators.required]],
        name: ['', [Validators.required]]
      });

      return viewCostTypeForm;

    } else {

      const argsData = args[0] as unknown as IViewCostTypeDto;
      const viewCostTypeForm = this.formBuilder.group({
        id: [argsData.id, [Validators.required]],
        name: [argsData.name, [Validators.required]]
      });

      return viewCostTypeForm;
    }
  }

  public getErrorMessage(controlName: string): string {
      const control = this.viewCostTypeForm.get(controlName);
      if (control?.errors) {
          if (control.errors["required"]) {
              return 'This field is required';
          }
      }

      return '';
  }

  public onClickBack(): void {
    this.router.navigate(['/features/setup/cost-type']);
  }

  public onClickSave(): void {

    const isValid = this.viewCostTypeForm.valid;
    if(isValid){

      const incomeType: IViewCostTypeDto = this.viewCostTypeForm.value as IViewCostTypeDto;
            
      this.isBusy = true;
      this.viewCostTypeHttpService.update(this.incomeTypeId, incomeType)
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
