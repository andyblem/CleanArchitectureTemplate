import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiEndpointService } from 'src/app/@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from 'src/app/@core/services/api-http/api-http.service';
import { IViewBookDto } from '../dtos/i-view-book-dto';

@Injectable({
  providedIn: 'root'
})
export class ViewBookHttpService {

  prefix: string = 'Books';

  constructor(private apiEndPointService: ApiEndpointService,
          private apiHttpService: ApiHttpService) { }

  
  public get(bookId: number): Observable<ArrayBuffer> {

    // create url
    const url = this.apiEndPointService
      .createUrl(`${this.prefix}/get/${bookId}`);

    // make request
    const result = this.apiHttpService.get(url);

    // return result
    return result;
  }

  public update(bookId: number, book: IViewBookDto): Observable<ArrayBuffer> {

    // create url
    const url = this.apiEndPointService
      .createUrl(`${this.prefix}/put/${bookId}`);

      // make request
      const result = this.apiHttpService.put(url, book);

      // return result
      return result;
  }
}
