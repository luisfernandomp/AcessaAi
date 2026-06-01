import { Component, ElementRef, EventEmitter, inject, Input, NgZone, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CATEGORIAS, Lugar, Ordenacao, TIPO_MAP, TIPO_REVERSE_MAP } from '../../mapa.models';
import { EstabelecimentoService } from '../../../../core/services/estabelecimento.service';
import { debounceTime, Subject, Subscription, takeUntil } from 'rxjs';
import { EstabelecimentoResponse, FiltroEstabelecimentoRequest } from '../../../../core/models/estabelecimento.model';
import { ListaEstabelecimentosComponent } from '../lista-estabelecimentos/lista-estabelecimentos.component';

declare const google: any;

@Component({
  selector: 'app-estabelecimento-filtrar-sidebar',
  standalone: true,
  imports: [FormsModule, ListaEstabelecimentosComponent],
  templateUrl: './estabelecimento-filtrar-sidebar.component.html',
  styleUrl: './estabelecimento-filtrar-sidebar.component.css'
})
export class EstabelecimentoFiltrarSidebarComponent implements OnInit, OnDestroy {
  @Input() lugarSelecionado: Lugar | null = null;
  @Output() lugarClick = new EventEmitter<Lugar>();
  @Output() lugaresFiltradosChange = new EventEmitter<Lugar[]>();
  @Output() localizacaoObtida = new EventEmitter<{ lat: number; lng: number }>();

  private readonly estabelecimentoService = inject(EstabelecimentoService);
  private readonly ngZone = inject(NgZone);

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  private autocomplete: any = null;
  private mapInputEl: HTMLInputElement | null = null;

  @ViewChild('buscaMapa') set buscaMapaRef(el: ElementRef<HTMLInputElement> | undefined) {
    this.mapInputEl = el?.nativeElement ?? null;
    this.autocomplete = null;
    if (this.mapInputEl) {
      this.tentarInicializarAutocomplete();
    }
  }

  categorias = CATEGORIAS;
  categoriaSelecionada = 'todos';
  busca = '';
  buscaLocalizada = false;
  buscaLocalizadaNome = '';
  apenasAcessiveis = false;
  distanciaMaxima = 50;
  recolhido = false;
  buscaFocada = false;
  filtrosExpandidos = false;
  ordenacao: Ordenacao = 'distancia';
  carregandoLista = true;
  lugaresFiltrados: Lugar[] = [];

  private todosLugares: Lugar[] = [];
  private userLat?: number;
  private userLng?: number;
  private readonly buscaSubject$ = new Subject<void>();
  private readonly destroy$ = new Subject<void>();
  private listaSubscription?: Subscription;

  ngOnInit(): void {
    this.buscaSubject$.pipe(
      debounceTime(400),
      takeUntil(this.destroy$),
    ).subscribe(() => this.processarBuscaPorTexto());

    this.estabelecimentoService.cadastroRealizado$.pipe(
      takeUntil(this.destroy$),
    ).subscribe(() => this.executarBusca());

    this.obterLocalizacao();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.listaSubscription?.unsubscribe();
    this.pararScrollCategorias();
  }

  // ─── Scroll por proximidade nas categorias ───────────────────────────────

  private scrollFrame: number | null = null;
  private scrollDir = 0;

  onCategoriasMouseMove(event: MouseEvent, el: HTMLElement): void {
    const { left, width } = el.getBoundingClientRect();
    const x = event.clientX - left;
    const zona = 44;
    const wrapper = el.parentElement!;
    const podirEsq = el.scrollLeft > 0;
    const podirDir = el.scrollLeft < el.scrollWidth - el.clientWidth - 1;

    if (x < zona && podirEsq) {
      wrapper.classList.add('mostra-esq');
      wrapper.classList.remove('mostra-dir');
      this.ativarScroll(el, -1);
    } else if (x > width - zona && podirDir) {
      wrapper.classList.remove('mostra-esq');
      wrapper.classList.add('mostra-dir');
      this.ativarScroll(el, 1);
    } else {
      this.pararScrollCategorias(el);
    }
  }

