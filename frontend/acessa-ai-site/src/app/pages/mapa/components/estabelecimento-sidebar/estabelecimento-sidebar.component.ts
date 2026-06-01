import { Component, EventEmitter, HostListener, inject, Input, OnDestroy, Output, SimpleChanges } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Lugar } from '../../mapa.models';
import { RecursoAcessibilidade } from '../../../../core/models/recurso-acessibilidade.model';
import { AvaliacaoResponse } from '../../../../core/models/estabelecimento.model';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../../core/services/auth.service';
import { EstabelecimentoService } from '../../../../core/services/estabelecimento.service';
import { AvaliacaoService } from '../../../../core/services/avaliacao.service';
import { AvaliacoesEstabelecimentoComponent } from '../avaliacoes-estabelecimento/avaliacoes-estabelecimento.component';
import { LoginModalComponent } from '../../../components/login-modal/login-modal.component';

const CORES_AVATAR = ['#1a73e8', '#137333', '#e37400', '#c5221f', '#7b2d8b', '#0097a7'];

@Component({
  selector: 'app-estabelecimento-sidebar',
  standalone: true,
  imports: [DecimalPipe, FormsModule, AvaliacoesEstabelecimentoComponent, LoginModalComponent],
  templateUrl: './estabelecimento-sidebar.component.html',
  styleUrl: './estabelecimento-sidebar.component.css'
})
export class EstabelecimentoSidebarComponent implements OnDestroy {
    @Input() lugar: Lugar | null = null;
    @Output() fechar = new EventEmitter<void>();

    private readonly estabelecimentoService = inject(EstabelecimentoService);
    private readonly avaliacaoService = inject(AvaliacaoService);
    private readonly authService = inject(AuthService);
    private readonly toastr = inject(ToastrService);

    avaliacoes: AvaliacaoResponse[] = [];
    recursosAcessibilidade: RecursoAcessibilidade[] = [];
    mediaEstrelas = 0;
    expandido = false;
    fotoAtual = 0;
    mostrarGaleria = false;
    fotoGaleria = 0;
    mostrarLoginModal = false;
    mostrarFormAvaliar = false;
    estrelaSelecionada = 0;
    estrelaHover = 0;
    comentario = '';
    carregandoAvaliacao = false;

    readonly labelsEstrela: Record<number, string> = {
      1: 'Péssimo', 2: 'Ruim', 3: 'Regular', 4: 'Bom', 5: 'Excelente',
    };

    get estrelaAtiva(): number {
      return this.estrelaHover || this.estrelaSelecionada;
    }
  
    ngOnChanges(changes: SimpleChanges): void {
      if (changes['lugar'] && this.lugar) {
        this.avaliacoes = [];
        this.recursosAcessibilidade = [];
        this.mediaEstrelas = 0;
        this.expandido = false;
        this.fotoAtual = 0;
        this.estabelecimentoService.getById(this.lugar.id).subscribe({
          next: (res) => {
            this.avaliacoes = res.avaliacaoResponses ?? [];
            this.recursosAcessibilidade = res.recursosAcessibilidade ?? [];
            this.mediaEstrelas = this.avaliacoes.length
              ? this.avaliacoes.reduce((sum, a) => sum + a.estrelas, 0) / this.avaliacoes.length
              : 0;
          },
          error: () => {},
        });
      }
    }
  
    iniciais(nome: string): string {
      return nome
        .split(' ')
        .slice(0, 2)
        .map((n) => n[0])
        .join('')
        .toUpperCase();
    }
  
    corAvatar(usuarioId: number): string {
      return CORES_AVATAR[usuarioId % CORES_AVATAR.length];
    }
  
    estrelas(avaliacao: number): { cheia: boolean }[] {
      return Array(5)
        .fill(0)
        .map((_, i) => ({ cheia: i < Math.round(avaliacao) }));
    }
  
