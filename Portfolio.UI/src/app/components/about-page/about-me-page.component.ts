import { AfterContentChecked, AfterViewChecked, AfterViewInit, Component, ElementRef, HostListener } from '@angular/core';
import { ImagesService } from 'src/app/services/images.service';
import { AboutMeService } from 'src/app/services/about-me.service';
import { OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AboutInformationResponse } from 'src/app/models/about-response-model';
import * as AOS from 'aos';
import { LanguageStats } from 'src/app/models/language-stats-model';
import { GitHubApiService } from 'src/app/services/github-api.service';
import { NgxSpinnerService } from 'ngx-spinner';

declare const Waypoint: any;

@Component({
  selector: 'app-about-page',
  templateUrl: './about-me-page.component.html',
  styleUrls: ['./about-me-page.component.css']
})

export class AboutComponent implements OnInit {
  constructor(
    private imagesService: ImagesService,
    private aboutMeService: AboutMeService,
    private githubApiService: GitHubApiService,
    private router: Router,
    private elRef: ElementRef) { }
    
    aboutResponse!: AboutInformationResponse
    imageURL: string = '../../../assets/assets/img/600x600.jpg';
    languageStats!: LanguageStats[];
    isDataLoaded = false;

  ngOnInit(): void {
    this.initAos();
    this.getAboutImage();
    this.getAboutInformation();
    this.timeOut();
  }
  
  private timeOut() {
    setTimeout(() => {
      this.getLanguageStats();
      this.initWaypoint();
      this.isDataLoaded = true;
    }, 100)
  }

  private initWaypoint(): void {
    const skillsContent = this.elRef.nativeElement.querySelector('.skills-content');
    if (skillsContent) {
      new Waypoint({
        element: skillsContent,
        offset: '80%',
        handler: (direction: string) => {
          const progressBars = this.elRef.nativeElement.querySelectorAll('.progress .progress-bar');
          progressBars.forEach((bar: HTMLElement, index: number) => {
            const percentage = this.languageStats[index]?.percentageOfUseLanguage || 0;
            bar.style.width = percentage + '%';
          });
        }
      });
    }
  }

  private initAos(): void {
    AOS.init({
      duration: 1000,
      easing: 'ease-in-out',
      once: true,
      mirror: false
    });
  }

  getAboutInformation(): void {
    this.aboutMeService.getAboutUsersInformation().subscribe({
      next: (response) => {
        if (response) {
          this.aboutResponse = response;
        }
      },
      error: () => {
        this.router.navigate(['about', 'add']);
      }
    });
  }

  getAboutImage(): void {
    this.imagesService.getAboutUserImage().subscribe({
      next: (response) => {
        this.imageURL = response.imageUrl;
        console.log(response.imageUrl);
      },
      error: (error) => {
         console.log(error.error);
      }
    });
  }

  getLanguageStats(): void {
    this.githubApiService.getLanguagesPercentageOfUse().subscribe({
      next: (response) => {
        if (response) {
          this.languageStats = response;
        }
      },
      error: (error) => {
        console.log(error.status, error.error)
      }
    });
  }
}
