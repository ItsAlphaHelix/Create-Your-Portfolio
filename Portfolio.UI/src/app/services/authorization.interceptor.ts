import { Injectable } from '@angular/core';
import {
    HttpInterceptor,
    HttpRequest,
    HttpHandler,
    HttpErrorResponse,
    HttpHeaders
} from '@angular/common/http';
import { AccountsService } from './accounts.service';
import { catchError, switchMap, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthHelperService } from './auth-helper.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    isRefreshing: boolean = false;

    constructor(
        private accountsService: AccountsService,
        private authHelperService: AuthHelperService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler) {
        debugger
        const accessToken = this.authHelperService.getAccessToken();
        
            request = request.clone({
                setHeaders: {
                    Authorization: 'Bearer ' + accessToken,
                    'Access-Control-Allow-Origin': environment.baseUrlApi,
                    'Access-Control-Allow-Credentials': 'true',
                },
            });

        return next.handle(request).pipe(
            catchError((error) => {
                if (error instanceof HttpErrorResponse && error.status === 401) {
                    return this.handle401Error(request, next);
               }

                return throwError(() => error);
            })
        );
    }

    private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
        
        if (!this.isRefreshing) {
            this.isRefreshing = true;

            if (this.accountsService.isLoggedIn) {
                return this.accountsService.refreshAccessToken().pipe(
                    switchMap((response) => {
                        this.isRefreshing = false;
                        localStorage.setItem('access_token', response.accessToken);
                        localStorage.setItem('refresh_token', response.refreshToken);

                        return next.handle(request);
                    }),
                    catchError((error) => {
                        this.isRefreshing = false;

                        if (error.status == '403') {
                          //  this.accountsService.logout();
                        }

                        return throwError(() => error);
                    })
                );
            }
        }

        return next.handle(request);
    }
}
