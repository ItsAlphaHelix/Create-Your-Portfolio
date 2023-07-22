import { HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { filter, first } from 'rxjs';
import { LoginRequest } from 'src/app/models/account-models/login-request-model';
import { RegisterRequest } from 'src/app/models/account-models/register-request-model';
import { AccountService } from 'src/app/services/account/accounts.service';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.css']
})
export class AuthenticationComponent {

  constructor(private accountsService: AccountService) { }

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
    acceptTACCheckbox: new FormControl(false, Validators.requiredTrue)
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

  isLoginMode = true; // Initially, the component is in login mode

  toggleMode() {
    this.isLoginMode = !this.isLoginMode;
  }

  onLogin(): void {
    this.loginRequest = {
      email: this.loginForm.value.email!,
      password: this.loginForm.value.password!
    }

    this.accountsService.loginUser(this.loginRequest)
      //.pipe(filter(x => !!x), first())
      .subscribe(response => {
        sessionStorage.setItem("userId", response.id);
        sessionStorage.setItem("userName", response.userName)
        sessionStorage.setItem("email", response.email)
        sessionStorage.setItem("accessToken", response.accessToken)
        sessionStorage.setItem("refreshToken", response.refreshToken)
        console.log('Successfully loged in user!');
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
      // this.router.navigate(['/post-register'], { queryParams: { email: response.protectedEmail } });
      console.log("Successfully registered!")
    });
  }
}
