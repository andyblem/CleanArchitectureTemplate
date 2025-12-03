import { Injectable } from '@angular/core';
import { QueryStringParameters } from '../../classes/query-string-parameters';
import { UrlBuilder } from '../../classes/url-builder';
import { IEnvironment } from '../../../@core/interfaces/i-environment';

@Injectable({
  providedIn: 'root',
})
export class ApiEndpointService {

  // application constants
  private environment!: IEnvironment;


  constructor(
  ) { }


  public init(environment: IEnvironment) {
    this.environment = environment;
  }


  /* #region URL CREATOR */
  // URL
  public createUrl(
    action: string,
    isMockAPI: boolean = false,
  ): string {
    const urlBuilder: UrlBuilder = new UrlBuilder(
      isMockAPI ? this.environment.apiConfig.apiMockUrl :
        this.environment.apiConfig.apiUrl,
      action,
    );
    return urlBuilder.toString();
  }

  // URL WITH QUERY PARAMS
  public createUrlWithQueryParameters(
    action: string,
    queryStringHandler?:
      (queryStringParameters: QueryStringParameters) => void,
  ): string {
    const urlBuilder: UrlBuilder = new UrlBuilder(
      this.environment.apiConfig.apiUrl,
      action,
    );
    // Push extra query string params
    if (queryStringHandler) {
      queryStringHandler(urlBuilder.queryString);
    }
    return urlBuilder.toString();
  }

  // URL WITH PATH VARIABLES
  public createUrlWithPathVariables(
    action: string,
    pathVariables: any[] = [],
  ): string {
    let encodedPathVariablesUrl: string = '';
    // Push extra path variables
    for (const pathVariable of pathVariables) {
      if (pathVariable !== null) {
        encodedPathVariablesUrl +=
          `/${encodeURIComponent(pathVariable.toString())}`;
      }
    }
    const urlBuilder: UrlBuilder = new UrlBuilder(
      this.environment.apiConfig.apiUrl,
      `${action}${encodedPathVariablesUrl}`,
    );
    return urlBuilder.toString();
  }
  /* #endregion */
}
