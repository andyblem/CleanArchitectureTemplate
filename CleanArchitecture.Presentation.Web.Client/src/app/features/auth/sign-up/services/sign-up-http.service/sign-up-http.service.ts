import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiEndpointService } from '../../../../../@core/services/api-endpoint/api-endpoint.service';
import { ApiHttpService } from '../../../../../@core/services/api-http/api-http.service';
import { ISignUpDto } from '../../dtos/i-sign-up-dto';

@Injectable({
  providedIn: 'root'
})
export class SignUpHttpService {

    constructor(
        private apiEndpointService: ApiEndpointService,
        private apiHttpService: ApiHttpService) { }


    public signUp(signUp: ISignUpDto): Observable<ArrayBuffer> {

        //create url
        const url: string = this.apiEndpointService
            .createUrl('accounts/signup');

        // make request
        const result: Observable<ArrayBuffer> = this.apiHttpService
            .post(url, signUp);

        // return result
        return result;

    }
}
