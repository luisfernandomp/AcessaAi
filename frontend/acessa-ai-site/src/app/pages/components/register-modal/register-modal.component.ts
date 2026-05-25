import { Component, EventEmitter, Output, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TuiButton, TuiIcon, TuiTextfield } from '@taiga-ui/core';
import { TuiPassword } from '@taiga-ui/kit';
import { ToastrService } from 'ngx-toastr';
import { UsuarioService } from '../../../core/services/usuario.service';
import { CadastrarUsuarioRequest } from '../../../core/models/usuario.model';

@Component({
  selector: 'app-register-modal',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule, TuiTextfield, TuiButton, TuiIcon, TuiPassword],
  templateUrl: './register-modal.component.html',
  styleUrl: './register-modal.component.css',
})
export class RegisterModalComponent {
  @Output() fechar = new EventEmitter<void>();
  @Output() irParaLogin = new EventEmitter<void>();

  private readonly toastr = inject(ToastrService);

  etapa = 1;
  carregando = false;

  form = this.fb.group({
    nome: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    senha: ['', [Validators.required, Validators.minLength(6)]],
    dataNascimento: ['', Validators.required],
    endereco: this.fb.group({
      logradouro: ['', Validators.required],
      numero: ['', Validators.required],
      complemento: [''],
      bairro: ['', Validators.required],
      cidade: ['', Validators.required],
      uf: ['', [Validators.required, Validators.maxLength(2)]],
      cep: ['', Validators.required],
    }),
  });

  constructor(
    private fb: FormBuilder,
    private usuarioService: UsuarioService,
    private router: Router,
  ) {}

  proximaEtapa(): void {
    const { nome, email, senha, dataNascimento } = this.form.controls;
    if (nome.invalid || email.invalid || senha.invalid || dataNascimento.invalid) {
      this.toastr.warning('Preencha todos os campos obrigatórios da etapa atual.');
      return;
    }
    this.etapa = 2;
  }

  voltarEtapa(): void {
    this.etapa = 1;
  }

  submit(): void {
    if (this.form.invalid) {
      this.toastr.warning('Preencha todos os campos obrigatórios.');
      return;
    }
    this.carregando = true;
    this.usuarioService.cadastrar(this.form.value as CadastrarUsuarioRequest).subscribe({
      next: () => {
        this.carregando = false;
        this.toastr.success('Conta criada com sucesso!');
        setTimeout(() => this.irParaLogin.emit(), 1500);
      },
      error: () => {
        this.carregando = false;
        this.toastr.error('Erro ao cadastrar. Verifique os dados e tente novamente.');
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
