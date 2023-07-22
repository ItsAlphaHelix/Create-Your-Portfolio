import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import * as routes from 'src/app/services/shared/routes.contants';
import { Observable } from 'rxjs';
import { RegisterRequest } from 'src/app/models/account-models/register-request-model';
import { LoginRequest } from '../../models/account-models/login-request-model';
import { LoginResponse } from 'src/app/models/account-models/login-response-model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  
  constructor(private http: HttpClient) {}

  headers = new HttpHeaders().set('Content-Type', 'application/json');

  registerUser(request: RegisterRequest): Observable<RegisterRequest> {
    return this.http.post<RegisterRequest>(routes.REGISTER_ENDPOINT, request);
   }

   loginUser(request: LoginRequest): Observable<LoginResponse> {
     return this.http.post<LoginResponse>(routes.LOGIN_ENDPOINT, request);
   }
}
