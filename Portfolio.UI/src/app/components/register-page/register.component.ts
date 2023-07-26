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
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  constructor(private accountsService: AccountsService, private router: Router, private elementRef: ElementRef, private renderer: Renderer2) { }

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

  private ExtendHeightOnContainer() {
    const containerElement = this.elementRef.nativeElement.querySelector('.container');

    const currentHeight = containerElement.offsetHeight;
    const newHeight = currentHeight + 300;

    if (newHeight > 1200) {
      return;
    }

    this.renderer.setStyle(containerElement, 'height', newHeight + 'px');
  }

  @ViewChild('containerRef', { static: true }) containerRef: ElementRef | undefined;

  registerForm = new FormGroup({
    email: new FormControl("", [Validators.required, Validators.email]),
    username: new FormControl("", Validators.required),
    firstName: new FormControl("", Validators.required),
    lastName: new FormControl("", Validators.required),
    password: new FormControl("",
      [
        Validators.required,
        //Validators.pattern('^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\\d]){1,})(?=(.*[\\W]){1,})(?!.*\\s).{8,}$'),

      ]),
    confirmPassword: new FormControl("", Validators.required),
    //acceptTACCheckbox: new FormControl(false, Validators.requiredTrue)
  }, { validators: this.passwordMatchValidator });

  formRequest!: RegisterRequest

  clickRegister = false;

  onRegister(): void {

    this.clickRegister = true;

    // let countOfExceptions = 0;

    // if (this.registerForm.controls['email'].invalid) {
    //   countOfExceptions++;
    // }

    // if (this.registerForm.controls['username'].invalid) {
    //   countOfExceptions++;
    // }

    // if (this.registerForm.controls['firstName'].invalid) {
    //   countOfExceptions++;
    // }

    // if (this.registerForm.controls['lastName'].invalid) {
    //   countOfExceptions++;
    // }

    // if (this.registerForm.controls['password'].invalid) {
    //   countOfExceptions++;
    // }

    // if (this.registerForm.controls['confirmPassword'].invalid) {
    //   countOfExceptions++;
    // }

    if (this.registerForm.invalid) {

      // if (countOfExceptions === 6) {
      //   this.ExtendHeightOnContainer();
      // }

      return;
    }

    this.formRequest = {
      email: this.registerForm.value.email!,
      username: this.registerForm.value.username!,
      firstName: this.registerForm.value.firstName!,
      lastName: this.registerForm.value.lastName!,
      password: this.registerForm.value.password!,
      confirmPassword: this.registerForm.value.confirmPassword!
    }
    this.accountsService.registerUser(this.formRequest).subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (error: HttpErrorResponse) => {

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
    );
  }
}
