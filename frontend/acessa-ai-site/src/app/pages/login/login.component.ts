import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { TuiButton, TuiIcon, TuiTextfield } from '@taiga-ui/core';
import { TuiPassword } from '@taiga-ui/kit';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../core/services/auth.service';
import { LoginRequest } from '../../core/models/auth.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, TuiTextfield, TuiButton, TuiIcon, TuiPassword],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  private readonly toastr = inject(ToastrService);

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    senha: ['', Validators.required],
  });

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
  ) {}

  submit(): void {
    if (this.loginForm.invalid) {
      this.toastr.warning('Por favor, preencha seu e-mail e senha corretamente.');
      return;
    }
    const request = this.loginForm.value as LoginRequest;
    this.authService.login(request).subscribe({
      next: () => this.router.navigate(['/mapa']),
      error: () =>
        this.toastr.error('Não foi possível efetuar o login. Verifique suas credenciais.'),
    });
  }
}
