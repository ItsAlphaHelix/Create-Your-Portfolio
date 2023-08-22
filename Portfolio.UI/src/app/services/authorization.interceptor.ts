import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})

export class AuthInterceptor implements HttpInterceptor {

  constructor(private toastr: ToastrService, private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<any> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = this.setError(error);
        this.toastr.error(errorMessage);
        throw new Error(errorMessage);
      })
    );
  }

  setError(error: HttpErrorResponse): string {
    let errorMessage = 'Unknown error occured'
    if (error.error instanceof ErrorEvent) {
      //client side error
      errorMessage = error.error.message;
    }
    else {
      //server side error
      switch (error.status) {
        case 404:
          console.log(error.error)
          errorMessage = error.error; 
        break;
        case 401:
          errorMessage = error.error;
          break;
        case 409:
          errorMessage = error.error;
          break;
        case 400:
          if (error.error[0]?.code === 'DuplicateUserName') {
            errorMessage = error.error[0]?.description;
          } else if (error.error.errors.FirstName) {
            errorMessage = error.error.errors.FirstName[0];
          } else if (error.error.errors.LastName) {
            errorMessage = error.error.errors.LastName[0];
          } else if (error.error.errors.JobTitle) {
            errorMessage = error.error.errors.JobTitle[0];
          }
          break;
        }
    }
    return errorMessage;
  }
}
