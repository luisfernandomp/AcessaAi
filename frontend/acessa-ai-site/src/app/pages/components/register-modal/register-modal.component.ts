import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { ESTADOS_BRASILEIROS } from '../../../core/constants/estados';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../core/services/auth.service';
import { UsuarioService } from '../../../core/services/usuario.service';
import { AtualizarUsuarioRequest, CadastrarUsuarioRequest, UsuarioResponse } from '../../../core/models/usuario.model';

function senhaValidator(control: AbstractControl): ValidationErrors | null {
  const v: string = control.value ?? '';
  const erros: ValidationErrors = {};
  if (v.length < 6) erros['minLength'] = true;
  if (!/[0-9]/.test(v)) erros['requireDigit'] = true;
  if (!/[A-Z]/.test(v)) erros['requireUppercase'] = true;
  return Object.keys(erros).length ? erros : null;
}

@Component({
  selector: 'app-register-modal',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule],
  templateUrl: './register-modal.component.html',
  styleUrl: './register-modal.component.css',
})
export class RegisterModalComponent implements OnInit {
  @Input() modoEdicao = false;
  @Input() dadosIniciais: UsuarioResponse | null = null;

  @Output() fechar = new EventEmitter<void>();
  @Output() irParaLogin = new EventEmitter<void>();
  @Output() perfilAtualizado = new EventEmitter<void>();

  private readonly toastr = inject(ToastrService);
  private readonly authService = inject(AuthService);

  readonly estados = ESTADOS_BRASILEIROS;
  etapa = 1;
  carregando = false;
  mostrarSenha = false;
  fotoPerfilPreview: string | null = null;
  uploadandoFoto = false;

  form = this.fb.group({
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
      uf: ['', [Validators.required, Validators.maxLength(2)]],
      cep: ['', Validators.required],
    }),
  });

  constructor(
    private fb: FormBuilder,
    private usuarioService: UsuarioService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    if (this.modoEdicao) {
      const senhaCtrl = this.form.controls.senha;
      senhaCtrl.clearValidators();
      senhaCtrl.updateValueAndValidity();

      const emailCtrl = this.form.controls.email;
      emailCtrl.clearValidators();
      emailCtrl.updateValueAndValidity();

      if (this.dadosIniciais) {
        const d = this.dadosIniciais;
        const dataFormatada = d.dataNascimento ? d.dataNascimento.split('T')[0] : '';
        this.form.patchValue({
          nome: d.nome,
          dataNascimento: dataFormatada,
          endereco: {
            logradouro: d.endereco?.logradouro ?? '',
            numero: d.endereco?.numero ?? '',
            complemento: d.endereco?.complemento ?? '',
            bairro: d.endereco?.bairro ?? '',
            cidade: d.endereco?.cidade ?? '',
            uf: d.endereco?.uf ?? '',
            cep: d.endereco?.cep ?? '',
          },
        });
        this.fotoPerfilPreview = d.urlFotoPerfil ?? null;
      }
    }
  }

  proximaEtapa(): void {
    const { nome, dataNascimento } = this.form.controls;
    if (this.modoEdicao) {
      if (nome.invalid || dataNascimento.invalid) {
        this.toastr.warning('Preencha todos os campos obrigatórios da etapa atual.');
        return;
      }
    } else {
      const { email, senha } = this.form.controls;
      if (nome.invalid || email.invalid || senha.invalid || dataNascimento.invalid) {
        this.toastr.warning('Preencha todos os campos obrigatórios da etapa atual.');
        return;
      }
    }
    this.etapa = 2;
  }

  voltarEtapa(): void {
    this.etapa = 1;
  }

  submit(): void {
    if (this.modoEdicao) {
      this.submitEdicao();
    } else {
      this.submitCadastro();
    }
  }

  private submitCadastro(): void {
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

  private submitEdicao(): void {
    const { nome, dataNascimento, endereco } = this.form.controls;
    if (nome.invalid || dataNascimento.invalid || endereco.invalid) {
      this.toastr.warning('Preencha todos os campos obrigatórios.');
      return;
    }

    const usuario = this.authService.getUsuarioLogado();
    if (!usuario) return;

    this.carregando = true;
    const v = this.form.value;

    const request: AtualizarUsuarioRequest = {
      nome: v.nome!,
      dataNascimento: v.dataNascimento!,
      endereco: v.endereco as AtualizarUsuarioRequest['endereco'],
    };

    this.usuarioService.atualizar(usuario.id, request).subscribe({
      next: (res) => {
        this.carregando = false;
        this.authService.atualizarNomeUsuario(res.nome);
        this.toastr.success('Perfil atualizado com sucesso!');
        setTimeout(() => this.perfilAtualizado.emit(), 800);
      },
      error: () => {
        this.carregando = false;
        this.toastr.error('Erro ao atualizar perfil. Tente novamente.');
      },
    });
  }

  onFotoChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    const arquivo = input.files?.[0];
    if (!arquivo) return;

    const reader = new FileReader();
    reader.onload = (e) => (this.fotoPerfilPreview = e.target!.result as string);
    reader.readAsDataURL(arquivo);

    const usuario = this.authService.getUsuarioLogado();
    if (!usuario) return;

    this.uploadandoFoto = true;
    this.usuarioService.uploadFotoPerfil(usuario.id, arquivo).subscribe({
      next: (url) => {
        this.uploadandoFoto = false;
        this.fotoPerfilPreview = url;
        this.toastr.success('Foto de perfil atualizada!');
      },
      error: () => {
        this.uploadandoFoto = false;
        this.fotoPerfilPreview = this.dadosIniciais?.urlFotoPerfil ?? null;
        this.toastr.error('Erro ao fazer upload da foto. Tente novamente.');
      },
    });

    input.value = '';
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
