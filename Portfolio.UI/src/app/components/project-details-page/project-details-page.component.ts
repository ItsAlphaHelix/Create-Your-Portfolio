import { Component, ElementRef, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectResponse } from 'src/app/models/project-response.model';
import { ImagesService } from 'src/app/services/images.service';
import { ProjectsService } from 'src/app/services/projects.service';


@Component({
  selector: 'app-project-details-page',
  templateUrl: './project-details-page.component.html',
  styleUrls: ['./project-details-page.component.css']
})
export class ProjectDetailsComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectsService,
    private elRef: ElementRef,
    private imagesService: ImagesService) {
  }

  projectResponse: ProjectResponse | undefined;
  imageUrl!: string;

  ngOnInit(): void {
    this.getProject();
  }

  getProject() {
    const projectId = this.route.snapshot.paramMap.get('projectId');
    this.projectService.getProjectById(Number(projectId)).subscribe({
      next: (response) => {
        if (response) {
          console.log(response)
          this.projectResponse = response;
        }
      }
    }
    );
  }
}
