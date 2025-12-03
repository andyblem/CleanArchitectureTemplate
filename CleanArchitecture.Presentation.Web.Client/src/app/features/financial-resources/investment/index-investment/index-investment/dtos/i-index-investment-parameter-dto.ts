import { IRequestParameterDto } from "src/app/@shared/dtos/i-request-parameter-dto";

export interface IIndexInvestmentParameterDto extends IRequestParameterDto {

    investmentTypeId: number;

    dates: Date[];
}
