import { AfterViewInit, Component, DebugNode, ElementRef, OnDestroy, OnInit, Renderer2, ViewChild } from '@angular/core';
import { AccountsService } from './services/accounts.service';
import { UserProfileService } from './services/user-profile.service';
import { Observable, Subscription, delay, interval, of, take } from 'rxjs';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { UserResponse } from './models/user-response-model';
import { ToastrService } from 'ngx-toastr';
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
    private userProfileService: UserProfileService,
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
            this.toastr.success('You have successfully uploaded your home image.If you wish to add new one, simply click on the window again.')
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
