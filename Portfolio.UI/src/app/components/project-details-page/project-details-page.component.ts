import { Component, ElementRef, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserProject } from 'src/app/models/user-project.model';
import { ProjectsService } from 'src/app/services/projects.service';

declare const Swiper: any;

@Component({
  selector: 'app-project-details-page',
  templateUrl: './project-details-page.component.html',
  styleUrls: ['./project-details-page.component.css']
})
export class ProjectDetailsComponent implements OnInit{

  constructor(
     private route: ActivatedRoute,
     private router: Router,
     private projectService: ProjectsService,
     private elRef: ElementRef) {
  }
  
  ngOnInit(): void {
    
    this.initImageSlider();
    this.getProject();
  }
  
  projectResponse: UserProject | undefined;

  private initImageSlider(): void {
    const portfolioDetails = this.elRef.nativeElement.querySelector('.portfolio-details-slider');
    const swiperPagination = this.elRef.nativeElement.querySelector('.swiper-pagination');

    new Swiper(portfolioDetails, {
      speed: 400,
      loop: true,
      autoplay: {
        delay: 5000,
        disableOnInteraction: false
      },
      pagination: {
        el: swiperPagination,
        type: 'bullets',
        clickable: true
      }
    });
  }

  // new Swiper('.testimonials-slider', {
  //   speed: 600,
  //   loop: true,
  //   autoplay: {
  //     delay: 5000,
  //     disableOnInteraction: false
  //   },
  //   slidesPerView: 'auto',
  //   pagination: {
  //     el: '.swiper-pagination',
  //     type: 'bullets',
  //     clickable: true
  //   },
  //   breakpoints: {
  //     320: {
  //       slidesPerView: 1,
  //       spaceBetween: 20
  //     },

  //     1200: {
  //       slidesPerView: 3,
  //       spaceBetween: 20
  //     }
  //   }
  // });

  getProject() {
    const projectId = this.route.snapshot.paramMap.get('projectId');
    this.projectService.getProjectById(Number(projectId)).subscribe({
      next: (response) => {
        if (response) {
          debugger;
          this.projectResponse = response;
        }
      }
    }
    );
  }
}
