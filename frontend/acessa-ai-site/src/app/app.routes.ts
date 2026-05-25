import { Routes } from '@angular/router';
import { MapaComponent } from './pages/mapa/mapa.component';
import { LoginComponent } from './pages/login/login.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'mapa', component: MapaComponent },
  { path: 'login', component: LoginComponent },
  { path: '**', redirectTo: 'login' },
];
