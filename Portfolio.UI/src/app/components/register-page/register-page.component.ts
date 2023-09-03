import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { RegisterRequest } from 'src/app/models/register-request-model';
import { AccountsService } from 'src/app/services/accounts.service';
import { ClientSideValidation } from 'src/app/services/client-side-validation';

@Component({
  selector: 'app-register',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterComponent {
  constructor(
    private accountsService: AccountsService,
    private router: Router,
    private clientSideValidation: ClientSideValidation,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService) { }

  formRequest!: RegisterRequest

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
    jobTitle: new FormControl("", Validators.required),
    password: new FormControl("",
      [
        Validators.required,
        Validators.pattern('^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\\d]){1,})(?=(.*[\\W]){1,})(?!.*\\s).{8,}$'),

      ]),
    confirmPassword: new FormControl("", Validators.required),
    //acceptTACCheckbox: new FormControl(false, Validators.requiredTrue)
  }, { validators: this.passwordMatchValidator });


  onRegister(): void {
    if (this.registerForm.invalid) {
      this.clientSideValidation.registerFormValidation(this.registerForm);
      return;
    }

    this.formRequest = {
      email: this.registerForm.value.email!,
      username: this.registerForm.value.username!,
      firstName: this.registerForm.value.firstName!,
      lastName: this.registerForm.value.lastName!,
      jobTitle: this.registerForm.value.jobTitle!,
      password: this.registerForm.value.password!,
      confirmPassword: this.registerForm.value.confirmPassword!
    }

    this.spinner.show();

    this.accountsService.registerUser(this.formRequest).subscribe({

      next: () => {

        this.toastr.success('You are successfully registered!');
        this.spinner.hide();
        this.router.navigate(['/login']);
      },
      error: (error) => {
        this.spinner.hide();
        let errorMessage = 'Unknown error occured';
        switch (error.status) {
          case 409:
            errorMessage = error.error;
            break;
          case 404:
            errorMessage = error.error;
            break;
          case 400:
            if (error.error[0]?.code === 'DuplicateUserName') {
              errorMessage = error.error[0]?.description;
            } else if (error.error.errors.FirstName) {
              errorMessage = error.error.errors.FirstName[0];
            } else if (error.error.errors.LastName) {
              errorMessage = error.error.errors.LastName[0];
            } else if (error.error.errors.JobTitle) {
              errorMessage = error.error.errors.JobTitle[0];
            }
            break;
          default:
            this.toastr.error(errorMessage);
            break;
        }

        this.toastr.error(errorMessage);
      }
    }
    );
  }
}
