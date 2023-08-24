import { HttpErrorResponse, HttpHeaders, HttpStatusCode } from '@angular/common/http';
import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, filter, first, throwError } from 'rxjs';
import { LoginRequest } from 'src/app/models/login-request-model';
import { LoginResponse } from 'src/app/models/login-response-model';
import { RegisterRequest } from 'src/app/models/register-request-model';
import { AccountsService } from 'src/app/services/accounts.service';
import { ClientSideValidation } from 'src/app/services/client-side-validation';

@Component({
  selector: 'app-login',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginComponent implements OnInit {
  formRequest!: LoginComponent
  loginRequest!: LoginRequest

  constructor(private accountsService: AccountsService,
    private router: Router,
    private toastr: ToastrService,
    private clientSideValidation: ClientSideValidation) { }


  ngOnInit() {
    if (this.accountsService.getAccessToken()) {
      this.router.navigate(['/home']);
    }
  }

  loginForm = new FormGroup({
    email: new FormControl("", [Validators.required, Validators.email]),
    password: new FormControl("",
      [
        Validators.required
      ])
  });

  onLogin(): void {
    if (this.loginForm.invalid) {
      this.clientSideValidation.LoginFormValidation(this.loginForm)
      return;
    }

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
        }
      });
  }
}
