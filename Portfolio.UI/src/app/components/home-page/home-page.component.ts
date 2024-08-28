import { Component, OnInit } from '@angular/core';
import { UserResponse } from 'src/app/models/user-response-model';
import { AccountsService } from 'src/app/services/accounts.service';
import Typed from 'typed.js';
import * as AOS from 'aos';
import { ToastrService } from 'ngx-toastr';
import { ImagesService } from 'src/app/services/images.service';
import { GitHubApiService } from 'src/app/services/github-api.service';
import { AuthHelperService } from 'src/app/services/auth-helper.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-home',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})

export default class HomeComponent implements OnInit {

  constructor(
    private imagesService: ImagesService,
    private accountsService: AccountsService,
    private toastr: ToastrService,
    private githubApiService: GitHubApiService,
    private spinner: NgxSpinnerService) { }

  imageURL: string | undefined;
  userJobTitle: string | undefined;
  userResonse: UserResponse | undefined
  isHomeImageExist = false;

  ngOnInit(): void {
    //this.getFromGithubRepositoryLanguages();
    this.getHomePagePicture();
    this.getUser();
    setTimeout(() => {
      this.accountsService.getUserById().subscribe(response => {
        this.userJobTitle = response.jobTitle;
        this.initAos();
        this.initTyped();
      });
    }, 100);
  }

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

  getFromGithubRepositoryLanguages() {
    this.githubApiService.getGitHubRepositoryLanguages().subscribe();
  }

  openFileInput(): void {
    const fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.accept = 'image/*';
    fileInput.style.display = 'none';
    fileInput.addEventListener('change', (event) => this.uploadHomeImage(event));
    document.body.appendChild(fileInput);
    fileInput.click();
    document.body.removeChild(fileInput);
  }

  uploadHomeImage(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      this.spinner.show();
      const file = target.files[0];
      if (this.isHomeImageExist == false) {
        this.imagesService.uploadHomePageImage(file).subscribe({
          next: (response) => {
            if (response) {
              this.imageURL = response.imageUrl;
              this.toastr.success('You have successfully uploaded your home image.If you wish to add new one, simply click on the window again.');
              this.spinner.hide();
            }
          },
          error: () => {
            this.spinner.hide();
            this.toastr.error('Invalid image type');
          }
        });
      } else {
        this.imagesService.updateHomeImage(file).subscribe({
          next: (response) => {
            if (response) {
              this.imageURL = response.imageUrl;
              this.toastr.success('You have successfully updated your home image.If you wish to add new one, simply click on the window again.');
              this.spinner.hide();
            }
          },
          error: () => {
            this.spinner.hide();
            this.toastr.error('Invalid image type');
          }
        });
      }
    }
  }

  getHomePagePicture(): void {
    this.imagesService.getUserHomePageImage().subscribe({
      next: (response) => {
        if (response) {
          this.imageURL = response.imageUrl;
          this.isHomeImageExist = true;
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
    this.accountsService.getUserById().subscribe(
      (response) => {
        this.userResonse = response;
      }
    );
  }
}