import { Component, OnInit } from '@angular/core';
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
     private projectService: ProjectsService) {
  }
  ngOnInit(): void {
    this.getProject();
  }

  projectResponse!: UserProject;

  getProject() {
    const projectId = this.route.snapshot.paramMap.get('projectId');
    debugger
    this.projectService.getProjectById(Number(projectId)).subscribe({
      next: (response) => {
        if (response) {
          this.projectResponse = response;
        }
      },
      error: () => {
        this.router.navigate(['projects', 'add', projectId]);
      }
    }
    );
  }
}
