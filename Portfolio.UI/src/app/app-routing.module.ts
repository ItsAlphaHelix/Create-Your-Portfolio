import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationComponent } from './components/authentication-page/authentication.component';
import { AppComponent } from './app.component';
import { AuthGuard } from './services/guards/auth.guard';

const routes: Routes = [
  {path: "", component: AppComponent, canActivate: [AuthGuard]},
  { path: "authentication", component: AuthenticationComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
