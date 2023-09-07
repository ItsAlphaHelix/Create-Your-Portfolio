import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { AuthHelperService } from './auth-helper.service';
import { UserProject } from '../models/user-project.model';
import { Observable } from 'rxjs';
import * as routes from 'src/app/shared/routes.contants';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {

  constructor(
    private http: HttpClient,
    private authHelperService: AuthHelperService) { }

  addProject(request: UserProject, projectId: Number): Observable<UserProject> {
    return this.http.post<UserProject>(routes.ADD_PROJECT_ENDPOINT + `${projectId}`, request);
  }

  getProjectById(projectId: Number): Observable<UserProject> {

    return this.http.get<UserProject>(routes.GET_PROJECT_BY_ID_ENDPOINT + `${projectId}`);
  }
}
