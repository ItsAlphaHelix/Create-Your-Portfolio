import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { catchError, filter, first, throwError } from 'rxjs';
import { LoginRequest } from 'src/app/models/account-models/login-request-model';
import { LoginResponse } from 'src/app/models/account-models/login-response-model';
import { RegisterRequest } from 'src/app/models/account-models/register-request-model';
import { AccountsService } from 'src/app/services/account/accounts.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {


  constructor(private accountsService: AccountsService, private router: Router, private elementRef: ElementRef, private renderer: Renderer2) { }

  ngOnInit() {
    if (this.accountsService.getAccessToken()) {
      this.router.navigate(['/home']);
    }

  }

  private ExtendHeightOnContainer() {
    const containerElement = this.elementRef.nativeElement.querySelector('.container');

    const currentHeight = containerElement.offsetHeight;
    const newHeight = currentHeight + 80;

    if (newHeight > 650) {
      return;
    }

    this.renderer.setStyle(containerElement, 'height', newHeight + 'px');
  }
  @ViewChild('containerRef', { static: true }) containerRef: ElementRef | undefined;

  formRequest!: LoginComponent

  
  loginForm = new FormGroup({
    email: new FormControl("", [Validators.required, Validators.email]),
    password: new FormControl("",
      [
        Validators.required,
        //Validators.pattern('^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\\d]){1,})(?=(.*[\\W]){1,})(?!.*\\s).{8,}$'),
        
      ])
    });
    
    
    
  clickLogin = false;

  loginRequest!: LoginRequest

  onLogin(): void {
    this.loginRequest = {
      email: this.loginForm.value.email!,
      password: this.loginForm.value.password!
    }

    let countOfExceptions = 0;

    if (this.loginForm.controls['email'].invalid) {
      countOfExceptions++;
    }

    if (this.loginForm.controls['password'].invalid) {
      countOfExceptions++;
    }

    this.clickLogin = true;

    if (this.loginForm.invalid) {

      if (countOfExceptions === 2) {
        this.ExtendHeightOnContainer();
      }
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
      });
  }
}
