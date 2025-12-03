import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiEndpointService } from '../../../../../../@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from '../../../../../../@core/services/api-http/api-http.service';
import { IRequestParameterDto } from '../../../../../../@shared/dtos/i-request-parameter-dto';
import { ICreateVehicleTypeDto } from '../dtos/i-create-vehicle-type-dto';

@Injectable({
  providedIn: 'root'
})
export class IndexVehicleTypeHttpService {

    prefix: string = 'VehicleTypes';

    constructor(private apiEndPointService: ApiEndpointService,
        private apiHttpService: ApiHttpService) { }


    public create(vehicleType: ICreateVehicleTypeDto): Observable<ArrayBuffer> {

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

    public getPaginatedList(requestParameter: IRequestParameterDto): Observable<ArrayBuffer> {
        
        // create url
        const url = this.apiEndPointService
            .createUrl(`${this.prefix}/getPaginatedList`);

        // set parameters
        let httpParams = new HttpParams()
            .set("pageNumber", requestParameter.pageNumber)
            .set("pageSize", requestParameter.pageSize)
            .set("sortFilter", requestParameter.sortFilter)
            .set("sortOrder", requestParameter.sortOrder);
        
        if (requestParameter.searchString) { 
            httpParams = httpParams.set("searchString", requestParameter.searchString);
        }

        // type request
        const result = this.apiHttpService.get(url, { params: httpParams });

        // return result
        return result;
    }
}
