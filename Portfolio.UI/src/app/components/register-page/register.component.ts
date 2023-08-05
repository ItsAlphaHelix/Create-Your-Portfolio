import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, filter, first, throwError } from 'rxjs';
import { RegisterRequest } from 'src/app/models/account-models/register-request-model';
import { AccountsService } from 'src/app/services/accounts.service';
import { ClientSideValidationService } from 'src/app/services/client-side-validation.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  constructor(private accountsService: AccountsService,
     private router: Router,
        private clientSideValidationService: ClientSideValidationService,
         private toastr: ToastrService
        ) { }

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
    email: new FormControl("", [Validators.required, Validators.email]),
    username: new FormControl("", Validators.required),
    firstName: new FormControl("", Validators.required),
    lastName: new FormControl("", Validators.required),
    password: new FormControl("",
      [
        Validators.required,
        Validators.pattern('^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\\d]){1,})(?=(.*[\\W]){1,})(?!.*\\s).{8,}$'),

      ]),
    confirmPassword: new FormControl("", Validators.required),
    //acceptTACCheckbox: new FormControl(false, Validators.requiredTrue)
  }, { validators: this.passwordMatchValidator });

  formRequest!: RegisterRequest

  onRegister(): void {
    
    if (this.registerForm.invalid) {
      this.clientSideValidationService.RegisterFormValidation(this.registerForm);
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

        this.toastr.success('You are successfully registered!');
        this.router.navigate(['/login']);
      }
    }
    );
  }
}
