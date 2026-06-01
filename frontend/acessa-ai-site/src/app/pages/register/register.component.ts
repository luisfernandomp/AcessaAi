import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UsuarioService } from '../../core/services/usuario.service';
import { CadastrarUsuarioRequest } from '../../core/models/usuario.model';
import { ESTADOS_BRASILEIROS } from '../../core/constants/estados';

function senhaValidator(control: AbstractControl): ValidationErrors | null {
  const v: string = control.value ?? '';
  const erros: ValidationErrors = {};
  if (v.length < 6) erros['minLength'] = true;
  if (!/[0-9]/.test(v)) erros['requireDigit'] = true;
  if (!/[A-Z]/.test(v)) erros['requireUppercase'] = true;
  return Object.keys(erros).length ? erros : null;
}

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  readonly estados = ESTADOS_BRASILEIROS;
  errorMessage = '';
  successMessage = '';

  registerForm = this.fb.group({
    nome: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    senha: ['', [Validators.required, senhaValidator]],
    dataNascimento: ['', Validators.required],
    endereco: this.fb.group({
      logradouro: ['', Validators.required],
      numero: ['', Validators.required],
      complemento: [''],
      bairro: ['', Validators.required],
      cidade: ['', Validators.required],
      uf: ['', Validators.required],
      cep: ['', Validators.required],
    }),
  });

  constructor(
    private fb: FormBuilder,
    private usuarioService: UsuarioService,
    private router: Router
  ) {}

  submit(): void {
    if (this.registerForm.invalid) {
      this.errorMessage = 'Por favor, preencha todos os campos obrigatórios.';
      this.successMessage = '';
      return;
    }

    this.errorMessage = '';
    const request = this.registerForm.value as CadastrarUsuarioRequest;
    this.usuarioService.cadastrar(request).subscribe({
      next: () => {
        this.successMessage = 'Cadastro realizado com sucesso. Faça login para continuar.';
        setTimeout(() => this.router.navigate(['/login']), 1200);
      },
      error: () => {
        this.errorMessage = 'Erro ao cadastrar. Verifique os dados e tente novamente.';
        this.successMessage = '';
      },
    });
  }
}
