import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { AuthHelperService } from './auth-helper.service';
import { Observable, map } from 'rxjs';
import * as routes from 'src/app/shared/routes.contants';
import { ProjectRequest } from '../models/project-request-model';
import { ProjectResponse } from '../models/project-response.model';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {

  constructor(
    private http: HttpClient,
    private authHelperService: AuthHelperService) { }

  addProject(request: ProjectRequest, projectId: Number): Observable<ProjectResponse> {
    return this.http.post<ProjectResponse>(routes.ADD_PROJECT_ENDPOINT + `${projectId}`, request);
  }

  getProjectById(projectId: Number): Observable<ProjectResponse> {

    return this.http.get<ProjectResponse>(routes.GET_PROJECT_BY_ID_ENDPOINT + `${projectId}`);
  }

  deleteProjectById(projectId: Number){
    return this.http.delete(routes.DELETE_PROJECT_ENDPOINT + `${projectId}`);
  }

  editProjectInformation(editForm: FormGroup): Observable<string> {
    return this.http.put<ProjectResponse>
      (routes.EDIT_PROJECT_INFORMATION_ENDPOINT, editForm.value, { headers: this.authHelperService.getHeaders() })
      .pipe(
        map((response) => response.id)
      );
  }
}
