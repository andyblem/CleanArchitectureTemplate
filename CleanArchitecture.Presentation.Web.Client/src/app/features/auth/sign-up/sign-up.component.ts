import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { LayoutService } from 'src/app/layout/service/app.layout.service';
import { IAuthenticateResponseDto } from '../../../@core/services/authentication-service/interfaces/i-authenticate-response-dto';
import { PasswordMatch } from '../../../@shared/validators/password-match-validator';
import { PasswordStrength } from '../../../@shared/validators/password-strength-validator';
import { ISignUpDto } from './dtos/i-sign-up-dto';
import { SignUpHttpService } from './services/sign-up-http.service/sign-up-http.service';

@Component({
  selector: 'app-sign-up',
    templateUrl: './sign-up.component.html',
    styles: [`
        :host ::ng-deep .pi-eye,
        :host ::ng-deep .pi-eye-slash {
            transform:scale(1.6);
            margin-right: 1rem;
            color: var(--primary-color) !important;
        }
    `],
    providers: [MessageService]
})
export class SignUpComponent {

    isBusy!: boolean;
    isShowingValidationErrors!: boolean;
    signUpForm!: FormGroup;


    get confirmPasswordControl() {
        return this.signUpForm.get('confirmPassword');
    }

    get emailControl() {
        return this.signUpForm.get('email');
    }

    get passwordControl() {
        return this.signUpForm.get('password');
    }


    constructor(private formBuilder: FormBuilder,
        public layoutService: LayoutService,
        private messageService: MessageService,
        private router: Router,
        private signUpHttpService: SignUpHttpService) {

        this.isBusy = false;
        this.isShowingValidationErrors = false;


        // create stuff
        this.signUpForm = this.createSignUpForm();
    }


    public createSignUpForm(): FormGroup {

        // create form
        const signUpForm = this.formBuilder.group({
            email: ['', [Validators.email, Validators.required]],
            confirmPassword: ['', [Validators.required, PasswordMatch]],
            password: ['', [Validators.required, PasswordStrength]]
        });

        // return result
        return signUpForm;
    }

    public onClickSubmitSignUpForm(): void {

        // check form validity
        const isFormValid = this.signUpForm.valid;

        if (isFormValid == true) {

            // set form is loading
            this.isBusy = true;

            // get form value
            const signUpDto: ISignUpDto = this.signUpForm.value as ISignUpDto;

            // make request
            this.signUpHttpService.signUp(signUpDto)
                .subscribe({
                    next: (result) => {

                        // set is not loading
                        this.isBusy = false;

                        // show success
                        this.messageService.add({
                            detail: 'Signed up successfully',
                            summary: 'Success',
                            severity: 'success'
                        });

                        // redirect to login
                        this.router.navigateByUrl('/auth/login');
                    },
                    error: (error) => {

                        // set is not busy
                        this.isBusy = false;

                        // log error to console
                        console.error(error);

                        // show message
                        const errorMsg: string = `Signup failed. ${error}`;
                        this.messageService.add({
                            detail: errorMsg,
                            summary: 'Error',
                            severity: 'error',
                        });

                    }
                });

        } else {

            // set form to show validation errors
            this.isShowingValidationErrors = true;

            // show error message
            this.messageService.add({
                detail: 'Fix form errors and try again',
                summary: 'Form invalid',
                severity: 'error',
            });
        }
    }
}
