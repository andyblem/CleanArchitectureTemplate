import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiEndpointService } from 'src/app/@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from 'src/app/@core/services/api-http/api-http.service';

@Injectable({
  providedIn: 'root'
})
export class VehicleReportHttpService {

  prefix: string = 'Vehicles';

  constructor(private apiEndPointService: ApiEndpointService,
          private apiHttpService: ApiHttpService) { }

  public getReport(id: number): Observable<ArrayBuffer> {
    
    // create url
    const url = this.apiEndPointService
        .createUrl(`${this.prefix}/getReport`);
        
    // set parameters
    let httpParams = new HttpParams()
        .set("id", id);

    // model request
    const result = this.apiHttpService.get(url, { params: httpParams });

    // return result
    return result;
  }

  public generateReport(id: number): Observable<ArrayBuffer> {
    
    // create url
    const url = this.apiEndPointService
        .createUrl(`${this.prefix}/generateReport`);
        
    // set parameters
    let httpParams = new HttpParams()
        .set("id", id);

    // model request
    const result = this.apiHttpService.get(url, { params: httpParams });

    // return result
    return result;
  }
}
