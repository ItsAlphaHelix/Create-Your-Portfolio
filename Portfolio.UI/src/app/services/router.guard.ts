import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { AccountsService } from "./accounts.service";
import { AboutMeService } from "./about-me.service";
@Injectable({
    providedIn: 'root',
})

export class RouterGuard implements CanActivate {

    constructor(
        public aboutMeService: AboutMeService,
        public router: Router
    ) { }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> | Promise<boolean> | boolean {
        if (this.aboutMeService.getAboutUsersInformationAsync()) {
            this.router.navigate(['/']);
            return false;
        }       
        return true;
    }
}