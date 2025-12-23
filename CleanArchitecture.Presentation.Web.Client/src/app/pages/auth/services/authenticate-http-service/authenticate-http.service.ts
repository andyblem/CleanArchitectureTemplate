import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IAuthenticateDto } from '../../dtos/i-authenticate-dto';
import { ApiEndpointService } from 'src/app/@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from 'src/app/@core/services/api-http/api-http.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticateHttpService {

    constructor(
        private apiEndpointService: ApiEndpointService,
        private apiHttpService: ApiHttpService) { }


    public authenticate(authenticateDto: IAuthenticateDto): Observable<ArrayBuffer> {

        const url: string = this.apiEndpointService
            .createUrl('accounts/authenticate', false, true);

        // make request
        const result: Observable<ArrayBuffer> = this.apiHttpService
            .post(url, authenticateDto);

        // return result
        return result;
    }
}
