import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './services/auth.guard';
import HomeComponent from './components/home-page/home-page.component';
import { RegisterComponent } from './components/register-page/register-page.component';
import { LoginComponent } from './components/login-page/login-page.component';
import { ForgottenPasswordComponent } from './components/forgotten-password-page/forgotten-password-page.component';
import { ErrorPageComponent } from './components/error-page/error-page.component';
import { AboutComponent } from './components/about-page/about-me-page.component';
import { EditAboutPageComponent } from './components/edit-about-page/edit-about-page.component';
import { AddAboutInformationComponent } from './components/add-about-page/add-about-page.component';
import ProjectComponent from './components/project-page/project-page.component';
import { AddProjectComponent } from './components/add-project-page/add-project-page.component';
import { ProjectDetailsComponent } from './components/project-details-page/project-details-page.component';
import { EditProjectPageComponent } from './components/edit-project-page/edit-project-page.component';

const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'home', redirectTo: '/', pathMatch: 'full' },
  { path: 'about', component: AboutComponent, canActivate: [AuthGuard] },
  { path: 'about/edit/:aboutId', component: EditAboutPageComponent, canActivate: [AuthGuard] },
  { path: 'about/add', component: AddAboutInformationComponent, canActivate: [AuthGuard] },
  { path: 'projects', component: ProjectComponent, canActivate: [AuthGuard] },
  { path: 'projects/:projectId', component: ProjectComponent, canActivate: [AuthGuard] },
  { path: 'projects/add/:projectId', component: AddProjectComponent, canActivate: [AuthGuard] },
  { path: 'project/details/:projectId', component: ProjectDetailsComponent, canActivate: [AuthGuard] },
  { path: 'project/edit/:projectId', component: EditProjectPageComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'forgot-password', component: ForgottenPasswordComponent },
  { path: 'page-not-found', component: ErrorPageComponent },
  { path: '**', redirectTo: 'page-not-found', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })], //{ useHash: true }
  exports: [RouterModule]
})
export class AppRoutingModule { }
