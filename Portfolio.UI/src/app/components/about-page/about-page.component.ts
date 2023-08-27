import { Component } from '@angular/core';
import { AboutUserResponse } from 'src/app/models/about-user-response-model';
import { UserProfileService } from 'src/app/services/user-profile.service';
import { OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-about-page',
  templateUrl: './about-page.component.html',
  styleUrls: ['./about-page.component.css']
})
export class AboutComponent implements OnInit {

  constructor(private userProfileService: UserProfileService, private router: Router) { }
  
  ngOnInit(): void {
    console.log(this.imageURL);
    this.getAboutImage();
    this.getAboutUser();
  }

  aboutResponse!: AboutUserResponse
  imageURL!: string | undefined

  getAboutUser(): void {
    this.userProfileService.getAboutUser().subscribe({
      next: (response) => {
        if (response) {
          this.aboutResponse = response;
        }
      },
      error: () => {
        this.router.navigate(['/personalize-about']);
      }
    });
  }

  getAboutImage(): void {
    this.userProfileService.getAboutUserImage().subscribe({
      next: (response) => {
        if (response) {
          this.imageURL = response.imageUrl;
          console.log(this.imageURL)
        }
      },
      error: (error) => {
        console.log(error.status)
      }
    });
  }
}
