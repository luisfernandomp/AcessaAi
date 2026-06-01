import { Endereco } from './endereco.model';
import { RecursoAcessibilidade } from './recurso-acessibilidade.model';

export enum TipoEstabelecimento {
  Restaurante = 1,
  Farmacia = 2,
  Saude = 3,
  Banco = 4,
  Shopping = 5,
  Transporte = 6,
  Educacao = 7,
  Outros = 8,
}

export interface Geocoordenadas {
  latitude: number;
  longitude: number;
}

export interface CriarEstabelecimentoRequest {
  nome: string;
  tipoEstabelecimento?: TipoEstabelecimento;
  geocordenadas: Geocoordenadas;
  endereco: Endereco;
  recursosAcessibilidadeIds?: number[];
}

export interface CriarEstabelecimentoJsonRequest {
  nome: string;
  tipo: TipoEstabelecimento;
  latitude: string;
  longitude: string;
  logradouro: string;
  uf: string;
  cidade: string;
  numero: string;
  cep: string;
  bairro: string;
  complemento?: string;
  recursosAcessibilidadesIds: number[];
  capaChave?: string;
  fotosChaves: string[];
}

export interface AtualizarEstabelecimentoRequest {
  nome: string;
  geocordenadas: Geocoordenadas;
}

export interface FotoEstabelecimento {
  url: string;
  isCapa: boolean;
}

export interface UsuarioResponse {
  nome: string;
  endereco: Endereco;
}

export interface AvaliacaoResponse {
  id: number;
  comentario: string;
  estrelas: number;
  usuarioId: number;
  usuarioResponse: UsuarioResponse;
}

export interface EstabelecimentoResponse {
  id: number;
  nome: string;
  tipo?: TipoEstabelecimento;
  geocordenadas: Geocoordenadas;
  fotos: FotoEstabelecimento[];
  recursosAcessibilidade: RecursoAcessibilidade[];
  endereco: Endereco;
  avaliacaoResponses: AvaliacaoResponse[];
  mediaEstrelas?: number;
  totalAvaliacoes?: number;
}

export interface FiltroEstabelecimentoRequest {
  nome?: string;
  tipo?: TipoEstabelecimento;
  distanciaMaxima?: number;
  'geocordenadasRequest.latitude'?: number;
  'geocordenadasRequest.longitude'?: number;
  'enderecoRequest.logradouro'?: string;
  'enderecoRequest.cidade'?: string;
  'enderecoRequest.uf'?: string;
  'enderecoRequest.cep'?: string;
  recursosAcessabilidadeIds?: number[];
}