  pararScrollCategorias(el?: HTMLElement): void {
    if (this.scrollFrame !== null) {
      cancelAnimationFrame(this.scrollFrame);
      this.scrollFrame = null;
    }
    this.scrollDir = 0;
    const wrapper = el?.parentElement ?? document.querySelector('.categorias-wrapper');
    wrapper?.classList.remove('mostra-esq', 'mostra-dir');
  }

  private ativarScroll(el: HTMLElement, dir: -1 | 1): void {
    if (this.scrollDir === dir) return;
    this.scrollDir = dir;
    if (this.scrollFrame !== null) cancelAnimationFrame(this.scrollFrame);
    const step = () => {
      el.scrollLeft += dir * 3;
      this.scrollFrame = requestAnimationFrame(step);
    };
    this.scrollFrame = requestAnimationFrame(step);
  }

  // ─── Localização e busca na API ──────────────────────────────────────────

  private obterLocalizacao(): void {
    if (!navigator.geolocation) {
      this.executarBusca();
      return;
    }
    navigator.geolocation.getCurrentPosition(
      (pos) => {
        this.userLat = pos.coords.latitude;
        this.userLng = pos.coords.longitude;
        this.localizacaoObtida.emit({ lat: this.userLat, lng: this.userLng });
        this.executarBusca();
      },
      () => this.executarBusca(),
      { timeout: 5000 },
    );
  }

  setUserLocation(coords: { lat: number; lng: number }): void {
    this.userLat = coords.lat;
    this.userLng = coords.lng;
    this.localizacaoObtida.emit(coords);
    this.executarBusca();
  }

  private processarBuscaPorTexto(): void {
    const texto = this.busca;

    if (!texto || typeof google === 'undefined' || !google.maps?.places?.AutocompleteService) {
      this.executarBusca();
      return;
    }

    const tiposEndereco = new Set([
      'route', 'street_address', 'geocode', 'sublocality', 'sublocality_level_1',
      'locality', 'administrative_area_level_1', 'administrative_area_level_2',
      'postal_code', 'neighborhood', 'premise',
    ]);

    const service = new google.maps.places.AutocompleteService();
    service.getPlacePredictions(
      { input: texto, componentRestrictions: { country: 'BR' } },
      (predictions: any[], status: string) => {
        this.ngZone.run(() => {
          if (this.busca !== texto) return;

          const primeira = predictions?.[0];
          const ehEndereco = status === 'OK' && primeira &&
            primeira.types.some((t: string) => tiposEndereco.has(t));

          if (!ehEndereco) {
            this.executarBusca();
            return;
          }

          this.resolverGeometriaEBuscar(primeira.place_id, texto);
        });
      },
    );
  }

  private resolverGeometriaEBuscar(placeId: string, textoOriginal: string): void {
    const placesService = new google.maps.places.PlacesService(document.createElement('div'));
    placesService.getDetails(
      { placeId, fields: ['geometry', 'formatted_address'] },
      (place: any, status: string) => {
        this.ngZone.run(() => {
          if (this.busca !== textoOriginal) return;

          if (status === 'OK' && place?.geometry?.location) {
            const lat = place.geometry.location.lat() as number;
            const lng = place.geometry.location.lng() as number;
            this.userLat = lat;
            this.userLng = lng;
            this.busca = '';
            this.buscaLocalizada = true;
            this.buscaLocalizadaNome = place.formatted_address ?? textoOriginal;
            this.localizacaoObtida.emit({ lat, lng });
          }

          this.executarBusca();
        });
      },
    );
  }

