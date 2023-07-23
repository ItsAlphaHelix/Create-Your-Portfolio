import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationComponent } from './components/authentication-page/authentication.component';
import { AuthGuard } from './services/guards/auth.guard';
import { HomeComponent } from './components/home/home.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] }, 
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'authentication', component: AuthenticationComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
