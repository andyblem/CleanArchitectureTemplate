import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiEndpointService } from 'src/app/@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from 'src/app/@core/services/api-http/api-http.service';
import { IViewIncomeDto } from '../dtos/i-view-income-dto';

@Injectable({
  providedIn: 'root'
})
export class ViewIncomeHttpService {

  prefix: string = 'Incomes';
  incomeTypePrefix: string = 'IncomeTypes';
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

  public update(vehicleTypeId: number, vehicleType: IViewIncomeDto): Observable<ArrayBuffer> {

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
