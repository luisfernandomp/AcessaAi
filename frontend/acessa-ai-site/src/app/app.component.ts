import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TuiRoot } from '@taiga-ui/core';
import { HeaderComponent } from './pages/components/header/header.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [HeaderComponent, RouterOutlet, TuiRoot],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {}
