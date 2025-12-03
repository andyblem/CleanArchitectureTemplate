import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiEndpointService } from '../../../../../../@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from '../../../../../../@core/services/api-http/api-http.service';
import { IRequestParameterDto } from '../../../../../../@shared/dtos/i-request-parameter-dto';
import { ICreateIncomeDto } from '../dtos/i-create-income-dto';
import { IIndexIncomeParameterDto as IIndexIncomeParameterDto } from '../dtos/i-index-income-parameter-dto';

@Injectable({
  providedIn: 'root'
})
export class IndexIncomeHttpService {

    prefix: string = 'Incomes';
    incomeTypePrefix: string = 'IncomeTypes';
    vehiclePrefix: string = 'Vehicles';

    constructor(private apiEndPointService: ApiEndpointService,
        private apiHttpService: ApiHttpService) { }


    public create(vehicleType: ICreateIncomeDto): Observable<ArrayBuffer> {

        // create url
        const url = this.apiEndPointService
            .createUrl(`${this.prefix}/create`);

        // type request
        const result = this.apiHttpService
            .post(url, vehicleType);

        // return result
        return result;
    }

    public delete(id: number): Observable<ArrayBuffer> {

        // create url
        const url = this.apiEndPointService
            .createUrl(`${this.prefix}/delete`);

        // set parameters
        let httpParams = new HttpParams()
            .set("id", id);

        // type request
        const result = this.apiHttpService.delete(url, { params: httpParams });

        // return result
        return result;
    }

    public getIncomeTypeSelectList(): Observable<ArrayBuffer> {
        
        // create url
        const url = this.apiEndPointService
            .createUrl(`${this.incomeTypePrefix}/getSelectList`);

        // model request
        const result = this.apiHttpService.get(url);

        // return result
        return result;
    } 

    public getVehicleSelectList(): Observable<ArrayBuffer> {
        
        // create url
        const url = this.apiEndPointService
            .createUrl(`${this.vehiclePrefix}/getSelectList`);

        // model request
        const result = this.apiHttpService.get(url);

        // return result
        return result;
    }

    public getPaginatedList(requestParameter: IIndexIncomeParameterDto): Observable<ArrayBuffer> {
        
        // create url
        const url = this.apiEndPointService
            .createUrl(`${this.prefix}/getPaginatedList`);
        
        // set parameters
        let httpParams = new HttpParams()
            .set("dates", requestParameter.dates
                .filter(d => d != null)
                .map(d => d.toISOString()).toString())
            .set("pageNumber", requestParameter.pageNumber)
            .set("pageSize", requestParameter.pageSize)
            .set("sortFilter", requestParameter.sortFilter)
            .set("sortOrder", requestParameter.sortOrder)
            .set("vehicleId", requestParameter.vehicleId);
        
        if (requestParameter.searchString) { 
            httpParams = httpParams.set("searchString", requestParameter.searchString);
        }

        // type request
        const result = this.apiHttpService.get(url, { params: httpParams });

        // return result
        return result;
    }
}
