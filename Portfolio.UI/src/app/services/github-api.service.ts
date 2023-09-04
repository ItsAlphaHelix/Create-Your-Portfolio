import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import * as routes from 'src/app/shared/routes.contants';
import { LanguageStats } from "../models/language-stats-model";
import { AuthHelperService } from "./auth-helper.service";

@Injectable({
    providedIn: 'root'
})

export class GitHubApiService {

    constructor(
        private http: HttpClient,
        private authHelperService: AuthHelperService) { }

    getGitHubRepositoryLanguages(): Observable<any> {

        return this.http.get(routes.GET_GITHUB_REPOSITORY_LANGUAGES_ENDPOINT, { params: this.authHelperService.getParams() });
    }

    getLanguagesPercentageOfUse(): Observable<LanguageStats[]> {
        
        return this.http.get<LanguageStats[]>(routes.GET_LANGUAGE_PERCENTAGES_ENDPOINT, { params: this.authHelperService.getParams() })
    }
}