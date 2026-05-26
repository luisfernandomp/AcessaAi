import {
  AfterViewInit,
  Component,
  ElementRef,
  NgZone,
  OnDestroy,
  ViewChild,
  inject,
} from '@angular/core';
import { EstabelecimentoBottomSheetComponent } from './components/estabelecimento-bottom-sheet/estabelecimento-bottom-sheet.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { environment } from '../../../environments/environment';
import { Lugar, CATEGORIAS } from './mapa.models';
import { EstabelecimentoService } from '../../core/services/estabelecimento.service';

const MAP_CENTER = { lat: -23.5560, lng: -46.6620 };
const MAP_ZOOM_DEFAULT = 14;
const MAP_ZOOM_SELECIONADO = 16;

// eslint-disable-next-line @typescript-eslint/no-explicit-any
declare const google: any;

@Component({
  selector: 'app-mapa',
  standalone: true,
  imports: [EstabelecimentoBottomSheetComponent, SidebarComponent],
  templateUrl: './mapa.component.html',
  styleUrl: './mapa.component.css',
})
export class MapaComponent implements AfterViewInit, OnDestroy {
  @ViewChild('mapaContainer') mapaContainer!: ElementRef<HTMLDivElement>;
  @ViewChild(SidebarComponent) private sidebar?: SidebarComponent;

  lugarSelecionado: Lugar | null = null;
  mapCarregando = true;
  semApiKey = !environment.googleMapsApiKey;

  private userLat?: number;
  private userLng?: number;

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  private map: any = null;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  private markers = new Map<number, any>();

  private readonly estabelecimentoService = inject(EstabelecimentoService);

  constructor(private ngZone: NgZone) {}

  async ngAfterViewInit(): Promise<void> {
    if (this.semApiKey) {
      this.mapCarregando = false;
      return;
    }
    try {
      await this.carregarScriptMaps();
      this.inicializarMapa();
    } catch {
      this.mapCarregando = false;
    }
  }

  ngOnDestroy(): void {
    this.markers.forEach((m) => (m.map = null));
    this.markers.clear();
  }

  // ─── Eventos da sidebar ──────────────────────────────────────────────────

  onLocalizacaoObtida(coords: { lat: number; lng: number }): void {
    this.userLat = coords.lat;
    this.userLng = coords.lng;
    if (this.map) {
      this.map.panTo(coords);
      this.map.setZoom(MAP_ZOOM_SELECIONADO);
    }
  }

  onLugaresFiltrados(lugares: Lugar[]): void {
    this.renderizarMarcadores(lugares);
  }

  selecionarLugar(lugar: Lugar): void {
    const anterior = this.lugarSelecionado;
    this.lugarSelecionado = lugar;

    if (anterior) this.atualizarIconeMarker(anterior, false);
    this.atualizarIconeMarker(lugar, true);

    if (!this.map) return;

    const coordsValidas = lugar.lat !== 0 || lugar.lng !== 0;
    if (coordsValidas) {
      this.map.panTo({ lat: lugar.lat, lng: lugar.lng });
      this.map.setZoom(MAP_ZOOM_SELECIONADO);
    } else {
      this.estabelecimentoService.getById(lugar.id).subscribe({
        next: (est) => {
          if (est.geocordenadas?.latitude && est.geocordenadas?.longitude) {
            lugar.lat = est.geocordenadas.latitude;
            lugar.lng = est.geocordenadas.longitude;
            this.atualizarPosicaoMarker(lugar);
            this.map.panTo({ lat: lugar.lat, lng: lugar.lng });
            this.map.setZoom(MAP_ZOOM_SELECIONADO);
          }
        },
      });
    }
  }

  irParaMinhaLocalizacao(): void {
    if (!navigator.geolocation || !this.map) return;
    navigator.geolocation.getCurrentPosition(
      (pos) => {
        this.ngZone.run(() => {
          const coords = { lat: pos.coords.latitude, lng: pos.coords.longitude };
          this.userLat = coords.lat;
          this.userLng = coords.lng;
          this.map.panTo(coords);
          this.map.setZoom(MAP_ZOOM_SELECIONADO);
          this.sidebar?.setUserLocation(coords);
        });
      },
      () => {},
    );
  }

  fecharBottomSheet(): void {
    if (this.lugarSelecionado) {
      this.atualizarIconeMarker(this.lugarSelecionado, false);
    }
    this.lugarSelecionado = null;
    if (this.map) {
      this.map.panTo(MAP_CENTER);
      this.map.setZoom(MAP_ZOOM_DEFAULT);
    }
  }

  // ─── Google Maps ─────────────────────────────────────────────────────────

  private carregarScriptMaps(): Promise<void> {
    return new Promise((resolve, reject) => {
      if (typeof google !== 'undefined' && google.maps) {
        resolve();
        return;
      }
      const existente = document.getElementById('gmaps-script');
      if (existente) {
        existente.addEventListener('load', () => resolve());
        existente.addEventListener('error', () => reject());
        return;
      }
      const script = document.createElement('script');
      script.id = 'gmaps-script';
      script.src = `https://maps.googleapis.com/maps/api/js?key=${environment.googleMapsApiKey}&language=pt-BR&libraries=places,marker`;
      script.async = true;
      script.defer = true;
      script.onload = () => resolve();
      script.onerror = () => reject(new Error('Falha ao carregar Google Maps'));
      document.head.appendChild(script);
    });
  }

