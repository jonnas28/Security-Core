import { NbAuthTokenParceler, NbTokenLocalStorage } from '@nebular/auth';

export class ApiBase extends NbTokenLocalStorage {
//   private tokenService = new TokenStorageService;
  protected constructor(parceler: NbAuthTokenParceler) {
    super(parceler);
    
  }

  protected transformOptions(options: any): Promise<any> {
    console.log(this.get().getValue())
    options.headers = options.headers.append('authorization', `Bearer ${this.get().getValue()}`);
    return Promise.resolve(options);
  }
}