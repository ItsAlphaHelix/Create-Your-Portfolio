import { Component } from '@angular/core';
import { AboutUserResponse } from 'src/app/models/about-user-response-model';
import { UserProfileService } from 'src/app/services/user-profile.service';
import { OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-about-page',
  templateUrl: './about-page.component.html',
  styleUrls: ['./about-page.component.css']
})
export class AboutComponent implements OnInit {

  constructor(private userProfileService: UserProfileService, private router: Router) { }
  
  ngOnInit(): void {
    this.getAboutUser();
  }

  aboutResponse!: AboutUserResponse

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
}