    estrelasParciais(avaliacao: number): { tipo: 'cheia' | 'meia' | 'vazia' }[] {
      return Array(5)
        .fill(0)
        .map((_, i) => {
          if (i < Math.floor(avaliacao)) return { tipo: 'cheia' };
          if (i < avaliacao) return { tipo: 'meia' };
          return { tipo: 'vazia' };
        });
    }
  
    getBarraPercentual(nota: number): number {
      if (!this.avaliacoes.length) return 0;
      const count = this.avaliacoes.filter((a) => a.estrelas === nota).length;
      return Math.round((count / this.avaliacoes.length) * 100);
    }
  
    toggleExpandido(): void {
      this.expandido = !this.expandido;
    }

    prevFoto(): void {
      const total = this.lugar?.fotos?.length ?? 0;
      this.fotoAtual = (this.fotoAtual - 1 + total) % total;
    }

    nextFoto(): void {
      const total = this.lugar?.fotos?.length ?? 0;
      this.fotoAtual = (this.fotoAtual + 1) % total;
    }

    onGaleria(): void {
      this.fotoGaleria = this.fotoAtual;
      this.mostrarGaleria = true;
      document.body.classList.add('galeria-aberta');
    }

    prevGaleria(): void {
      const total = this.lugar?.fotos?.length ?? 0;
      this.fotoGaleria = (this.fotoGaleria - 1 + total) % total;
    }

    nextGaleria(): void {
      const total = this.lugar?.fotos?.length ?? 0;
      this.fotoGaleria = (this.fotoGaleria + 1) % total;
    }

    fecharGaleria(): void {
      this.mostrarGaleria = false;
      document.body.classList.remove('galeria-aberta');
    }

    onCompartilhar(): void {
      navigator.clipboard.writeText(window.location.href).then(
        () => this.toastr.success('Link copiado para a área de transferência!'),
        () => this.toastr.error('Não foi possível copiar o link.'),
      );
    }

    @HostListener('document:keydown.escape')
    onKeyEscape(): void {
      if (this.mostrarGaleria) this.fecharGaleria();
    }

    ngOnDestroy(): void {
      document.body.classList.remove('galeria-aberta');
    }

    onFechar(): void {
      this.expandido = false;
      this.fechar.emit();
    }
  
    onAvaliar(): void {
      if (!this.authService.isLoggedIn()) {
        this.mostrarLoginModal = true;
        return;
      }
      this.estrelaSelecionada = 0;
      this.estrelaHover = 0;
      this.comentario = '';
      this.mostrarFormAvaliar = true;
    }

    onCancelarAvaliacao(): void {
      this.mostrarFormAvaliar = false;
      this.estrelaSelecionada = 0;
      this.estrelaHover = 0;
      this.comentario = '';
    }

    onConfirmarAvaliacao(): void {
      if (this.estrelaSelecionada === 0) {
        this.toastr.warning('Selecione uma nota de 1 a 5 estrelas.');
        return;
      }
      const usuario = this.authService.getUsuarioLogado();
      if (!usuario || !this.lugar) return;

      this.carregandoAvaliacao = true;
      this.avaliacaoService.criar({
        comentario: this.comentario.trim(),
        estrelas: this.estrelaSelecionada,
        usuarioId: usuario.id,
        estabelecimentoId: this.lugar.id,
      }).subscribe({
        next: () => {
          this.carregandoAvaliacao = false;
          this.toastr.success('Avaliação enviada com sucesso!');
          this.mostrarFormAvaliar = false;
          setTimeout(() => this.onAvaliacaoEnviada(), 800);
        },
        error: () => {
          this.carregandoAvaliacao = false;
          this.toastr.error('Erro ao enviar avaliação. Tente novamente.');
        },
      });
    }

    onAvaliacaoEnviada(): void {
      if (this.lugar) {
        this.estabelecimentoService.getById(this.lugar.id).subscribe({
          next: (res) => {
            this.avaliacoes = res.avaliacaoResponses ?? [];
            this.mediaEstrelas = this.avaliacoes.length
              ? this.avaliacoes.reduce((sum, a) => sum + a.estrelas, 0) / this.avaliacoes.length
              : 0;
          },
          error: () => {},
        });
      }
    }
}
