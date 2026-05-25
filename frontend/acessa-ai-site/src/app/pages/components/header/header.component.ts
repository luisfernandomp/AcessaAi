import { Component, HostListener, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { TuiButton } from '@taiga-ui/core';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../core/services/auth.service';
import { EstabelecimentoService } from '../../../core/services/estabelecimento.service';
import { EstabelecimentoResponse } from '../../../core/models/estabelecimento.model';
import { RegisterModalComponent } from '../register-modal/register-modal.component';
import { CadastrarEstabelecimentoModalComponent } from '../cadastrar-estabelecimento-modal/cadastrar-estabelecimento-modal.component';
import { UserMenuComponent } from '../user-menu/user-menu.component';
import { EditarEstabelecimentosModalComponent } from '../editar-estabelecimentos-modal/editar-estabelecimentos-modal.component';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TuiButton,
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

  showRegisterModal = false;
  showEstabelecimentoModal = false;
  showEditarEstabelecimentosModal = false;
  showEditarEstabelecimentoModal = false;
  estabelecimentoSelecionado: EstabelecimentoResponse | null = null;
  scrolled = false;

  constructor(
    public authService: AuthService,
    private router: Router,
    private estabelecimentoService: EstabelecimentoService,
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

  openRegisterModal(): void {
    this.showRegisterModal = true;
  }

  closeRegisterModal(): void {
    this.showRegisterModal = false;
  }

  registerToLogin(): void {
    this.showRegisterModal = false;
    this.router.navigate(['/login']);
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
  }

  editEstabelecimentos(): void {
    this.showEditarEstabelecimentosModal = true;
  }

  closeEditarEstabelecimentosModal(): void {
    this.showEditarEstabelecimentosModal = false;
  }

  onEditarEstabelecimento(estabelecimento: EstabelecimentoResponse): void {
    this.estabelecimentoSelecionado = estabelecimento;
    const novoNome = window
      .prompt('Informe o novo nome do estabelecimento:', estabelecimento.nome)
      ?.trim();

    if (!novoNome) return;

    const latitudeTexto = window.prompt(
      'Informe a nova latitude:',
      String(estabelecimento.geocordenadas.latitude),
    );
    if (latitudeTexto == null) return;

    const longitudeTexto = window.prompt(
      'Informe a nova longitude:',
      String(estabelecimento.geocordenadas.longitude),
    );
    if (longitudeTexto == null) return;

    const latitude = Number.parseFloat(latitudeTexto.replace(',', '.'));
    const longitude = Number.parseFloat(longitudeTexto.replace(',', '.'));

    if (Number.isNaN(latitude) || Number.isNaN(longitude)) {
      this.toastr.error('Latitude e longitude precisam ser números válidos.');
      return;
    }

    this.estabelecimentoService
      .atualizar(estabelecimento.id, { nome: novoNome, geocordenadas: { latitude, longitude } })
      .subscribe({
        next: () => {
          this.showEditarEstabelecimentosModal = false;
          this.showEditarEstabelecimentoModal = false;
          this.estabelecimentoSelecionado = null;
          this.toastr.success('Estabelecimento atualizado com sucesso!');
        },
        error: (erro) => {
          console.error('Erro ao editar estabelecimento:', erro);
          this.toastr.error('Não foi possível atualizar o estabelecimento. Tente novamente.');
        },
      });
  }

  closeEditarEstabelecimentoModal(): void {
    this.showEditarEstabelecimentoModal = false;
    this.estabelecimentoSelecionado = null;
  }

  onDeletarEstabelecimento(id: number): void {
    console.log('Deletar estabelecimento:', id);
  }
}
