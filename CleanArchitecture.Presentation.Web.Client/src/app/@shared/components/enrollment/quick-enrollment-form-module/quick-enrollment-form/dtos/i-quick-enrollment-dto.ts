export interface IQuickEnrollmentDto {

  attendanceTypeId: number;
  campusId: number;
  countryOfResidanceId: number;
  genderId: number;
  humanDemographicTitleId: number;
  maritalStatusId: number;
  programmeId: number;

  address: string;
  email: string;
  firstname: string;
  nationalID: string;
  nationality: string;
  nextOfKinAddress: string;
  nextOfKinEmail: string;
  nextOfKinName: string;
  nextOfKinPhoneNumber: string;
  passportNumber: string;
  placeOfBirth: string;
  phoneNumber: string;
  religion: string;
  surname: string;
  staffDependency: string;
  staffMembership: string;
  
  dateOfBirth: Date;
}
