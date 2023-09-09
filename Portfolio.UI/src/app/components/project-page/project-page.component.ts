import { ChangeDetectorRef, Component, ElementRef, HostListener, OnInit, Renderer2 } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ProjectImageResoponse } from 'src/app/models/project-images-response-model';
import { UserProject } from 'src/app/models/user-project.model';
import { ImagesService } from 'src/app/services/images.service';
import { ProjectsService } from 'src/app/services/projects.service';

@Component({
  selector: 'app-project-page',
  templateUrl: './project-page.component.html',
  styleUrls: ['./project-page.component.css']
})
export class ProjectComponent implements OnInit{

  constructor(
    private imagesService: ImagesService,
    private spinner: NgxSpinnerService,
    private projectService: ProjectsService,
    private route: ActivatedRoute,
    private elRef: ElementRef,
    private renderer: Renderer2,
    private router: Router) { }

  projectImageResponse!: ProjectImageResoponse[];
  projectId!: Number;

  ngOnInit(): void {
    this.getProjectImages();
    this.projectId = Number(this.route.snapshot.paramMap.get('projectId'));
  }

  openFileInput(): void {
    const fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.accept = 'image/*';
    fileInput.style.display = 'none';
    fileInput.addEventListener('change', (event) => this.uploadProjectImage(event));
    document.body.appendChild(fileInput);
    fileInput.click();
    document.body.removeChild(fileInput);
  }

  @HostListener('document:click', ['$event'])
   handleClick(event: Event): void {
    if (event.target instanceof HTMLAnchorElement) {
      const element = event.target as HTMLAnchorElement;
      if (element.className === 'routerlink') {
        event.preventDefault();
        const route = element?.getAttribute('href');
        if (route) {
          this.router.navigate([`/${route}`]);
        }
      }
    }
  }

  uploadProjectImage(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      const file = target.files[0];
      this.spinner.show();
      this.imagesService.uploadMainProjectImage(file).subscribe(
        (response) => {
          if (response) {
            this.createProjectDinamically(response);
            console.log(response);
          }
        }
      );
    }
  }

  getProjectImages() {
    this.imagesService.getAllProjectImages().subscribe(
      (response) => {
        this.projectImageResponse = response;
      }
    );
  }

  private createProjectDinamically(response: any) {
    const divPortfolioContainer = this.elRef.nativeElement.querySelector('.portfolio-container');
    const divPortfolioItem = this.renderer.createElement('div');

    this.renderer.addClass(divPortfolioItem, 'col-lg-4');
    this.renderer.addClass(divPortfolioItem, 'col-md-6');
    this.renderer.addClass(divPortfolioItem, 'portfolio-item');
    this.renderer.addClass(divPortfolioItem, 'filter-app');

    const divPortfolioWrap = this.renderer.createElement('div');
    this.renderer.addClass(divPortfolioWrap, 'portfolio-wrap');

    const mainImg = this.renderer.createElement('img');
    this.renderer.addClass(mainImg, 'img-fluid');
    this.renderer.setAttribute(mainImg, 'src', `${response.imageUrl}`);

    const divPortfolioLinks = this.renderer.createElement('div');
    this.renderer.addClass(divPortfolioLinks, 'portfolio-links');
    const anchorImage = this.renderer.createElement('a');
    this.renderer.setAttribute(anchorImage, 'href', `${response.imageUrl}`);
    this.renderer.setAttribute(anchorImage, 'data-gallery', 'portfolioGallery');
    this.renderer.addClass(anchorImage, 'portfolio-lightbox');
    this.renderer.setAttribute(anchorImage, 'title', 'Zoom');

    const iBxPlusElement = this.renderer.createElement('i');
    this.renderer.addClass(iBxPlusElement, 'bx');
    this.renderer.addClass(iBxPlusElement, 'bx-plus');

    const anchorPortfolioDetails = this.renderer.createElement('a');
    this.renderer.addClass(anchorPortfolioDetails, 'routerlink');
    this.renderer.setAttribute(anchorPortfolioDetails, 'href', `projects/add/34`);
    this.renderer.setAttribute(anchorPortfolioDetails, 'title', 'More Details');
    
    const iBxLink = this.renderer.createElement('i');
    this.renderer.addClass(iBxLink, 'bx');
    this.renderer.addClass(iBxLink, 'bx-link');

    anchorPortfolioDetails.appendChild(iBxLink);
    anchorImage.appendChild(iBxPlusElement);
    divPortfolioLinks.appendChild(anchorImage);
    divPortfolioLinks.appendChild(anchorPortfolioDetails);
    divPortfolioWrap.appendChild(mainImg);
    divPortfolioWrap.appendChild(divPortfolioLinks);
    divPortfolioItem.appendChild(divPortfolioWrap);
    divPortfolioContainer.appendChild(divPortfolioItem);
    this.spinner.hide();
  }
}
