import { Injectable } from '@angular/core';
import { HttpClient } from '@aspnet/signalr';
import { AuthHelperService } from './auth-helper.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {

  constructor(
    private http: HttpClient,
    private authHelperService: AuthHelperService) { }

    
}
