import { Component, ElementRef, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserProject } from 'src/app/models/user-project.model';
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
     private elRef: ElementRef) {
  }
  
  ngOnInit(): void {
    
    this.getProject();
  }
  //  fileInput.setAttribute('multiple', 'multiple');
  projectResponse: UserProject | undefined;

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
