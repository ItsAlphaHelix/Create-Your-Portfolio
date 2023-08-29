import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AboutInformationRequest } from 'src/app/models/about-request-model';
import { AboutMeService } from 'src/app/services/about-me.service';
import { ClientSideValidation } from 'src/app/services/client-side-validation';
import { ImagesService } from 'src/app/services/images.service';

@Component({
  selector: 'app-add-about-page',
  templateUrl: './add-about-page.component.html',
  styleUrls: ['./add-about-page.component.css']
})

export class AddAboutInformationComponent {
  constructor(
    private aboutMeService: AboutMeService,
    private imagesService: ImagesService,
    private toastr: ToastrService,
    private router: Router,
    private clientSideValidation: ClientSideValidation) { }

  aboutForm = new FormGroup({
    age: new FormControl("", [Validators.required, Validators.pattern(/[\d]+$/)]),
    education: new FormControl("", Validators.required),
    country: new FormControl("", Validators.required),
    city: new FormControl("", Validators.required),
    aboutMessage: new FormControl("", Validators.required),
    phoneNumber: new FormControl("", [Validators.required, Validators.pattern('^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$')]),
  });

  aboutFormRequest!: AboutInformationRequest

  addAboutInformation(): void {

    if (this.aboutForm.invalid) {
      debugger;
      this.clientSideValidation.aboutUserFormValidation(this.aboutForm);
      return;
    }

    this.aboutFormRequest = {
      phoneNumber: this.aboutForm.value.phoneNumber!,
      age: Number(this.aboutForm.value.age),
      education: this.aboutForm.value.education!,
      country: this.aboutForm.value.country!,
      city: this.aboutForm.value.city!,
      aboutMessage: this.aboutForm.value.aboutMessage!
    }

    this.aboutMeService.addAboutUsersInformation(this.aboutFormRequest).subscribe({
      next: () => {
        this.toastr.success('You have successfully completed the about form.')
        this.router.navigate(['/about']);
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

  uploadAboutImage(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      const file = target.files[0];
      this.imagesService.uploadAboutImage(file).subscribe(
        (response) => {
          if (response) {
            this.toastr.success('You have successfully uploaded your about image');
            return response.imageUrl;
          }
        }
      );
    }
  }
}
