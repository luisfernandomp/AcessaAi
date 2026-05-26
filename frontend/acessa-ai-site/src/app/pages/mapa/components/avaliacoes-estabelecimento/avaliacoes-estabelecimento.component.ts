import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AvaliacaoResponse } from '../../../../core/models/estabelecimento.model';

@Component({
  selector: 'app-avaliacoes-estabelecimento',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './avaliacoes-estabelecimento.component.html',
  styleUrls: ['./avaliacoes-estabelecimento.component.css'],
})
export class AvaliacoesEstabelecimentoComponent {
  @Input() avaliacoes: AvaliacaoResponse[] = [];
  @Input() mediaEstrelas = 0;
  @Input() maxExibir = 2;
  @Output() verTodas = new EventEmitter<void>();

  trackById(_index: number, avaliacao: AvaliacaoResponse): number {
    return avaliacao.id;
  }

  getEstrelas(nota: number): string {
    const cheias = Math.round(nota);
    return '★'.repeat(cheias) + '☆'.repeat(5 - cheias);
  }

  iniciais(nome: string): string {
    return nome
      .split(' ')
      .slice(0, 2)
      .map((parte) => parte.charAt(0).toUpperCase())
      .join('');
  }
}
