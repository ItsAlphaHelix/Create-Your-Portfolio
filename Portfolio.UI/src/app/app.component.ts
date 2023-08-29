import { Component, OnInit, Renderer2 } from '@angular/core';
import { AccountsService } from './services/accounts.service';
import { NavigationEnd, NavigationError, Router } from '@angular/router';
import { UserResponse } from './models/user-response-model';
import { ToastrService } from 'ngx-toastr';
import { ImagesService } from './services/images.service';
declare function handleClick(): void;

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
    private toastr: ToastrService) { }

  imageURL: string = '\\assets\\img\\600x600.jpg';
  userResonse: UserResponse | undefined

  private getUserId() {
    return sessionStorage.getItem('userId') || '';
  }

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
      const file = target.files[0];
      this.imagesService.uploadProfileImage(file).subscribe(
        (response) => {
          if (response) {
            this.imageURL = response.imageUrl;
            this.toastr.success('You have successfully uploaded your home image.If you wish to add new one, simply click on the window again.')
          }
        }
      );
    }
  }

  getProfilePicture(): void {
    this.imagesService.getUserProfileImage().subscribe(
      (response) => {
        if (response) {
          this.imageURL = response.imageUrl;
        }
      },
      (error) => {
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
    );
  }



  getUser(): void {
    const userId = this.getUserId();

    this.accountsService.getUserById(userId).subscribe(
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
