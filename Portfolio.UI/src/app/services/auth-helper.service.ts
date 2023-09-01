import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})

export class AuthHelperService {

    public getJwtToken() {
        return sessionStorage.getItem('accessToken') || '';
    }

    public getUserId() {
        return sessionStorage.getItem('userId') || '';
    }
}