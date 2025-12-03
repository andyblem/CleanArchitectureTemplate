import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiEndpointService } from '../../../../../../@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from '../../../../../../@core/services/api-http/api-http.service';
import { IRequestParameterDto } from '../../../../../../@shared/dtos/i-request-parameter-dto';
import { ICreateVehicleDto } from '../dtos/i-create-vehicle-dto';

@Injectable({
  providedIn: 'root'
})
export class IndexVehicleHttpService {

    prefix: string = 'Vehicles';
    vehicleMakePrefix: string = 'VehicleMakes';
    vehicleModelPrefix: string = 'VehicleModels';

    constructor(private apiEndPointService: ApiEndpointService,
        private apiHttpService: ApiHttpService) { }


    public create(vehicle: ICreateVehicleDto): Observable<ArrayBuffer> {

        // create url
        const url = this.apiEndPointService
            .createUrl(`${this.prefix}/create`);

        //  request
        const result = this.apiHttpService
            .post(url, vehicle);

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

        //  request
        const result = this.apiHttpService.delete(url, { params: httpParams });

        // return result
        return result;
    }

    public getVehicleMakeSelectList(): Observable<ArrayBuffer> {
        
        // create url
        const url = this.apiEndPointService
            .createUrl(`${this.vehicleMakePrefix}/getSelectList`);

        // model request
        const result = this.apiHttpService.get(url);

        // return result
        return result;
    } 

    public getVehicleModelSelectList(vehicleMakeId: number): Observable<ArrayBuffer> {
        
        // create url
        const url = this.apiEndPointService
            .createUrl(`${this.vehicleModelPrefix}/getSelectList`);

        // set parameters
        let httpParams = new HttpParams()
            .set("vehicleMakeId", vehicleMakeId);

        // model request
        const result = this.apiHttpService.get(url, { params: httpParams });

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

        //  request
        const result = this.apiHttpService.get(url, { params: httpParams });

        // return result
        return result;
    }
}
