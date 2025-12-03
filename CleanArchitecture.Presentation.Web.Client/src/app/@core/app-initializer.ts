import { environment } from '../../environments/environment';
import { ApiEndpointService } from './services/api-endpoint/api-endpoint.service';
import { EnvironmentService } from './services/environment/environment.service';

export function initializer(
  apiEndpointsService: ApiEndpointService,
  environmentService: EnvironmentService) {

  // init
  environmentService.init(environment);
  apiEndpointsService.init(environmentService);

  // return result
  return () => {
    return new Promise((resolve, reject) => {
      return setTimeout(() => resolve(true), 1000);
    });
  };
}
