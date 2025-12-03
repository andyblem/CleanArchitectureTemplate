import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiEndpointService } from 'src/app/@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from 'src/app/@core/services/api-http/api-http.service';
import { IViewVehicleModelDto } from '../dtos/i-view-vehicle-model-dto';

@Injectable({
  providedIn: 'root'
})
export class ViewVehicleModelHttpService {

  prefix: string = 'VehicleModels';
  vehicleMakePrefix: string = 'VehicleMakes';
  vehicleTypePrefix: string = 'VehicleTypes';

  constructor(private apiEndPointService: ApiEndpointService,
          private apiHttpService: ApiHttpService) { }

  
  public get(vehicleModelId: number): Observable<ArrayBuffer> {

    // create url
    const url = this.apiEndPointService
      .createUrl(`${this.prefix}/get`);
      
    // set parameters
    let httpParams = new HttpParams()
        .set("id", vehicleModelId);

    // model request
    const result = this.apiHttpService.get(url, { params: httpParams });

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

  public getVehicleTypeSelectList(): Observable<ArrayBuffer> {
    
    // create url
    const url = this.apiEndPointService
        .createUrl(`${this.vehicleTypePrefix}/getSelectList`);

    // model request
    const result = this.apiHttpService.get(url);

    // return result
    return result;
  }

  public update(vehicleModelId: number, vehicleModel: IViewVehicleModelDto): Observable<ArrayBuffer> {

    // create url
    const url = this.apiEndPointService
      .createUrl(`${this.prefix}/update`);
      
      // set parameters
      let httpParams = new HttpParams()
          .set("id", vehicleModelId);

      // model request
      const result = this.apiHttpService.put(url, vehicleModel, { params: httpParams });

      // return result
      return result;
  }
}
