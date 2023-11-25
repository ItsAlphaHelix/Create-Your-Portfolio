import { ChangeDetectorRef, Component, ElementRef, HostListener, OnInit, Renderer2 } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ProjectImageResoponse } from 'src/app/models/project-images-response-model';
import { ImagesService } from 'src/app/services/images.service';
import { ProjectsService } from 'src/app/services/projects.service';

@Component({
  selector: 'app-project-page',
  templateUrl: './project-page.component.html',
  styleUrls: ['./project-page.component.css']
})

export default class ProjectComponent implements OnInit {

  constructor(
    private imagesService: ImagesService,
    private spinner: NgxSpinnerService,
    private route: ActivatedRoute,
    private elRef: ElementRef,
    private renderer: Renderer2,
    private router: Router) { }

  projectImageResponse!: ProjectImageResoponse[];

  ngOnInit(): void {
    this.getProjectImages();
  }

  openFileInput(): void {
    const fileInput = generateFileInput();
    fileInput.addEventListener('change', (event) => this.uploadProjectImage(event));
    document.body.appendChild(fileInput);
    fileInput.click();
    document.body.removeChild(fileInput);
  }

  openFileInputUpdateImage(projectId: Number) {
    const fileInput = generateFileInput();
    fileInput.addEventListener('change', (event) => this.updateImage(event, projectId));
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
    return this.imagesService
  }


  updateImage(event: Event, projectId: Number): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      const file = target.files[0];
      this.spinner.show();
      this.imagesService.updateProjectMainImage(file, projectId).subscribe(
        (response) => {
          if (response) {
            this.spinner.hide();
            this.getProjectImages();
            return response.imageUrl;
          }
        }
      );
    }
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
    this.renderer.addClass(anchorImage, 'portfolio-lightbox');
    this.renderer.setAttribute(anchorImage, 'title', 'Edit Image');

    const iBxPlusElement = this.renderer.createElement('i');
    this.renderer.addClass(iBxPlusElement, 'bx');
    this.renderer.addClass(iBxPlusElement, 'bxs-edit');

    anchorImage.addEventListener('click', () => {
      
      this.openFileInputUpdateImage(response.projectId);
      this.getProjectImages();
      divPortfolioItem.remove();
    });

    const anchorPortfolioDetails = this.renderer.createElement('a');
    this.renderer.addClass(anchorPortfolioDetails, 'routerlink');
    anchorPortfolioDetails.addEventListener('click', () => {
      this.router.navigate(['/projects', 'add', response.projectId]);
    });
    this.renderer.setAttribute(anchorPortfolioDetails, 'routerLink', `projects/add/${response.projectId}`);
    this.renderer.setAttribute(anchorPortfolioDetails, 'title', 'Add Project');

    const iBxLink = this.renderer.createElement('i');
    this.renderer.addClass(iBxLink, 'bx');
    this.renderer.addClass(iBxLink, 'bx-plus');

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
 function generateFileInput() {
  const fileInput = document.createElement('input');
  fileInput.type = 'file';
  fileInput.accept = 'image/*';
  fileInput.style.display = 'none';
  return fileInput;
}

