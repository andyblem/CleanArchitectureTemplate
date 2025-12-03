import { AbstractControl, ValidationErrors } from "@angular/forms";

export function PasswordMatch(control: AbstractControl): ValidationErrors | null {

    // get control parent
    const controlParent = control.parent;
    if (!controlParent) {
        return null;
    }

    // Get the password control
    const passwordControl = controlParent.get('password');
    if (!passwordControl) {
        return null;
    }

    // get data
    const controlMatchValue: string = passwordControl.value;
    const controlValue = control.value;

    // validate data
    if (controlMatchValue === controlValue) {
        return null;
    }

    //return result
    return {
        passwordMismatch: true
    }
}
