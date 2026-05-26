import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../core/services/auth.service';
import { AvaliacaoService } from '../../../core/services/avaliacao.service';

@Component({
  selector: 'app-avaliar-estabelecimento-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './avaliar-estabelecimento-modal.component.html',
  styleUrl: './avaliar-estabelecimento-modal.component.css',
})
export class AvaliarEstabelecimentoModalComponent {
  @Input() estabelecimentoId!: number;
  @Output() fechar = new EventEmitter<void>();
  @Output() avaliado = new EventEmitter<void>();

  private authService = inject(AuthService);
  private avaliacaoService = inject(AvaliacaoService);
  private toastr = inject(ToastrService);

  readonly estrelas = [1, 2, 3, 4, 5];
  estrelaSelecionada = 0;
  estrelaHover = 0;
  comentario = '';
  carregando = false;

  get estrelaAtiva(): number {
    return this.estrelaHover || this.estrelaSelecionada;
  }

  readonly labels: Record<number, string> = {
    1: 'Péssimo',
    2: 'Ruim',
    3: 'Regular',
    4: 'Bom',
    5: 'Excelente',
  };

  submit(): void {
    if (this.estrelaSelecionada === 0) {
      this.toastr.warning('Selecione uma nota de 1 a 5 estrelas.');
      return;
    }

    const usuario = this.authService.getUsuarioLogado();
    if (!usuario) {
      this.toastr.error('Você precisa estar logado para avaliar.');
      return;
    }

    this.carregando = true;
    this.avaliacaoService
      .criar({
        comentario: this.comentario.trim(),
        estrelas: this.estrelaSelecionada,
        usuarioId: usuario.id,
        estabelecimentoId: this.estabelecimentoId,
      })
      .subscribe({
        next: () => {
          this.carregando = false;
          this.toastr.success('Avaliação enviada com sucesso!');
          setTimeout(() => this.avaliado.emit(), 1000);
        },
        error: () => {
          this.carregando = false;
          this.toastr.error('Erro ao enviar avaliação. Tente novamente.');
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
