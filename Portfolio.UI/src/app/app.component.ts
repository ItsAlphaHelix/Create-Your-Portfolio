import { Component } from '@angular/core';
import { AccountsService } from './services/account/accounts.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(
    private accountsService: AccountsService,
    private router: Router) {}
    
   get isUserLoggedIn() {
    
    return this.accountsService.isLoggedIn;
  
  }
}
