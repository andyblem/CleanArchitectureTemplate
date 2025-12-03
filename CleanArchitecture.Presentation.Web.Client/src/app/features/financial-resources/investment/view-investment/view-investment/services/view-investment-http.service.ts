import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiEndpointService } from 'src/app/@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from 'src/app/@core/services/api-http/api-http.service';
import { IViewInvestmentDto } from '../dtos/i-view-investment-dto';

@Injectable({
  providedIn: 'root'
})
export class ViewInvestmentHttpService {

  prefix: string = 'Investments';
  investmentTypePrefix: string = 'InvestmentTypes';


  constructor(private apiEndPointService: ApiEndpointService,
          private apiHttpService: ApiHttpService) { }

  
  public get(investmentId: number): Observable<ArrayBuffer> {

    // create url
    const url = this.apiEndPointService
      .createUrl(`${this.prefix}/get`);
      
    // set parameters
    let httpParams = new HttpParams()
        .set("id", investmentId);

    // type request
    const result = this.apiHttpService.get(url, { params: httpParams });

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

  public update(investmentId: number, investment: IViewInvestmentDto): Observable<ArrayBuffer> {

    // create url
    const url = this.apiEndPointService
      .createUrl(`${this.prefix}/update`);
      
      // set parameters
      let httpParams = new HttpParams()
          .set("id", investmentId);

      // type request
      const result = this.apiHttpService.put(url, investment, { params: httpParams });

      // return result
      return result;
  }
}
