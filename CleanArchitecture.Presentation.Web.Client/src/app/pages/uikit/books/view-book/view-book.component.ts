import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ViewBookHttpService } from './services/view-book-http.service';
import { switchMap } from 'rxjs';
import { IViewBookDto } from './dtos/i-view-book-dto';
import { CommonModule } from '@angular/common';
import { EntityMenuComponentModule } from '@/@shared/components/shared/entity-menu-component-module/entity-menu-component.module';
import { PageLoadingComponentModule } from '@/@shared/components/shared/page-loading-component-module/page-loading-component.module';
import { PageProcessingComponentModule } from '@/@shared/components/shared/page-processing-component-module/page-processing-component.module';
import { ButtonModule } from 'primeng/button';
import { EditorModule } from 'primeng/editor';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { InputNumberModule } from 'primeng/inputnumber';

@Component({
    selector: 'app-view-book',
    imports: [
      CommonModule,
      ReactiveFormsModule,
      
      EntityMenuComponentModule,
      PageLoadingComponentModule,
      PageProcessingComponentModule,
      
      ButtonModule,
      EditorModule,
      InputNumberModule,
      InputTextModule,
      ToastModule
    ],
    providers: [MessageService],
    standalone: true,
    template: `
      @if (isInitializing == true) {
        <app-page-loading></app-page-loading>
      } @else {
        <!--entity-menu-->
        <app-entity-menu>
          <ng-template #brand>
            Book Details
          </ng-template>
          <ng-template #items>
            <p-button label="Back"
              icon="pi pi-angle-left"
              pTooltip="Back to list"
              tooltipPosition="bottom"
              [text]="true"
              (onClick)="onClickBack()" />
          </ng-template>
        </app-entity-menu>

        <div class="pt-3"></div>
        
        <!--content-->
        <div class="card">
          <form [formGroup]="viewBookForm">

            <input pInputText
              id="id"
              formControlName="id"
              type="hidden" />

            <div class="flex flex-wrap gap-6">
                <div class="flex flex-col grow basis-0 gap-2">
                    <label for="title">Title</label>
                    <input pInputText
                      id="title"
                      formControlName="title"
                      type="text"
                      class="w-full"
                      [invalid]="viewBookForm.get('title')?.invalid && viewBookForm.get('title')?.touched" />
                    @if (viewBookForm.get('title')?.invalid && viewBookForm.get('title')?.touched) {
                      <small class="text-red-700">
                        {{ getErrorMessage('title') }}
                      </small>
                    }
                </div>

                <div class="flex flex-col grow basis-0 gap-2">
                    <label htmlFor="isbn">ISBN</label>
                    <input pInputText
                      id="isbn"
                      placeholder="ISBN"
                      formControlName="isbn"
                      type="text"
                      class="w-full"
                      [invalid]="viewBookForm.get('isbn')?.invalid && viewBookForm.get('isbn')?.touched" />
                    @if (viewBookForm.get('isbn')?.invalid && viewBookForm.get('isbn')?.touched) {
                      <small class="text-red-700">
                        {{ getErrorMessage('isbn') }}
                      </small>
                    }
                </div>

                <div class="flex flex-col grow basis-0 gap-2">
                    <label class="font-medium" htmlFor="price">Price</label>
                    <p-inputnumber 
                      formControlName="price" 
                      id="price" 
                      mode="currency" 
                      currency="USD" 
                      locale="en-US"
                      class="w-full"
                      [invalid]="viewBookForm.get('price')?.invalid && viewBookForm.get('price')?.touched" />
                    @if (viewBookForm.get('price')?.invalid && viewBookForm.get('price')?.touched) {
                      <small class="text-red-700">
                        {{ getErrorMessage('price') }}
                      </small>
                    }
                </div>
            </div>

            <div class="flex flex-col gap-2">
              <label class="font-medium" htmlFor="summary">Summary</label>
              <p-editor id="summary"
                formControlName="summary"
                [invalid]="viewBookForm.get('summary')?.invalid && viewBookForm.get('summary')?.touched"
                [style]="{ height: '100px' }">
              </p-editor>
              @if (viewBookForm.get('summary')?.invalid && viewBookForm.get('summary')?.touched) {
                <small class="text-red-700">
                  {{ getErrorMessage('summary') }}
                </small>
              }
            </div>
          </form>

          <div class="pt-5"></div>
                
          <div class="flex justify-content-end flex-wrap">
            <p-button label="Update"
              icon="pi pi-save"
              pTooltip="Update record"
              tooltipPosition="bottom"
              (onClick)="onClickSave()" />
          </div>
        </div>
      }

      <!--page processing-->
      @if (isBusy) {
        <app-page-processing></app-page-processing>
      }

      <!--toast-->
      <p-toast></p-toast>
    `,
})
export class ViewBookComponent implements OnInit {
  
  isBusy: boolean;
  isInitializing: boolean;

  bookId: number;

  viewBookForm: FormGroup;
  

  constructor(private formBuilder: FormBuilder,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private router: Router,
    private viewBookHttpService: ViewBookHttpService
  ) {
    this.isBusy = false;
    this.isInitializing = false;
    this.bookId = 0;

    this.viewBookForm = this.generateViewBookForm();
  }

  ngOnInit(): void {

    this.isInitializing = true;
    this.route.paramMap.pipe(
      switchMap(params => {

        this.bookId = parseInt(params.get('id')!, 10);
        return this.viewBookHttpService.get(this.bookId);
      })
    )
    .subscribe({
      next: (successResponse) => {
        this.viewBookForm = this.generateViewBookForm((successResponse as any).data as IViewBookDto);
        this.isInitializing = false;
      },
      error: (errorResponse) => {
        this.isInitializing = false;
      }
    });
  }

  public generateViewBookForm(): FormGroup;
  public generateViewBookForm(book: IViewBookDto): FormGroup;
  public generateViewBookForm(...args: any[]): FormGroup {

    if(args.length == 0){

      const viewBookForm = this.formBuilder.group({
        id: ['', [Validators.required]],
        price: ['', [Validators.required]],

        isbn: ['', [Validators.required]],
        summary: ['', [Validators.required]],
        title: ['', [Validators.required]]
      });

      return viewBookForm;

    } else {

      const argsData = args[0] as unknown as IViewBookDto;
      const viewBookForm = this.formBuilder.group({
        id: [argsData.id, [Validators.required]],
        price: [argsData.price, [Validators.required]],

        isbn: [argsData.isbn, [Validators.required]],
        summary: [argsData.summary, [Validators.required]],
        title: [argsData.title, [Validators.required]]
      });

      return viewBookForm;
    }
  }

  public getErrorMessage(controlName: string): string {
      const control = this.viewBookForm.get(controlName);
      if (control?.errors) {
          if (control.errors["required"]) {
              return 'This field is required';
          }
      }

      return '';
  }

  public onClickBack(): void {
    this.router.navigate(['/uikit/books']);
  }

  public onClickSave(): void {

    const isValid = this.viewBookForm.valid;
    if(isValid){

      const book: IViewBookDto = this.viewBookForm.value as IViewBookDto;
            
      this.isBusy = true;
      this.viewBookHttpService.update(this.bookId, book)
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
