import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterRequest } from '../models/account-models/register-request-model';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { timeout } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClientSideValidationService {

  constructor(private toastr: ToastrService) { }

  RegisterFormValidation(error?: HttpErrorResponse, registerForm?: FormGroup) {

    if (registerForm !== undefined) {

      if (registerForm.controls['email'].hasError('required') || registerForm.controls['username'].hasError('required') || registerForm.controls['firstName'].hasError('required') || registerForm.controls['lastName'].hasError('required') || registerForm.controls['password'].hasError('required') || registerForm.controls['confirmPassword'].hasError('required')) {

        this.toastr.error('All fields are required!');
        return;

      }

      if (registerForm.controls['email'].invalid && !(registerForm.controls['email'].hasError('required') && registerForm.controls['username'].hasError('required') && registerForm.controls['firstName'].hasError('required') && registerForm.controls['lastName'].hasError('required') && registerForm.controls['password'].hasError('required') && registerForm.controls['confirmPassword'].hasError('required'))) {
        this.toastr.error('Invalid email address!');
        return;
      }
      
      if (registerForm.controls['password'].hasError('pattern') && !(registerForm.controls['password'].hasError('required') && registerForm.controls['firstName'].hasError('required') && registerForm.controls['username'].hasError('required') && registerForm.controls['email'].hasError('required') && registerForm.controls['confirmPassword'].hasError('required'))) {

        this.toastr.error('Invalid password. Password should be at least 8 characters long and also should contain at least one lower case, one upper case, one digit and one special symbol.');
        return;

      }

      if (registerForm.controls['password'].invalid || registerForm.controls['confirmPassword'].invalid && !(registerForm.controls['email'].hasError('required') && registerForm.controls['username'].hasError('required') && registerForm.controls['firstName'].hasError('required') && registerForm.controls['lastName'].hasError('required') && registerForm.controls['password'].hasError('required') && registerForm.controls['confirmPassword'].hasError('required'))) {
        this.toastr.error('Password doesn\'t match!');
        return;
      }

    }

    if (error !== undefined) {
      if (error.status === 400) {

        if (error.error === "Email address is already taken.") {

          this.toastr.error('The email you provided is already taken!');

        }

        if (error.error instanceof Array) {

          this.toastr.error('The username you provided is already taken!');

        } else if (error.error instanceof Object) {

          if (error.error.errors.FirstName !== undefined) {

            if (error.error.errors.FirstName[0] === "The first name length shouldn't be less than 3 symbols.") {

              this.toastr.error('The length of the first name must be between 3 and 30 characters!');

            } else {

              this.toastr.error('The first name should start with a capital letter, and it should contain only letters!');

            }

          } else if (error.error.errors.LastName !== undefined) {

            if (error.error.errors.LastName[0] === "The last name length shouldn't be less than 3 symbols.") {

              this.toastr.error('The length of the last name must be between 3 and 30 characters!');

            } else {

              this.toastr.error('The last name should start with a capital letter, and it should contain only letters!');

            }
          }
        }
      }
    }
  }

  LoginFormValidation(error?: HttpErrorResponse, loginForm?: FormGroup) {

    if (loginForm !== undefined) {

      if (loginForm.controls['email'].hasError('required') || loginForm.controls['password'].hasError('required')) {

        this.toastr.error('All fields are required');

      }
      if (loginForm.controls['email'].invalid && !loginForm.controls['email'].hasError('required')) {

        this.toastr.error('Invalid email address!');

      }
    }

    if (error !== undefined) {

      if (error.status === 404) {

        this.toastr.error('Your email or password is incorrect. Please try again!');

      } else if (error.status === 401) {

        this.toastr.error('Your email or password is incorrect. Please try again!');
      } else {
        console.log(error)
      }
    }
  }
}