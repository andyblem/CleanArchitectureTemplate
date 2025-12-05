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
  public createUrl(action: string): string;
  public createUrl(action: string, isMockAPI: boolean, ignoreVersion: boolean): string;
  public createUrl(...args: any[]): string {
    let baseUrl: string;

    // get data from args
    let action: string = args[0] as string;
    let isMockAPI: boolean = args.length > 1 ? args[1] as boolean : false;
    let ignoreVersion: boolean = args.length > 2 ? args[2] as boolean : false;

    if (ignoreVersion && this.environment.apiConfig.apiUrlNoVersion) {
      baseUrl = this.environment.apiConfig.apiUrlNoVersion;
    } else {
      baseUrl = isMockAPI ? this.environment.apiConfig.apiMockUrl :
        this.environment.apiConfig.apiUrl;
    }
    const urlBuilder: UrlBuilder = new UrlBuilder(
      baseUrl,
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
