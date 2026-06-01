import { Component, HostListener, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../core/services/auth.service';
import { UsuarioService } from '../../../core/services/usuario.service';
import { EstabelecimentoService } from '../../../core/services/estabelecimento.service';
import { EstabelecimentoResponse } from '../../../core/models/estabelecimento.model';
import { UsuarioResponse } from '../../../core/models/usuario.model';
import { RegisterModalComponent } from '../register-modal/register-modal.component';
import { LoginModalComponent } from '../login-modal/login-modal.component';
import { CadastrarEstabelecimentoModalComponent } from '../cadastrar-estabelecimento-modal/cadastrar-estabelecimento-modal.component';
import { UserMenuComponent } from '../user-menu/user-menu.component';
import { EditarEstabelecimentosModalComponent } from '../editar-estabelecimentos-modal/editar-estabelecimentos-modal.component';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    LoginModalComponent,
    RegisterModalComponent,
    CadastrarEstabelecimentoModalComponent,
    UserMenuComponent,
    EditarEstabelecimentosModalComponent,
  ],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  private readonly toastr = inject(ToastrService);

  showLoginModal = false;
  showRegisterModal = false;
  showEstabelecimentoModal = false;
  showEditarEstabelecimentosModal = false;
  showEditarEstabelecimentoModal = false;
  showPerfilModal = false;
  estabelecimentoSelecionado: EstabelecimentoResponse | null = null;
  dadosPerfil: UsuarioResponse | null = null;
  scrolled = false;

  constructor(
    public authService: AuthService,
    private router: Router,
    private estabelecimentoService: EstabelecimentoService,
    private usuarioService: UsuarioService,
  ) {}

  ngOnInit(): void {}

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  get usuarioNome(): string {
    return this.authService.getUsuarioLogado()?.nome ?? '';
  }

  get usuarioIniciais(): string {
    const nome = this.usuarioNome;
    if (!nome) return '?';
    const partes = nome.trim().split(' ');
    if (partes.length === 1) return partes[0][0].toUpperCase();
    return (partes[0][0] + partes[partes.length - 1][0]).toUpperCase();
  }

  get avatarCor(): string {
    const cores = ['#2563eb', '#7c3aed', '#059669', '#d97706', '#dc2626', '#0891b2'];
    const idx = this.usuarioNome.charCodeAt(0) % cores.length;
    return cores[idx] ?? '#2563eb';
  }

  @HostListener('window:scroll')
  onScroll(): void {
    this.scrolled = window.scrollY > 10;
  }

  openLoginModal(): void {
    this.showLoginModal = true;
  }

  closeLoginModal(): void {
    this.showLoginModal = false;
  }

  loginToRegister(): void {
    this.showLoginModal = false;
    this.showRegisterModal = true;
  }

  openRegisterModal(): void {
    this.showRegisterModal = true;
  }

  closeRegisterModal(): void {
    this.showRegisterModal = false;
  }

  registerToLogin(): void {
    this.showRegisterModal = false;
    this.showLoginModal = true;
  }

  handleCadastrarLocal(): void {
    if (this.isLoggedIn) {
      this.showEstabelecimentoModal = true;
    } else {
      this.showRegisterModal = true;
    }
  }

  openEstabelecimentoModal(): void {
    this.showEstabelecimentoModal = true;
  }

  closeEstabelecimentoModal(): void {
    this.showEstabelecimentoModal = false;
  }

  onEstabelecimentoCadastrado(): void {
    this.showEstabelecimentoModal = false;
    this.estabelecimentoService.cadastroRealizado$.next();
  }

  editEstabelecimentos(): void {
    this.showEditarEstabelecimentosModal = true;
  }

  closeEditarEstabelecimentosModal(): void {
    this.showEditarEstabelecimentosModal = false;
  }

  onEditarEstabelecimento(estabelecimento: EstabelecimentoResponse): void {
    this.estabelecimentoSelecionado = estabelecimento;
    this.showEditarEstabelecimentosModal = false;
    this.showEditarEstabelecimentoModal = true;
  }

  closeEditarEstabelecimentoModal(): void {
    this.showEditarEstabelecimentoModal = false;
    this.estabelecimentoSelecionado = null;
  }

  onEdicaoSucesso(): void {
    this.showEditarEstabelecimentoModal = false;
    this.estabelecimentoSelecionado = null;
    this.estabelecimentoService.cadastroRealizado$.next();
  }

  onDeletarEstabelecimento(id: number): void {
    console.log('Deletar estabelecimento:', id);
  }

  openPerfilModal(): void {
    const usuario = this.authService.getUsuarioLogado();
    if (!usuario) return;
    this.usuarioService.getById(usuario.id).subscribe({
      next: (dados) => {
        this.dadosPerfil = dados;
        this.showPerfilModal = true;
      },
      error: () => {
        this.showPerfilModal = true;
      },
    });
  }

  closePerfilModal(): void {
    this.showPerfilModal = false;
    this.dadosPerfil = null;
  }
}
