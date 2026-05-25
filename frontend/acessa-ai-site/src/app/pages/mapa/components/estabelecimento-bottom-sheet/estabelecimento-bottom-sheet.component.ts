import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { TuiCarousel } from '@taiga-ui/kit';
import { Lugar } from '../../mapa.models';
import { AvaliarEstabelecimentoModalComponent } from '../../../components/avaliar-estabelecimento-modal/avaliar-estabelecimento-modal.component';

export interface Avaliacao {
  id: number;
  autor: string;
  iniciais: string;
  cor: string;
  estrelas: number;
  comentario: string;
  data: string;
}

const AVALIACOES_MOCK: Record<number, Avaliacao[]> = {
  1: [
    {
      id: 1,
      autor: 'João Silva',
      iniciais: 'JS',
      cor: '#1a73e8',
      estrelas: 5,
      comentario: 'Excelente restaurante! Totalmente acessível, rampa bem sinalizada e banheiro adaptado impecável.',
      data: 'há 2 dias',
    },
    {
      id: 2,
      autor: 'Maria Fernandes',
      iniciais: 'MF',
      cor: '#137333',
      estrelas: 4,
      comentario: 'Ótima comida e ambiente acolhedor. O cardápio em braille fez toda a diferença para meu pai.',
      data: 'há 1 semana',
    },
    {
      id: 3,
      autor: 'Carlos Rocha',
      iniciais: 'CR',
      cor: '#e37400',
      estrelas: 5,
      comentario: 'Recomendo muito! Equipe bem treinada para atender pessoas com deficiência.',
      data: 'há 2 semanas',
    },
  ],
  2: [
    {
      id: 1,
      autor: 'Ana Costa',
      iniciais: 'AC',
      cor: '#c5221f',
      estrelas: 5,
      comentario: 'Farmácia 24h com ótimo atendimento em Libras. Muito importante para a comunidade surda.',
      data: 'há 3 dias',
    },
    {
      id: 2,
      autor: 'Pedro Alves',
      iniciais: 'PA',
      cor: '#1a73e8',
      estrelas: 4,
      comentario: 'Bom atendimento, rampa de acesso funcionando corretamente. Poderia ter mais opções de estacionamento.',
      data: 'há 5 dias',
    },
  ],
  3: [
    {
      id: 1,
      autor: 'Lucia Mendes',
      iniciais: 'LM',
      cor: '#137333',
      estrelas: 4,
      comentario: 'Hospital com ótima infraestrutura de acessibilidade. Elevadores sempre funcionando.',
      data: 'há 1 dia',
    },
    {
      id: 2,
      autor: 'Roberto Santos',
      iniciais: 'RS',
      cor: '#e37400',
      estrelas: 4,
      comentario: 'Atendimento demorado mas estrutura acessível de primeira. Cadeiras de rodas disponíveis na entrada.',
      data: 'há 4 dias',
    },
    {
      id: 3,
      autor: 'Fernanda Lima',
      iniciais: 'FL',
      cor: '#c5221f',
      estrelas: 3,
      comentario: 'A infraestrutura é boa mas alguns elevadores estavam em manutenção na minha visita.',
      data: 'há 1 semana',
    },
  ],
  4: [
    {
      id: 1,
      autor: 'Marcos Oliveira',
      iniciais: 'MO',
      cor: '#1a73e8',
      estrelas: 4,
      comentario: 'Caixas eletrônicos adaptados funcionando, ótimo para quem usa cadeira de rodas.',
      data: 'há 2 dias',
    },
    {
      id: 2,
      autor: 'Juliana Pereira',
      iniciais: 'JP',
      cor: '#137333',
      estrelas: 3,
      comentario: 'Atendimento preferencial sempre respeitado. Fila um pouco demorada nas segundas-feiras.',
      data: 'há 3 semanas',
    },
  ],
  5: [
    {
      id: 1,
      autor: 'Thiago Nunes',
      iniciais: 'TN',
      cor: '#e37400',
      estrelas: 5,
      comentario: 'Shopping muito acessível! Elevadores em todos os andares e estacionamento exclusivo bem sinalizado.',
      data: 'há 1 dia',
    },
    {
      id: 2,
      autor: 'Camila Souza',
      iniciais: 'CS',
      cor: '#1a73e8',
      estrelas: 4,
      comentario: 'Banheiros adaptados em bom estado. Equipe de segurança bem preparada para auxiliar.',
      data: 'há 6 dias',
    },
    {
      id: 3,
      autor: 'Diego Martins',
      iniciais: 'DM',
      cor: '#c5221f',
      estrelas: 5,
      comentario: 'Melhor shopping da região em termos de acessibilidade. Referência para outros estabelecimentos.',
      data: 'há 2 semanas',
    },
  ],
  6: [
    {
      id: 1,
      autor: 'Renata Cardoso',
      iniciais: 'RC',
      cor: '#137333',
      estrelas: 4,
      comentario: 'Elevadores sempre funcionando, plataforma tátil bem conservada. Ótima opção de transporte.',
      data: 'há 2 dias',
    },
    {
      id: 2,
      autor: 'Bruno Ferreira',
      iniciais: 'BF',
      cor: '#e37400',
      estrelas: 4,
      comentario: 'Muito bom para usuários de cadeira de rodas. Funcionários prestam auxílio sempre que necessário.',
      data: 'há 1 semana',
    },
  ],
  7: [
    {
      id: 1,
      autor: 'Isabela Torres',
      iniciais: 'IT',
      cor: '#1a73e8',
      estrelas: 5,
      comentario: 'Café charmoso, café excelente! Ainda não tem rampa mas o dono comentou que está providenciando.',
      data: 'há 3 dias',
    },
    {
      id: 2,
      autor: 'Gustavo Ramos',
      iniciais: 'GR',
      cor: '#c5221f',
      estrelas: 4,
      comentario: 'Lugar incrível, mas infelizmente sem acessibilidade para cadeirantes. Espero que melhore em breve.',
      data: 'há 2 semanas',
    },
  ],
  8: [
    {
      id: 1,
      autor: 'Patrícia Gomes',
      iniciais: 'PG',
      cor: '#137333',
      estrelas: 4,
      comentario: 'UPA com bom atendimento e rampa de acesso acessível. Sempre há alguém para ajudar na entrada.',
      data: 'há 4 dias',
    },
    {
      id: 2,
      autor: 'Eduardo Vieira',
      iniciais: 'EV',
      cor: '#e37400',
      estrelas: 3,
      comentario: 'Estrutura razoável, mas o banheiro adaptado precisa de manutenção. Atendimento rápido.',
      data: 'há 10 dias',
    },
  ],
};

