import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { AccountsService } from "./accounts.service";
import { AuthHelperService } from "./auth-helper.service";
@Injectable({
    providedIn: 'root',
})

export class AuthGuard implements CanActivate {

    constructor(
        public accountsService: AccountsService,
        public router: Router,
        public autheHelperService: AuthHelperService
    ) { }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> | Promise<boolean> | boolean {
        if (!this.autheHelperService.getAccessToken()) {
            this.router.navigate(['/login']);
            return false;
        }
        return true;
    }
}
