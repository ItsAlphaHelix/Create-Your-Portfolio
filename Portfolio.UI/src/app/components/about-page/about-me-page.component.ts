import { AfterViewInit, Component, ElementRef, HostListener } from '@angular/core';
import { ImagesService } from 'src/app/services/images.service';
import { AboutMeService } from 'src/app/services/about-me.service';
import { OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AboutInformationResponse } from 'src/app/models/about-response-model';
import * as AOS from 'aos';
//import { Waypoint} from 'waypoints/lib/noframework.waypoints.js';
import { LanguagePercentage } from 'src/app/models/language-percentages-model';
declare const Waypoint: any;

@Component({
  selector: 'app-about-page',
  templateUrl: './about-me-page.component.html',
  styleUrls: ['./about-me-page.component.css']
})

export class AboutComponent implements OnInit, AfterViewInit {
  constructor(
    private imagesService: ImagesService,
    private aboutMeService: AboutMeService,
    private router: Router,
    private elRef: ElementRef) {}

  ngAfterViewInit(): void {
  }
  
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
        if (response) {
          this.imageURL = response.imageUrl;
        }
      },
      error: (error) => {
      }
    });
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
            const percentage = this.languagePercentages[index]?.percentageOfUseLanguage || 0;
            bar.style.width = percentage + '%';
          });
        }
      });
    }
  }

  languagePercentages!: LanguagePercentage[];
  
  getLanguagePercentages(): void {
    this.aboutMeService.getLanguagesPercentageOfUse().subscribe({
      next: (response) => {
        if (response) {
          this.languagePercentages = response;
          console.log(this.languagePercentages)
        }
      },
      error: (error) => {
        console.log(error.status, error.error)
      }
    });
  }
}
