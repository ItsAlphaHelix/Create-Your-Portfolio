import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserProject } from 'src/app/models/user-project.model';
import { ProjectsService } from 'src/app/services/projects.service';

@Component({
  selector: 'app-add-project-page',
  templateUrl: './add-project-page.component.html',
  styleUrls: ['./add-project-page.component.css']
})
export class AddProjectComponent {

  constructor(
    private projectService: ProjectsService,
    private route: ActivatedRoute,
    private router: Router) {}

    projectForm = new FormGroup({
    name: new FormControl("", Validators.required),
    category: new FormControl("", Validators.required),
    environment: new FormControl("", Validators.required),
    deploymentUrl: new FormControl("", Validators.required),
    gitHubUrl: new FormControl("", Validators.required),
    description: new FormControl("", Validators.required),
  });

  projectFormRequest!: UserProject

  addProject() {
    debugger
    const projectId = this.route.snapshot.paramMap.get('projectId');
    this.projectFormRequest = {
      name: this.projectForm.value.name!,
      category: this.projectForm.value.category!,
      environment: this.projectForm.value.environment!,
      deploymentUrl: this.projectForm.value.deploymentUrl!,
      gitHubUrl: this.projectForm.value.gitHubUrl!,
      description: this.projectForm.value.description!
    }
    this.projectService.addProject(this.projectFormRequest, Number(projectId)).subscribe({
      next: (response) => {
        this.router.navigate(['project', 'details', projectId]);
      },
      error: (error) => {
        console.log(error.error);
      }
    });
  }
}
