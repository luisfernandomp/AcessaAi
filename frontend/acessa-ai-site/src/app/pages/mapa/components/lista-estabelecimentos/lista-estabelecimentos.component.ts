import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TuiButton, TuiLoader } from '@taiga-ui/core';
import { Lugar, CATEGORIAS } from '../../mapa.models';

@Component({
  selector: 'app-lista-estabelecimentos',
  standalone: true,
  imports: [CommonModule, TuiLoader, TuiButton],
  templateUrl: './lista-estabelecimentos.component.html',
  styleUrl: './lista-estabelecimentos.component.css',
})
export class ListaEstabelecimentosComponent {
  @Input() lugaresFiltrados: Lugar[] = [];
  @Input() lugarSelecionado: Lugar | null = null;
  @Input() carregandoLista = false;
  @Output() lugarClick = new EventEmitter<Lugar>();
  @Output() limparFiltros = new EventEmitter<void>();

  selecionarLugar(lugar: Lugar): void {
    this.lugarClick.emit(lugar);
  }

  estrelas(avaliacao: number): { cheia: boolean }[] {
    return Array(5).fill(0).map((_, i) => ({ cheia: i < Math.round(avaliacao) }));
  }

  categoriaIcone(cat: string): string {
    return CATEGORIAS.find((c) => c.id === cat)?.icone ?? 'fa-solid fa-location-dot';
  }

  capaUrl(lugar: Lugar): string | null {
    return lugar.fotos?.find((f) => f.isCapa)?.url ?? lugar.fotos?.[0]?.url ?? null;
  }
}
