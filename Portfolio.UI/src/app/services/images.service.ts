import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import * as routes from 'src/app/shared/routes.contants';

@Injectable({
    providedIn: 'root'
})

export class ImagesService {
    constructor(private http: HttpClient) { }

    private getJwtToken() {
        return sessionStorage.getItem('accessToken') || '';
    }

    private getUserId() {
        return sessionStorage.getItem('userId') || '';
    }


    uploadProfileImage(file: File): Observable<any> {
        const jwtToken = this.getJwtToken();
        // Add the token to the request headers
        const headers = new HttpHeaders()
            .set('Authorization', `Bearer ${jwtToken}`);

        const formData = new FormData();
        formData.append('file', file);

        return this.http.post(routes.UPLOAD_PROFILE_IMAGE_ENDPOINT, formData, { headers });
    }

    uploadHomePageImage(file: File): Observable<any> {
        const jwtToken = this.getJwtToken();
        // Add the token to the request headers
        const headers = new HttpHeaders()
            .set('Authorization', `Bearer ${jwtToken}`);

        debugger
        const formData = new FormData();
        formData.append('file', file);

        return this.http.post(routes.UPLOAD_HOME_PAGE_IMAGE_ENDPOINT, formData, { headers });
    }

    getUserProfileImage(): Observable<any> {

        const jwtToken = this.getJwtToken();
        const userId = this.getUserId();

        const headers = new HttpHeaders()
            .set('Authorization', `Bearer ${jwtToken}`)
        return this.http.get<{ imageUrl: string }>(routes.GET_PROFILE_IMAGE_ENDPOINT + `${userId}`, { headers });
    }

    getUserHomePageImage(): Observable<any> {
        const jwtToken = this.getJwtToken();
        const userId = this.getUserId();

        const headers = new HttpHeaders()
            .set('Authorization', `Bearer ${jwtToken}`)
        return this.http.get<{ imageUrl: string }>(routes.GET_HOME_PAGE_IMAGE_ENDPOINT + `${userId}`, { headers });
    }

    uploadAboutImage(file: File): Observable<any> {
        const jwtToken = this.getJwtToken();
        const userId = this.getUserId();

        const headers = new HttpHeaders()
            .set('Authorization', `Bearer ${jwtToken}`);
        const params = { userId }

        debugger
        const formData = new FormData();
        formData.append('file', file);

        return this.http.post(routes.UPLOAD_ABOUT_IMAGE_ENDPOINT, formData, { headers, params });
    }

    getAboutUserImage(): Observable<any> {
        const jwtToken = this.getJwtToken();
        const userId = this.getUserId();

        const headers = new HttpHeaders()
            .set('Authorization', `Bearer ${jwtToken}`)
        return this.http.get<{ imageUrl: string }>(routes.GET_ABOUT_IMAGE_ENDPOINT + `${userId}`, { headers });
    }
}