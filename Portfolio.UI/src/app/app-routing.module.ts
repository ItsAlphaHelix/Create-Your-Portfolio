import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationComponent } from './components/authentication-page/authentication.component';
import { AuthGuard } from './services/guards/auth.guard';
import { HomeComponent } from './components/home-page/home.component';
import { RegisterComponent } from './components/register-page/register.component';
import { LoginComponent } from './components/login-page/login.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] }, 
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'authentication', component: AuthenticationComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
