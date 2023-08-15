import { Component, ElementRef, OnInit, ViewChild, resolveForwardRef } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { AccountsService } from 'src/app/services/accounts.service';
import { UserProfileService } from 'src/app/services/user-profile.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  imageURL: string = '';

  constructor(
    private userProfileService: UserProfileService,
    private accountsService: AccountsService,
    private router: Router) {}

  ngOnInit(): void {
    this.getHomePagePicture();
    // this.router.events.subscribe((event) => {
    //   if (event instanceof NavigationEnd) {
    //     this.accountsService.isLoggedIn.subscribe((loggedIn: boolean) => {
    //       if (loggedIn) {
    //         this.getHomePagePicture();
    //       }
    //     });
    //   }
    // });
  }

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  openFileInput(): void {
    const fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.accept = 'image/*';
    fileInput.style.display = 'none';
    fileInput.addEventListener('change', (event) => this.handleFileInputChange(event));
    document.body.appendChild(fileInput);
    fileInput.click();
    document.body.removeChild(fileInput);
  }

  handleFileInputChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      const file = target.files[0];
      this.userProfileService.uploadUserHomePagePicture(file).subscribe(
        (response) => {
          if (response) {     
            this.imageURL = response.imageUrl;
          }
        }
      );
    }
  }

  getHomePagePicture(): void {
    debugger
    this.userProfileService.getUserHomePagePicture().subscribe(
      (response) => {
        if (response) {

          this.imageURL = response.imageUrl;
        }
      }
    );
  }
}