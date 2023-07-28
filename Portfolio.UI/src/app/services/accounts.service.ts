import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import * as routes from 'src/app/shared/routes.contants';
import { Observable, of, throwError } from 'rxjs';
import { RegisterRequest } from 'src/app/models/account-models/register-request-model';
import { LoginRequest } from '../models/account-models/login-request-model';
import { LoginResponse } from 'src/app/models/account-models/login-response-model';
import { Router } from '@angular/router';
import { __param } from 'tslib';

@Injectable({
  providedIn: 'root'
})
export class AccountsService {
  [x: string]: any;

  constructor(private http: HttpClient, private router: Router) { }

  headers = new HttpHeaders().set('Content-Type', 'application/json');

  get isLoggedIn(): Observable<boolean> {
    return of(this.getAccessToken() ? true : false);
  }

  getAccessToken() {
    return sessionStorage.getItem('accessToken');
  }

  registerUser(request: RegisterRequest): Observable<RegisterRequest> {
    return this.http.post<RegisterRequest>(routes.REGISTER_ENDPOINT, request)
      ;
  }

  loginUser(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(routes.LOGIN_ENDPOINT, request);
  }

  logout() {
    sessionStorage.clear();
    if (!this.getAccessToken()) {
      this.router.navigate(['/login']);
    }
  }
}
