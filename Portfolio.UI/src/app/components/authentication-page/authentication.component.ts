import { HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { filter, first } from 'rxjs';
import { LoginRequest } from 'src/app/models/account-models/login-request-model';
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
  

  registerForm = new FormGroup({
    username: new FormControl("", Validators.required),
    firstName: new FormControl("", Validators.required),
    lastName: new FormControl("", Validators.required),
    email: new FormControl("", [Validators.required, Validators.email]),
    password: new FormControl("",
      [
        Validators.required,
        //Validators.pattern('^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\\d]){1,})(?=(.*[\\W]){1,})(?!.*\\s).{8,}$'),

      ]),
    confirmPassword: new FormControl("", Validators.required),
    //acceptTACCheckbox: new FormControl(false, Validators.requiredTrue)
  });

  formRequest!: RegisterRequest

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

    this.accountsService.loginUser(this.loginRequest)
      .subscribe(response => {
        sessionStorage.setItem("userId", response.id);
        sessionStorage.setItem("email", response.email)
        sessionStorage.setItem("accessToken", response.accessToken)
        sessionStorage.setItem("refreshToken", response.refreshToken)
        this.router.navigate(['/'])
      })
  }

  onRegister(): void {
    this.formRequest = {
      email: this.registerForm.value.email!,
      username: this.registerForm.value.username!,
      firstName: this.registerForm.value.firstName!,
      lastName: this.registerForm.value.lastName!,
      password: this.registerForm.value.password!,
      confirmPassword: this.registerForm.value.confirmPassword!
    }
      this.accountsService.registerUser(this.formRequest).subscribe(response => {
      this.router.navigate(['/'])
    });
  }

  onLogout(): void {
      this.accountsService.logout();
  }
}
