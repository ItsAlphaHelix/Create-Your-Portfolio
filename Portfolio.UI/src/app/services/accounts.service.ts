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
import { AuthHelperService } from './auth-helper.service';
import { RefreshAccessTokenResponse } from '../models/refresh-access-token-response-model';

@Injectable({
  providedIn: 'root'
})
export class AccountsService {

  constructor(
    private http: HttpClient,
    private router: Router,
    private authHelperService: AuthHelperService) { }

  get isLoggedIn(): Observable<boolean> {
    return of(this.authHelperService.getAccessToken() ? true : false);
  }


  registerUser(request: RegisterRequest): Observable<RegisterRequest> {
    return this.http.post<RegisterRequest>(routes.REGISTER_ENDPOINT, request)
      ;
  }

  loginUser(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(routes.LOGIN_ENDPOINT, request);
  }

  getUserById(): Observable<UserResponse> {
    return this.http.get<UserResponse>(routes.GET_USER_ENDPOINT, { params: this.authHelperService.getParams() });
  }

  refreshAccessToken() {
    const refreshToken = localStorage.getItem('refresh_token');
    const userId = localStorage.getItem('userId');

    return this.http.get<RefreshAccessTokenResponse>(
      routes.REFRESH_TOKEN_ENDPOINT,
      {
        params: {
          refreshToken: refreshToken!,
          userId: userId!
        },
      }
    );
  }

  logout() {
    localStorage.clear();
    if (!this.authHelperService.getAccessToken()) {
      this.router.navigate(['/login']);
    }
  }
}
