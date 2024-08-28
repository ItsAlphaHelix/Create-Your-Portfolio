import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { ProjectRequest } from 'src/app/models/project-request-model';
import { ProjectResponse } from 'src/app/models/project-response.model';
import { ImagesService } from 'src/app/services/images.service';
import { ProjectsService } from 'src/app/services/projects.service';

@Component({
  selector: 'app-edit-project-page',
  templateUrl: './edit-project-page.component.html',
  styleUrls: ['./edit-project-page.component.css']
})
export class EditProjectPageComponent implements OnInit {

  constructor(
    private projectService: ProjectsService,
    private route: ActivatedRoute,
    private router: Router,
    private imageService: ImagesService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,) {}

  ngOnInit(): void {
    this.getProjectForEdit();
  }
  
  projectFormResponse!: ProjectResponse
  projectId = Number(this.route.snapshot.paramMap.get('projectId'));

  editForm = new FormGroup({
    id: new FormControl(""),
    name: new FormControl(""),
    category: new FormControl(""),
    environment: new FormControl(""),
    deploymentUrl: new FormControl(""),
    gitHubUrl: new FormControl(""),
    description: new FormControl(""),
  });

  getProjectForEdit(): void {
    this.projectService.getProjectById(this.projectId).subscribe({
      next: (response) => {
        if (response) {
          this.editForm.setValue({
            id: response.id,
            name: response.name,
            category: response.category,
            environment: response.environment,
            deploymentUrl: response.deploymentUrl,
            gitHubUrl: response.gitHubUrl,
            description: response.description
          });
        }
      },
      error: () => {
      }
    });
  }

  updateImage(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      const file = target.files[0];
      this.spinner.show();
      this.imageService.updateProjectDetailsImage(file, this.projectId).subscribe({
        next: (response) => {
          if (response) {
            this.spinner.hide();
            this.toastr.success('You have successfully updated your project details image.')
            return response.imageUrl;
          }
        },
        error: () => {
          this.spinner.hide();
          this.toastr.error('Invalid image type.');
        }
    });
    }
  }

  editProject(): void {

    // if (this.editForm.invalid) {
    //   this.clientSideValidation.aboutUserFormValidation(this.editForm);
    //   return;
    // }

    this.projectService.editProjectInformation(this.editForm).subscribe({
      next: (id) => {
        if (id) {
          this.toastr.success('You have been successfully edit your information.')
          this.router.navigate(['/project', 'details', id])
        }
      },
      error: (error) => {
        if (error.status == 400) {
          this.toastr.error('The project name, category, and description fields are required.')
        }
      }
    });
  }
}
