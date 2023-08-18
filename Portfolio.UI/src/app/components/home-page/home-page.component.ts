import { AfterViewChecked, AfterViewInit, Component, ElementRef, OnChanges, OnInit, Renderer2, ViewChild, resolveForwardRef } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { UserResponse } from 'src/app/models/account-models/user-response-model';
import { AccountsService } from 'src/app/services/accounts.service';
import { UserProfileService } from 'src/app/services/user-profile.service';
import Typed from 'typed.js';
import * as AOS from 'aos';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export default class HomeComponent implements OnInit {
  @ViewChild('typed') typedElement!: ElementRef;
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  
  imageURL: string | undefined;
  userJobTitle: string | undefined;
  userResonse: UserResponse | undefined
  
  private initAos(): void {
    AOS.init({
      duration: 1000,
      easing: 'ease-in-out',
      once: true,
      mirror: false
  });
  }
  
  private initTyped(): void {
  if(this.userJobTitle) {
    new Typed('.typed', {
      strings: [this.userJobTitle],
      typeSpeed: 100,
      backSpeed: 50,
      backDelay: 2000
    });
  }
  }
  
  private getUserId() {
  return sessionStorage.getItem('userId') || '';
  }
  
  constructor(
    private userProfileService: UserProfileService,
    private accountsService: AccountsService,
    private router: Router,
    private renderer: Renderer2) { }
    
    
    ngOnInit(): void {
      this.getHomePagePicture();
      this.getUser();
      
      setTimeout(() => {
        this.accountsService.getUserById(this.getUserId()).subscribe(response => {
          this.userJobTitle = response.jobTitle;
          this.initAos();
          this.initTyped();
        });         
      }, 100);
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

  getUser(): void {
    const userId = this.getUserId();
    this.accountsService.getUserById(userId).subscribe(
      (response) => {
        this.userResonse = response;
      }
    );
  }
}