  private inicializarMapa(): void {
    const center = (this.userLat != null && this.userLng != null)
      ? { lat: this.userLat, lng: this.userLng }
      : MAP_CENTER;

    this.map = new google.maps.Map(this.mapaContainer.nativeElement, {
      center,
      zoom: MAP_ZOOM_DEFAULT,
      mapId: environment.googleMapsMapId,
      mapTypeControl: false,
      streetViewControl: false,
      fullscreenControl: false,
      zoomControl: true,
      zoomControlOptions: {
        position: google.maps.ControlPosition.RIGHT_CENTER,
      },
    });

    this.mapCarregando = false;
  }

  private renderizarMarcadores(lugares: Lugar[]): void {
    if (!this.map) return;

    const idsVisiveis = new Set(lugares.map((l) => l.id));

    this.markers.forEach((marker, id) => {
      if (!idsVisiveis.has(id)) {
        marker.map = null;
        this.markers.delete(id);
      }
    });

    lugares.forEach((lugar) => {
      const selecionado = this.lugarSelecionado?.id === lugar.id;
      if (this.markers.has(lugar.id)) {
        this.atualizarIconeMarker(lugar, selecionado);
      } else {
        this.criarMarker(lugar, selecionado);
      }
    });
  }

  private criarMarker(lugar: Lugar, selecionado: boolean): void {
    const marker = new google.maps.marker.AdvancedMarkerElement({
      position: { lat: lugar.lat, lng: lugar.lng },
      map: this.map,
      title: lugar.nome,
      content: this.buildContent(lugar, selecionado),
      zIndex: selecionado ? 100 : 1,
    });

    marker.addListener('click', () => {
      this.ngZone.run(() => this.selecionarLugar(lugar));
    });

    this.markers.set(lugar.id, marker);
  }

  private atualizarIconeMarker(lugar: Lugar, selecionado: boolean): void {
    const marker = this.markers.get(lugar.id);
    if (!marker) return;
    marker.content = this.buildContent(lugar, selecionado);
    marker.zIndex = selecionado ? 100 : 1;
  }

  private atualizarPosicaoMarker(lugar: Lugar): void {
    const marker = this.markers.get(lugar.id);
    if (!marker) return;
    marker.position = { lat: lugar.lat, lng: lugar.lng };
  }

  private buildContent(lugar: Lugar, selecionado: boolean): HTMLElement {
    const el = document.createElement('div');
    el.style.cursor = 'pointer';
    el.innerHTML = this.buildMarkerSvg(lugar, selecionado);
    return el;
  }

  private buildMarkerSvg(lugar: Lugar, selecionado: boolean): string {
    const cat = CATEGORIAS.find((c) => c.id === lugar.categoria);
    const cor = cat?.cor ?? '#6366f1';
    const emoji = cat?.emoji ?? '📍';

    if (selecionado) {
      return `<svg xmlns="http://www.w3.org/2000/svg" width="56" height="72" viewBox="0 0 56 72">
        <circle cx="28" cy="27" r="24" fill="${cor}" opacity="0.18"/>
        <ellipse cx="28" cy="70" rx="10" ry="4" fill="rgba(0,0,0,0.18)"/>
        <path d="M28 4C17 4 8 13 8 24c0 15 20 44 20 44s20-29 20-44C48 13 39 4 28 4z"
              fill="${cor}" stroke="white" stroke-width="2.5"/>
        <circle cx="28" cy="23" r="13" fill="white" opacity="0.96"/>
        <text x="28" y="29" text-anchor="middle" dominant-baseline="middle"
              font-size="17" font-family="Apple Color Emoji,Segoe UI Emoji,Noto Color Emoji,sans-serif">${emoji}</text>
        ${lugar.acessivel
          ? `<circle cx="47" cy="11" r="9" fill="#2563eb" stroke="white" stroke-width="2"/>
             <text x="47" y="15" text-anchor="middle" font-size="9" fill="white"
                   font-family="system-ui">♿</text>`
          : ''}
      </svg>`;
    }

    return `<svg xmlns="http://www.w3.org/2000/svg" width="42" height="54" viewBox="0 0 42 54">
      <ellipse cx="21" cy="52" rx="8" ry="3" fill="rgba(0,0,0,0.15)"/>
      <path d="M21 2C12 2 4 10 4 19c0 13 17 33 17 33s17-20 17-33C38 10 30 2 21 2z"
            fill="${cor}" stroke="white" stroke-width="2"/>
      <circle cx="21" cy="19" r="11" fill="white" opacity="0.96"/>
      <text x="21" y="24" text-anchor="middle" dominant-baseline="middle"
            font-size="13" font-family="Apple Color Emoji,Segoe UI Emoji,Noto Color Emoji,sans-serif">${emoji}</text>
      ${lugar.acessivel
        ? `<circle cx="36" cy="9" r="8" fill="#2563eb" stroke="white" stroke-width="1.5"/>
           <text x="36" y="13" text-anchor="middle" font-size="8" fill="white"
                 font-family="system-ui">♿</text>`
        : ''}
    </svg>`;
  }
}
