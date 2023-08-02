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

@Injectable({
  providedIn: 'root',
})

export class AuthInterceptor implements HttpInterceptor {

  constructor(private toastr: ToastrService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler){
    return next.handle(request)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          const errorMessage = this.setError(error);
          this.toastr.error(errorMessage);
          return throwError(errorMessage);
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
          }
          break;
       }
    }
    return errorMessage;
  }
}
