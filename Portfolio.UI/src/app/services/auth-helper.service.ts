import { HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})

export class AuthHelperService {


    public getAccessToken() {
        const localStorageRememberMe = localStorage.getItem('remember_me');

            return localStorage.getItem('access_token') || '';
    }

    public getParams() {
        const userId = this.getUserId();
        const params = { userId }

        return params;
    }

    public getHeaders() {
        const jwtToken = this.getAccessToken();
        const headers = new HttpHeaders()
            .set('Authorization', `Bearer ${jwtToken}`);

        return headers;
    }
    
    private getUserId() {
        const localStorageRememberMe = localStorage.getItem('userId');

       return localStorage.getItem('userId') || '';
    }
}