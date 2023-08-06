import { Component, DebugNode, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AccountsService } from './services/accounts.service';
import { UserProfileService } from './services/user-profile.service';
import { delay, interval, take } from 'rxjs';
import { NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  imageURL: string = '\\assets\\img\\600x600.jpg';

  constructor(
    private accountsService: AccountsService,
    private router: Router,
    private userProfileService: UserProfileService,) { }
  ngOnInit(): void {
    
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.accountsService.isLoggedIn.subscribe((loggedIn: boolean) => {
          if (loggedIn) {
            this.getProfilePicture();
          }
        });
      }
    });
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
      this.userProfileService.uploadUserProfilePicture(file).subscribe(
        (response) => {
          if (response) {
            this.imageURL = response.imageUrl;
          }
        }
      );
    }
  }

  getProfilePicture(): void {
    this.userProfileService.getUserProfilePicture().subscribe(
      (response) => {
        if (response) {
          this.imageURL = response.imageUrl;
        }
      }
    );
  }

  get isUserLoggedIn() {
    return this.accountsService.isLoggedIn;
  }

  onLogout() {
    this.accountsService.logout();
    this.router.navigate(['/login']);
  }
}
