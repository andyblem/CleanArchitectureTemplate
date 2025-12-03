export interface IPaginatedResponseDto {

    pageNumber: number;
    pageSize: number;
    totalRecords: number;

    data: any;
}