  private executarBusca(): void {
    this.listaSubscription?.unsubscribe();
    this.carregandoLista = true;

    const filtro: FiltroEstabelecimentoRequest = {};
    if (this.busca) filtro.nome = this.busca;
    if (this.categoriaSelecionada !== 'todos') {
      filtro.tipo = TIPO_MAP[this.categoriaSelecionada];
    }
    if (this.userLat != null && this.userLng != null) {
      filtro.latitude = this.userLat;
      filtro.longitude = this.userLng;
      filtro.distanciaMaxima = this.buscaLocalizada ? 0.5 : this.distanciaMaxima;
    }

    console.log('Executando busca com filtro:', filtro);

    const timeoutId = setTimeout(() => {
      console.warn('Timeout na busca de estabelecimentos');
      this.carregandoLista = false;
      this.lugaresFiltrados = [];
      this.lugaresFiltradosChange.emit([]);
    }, 10000);

    this.listaSubscription = this.estabelecimentoService.filtrar(filtro).subscribe({
      next: (dados) => {
        clearTimeout(timeoutId);
        console.log('Dados recebidos da API:', dados, 'Total:', Array.isArray(dados) ? dados.length : 0);
        const dadosArray = Array.isArray(dados) ? dados : (dados ? [dados] : []);
        this.processarResultados(dadosArray);
      },
      error: (erro) => {
        clearTimeout(timeoutId);
        console.error('Erro ao buscar estabelecimentos:', erro);
        this.carregandoLista = false;
        this.lugaresFiltrados = [];
        this.lugaresFiltradosChange.emit([]);
      },
      complete: () => {
        clearTimeout(timeoutId);
        console.log('Busca completada');
      },
    });
  }

  private processarResultados(dados: EstabelecimentoResponse[]): void {
    console.log('Processando resultados:', dados ? dados.length : 0, 'registros');

    if (!dados || !Array.isArray(dados)) {
      console.warn('Dados inválidos recebidos:', dados);
      this.carregandoLista = false;
      this.lugaresFiltrados = [];
      this.lugaresFiltradosChange.emit([]);
      return;
    }

    const lugares = dados.map((e) => this.mapearParaLugar(e));
    console.log('Lugares mapeados:', lugares);

    this.todosLugares = lugares;

    const resultado = lugares.filter((l) => {
      const matchCategoria =
        this.categoriaSelecionada === 'todos' || l.categoria === this.categoriaSelecionada;
      const matchAcessivel = !this.apenasAcessiveis || l.acessivel;
      const matches = matchCategoria && matchAcessivel;
      if (!matches) console.log('Filtrado:', l.nome, { matchCategoria, matchAcessivel, categoria: l.categoria, acessivel: l.acessivel });
      return matches;
    });

    console.log('Resultado após filtros:', resultado.length, 'registros');
    this.lugaresFiltrados = this.aplicarOrdenacao(resultado);
    this.carregandoLista = false;
    console.log('carregandoLista setado para false. Total de lugares a exibir:', this.lugaresFiltrados.length);
    this.lugaresFiltradosChange.emit(this.lugaresFiltrados);
  }

  private mapearParaLugar(e: EstabelecimentoResponse): Lugar {
    const end = e.endereco || {};
    const partes: string[] = [];
    const logNum = [end.logradouro, end.numero].filter(Boolean).join(', ');
    if (logNum) partes.push(logNum);
    if (end.bairro) partes.push(end.bairro);
    if (end.cidade) partes.push(`${end.cidade}${end.uf ? ' - ' + end.uf : ''}`);

    const distanciaKm = e.distanciaKm ?? 0;
    const distancia = e.distanciaKm == null ? ''
      : distanciaKm < 1
        ? `${Math.round(distanciaKm * 1000)} m`
        : `${distanciaKm.toFixed(1).replace('.', ',')} km`;

    const categoria = e.tipo != null
      ? TIPO_REVERSE_MAP[e.tipo] ?? 'todos'
      : 'todos';

    const avaliacoes = e.avaliacaoResponses ?? [];
    const totalAvaliacoes = e.totalAvaliacoes ?? avaliacoes.length;
    const avaliacao = e.mediaEstrelas != null
      ? Math.round(e.mediaEstrelas * 10) / 10
      : totalAvaliacoes > 0
        ? Math.round((avaliacoes.reduce((s, a) => s + a.estrelas, 0) / avaliacoes.length) * 10) / 10
        : 0;

    return {
      id: e.id,
      nome: e.nome || 'Estabelecimento sem nome',
      categoria,
      endereco: partes.length > 0 ? partes.join(' · ') : 'Endereço não informado',
      avaliacao,
      totalAvaliacoes,
      acessivel: e.recursosAcessibilidade && e.recursosAcessibilidade.length > 0,
      distancia,
      distanciaKm,
      recursos: (e.recursosAcessibilidade || []).map((r) => r.nome),
      recursosAcessibilidade: e.recursosAcessibilidade || [],
      lat: e.geocordenadas?.latitude ?? 0,
      lng: e.geocordenadas?.longitude ?? 0,
      fotos: e.fotos || [],
      horario: '',
    };
  }

