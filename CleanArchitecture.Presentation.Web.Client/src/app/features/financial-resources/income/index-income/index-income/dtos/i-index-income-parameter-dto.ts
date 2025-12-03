import { IRequestParameterDto } from "src/app/@shared/dtos/i-request-parameter-dto";

export interface IIndexIncomeParameterDto extends IRequestParameterDto {

    vehicleId: number;

    dates: Date[];
}
