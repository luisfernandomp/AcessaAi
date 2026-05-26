import { RecursoAcessibilidade } from '../../core/models/recurso-acessibilidade.model';
import { TipoEstabelecimento } from '../../core/models/estabelecimento.model';

export const TIPO_MAP: Record<string, TipoEstabelecimento> = {
  restaurante: TipoEstabelecimento.Restaurante,
  farmacia: TipoEstabelecimento.Farmacia,
  saude: TipoEstabelecimento.Saude,
  banco: TipoEstabelecimento.Banco,
  shopping: TipoEstabelecimento.Shopping,
  transporte: TipoEstabelecimento.Transporte,
};

export const TIPO_REVERSE_MAP: Record<TipoEstabelecimento, string> = Object.fromEntries(
  Object.entries(TIPO_MAP).map(([k, v]) => [v, k]),
) as Record<TipoEstabelecimento, string>;

export interface Lugar {
  id: number;
  nome: string;
  categoria: string;
  endereco: string;
  avaliacao: number;
  totalAvaliacoes: number;
  acessivel: boolean;
  distancia: string;
  distanciaKm: number;
  recursos: string[];
  recursosAcessibilidade: RecursoAcessibilidade[];
  aberto?: boolean;
  horario: string;
  lat: number;
  lng: number;
  fotos?: { url: string; isCapa: boolean }[];
}

export type Ordenacao = 'distancia' | 'avaliacao' | 'nome';

export const CATEGORIAS = [
  { id: 'todos',       label: 'Todos',       icone: 'fa-solid fa-map',              emoji: '🗺️', cor: '#6366f1' },
  { id: 'restaurante', label: 'Restaurantes', icone: 'fa-solid fa-utensils',         emoji: '🍽️', cor: '#ef4444' },
  { id: 'farmacia',    label: 'Farmácias',    icone: 'fa-solid fa-pills',            emoji: '💊', cor: '#10b981' },
  { id: 'saude',       label: 'Saúde',        icone: 'fa-solid fa-hospital',         emoji: '🏥', cor: '#3b82f6' },
  { id: 'banco',       label: 'Bancos',       icone: 'fa-solid fa-building-columns', emoji: '🏦', cor: '#f59e0b' },
  { id: 'shopping',    label: 'Shopping',     icone: 'fa-solid fa-bag-shopping',     emoji: '🛍️', cor: '#8b5cf6' },
  { id: 'transporte',  label: 'Transporte',   icone: 'fa-solid fa-bus',              emoji: '🚌', cor: '#06b6d4' },
];
