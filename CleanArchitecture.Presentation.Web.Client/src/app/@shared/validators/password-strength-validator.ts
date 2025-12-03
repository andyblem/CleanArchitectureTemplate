import { AbstractControl, ValidationErrors } from "@angular/forms";

export function PasswordStrength(control: AbstractControl): ValidationErrors | null {

    // get value
    const value = control.value;
    if (!value) {
        return null;
    }

    // test individual quality
    const hasAlpha = /[a-zA-Z]+/.test(value);
    const hasUpperCase = /[A-Z]+/.test(value);
    const hasLowerCase = /[a-z]+/.test(value);
    const hasNumeric = /[0-9]+/.test(value);
    const isTooLong = value.length > 20;
    const isTooShort = value.length < 8;

    // check validity
    const passwordValid = hasAlpha
        && hasLowerCase
        && hasNumeric
        && hasUpperCase
        && !isTooLong
        && !isTooShort;

    // return result
    return !passwordValid
        ? {
            hasAlpha: !hasAlpha,
            hasLowerCase: !hasLowerCase,
            hasNumeric: !hasNumeric,
            hasUpperCase: !hasUpperCase,
            isTooLong: isTooLong,
            isTooShort: isTooShort
          }
        : null;
}
