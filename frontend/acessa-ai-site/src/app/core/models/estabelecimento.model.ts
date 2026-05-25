import { Endereco } from './endereco.model';
import { RecursoAcessibilidade } from './recurso-acessibilidade.model';

export interface Geocoordenadas {
  latitude: number;
  longitude: number;
}

export interface CriarEstabelecimentoRequest {
  nome: string;
  tipoEstabelecimento?: string;
  geocordenadas: Geocoordenadas;
  endereco: Endereco;
  recursosAcessibilidadeIds?: number[];
}

export interface AtualizarEstabelecimentoRequest {
  nome: string;
  geocordenadas: Geocoordenadas;
}

export interface FotoEstabelecimento {
  url: string;
  isCapa: boolean;
}

export interface EstabelecimentoResponse {
  id: number;
  nome: string;
  geocordenadas: Geocoordenadas;
  fotos: FotoEstabelecimento[];
  recursosAcessibilidade: RecursoAcessibilidade[];
  endereco: Endereco;
}

export interface FiltroEstabelecimentoRequest {
  nome?: string;
  distanciaMaxima?: number;
  'geocordenadasRequest.latitude'?: number;
  'geocordenadasRequest.longitude'?: number;
  'enderecoRequest.logradouro'?: string;
  'enderecoRequest.cidade'?: string;
  'enderecoRequest.uf'?: string;
  'enderecoRequest.cep'?: string;
  recursosAcessabilidadeIds?: number[];
}
