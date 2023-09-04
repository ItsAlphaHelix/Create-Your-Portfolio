import { HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})

export class AuthHelperService {

    private getJwtToken() {
        return sessionStorage.getItem('accessToken') || '';
    }

    private getUserId() {
        return sessionStorage.getItem('userId') || '';
    }

    public getParams() {
        const userId = this.getUserId();
        const params = { userId }

        return params;
    }

    public getHeaders() {
        const jwtToken = this.getJwtToken();
        const headers = new HttpHeaders()
            .set('Authorization', `Bearer ${jwtToken}`);

        return headers;
    }
}