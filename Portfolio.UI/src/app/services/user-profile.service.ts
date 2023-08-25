import { HttpClient, HttpEventType, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import * as routes from 'src/app/shared/routes.contants';
import { environment } from 'src/environments/environment';
import { UserResponse } from '../models/user-response-model';
import { PersonalizeAboutUserRequest } from '../models/personalize-about-user-request';
import { AboutUserResponse } from '../models/about-user-response-model';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {
  private getJwtToken() {
    return sessionStorage.getItem('accessToken') || '';
  }

  private getUserId() {
    return sessionStorage.getItem('userId') || '';
  }

  constructor(private http: HttpClient) { }


  uploadUserProfilePicture(file: File): Observable<any> {
    const jwtToken = this.getJwtToken();
    // Add the token to the request headers
    const headers = new HttpHeaders()
      .set('Authorization', `Bearer ${jwtToken}`);

    const formData = new FormData();
    formData.append('file', file);

    return this.http.post(routes.UPLOAD_PROFILE_IMAGE_ENDPOINT, formData, { headers });
  }

  uploadUserHomePagePicture(file: File): Observable<any> {
    const jwtToken = this.getJwtToken();
    // Add the token to the request headers
    const headers = new HttpHeaders()
      .set('Authorization', `Bearer ${jwtToken}`);

    debugger
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post(routes.UPLOAD_HOME_PAGE_IMAGE_ENDPOINT, formData, { headers });
  }

  getUserProfilePicture(): Observable<any> {

    const jwtToken = this.getJwtToken();
    const userId = this.getUserId();

    const api = `${environment.baseUrlApi}/api/users-profile/get-profile-image/${userId}`

    const headers = new HttpHeaders()
      .set('Authorization', `Bearer ${jwtToken}`)
    return this.http.get<{ imageUrl: string }>(api, { headers });
  }

  getUserHomePagePicture(): Observable<any> {
    const jwtToken = this.getJwtToken();
    const userId = this.getUserId();

    const api = `${environment.baseUrlApi}/api/users-profile/get-home-page-image/${userId}`

    const headers = new HttpHeaders()
      .set('Authorization', `Bearer ${jwtToken}`)
    return this.http.get<{ imageUrl: string }>(api, { headers });
  }

  personalizeAboutUser(request: PersonalizeAboutUserRequest): Observable<AboutUserResponse> {
    const jwtToken = this.getJwtToken();
    const userId = this.getUserId();

    const params = { userId };
    const headers = new HttpHeaders()
      .set('Authorization', `Bearer ${jwtToken}`)

    return this.http.post<AboutUserResponse>(routes.PERSONALIZE_ABOUT_USER_ENDPOINT, request, { headers, params });
  }

  getAboutUser(): Observable<AboutUserResponse> {
    const userId = this.getUserId();
    const jwtToken = this.getJwtToken();

    const params = { userId }
    const headers = new HttpHeaders()
      .set('Authorization', `Bearer ${jwtToken}`)

    return this.http.get<AboutUserResponse>(routes.GET_ABOUT_USER_ENDPOINT, { headers, params });
  }
}
