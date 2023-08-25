import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AboutUserResponse } from 'src/app/models/about-user-response-model';
import { PersonalizeAboutUserRequest } from 'src/app/models/personalize-about-user-request';
import { ClientSideValidation } from 'src/app/services/client-side-validation';
import { UserProfileService } from 'src/app/services/user-profile.service';

@Component({
  selector: 'app-personalize-about-page',
  templateUrl: './personalize-about-page.component.html',
  styleUrls: ['./personalize-about-page.component.css']
})
export class PersonalizeAboutComponent {
  constructor(
     private userProfileService: UserProfileService,
     private toastr: ToastrService,
     private router: Router,
    private clientSideValidation: ClientSideValidation) {}

  aboutForm = new FormGroup({
    age: new FormControl("", Validators.required),
    education: new FormControl("", Validators.required),
    country: new FormControl("", Validators.required),
    city: new FormControl("", Validators.required),
    aboutDescription: new FormControl("", Validators.required),
    phoneNumber: new FormControl("", [Validators.required, Validators.pattern('^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$')]),
  });

  aboutFormRequest!: PersonalizeAboutUserRequest

  onPersonalize(): void {

    if (this.aboutForm.invalid) {
      this.clientSideValidation.aboutUserFormValidation(this.aboutForm);
      return;
    }

    this.aboutFormRequest = {
      phoneNumber: this.aboutForm.value.phoneNumber!,
      age: Number(this.aboutForm.value.age),
      education: this.aboutForm.value.education!,
      country: this.aboutForm.value.country!,
      city: this.aboutForm.value.city!,
      aboutDescription: this.aboutForm.value.aboutDescription!
    }
    debugger
    this.userProfileService.personalizeAboutUser(this.aboutFormRequest).subscribe(
      () => {
        this.toastr.success('You have successfully completed the about form.')
        this.router.navigate(['/about']);
      }
    );
  }
}
