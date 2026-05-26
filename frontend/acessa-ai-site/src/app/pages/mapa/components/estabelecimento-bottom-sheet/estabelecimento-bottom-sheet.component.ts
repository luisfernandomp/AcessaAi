import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnChanges,
  SimpleChanges,
  inject,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { TuiCarousel } from '@taiga-ui/kit';
import { Lugar } from '../../mapa.models';
import { AvaliarEstabelecimentoModalComponent } from '../../../components/avaliar-estabelecimento-modal/avaliar-estabelecimento-modal.component';
import { EstabelecimentoService } from '../../../../core/services/estabelecimento.service';
import { AvaliacaoResponse } from '../../../../core/models/estabelecimento.model';
import { RecursoAcessibilidade } from '../../../../core/models/recurso-acessibilidade.model';
import { AuthService } from '../../../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { AvaliacoesEstabelecimentoComponent } from '../avaliacoes-estabelecimento/avaliacoes-estabelecimento.component';

const CORES_AVATAR = ['#1a73e8', '#137333', '#e37400', '#c5221f', '#7b2d8b', '#0097a7'];

@Component({
  selector: 'app-estabelecimento-bottom-sheet',
  standalone: true,
  imports: [CommonModule, TuiCarousel, AvaliarEstabelecimentoModalComponent, AvaliacoesEstabelecimentoComponent],
  templateUrl: './estabelecimento-bottom-sheet.component.html',
  styleUrl: './estabelecimento-bottom-sheet.component.css',
})
export class EstabelecimentoBottomSheetComponent implements OnChanges {
  @Input() lugar: Lugar | null = null;
  @Output() fechar = new EventEmitter<void>();

  private readonly estabelecimentoService = inject(EstabelecimentoService);
  private readonly authService = inject(AuthService);
  private readonly toastr = inject(ToastrService);

  avaliacoes: AvaliacaoResponse[] = [];
  recursosAcessibilidade: RecursoAcessibilidade[] = [];
  mediaEstrelas = 0;
  expandido = false;
  fotoAtual = 0;
  mostrarModalAvaliar = false;

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

  onFechar(): void {
    this.expandido = false;
    this.fechar.emit();
  }

  onAvaliar(): void {
    if (!this.authService.isLoggedIn()) {
      this.toastr.warning('Somente usuários logados podem avaliar. Faça login para continuar.');
      return;
    }
    this.mostrarModalAvaliar = true;
  }

  onAvaliacaoEnviada(): void {
    this.mostrarModalAvaliar = false;
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
