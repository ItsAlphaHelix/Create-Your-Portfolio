import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AboutUserResponse } from 'src/app/models/about-user-response-model';
import { PersonalizeAboutUserRequest } from 'src/app/models/personalize-about-user-request';
import { ClientSideValidation } from 'src/app/services/client-side-validation';
import { UserProfileService } from 'src/app/services/user-profile.service';

@Component({
  selector: 'app-edit-about-page',
  templateUrl: './edit-about-page.component.html',
  styleUrls: ['./edit-about-page.component.css']
})
export class EditAboutPageComponent implements OnInit {
  constructor(private userProfileService: UserProfileService,
     private route: ActivatedRoute, 
     private router: Router,
     private clientSideValidation: ClientSideValidation,
     private toastr: ToastrService) {}

  ngOnInit(): void {
    this.getEditAboutInformation();
  }

  editFormRequest!: PersonalizeAboutUserRequest

  editForm = new FormGroup({
    id: new FormControl(""),
    age: new FormControl("", [Validators.required, Validators.pattern(/[\d]+$/)]),
    education: new FormControl("", Validators.required),
    country: new FormControl("", Validators.required),
    city: new FormControl("", Validators.required),
    aboutMessage: new FormControl("", Validators.required),
    phoneNumber: new FormControl("", [Validators.required, Validators.pattern('^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$')]),
  });

  getEditAboutInformation(): void {

    const aboutId = this.route.snapshot.paramMap.get('aboutId');
    this.userProfileService.getEditAbout(Number(aboutId)).subscribe({
      next: (response) => {
        if (response) {
          this.editForm.setValue({
            id: response.id,
            phoneNumber: response.phoneNumber,
            age: response.age.toString(),
            education: response.education,
            country: response.country,
            city: response.city,
            aboutMessage: response.aboutMessage
          });
        }
      },
      error: () => {
      }
    });
  }

  editAboutInformation(): void {

    if (this.editForm.invalid) {
      this.clientSideValidation.aboutUserFormValidation(this.editForm);
      return;
    }
    const aboutId = this.route.snapshot.paramMap.get('aboutId');
    // this.aboutFormRequest = {
    //   phoneNumber: this.aboutForm.value.phoneNumber!,
    //   age: Number(this.aboutForm.value.age),
    //   education: this.aboutForm.value.education!,
    //   country: this.aboutForm.value.country!,
    //   city: this.aboutFsageorm.value.city!,
    //   aboutMessage: this.aboutForm.value.aboutMes!
    // }
    debugger
    this.userProfileService.editAbout(this.editForm).subscribe({
      next: (response) => {
        if (response) {
          this.toastr.success('You have been successfully edit your information.')
          this.router.navigate(['/about'])
        }
      },
      error: (error) => {
        let errorMessage = 'Unknown error occured';
        switch (error.status) {
          case 400:
            if (error.error.errors.Education) {
              errorMessage = error.error.errors.Education[0]
            } else if (error.error.errors.Country) {
              errorMessage = error.error.errors.Country[0]
            } else if (error.error.errors.City) {
              errorMessage = error.error.errors.City[0]
            } else if (error.error.errors.AboutMessage) {
              errorMessage = error.error.errors.AboutMessage[0]
            }
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
