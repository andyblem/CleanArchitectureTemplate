import { IRequestParameterDto } from "src/app/@shared/dtos/i-request-parameter-dto";

export interface IIndexCostParameterDto extends IRequestParameterDto {

    vehicleId: number;

    dates: Date[];
}
