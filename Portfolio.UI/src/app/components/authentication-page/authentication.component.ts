import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { catchError, filter, first, throwError } from 'rxjs';
import { LoginRequest } from 'src/app/models/account-models/login-request-model';
import { LoginResponse } from 'src/app/models/account-models/login-response-model';
import { RegisterRequest } from 'src/app/models/account-models/register-request-model';
import { AccountsService } from 'src/app/services/account/accounts.service';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.css']
})
export class AuthenticationComponent implements OnInit {

  constructor(private accountsService: AccountsService, private router: Router) { }

  ngOnInit() {
    if (this.accountsService.getAccessToken()) {
      this.router.navigate(['/home']);
    }

    const loginTab = document.querySelector('.login-tab');
    const signupTab = document.querySelector('.signup-tab');
    const loginTabContent = document.getElementById('login-tab-content');
    const signupTabContent = document.getElementById('signup-tab-content');

    loginTab?.addEventListener('click', () => {
      signupTabContent?.classList.remove('active');
      loginTabContent?.classList.add('active');
    });
    signupTab?.addEventListener('click', () => {
      loginTabContent?.classList.remove('active');
      signupTabContent?.classList.add('active');
    });
  }

  private passwordMatchValidator: ValidatorFn = (formGroup: AbstractControl): ValidationErrors | null => {
    if (formGroup.get('password')?.value === formGroup.get('confirmPassword')?.value) {

      return null;

    } else if (formGroup.get('confirmPassword')?.value === '') {

      return { required: true };

    } else {

      this.registerForm.controls['confirmPassword'].setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }
  };

  registerForm = new FormGroup({
    username: new FormControl("", Validators.required),
    fullName: new FormControl("", Validators.required),
    email: new FormControl("", [Validators.required, Validators.email]),
    password: new FormControl("",
      [
        Validators.required,
        //Validators.pattern('^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\\d]){1,})(?=(.*[\\W]){1,})(?!.*\\s).{8,}$'),

      ]),
    confirmPassword: new FormControl("", Validators.required),
    //acceptTACCheckbox: new FormControl(false, Validators.requiredTrue)
  }, { validators: this.passwordMatchValidator });

  formRequest!: RegisterRequest

  clickLogin = false;

  loginForm = new FormGroup({
    email: new FormControl("", [Validators.required, Validators.email]),
    password: new FormControl("",
      [
        Validators.required,
        //Validators.pattern('^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\\d]){1,})(?=(.*[\\W]){1,})(?!.*\\s).{8,}$'),

      ])
  });

  loginRequest!: LoginRequest
  onLogin(): void {
    this.loginRequest = {
      email: this.loginForm.value.email!,
      password: this.loginForm.value.password!
    }
    this.clickLogin = true;

    if (this.loginForm.invalid) {
      return;
    }

    this.accountsService.loginUser(this.loginRequest)
      .subscribe({
        next: (response: LoginResponse) => {
          sessionStorage.setItem("userId", response.id);
          sessionStorage.setItem("email", response.email)
          sessionStorage.setItem("accessToken", response.accessToken)
          sessionStorage.setItem("refreshToken", response.refreshToken)
          this.router.navigate(['/'])
        },
        error: (error: HttpErrorResponse) => {
          if (error.status === 404) {

            this.loginForm.controls['email'].setErrors({ noExistEmail: true });

          } else if (error.status === 401) {

            this.loginForm.controls['password'].setErrors({ Unauthorized: true });

          }
        }
      }
      );
  }

  clickRegister = false;

  onRegister(): void {
    this.clickRegister = true;

    if (this.registerForm.invalid) {
      return;
    }

    this.formRequest = {
      email: this.registerForm.value.email!,
      username: this.registerForm.value.username!,
      fullName: this.registerForm.value.fullName!,
      password: this.registerForm.value.password!,
      confirmPassword: this.registerForm.value.confirmPassword!
    }
    this.accountsService.registerUser(this.formRequest).subscribe({
      next: () => {
        location.reload();
      },
      error: (error: HttpErrorResponse) => {

        if (error.status == 400) {

          const usernameErrorMessage = error.error[0].description;
          const username = this.formRequest.username

          const emailErrorMessage = error.error;

          if (usernameErrorMessage === `Username '${username}' is already taken.`) {

            this.registerForm.controls['username'].setErrors({ 'usernameAlreadyTaken': true });

          } else if (emailErrorMessage === 'Email address is already taken.') {

            this.registerForm.controls['email'].setErrors({ 'emailAlreadyTaken': true });

          }
        }
      }
    }
    );
  }

  onLogout(): void {
    this.accountsService.logout();
  }
}
