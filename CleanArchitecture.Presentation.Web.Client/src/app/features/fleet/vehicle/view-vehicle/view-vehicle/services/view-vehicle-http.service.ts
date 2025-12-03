import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiEndpointService } from 'src/app/@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from 'src/app/@core/services/api-http/api-http.service';
import { IViewVehicleDto } from '../dtos/i-view-vehicle-dto';

@Injectable({
  providedIn: 'root'
})
export class ViewVehicleHttpService {

  prefix: string = 'Vehicles';
  vehicleMakePrefix: string = 'VehicleMakes';
  vehicleModelPrefix: string = 'VehicleModels';


  constructor(private apiEndPointService: ApiEndpointService,
          private apiHttpService: ApiHttpService) { }

  
  public get(vehicleId: number): Observable<ArrayBuffer> {

    // create url
    const url = this.apiEndPointService
      .createUrl(`${this.prefix}/get`);
      
    // set parameters
    let httpParams = new HttpParams()
        .set("id", vehicleId);

    //  request
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

  public update(vehicleId: number, vehicle: IViewVehicleDto): Observable<ArrayBuffer> {

    // create url
    const url = this.apiEndPointService
      .createUrl(`${this.prefix}/update`);
      
      // set parameters
      let httpParams = new HttpParams()
          .set("id", vehicleId);

      //  request
      const result = this.apiHttpService.put(url, vehicle, { params: httpParams });

      // return result
      return result;
  }
}
