import { Component } from '@angular/core';

@Component({
  selector: 'app-footer-page',
  templateUrl: './footer-page.component.html',
  styleUrls: ['./footer-page.component.css']
})
export class FooterComponent {

  currentYearLong(): Number {
    return new Date().getFullYear();
  }
}  

