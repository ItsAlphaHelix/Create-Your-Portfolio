import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AboutInformationRequest } from "../models/about-request-model";
import { Observable, map } from "rxjs";
import { AboutInformationResponse } from "../models/about-response-model";
import * as routes from 'src/app/shared/routes.contants';
import { FormGroup } from "@angular/forms";
import { LanguagePercentage } from "../models/language-percentages-model";

@Injectable({
    providedIn: 'root'
})

export class AboutMeService {

    constructor(private http: HttpClient) { }

    private getJwtToken() {
        return sessionStorage.getItem('accessToken') || '';
    }

    private getUserId() {
        return sessionStorage.getItem('userId') || '';
    }

    addAboutUsersInformation(request: AboutInformationRequest): Observable<AboutInformationResponse> {
        const jwtToken = this.getJwtToken();
        const userId = this.getUserId();

        const params = { userId };
        const headers = new HttpHeaders()
            .set('Authorization', `Bearer ${jwtToken}`)

        return this.http.post<AboutInformationResponse>(routes.ADD_ABOUT_INFORMATION_ENDPOINT, request, { headers, params });
    }

    getAboutUsersInformationAsync(): Observable<AboutInformationResponse> {
        const userId = this.getUserId();
        const jwtToken = this.getJwtToken();

        const params = { userId }
        const headers = new HttpHeaders()
            .set('Authorization', `Bearer ${jwtToken}`)

        return this.http.get<AboutInformationResponse>(routes.GET_ABOUT_ENDPOINT, { headers, params });
    }

    getEditUsersAboutInformationAsync(aboutId: number): Observable<AboutInformationResponse> {
        const userId = this.getUserId();
        const jwtToken = this.getJwtToken();

        const params = { userId }
        const headers = new HttpHeaders()
            .set('Authorization', `Bearer ${jwtToken}`)

        return this.http.get<AboutInformationResponse>(routes.GET_EDIT_ABOUT_ENDPOINT + `${aboutId}`);
    }

    editAboutUsersInformationAsync(editForm: FormGroup): Observable<string> {
        const userId = this.getUserId();
        const jwtToken = this.getJwtToken();

        //const params = { aboutId }
        const headers = new HttpHeaders()
            .set('Authorization', `Bearer ${jwtToken}`)

        return this.http.put<AboutInformationResponse>(routes.EDIT_ABOUT_ENDPOINT, editForm.value, { headers })
            .pipe(
                map((response) => response.id)
            );
    }
    
    getLanguagesPercentageOfUse(): Observable<LanguagePercentage[]> {
        const userId = this.getUserId();
        const params = { userId };

        return this.http.get<LanguagePercentage[]>(routes.GET_LANGUAGE_PERCENTAGES, { params })
    }
}