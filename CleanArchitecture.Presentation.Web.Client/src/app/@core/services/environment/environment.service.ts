import { Injectable } from '@angular/core';
import { IEnvironment, LogLevel } from '../../../@core/interfaces/i-environment';
import { IApiConfig } from '../../interfaces/i-api-config';

@Injectable({
  providedIn: 'root',
})
export class EnvironmentService implements IEnvironment {

  private environment!: IEnvironment;


  get production(): boolean {
    return this.environment.production;
  }

  get enableDebugTools(): boolean {
    return this.environment.enableDebugTools;
  }

  get apiConfig(): IApiConfig {
    return this.environment.apiConfig;
  }

  get logLevel(): LogLevel {
    return this.environment.logLevel;
  }


  constructor() { }


  init(environment: IEnvironment) {
    this.environment = environment;
  }
}