@Component({
  selector: 'app-estabelecimento-bottom-sheet',
  standalone: true,
  imports: [CommonModule, TuiCarousel, AvaliarEstabelecimentoModalComponent],
  templateUrl: './estabelecimento-bottom-sheet.component.html',
  styleUrl: './estabelecimento-bottom-sheet.component.css',
})
export class EstabelecimentoBottomSheetComponent implements OnChanges {
  @Input() lugar: Lugar | null = null;
  @Output() fechar = new EventEmitter<void>();

  avaliacoes: Avaliacao[] = [];
  expandido = false;
  fotoAtual = 0;
  mostrarModalAvaliar = false;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['lugar'] && this.lugar) {
      this.avaliacoes = AVALIACOES_MOCK[this.lugar.id] ?? [];
      this.expandido = false;
      this.fotoAtual = 0;
    }
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

  toggleExpandido(): void {
    this.expandido = !this.expandido;
  }

  onFechar(): void {
    this.expandido = false;
    this.fechar.emit();
  }

  onAvaliar(): void {
    this.mostrarModalAvaliar = true;
  }

  onAvaliacaoEnviada(): void {
    this.mostrarModalAvaliar = false;
  }

  getBarraPercentual(nota: number): number {
    // Simular percentuais da distribuição de notas
    const percentuais: Record<number, number> = {
      5: 60,
      4: 25,
      3: 10,
      2: 3,
      1: 2,
    };
    return percentuais[nota] || 0;
  }
}
