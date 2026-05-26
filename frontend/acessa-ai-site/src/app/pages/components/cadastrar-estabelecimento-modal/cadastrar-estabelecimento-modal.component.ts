import { Component, ElementRef, EventEmitter, NgZone, OnInit, Output, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { EstabelecimentoService } from '../../../core/services/estabelecimento.service';
import { RecursoAcessibilidadeService } from '../../../core/services/recurso-acessibilidade.service';
import { RecursoAcessibilidade } from '../../../core/models/recurso-acessibilidade.model';
import { TipoEstabelecimento } from '../../../core/models/estabelecimento.model';
import { CATEGORIAS } from '../../mapa/mapa.models';
import { environment } from '../../../../environments/environment';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
declare const google: any;

const TIPO_ENUM: Record<string, TipoEstabelecimento> = {
  restaurante: TipoEstabelecimento.Restaurante,
  farmacia:    TipoEstabelecimento.Farmacia,
  saude:       TipoEstabelecimento.Saude,
  banco:       TipoEstabelecimento.Banco,
  shopping:    TipoEstabelecimento.Shopping,
  transporte:  TipoEstabelecimento.Transporte,
};

@Component({
  selector: 'app-cadastrar-estabelecimento-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './cadastrar-estabelecimento-modal.component.html',
  styleUrl: './cadastrar-estabelecimento-modal.component.css',
})
export class CadastrarEstabelecimentoModalComponent implements OnInit {
  @Output() fechar = new EventEmitter<void>();
  @Output() sucesso = new EventEmitter<void>();

  private readonly toastr = inject(ToastrService);
  private readonly recursoService = inject(RecursoAcessibilidadeService);

  etapa = 1;
  readonly totalEtapas = 4;
  carregando = false;
  mapCarregando = false;

  tiposEstabelecimento = CATEGORIAS.filter((c) => c.id !== 'todos');

  localizacaoSelecionada: { lat: number; lng: number } | null = null;

  recursosAcessibilidade: RecursoAcessibilidade[] = [];

  imagemCapa: File | null = null;
  imagemCapaPreview: string | null = null;

  imagensCarrossel: File[] = [];
  imagensCarrosselPreviews: string[] = [];
  imagemCapaAtiva: number | null = null;

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  private map: any = null;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  private marcadorSeletor: any = null;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  private autocomplete: any = null;
  private buscaInputEl: HTMLInputElement | null = null;

  @ViewChild('mapaContainer') set mapaContainerRef(el: ElementRef<HTMLDivElement> | undefined) {
    if (el) {
      this.map = null;
      this.marcadorSeletor = null;
      this.autocomplete = null;
      setTimeout(() => this.inicializarMapaSeletor(el.nativeElement), 50);
    } else {
      this.map = null;
      this.marcadorSeletor = null;
      this.autocomplete = null;
    }
  }

  @ViewChild('buscaInput') set buscaInputRef(el: ElementRef<HTMLInputElement> | undefined) {
    this.buscaInputEl = el?.nativeElement ?? null;
    this.autocomplete = null;
    if (this.buscaInputEl) this.tentarInicializarAutocomplete();
  }

  form = this.fb.group({
    nome: ['', Validators.required],
    tipoEstabelecimento: ['', Validators.required],
    recursosIds: this.fb.control<number[]>([]),
    endereco: this.fb.group({
      logradouro: ['', Validators.required],
      numero:     ['', Validators.required],
      complemento:[''],
      bairro:     ['', Validators.required],
      cidade:     ['', Validators.required],
      uf:         ['', [Validators.required, Validators.maxLength(2)]],
      cep:        ['', Validators.required],
    }),
  });

  constructor(
    private fb: FormBuilder,
    private ngZone: NgZone,
    private estabelecimentoService: EstabelecimentoService,
  ) {}

  ngOnInit(): void {
    this.recursoService.listarAtivas().subscribe({
      next: (r) => (this.recursosAcessibilidade = r),
      error: () => {},
    });
  }

  toggleRecurso(id: number): void {
    const ctrl = this.form.controls.recursosIds;
    const atual = ctrl.value ?? [];
    ctrl.setValue(atual.includes(id) ? atual.filter((v) => v !== id) : [...atual, id]);
  }

  isRecursoSelecionado(id: number): boolean {
    return (this.form.controls.recursosIds.value ?? []).includes(id);
  }

  selecionarTipo(id: string): void {
    this.form.controls.tipoEstabelecimento.setValue(id);
  }

  onImagemCapaChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.[0]) return;
    const file = input.files[0];
    this.imagemCapa = file;
    const reader = new FileReader();
    reader.onload = (e) => (this.imagemCapaPreview = e.target!.result as string);
    reader.readAsDataURL(file);
    input.value = '';
  }

  removerImagemCapa(): void {
    this.imagemCapa = null;
    this.imagemCapaPreview = null;
  }

  onImagensCarrosselChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files) return;
    for (const file of Array.from(input.files)) {
      if (this.imagensCarrossel.length >= 5) break;
      this.imagensCarrossel.push(file);
      const reader = new FileReader();
      reader.onload = (e) => this.imagensCarrosselPreviews.push(e.target!.result as string);
      reader.readAsDataURL(file);
    }
    input.value = '';
  }

  selecionarImagemComoCapaDoCarrossel(index: number): void {
    this.imagemCapaAtiva = this.imagemCapaAtiva === index ? null : index;
  }

  removerImagemCarrossel(index: number): void {
    this.imagensCarrossel.splice(index, 1);
    this.imagensCarrosselPreviews.splice(index, 1);
    if (this.imagemCapaAtiva === index) {
      this.imagemCapaAtiva = null;
    } else if (this.imagemCapaAtiva !== null && this.imagemCapaAtiva > index) {
      this.imagemCapaAtiva--;
    }
  }

  buscarLocalizacao(query: string): void {
    if (!query.trim() || !this.map) return;
    const geocoder = new google.maps.Geocoder();
    geocoder.geocode(
      { address: query, region: 'BR' },
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      (results: any[], status: string) => {
        this.ngZone.run(() => {
          if (status === 'OK' && results?.[0]) {
            const location = results[0].geometry.location;
            this.definirLocalizacao(location.lat(), location.lng(), location);
            this.reverseGeocode(location.lat(), location.lng());
          }
        });
      },
    );
  }

  proximaEtapa(): void {
    if (this.etapa === 1) {
      const { nome, tipoEstabelecimento } = this.form.controls;
      if (nome.invalid || tipoEstabelecimento.invalid) {
        this.toastr.warning('Preencha o nome e selecione o tipo de estabelecimento.');
        return;
      }
    }

    if (this.etapa === 2) {
      if (!this.localizacaoSelecionada) {
        this.toastr.warning('Selecione a localização do estabelecimento no mapa.');
        return;
      }
    }

    if (this.etapa === 3) {
      if (this.form.controls.endereco.invalid) {
        this.toastr.warning('Logradouro, Número, Bairro, Cidade, UF e CEP são obrigatórios.');
        return;
      }
      if (!(this.form.controls.recursosIds.value ?? []).length) {
        this.toastr.warning('Selecione ao menos um recurso de acessibilidade.');
        return;
      }
    }

    this.etapa++;
  }

  voltarEtapa(): void {
    this.etapa--;
  }

  submit(): void {
    if (this.form.controls.nome.invalid || this.form.controls.tipoEstabelecimento.invalid) {
      this.toastr.warning('Nome e tipo são obrigatórios.');
      return;
    }
    if (!this.localizacaoSelecionada) {
      this.toastr.warning('Selecione a localização no mapa.');
      return;
    }
    if (this.form.controls.endereco.invalid) {
      this.toastr.warning('Preencha todos os campos obrigatórios do endereço.');
      return;
    }

    this.carregando = true;
    const v = this.form.value;
    const end = v.endereco!;

    const formData = new FormData();
    formData.append('Nome',       v.nome!);
    formData.append('Tipo',       String(TIPO_ENUM[v.tipoEstabelecimento!] ?? 1));
    formData.append('Latitude',   this.localizacaoSelecionada.lat.toFixed(7));
    formData.append('Longitude',  this.localizacaoSelecionada.lng.toFixed(7));
    formData.append('Logradouro', end.logradouro ?? '');
    formData.append('UF',         end.uf ?? '');
    formData.append('Cidade',     end.cidade ?? '');
    formData.append('Numero',     end.numero ?? '');
    formData.append('CEP',        end.cep ?? '');
    formData.append('Bairro',     end.bairro ?? '');
    formData.append('Complemento',end.complemento ?? '');

    (v.recursosIds ?? [])
      .forEach((id) => formData.append('RecursosAcessibilidadesIds', String(id)));

    if (this.imagemCapa) {
      formData.append('Capa', this.imagemCapa, this.imagemCapa.name);
    }
    this.imagensCarrossel.forEach((f) => formData.append('Fotos', f, f.name));

    this.estabelecimentoService.criar(formData).subscribe({
      next: () => {
        this.carregando = false;
        this.toastr.success('Estabelecimento cadastrado com sucesso!');
        setTimeout(() => this.sucesso.emit(), 1500);
      },
      error: () => {
        this.carregando = false;
        this.toastr.error('Erro ao cadastrar. Verifique os dados e tente novamente.');
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

  private definirLocalizacao(lat: number, lng: number, position: unknown): void {
    this.localizacaoSelecionada = { lat, lng };
    this.map.panTo(position);
    this.map.setZoom(16);
    if (this.marcadorSeletor) {
      this.marcadorSeletor.position = position;
    } else {
      this.marcadorSeletor = new google.maps.marker.AdvancedMarkerElement({
        position,
        map: this.map,
        title: 'Localização selecionada',
      });
    }
  }

  private tentarInicializarAutocomplete(): void {
    if (!this.buscaInputEl || !this.map || this.autocomplete) return;
    if (typeof google === 'undefined' || !google.maps?.places) return;

    this.autocomplete = new google.maps.places.Autocomplete(this.buscaInputEl, {
      componentRestrictions: { country: 'BR' },
      fields: ['geometry', 'address_components'],
    });

    this.autocomplete.addListener('place_changed', () => {
      this.ngZone.run(() => {
        const place = this.autocomplete.getPlace();
        if (!place.geometry?.location) return;
        const lat = place.geometry.location.lat();
        const lng = place.geometry.location.lng();
        this.definirLocalizacao(lat, lng, place.geometry.location);
        if (place.address_components) {
          this.preencherEndereco(place.address_components);
        }
      });
    });
  }

  private async inicializarMapaSeletor(container: HTMLDivElement): Promise<void> {
    this.mapCarregando = true;
    try {
      await this.carregarScriptMaps();
      const center = this.localizacaoSelecionada
        ? { lat: this.localizacaoSelecionada.lat, lng: this.localizacaoSelecionada.lng }
        : { lat: -23.556, lng: -46.662 };

      this.map = new google.maps.Map(container, {
        center,
        zoom: 13,
        mapId: environment.googleMapsMapId,
        mapTypeControl: false,
        streetViewControl: false,
        fullscreenControl: false,
        zoomControl: true,
        zoomControlOptions: { position: google.maps.ControlPosition.RIGHT_CENTER },
      });

      if (this.localizacaoSelecionada) {
        this.marcadorSeletor = new google.maps.marker.AdvancedMarkerElement({
          position: center,
          map: this.map,
          title: 'Localização selecionada',
        });
      }

      this.map.addListener('click', (e: { latLng: { lat(): number; lng(): number } }) => {
        this.ngZone.run(() => {
          const lat = e.latLng.lat();
          const lng = e.latLng.lng();
          this.definirLocalizacao(lat, lng, e.latLng);
          this.reverseGeocode(lat, lng);
        });
      });

      this.tentarInicializarAutocomplete();
    } catch {
      // Google Maps não disponível
    } finally {
      this.ngZone.run(() => (this.mapCarregando = false));
    }
  }

  private reverseGeocode(lat: number, lng: number): void {
    const geocoder = new google.maps.Geocoder();
    geocoder.geocode(
      { location: { lat, lng } },
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      (results: any[], status: string) => {
        this.ngZone.run(() => {
          if (status === 'OK' && results?.[0]) {
            this.preencherEndereco(results[0].address_components);
          }
        });
      },
    );
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  private preencherEndereco(components: any[]): void {
    const get = (type: string, short = false): string => {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      const comp = components.find((c: any) => c.types.includes(type));
      return comp ? (short ? comp.short_name : comp.long_name) : '';
    };

    const rawCep = get('postal_code');
    const cep = rawCep.replace(/\D/g, '').replace(/^(\d{5})(\d{3})$/, '$1-$2');

    this.form.controls.endereco.patchValue({
      logradouro:  get('route'),
      numero:      get('street_number'),
      bairro:      get('sublocality_level_1') || get('sublocality') || get('neighborhood'),
      cidade:      get('administrative_area_level_2'),
      uf:          get('administrative_area_level_1', true),
      cep,
      complemento: '',
    });
  }

  private carregarScriptMaps(): Promise<void> {
    return new Promise((resolve, reject) => {
      if (typeof google !== 'undefined' && google.maps && google.maps.places) {
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
      script.src = `https://maps.googleapis.com/maps/api/js?key=${environment.googleMapsApiKey}&language=pt-BR&libraries=places,marker&loading=async`;
      script.async = true;
      script.defer = true;
      script.onload = () => resolve();
      script.onerror = () => reject(new Error('Falha ao carregar Google Maps'));
      document.head.appendChild(script);
    });
  }
}
