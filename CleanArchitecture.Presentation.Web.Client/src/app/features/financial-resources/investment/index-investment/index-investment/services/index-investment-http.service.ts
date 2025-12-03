import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiEndpointService } from '../../../../../../@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from '../../../../../../@core/services/api-http/api-http.service';
import { IRequestParameterDto } from '../../../../../../@shared/dtos/i-request-parameter-dto';
import { ICreateInvestmentDto } from '../dtos/i-create-investment-dto';
import { IIndexInvestmentParameterDto } from '../dtos/i-index-investment-parameter-dto';

@Injectable({
  providedIn: 'root'
})
export class IndexInvestmentHttpService {

    prefix: string = 'Investments';
    investmentTypePrefix: string = 'InvestmentTypes';

    constructor(private apiEndPointService: ApiEndpointService,
        private apiHttpService: ApiHttpService) { }


    public create(investmentType: ICreateInvestmentDto): Observable<ArrayBuffer> {

        // create url
        const url = this.apiEndPointService
            .createUrl(`${this.prefix}/create`);

        // type request
        const result = this.apiHttpService
            .post(url, investmentType);

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

    public getInvestmentTypeSelectList(): Observable<ArrayBuffer> {
        
        // create url
        const url = this.apiEndPointService
            .createUrl(`${this.investmentTypePrefix}/getSelectList`);

        // model request
        const result = this.apiHttpService.get(url);

        // return result
        return result;
    } 

    public getPaginatedList(requestParameter: IIndexInvestmentParameterDto): Observable<ArrayBuffer> {
        
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
            .set("investmentTypeId", requestParameter.investmentTypeId);
        
        if (requestParameter.searchString) { 
            httpParams = httpParams.set("searchString", requestParameter.searchString);
        }

        // type request
        const result = this.apiHttpService.get(url, { params: httpParams });

        // return result
        return result;
    }
}
