import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiEndpointService } from '../../../../../../@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from '../../../../../../@core/services/api-http/api-http.service';
import { IRequestParameterDto } from '../../../../../../@shared/dtos/i-request-parameter-dto';
import { ICreateBookDto } from '../dtos/i-create-book-dto';

@Injectable({
  providedIn: 'root'
})
export class IndexBookHttpService {

    prefix: string = 'Books';

    constructor(private apiEndPointService: ApiEndpointService,
        private apiHttpService: ApiHttpService) { }


    public create(vehicleMake: ICreateBookDto): Observable<ArrayBuffer> {

        // create url
        const url = this.apiEndPointService
            .createUrl(`${this.prefix}/post`);

        // make request
        const result = this.apiHttpService
            .post(url, vehicleMake);

        // return result
        return result;
    }

    public delete(id: number): Observable<ArrayBuffer> {

        // create url
        const url = this.apiEndPointService
            .createUrl(`${this.prefix}/delete/${id}`);

        // make request
        const result = this.apiHttpService.delete(url);

        // return result
        return result;
    }

    public getPaginatedList(requestParameter: IRequestParameterDto): Observable<ArrayBuffer> {

        // create url
        const url = this.apiEndPointService
            .createUrl(`${this.prefix}/getList`);

        // set parameters
        let httpParams = new HttpParams()
            .set("pageNumber", requestParameter.pageNumber)
            .set("pageSize", requestParameter.pageSize)
            .set("sortFilter", requestParameter.sortFilter)
            .set("sortOrder", requestParameter.sortOrder);
        
        if (requestParameter.searchString) { 
            httpParams = httpParams.set("searchString", requestParameter.searchString);
        }

        // make request
        const result = this.apiHttpService.get(url, { params: httpParams });

        // return result
        return result;
    }
}