  // ─── Filtros e ordenação ─────────────────────────────────────────────────

  filtrar(): void {
    this.executarBusca();
  }

  onBuscaInput(): void {
    this.buscaSubject$.next();
  }

  private aplicarOrdenacao(lista: Lugar[]): Lugar[] {
    return [...lista].sort((a, b) => {
      if (this.ordenacao === 'distancia') return a.distanciaKm - b.distanciaKm;
      if (this.ordenacao === 'avaliacao') return b.avaliacao - a.avaliacao;
      return a.nome.localeCompare(b.nome);
    });
  }

  ordenarPor(tipo: Ordenacao): void {
    this.ordenacao = tipo;
    this.lugaresFiltrados = this.aplicarOrdenacao(this.lugaresFiltrados);
    this.lugaresFiltradosChange.emit(this.lugaresFiltrados);
  }

  get categoriaSelecionadaIndex(): number {
    return this.categorias.findIndex((c) => c.id === this.categoriaSelecionada);
  }

  onCategoriaChange(index: number): void {
    this.selecionarCategoria(this.categorias[index].id);
  }

  selecionarCategoria(id: string): void {
    this.categoriaSelecionada = id;
    this.filtrar();
  }

  contarCategoria(id: string): number {
    if (id === 'todos') return this.todosLugares.length;
    return this.todosLugares.filter((l) => l.categoria === id).length;
  }

  toggleAcessiveis(): void {
    this.apenasAcessiveis = !this.apenasAcessiveis;
    this.filtrar();
  }

  toggleRecolher(): void {
    this.recolhido = !this.recolhido;
  }

  onAccordionToggle(open: boolean): void {
    this.filtrosExpandidos = open;
  }

  get filtrosAtivos(): number {
    let count = 0;
    if (this.apenasAcessiveis) count++;
    if (this.categoriaSelecionada !== 'todos') count++;
    if (this.busca) count++;
    if (this.buscaLocalizada) count++;
    if (this.distanciaMaxima < 50) count++;
    return count;
  }

  limparBusca(): void {
    this.busca = '';
    this.buscaLocalizada = false;
    this.buscaLocalizadaNome = '';
    this.filtrar();
  }

  limparFiltros(): void {
    this.busca = '';
    this.buscaLocalizada = false;
    this.buscaLocalizadaNome = '';
    this.apenasAcessiveis = false;
    this.categoriaSelecionada = 'todos';
    this.distanciaMaxima = 50;
    this.filtrar();
  }

  // ─── Utilitários de template ─────────────────────────────────────────────

  selecionarLugar(lugar: Lugar): void {
    this.lugarClick.emit(lugar);
  }

  // ─── Google Maps Places Autocomplete ────────────────────────────────────

  private tentarInicializarAutocomplete(tentativas = 0): void {
    if (typeof google === 'undefined' || !google.maps?.places) {
      if (tentativas < 30) {
        setTimeout(() => this.tentarInicializarAutocomplete(tentativas + 1), 100);
      }
      return;
    }
    if (!this.mapInputEl || this.autocomplete) return;

    this.autocomplete = new google.maps.places.Autocomplete(this.mapInputEl, {
      componentRestrictions: { country: 'BR' },
      fields: ['geometry', 'name'],
    });

    this.autocomplete.addListener('place_changed', () => {
      this.ngZone.run(() => {
        const place = this.autocomplete.getPlace();

        if (!place.geometry?.location) {
          // Enter pressionado sem selecionar sugestão — tenta geocodificar o texto digitado
          if (this.busca) this.processarBuscaPorTexto();
          return;
        }

        const lat = place.geometry.location.lat();
        const lng = place.geometry.location.lng();

        this.userLat = lat;
        this.userLng = lng;
        this.busca = '';
        this.buscaLocalizada = true;
        this.buscaLocalizadaNome = place.name ?? '';

        this.localizacaoObtida.emit({ lat, lng });
        this.executarBusca();
      });
    });
  }
}
