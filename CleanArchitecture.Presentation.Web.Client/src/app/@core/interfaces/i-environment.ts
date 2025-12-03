import { IApiConfig } from './i-api-config';

export type LogLevel = 'debug' | 'info' | 'warn' | 'error';

export interface IEnvironment {

  production: boolean;
  enableDebugTools: boolean;

  apiConfig: IApiConfig;
  logLevel: LogLevel;
}
