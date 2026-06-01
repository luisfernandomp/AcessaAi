import { Endereco } from './endereco.model';

export interface CadastrarUsuarioRequest {
  nome: string;
  email: string;
  senha: string;
  dataNascimento: string;
  endereco: Endereco;
}

export interface UsuarioResponse {
  nome: string;
  dataNascimento: string;
  ativo: boolean;
  endereco: Endereco;
  urlFotoPerfil?: string | null;
}

export interface AtualizarUsuarioRequest {
  nome: string;
  dataNascimento: string;
  endereco: Endereco;
}
