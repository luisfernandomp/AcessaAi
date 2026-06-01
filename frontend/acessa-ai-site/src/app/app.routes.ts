import { Routes } from '@angular/router';
import { MapaComponent } from './pages/mapa/mapa.component';

export const routes: Routes = [
  { path: '', redirectTo: 'mapa', pathMatch: 'full' },
  { path: 'mapa', component: MapaComponent },
  { path: 'mapa/:idEstabelecimento', component: MapaComponent },
  { path: '**', redirectTo: 'mapa' },
];
