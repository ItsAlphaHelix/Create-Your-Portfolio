import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserResponse } from 'src/app/models/user-response-model';
import { AccountsService } from 'src/app/services/accounts.service';
import Typed from 'typed.js';
import * as AOS from 'aos';
import { Observable, of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { ImagesService } from 'src/app/services/images.service';

@Component({
  selector: 'app-home',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})

export default class HomeComponent implements OnInit {

  constructor(
    private imagesService: ImagesService,
    private accountsService: AccountsService,
    private toastr: ToastrService) { }

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
    if (this.userJobTitle) {
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
      this.imagesService.uploadHomePageImage(file).subscribe(
        (response) => {
          if (response) {
            this.imageURL = response.imageUrl;
            this.toastr.success('You have successfully uploaded your home image.If you wish to add new one, simply click on the window again.');
          }
        }
      );
    }
  }

  getHomePagePicture(): void {
    this.imagesService.getUserHomePageImage().subscribe({
      next: (response) => {
        if (response) {
          this.imageURL = response.imageUrl;
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
    this.accountsService.getUserById(userId).subscribe(
      (response) => {
        this.userResonse = response;
      }
    );
  }
}