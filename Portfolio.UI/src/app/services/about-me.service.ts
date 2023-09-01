import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AboutInformationRequest } from "../models/about-request-model";
import { Observable, catchError, map, throwError } from "rxjs";
import { AboutInformationResponse } from "../models/about-response-model";
import * as routes from 'src/app/shared/routes.contants';
import { FormGroup } from "@angular/forms";
import { AuthHelperService } from "./auth-helper.service";

@Injectable({
    providedIn: 'root'
})

export class AboutMeService {

    constructor(
        private http: HttpClient,
        private authHelperService: AuthHelperService) { }


    private jwtToken = this.authHelperService.getJwtToken();
    private userId = this.authHelperService.getUserId();

    private headers = new HttpHeaders()
        .set('Authorization', `Bearer ${this.jwtToken}`)
    private params = { userId: this.userId };

    addAboutUsersInformation(request: AboutInformationRequest): Observable<AboutInformationResponse> {
        return this.http.post<AboutInformationResponse>(routes.ADD_ABOUT_INFORMATION_ENDPOINT, request,
            { headers: this.headers, params: this.params });
    }

    getAboutUsersInformationAsync(): Observable<AboutInformationResponse> {
        return this.http.get<AboutInformationResponse>(routes.GET_ABOUT_ENDPOINT, { headers: this.headers, params: this.params });
    }

    getEditUsersAboutInformationAsync(aboutId: number): Observable<AboutInformationResponse> {
        return this.http.get<AboutInformationResponse>(routes.GET_EDIT_ABOUT_ENDPOINT + `${aboutId}`);
    }

    editAboutUsersInformationAsync(editForm: FormGroup): Observable<string> {
        return this.http.put<AboutInformationResponse>(routes.EDIT_ABOUT_ENDPOINT, editForm.value, { headers: this.headers })
            .pipe(
                map((response) => response.id)
            );
    }
}