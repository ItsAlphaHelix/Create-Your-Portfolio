import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserProfileService } from 'src/app/services/user-profile.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  //imageURL: string = '\\assets\\img\\hero-bg.jpg';

  constructor(private userProfileService: UserProfileService) {}

  ngOnInit(): void {
  }

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  openFileInput(): void {
    const fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.accept = 'image/*';
    fileInput.style.display = 'none';
    fileInput.addEventListener('change', (event) => this.handleFileInputChange(event));
    document.body.appendChild(fileInput);
    fileInput.click();
    document.body.removeChild(fileInput);
  }

  handleFileInputChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      const file = target.files[0];
      this.userProfileService.uploadUserHomePagePicture(file).subscribe(
        (response) => {
          if (response) {
            const heroClass = document.getElementById('#hero');
            if (heroClass) {
              heroClass.style.background = `url(${response.imageUrl}),  top center`;
            }
          }
        }
      );
    }
  }
}
