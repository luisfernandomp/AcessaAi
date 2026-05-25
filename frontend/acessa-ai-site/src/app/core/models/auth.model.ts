export interface LoginRequest {
  email: string;
  senha: string;
}

export interface LoginResponse {
  idUsuario: number;
  nomeUsuario: string;
  token: string;
  expiraEm: number;
}
