import { HttpErrorResponse, HttpHeaders, HttpStatusCode } from '@angular/common/http';
import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, filter, first, throwError } from 'rxjs';
import { LoginRequest } from 'src/app/models/account-models/login-request-model';
import { LoginResponse } from 'src/app/models/account-models/login-response-model';
import { RegisterRequest } from 'src/app/models/account-models/register-request-model';
import { AccountsService } from 'src/app/services/accounts.service';
import { ClientSideValidationService } from 'src/app/services/client-side-validation.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {


  constructor(private accountsService: AccountsService,
     private router: Router,
      private toastr: ToastrService,
       private clientSideValidationService: ClientSideValidationService) { }

  ngOnInit() {
    if (this.accountsService.getAccessToken()) {
      this.router.navigate(['/home']);
    }

  }

  formRequest!: LoginComponent


  loginForm = new FormGroup({
    email: new FormControl("", [Validators.required, Validators.email]),
    password: new FormControl("",
      [
        Validators.required
      ])
  });


  loginRequest!: LoginRequest

  onLogin(): void {

    this.clientSideValidationService.LoginFormValidation(undefined, this.loginForm)

    this.loginRequest = {
      email: this.loginForm.value.email!,
      password: this.loginForm.value.password!
    }

    this.accountsService.loginUser(this.loginRequest)
      .subscribe({

        next: (response: LoginResponse) => {
          sessionStorage.setItem("userId", response.id);
          sessionStorage.setItem("email", response.email);
          sessionStorage.setItem("accessToken", response.accessToken);
          sessionStorage.setItem("refreshToken", response.refreshToken);
          this.toastr.success('You are successfully logged in!');
          this.router.navigate(['/']);
        },
        error: (error: HttpErrorResponse) => {

          this.clientSideValidationService.LoginFormValidation(error);

        }
      });
  }
}
