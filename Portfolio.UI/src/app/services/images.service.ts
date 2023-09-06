import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import * as routes from 'src/app/shared/routes.contants';
import { AuthHelperService } from "./auth-helper.service";
import { ProjectImageResoponse } from "../models/project-images-response-model";

@Injectable({
    providedIn: 'root'
})

export class ImagesService {
    constructor(
        private http: HttpClient,
        private authHelperService: AuthHelperService) { }

    uploadProfileImage(file: File): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);
        return this.http.post(routes.UPLOAD_PROFILE_IMAGE_ENDPOINT, formData, { params: this.authHelperService.getParams() });
    }

    uploadHomePageImage(file: File): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);

        return this.http.post(routes.UPLOAD_HOME_PAGE_IMAGE_ENDPOINT, formData, { params: this.authHelperService.getParams() });
    }

    getUserProfileImage(): Observable<any> {
        return this.http.get<{ imageUrl: string }>
        (routes.GET_PROFILE_IMAGE_ENDPOINT + `${this.authHelperService.getParams().userId}`, { headers: this.authHelperService.getHeaders() });
    }

    getUserHomePageImage(): Observable<any> {
        return this.http.get<{ imageUrl: string }>
        (routes.GET_HOME_PAGE_IMAGE_ENDPOINT + `${this.authHelperService.getParams().userId}`, { headers: this.authHelperService.getHeaders() });
    }

    uploadAboutImage(file: File): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);       

        return this.http.post(routes.UPLOAD_ABOUT_IMAGE_ENDPOINT, formData,
             { headers : this.authHelperService.getHeaders(), params: this.authHelperService.getParams() });
    }

    getAboutUserImage(): Observable<any> {
        return this.http.get<{ imageUrl: string }>(routes.GET_ABOUT_IMAGE_ENDPOINT + `${this.authHelperService.getParams().userId}`);
    }

    updateAboutImage(file: File): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);
        return this.http.put(routes.UPDATE_ABOUT_IMAGE_ENDPOINT, formData, { params: this.authHelperService.getParams() });
    }

    updateProfileImage(file: File): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);

        return this.http.put(routes.UPDATE_PROFILE_IMAGE_ENDPOINT, formData, { params: this.authHelperService.getParams() });
    }

    updateHomeImage(file: File): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);

        return this.http.put(routes.UPDATE_HOME_IMAGE_ENDPOINT, formData, { params: this.authHelperService.getParams() });
    }

    uploadMainProjectImage(file: File): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);  

        return this.http.post(routes.UPLOAD_MAIN_PROJECT_IMAGE_ENDPOINT, formData, { params: this.authHelperService.getParams() });
    }

    getAllProjectImages(): Observable<ProjectImageResoponse[]> {

        return this.http.get<ProjectImageResoponse[]>(routes.GET_ALL_PROJECT_IMAGES_ENDPOINT, { params: this.authHelperService.getParams() });
    }
}