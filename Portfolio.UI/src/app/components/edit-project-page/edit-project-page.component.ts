import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ProjectRequest } from 'src/app/models/project-request-model';
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
    private imageService: ImagesService,
    private spinner: NgxSpinnerService) {}

  ngOnInit(): void {
    this.getProjectForEdit();
  }
  
  projectFormRequest!: ProjectRequest

  editForm = new FormGroup({
    name: new FormControl(""),
    category: new FormControl(""),
    environment: new FormControl(""),
    deploymentUrl: new FormControl(""),
    gitHubUrl: new FormControl(""),
    description: new FormControl(""),
  });

  getProjectForEdit(): void {

    const projectId = this.route.snapshot.paramMap.get('projectId');
    this.projectService.getProjectById(Number(projectId)).subscribe({
      next: (response) => {
        if (response) {
          this.editForm.setValue({
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
      this.imageService.updateAboutImage(file).subscribe(
        (response) => {
          if (response) {
            this.spinner.hide();
            return response.imageUrl;
          }
        }
      );
    }
  }
}
