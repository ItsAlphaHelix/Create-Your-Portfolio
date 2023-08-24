import { Component } from '@angular/core';
import { AboutUserResponse } from 'src/app/models/about-user-response-model';
import { UserProfileService } from 'src/app/services/user-profile.service';
import { OnInit } from '@angular/core';

@Component({
  selector: 'app-about-page',
  templateUrl: './about-page.component.html',
  styleUrls: ['./about-page.component.css']
})
export class AboutComponent implements OnInit {

  constructor(private userProfileService: UserProfileService) {

  }

  ngOnInit(): void {
    this.getAboutUser();
  }

  aboutResponse!: AboutUserResponse

  getAboutUser(): void {
    this.userProfileService.getAboutUser().subscribe(
      (response) => {
        debugger;
        if (response) {
          this.aboutResponse = response;
        }
      }
    );
  }
}
