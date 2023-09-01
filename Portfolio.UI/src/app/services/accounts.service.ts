import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import * as routes from 'src/app/shared/routes.contants';
import { Observable, of, throwError } from 'rxjs';
import { RegisterRequest } from 'src/app/models/register-request-model';
import { LoginRequest } from '../models/login-request-model';
import { LoginResponse } from 'src/app/models/login-response-model';
import { Router } from '@angular/router';
import { __param } from 'tslib';
import { UserResponse } from '../models/user-response-model';

@Injectable({
  providedIn: 'root'
})
export class AccountsService {

  constructor(
    private http: HttpClient,
    private router: Router) { }

  //private headers = new HttpHeaders().set('Content-Type', 'application/json');

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

  getUserById(userId: string): Observable<UserResponse> {
    const params = { userId };
    return this.http.get<UserResponse>(routes.GET_USER_ENDPOINT, { params });
  }

  logout() {
    sessionStorage.clear();
    if (!this.getAccessToken()) {
      this.router.navigate(['/login']);
    }
  }
}
