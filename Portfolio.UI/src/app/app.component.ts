import { Component, OnInit } from '@angular/core';
import { AccountsService } from './services/accounts.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(
    private accountsService: AccountsService,
    private router: Router,) { }

  get isUserLoggedIn() {

    return this.accountsService.isLoggedIn;

  }

  onLogout() {
    this.accountsService.logout();
    this.router.navigate(['/login']);
  }
}
