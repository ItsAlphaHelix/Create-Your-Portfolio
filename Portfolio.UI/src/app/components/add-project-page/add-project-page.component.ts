import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { ProjectRequest } from 'src/app/models/project-request-model';
import { ImagesService } from 'src/app/services/images.service';
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
    private router: Router,
    private spinner: NgxSpinnerService,
    private imagesService: ImagesService,
    private toastr: ToastrService) { }

  projectForm = new FormGroup({
    name: new FormControl("", Validators.required),
    category: new FormControl("", Validators.required),
    environment: new FormControl("Production", Validators.required),
    deploymentUrl: new FormControl(""),
    gitHubUrl: new FormControl(""),
    description: new FormControl("", Validators.required),
  });

  projectFormRequest!: ProjectRequest

  addProject() {
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
        this.toastr.success('You have successfully added the project details.')
        this.router.navigate(['project', 'details', projectId]);
      },
      error: (error) => {
        if (error.status == 400) {
          this.toastr.error('The project name, category, and description fields are required.')
        }
      }
    });
  }

  uploadImage(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      const file = target.files[0];
      const projectId = this.route.snapshot.paramMap.get('projectId')!;
      this.spinner.show();
      this.imagesService.uploadProjectDetailsImage(file, projectId).subscribe({
        next: (response) => {
          if (response) {
            this.spinner.hide();
            return response.imageUrl;
          }
        },
        error: () => {
          this.spinner.hide();
          this.toastr.error('Invalid image type');
        }
    });
    }
  }
}
