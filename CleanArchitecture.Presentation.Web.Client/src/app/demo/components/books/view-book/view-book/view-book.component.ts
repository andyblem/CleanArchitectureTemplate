import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ViewBookHttpService } from './services/view-book-http.service';
import { switchMap } from 'rxjs';
import { IViewBookDto } from './dtos/i-view-book-dto';

@Component({
    selector: 'app-view-book',
    templateUrl: './view-book.component.html',
    styleUrls: ['./view-book.component.scss'],
    providers: [MessageService],
    standalone: false
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
    this.router.navigate(['/features/books']);
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
