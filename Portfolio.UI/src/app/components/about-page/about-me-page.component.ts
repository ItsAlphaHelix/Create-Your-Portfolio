import { AfterContentChecked, AfterViewChecked, AfterViewInit, Component, ElementRef, HostListener } from '@angular/core';
import { ImagesService } from 'src/app/services/images.service';
import { AboutMeService } from 'src/app/services/about-me.service';
import { OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AboutInformationResponse } from 'src/app/models/about-response-model';
import * as AOS from 'aos';
import { LanguageStats } from 'src/app/models/language-stats-model';
import { GitHubApiService } from 'src/app/services/github-api.service';

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

  ngOnInit(): void {
    this.initAos();
    this.getAboutImage();
    this.getAboutUser();

    setTimeout(() => {
      this.getLanguagePercentages();
      this.initWaypoint();
    }, 100)
  }

  aboutResponse!: AboutInformationResponse
  imageURL!: string | undefined
  languagePercentages!: LanguageStats[];

  private initWaypoint(): void {
    const skillsContent = this.elRef.nativeElement.querySelector('.skills-content');
    if (skillsContent) {
      new Waypoint({
        element: skillsContent,
        offset: '80%',
        handler: (direction: string) => {
          const progressBars = this.elRef.nativeElement.querySelectorAll('.progress .progress-bar');
          progressBars.forEach((bar: HTMLElement, index: number) => {
            const percentage = this.languagePercentages[index]?.percentageOfUseLanguage || 0;
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

  getAboutUser(): void {
    this.aboutMeService.getAboutUsersInformationAsync().subscribe({
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
      }
    });
  }

  getFromGithubRepositoryLanguages() {
    this.githubApiService.getGitHubRepositoryLanguages().subscribe();
  }

  getLanguagePercentages(): void {
    this.githubApiService.getLanguagesPercentageOfUse().subscribe({
      next: (response) => {
        if (response) {
          this.languagePercentages = response;
        }
      },
      error: (error) => {
        console.log(error.status, error.error)
      }
    });
  }
}
