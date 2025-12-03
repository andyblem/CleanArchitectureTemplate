import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { LayoutService } from 'src/app/layout/service/app.layout.service';
import { AuthenticationService } from '../../../@core/services/authentication-service/authentication.service';
import { IAuthenticateResponseDto } from '../../../@core/services/authentication-service/interfaces/i-authenticate-response-dto';
import { IAuthenticateDto } from './dtos/i-authenticate-dto';
import { AuthenticateHttpService } from './services/authenticate-http-service/authenticate-http.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
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
export class LoginComponent {

    isBusy!: boolean;
    isShowingValidationErrors!: boolean;

    returnUrl!: string;

    loginForm!: FormGroup;


    get emailControl() {
        return this.loginForm.get('email');
    }

    get isRememberMeControl() {
        return this.loginForm.get('isRememberMe');
    }

    get passwordControl() {
        return this.loginForm.get('password');
    }

    constructor(private formBuilder: FormBuilder,
        private activedRoute: ActivatedRoute,
        private authenticationService: AuthenticationService,
        private loginHttpService: AuthenticateHttpService,
        private messageService: MessageService,
        private router: Router,
        public layoutService: LayoutService) {

        this.isBusy = false;
        this.isShowingValidationErrors = false;

        // set up the return url
        this.returnUrl = this.activedRoute.snapshot.queryParams['returnUrl'] || 'features';

        // if already logged in go to redirect url
        const isStillLoggedIn = this.authenticationService.isStillLoggedIn;
        if (isStillLoggedIn == true) {

            // redirect to redirect url
            this.router.navigateByUrl(this.returnUrl);
        }

        // create stuff
        this.loginForm = this.createLoginForm();
    }

    public createLoginForm(): FormGroup {

        // create form
        const loginForm = this.formBuilder.group({
            email: ['', [Validators.email, Validators.required]],
            isRememberMe: [false],
            password: ['', [Validators.required]]
        });

        // return result
        return loginForm;
    }


    public onClickSubmitLoginForm(): void {

        // check form validity
        const isFormValid = this.loginForm.valid;

        if (isFormValid == true) {

            // set form is loading
            this.isBusy = true;

            // get form value
            const authenticateDTO: IAuthenticateDto = this.loginForm.value as IAuthenticateDto;

            // make request
            this.loginHttpService.authenticate(authenticateDTO)
                .subscribe({
                    next: (result) => {
                        
                        // set is not loading
                        this.isBusy = false;

                        //todo::enable 2fa
                        // redirect to 2fa url
                        //this.router.navigate(['auth/verify'], { queryParams: { returnUrl: this.returnUrl } });


                        // get data
                        const authenticationData: IAuthenticateResponseDto = result as any as IAuthenticateResponseDto;

                        // save data to local storage
                        this.authenticationService.authenticate(authenticationData)

                        // redirect to redirect url
                        this.router.navigateByUrl(this.returnUrl);
                    },
                    error: (error) => {

                        // set is not busy
                        this.isBusy = false;

                        // log error to console
                        console.log(error);

                        // show message
                        const errorMsg: string = 'Login failed ';
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
