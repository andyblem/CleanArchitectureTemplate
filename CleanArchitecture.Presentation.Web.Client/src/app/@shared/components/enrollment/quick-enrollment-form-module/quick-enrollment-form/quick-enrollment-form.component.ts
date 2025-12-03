import { Component, Input, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SelectItem } from 'primeng/api';
import { IQuickEnrollmentDto } from './dtos/i-quick-enrollment-dto';

@Component({
  selector: 'app-quick-enrollment-form',
  templateUrl: './quick-enrollment-form.component.html',
  styleUrls: ['./quick-enrollment-form.component.scss'],
})
export class QuickEnrollmentFormComponent implements OnInit {

  firstnameStatus!: string;

  quickEnrollmentForm: FormGroup;

  @Input() isShowingValidationErrors: boolean;
  @Input() attendanceTypeSelectItems!: SelectItem[];
  @Input() compassSelectItems!: SelectItem[];
  @Input() countrySelectItems!: SelectItem[];
  @Input() genderSelectItems!: SelectItem[];
  @Input() maritalStatusSelectItems!: SelectItem[];
  @Input() programmeSelectItems!: SelectItem[];
  @Input() staffDependantSelectItems!: SelectItem[];
  @Input() staffMemberSelectItems!: SelectItem[];
  @Input() titleSelectItems!: SelectItem[];


  get address() {
    return this.quickEnrollmentForm.get('address');
  }

  get attendanceTypeId() {
    return this.quickEnrollmentForm.get('attendanceTypeId');
  }

  get campusId() {
    return this.quickEnrollmentForm.get('campusId');
  }

  get countryOfResidanceId() {
    return this.quickEnrollmentForm.get('countryOfResidanceId');
  }

  get dateOfBirth() {
    return this.quickEnrollmentForm.get('dateOfBirth');
  }

  get email() {
    return this.quickEnrollmentForm.get('email');
  }

  get firstname() {
    return this.quickEnrollmentForm.get('firstname');
  }

  get genderId() {
    return this.quickEnrollmentForm.get('genderId');
  }

  get honours() {
    return this.quickEnrollmentForm.get('honours');
  }

  get humanDemographicTitleId() {
    return this.quickEnrollmentForm.get('humanDemographicTitleId');
  }

  get maritalStatusId() {
    return this.quickEnrollmentForm.get('maritalStatusId');
  }

  get nationalID() {
    return this.quickEnrollmentForm.get('nationalID');
  }

  get nationality() {
    return this.quickEnrollmentForm.get('nationality');
  }

  get nextOfKinAddress() {
    return this.quickEnrollmentForm.get('nextOfKinAddress');
  }

  get nextOfKinEmail() {
    return this.quickEnrollmentForm.get('nextOfKinEmail');
  }

  get nextOfKinName() {
    return this.quickEnrollmentForm.get('nextOfKinName');
  }

  get nextOfKinPhoneNumber() {
    return this.quickEnrollmentForm.get('nextOfKinPhoneNumber');
  }

  get passportNumber() {
    return this.quickEnrollmentForm.get('passportNumber');
  }

  get phoneNumber() {
    return this.quickEnrollmentForm.get('phoneNumber');
  }

  get placeOfBirth() {
    return this.quickEnrollmentForm.get('placeOfBirth');
  }

  get programmeId() {
    return this.quickEnrollmentForm.get('programmeId');
  }

  get religion() {
    return this.quickEnrollmentForm.get('religion');
  }

  get surname() {
    return this.quickEnrollmentForm.get('surname');
  }

  get staffDependency() {
    return this.quickEnrollmentForm.get('staffDependency');
  }

  get staffMembership() {
    return this.quickEnrollmentForm.get('staffMembership');
  }


  constructor(
    private formBuilder: FormBuilder) {

    this.isShowingValidationErrors = false;
    this.quickEnrollmentForm = this.createQuickEnrollmentForm();
  }

  ngOnInit(): void {
  }


  public calculateContactAddressStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.address);
  }

  public calculateEmailStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.email);
  }

  public calculateFirstnameStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.firstname);
  }

  public calculateNationalIDStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.nationalID);
  }

  public calculateNationalityStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.nationality);
  }

  public calculateNextOfKinAddressStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.nextOfKinAddress);
  }

  public calculateNextOfKinNameStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.nextOfKinName);
  }

  public calculateNextOfKinPhoneNumberStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.nextOfKinPhoneNumber);
  }

  public calculateNextOfKinEmailStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.nextOfKinEmail);
  }

  public calculatePassportNumberStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.passportNumber);
  }

  public calculatePlaceOfBirthStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.placeOfBirth);
  }

  public calculatePhoneNumberStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.phoneNumber);
  }

  public calculateReligionStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.religion);
  }

  public calculateSurnameStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.surname);
  }

  public calculateStaffDependencyStatus(): string {

    return this.getFormControlStatus(this.isShowingValidationErrors,
      this.staffDependency);
  }


  public createQuickEnrollmentForm(): FormGroup {

    // create form
    const quickEnrollmentFormGroup = this.formBuilder.group({

      address: ['', [Validators.required]],
      attendanceTypeId: ['', [Validators.required]],
      campusId: ['', [Validators.required]],
      countryOfResidanceId: ['', [Validators.required]],
      dateOfBirth: ['', [Validators.required]],
      email: ['', [Validators.email, Validators.required]],
      firstname: ['', [Validators.required]],
      genderId: ['', [Validators.required]],
      honours: ['', [Validators.required]],
      humanDemographicTitleId: ['', [Validators.required]],
      maritalStatusId: ['', [Validators.required]],
      nationalID: ['', [Validators.required]],
      nationality: ['', [Validators.required]],
      nextOfKinAddress: ['', [Validators.required]],
      nextOfKinEmail: ['', [Validators.required]],
      nextOfKinName: ['', [Validators.required]],
      nextOfKinPhoneNumber: ['', [Validators.required]],
      placeOfBirth: ['', [Validators.required]],
      passportNumber: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required]],
      programmeId: ['', [Validators.required]],
      religion: ['', [Validators.required]],
      surname: ['', [Validators.required]],
      staffDependency: ['', [Validators.required]],
      staffMembership: ['', [Validators.required]],
      withEducation: ['', [Validators.required]],
    });

    // return result
    return quickEnrollmentFormGroup;
  }

    public getFormControlStatus(isShowingValidationErrors: boolean, formControl: AbstractControl | null): string {

        if (formControl == null)
            return 'default';

        // get form-control validity
        const isFormControlInValid: boolean = isShowingValidationErrors === true && formControl.invalid === true
        || formControl.invalid && (formControl.dirty || formControl.touched);

        // create form-control status
        const formControlStatus = isFormControlInValid === true ? 'danger' : 'default';

        // return result
        return formControlStatus;
    }

  public getFormValue(): IQuickEnrollmentDto {

    // get form value
    const formValue = this.quickEnrollmentForm.value as IQuickEnrollmentDto;

    // return result
    return formValue;
  }

  public isFormValid(): boolean {
    return this.quickEnrollmentForm.valid;
  }
}
