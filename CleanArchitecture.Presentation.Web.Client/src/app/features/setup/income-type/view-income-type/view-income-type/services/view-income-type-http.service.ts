import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiEndpointService } from 'src/app/@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from 'src/app/@core/services/api-http/api-http.service';
import { IViewIncomeTypeDto } from '../dtos/i-view-income-type-dto';

@Injectable({
  providedIn: 'root'
})
export class ViewIncomeTypeHttpService {

  prefix: string = 'IncomeTypes';

  constructor(private apiEndPointService: ApiEndpointService,
          private apiHttpService: ApiHttpService) { }

  
  public get(vehicleMakeId: number): Observable<ArrayBuffer> {

    // create url
    const url = this.apiEndPointService
      .createUrl(`${this.prefix}/get`);
      
    // set parameters
    let httpParams = new HttpParams()
        .set("id", vehicleMakeId);

    // make request
    const result = this.apiHttpService.get(url, { params: httpParams });

    // return result
    return result;
  }

  public update(vehicleMakeId: number, vehicleMake: IViewIncomeTypeDto): Observable<ArrayBuffer> {

    // create url
    const url = this.apiEndPointService
      .createUrl(`${this.prefix}/update`);
      
      // set parameters
      let httpParams = new HttpParams()
          .set("id", vehicleMakeId);

      // make request
      const result = this.apiHttpService.put(url, vehicleMake, { params: httpParams });

      // return result
      return result;
  }
}
