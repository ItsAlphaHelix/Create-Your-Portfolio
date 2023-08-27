import { Component } from '@angular/core';
import { ImagesService } from 'src/app/services/images.service';
import { AboutMeService } from 'src/app/services/about-me.service';
import { OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AboutInformationResponse } from 'src/app/models/about-response-model';

@Component({
  selector: 'app-about-page',
  templateUrl: './about-page.component.html',
  styleUrls: ['./about-page.component.css']
})
export class AboutComponent implements OnInit {

  constructor(
    private imagesService: ImagesService,
    private aboutMeService: AboutMeService,
    private router: Router) { }

  ngOnInit(): void {
    console.log(this.imageURL);
    this.getAboutImage();
    this.getAboutUser();
  }

  aboutResponse!: AboutInformationResponse
  imageURL!: string | undefined

  getAboutUser(): void {
    this.aboutMeService.getAboutUsersInformationAsync().subscribe({
      next: (response) => {
        if (response) {
          this.aboutResponse = response;
        }
      },
      error: () => {
        this.router.navigate(['/add-about-information']);
      }
    });
  }

  getAboutImage(): void {
    this.imagesService.getAboutUserImage().subscribe({
      next: (response) => {
        if (response) {
          this.imageURL = response.imageUrl;
        }
      },
      error: (error) => {
      }
    });
  }
}
