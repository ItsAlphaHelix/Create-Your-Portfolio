import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer-page',
  templateUrl: './footer-page.component.html',
  styleUrls: ['./footer-page.component.css']
})
export class FooterComponent implements OnInit{
  ngOnInit(): void {
  }

  currentYearLong(): Number {
    return new Date().getFullYear();
  }
}  

