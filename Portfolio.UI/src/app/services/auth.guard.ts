import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { AccountsService } from "./accounts.service";
@Injectable({
    providedIn: 'root',
})

export class AuthGuard implements CanActivate {

    constructor(
        public accountsService: AccountsService,
        public router: Router
    ) { }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> | Promise<boolean> | boolean {
        if (!this.accountsService.getAccessToken()) {
            this.router.navigate(['/login']);
            return false;
        }
        return true;
    }
}
