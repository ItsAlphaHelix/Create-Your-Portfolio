import { Injectable } from "@angular/core";
import { CanActivate, Router} from "@angular/router";
import { Observable } from "rxjs";
import { AccountsService } from "../account/accounts.service";

@Injectable({
    providedIn: 'root',
})

export class AuthGuard implements CanActivate {
    
    constructor(
        public accountsService: AccountsService,
        public router: Router
    ) { }

    canActivate(
    ): Observable<boolean> | Promise<boolean> | boolean {
        if (!this.accountsService.getAccessToken()){
            this.router.navigate(['/authentication']);
            return false;
        }
        return true;
        }
    }
