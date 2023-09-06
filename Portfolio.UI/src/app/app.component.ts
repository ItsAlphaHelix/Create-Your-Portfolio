import { Component, OnInit, Renderer2 } from '@angular/core';
import { AccountsService } from './services/accounts.service';
import { NavigationEnd, NavigationError, Router } from '@angular/router';
import { UserResponse } from './models/user-response-model';
import { ToastrService } from 'ngx-toastr';
import { ImagesService } from './services/images.service';
import { AboutMeService } from './services/about-me.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor(
    private accountsService: AccountsService,
    private router: Router,
    private imagesService: ImagesService,
    private renderer: Renderer2,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService) { }

  userResonse: UserResponse | undefined
  imageURL: string = '\\assets\\img\\profile-upload-image.png';
  isUserProfileImageExist = false;

  ngOnInit(): void {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.accountsService.isLoggedIn.subscribe((loggedIn: boolean) => {
          if (loggedIn) {
            this.getUser();
            this.getProfilePicture();
          }
        });
      }
    });
  }
  private getUserId() {
    return sessionStorage.getItem('userId') || '';
  }

  onClickMobileNav() {
    const mobileNavToggle = document.querySelector('.mobile-nav-toggle');
    const body = document.getElementsByTagName('body')[0];
    body.classList.toggle('mobile-nav-active');

    const iconElement = mobileNavToggle as HTMLElement;
    const biList = document.querySelector('.bi-list') as HTMLElement;
    if (biList) {
      this.renderer.removeClass(iconElement, 'bi-list');
      this.renderer.addClass(iconElement, 'bi-x');
    }
    else {
      this.renderer.removeClass(iconElement, 'bi-x');
      this.renderer.addClass(iconElement, 'bi-list');
    }
  }

  openFileInput(): void {
    const fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.accept = 'image/*';
    fileInput.style.display = 'none';
    fileInput.addEventListener('change', (event) => this.uploadProfileImage(event));
    document.body.appendChild(fileInput);
    fileInput.click();
    document.body.removeChild(fileInput);
  }

  uploadProfileImage(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      this.spinner.show();
      debugger;
      const file = target.files[0];
      if (this.isUserProfileImageExist == false) {
        this.imagesService.uploadProfileImage(file).subscribe(
          (response) => {
            if (response) {
              this.imageURL = response.imageUrl;
              this.toastr.success('You have successfully uploaded your profile image.If you wish to add new one, simply click on the window again.')            
              this.spinner.hide();
            }
          }
        );
      } else {
        this.imagesService.updateProfileImage(file).subscribe(
          (response) => {
            if (response) {
              this.imageURL = response.imageUrl;
              this.toastr.success('You have successfully updated your profile image.If you wish to add new one, simply click on the window again.')
              this.spinner.hide();
            }
          }
        );
      }
    }
  }

  getProfilePicture(): void {
    this.imagesService.getUserProfileImage().subscribe({
      next: (response) => {
        if (response) {
          this.imageURL = response.imageUrl;
          this.isUserProfileImageExist  = true;
        }
      },
      error: (error) => {
        let errorMessage = 'Unknown error occured'
        switch (error.status) {
          case 404:
            errorMessage = error.error;
            break;
          default:
            this.toastr.error(errorMessage);
            break;
        }
        this.toastr.error(errorMessage);
      }
  });
  }



  getUser(): void {
    const userId = this.getUserId();

    this.accountsService.getUserById().subscribe(
      (response) => {
        this.userResonse = response;
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
