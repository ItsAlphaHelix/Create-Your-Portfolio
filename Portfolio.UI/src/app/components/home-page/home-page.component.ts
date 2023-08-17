import { Component, ElementRef, OnInit, Renderer2, ViewChild, resolveForwardRef } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { UserResponse } from 'src/app/models/account-models/user-response-model';
import { AccountsService } from 'src/app/services/accounts.service';
import { UserProfileService } from 'src/app/services/user-profile.service';
import Typed from 'typed.js';

@Component({
  selector: 'app-home',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomeComponent implements OnInit{
  imageURL: string | undefined;

  constructor(
    private userProfileService: UserProfileService,
    private accountsService: AccountsService,
    private router: Router,
    private renderer: Renderer2) {}

  @ViewChild('typed') typedElement!: ElementRef;

  ngOnInit(): void {
    
    this.renderer.listen('window', 'load', () => {
      this.initTyped();
    });

    this.getHomePagePicture();
    this.getUser();
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

  private initTyped(): void {
    
    const typedStrings = this.typedElement.nativeElement.getAttribute('data-typed-items');
    
    console.log(typedStrings) 
    const typedStringsArray = typedStrings.split(',');
    new Typed('.typed', {
      strings: typedStringsArray,
      loop: true,
      typeSpeed: 100,
      backSpeed: 50,
      backDelay: 2000
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
    this.userProfileService.getUserHomePagePicture().subscribe(
      (response) => {
        if (response) {
          this.imageURL = response.imageUrl;
        }
      }
    );
  }

  private getUserId() {
    return sessionStorage.getItem('userId') || '';
  }

  userResonse: UserResponse | undefined

  getUser(): void {
    const userId = this.getUserId();
    this.accountsService.getUserById(userId).subscribe(
      (response) => {
        this.userResonse = response;
      }
    );
  }
}