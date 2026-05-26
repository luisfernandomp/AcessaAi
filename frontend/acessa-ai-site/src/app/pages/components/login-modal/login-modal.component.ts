import { Component, EventEmitter, Output, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../core/services/auth.service';
import { LoginRequest } from '../../../core/models/auth.model';

@Component({
  selector: 'app-login-modal',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login-modal.component.html',
  styleUrl: './login-modal.component.css',
})
export class LoginModalComponent {
  @Output() fechar = new EventEmitter<void>();
  @Output() irParaRegister = new EventEmitter<void>();

  private readonly toastr = inject(ToastrService);

  carregando = false;
  mostrarSenha = false;

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    senha: ['', Validators.required],
  });

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
  ) {}

  submit(): void {
    if (this.form.invalid) {
      this.toastr.warning('Por favor, preencha seu e-mail e senha corretamente.');
      return;
    }
    this.carregando = true;
    this.authService.login(this.form.value as LoginRequest).subscribe({
      next: () => {
        this.carregando = false;
        this.fechar.emit();
        this.router.navigate(['/mapa']);
      },
      error: () => {
        this.carregando = false;
        this.toastr.error('Não foi possível efetuar o login. Verifique suas credenciais.');
      },
    });
  }

  fecharModal(): void {
    this.fechar.emit();
  }

  onBackdropClick(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('modal-backdrop')) {
      this.fecharModal();
    }
  }
}
