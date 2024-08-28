import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { LoginRequest } from 'src/app/models/login-request-model';
import { LoginResponse } from 'src/app/models/login-response-model';
import { AccountsService } from 'src/app/services/accounts.service';
import { AuthHelperService } from 'src/app/services/auth-helper.service';
import { ClientSideValidation } from 'src/app/services/client-side-validation';

@Component({
  selector: 'app-login',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginComponent implements OnInit {
  
  constructor(
    private accountsService: AccountsService,
    private router: Router,
    private toastr: ToastrService,
    private clientSideValidation: ClientSideValidation,
    private spinner: NgxSpinnerService,
    private authHelperService: AuthHelperService) { }
    
    @ViewChild('rememberMeCheckbox', { static: false }) rememberMeCheckbox: ElementRef | undefined;

    formRequest!: LoginComponent
    loginRequest!: LoginRequest

    
    ngOnInit() {
      if (this.authHelperService.getAccessToken()) {
        this.router.navigate(['/home']);
      }
    } 

  loginForm = new FormGroup({
    email: new FormControl("", [Validators.required, Validators.email]),
    password: new FormControl("",
      [
        Validators.required
      ]),
  });


  onLogin(): void {
    if (this.loginForm.invalid) {
      this.clientSideValidation.loginFormValidation(this.loginForm)
      return;
    }

    this.loginRequest = {
      email: this.loginForm.value.email!,
      password: this.loginForm.value.password!,
    }
    this.spinner.show();
    this.accountsService.loginUser(this.loginRequest)
      .subscribe({
        next: (response: LoginResponse) => {
          if (this.rememberMeCheckbox) {
            const isChecked = this.rememberMeCheckbox?.nativeElement.checked;
            if (isChecked) {
              localStorage.setItem("userId", response.id);
              localStorage.setItem("email", response.email);
              localStorage.setItem("access_token", response.accessToken);
              localStorage.setItem("refresh_token", response.refreshToken);
              localStorage.setItem("remember_me", isChecked.toString())
            } else {
              sessionStorage.setItem("userId", response.id);
              sessionStorage.setItem("email", response.email);
              sessionStorage.setItem("access_token", response.accessToken);
              sessionStorage.setItem("refresh_token", response.refreshToken);
              sessionStorage.setItem("remember_me", isChecked.toString())
            }
          }
          this.toastr.success('You are successfully logged in!');
          this.spinner.hide();
          this.router.navigate(['/']);
        },
        error: (error) => {
          let errorMessage = 'Unknown error occured';
          this.spinner.hide();
          switch (error.status) {
            case 401:
              errorMessage = error.error;
              break;
            default:
              this.toastr.error(errorMessage);
              break;
          }
          this.toastr.error(errorMessage);
        }
      });
  }
}
