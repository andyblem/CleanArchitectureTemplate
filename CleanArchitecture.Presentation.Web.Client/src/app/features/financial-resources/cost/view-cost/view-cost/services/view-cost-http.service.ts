import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiEndpointService } from 'src/app/@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from 'src/app/@core/services/api-http/api-http.service';
import { IViewCostDto } from '../dtos/i-view-cost-dto';

@Injectable({
  providedIn: 'root'
})
export class ViewCostHttpService {

  prefix: string = 'Costs';
  costTypePrefix: string = 'CostTypes';
  vehiclePrefix: string = 'Vehicles';

  constructor(private apiEndPointService: ApiEndpointService,
          private apiHttpService: ApiHttpService) { }

  
  public get(vehicleTypeId: number): Observable<ArrayBuffer> {

    // create url
    const url = this.apiEndPointService
      .createUrl(`${this.prefix}/get`);
      
    // set parameters
    let httpParams = new HttpParams()
        .set("id", vehicleTypeId);

    // type request
    const result = this.apiHttpService.get(url, { params: httpParams });

    // return result
    return result;
  }
  
  public getCostTypeSelectList(): Observable<ArrayBuffer> {
        
    // create url
    const url = this.apiEndPointService
        .createUrl(`${this.costTypePrefix}/getSelectList`);

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

  public update(vehicleTypeId: number, vehicleType: IViewCostDto): Observable<ArrayBuffer> {

    // create url
    const url = this.apiEndPointService
      .createUrl(`${this.prefix}/update`);
      
      // set parameters
      let httpParams = new HttpParams()
          .set("id", vehicleTypeId);

      // type request
      const result = this.apiHttpService.put(url, vehicleType, { params: httpParams });

      // return result
      return result;
  }
}
