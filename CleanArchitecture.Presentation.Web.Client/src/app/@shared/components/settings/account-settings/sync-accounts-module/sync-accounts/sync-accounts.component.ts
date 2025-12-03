import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PasswordMatch } from '../../../../../validators/password-match-validator';
import { PasswordStrength } from '../../../../../validators/password-strength-validator';

@Component({
  selector: 'app-sync-accounts',
  templateUrl: './sync-accounts.component.html',
  styleUrls: ['./sync-accounts.component.scss']
})
export class SyncAccountsComponent {

    isVisible: boolean = false;
    syncAccountsForm!: FormGroup;

    get emailControl() {
        return this.syncAccountsForm.get('email');
    }

    get passwordControl() {
        return this.syncAccountsForm.get('password');
    }

    constructor(private formBuilder: FormBuilder) {

        // create stuff
        this.syncAccountsForm = this.createSyncAccountsForm();
    }

    public createSyncAccountsForm(): FormGroup {

        // create form
        const syncAccountsForm = this.formBuilder.group({
            email: ['', [Validators.email, Validators.required]],
            confirmPassword: ['', [Validators.required, PasswordMatch]],
            password: ['', [Validators.required, PasswordStrength]]
        });

        // return result
        return syncAccountsForm;
    }

    onClickCancel(): void {

    }

    onClickSave(): void {

    }
}
