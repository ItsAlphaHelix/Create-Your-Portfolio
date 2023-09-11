import { Component, ElementRef, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserProject } from 'src/app/models/user-project.model';
import { ImagesService } from 'src/app/services/images.service';
import { ProjectsService } from 'src/app/services/projects.service';


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
     private elRef: ElementRef,
     private imagesService: ImagesService) {
  }

  projectResponse: UserProject | undefined;
  imageUrl!: string;

  ngOnInit(): void {
    this.getProject();
    this.getImage();
  }

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

  getImage() {
    debugger;
    const projectId = this.route.snapshot.paramMap.get('projectId')!;
    this.imagesService.getProjectDetailsImage(projectId).subscribe({
      next: (response) => {
        this.imageUrl = response.imageUrl;
      }
    });
  }
}
